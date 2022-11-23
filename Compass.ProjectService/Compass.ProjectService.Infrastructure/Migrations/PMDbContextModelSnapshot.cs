﻿// <auto-generated />
using System;
using Compass.ProjectService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Compass.ProjectService.Infrastructure.Migrations
{
    [DbContext(typeof(PMDbContext))]
    partial class PMDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Compass.ProjectService.Domain.Entities.Drawing", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("DrawingUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ItemNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("Id", "IsDeleted");

                    b.HasIndex("ProjectId", "IsDeleted");

                    b.ToTable("Drawings");
                });

            modelBuilder.Entity("Compass.ProjectService.Domain.Entities.DrawingPlan", b =>
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

                    b.Property<DateTime>("ReleaseTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("Id", "IsDeleted");

                    b.ToTable("DrawingsPlan");
                });

            modelBuilder.Entity("Compass.ProjectService.Domain.Entities.Issue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DescriptionUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ProjectStatus")
                        .HasColumnType("int");

                    b.Property<Guid>("ReportUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("Id", "IsDeleted");

                    b.HasIndex("ProjectId", "IsDeleted");

                    b.ToTable("Issues");
                });

            modelBuilder.Entity("Compass.ProjectService.Domain.Entities.Module", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("DrawingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsReleased")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ModelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SpecialNotes")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("DrawingId", "IsDeleted");

                    b.HasIndex("Id", "IsDeleted");

                    b.ToTable("Modules");
                });

            modelBuilder.Entity("Compass.ProjectService.Domain.Entities.Problem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CloseTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Deadline")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DescriptionUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ProblemTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ReportUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ResponseUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Solution")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SolutionUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("Id", "IsDeleted");

                    b.HasIndex("ProjectId", "IsDeleted");

                    b.ToTable("Problems");
                });

            modelBuilder.Entity("Compass.ProjectService.Domain.Entities.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AttachmentsUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BomUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContractUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeliveryDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OdpNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ProjectType")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReceiveDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RiskLevel")
                        .HasColumnType("int");

                    b.Property<string>("SpecialNotes")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("Id", "IsDeleted");

                    b.HasIndex("OdpNumber", "IsDeleted");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Compass.ProjectService.Domain.Entities.Tracking", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ClosedTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DrawingPlanedTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModuleReleaseTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("ProblemNotResolved")
                        .HasColumnType("bit");

                    b.Property<int>("ProjectStatus")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ShippingTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("WarehousingTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("Id", "IsDeleted");

                    b.ToTable("Trackings");
                });
#pragma warning restore 612, 618
        }
    }
}
