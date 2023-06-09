﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TheProjector.Application.Persistence;

#nullable disable

namespace TheProjector.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20230505103817_AddProjectsRelatedTables")]
    partial class AddProjectsRelatedTables
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("TheProjector.Application.Persistence.Person", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("TheProjector.Application.Persistence.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Budget")
                        .HasColumnType("decimal(18,4)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(MAX)");

                    b.HasKey("Id");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("TheProjector.Application.Persistence.ProjectAssignment", b =>
                {
                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsOwner")
                        .HasColumnType("bit");

                    b.HasKey("ProjectId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("ProjectAssignment");
                });

            modelBuilder.Entity("TheProjector.Application.Persistence.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = new Guid("e27b8fa8-6c20-414f-bd0a-d9c98f808d25"),
                            Name = "Administrator"
                        },
                        new
                        {
                            Id = new Guid("d48c2db8-da40-4336-9475-e8dd42b306d7"),
                            Name = "Manager"
                        },
                        new
                        {
                            Id = new Guid("7f13ba44-9a9d-4eac-9615-98dc610c5563"),
                            Name = "Employee"
                        });
                });

            modelBuilder.Entity("TheProjector.Application.Persistence.Status", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.ToTable("Status");

                    b.HasData(
                        new
                        {
                            Id = new Guid("ca7a477d-ab7a-41e1-8d0c-86a91efd219d"),
                            Name = "Active"
                        },
                        new
                        {
                            Id = new Guid("95130666-173e-41d2-bfa1-6faae47355f8"),
                            Name = "Pending"
                        },
                        new
                        {
                            Id = new Guid("4ece0109-f9b9-4f55-822b-1524508e27f6"),
                            Name = "Disabled"
                        });
                });

            modelBuilder.Entity("TheProjector.Application.Persistence.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("PasswordResetToken")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<DateTime>("PasswordResetTokenExpiry")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("StatusId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("StatusId");

                    b.HasIndex(new[] { "Email" }, "user_email_index")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TheProjector.Application.Persistence.Person", b =>
                {
                    b.HasOne("TheProjector.Application.Persistence.User", "User")
                        .WithOne("Person")
                        .HasForeignKey("TheProjector.Application.Persistence.Person", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TheProjector.Application.Persistence.ProjectAssignment", b =>
                {
                    b.HasOne("TheProjector.Application.Persistence.Project", "Project")
                        .WithMany("ProjectAssignments")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheProjector.Application.Persistence.User", "User")
                        .WithMany("ProjectAssignments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TheProjector.Application.Persistence.User", b =>
                {
                    b.HasOne("TheProjector.Application.Persistence.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheProjector.Application.Persistence.Status", "Status")
                        .WithMany("Users")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("TheProjector.Application.Persistence.Project", b =>
                {
                    b.Navigation("ProjectAssignments");
                });

            modelBuilder.Entity("TheProjector.Application.Persistence.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("TheProjector.Application.Persistence.Status", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("TheProjector.Application.Persistence.User", b =>
                {
                    b.Navigation("Person");

                    b.Navigation("ProjectAssignments");
                });
#pragma warning restore 612, 618
        }
    }
}
