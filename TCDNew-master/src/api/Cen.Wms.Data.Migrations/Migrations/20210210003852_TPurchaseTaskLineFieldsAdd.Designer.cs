﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Cen.Wms.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Cen.Wms.Data.Migrations
{
    [DbContext(typeof(WmsContext))]
    [Migration("20210210003852_TPurchaseTaskLineFieldsAdd")]
    partial class TPurchaseTaskLineFieldsAdd
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.HasSequence("purchase_task_code_seq");

            modelBuilder.Entity("Cen.Wms.Data.Models.Device.DeviceRegistrationRequestRow", b =>
                {
                    b.Property<Guid?>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Instant>("CreatedAt")
                        .HasColumnType("timestamp")
                        .HasColumnName("created_at");

                    b.Property<string>("DevicePublicKey")
                        .HasColumnType("text")
                        .HasColumnName("device_public_key");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("p_k_device_registration_request");

                    b.HasIndex("DevicePublicKey")
                        .IsUnique();

                    b.HasIndex("DevicePublicKey", "CreatedAt")
                        .IsUnique();

                    b.ToTable("device_registration_request");
                });

            modelBuilder.Entity("Cen.Wms.Data.Models.Device.DeviceRegistrationRequestStateRow", b =>
                {
                    b.Property<Guid?>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<bool>("IsAccepted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_accepted");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("p_k_device_registration_request_state");

                    b.ToTable("device_registration_request_state");
                });

            modelBuilder.Entity("Cen.Wms.Data.Models.Device.DeviceRow", b =>
                {
                    b.Property<Guid?>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("DevicePublicKey")
                        .HasColumnType("text")
                        .HasColumnName("device_public_key");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("p_k_device");

                    b.HasIndex("DevicePublicKey")
                        .IsUnique();

                    b.ToTable("device");
                });

            modelBuilder.Entity("Cen.Wms.Data.Models.Device.DeviceStateRow", b =>
                {
                    b.Property<Guid?>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("DevicePublicKey")
                        .HasColumnType("text")
                        .HasColumnName("device_public_key");

                    b.Property<int>("DeviceStatus")
                        .HasColumnType("integer")
                        .HasColumnName("device_status");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("p_k_device_state");

                    b.HasIndex("DevicePublicKey")
                        .IsUnique();

                    b.ToTable("device_state");
                });

            modelBuilder.Entity("Cen.Wms.Data.Models.Facility.FacilityAccessRow", b =>
                {
                    b.Property<Guid?>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("FacilityId")
                        .HasColumnType("uuid")
                        .HasColumnName("facility_id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("p_k_facility_access");

                    b.ToTable("facility_access");
                });

            modelBuilder.Entity("Cen.Wms.Data.Models.Facility.FacilityConfigRow", b =>
                {
                    b.Property<Guid?>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("AcceptanceProcessType")
                        .HasColumnType("integer")
                        .HasColumnName("acceptance_process_type");

                    b.Property<string>("PalletCodePrefix")
                        .HasColumnType("text")
                        .HasColumnName("pallet_code_prefix");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("p_k_facility_config");

                    b.ToTable("facility_config");
                });

            modelBuilder.Entity("Cen.Wms.Data.Models.Facility.FacilityRow", b =>
                {
                    b.Property<Guid?>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Instant>("ChangedAt")
                        .HasColumnType("timestamp")
                        .HasColumnName("changed_at");

                    b.Property<string>("Code")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)")
                        .HasColumnName("code");

                    b.Property<string>("ExtId")
                        .HasColumnType("text")
                        .HasColumnName("ext_id");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("p_k_facility");

                    b.ToTable("facility");
                });

            modelBuilder.Entity("Cen.Wms.Data.Models.Purchase.PacHeadRow", b =>
                {
                    b.Property<Guid?>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Instant>("ChangedAt")
                        .HasColumnType("timestamp")
                        .HasColumnName("changed_at");

                    b.Property<string>("ExtId")
                        .HasColumnType("text")
                        .HasColumnName("ext_id");

                    b.Property<string>("FacilityId")
                        .HasColumnType("text")
                        .HasColumnName("facility_id");

                    b.Property<Instant>("PacDateTime")
                        .HasColumnType("timestamp")
                        .HasColumnName("pac_date_time");

                    b.Property<LocalDate>("PurchaseBookingDate")
                        .HasColumnType("date")
                        .HasColumnName("purchase_booking_date");

                    b.Property<string>("PurchaseBookingId")
                        .HasColumnType("text")
                        .HasColumnName("purchase_booking_id");

                    b.Property<LocalDate>("PurchaseDate")
                        .HasColumnType("date")
                        .HasColumnName("purchase_date");

                    b.Property<string>("PurchaseId")
                        .HasColumnType("text")
                        .HasColumnName("purchase_id");

                    b.Property<string>("SupplierId")
                        .HasColumnType("text")
                        .HasColumnName("supplier_id");

                    b.Property<string>("SupplierName")
                        .HasColumnType("text")
                        .HasColumnName("supplier_name");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("p_k_pac_head");

                    b.ToTable("pac_head");
                });

            modelBuilder.Entity("Cen.Wms.Data.Models.Purchase.PacLineRow", b =>
                {
                    b.Property<Guid?>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Instant>("ChangedAt")
                        .HasColumnType("timestamp")
                        .HasColumnName("changed_at");

                    b.Property<string>("ExtId")
                        .HasColumnType("text")
                        .HasColumnName("ext_id");

                    b.Property<int>("LineNum")
                        .HasColumnType("integer")
                        .HasColumnName("line_num");

                    b.Property<Guid>("PacHeadId")
                        .HasColumnType("uuid")
                        .HasColumnName("pac_head_id");

                    b.Property<string>("ProductBarcodeMain")
                        .HasColumnType("text")
                        .HasColumnName("product_barcode_main");

                    b.Property<List<string>>("ProductBarcodes")
                        .HasColumnType("text[]")
                        .HasColumnName("product_barcodes");

                    b.Property<string>("ProductId")
                        .HasColumnType("text")
                        .HasColumnName("product_id");

                    b.Property<string>("ProductName")
                        .HasColumnType("text")
                        .HasColumnName("product_name");

                    b.Property<string>("ProductUnitOfMeasure")
                        .HasColumnType("text")
                        .HasColumnName("product_unit_of_measure");

                    b.Property<decimal>("QtyExpected")
                        .HasColumnType("numeric")
                        .HasColumnName("qty_expected");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("p_k_pac_line");

                    b.HasIndex("PacHeadId")
                        .HasDatabaseName("i_x_pac_line_pac_head_id");

                    b.ToTable("pac_line");
                });

            modelBuilder.Entity("Cen.Wms.Data.Models.Purchase.PurchaseTaskHeadRow", b =>
                {
                    b.Property<Guid?>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Instant>("ChangedAt")
                        .HasColumnType("timestamp")
                        .HasColumnName("changed_at");

                    b.Property<string>("Code")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)")
                        .HasColumnName("code");

                    b.Property<Instant>("CreatedAt")
                        .HasColumnType("timestamp")
                        .HasColumnName("created_at");

                    b.Property<string>("ExtId")
                        .HasColumnType("text")
                        .HasColumnName("ext_id");

                    b.Property<Guid>("FacilityId")
                        .HasColumnType("uuid")
                        .HasColumnName("facility_id");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("p_k_purchase_task_head");

                    b.HasIndex("FacilityId")
                        .HasDatabaseName("i_x_purchase_task_head_facility_id");

                    b.ToTable("purchase_task_head");
                });

            modelBuilder.Entity("Cen.Wms.Data.Models.Purchase.PurchaseTaskLineRow", b =>
                {
                    b.Property<Guid?>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("ItemAbc")
                        .HasColumnType("text")
                        .HasColumnName("item_abc");

                    b.Property<string[]>("ItemBarcodes")
                        .HasColumnType("text[]")
                        .HasColumnName("item_barcodes");

                    b.Property<string>("ItemExtId")
                        .HasColumnType("text")
                        .HasColumnName("item_ext_id");

                    b.Property<string>("ItemName")
                        .HasColumnType("text")
                        .HasColumnName("item_name");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<Guid>("PurchaseTaskHeadId")
                        .HasColumnType("uuid")
                        .HasColumnName("purchase_task_head_id");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("numeric")
                        .HasColumnName("quantity");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("p_k_purchase_task_line");

                    b.HasIndex("PurchaseTaskHeadId")
                        .HasDatabaseName("i_x_purchase_task_line_purchase_task_head_id");

                    b.ToTable("purchase_task_line");
                });

            modelBuilder.Entity("Cen.Wms.Data.Models.Purchase.PurchaseTaskLineStateRow", b =>
                {
                    b.Property<Guid?>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Instant?>("ExpirationDate")
                        .HasColumnType("timestamp")
                        .HasColumnName("expiration_date");

                    b.Property<long>("ExpirationDaysPlus")
                        .HasColumnType("bigint")
                        .HasColumnName("expiration_days_plus");

                    b.Property<Guid>("PurchaseTaskLineId")
                        .HasColumnType("uuid")
                        .HasColumnName("purchase_task_line_id");

                    b.Property<decimal>("QtyBroken")
                        .HasColumnType("numeric")
                        .HasColumnName("qty_broken");

                    b.Property<decimal>("QtyNormal")
                        .HasColumnType("numeric")
                        .HasColumnName("qty_normal");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("p_k_purchase_task_line_state");

                    b.HasIndex("PurchaseTaskLineId")
                        .IsUnique()
                        .HasDatabaseName("i_x_purchase_task_line_state_purchase_task_line_id");

                    b.ToTable("purchase_task_line_state");
                });

            modelBuilder.Entity("Cen.Wms.Data.Models.Purchase.PurchaseTaskPalletRow", b =>
                {
                    b.Property<Guid?>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Abc")
                        .HasColumnType("text")
                        .HasColumnName("abc");

                    b.Property<string>("Code")
                        .HasColumnType("text")
                        .HasColumnName("code");

                    b.Property<Guid>("PurchaseTaskHeadId")
                        .HasColumnType("uuid")
                        .HasColumnName("purchase_task_head_id");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("p_k_purchase_task_pallet");

                    b.HasIndex("PurchaseTaskHeadId")
                        .HasDatabaseName("i_x_purchase_task_pallet_purchase_task_head_id");

                    b.ToTable("purchase_task_pallet");
                });

            modelBuilder.Entity("Cen.Wms.Data.Models.Sync.SyncPositionRow", b =>
                {
                    b.Property<Guid?>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Code")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)")
                        .HasColumnName("code");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.Property<long>("Position")
                        .HasColumnType("bigint")
                        .HasColumnName("position");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("p_k_sync_position");

                    b.ToTable("sync_position");
                });

            modelBuilder.Entity("Cen.Wms.Data.Models.User.UserConfigRow", b =>
                {
                    b.Property<Guid?>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("DefaultFacilityId")
                        .HasColumnType("uuid")
                        .HasColumnName("default_facility_id");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("p_k_user_config");

                    b.ToTable("user_config");
                });

            modelBuilder.Entity("Cen.Wms.Data.Models.User.UserRow", b =>
                {
                    b.Property<Guid?>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Instant>("ChangedAt")
                        .HasColumnType("timestamp")
                        .HasColumnName("changed_at");

                    b.Property<string>("Code")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)")
                        .HasColumnName("code");

                    b.Property<string>("DepartmentExtId")
                        .HasColumnType("text")
                        .HasColumnName("department_ext_id");

                    b.Property<string>("DepartmentName")
                        .HasColumnType("text")
                        .HasColumnName("department_name");

                    b.Property<string>("ExtId")
                        .HasColumnType("text")
                        .HasColumnName("ext_id");

                    b.Property<string>("FacilityExtId")
                        .HasColumnType("text")
                        .HasColumnName("facility_ext_id");

                    b.Property<string>("FacilityName")
                        .HasColumnType("text")
                        .HasColumnName("facility_name");

                    b.Property<bool>("IsLockedExt")
                        .HasColumnType("boolean")
                        .HasColumnName("is_locked_ext");

                    b.Property<string>("Login")
                        .HasColumnType("text")
                        .HasColumnName("login");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.Property<string>("PositionExtId")
                        .HasColumnType("text")
                        .HasColumnName("position_ext_id");

                    b.Property<string>("PositionName")
                        .HasColumnType("text")
                        .HasColumnName("position_name");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("p_k_user");

                    b.ToTable("user");
                });

            modelBuilder.Entity("Cen.Wms.Data.Models.User.UserStateRow", b =>
                {
                    b.Property<Guid?>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("boolean")
                        .HasColumnName("is_locked");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("p_k_user_state");

                    b.ToTable("user_state");
                });

            modelBuilder.Entity("Cen.Wms.Data.Models.Purchase.PacLineRow", b =>
                {
                    b.HasOne("Cen.Wms.Data.Models.Purchase.PacHeadRow", "PacHead")
                        .WithMany("Lines")
                        .HasForeignKey("PacHeadId")
                        .HasConstraintName("f_k_pac_line_pac_head_pac_head_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PacHead");
                });

            modelBuilder.Entity("Cen.Wms.Data.Models.Purchase.PurchaseTaskHeadRow", b =>
                {
                    b.HasOne("Cen.Wms.Data.Models.Facility.FacilityRow", "Facility")
                        .WithMany()
                        .HasForeignKey("FacilityId")
                        .HasConstraintName("f_k_purchase_task_head_facility_facility_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Facility");
                });

            modelBuilder.Entity("Cen.Wms.Data.Models.Purchase.PurchaseTaskLineRow", b =>
                {
                    b.HasOne("Cen.Wms.Data.Models.Purchase.PurchaseTaskHeadRow", "PurchaseTaskHead")
                        .WithMany("Lines")
                        .HasForeignKey("PurchaseTaskHeadId")
                        .HasConstraintName("f_k_purchase_task_line_purchase_task_head_purchase_task_head_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PurchaseTaskHead");
                });

            modelBuilder.Entity("Cen.Wms.Data.Models.Purchase.PurchaseTaskLineStateRow", b =>
                {
                    b.HasOne("Cen.Wms.Data.Models.Purchase.PurchaseTaskLineRow", "PurchaseTaskLine")
                        .WithOne("PurchaseTaskLineState")
                        .HasForeignKey("Cen.Wms.Data.Models.Purchase.PurchaseTaskLineStateRow", "PurchaseTaskLineId")
                        .HasConstraintName("f_k_purchase_task_line_state_purchase_task_line_purchase_task_l~")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PurchaseTaskLine");
                });

            modelBuilder.Entity("Cen.Wms.Data.Models.Purchase.PurchaseTaskPalletRow", b =>
                {
                    b.HasOne("Cen.Wms.Data.Models.Purchase.PurchaseTaskHeadRow", "PurchaseTaskHead")
                        .WithMany("Pallets")
                        .HasForeignKey("PurchaseTaskHeadId")
                        .HasConstraintName("f_k_purchase_task_pallet_purchase_task_head_purchase_task_head_~")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PurchaseTaskHead");
                });

            modelBuilder.Entity("Cen.Wms.Data.Models.Purchase.PacHeadRow", b =>
                {
                    b.Navigation("Lines");
                });

            modelBuilder.Entity("Cen.Wms.Data.Models.Purchase.PurchaseTaskHeadRow", b =>
                {
                    b.Navigation("Lines");

                    b.Navigation("Pallets");
                });

            modelBuilder.Entity("Cen.Wms.Data.Models.Purchase.PurchaseTaskLineRow", b =>
                {
                    b.Navigation("PurchaseTaskLineState");
                });
#pragma warning restore 612, 618
        }
    }
}
