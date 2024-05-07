﻿// <auto-generated />
using System;
using Gestión_Tickest_Pendientes.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Gestión_Tickest_Pendientes.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Gestión_Tickest_Pendientes.Entitys.Cliente", b =>
                {
                    b.Property<string>("CIF_Cliente")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Codigo_Postal")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Direccion_Cliente")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email_Cliente")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Id_Empresa")
                        .HasColumnType("int");

                    b.Property<string>("Nombre_Cliente")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefono_Cliente")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CIF_Cliente");

                    b.HasIndex("Id_Empresa");

                    b.ToTable("clientes");
                });

            modelBuilder.Entity("Gestión_Tickest_Pendientes.Entitys.Empresa", b =>
                {
                    b.Property<int>("Id_Empresa")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_Empresa"));

                    b.Property<string>("CIF_Empresa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Direccion_Empresa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email_Empresa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre_Empresa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefono_Empresa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_Empresa");

                    b.ToTable("empresas");

                    b.HasData(
                        new
                        {
                            Id_Empresa = 1,
                            CIF_Empresa = "A04052775",
                            Direccion_Empresa = "Vnta del Pobre - Níjar - Almería",
                            Email_Empresa = "ventadelpobre@gmail.com",
                            Nombre_Empresa = "Venta del Pobre Gastro Bar",
                            Telefono_Empresa = "950385544"
                        });
                });

            modelBuilder.Entity("Gestión_Tickest_Pendientes.Entitys.Ticket", b =>
                {
                    b.Property<string>("Id_Albaran")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CIF_Cliente")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Firma_Cliente")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Mesa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sala")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Total")
                        .HasColumnType("float");

                    b.HasKey("Id_Albaran");

                    b.HasIndex("CIF_Cliente");

                    b.ToTable("tickets");
                });

            modelBuilder.Entity("Gestión_Tickest_Pendientes.Entitys.Usuario", b =>
                {
                    b.Property<int>("Id_Usuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_Usuario"));

                    b.Property<int>("Id_Empresa")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_Usuario");

                    b.HasIndex("Id_Empresa");

                    b.ToTable("usuarios");

                    b.HasData(
                        new
                        {
                            Id_Usuario = 1,
                            Id_Empresa = 1,
                            Name = "Admin",
                            Password = "e9cf3f0ecd469045446f50e517eb125a733482231ab0a93ee8c3492cde823116"
                        },
                        new
                        {
                            Id_Usuario = 2,
                            Id_Empresa = 1,
                            Name = "Restaurante",
                            Password = "9faf3c3c97bd95f2b15b1a3904e9cbbbe730c1ecf60818a476e83e4aa7a3b595"
                        });
                });

            modelBuilder.Entity("Gestión_Tickest_Pendientes.Entitys.Cliente", b =>
                {
                    b.HasOne("Gestión_Tickest_Pendientes.Entitys.Empresa", "Empresa")
                        .WithMany()
                        .HasForeignKey("Id_Empresa")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Empresa");
                });

            modelBuilder.Entity("Gestión_Tickest_Pendientes.Entitys.Ticket", b =>
                {
                    b.HasOne("Gestión_Tickest_Pendientes.Entitys.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("CIF_Cliente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("Gestión_Tickest_Pendientes.Entitys.Usuario", b =>
                {
                    b.HasOne("Gestión_Tickest_Pendientes.Entitys.Empresa", "Empresa")
                        .WithMany()
                        .HasForeignKey("Id_Empresa")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Empresa");
                });
#pragma warning restore 612, 618
        }
    }
}
