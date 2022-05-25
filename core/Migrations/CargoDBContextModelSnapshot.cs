﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using core.Context;

#nullable disable

namespace core.Migrations
{
    [DbContext(typeof(CargoDBContext))]
    partial class CargoDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("domain.Models.CargoType", b =>
                {
                    b.Property<int>("CargoTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CargoTypeId"), 1L, 1);

                    b.Property<string>("CargoTypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CargoTypeId");

                    b.ToTable("CargoTypes");

                    b.HasData(
                        new
                        {
                            CargoTypeId = 1,
                            CargoTypeName = "Refrigerators"
                        },
                        new
                        {
                            CargoTypeId = 2,
                            CargoTypeName = "Building Materials"
                        },
                        new
                        {
                            CargoTypeId = 3,
                            CargoTypeName = "Clothes"
                        });
                });

            modelBuilder.Entity("domain.Models.Company", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CompanyId"), 1L, 1);

                    b.Property<string>("CompanyDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CompanyId");

                    b.ToTable("Companies");

                    b.HasData(
                        new
                        {
                            CompanyId = 1,
                            CompanyDescription = "Doing intersting stuff",
                            CompanyName = "GeoTrans"
                        },
                        new
                        {
                            CompanyId = 2,
                            CompanyDescription = "Transporting company",
                            CompanyName = "NeoTrans"
                        });
                });

            modelBuilder.Entity("domain.Models.Country", b =>
                {
                    b.Property<int>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CountryId"), 1L, 1);

                    b.Property<string>("CountryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CountryId");

                    b.ToTable("Countries");

                    b.HasData(
                        new
                        {
                            CountryId = 1,
                            CountryName = "Moldova"
                        },
                        new
                        {
                            CountryId = 2,
                            CountryName = "Romania"
                        },
                        new
                        {
                            CountryId = 3,
                            CountryName = "Bulgaria"
                        },
                        new
                        {
                            CountryId = 4,
                            CountryName = "Turkey"
                        });
                });

            modelBuilder.Entity("domain.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"), 1L, 1);

                    b.Property<DateTime>("ArrivalDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CargoTypeId")
                        .HasColumnType("int");

                    b.Property<int>("DestinationCountryId")
                        .HasColumnType("int");

                    b.Property<int>("Payment")
                        .HasColumnType("int");

                    b.Property<int>("PaymentMethodId")
                        .HasColumnType("int");

                    b.Property<int>("SendingCountryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ShipmentDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("OrderId");

                    b.HasIndex("CargoTypeId");

                    b.HasIndex("DestinationCountryId");

                    b.HasIndex("PaymentMethodId");

                    b.HasIndex("SendingCountryId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("domain.Models.PaymentMethod", b =>
                {
                    b.Property<int>("PaymentMethodId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentMethodId"), 1L, 1);

                    b.Property<string>("PaymentMethodName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PaymentMethodId");

                    b.ToTable("PaymentMethods");

                    b.HasData(
                        new
                        {
                            PaymentMethodId = 1,
                            PaymentMethodName = "Transfer"
                        },
                        new
                        {
                            PaymentMethodId = 2,
                            PaymentMethodName = "Cash"
                        });
                });

            modelBuilder.Entity("domain.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"), 1L, 1);

                    b.Property<string>("RoleType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            RoleId = 1,
                            RoleType = "User"
                        },
                        new
                        {
                            RoleId = 2,
                            RoleType = "Editor"
                        },
                        new
                        {
                            RoleId = 3,
                            RoleType = "Admin"
                        });
                });

            modelBuilder.Entity("domain.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"), 1L, 1);

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CountryId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            CompanyId = 2,
                            CountryId = 1,
                            Email = "john.doe@gmail.com",
                            FirstName = "John",
                            LastName = "Doe",
                            Password = "$2a$11$QbnCIuSAaFeApJ00mzDG/e5R6bhNkUsvo1qZX6mppDxeQnReyowR.",
                            PhoneNumber = "+37368742312",
                            RoleId = 1
                        },
                        new
                        {
                            UserId = 2,
                            CompanyId = 1,
                            CountryId = 2,
                            Email = "vasea.buzu@gmail.com",
                            FirstName = "Vasea",
                            LastName = "Buzu",
                            Password = "$2a$11$jtE2jLiX/BXcCEOAZcdA1Ocq/Ps4e.CRpDDujUpJQ/wxIkvRhmog2",
                            PhoneNumber = "+37369252473",
                            RoleId = 2
                        },
                        new
                        {
                            UserId = 3,
                            CompanyId = 1,
                            CountryId = 3,
                            Email = "ivan.doe@gmail.com",
                            FirstName = "Ivan",
                            LastName = "Doe",
                            Password = "$2a$11$Svm9pfPOUTBv5klCd7sgeOS2t05ku5bzsvrc4HMp10HgYcc9w53gS",
                            PhoneNumber = "+37364535279",
                            RoleId = 3
                        });
                });

            modelBuilder.Entity("domain.Models.Order", b =>
                {
                    b.HasOne("domain.Models.CargoType", "CargoType")
                        .WithMany()
                        .HasForeignKey("CargoTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("domain.Models.Country", "DestinationCountry")
                        .WithMany()
                        .HasForeignKey("DestinationCountryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("domain.Models.PaymentMethod", "PaymentMethod")
                        .WithMany()
                        .HasForeignKey("PaymentMethodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("domain.Models.Country", "SendingCountry")
                        .WithMany()
                        .HasForeignKey("SendingCountryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("domain.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CargoType");

                    b.Navigation("DestinationCountry");

                    b.Navigation("PaymentMethod");

                    b.Navigation("SendingCountry");

                    b.Navigation("User");
                });

            modelBuilder.Entity("domain.Models.User", b =>
                {
                    b.HasOne("domain.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("domain.Models.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("domain.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("Country");

                    b.Navigation("Role");
                });
#pragma warning restore 612, 618
        }
    }
}