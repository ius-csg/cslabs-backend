﻿// <auto-generated />
using System;
using CSLabsBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CSLabsBackend.Migrations
{
    [DbContext(typeof(DefaultContext))]
    [Migration("20190910222306_AddModuleConstraints")]
    partial class AddModuleConstraints
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CSLabsBackend.Models.Module", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("UTC_TIMESTAMP()");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name");

                    b.Property<bool>("Published")
                        .HasColumnName("published");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasColumnName("short_name");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_modules");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("ix_modules_name");

                    b.HasIndex("ShortName")
                        .IsUnique()
                        .HasName("ix_modules_short_name");

                    b.ToTable("modules");
                });
#pragma warning restore 612, 618
        }
    }
}
