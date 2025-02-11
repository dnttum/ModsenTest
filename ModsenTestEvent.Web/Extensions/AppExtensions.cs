namespace ModsenTestEvent.Web.Extensions;

public static class AppExtensions
{
    public static async Task AddMigrations(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dbContext =  scope.ServiceProvider.GetRequiredService<DataContext>();

        await dbContext.Database.MigrateAsync();   
    }
}