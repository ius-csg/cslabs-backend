﻿// <auto-generated />
using System;
using CSLabs.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CSLabs.Api.Migrations
{
    [DbContext(typeof(DefaultContext))]
    [Migration("20200418203352_AddOwnerIdToVmTemplate")]
    partial class AddOwnerIdToVmTemplate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CSLabs.Api.Models.Badge", b =>
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

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("UTC_TIMESTAMP()");

                    b.HasKey("Id")
                        .HasName("pk_badges");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("ix_badges_name");

                    b.ToTable("badges");
                });

            modelBuilder.Entity("CSLabs.Api.Models.ContactEmail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .HasColumnName("email")
                        .HasColumnType("VARCHAR(100)");

                    b.HasKey("Id")
                        .HasName("pk_contact_emails");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasName("ix_contact_emails_email");

                    b.ToTable("contact_emails");
                });

            modelBuilder.Entity("CSLabs.Api.Models.HypervisorModels.Hypervisor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Host")
                        .HasColumnName("host");

                    b.Property<string>("NoVncUrl")
                        .IsRequired()
                        .HasColumnName("no_vnc_url");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnName("password");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnName("user_name");

                    b.HasKey("Id")
                        .HasName("pk_hypervisors");

                    b.HasIndex("Host")
                        .IsUnique()
                        .HasName("ix_hypervisors_host");

                    b.HasIndex("NoVncUrl")
                        .IsUnique()
                        .HasName("ix_hypervisors_no_vnc_url");

                    b.ToTable("hypervisors");
                });

            modelBuilder.Entity("CSLabs.Api.Models.HypervisorModels.HypervisorNode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<int>("HypervisorId")
                        .HasColumnName("hypervisor_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_hypervisor_nodes");

                    b.HasIndex("HypervisorId")
                        .HasName("ix_hypervisor_nodes_hypervisor_id");

                    b.HasIndex("Name", "HypervisorId")
                        .IsUnique()
                        .HasName("ix_hypervisor_nodes_name_hypervisor_id");

                    b.ToTable("hypervisor_nodes");
                });

            modelBuilder.Entity("CSLabs.Api.Models.HypervisorModels.HypervisorVmTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<int>("HypervisorNodeId")
                        .HasColumnName("hypervisor_node_id");

                    b.Property<int>("TemplateVmId")
                        .HasColumnName("template_vm_id");

                    b.Property<int>("VmTemplateId")
                        .HasColumnName("vm_template_id");

                    b.HasKey("Id")
                        .HasName("pk_hypervisor_vm_templates");

                    b.HasIndex("HypervisorNodeId")
                        .HasName("ix_hypervisor_vm_templates_hypervisor_node_id");

                    b.HasIndex("VmTemplateId")
                        .HasName("ix_hypervisor_vm_templates_vm_template_id");

                    b.ToTable("hypervisor_vm_templates");
                });

            modelBuilder.Entity("CSLabs.Api.Models.HypervisorModels.VmTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<bool>("IsCoreRouter")
                        .HasColumnName("is_core_router");

                    b.Property<int?>("OwnerId")
                        .HasColumnName("owner_id");

                    b.HasKey("Id")
                        .HasName("pk_vm_template");

                    b.HasIndex("OwnerId")
                        .HasName("ix_vm_template_owner_id");

                    b.ToTable("vm_template");
                });

            modelBuilder.Entity("CSLabs.Api.Models.ModuleModels.BridgeTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<bool>("IsCoreBridge")
                        .HasColumnName("is_core_bridge");

                    b.Property<int>("LabId")
                        .HasColumnName("lab_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name");

                    b.Property<string>("Uuid")
                        .IsRequired()
                        .HasColumnName("uuid");

                    b.HasKey("Id")
                        .HasName("pk_bridge_templates");

                    b.HasIndex("LabId")
                        .HasName("ix_bridge_templates_lab_id");

                    b.HasIndex("LabId", "Name")
                        .IsUnique()
                        .HasName("ix_bridge_templates_lab_id_name");

                    b.HasIndex("LabId", "Uuid")
                        .IsUnique()
                        .HasName("ix_bridge_templates_lab_id_uuid");

                    b.ToTable("bridge_templates");
                });

            modelBuilder.Entity("CSLabs.Api.Models.ModuleModels.Lab", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("UTC_TIMESTAMP()");

                    b.Property<int>("EstimatedCpusUsed")
                        .HasColumnName("estimated_cpus_used");

                    b.Property<int>("EstimatedMemoryUsedMb")
                        .HasColumnName("estimated_memory_used_mb");

                    b.Property<int>("LabDifficulty")
                        .HasColumnName("lab_difficulty");

                    b.Property<int>("ModuleId")
                        .HasColumnName("module_id");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnName("type");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("UTC_TIMESTAMP()");

                    b.HasKey("Id")
                        .HasName("pk_labs");

                    b.HasIndex("ModuleId")
                        .HasName("ix_labs_module_id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("ix_labs_name");

                    b.ToTable("labs");
                });

            modelBuilder.Entity("CSLabs.Api.Models.ModuleModels.LabVm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("UTC_TIMESTAMP()");

                    b.Property<int>("LabId")
                        .HasColumnName("lab_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("VARCHAR(100)");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("UTC_TIMESTAMP()");

                    b.Property<int>("VmTemplateId")
                        .HasColumnName("vm_template_id");

                    b.HasKey("Id")
                        .HasName("pk_lab_vms");

                    b.HasIndex("LabId")
                        .HasName("ix_lab_vms_lab_id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("ix_lab_vms_name");

                    b.HasIndex("VmTemplateId")
                        .HasName("ix_lab_vms_vm_template_id");

                    b.ToTable("lab_vms");
                });

            modelBuilder.Entity("CSLabs.Api.Models.ModuleModels.Module", b =>
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

                    b.Property<int?>("OwnerId")
                        .HasColumnName("owner_id");

                    b.Property<bool>("Published")
                        .HasColumnName("published");

                    b.Property<string>("SpecialCode")
                        .HasColumnName("special_code");

                    b.Property<string>("Type")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnName("type")
                        .HasDefaultValue("SingleUser");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("UTC_TIMESTAMP()");

                    b.HasKey("Id")
                        .HasName("pk_modules");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("ix_modules_name");

                    b.HasIndex("OwnerId")
                        .HasName("ix_modules_owner_id");

                    b.HasIndex("SpecialCode")
                        .IsUnique()
                        .HasName("ix_modules_special_code");

                    b.ToTable("modules");
                });

            modelBuilder.Entity("CSLabs.Api.Models.ModuleModels.VmInterfaceTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<int>("BridgeTemplateId")
                        .HasColumnName("bridge_template_id");

                    b.Property<string>("BridgeTemplateUuid")
                        .IsRequired()
                        .HasColumnName("bridge_template_uuid");

                    b.Property<int>("InterfaceNumber")
                        .HasColumnName("interface_number");

                    b.Property<int>("LabVmId")
                        .HasColumnName("lab_vm_id");

                    b.HasKey("Id")
                        .HasName("pk_vm_interface_templates");

                    b.HasIndex("BridgeTemplateId")
                        .HasName("ix_vm_interface_templates_bridge_template_id");

                    b.HasIndex("LabVmId")
                        .HasName("ix_vm_interface_templates_lab_vm_id");

                    b.HasIndex("InterfaceNumber", "LabVmId")
                        .IsUnique()
                        .HasName("ix_vm_interface_templates_interface_number_lab_vm_id");

                    b.ToTable("vm_interface_templates");
                });

            modelBuilder.Entity("CSLabs.Api.Models.UserModels.BridgeInstance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<int>("BridgeTemplateId")
                        .HasColumnName("bridge_template_id");

                    b.Property<int>("HypervisorInterfaceId")
                        .HasColumnName("hypervisor_interface_id");

                    b.Property<int>("UserLabId")
                        .HasColumnName("user_lab_id");

                    b.Property<int>("UserLabVmId")
                        .HasColumnName("user_lab_vm_id");

                    b.HasKey("Id")
                        .HasName("pk_bridge_instances");

                    b.HasIndex("BridgeTemplateId")
                        .HasName("ix_bridge_instances_bridge_template_id");

                    b.HasIndex("UserLabId")
                        .HasName("ix_bridge_instances_user_lab_id");

                    b.HasIndex("UserLabVmId")
                        .HasName("ix_bridge_instances_user_lab_vm_id");

                    b.HasIndex("UserLabVmId", "HypervisorInterfaceId")
                        .IsUnique()
                        .HasName("ix_bridge_instances_user_lab_vm_id_hypervisor_interface_id");

                    b.ToTable("bridge_instances");
                });

            modelBuilder.Entity("CSLabs.Api.Models.UserModels.User", b =>
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

                    b.Property<string>("Email")
                        .HasColumnName("email")
                        .HasColumnType("VARCHAR(45)");

                    b.Property<string>("EmailVerificationCode")
                        .HasColumnName("email_verification_code")
                        .HasColumnType("VARCHAR(100)");

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

                    b.Property<string>("Password")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnName("password")
                        .HasDefaultValue(null);

                    b.Property<string>("PasswordRecoveryCode")
                        .HasColumnName("password_recovery_code")
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnName("role");

                    b.Property<DateTime?>("TerminationDate")
                        .HasColumnName("termination_date");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("UTC_TIMESTAMP()");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("CardCodeHash")
                        .IsUnique()
                        .HasName("ix_users_card_code_hash");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasName("ix_users_email");

                    b.HasIndex("PasswordRecoveryCode")
                        .IsUnique()
                        .HasName("ix_users_password_recovery_code");

                    b.ToTable("users");
                });

            modelBuilder.Entity("CSLabs.Api.Models.UserModels.UserLab", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("UTC_TIMESTAMP()");

                    b.Property<DateTime?>("EndDateTime")
                        .HasColumnName("end_date_time");

                    b.Property<int?>("HypervisorNodeId")
                        .HasColumnName("hypervisor_node_id");

                    b.Property<int>("LabId")
                        .HasColumnName("lab_id");

                    b.Property<DateTime?>("LastUsed")
                        .HasColumnName("last_used");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnName("status");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("UTC_TIMESTAMP()");

                    b.Property<int>("UserModuleId")
                        .HasColumnName("user_module_id");

                    b.HasKey("Id")
                        .HasName("pk_user_labs");

                    b.HasIndex("EndDateTime")
                        .HasName("ix_user_labs_end_date_time");

                    b.HasIndex("HypervisorNodeId")
                        .HasName("ix_user_labs_hypervisor_node_id");

                    b.HasIndex("LabId")
                        .HasName("ix_user_labs_lab_id");

                    b.HasIndex("UserModuleId")
                        .HasName("ix_user_labs_user_module_id");

                    b.HasIndex("UserModuleId", "LabId")
                        .IsUnique()
                        .HasName("ix_user_labs_user_module_id_lab_id");

                    b.ToTable("user_labs");
                });

            modelBuilder.Entity("CSLabs.Api.Models.UserModels.UserLabVm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("UTC_TIMESTAMP()");

                    b.Property<int>("HypervisorVmTemplateId")
                        .HasColumnName("hypervisor_vm_template_id");

                    b.Property<bool>("IsCoreRouter")
                        .HasColumnName("is_core_router");

                    b.Property<int>("LabVmId")
                        .HasColumnName("lab_vm_id");

                    b.Property<int>("ProxmoxVmId")
                        .HasColumnName("proxmox_vm_id");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("UTC_TIMESTAMP()");

                    b.Property<int>("UserLabId")
                        .HasColumnName("user_lab_id");

                    b.HasKey("Id")
                        .HasName("pk_user_lab_vms");

                    b.HasIndex("HypervisorVmTemplateId")
                        .HasName("ix_user_lab_vms_hypervisor_vm_template_id");

                    b.HasIndex("LabVmId")
                        .HasName("ix_user_lab_vms_lab_vm_id");

                    b.HasIndex("UserLabId")
                        .HasName("ix_user_lab_vms_user_lab_id");

                    b.HasIndex("UserLabId", "LabVmId")
                        .IsUnique()
                        .HasName("ix_user_lab_vms_user_lab_id_lab_vm_id");

                    b.ToTable("user_lab_vms");
                });

            modelBuilder.Entity("CSLabs.Api.Models.UserModels.UserModule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("UTC_TIMESTAMP()");

                    b.Property<int>("ModuleId")
                        .HasColumnName("module_id");

                    b.Property<DateTime>("TerminationDate")
                        .HasColumnName("termination_date");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("UTC_TIMESTAMP()");

                    b.HasKey("Id")
                        .HasName("pk_user_modules");

                    b.HasIndex("ModuleId")
                        .HasName("ix_user_modules_module_id");

                    b.ToTable("user_modules");
                });

            modelBuilder.Entity("CSLabs.Api.Models.UserModels.UserUserModule", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnName("user_id");

                    b.Property<int>("UserModuleId")
                        .HasColumnName("user_module_id");

                    b.HasKey("UserId", "UserModuleId")
                        .HasName("pk_user_user_module");

                    b.HasIndex("UserModuleId")
                        .HasName("ix_user_user_module_user_module_id");

                    b.ToTable("user_user_module");
                });

            modelBuilder.Entity("CSLabs.Api.Models.UserModels.VmInterfaceInstance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<int>("BridgeInstanceId")
                        .HasColumnName("bridge_instance_id");

                    b.Property<int>("UserLabVmId")
                        .HasColumnName("user_lab_vm_id");

                    b.Property<int>("VmInterfaceTemplateId")
                        .HasColumnName("vm_interface_template_id");

                    b.HasKey("Id")
                        .HasName("pk_vm_interface_instances");

                    b.HasIndex("BridgeInstanceId")
                        .HasName("ix_vm_interface_instances_bridge_instance_id");

                    b.HasIndex("UserLabVmId")
                        .HasName("ix_vm_interface_instances_user_lab_vm_id");

                    b.HasIndex("VmInterfaceTemplateId")
                        .HasName("ix_vm_interface_instances_vm_interface_template_id");

                    b.ToTable("vm_interface_instances");
                });

            modelBuilder.Entity("CSLabs.Api.Models.HypervisorModels.HypervisorNode", b =>
                {
                    b.HasOne("CSLabs.Api.Models.HypervisorModels.Hypervisor", "Hypervisor")
                        .WithMany("HypervisorNodes")
                        .HasForeignKey("HypervisorId")
                        .HasConstraintName("fk_hypervisor_nodes_hypervisors_hypervisor_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CSLabs.Api.Models.HypervisorModels.HypervisorVmTemplate", b =>
                {
                    b.HasOne("CSLabs.Api.Models.HypervisorModels.HypervisorNode", "HypervisorNode")
                        .WithMany()
                        .HasForeignKey("HypervisorNodeId")
                        .HasConstraintName("fk_hypervisor_vm_templates_hypervisor_nodes_hypervisor_node_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CSLabs.Api.Models.HypervisorModels.VmTemplate", "VmTemplate")
                        .WithMany("HypervisorVmTemplates")
                        .HasForeignKey("VmTemplateId")
                        .HasConstraintName("fk_hypervisor_vm_templates_vm_template_vm_template_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CSLabs.Api.Models.HypervisorModels.VmTemplate", b =>
                {
                    b.HasOne("CSLabs.Api.Models.UserModels.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .HasConstraintName("fk_vm_template_users_owner_id")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("CSLabs.Api.Models.ModuleModels.BridgeTemplate", b =>
                {
                    b.HasOne("CSLabs.Api.Models.ModuleModels.Lab", "Lab")
                        .WithMany("BridgeTemplates")
                        .HasForeignKey("LabId")
                        .HasConstraintName("fk_bridge_templates_labs_lab_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CSLabs.Api.Models.ModuleModels.Lab", b =>
                {
                    b.HasOne("CSLabs.Api.Models.ModuleModels.Module", "Module")
                        .WithMany("Labs")
                        .HasForeignKey("ModuleId")
                        .HasConstraintName("fk_labs_modules_module_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CSLabs.Api.Models.ModuleModels.LabVm", b =>
                {
                    b.HasOne("CSLabs.Api.Models.ModuleModels.Lab", "Lab")
                        .WithMany("LabVms")
                        .HasForeignKey("LabId")
                        .HasConstraintName("fk_lab_vms_labs_lab_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CSLabs.Api.Models.HypervisorModels.VmTemplate", "VmTemplate")
                        .WithMany()
                        .HasForeignKey("VmTemplateId")
                        .HasConstraintName("fk_lab_vms_vm_template_vm_template_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CSLabs.Api.Models.ModuleModels.Module", b =>
                {
                    b.HasOne("CSLabs.Api.Models.UserModels.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .HasConstraintName("fk_modules_users_owner_id");
                });

            modelBuilder.Entity("CSLabs.Api.Models.ModuleModels.VmInterfaceTemplate", b =>
                {
                    b.HasOne("CSLabs.Api.Models.ModuleModels.BridgeTemplate", "BridgeTemplate")
                        .WithMany()
                        .HasForeignKey("BridgeTemplateId")
                        .HasConstraintName("fk_vm_interface_templates_bridge_templates_bridge_template_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CSLabs.Api.Models.ModuleModels.LabVm", "LabVm")
                        .WithMany("TemplateInterfaces")
                        .HasForeignKey("LabVmId")
                        .HasConstraintName("fk_vm_interface_templates_lab_vms_lab_vm_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CSLabs.Api.Models.UserModels.BridgeInstance", b =>
                {
                    b.HasOne("CSLabs.Api.Models.ModuleModels.BridgeTemplate", "BridgeTemplate")
                        .WithMany()
                        .HasForeignKey("BridgeTemplateId")
                        .HasConstraintName("fk_bridge_instances_bridge_templates_bridge_template_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CSLabs.Api.Models.UserModels.UserLab", "UserLab")
                        .WithMany("BridgeInstances")
                        .HasForeignKey("UserLabId")
                        .HasConstraintName("fk_bridge_instances_user_labs_user_lab_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CSLabs.Api.Models.UserModels.UserLabVm", "UserLabVm")
                        .WithMany()
                        .HasForeignKey("UserLabVmId")
                        .HasConstraintName("fk_bridge_instances_user_lab_vms_user_lab_vm_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CSLabs.Api.Models.UserModels.UserLab", b =>
                {
                    b.HasOne("CSLabs.Api.Models.HypervisorModels.HypervisorNode", "HypervisorNode")
                        .WithMany("UserLabs")
                        .HasForeignKey("HypervisorNodeId")
                        .HasConstraintName("fk_user_labs_hypervisor_nodes_hypervisor_node_id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CSLabs.Api.Models.ModuleModels.Lab", "Lab")
                        .WithMany()
                        .HasForeignKey("LabId")
                        .HasConstraintName("fk_user_labs_labs_lab_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CSLabs.Api.Models.UserModels.UserModule", "UserModule")
                        .WithMany("UserLabs")
                        .HasForeignKey("UserModuleId")
                        .HasConstraintName("fk_user_labs_user_modules_user_module_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CSLabs.Api.Models.UserModels.UserLabVm", b =>
                {
                    b.HasOne("CSLabs.Api.Models.HypervisorModels.HypervisorVmTemplate", "HypervisorVmTemplate")
                        .WithMany()
                        .HasForeignKey("HypervisorVmTemplateId")
                        .HasConstraintName("fk_user_lab_vms_hypervisor_vm_templates_hypervisor_vm_template_~")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CSLabs.Api.Models.ModuleModels.LabVm", "LabVm")
                        .WithMany()
                        .HasForeignKey("LabVmId")
                        .HasConstraintName("fk_user_lab_vms_lab_vms_lab_vm_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CSLabs.Api.Models.UserModels.UserLab", "UserLab")
                        .WithMany("UserLabVms")
                        .HasForeignKey("UserLabId")
                        .HasConstraintName("fk_user_lab_vms_user_labs_user_lab_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CSLabs.Api.Models.UserModels.UserModule", b =>
                {
                    b.HasOne("CSLabs.Api.Models.ModuleModels.Module", "Module")
                        .WithMany()
                        .HasForeignKey("ModuleId")
                        .HasConstraintName("fk_user_modules_modules_module_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CSLabs.Api.Models.UserModels.UserUserModule", b =>
                {
                    b.HasOne("CSLabs.Api.Models.UserModels.User", "User")
                        .WithMany("UserUserModules")
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_user_user_module_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CSLabs.Api.Models.UserModels.UserModule", "UserModule")
                        .WithMany("UserUserModules")
                        .HasForeignKey("UserModuleId")
                        .HasConstraintName("fk_user_user_module_user_modules_user_module_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CSLabs.Api.Models.UserModels.VmInterfaceInstance", b =>
                {
                    b.HasOne("CSLabs.Api.Models.UserModels.BridgeInstance", "BridgeInstance")
                        .WithMany("InterfaceInstances")
                        .HasForeignKey("BridgeInstanceId")
                        .HasConstraintName("fk_vm_interface_instances_bridge_instances_bridge_instance_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CSLabs.Api.Models.UserModels.UserLabVm", "UserLabVm")
                        .WithMany("InterfaceInstances")
                        .HasForeignKey("UserLabVmId")
                        .HasConstraintName("fk_vm_interface_instances_user_lab_vms_user_lab_vm_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CSLabs.Api.Models.ModuleModels.VmInterfaceTemplate", "Template")
                        .WithMany()
                        .HasForeignKey("VmInterfaceTemplateId")
                        .HasConstraintName("fk_vm_interface_instances_vm_interface_templates_vm_interface_t~")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
