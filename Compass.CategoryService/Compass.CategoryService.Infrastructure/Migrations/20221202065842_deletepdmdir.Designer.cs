﻿// <auto-generated />
using System;
using Compass.CategoryService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Compass.CategoryService.Infrastructure.Migrations
{
    [DbContext(typeof(CSDbContext))]
    [Migration("20221202065842_deletepdmdir")]
    partial class deletepdmdir
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Compass.CategoryService.Domain.Entities.Model", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("SequenceNumber")
                        .HasColumnType("int");

                    b.Property<double>("Workload")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("ProductId", "IsDeleted");

                    b.ToTable("Models");
                });

            modelBuilder.Entity("Compass.CategoryService.Domain.Entities.ModelType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ModelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SequenceNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("ModelId", "IsDeleted");

                    b.ToTable("ModelTypes");
                });

            modelBuilder.Entity("Compass.CategoryService.Domain.Entities.ProblemType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Stakeholder")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"));

                    b.ToTable("ProblemTypes");
                });

            modelBuilder.Entity("Compass.CategoryService.Domain.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Sbu")
                        .HasColumnType("int");

                    b.Property<int>("SequenceNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"));

                    b.ToTable("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
