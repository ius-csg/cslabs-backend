﻿// <auto-generated />
using System;
using CSLabsBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CSLabsBackend.Migrations
{
    [DbContext(typeof(DefaultContext))]
    partial class DefaultContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CSLabsBackend.Models.Badge", b =>
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
                        .HasColumnName("description")
                        .HasColumnType("TEXT");

                    b.Property<string>("IconPath")
                        .IsRequired()
                        .HasColumnName("icon_path")
                        .HasColumnType("VARCHAR(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name");

                    b.Property<int>("RequiredNumOfModules")
                        .HasColumnName("required_num_of_modules");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasColumnName("short_name");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("UTC_TIMESTAMP()");

                    b.HasKey("Id")
                        .HasName("pk_badges");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("ix_badges_name");

                    b.HasIndex("ShortName")
                        .IsUnique()
                        .HasName("ix_badges_short_name");

                    b.ToTable("badges");
                });

            modelBuilder.Entity("CSLabsBackend.Models.Lab", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("UTC_TIMESTAMP()");

                    b.Property<int>("CreatorId")
                        .HasColumnName("creator_id");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnName("deleted_at");

                    b.Property<int>("EstimatedCpusUsed")
                        .HasColumnName("estimated_cpus_used");

                    b.Property<int>("EstimatedMemoryUsedMb")
                        .HasColumnName("estimated_memory_used_mb");

                    b.Property<int>("LabDifficulty")
                        .HasColumnName("lab_difficulty");

                    b.Property<int>("LabType")
                        .HasColumnName("lab_type");

                    b.Property<int>("ModuleId")
                        .HasColumnName("module_id");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("RundeckCreateUrl")
                        .HasColumnName("rundeck_create_url")
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("RundeckDestroyUrl")
                        .HasColumnName("rundeck_destroy_url")
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("RundeckRestoreSnapshotUrl")
                        .HasColumnName("rundeck_restore_snapshot_url")
                        .HasColumnType("VARCHAR(45)");

                    b.Property<string>("RundeckSnapshotUrl")
                        .HasColumnName("rundeck_snapshot_url")
                        .HasColumnType("VARCHAR(45)");

                    b.Property<string>("RundeckStartUrl")
                        .HasColumnName("rundeck_start_url")
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("RundeckStopUrl")
                        .HasColumnName("rundeck_stop_url")
                        .HasColumnType("VARCHAR(100)");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("UTC_TIMESTAMP()");

                    b.HasKey("Id")
                        .HasName("pk_labs");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("ix_labs_name");

                    b.ToTable("labs");
                });

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
                        .ValueGeneratedOnAdd()
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("UTC_TIMESTAMP()");

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

            modelBuilder.Entity("CSLabsBackend.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("CardCodeHash")
                        .HasColumnName("card_code_hash")
                        .HasColumnType("VARCHAR(100)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("UTC_TIMESTAMP()");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnName("first_name")
                        .HasColumnType("VARCHAR(100)");

                    b.Property<int?>("GraduationYear")
                        .HasColumnName("graduation_year");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnName("last_name")
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("MiddleName")
                        .HasColumnName("middle_name")
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("PersonalEmail")
                        .HasColumnName("personal_email")
                        .HasColumnType("VARCHAR(45)");

                    b.Property<string>("SchoolEmail")
                        .HasColumnName("school_email")
                        .HasColumnType("VARCHAR(100)");

                    b.Property<DateTime?>("TerminationDate")
                        .HasColumnName("termination_date");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("UTC_TIMESTAMP()");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasColumnName("user_type")
                        .HasColumnType("VARCHAR(45)");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("CardCodeHash")
                        .IsUnique()
                        .HasName("ix_users_card_code_hash");

                    b.HasIndex("PersonalEmail")
                        .IsUnique()
                        .HasName("ix_users_personal_email");

                    b.HasIndex("SchoolEmail")
                        .IsUnique()
                        .HasName("ix_users_school_email");

                    b.ToTable("users");
                });
#pragma warning restore 612, 618
        }
    }
}
