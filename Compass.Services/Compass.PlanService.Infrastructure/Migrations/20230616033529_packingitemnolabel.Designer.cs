﻿// <auto-generated />
using System;
using Compass.PlanService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Compass.PlanService.Infrastructure.Migrations
{
    [DbContext(typeof(PlanDbContext))]
    [Migration("20230616033529_packingitemnolabel")]
    partial class packingitemnolabel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Compass.PlanService.Domain.Entities.Issue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CloseTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContentUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Deadline")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("MainPlanId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ReporterId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ResponderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Solution")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SolutionUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("Id", "IsDeleted");

                    b.HasIndex("MainPlanId", "IsDeleted");

                    b.ToTable("Issues");
                });

            modelBuilder.Entity("Compass.PlanService.Domain.Entities.MainPlan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Batch")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ClosedTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DrwReleaseTarget")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DrwReleaseTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FinishTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("ItemLine")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("MainPlanType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModelSummary")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("MonthOfInvoice")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ShippingTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<double>("Value")
                        .HasColumnType("float");

                    b.Property<DateTime?>("WarehousingTime")
                        .HasColumnType("datetime2");

                    b.Property<double>("Workload")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("Id", "IsDeleted");

                    b.HasIndex("ProjectId", "IsDeleted");

                    b.ToTable("MainPlans");
                });

            modelBuilder.Entity("Compass.PlanService.Domain.Entities.PackingItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CalcRule")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EnDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Height")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Length")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Material")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MtlNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("NoLabel")
                        .HasColumnType("bit");

                    b.Property<bool>("OneLabel")
                        .HasColumnType("bit");

                    b.Property<Guid>("PackingListId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Pallet")
                        .HasColumnType("bit");

                    b.Property<double>("Quantity")
                        .HasColumnType("float");

                    b.Property<bool>("ReCreate")
                        .HasColumnType("bit");

                    b.Property<string>("Remark")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Width")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("Id", "IsDeleted");

                    b.HasIndex("PackingListId", "IsDeleted");

                    b.ToTable("PackingItems");
                });

            modelBuilder.Entity("Compass.PlanService.Domain.Entities.PackingList", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AssyPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeliveryType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("MainPlanId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PackingType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Product")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("Id", "IsDeleted");

                    b.HasIndex("MainPlanId", "IsDeleted");

                    b.ToTable("PackingLists");
                });

            modelBuilder.Entity("Compass.PlanService.Domain.Entities.Pallet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("GrossWeight")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Height")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ItemHeight")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ItemLength")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ItemNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ItemType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ItemWidth")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Length")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NetWeight")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PackingListId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("PalletNumber")
                        .HasColumnType("int");

                    b.Property<string>("Remark")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Width")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("Id", "IsDeleted");

                    b.HasIndex("PackingListId", "IsDeleted");

                    b.ToTable("Pallets");
                });
#pragma warning restore 612, 618
        }
    }
}
