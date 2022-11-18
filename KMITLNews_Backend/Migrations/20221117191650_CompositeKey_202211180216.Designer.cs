﻿// <auto-generated />
using System;
using KMITLNews_Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace UserAPI.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20221117191650_CompositeKey_202211180216")]
    partial class CompositeKey202211180216
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("KMITLNews_Backend.Models.Advertiser", b =>
                {
                    b.Property<int>("advertiser_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("advertiser_id"));

                    b.Property<string>("ad_image_url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("advertiser_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("advertiser_id");

                    b.ToTable("Advertisers");
                });

            modelBuilder.Entity("KMITLNews_Backend.Models.Post", b =>
                {
                    b.Property<int>("post_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("post_id"));

                    b.Property<string>("attached_image_url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("post_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("post_text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("report_count")
                        .HasColumnType("int");

                    b.Property<bool>("verified")
                        .HasColumnType("bit");

                    b.HasKey("post_id");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("KMITLNews_Backend.Models.Posts_Users", b =>
                {
                    b.Property<int>("post_id")
                        .HasColumnType("int");

                    b.Property<int>("user_id")
                        .HasColumnType("int");

                    b.HasKey("post_id", "user_id");

                    b.ToTable("Posts_Users");
                });

            modelBuilder.Entity("KMITLNews_Backend.Models.Tags_Follows", b =>
                {
                    b.Property<string>("tag_name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("follower_id")
                        .HasColumnType("int");

                    b.HasKey("tag_name", "follower_id");

                    b.ToTable("Tags_Follows");
                });

            modelBuilder.Entity("KMITLNews_Backend.Models.Tags_Posts", b =>
                {
                    b.Property<string>("tag_name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("post_id")
                        .HasColumnType("int");

                    b.HasKey("tag_name", "post_id");

                    b.ToTable("Tags_Posts");
                });

            modelBuilder.Entity("KMITLNews_Backend.Models.User", b =>
                {
                    b.Property<int>("user_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("user_id"));

                    b.Property<string>("display_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("first_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("last_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("mobile_no")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("pass_hash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("pass_salt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("profile_pic_url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("report_count")
                        .HasColumnType("int");

                    b.Property<int>("user_type")
                        .HasColumnType("int");

                    b.Property<string>("verificationToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("verified")
                        .HasColumnType("int");

                    b.HasKey("user_id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("KMITLNews_Backend.Models.Users_Follows", b =>
                {
                    b.Property<int>("user_id")
                        .HasColumnType("int");

                    b.Property<int>("follower_id")
                        .HasColumnType("int");

                    b.HasKey("user_id", "follower_id");

                    b.ToTable("Users_Follows");
                });

            modelBuilder.Entity("KMITLNews_Backend.Models.Users_SharedPosts", b =>
                {
                    b.Property<int>("user_id")
                        .HasColumnType("int");

                    b.Property<int>("shared_post_id")
                        .HasColumnType("int");

                    b.HasKey("user_id", "shared_post_id");

                    b.ToTable("Users_SharedPosts");
                });
#pragma warning restore 612, 618
        }
    }
}