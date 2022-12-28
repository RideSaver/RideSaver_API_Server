﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ServicesAPI.Data;

#nullable disable

namespace ServicesAPI.Data.Migrations
{
    [DbContext(typeof(ServiceContext))]
    [Migration("20221228095010_UpdateRefreshToken")]
    partial class UpdateRefreshToken
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("DataAccess.DataModels.ProviderModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedAt")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("providers");
                });

            modelBuilder.Entity("DataAccess.DataModels.ServiceAreaModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("ServicesModelId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("ServicesModelId");

                    b.ToTable("service_areas");
                });

            modelBuilder.Entity("DataAccess.DataModels.ServiceFeaturesModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("Feature")
                        .HasColumnType("int");

                    b.Property<Guid?>("ServicesModelId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("ServicesModelId");

                    b.ToTable("service_features");
                });

            modelBuilder.Entity("DataAccess.DataModels.ServicesModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedAt")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("ProviderId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("ProviderId");

                    b.ToTable("services");
                });

            modelBuilder.Entity("DataAccess.DataModels.ServiceAreaModel", b =>
                {
                    b.HasOne("DataAccess.DataModels.ServicesModel", null)
                        .WithMany("ServiceArea")
                        .HasForeignKey("ServicesModelId");
                });

            modelBuilder.Entity("DataAccess.DataModels.ServiceFeaturesModel", b =>
                {
                    b.HasOne("DataAccess.DataModels.ServicesModel", null)
                        .WithMany("ServiceFeatures")
                        .HasForeignKey("ServicesModelId");
                });

            modelBuilder.Entity("DataAccess.DataModels.ServicesModel", b =>
                {
                    b.HasOne("DataAccess.DataModels.ProviderModel", "ProviderModel")
                        .WithMany("Services")
                        .HasForeignKey("ProviderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProviderModel");
                });

            modelBuilder.Entity("DataAccess.DataModels.ProviderModel", b =>
                {
                    b.Navigation("Services");
                });

            modelBuilder.Entity("DataAccess.DataModels.ServicesModel", b =>
                {
                    b.Navigation("ServiceArea");

                    b.Navigation("ServiceFeatures");
                });
#pragma warning restore 612, 618
        }
    }
}
