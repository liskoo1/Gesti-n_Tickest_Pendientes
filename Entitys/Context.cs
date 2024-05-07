using Gestión_Tickest_Pendientes.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Gestión_Tickest_Pendientes.Entitys
{
    public class Context : DbContext, IDisposable
    {
        
        
        public DbSet<Empresa> empresas {  get; set; }
        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<Cliente> clientes { get; set; }
        public DbSet<Ticket> tickets { get; set; }

        // Sobrescribe el método OnConfiguring para configurar las opciones del contexto de la base de datos
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Verifica si las opciones ya están configuradas
            if (!optionsBuilder.IsConfigured)
            {
                // Crea un objeto de configuración a partir del archivo appsettings.json
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory()) // Establece la ruta base como el directorio actual de ejecución
                    .AddJsonFile("appsettings.json") // Agrega el archivo appsettings.json como fuente de configuración
                    .Build(); // Construye la configuración

                // Obtiene la cadena de conexión llamada "DefaultConnection" del archivo de configuración
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                // Configura las opciones del constructor para utilizar SQL Server con la cadena de conexión obtenida
                optionsBuilder.UseSqlServer(connectionString);

                // Habilita la carga diferida para entidades relacionadas utilizando proxies de carga diferida
                optionsBuilder.UseLazyLoadingProxies();
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
              La función OnModelCreating es un método que se sobrescribe en una clase derivada de DbContext en Entity Framework Core. 
              Este método se utiliza para configurar el modelo de datos de la base de datos, específicamente para definir cómo se mapean las clases de entidad a 
              tablas de base de datos y cómo se configuran las relaciones entre ellas.
             */
            //Añadimos nuestras seeds
            modelBuilder.ApplyConfiguration(new SeedEmpresa());
            modelBuilder.ApplyConfiguration(new SeedUsuario());
                
        }
        public void dispose()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }

    }
}
