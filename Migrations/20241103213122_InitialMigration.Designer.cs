﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OrderPickingSystem.Context;

#nullable disable

namespace OrderPickingSystem.Migrations
{
    [DbContext(typeof(OrderPickingContext))]
    [Migration("20241103213122_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OrderPickingSystem.Models.Container", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("PaletteId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PaletteId");

                    b.ToTable("Containers");
                });

            modelBuilder.Entity("OrderPickingSystem.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LocationId")
                        .IsUnique();

                    b.ToTable("Items");
                });

            modelBuilder.Entity("OrderPickingSystem.Models.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Floor")
                        .HasColumnType("int");

                    b.Property<int>("Isle")
                        .HasColumnType("int");

                    b.Property<string>("Letter")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("OrderPickingSystem.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ContactNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CurrentUserId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("Destination")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrderStatus")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("OrderPickingSystem.Models.Palette", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("Palettes");
                });

            modelBuilder.Entity("OrderPickingSystem.Models.Pick", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ContainerId")
                        .HasColumnType("int");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<int?>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ContainerId");

                    b.HasIndex("OrderId");

                    b.ToTable("Picks");
                });

            modelBuilder.Entity("OrderPickingSystem.Models.Requests.PickRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<int?>("OrderId")
                        .HasColumnType("int");

                    b.Property<int?>("OrderId1")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("OrderId1");

                    b.ToTable("PickRequest");
                });

            modelBuilder.Entity("OrderPickingSystem.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CurrentOrderId")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserRights")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CurrentOrderId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1000,
                            PasswordHash = "$2a$11$IP.C48VvB89qgPdZE/gRpO1hykJzkbh.LTgxeRbLPbDC8IO5TGt0m",
                            UserRights = 1,
                            Username = "Vlad"
                        },
                        new
                        {
                            Id = 1001,
                            PasswordHash = "$2a$11$qRjLd6nYVhSqmbhffGj81ejMgoW7sHbsV7bnYSrAqSDEV84Z0eg26",
                            UserRights = 1,
                            Username = "Malaka"
                        },
                        new
                        {
                            Id = 1002,
                            PasswordHash = "$2a$11$rALanMZH0dIIpX8KvHdJkeMp8e3N533BB4Mfj5MHn5RYoFrDj0HbS",
                            UserRights = 1,
                            Username = "Tomas"
                        },
                        new
                        {
                            Id = 1004,
                            PasswordHash = "$2a$11$SGCsflodSDaRLr9zdTvLk.IZbL99RIsQ4UnkIvQVRBRSREpytJnEW",
                            UserRights = 0,
                            Username = "Admin"
                        });
                });

            modelBuilder.Entity("OrderPickingSystem.Models.Container", b =>
                {
                    b.HasOne("OrderPickingSystem.Models.Palette", null)
                        .WithMany("Containers")
                        .HasForeignKey("PaletteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OrderPickingSystem.Models.Item", b =>
                {
                    b.HasOne("OrderPickingSystem.Models.Location", null)
                        .WithOne("Item")
                        .HasForeignKey("OrderPickingSystem.Models.Item", "LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OrderPickingSystem.Models.Palette", b =>
                {
                    b.HasOne("OrderPickingSystem.Models.Order", null)
                        .WithMany("Palettes")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OrderPickingSystem.Models.Pick", b =>
                {
                    b.HasOne("OrderPickingSystem.Models.Container", null)
                        .WithMany("Picks")
                        .HasForeignKey("ContainerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OrderPickingSystem.Models.Order", null)
                        .WithMany("Picks")
                        .HasForeignKey("OrderId");
                });

            modelBuilder.Entity("OrderPickingSystem.Models.Requests.PickRequest", b =>
                {
                    b.HasOne("OrderPickingSystem.Models.Order", null)
                        .WithMany("ReplenishedRequestedItems")
                        .HasForeignKey("OrderId");

                    b.HasOne("OrderPickingSystem.Models.Order", null)
                        .WithMany("RequestedItems")
                        .HasForeignKey("OrderId1");
                });

            modelBuilder.Entity("OrderPickingSystem.Models.User", b =>
                {
                    b.HasOne("OrderPickingSystem.Models.Order", "CurrentOrder")
                        .WithMany()
                        .HasForeignKey("CurrentOrderId");

                    b.Navigation("CurrentOrder");
                });

            modelBuilder.Entity("OrderPickingSystem.Models.Container", b =>
                {
                    b.Navigation("Picks");
                });

            modelBuilder.Entity("OrderPickingSystem.Models.Location", b =>
                {
                    b.Navigation("Item")
                        .IsRequired();
                });

            modelBuilder.Entity("OrderPickingSystem.Models.Order", b =>
                {
                    b.Navigation("Palettes");

                    b.Navigation("Picks");

                    b.Navigation("ReplenishedRequestedItems");

                    b.Navigation("RequestedItems");
                });

            modelBuilder.Entity("OrderPickingSystem.Models.Palette", b =>
                {
                    b.Navigation("Containers");
                });
#pragma warning restore 612, 618
        }
    }
}
