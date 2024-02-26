﻿// <auto-generated />
using DataProcessorSrvice.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataProcessorSrvice.Migrations
{
    [DbContext(typeof(DataProcessorDbContext))]
    partial class DataProcessorDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.16");

            modelBuilder.Entity("DataProcessorSrvice.Models.RapidControlStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ModuleCategoryID")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ModuleState")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("RapidControlStatus");
                });
#pragma warning restore 612, 618
        }
    }
}
