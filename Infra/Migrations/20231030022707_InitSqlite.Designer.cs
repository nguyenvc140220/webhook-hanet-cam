﻿// <auto-generated />
using System;
using Infra.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infra.Migrations
{
    [DbContext(typeof(SqliteDbContext))]
    [Migration("20231030022707_InitSqlite")]
    partial class InitSqlite
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("Infra.Models.CheckinFail", b =>
                {
                    b.Property<string>("id")
                        .HasColumnType("TEXT");

                    b.Property<string>("aliasID")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("date")
                        .HasColumnType("TEXT");

                    b.Property<string>("deviceName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("errorCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("status")
                        .HasColumnType("INTEGER");

                    b.HasKey("id");

                    b.ToTable("CheckinData");
                });
#pragma warning restore 612, 618
        }
    }
}