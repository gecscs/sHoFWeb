// <auto-generated />
using System;
using HoFWeb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HoFWeb.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HoFWeb.Models.Creator", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatorName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatorNameLatinized")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatorNameLocale")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatorNameSlug")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatorNameTranslated")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Creators");
                });

            modelBuilder.Entity("HoFWeb.Models.ScreenshotDataPoint", b =>
                {
                    b.Property<Guid>("ScreenshotScreenshotDataPointId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Favorites")
                        .HasColumnType("int");

                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Views")
                        .HasColumnType("int");

                    b.HasKey("ScreenshotScreenshotDataPointId");

                    b.ToTable("ScreenshotDataPoints");
                });

            modelBuilder.Entity("HoFWeb.Models.ScreenshotItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("CityMilestone")
                        .HasColumnType("int");

                    b.Property<string>("CityName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CityNameLatinized")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CityNameLocale")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CityNameTranslated")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CityPopulation")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedAtFormatted")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedAtFormattedDistance")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Favorited")
                        .HasColumnType("bit");

                    b.Property<int>("FavoritesCount")
                        .HasColumnType("int");

                    b.Property<double>("FavoritesPerDay")
                        .HasColumnType("float");

                    b.Property<double>("FavoritingPercentage")
                        .HasColumnType("float");

                    b.Property<string>("ImageUrl4K")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrlFHD")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrlThumbnail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<bool>("IsReported")
                        .HasColumnType("bit");

                    b.Property<int>("ViewsCount")
                        .HasColumnType("int");

                    b.Property<double>("ViewsPerDay")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Screenshots");
                });

            modelBuilder.Entity("HoFWeb.Models.ScreenshotItem", b =>
                {
                    b.HasOne("HoFWeb.Models.Creator", "Creator")
                        .WithMany("ScreenshotItems")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("HoFWeb.Models.Creator", b =>
                {
                    b.Navigation("ScreenshotItems");
                });
#pragma warning restore 612, 618
        }
    }
}
