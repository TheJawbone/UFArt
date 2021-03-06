﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UFArt.Models;

namespace UFArt.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20181116164834_ArtPieceTextAssets")]
    partial class ArtPieceTextAssets
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("UFArt.Models.Gallery.ArtPiece", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AdditionalInfoId");

                    b.Property<string>("CreationDate");

                    b.Property<int?>("DescriptionId");

                    b.Property<string>("Dimensions");

                    b.Property<bool>("ForSale");

                    b.Property<string>("ImageUri");

                    b.Property<int?>("NameId");

                    b.Property<int>("TechniqueID");

                    b.HasKey("ID");

                    b.HasIndex("AdditionalInfoId");

                    b.HasIndex("DescriptionId");

                    b.HasIndex("NameId");

                    b.HasIndex("TechniqueID");

                    b.ToTable("ArtPieces");
                });

            modelBuilder.Entity("UFArt.Models.Gallery.Technique", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("NameId");

                    b.HasKey("ID");

                    b.HasIndex("NameId");

                    b.ToTable("Techniques");
                });

            modelBuilder.Entity("UFArt.Models.Identity.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Email");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail");

                    b.Property<string>("NormalizedUserName");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("UFArt.Models.Newsfeed.News", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("HeaderId");

                    b.Property<string>("ImageUrl");

                    b.Property<int?>("TextId");

                    b.Property<DateTime>("Timestamp");

                    b.HasKey("ID");

                    b.HasIndex("HeaderId");

                    b.HasIndex("TextId");

                    b.ToTable("News");
                });

            modelBuilder.Entity("UFArt.Models.TextAssets.TextAsset", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Key");

                    b.Property<string>("Value_en");

                    b.Property<string>("Value_pl");

                    b.HasKey("Id");

                    b.ToTable("TextAssets");
                });

            modelBuilder.Entity("UFArt.Models.Gallery.ArtPiece", b =>
                {
                    b.HasOne("UFArt.Models.TextAssets.TextAsset", "AdditionalInfo")
                        .WithMany()
                        .HasForeignKey("AdditionalInfoId");

                    b.HasOne("UFArt.Models.TextAssets.TextAsset", "Description")
                        .WithMany()
                        .HasForeignKey("DescriptionId");

                    b.HasOne("UFArt.Models.TextAssets.TextAsset", "Name")
                        .WithMany()
                        .HasForeignKey("NameId");

                    b.HasOne("UFArt.Models.Gallery.Technique", "Technique")
                        .WithMany()
                        .HasForeignKey("TechniqueID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("UFArt.Models.Gallery.Technique", b =>
                {
                    b.HasOne("UFArt.Models.TextAssets.TextAsset", "Name")
                        .WithMany()
                        .HasForeignKey("NameId");
                });

            modelBuilder.Entity("UFArt.Models.Newsfeed.News", b =>
                {
                    b.HasOne("UFArt.Models.TextAssets.TextAsset", "Header")
                        .WithMany()
                        .HasForeignKey("HeaderId");

                    b.HasOne("UFArt.Models.TextAssets.TextAsset", "Text")
                        .WithMany()
                        .HasForeignKey("TextId");
                });
#pragma warning restore 612, 618
        }
    }
}
