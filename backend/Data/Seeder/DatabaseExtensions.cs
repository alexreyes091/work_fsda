using Microsoft.EntityFrameworkCore;

namespace app.webapi.backoffice_viajes_altairis.Data.Seeder
{
    public static class DatabaseExtensions
    {
        public static async Task ApplyMigrationsAndSeedAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<AltarisDbContext>();
                await context.Database.MigrateAsync();
                await DbSeeder.SeedAsync(context);

                Console.WriteLine("Se agrego el seeder correctamente.");
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<AltarisDbContext>>();
                logger.LogError(ex, "Ocurrio un error durante la migracion del seeder.");
            }
        }
    }
}
