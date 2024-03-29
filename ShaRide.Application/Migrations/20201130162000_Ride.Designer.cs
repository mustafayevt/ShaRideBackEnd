﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ShaRide.Application.Contexts;

namespace ShaRide.Application.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20201130162000_Ride")]
    partial class Ride
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("ShaRide.Domain.Entities.BanType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AssetPath")
                        .HasColumnType("text");

                    b.Property<bool>("IsRowActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("BanTypes");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("CarBrandId")
                        .HasColumnType("integer");

                    b.Property<int?>("CarModelId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsRowActive")
                        .HasColumnType("boolean");

                    b.Property<string>("RegisterNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CarBrandId");

                    b.HasIndex("CarModelId");

                    b.ToTable("Car");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.CarBrand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("IsRowActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CarBrands");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.CarImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("CarId")
                        .HasColumnType("integer");

                    b.Property<string>("ImagePath")
                        .HasColumnType("text");

                    b.Property<bool>("IsRowActive")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.ToTable("CarImages");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.CarModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("BanTypeId")
                        .HasColumnType("integer");

                    b.Property<int>("CarBrandId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsRowActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("BanTypeId");

                    b.HasIndex("CarBrandId");

                    b.ToTable("CarModels");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.CarSeatComposition", b =>
                {
                    b.Property<int>("CarId")
                        .HasColumnType("integer");

                    b.Property<int>("SeatId")
                        .HasColumnType("integer");

                    b.Property<int>("SeatRotate")
                        .HasColumnType("integer");

                    b.HasKey("CarId", "SeatId");

                    b.HasIndex("SeatId");

                    b.ToTable("CarSeatComposition","Ride");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.Driver", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<short>("Rating")
                        .HasColumnType("smallint");

                    b.HasKey("UserId");

                    b.ToTable("Drivers","Ride");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("IsRowActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.LocationPoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("IsRowActive")
                        .HasColumnType("boolean");

                    b.Property<int>("LocationId")
                        .HasColumnType("integer");

                    b.Property<int>("LocationPointType")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("LocationPoints");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.Restriction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AssetPath")
                        .HasColumnType("text");

                    b.Property<bool>("IsRowActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Title");

                    b.ToTable("Restrictions");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.RestrictionRideComposition", b =>
                {
                    b.Property<int>("RestrictionId")
                        .HasColumnType("integer");

                    b.Property<int>("RideId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsPossible")
                        .HasColumnType("boolean");

                    b.HasKey("RestrictionId", "RideId");

                    b.HasIndex("RideId");

                    b.ToTable("RestrictionRideComposition");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.Ride", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("DriverId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsRowActive")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("DriverId");

                    b.ToTable("Rides","Ride");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.RideCarComposition", b =>
                {
                    b.Property<int>("CarId")
                        .HasColumnType("integer");

                    b.Property<int>("RideId")
                        .HasColumnType("integer");

                    b.HasKey("CarId", "RideId");

                    b.HasIndex("RideId");

                    b.ToTable("RideCarComposition","Ride");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.RideCarSeatComposition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("CarSeatCompositionCarId")
                        .HasColumnType("integer");

                    b.Property<int?>("CarSeatCompositionSeatId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsRowActive")
                        .HasColumnType("boolean");

                    b.Property<int?>("RideCarCompositionCarId")
                        .HasColumnType("integer");

                    b.Property<int?>("RideCarCompositionRideId")
                        .HasColumnType("integer");

                    b.Property<int>("SeatStatus")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CarSeatCompositionCarId", "CarSeatCompositionSeatId");

                    b.HasIndex("RideCarCompositionCarId", "RideCarCompositionRideId");

                    b.ToTable("RideCarSeatComposition","Ride");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("IsRowActive")
                        .HasColumnType("boolean");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.Seat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("IsRowActive")
                        .HasColumnType("boolean");

                    b.Property<short>("xCordinant")
                        .HasColumnType("smallint");

                    b.Property<short>("yCordinant")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.ToTable("Seats");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<byte[]>("Img")
                        .HasColumnType("bytea");

                    b.Property<string>("ImgExtension")
                        .HasColumnType("text");

                    b.Property<bool>("IsRowActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.UserPhone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("IsConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsMain")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsRowActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserPhones");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.UserRoleComposition", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoleComposition");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.Car", b =>
                {
                    b.HasOne("ShaRide.Domain.Entities.CarBrand", "CarBrand")
                        .WithMany()
                        .HasForeignKey("CarBrandId");

                    b.HasOne("ShaRide.Domain.Entities.CarModel", "CarModel")
                        .WithMany()
                        .HasForeignKey("CarModelId");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.CarImage", b =>
                {
                    b.HasOne("ShaRide.Domain.Entities.Car", "Car")
                        .WithMany("ImagePaths")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.CarModel", b =>
                {
                    b.HasOne("ShaRide.Domain.Entities.BanType", "BanType")
                        .WithMany()
                        .HasForeignKey("BanTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShaRide.Domain.Entities.CarBrand", "CarBrand")
                        .WithMany()
                        .HasForeignKey("CarBrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.CarSeatComposition", b =>
                {
                    b.HasOne("ShaRide.Domain.Entities.Car", "Car")
                        .WithMany()
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShaRide.Domain.Entities.Seat", "Seat")
                        .WithMany()
                        .HasForeignKey("SeatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.Driver", b =>
                {
                    b.HasOne("ShaRide.Domain.Entities.User", "User")
                        .WithMany("Drivers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.LocationPoint", b =>
                {
                    b.HasOne("ShaRide.Domain.Entities.Location", "Location")
                        .WithMany("LocationPoints")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.RestrictionRideComposition", b =>
                {
                    b.HasOne("ShaRide.Domain.Entities.Restriction", "Restriction")
                        .WithMany()
                        .HasForeignKey("RestrictionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShaRide.Domain.Entities.Ride", "Ride")
                        .WithMany()
                        .HasForeignKey("RideId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.Ride", b =>
                {
                    b.HasOne("ShaRide.Domain.Entities.Driver", "Driver")
                        .WithMany()
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.RideCarComposition", b =>
                {
                    b.HasOne("ShaRide.Domain.Entities.Car", "Car")
                        .WithMany()
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShaRide.Domain.Entities.Ride", "Ride")
                        .WithMany("RideCarSeatComposition")
                        .HasForeignKey("RideId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.RideCarSeatComposition", b =>
                {
                    b.HasOne("ShaRide.Domain.Entities.CarSeatComposition", "CarSeatComposition")
                        .WithMany()
                        .HasForeignKey("CarSeatCompositionCarId", "CarSeatCompositionSeatId");

                    b.HasOne("ShaRide.Domain.Entities.RideCarComposition", null)
                        .WithMany("RideCarSeatComposition")
                        .HasForeignKey("RideCarCompositionCarId", "RideCarCompositionRideId");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.UserPhone", b =>
                {
                    b.HasOne("ShaRide.Domain.Entities.User", "User")
                        .WithMany("Phones")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.UserRoleComposition", b =>
                {
                    b.HasOne("ShaRide.Domain.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShaRide.Domain.Entities.User", "User")
                        .WithMany("UserRoleComposition")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
