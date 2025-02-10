namespace ModsenTestEvent.Infrastructure.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Participant> Participants { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ParticipantConfiguration());
        modelBuilder.ApplyConfiguration(new EventConfiguration());
        modelBuilder.ApplyConfiguration(new ImageConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
    }
}

public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        builder.HasKey(x => x.Id);
    }
}

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasKey(x => x.Id);
    }
}

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.Participants)
            .WithOne(x => x.Event)
            .HasForeignKey(x => x.EventId);
        builder.HasMany(x => x.Images)
            .WithOne(x => x.Event)
            .HasForeignKey(x => x.EventId);
    }
}