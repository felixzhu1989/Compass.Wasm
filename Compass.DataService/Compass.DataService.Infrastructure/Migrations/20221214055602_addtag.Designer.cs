﻿// <auto-generated />
using System;
using Compass.DataService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Compass.DataService.Infrastructure.Migrations
{
    [DbContext(typeof(DataDbContext))]
    [Migration("20221214055602_addtag")]
    partial class addtag
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Compass.DataService.Domain.Entities.ModuleData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id");

                    b.Property<double>("Height")
                        .HasColumnType("float")
                        .HasColumnName("Height");

                    b.Property<double>("Length")
                        .HasColumnType("float")
                        .HasColumnName("Length");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Tag");

                    b.Property<double>("Width")
                        .HasColumnType("float")
                        .HasColumnName("Width");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.ToTable("ModulesData");
                });
#pragma warning restore 612, 618
        }
    }
}