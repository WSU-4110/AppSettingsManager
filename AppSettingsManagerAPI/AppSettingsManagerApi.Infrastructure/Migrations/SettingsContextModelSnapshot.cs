﻿// <auto-generated />
using System;
using AppSettingsManagerApi.Infrastructure.MySql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AppSettingsManagerApi.Infrastructure.Migrations
{
    [DbContext(typeof(SettingsContext))]
    partial class SettingsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("AppSettingsManagerApi.Infrastructure.MySql.Permission", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(36)");

                    b.Property<string>("SettingGroupId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ApprovedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("CurrentPermissionLevel")
                        .HasColumnType("int");

                    b.Property<int>("RequestedPermissionLevel")
                        .HasColumnType("int");

                    b.Property<bool>("WaitingForApproval")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("UserId", "SettingGroupId");

                    b.HasIndex("SettingGroupId");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("AppSettingsManagerApi.Infrastructure.MySql.Setting", b =>
                {
                    b.Property<string>("SettingGroupId")
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.Property<string>("Input")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsCurrent")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("LastUpdatedAt")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp(6)");

                    b.HasKey("SettingGroupId", "Version");

                    b.HasIndex("IsCurrent");

                    b.HasIndex("SettingGroupId");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("AppSettingsManagerApi.Infrastructure.MySql.SettingGroup", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("LastUpdatedAt")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp(6)");

                    b.HasKey("Id");

                    b.ToTable("SettingGroups");
                });

            modelBuilder.Entity("AppSettingsManagerApi.Infrastructure.MySql.User", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AppSettingsManagerApi.Infrastructure.MySql.Permission", b =>
                {
                    b.HasOne("AppSettingsManagerApi.Infrastructure.MySql.SettingGroup", "SettingGroup")
                        .WithMany("Permissions")
                        .HasForeignKey("SettingGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AppSettingsManagerApi.Infrastructure.MySql.User", "User")
                        .WithMany("Permissions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SettingGroup");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AppSettingsManagerApi.Infrastructure.MySql.Setting", b =>
                {
                    b.HasOne("AppSettingsManagerApi.Infrastructure.MySql.SettingGroup", "SettingGroup")
                        .WithMany("Settings")
                        .HasForeignKey("SettingGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SettingGroup");
                });

            modelBuilder.Entity("AppSettingsManagerApi.Infrastructure.MySql.SettingGroup", b =>
                {
                    b.Navigation("Permissions");

                    b.Navigation("Settings");
                });

            modelBuilder.Entity("AppSettingsManagerApi.Infrastructure.MySql.User", b =>
                {
                    b.Navigation("Permissions");
                });
#pragma warning restore 612, 618
        }
    }
}
