﻿// <auto-generated />
using System;
using APForums.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace APForums.Server.Migrations
{
    [DbContext(typeof(ForumsDbContext))]
    [Migration("20230720041011_Create-Connections-Table")]
    partial class CreateConnectionsTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("APForums.Server.Models.Club", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("Name");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("clubs");
                });

            modelBuilder.Entity("APForums.Server.Models.Connection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("Date")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int>("FollowedId")
                        .HasColumnType("int");

                    b.Property<int>("FollowerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FollowedId");

                    b.HasIndex("FollowerId");

                    b.ToTable("connections");
                });

            modelBuilder.Entity("APForums.Server.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("ClubId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ImagePath")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("PostedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP()");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("SubTitle")
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("Title");

                    b.HasKey("Id");

                    b.HasIndex("ClubId");

                    b.ToTable("events");
                });

            modelBuilder.Entity("APForums.Server.Models.Social", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP()");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("socials");
                });

            modelBuilder.Entity("APForums.Server.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Course")
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime?>("DOB")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("DOB");

                    b.Property<string>("DegreeType")
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Department")
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("Email")
                        .HasComputedColumnSql("CONCAT(TpNumber, '@mail.apu.edu.my')", false);

                    b.Property<string>("Enrollment")
                        .HasColumnType("nvarchar(20)");

                    b.Property<int?>("Level")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("Phone");

                    b.Property<string>("TPNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)")
                        .HasColumnName("TpNumber");

                    b.HasKey("Id");

                    b.HasIndex("TPNumber")
                        .IsUnique();

                    b.ToTable("users");
                });

            modelBuilder.Entity("APForums.Server.Models.UserClub", b =>
                {
                    b.Property<int>("ClubId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP()");

                    b.Property<string>("Role")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(10)")
                        .HasDefaultValue("Member");

                    b.HasKey("ClubId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("users_clubs");
                });

            modelBuilder.Entity("APForums.Server.Models.Connection", b =>
                {
                    b.HasOne("APForums.Server.Models.User", "Follower")
                        .WithMany()
                        .HasForeignKey("FollowedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APForums.Server.Models.User", "Followed")
                        .WithMany()
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Followed");

                    b.Navigation("Follower");
                });

            modelBuilder.Entity("APForums.Server.Models.Event", b =>
                {
                    b.HasOne("APForums.Server.Models.Club", "Club")
                        .WithMany()
                        .HasForeignKey("ClubId");

                    b.Navigation("Club");
                });

            modelBuilder.Entity("APForums.Server.Models.Social", b =>
                {
                    b.HasOne("APForums.Server.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("APForums.Server.Models.UserClub", b =>
                {
                    b.HasOne("APForums.Server.Models.Club", null)
                        .WithMany("UserClubs")
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APForums.Server.Models.User", null)
                        .WithMany("UserClubs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("APForums.Server.Models.Club", b =>
                {
                    b.Navigation("UserClubs");
                });

            modelBuilder.Entity("APForums.Server.Models.User", b =>
                {
                    b.Navigation("UserClubs");
                });
#pragma warning restore 612, 618
        }
    }
}