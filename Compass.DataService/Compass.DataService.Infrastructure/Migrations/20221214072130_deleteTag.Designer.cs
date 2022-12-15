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
    [Migration("20221214072130_deleteTag")]
    partial class deleteTag
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

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Discriminator");

                    b.Property<double>("Height")
                        .HasColumnType("float")
                        .HasColumnName("Height");

                    b.Property<double>("Length")
                        .HasColumnType("float")
                        .HasColumnName("Length");

                    b.Property<double>("Width")
                        .HasColumnType("float")
                        .HasColumnName("Width");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.ToTable("ModulesData");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ModuleData");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Compass.DataService.Domain.Entities.KvfData", b =>
                {
                    b.HasBaseType("Compass.DataService.Domain.Entities.ModuleData");

                    b.Property<bool>("Ansul")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("Ansul");

                    b.Property<int>("AnsulDetector")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("AnsulDetector");

                    b.Property<int>("AnsulDrop")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("AnsulDrop");

                    b.Property<int>("AnsulSide")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("AnsulSide");

                    b.Property<bool>("BackCj")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("BackCj");

                    b.Property<bool>("BackToBack")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("BackToBack");

                    b.Property<bool>("CoverBoard")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("CoverBoard");

                    b.Property<int>("DrainType")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("DrainType");

                    b.Property<double>("ExhaustSpigotHeight")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("float")
                        .HasColumnName("ExhaustSpigotHeight");

                    b.Property<double>("ExhaustSpigotLength")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("float")
                        .HasColumnName("ExhaustSpigotLength");

                    b.Property<int>("ExhaustSpigotNumber")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("ExhaustSpigotNumber");

                    b.Property<double>("ExhaustSpigotWidth")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("float")
                        .HasColumnName("ExhaustSpigotWidth");

                    b.Property<bool>("LedLogo")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("LedLogo");

                    b.Property<int>("LightType")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("LightType");

                    b.Property<bool>("Marvel")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("Marvel");

                    b.Property<double>("MiddleToRight")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("float")
                        .HasColumnName("MiddleToRight");

                    b.Property<int>("SidePanel")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("SidePanel");

                    b.Property<double>("SpotLightDistance")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("float")
                        .HasColumnName("SpotLightDistance");

                    b.Property<int>("SpotLightNumber")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("SpotLightNumber");

                    b.Property<double>("SupplySpigotDistance")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("float")
                        .HasColumnName("SupplySpigotDistance");

                    b.Property<double>("SupplySpigotNumber")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("float")
                        .HasColumnName("SupplySpigotNumber");

                    b.Property<bool>("WaterCollection")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("WaterCollection");

                    b.HasDiscriminator().HasValue("KvfData");
                });

            modelBuilder.Entity("Compass.DataService.Domain.Entities.KviData", b =>
                {
                    b.HasBaseType("Compass.DataService.Domain.Entities.ModuleData");

                    b.Property<bool>("Ansul")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("Ansul");

                    b.Property<int>("AnsulDetector")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("AnsulDetector");

                    b.Property<int>("AnsulDrop")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("AnsulDrop");

                    b.Property<int>("AnsulSide")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("AnsulSide");

                    b.Property<bool>("BackCj")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("BackCj");

                    b.Property<bool>("BackToBack")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("BackToBack");

                    b.Property<bool>("CoverBoard")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("CoverBoard");

                    b.Property<int>("DrainType")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("DrainType");

                    b.Property<double>("ExhaustSpigotHeight")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("float")
                        .HasColumnName("ExhaustSpigotHeight");

                    b.Property<double>("ExhaustSpigotLength")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("float")
                        .HasColumnName("ExhaustSpigotLength");

                    b.Property<int>("ExhaustSpigotNumber")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("ExhaustSpigotNumber");

                    b.Property<double>("ExhaustSpigotWidth")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("float")
                        .HasColumnName("ExhaustSpigotWidth");

                    b.Property<bool>("LedLogo")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("LedLogo");

                    b.Property<int>("LightType")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("LightType");

                    b.Property<bool>("Marvel")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("Marvel");

                    b.Property<double>("MiddleToRight")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("float")
                        .HasColumnName("MiddleToRight");

                    b.Property<int>("SidePanel")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("SidePanel");

                    b.Property<double>("SpotLightDistance")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("float")
                        .HasColumnName("SpotLightDistance");

                    b.Property<int>("SpotLightNumber")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("SpotLightNumber");

                    b.Property<bool>("WaterCollection")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("WaterCollection");

                    b.HasDiscriminator().HasValue("KviData");
                });

            modelBuilder.Entity("Compass.DataService.Domain.Entities.UvfData", b =>
                {
                    b.HasBaseType("Compass.DataService.Domain.Entities.ModuleData");

                    b.Property<bool>("Ansul")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("Ansul");

                    b.Property<int>("AnsulDetector")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("AnsulDetector");

                    b.Property<int>("AnsulDrop")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("AnsulDrop");

                    b.Property<int>("AnsulSide")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("AnsulSide");

                    b.Property<bool>("BackCj")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("BackCj");

                    b.Property<bool>("BackToBack")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("BackToBack");

                    b.Property<bool>("BlueTooth")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("BlueTooth");

                    b.Property<bool>("CoverBoard")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("CoverBoard");

                    b.Property<int>("DrainType")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("DrainType");

                    b.Property<double>("ExhaustSpigotHeight")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("float")
                        .HasColumnName("ExhaustSpigotHeight");

                    b.Property<double>("ExhaustSpigotLength")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("float")
                        .HasColumnName("ExhaustSpigotLength");

                    b.Property<int>("ExhaustSpigotNumber")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("ExhaustSpigotNumber");

                    b.Property<double>("ExhaustSpigotWidth")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("float")
                        .HasColumnName("ExhaustSpigotWidth");

                    b.Property<bool>("LedLogo")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("LedLogo");

                    b.Property<int>("LightType")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("LightType");

                    b.Property<bool>("Marvel")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("Marvel");

                    b.Property<double>("MiddleToRight")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("float")
                        .HasColumnName("MiddleToRight");

                    b.Property<int>("SidePanel")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("SidePanel");

                    b.Property<double>("SpotLightDistance")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("float")
                        .HasColumnName("SpotLightDistance");

                    b.Property<int>("SpotLightNumber")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("SpotLightNumber");

                    b.Property<double>("SupplySpigotDistance")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("float")
                        .HasColumnName("SupplySpigotDistance");

                    b.Property<double>("SupplySpigotNumber")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("float")
                        .HasColumnName("SupplySpigotNumber");

                    b.Property<int>("UvLightType")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("UvLightType");

                    b.Property<bool>("WaterCollection")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("WaterCollection");

                    b.HasDiscriminator().HasValue("UvfData");
                });

            modelBuilder.Entity("Compass.DataService.Domain.Entities.UviData", b =>
                {
                    b.HasBaseType("Compass.DataService.Domain.Entities.ModuleData");

                    b.Property<bool>("Ansul")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("Ansul");

                    b.Property<int>("AnsulDetector")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("AnsulDetector");

                    b.Property<int>("AnsulDrop")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("AnsulDrop");

                    b.Property<int>("AnsulSide")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("AnsulSide");

                    b.Property<bool>("BackCj")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("BackCj");

                    b.Property<bool>("BackToBack")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("BackToBack");

                    b.Property<bool>("BlueTooth")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("BlueTooth");

                    b.Property<bool>("CoverBoard")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("CoverBoard");

                    b.Property<int>("DrainType")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("DrainType");

                    b.Property<double>("ExhaustSpigotHeight")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("float")
                        .HasColumnName("ExhaustSpigotHeight");

                    b.Property<double>("ExhaustSpigotLength")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("float")
                        .HasColumnName("ExhaustSpigotLength");

                    b.Property<int>("ExhaustSpigotNumber")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("ExhaustSpigotNumber");

                    b.Property<double>("ExhaustSpigotWidth")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("float")
                        .HasColumnName("ExhaustSpigotWidth");

                    b.Property<bool>("LedLogo")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("LedLogo");

                    b.Property<int>("LightType")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("LightType");

                    b.Property<bool>("Marvel")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("Marvel");

                    b.Property<double>("MiddleToRight")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("float")
                        .HasColumnName("MiddleToRight");

                    b.Property<int>("SidePanel")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("SidePanel");

                    b.Property<double>("SpotLightDistance")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("float")
                        .HasColumnName("SpotLightDistance");

                    b.Property<int>("SpotLightNumber")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("SpotLightNumber");

                    b.Property<int>("UvLightType")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("int")
                        .HasColumnName("UvLightType");

                    b.Property<bool>("WaterCollection")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("bit")
                        .HasColumnName("WaterCollection");

                    b.HasDiscriminator().HasValue("UviData");
                });
#pragma warning restore 612, 618
        }
    }
}
