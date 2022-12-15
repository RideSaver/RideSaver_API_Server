﻿// <auto-generated />
using System;
using IdentityService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IdentityService.Data.Migrations
{
    [DbContext(typeof(UserContext))]
    partial class UserContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("DataAccess.DataModels.ServicesModel", b =>
                {
                    b.HasOne("DataAccess.DataModels.ProviderModel", "ProviderModel")
                        .WithMany("Services")
                        .HasForeignKey("ProviderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("DataAccess.DataModels.ServiceAreaModel", "ServiceArea", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("char(36)");

                            b1.Property<Guid>("ServicesModelId")
                                .HasColumnType("char(36)");

                            b1.HasKey("Id");

                            b1.HasIndex("ServicesModelId");

                            b1.ToTable("service_areas");

                            b1.WithOwner()
                                .HasForeignKey("ServicesModelId");
                        });

                    b.OwnsMany("DataAccess.DataModels.ServiceFeaturesModel", "ServiceFeatures", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("char(36)");

                            b1.Property<int>("Feature")
                                .HasColumnType("int");

                            b1.Property<Guid>("ServicesModelId")
                                .HasColumnType("char(36)");

                            b1.HasKey("Id");

                            b1.HasIndex("ServicesModelId");

                            b1.ToTable("service_features");

                            b1.WithOwner()
                                .HasForeignKey("ServicesModelId");
                        });

                    b.Navigation("ProviderModel");

                    b.Navigation("ServiceArea");

                    b.Navigation("ServiceFeatures");
                });

            modelBuilder.Entity("DataAccess.DataModels.UserModel", b =>
                {
                    b.OwnsMany("DataAccess.DataModels.AuthorizationModel", "Authorizations", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("char(36)");

                            b1.Property<string>("RefreshToken")
                                .IsRequired()
                                .HasColumnType("longtext");

                            b1.Property<Guid>("ServiceId")
                                .HasColumnType("char(36)");

                            b1.Property<Guid>("UserId")
                                .HasColumnType("char(36)");

                            b1.Property<Guid>("UserModelId")
                                .HasColumnType("char(36)");

                            b1.HasKey("Id");

                            b1.HasIndex("UserModelId");

                            b1.ToTable("authorizations");

                            b1.WithOwner()
                                .HasForeignKey("UserModelId");
                        });

                    b.OwnsMany("DataAccess.DataModels.RefreshToken", "RefreshTokens", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("char(36)");

                            b1.Property<DateTime>("Created")
                                .HasColumnType("datetime(6)");

                            b1.Property<string>("CreatedByIp")
                                .HasColumnType("longtext");

                            b1.Property<DateTime>("Expires")
                                .HasColumnType("datetime(6)");

                            b1.Property<string>("ReplacedByToken")
                                .HasColumnType("longtext");

                            b1.Property<DateTime?>("Revoked")
                                .HasColumnType("datetime(6)");

                            b1.Property<string>("RevokedByIp")
                                .HasColumnType("longtext");

                            b1.Property<string>("Token")
                                .HasColumnType("longtext");

                            b1.Property<Guid>("UserModelId")
                                .HasColumnType("char(36)");

                            b1.HasKey("Id");

                            b1.HasIndex("UserModelId");

                            b1.ToTable("RefreshToken");

                            b1.WithOwner()
                                .HasForeignKey("UserModelId");
                        });

                    b.OwnsMany("DataAccess.DataModels.RideHistoryModel", "RideHistory", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("char(36)");

                            b1.Property<string>("Currency")
                                .IsRequired()
                                .HasColumnType("longtext");

                            b1.Property<DateTime>("Date")
                                .IsConcurrencyToken()
                                .ValueGeneratedOnAddOrUpdate()
                                .HasColumnType("datetime(6)");

                            b1.Property<int>("Price")
                                .HasColumnType("int");

                            b1.Property<Guid>("ServiceId")
                                .HasColumnType("char(36)");

                            b1.Property<string>("Url")
                                .IsRequired()
                                .HasColumnType("longtext");

                            b1.Property<Guid>("UserModelId")
                                .HasColumnType("char(36)");

                            b1.HasKey("Id");

                            b1.HasIndex("ServiceId");

                            b1.HasIndex("UserModelId");

                            b1.ToTable("ride_history");

                            b1.HasOne("DataAccess.DataModels.ServicesModel", "ServicesModel")
                                .WithMany()
                                .HasForeignKey("ServiceId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.WithOwner()
                                .HasForeignKey("UserModelId");

                            b1.Navigation("ServicesModel");
                        });

                    b.Navigation("Authorizations");

                    b.Navigation("RefreshTokens");

                    b.Navigation("RideHistory");
                });

            modelBuilder.Entity("DataAccess.DataModels.ProviderModel", b =>
                {
                    b.Navigation("Services");
                });
#pragma warning restore 612, 618
        }
    }
}
