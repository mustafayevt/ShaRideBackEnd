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
    [Migration("20210324144410_LatitudeLongitudeUniqueIndexAdded")]
    partial class LatitudeLongitudeUniqueIndexAdded
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

                    b.Property<int>("CarModelId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsRowActive")
                        .HasColumnType("boolean");

                    b.Property<string>("RegisterNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CarModelId");

                    b.ToTable("Cars");
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

                    b.Property<string>("Extension")
                        .HasColumnType("text");

                    b.Property<byte[]>("Image")
                        .HasColumnType("bytea");

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
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("CarId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsRowActive")
                        .HasColumnType("boolean");

                    b.Property<int>("SeatId")
                        .HasColumnType("integer");

                    b.Property<int>("SeatRotate")
                        .HasColumnType("integer");

                    b.Property<int>("SeatType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.HasIndex("SeatId", "CarId")
                        .IsUnique();

                    b.ToTable("CarSeatComposition","Ride");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.Counter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsRowActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Counters");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.Feedback", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<int>("CreatedByUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedTimestamp")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsRowActive")
                        .HasColumnType("boolean");

                    b.Property<int?>("LastModifiedByUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("LastModifiedTimestamp")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("LastModifiedByUserId");

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,6)");

                    b.Property<string>("InvoiceNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("InvoiceRegisterDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsRowActive")
                        .HasColumnType("boolean");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Invoices");
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

                    b.Property<double>("Latitude")
                        .HasColumnType("double precision");

                    b.Property<int>("LocationId")
                        .HasColumnType("integer");

                    b.Property<double>("Longitude")
                        .HasColumnType("double precision");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.HasIndex("Latitude", "Longitude")
                        .IsUnique();

                    b.ToTable("LocationPoints");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.PassengerToRideRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsRowActive")
                        .HasColumnType("boolean");

                    b.Property<int>("RequestStatus")
                        .HasColumnType("integer");

                    b.Property<int>("RideCarSeatCompositionId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RideCarSeatCompositionId");

                    b.HasIndex("UserId");

                    b.ToTable("PassengerToRideRequests");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.PotentialClientNumber", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("IsRowActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Phone");

                    b.ToTable("PotentialClientNumbers");
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

                    b.ToTable("RestrictionRideComposition","Ride");
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

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.Property<decimal>("PricePerSeat")
                        .HasColumnType("decimal(18,6)");

                    b.Property<int>("RideState")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("DriverId");

                    b.ToTable("Rides","Ride");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.RideCarSeatComposition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("CarSeatCompositionId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsRowActive")
                        .HasColumnType("boolean");

                    b.Property<int?>("PassengerId")
                        .HasColumnType("integer");

                    b.Property<int>("RideId")
                        .HasColumnType("integer");

                    b.Property<int>("SeatStatus")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CarSeatCompositionId");

                    b.HasIndex("PassengerId");

                    b.HasIndex("RideId");

                    b.ToTable("RideCarSeatComposition","Ride");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.RideLocationPointComposition", b =>
                {
                    b.Property<int>("RideId")
                        .HasColumnType("integer");

                    b.Property<int>("LocationPointId")
                        .HasColumnType("integer");

                    b.Property<int>("LocationPointType")
                        .HasColumnType("integer");

                    b.HasKey("RideId", "LocationPointId");

                    b.HasIndex("LocationPointId");

                    b.ToTable("RideLocationPointComposition","Ride");
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

            modelBuilder.Entity("ShaRide.Domain.Entities.SiteVisitor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Ip")
                        .HasColumnType("text");

                    b.Property<bool>("IsRowActive")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("SiteVisitors");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,6)");

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

                    b.Property<string>("UserUniqueKey")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.UserFcmToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("IsRowActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserFcmTokens");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.UserImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Extension")
                        .HasColumnType("text");

                    b.Property<byte[]>("Image")
                        .HasColumnType("bytea");

                    b.Property<bool>("IsRowActive")
                        .HasColumnType("boolean");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserImage");
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

                    b.HasIndex("Number")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("UserPhones");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.UserRating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("DestinationUserId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsRowActive")
                        .HasColumnType("boolean");

                    b.Property<int>("RideId")
                        .HasColumnType("integer");

                    b.Property<int>("SourceUserId")
                        .HasColumnType("integer");

                    b.Property<short>("Value")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("DestinationUserId");

                    b.HasIndex("RideId");

                    b.HasIndex("SourceUserId");

                    b.ToTable("UserRatings");
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
                    b.HasOne("ShaRide.Domain.Entities.CarModel", "CarModel")
                        .WithMany()
                        .HasForeignKey("CarModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.CarImage", b =>
                {
                    b.HasOne("ShaRide.Domain.Entities.Car", "Car")
                        .WithMany("CarImages")
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
                        .WithMany("CarSeatComposition")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShaRide.Domain.Entities.Seat", "Seat")
                        .WithMany()
                        .HasForeignKey("SeatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.Feedback", b =>
                {
                    b.HasOne("ShaRide.Domain.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShaRide.Domain.Entities.User", "LastModifiedByUser")
                        .WithMany()
                        .HasForeignKey("LastModifiedByUserId");
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.Invoice", b =>
                {
                    b.HasOne("ShaRide.Domain.Entities.User", "User")
                        .WithMany()
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

            modelBuilder.Entity("ShaRide.Domain.Entities.PassengerToRideRequest", b =>
                {
                    b.HasOne("ShaRide.Domain.Entities.RideCarSeatComposition", "RideCarSeatComposition")
                        .WithMany()
                        .HasForeignKey("RideCarSeatCompositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShaRide.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
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
                        .WithMany("RestrictionRideComposition")
                        .HasForeignKey("RideId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.Ride", b =>
                {
                    b.HasOne("ShaRide.Domain.Entities.User", "Driver")
                        .WithMany()
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.RideCarSeatComposition", b =>
                {
                    b.HasOne("ShaRide.Domain.Entities.CarSeatComposition", "CarSeatComposition")
                        .WithMany()
                        .HasForeignKey("CarSeatCompositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShaRide.Domain.Entities.User", "Passenger")
                        .WithMany()
                        .HasForeignKey("PassengerId");

                    b.HasOne("ShaRide.Domain.Entities.Ride", "Ride")
                        .WithMany("RideCarSeatComposition")
                        .HasForeignKey("RideId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.RideLocationPointComposition", b =>
                {
                    b.HasOne("ShaRide.Domain.Entities.LocationPoint", "LocationPoint")
                        .WithMany()
                        .HasForeignKey("LocationPointId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShaRide.Domain.Entities.Ride", "Ride")
                        .WithMany("RideLocationPointComposition")
                        .HasForeignKey("RideId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.UserFcmToken", b =>
                {
                    b.HasOne("ShaRide.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.UserImage", b =>
                {
                    b.HasOne("ShaRide.Domain.Entities.User", "User")
                        .WithMany("UserImages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.UserPhone", b =>
                {
                    b.HasOne("ShaRide.Domain.Entities.User", "User")
                        .WithMany("Phones")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShaRide.Domain.Entities.UserRating", b =>
                {
                    b.HasOne("ShaRide.Domain.Entities.User", "DestinationUser")
                        .WithMany()
                        .HasForeignKey("DestinationUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShaRide.Domain.Entities.Ride", "Ride")
                        .WithMany()
                        .HasForeignKey("RideId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShaRide.Domain.Entities.User", "SourceUser")
                        .WithMany()
                        .HasForeignKey("SourceUserId")
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
