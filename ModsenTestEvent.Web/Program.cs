var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(EventProfile), typeof(ParticipantProfile), typeof(UserProfile));
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IParticipantService, ParticipantService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddValidatorsFromAssemblyContaining<FileValidator>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"], 
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("UserPolicy", policy =>
        policy.RequireRole("User", "Admin"));
    
    options.AddPolicy("ParticipantPolicy", policy =>
        policy.RequireRole("Participant", "Admin"));
});

builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ModsenTest API", Version = "v1" });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Введите токен, начиная с 'Bearer '",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads"
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

    var retries = 10;
    while (retries > 0)
    {
        try
        {
            await dbContext.Database.OpenConnectionAsync();
            await dbContext.Database.CloseConnectionAsync();
            break;
        }
        catch (Exception ex)
        {
            retries--;
            if (retries == 0)
            {
                throw new InvalidOperationException("Unable to connect to the database", ex);
            }
            await Task.Delay(1000);
        }
        await dbContext.Database.MigrateAsync();
    }
}

app.UseExceptionHandler("/error"); 
app.UseMiddleware<ExceptionHandlingMiddleware>(); 

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();