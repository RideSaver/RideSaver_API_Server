﻿// <auto-generated />
using System;
using IdentityService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IdentityService.Data.Migrations
{
    [DbContext(typeof(UserContext))]
    [Migration("20221228094933_UpdateRefreshToken")]
    partial class UpdateRefreshToken
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("DataAccess.DataModels.AuthorizationModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("RefreshhToken")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("ServiceId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("UserModelId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("UserModelId");

                    b.ToTable("authorizations");
                });

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

            modelBuilder.Entity("DataAccess.DataModels.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreatedByIp")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ReplacedByToken")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("Revoked")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("RevokedByIp")
                        .HasColumnType("longtext");

                    b.Property<string>("Token")
                        .HasColumnType("longtext");

                    b.Property<Guid?>("UserModelId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("UserModelId");

                    b.ToTable("refresh_tokens");
                });

            modelBuilder.Entity("DataAccess.DataModels.RideHistoryModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Date")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<Guid>("ServiceId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid?>("UserModelId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("ServiceId");

                    b.HasIndex("UserModelId");

                    b.ToTable("ride_history");
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

            modelBuilder.Entity("DataAccess.DataModels.UserModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Avatar")
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("DataAccess.DataModels.AuthorizationModel", b =>
                {
                    b.HasOne("DataAccess.DataModels.UserModel", null)
                        .WithMany("Authorizations")
                        .HasForeignKey("UserModelId");
                });

            modelBuilder.Entity("DataAccess.DataModels.RefreshToken", b =>
                {
                    b.HasOne("DataAccess.DataModels.UserModel", null)
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserModelId");
                });

            modelBuilder.Entity("DataAccess.DataModels.RideHistoryModel", b =>
                {
                    b.HasOne("DataAccess.DataModels.ServicesModel", "ServicesModel")
                        .WithMany()
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.DataModels.UserModel", null)
                        .WithMany("RideHistory")
                        .HasForeignKey("UserModelId");

                    b.Navigation("ServicesModel");
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

            modelBuilder.Entity("DataAccess.DataModels.UserModel", b =>
                {
                    b.Navigation("Authorizations");

                    b.Navigation("RefreshTokens");

                    b.Navigation("RideHistory");
                });
#pragma warning restore 612, 618
        }
    }
}
