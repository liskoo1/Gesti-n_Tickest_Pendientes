using Gestión_Tickest_Pendientes.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Gestión_Tickest_Pendientes.Seeds
{
    // Clase que implementa la interfaz IEntityTypeConfiguration para configurar la entidad Empresa
    public class SeedEmpresa : IEntityTypeConfiguration<Empresa>
    {
        // Método que configura la entidad Empresa
        public void Configure(EntityTypeBuilder<Empresa> builder)
        {
            // Se establecen los datos iniciales para la entidad Empresa
            builder.HasData(
                new Empresa()
                {
                    Id_Empresa = 1, // ID de la empresa
                    Nombre_Empresa = "Nombre de la empresa", // Nombre de la empresa
                    CIF_Empresa = "CIF de la empresa", // CIF de la empresa
                    Telefono_Empresa = "Teléfono de la empresa", // Teléfono de la empresa
                    Direccion_Empresa = "Dirección de la empresa", // Dirección de la empresa
                    Email_Empresa = "Email de la empresa" // Email de la empresa
                });
        }
    }

    // Clase que implementa la interfaz IEntityTypeConfiguration para configurar la entidad Usuario
    public class SeedUsuario : IEntityTypeConfiguration<Usuario>
    {
        // Método que configura la entidad Usuario
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            // Se establecen los datos iniciales para la entidad Usuario
            builder.HasData(new Usuario()
            {
                Id_Usuario = 1, // ID del usuario
                Name = "Admin", // Nombre del usuario
                Password = $"{Services.Tools.CreateHash("Admin")}", // Contraseña del usuario
                Id_Empresa = 1 // ID de la empresa asociada al usuario
            },
            new Usuario()
            {
                Id_Usuario = 2, // ID del usuario
                Name = "Admin", // Nombre del usuario
                Password = $"{Services.Tools.CreateHash("Admin")}", // Contraseña del usuario
                Id_Empresa = 1 // ID de la empresa asociada al usuario
            });
        }
    }
}

