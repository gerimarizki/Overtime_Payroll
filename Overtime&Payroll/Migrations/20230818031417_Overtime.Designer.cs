﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Overtime_Payroll.Data;

#nullable disable

namespace Overtime_Payroll.Migrations
{
    [DbContext(typeof(OvertimeDbContext))]
    [Migration("20230818031417_Overtime")]
    partial class Overtime
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Overtime_Payroll.Models.Account", b =>
                {
                    b.Property<Guid>("Guid")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("guid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("email");

                    b.Property<DateTime?>("ExpiredTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("expired_time");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit")
                        .HasColumnName("is_active");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit")
                        .HasColumnName("is_used");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("modified_date");

                    b.Property<int?>("OTP")
                        .HasColumnType("int")
                        .HasColumnName("otp");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasColumnName("password");

                    b.HasKey("Guid");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("tb_m_accounts");
                });

            modelBuilder.Entity("Overtime_Payroll.Models.AccountRole", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("guid");

                    b.Property<Guid>("AccountGuid")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("account_guid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_date");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("modified_date");

                    b.Property<Guid>("RoleGuid")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("role_guid");

                    b.HasKey("Guid");

                    b.HasIndex("AccountGuid");

                    b.HasIndex("RoleGuid");

                    b.ToTable("tb_tr_account_roles");
                });

            modelBuilder.Entity("Overtime_Payroll.Models.Employee", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("guid");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("birth_date");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_date");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("first_name");

                    b.Property<int>("Gender")
                        .HasColumnType("int")
                        .HasColumnName("gender");

                    b.Property<DateTime>("HiringDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("hiring_date");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("last_name");

                    b.Property<Guid?>("ManagerGuid")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("manager_guid");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("modified_date");

                    b.Property<string>("NIK")
                        .IsRequired()
                        .HasColumnType("nchar(6)")
                        .HasColumnName("nik");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("phone_number");

                    b.HasKey("Guid");

                    b.HasIndex("ManagerGuid");

                    b.HasIndex("NIK", "PhoneNumber")
                        .IsUnique();

                    b.ToTable("tb_m_employees");
                });

            modelBuilder.Entity("Overtime_Payroll.Models.HistoryOvertime", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("guid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_date");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("modified_date");

                    b.Property<Guid>("OvertimeGuid")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("overtime_guid");

                    b.HasKey("Guid");

                    b.HasIndex("OvertimeGuid");

                    b.ToTable("tb_tr_histories_overtimes");
                });

            modelBuilder.Entity("Overtime_Payroll.Models.Overtime", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("guid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_date");

                    b.Property<Guid>("EmployeeGuid")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("employee_guid");

                    b.Property<DateTime>("EndOvertimeDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("end_overtime_date");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("modified_date");

                    b.Property<string>("OvertimeId")
                        .IsRequired()
                        .HasColumnType("nchar(8)")
                        .HasColumnName("overtime_Id");

                    b.Property<int>("OvertimeRemaining")
                        .HasColumnType("int")
                        .HasColumnName("overtime_remaining");

                    b.Property<double>("PaidOvertime")
                        .HasColumnType("float")
                        .HasColumnName("paid");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("remarks");

                    b.Property<DateTime>("StartOvertimeDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("start_overtime_date");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasColumnName("status");

                    b.HasKey("Guid");

                    b.HasIndex("EmployeeGuid");

                    b.HasIndex("OvertimeId")
                        .IsUnique();

                    b.ToTable("tb_m_overtimes");
                });

            modelBuilder.Entity("Overtime_Payroll.Models.Payroll", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("guid");

                    b.Property<double>("Allowance")
                        .HasColumnType("float")
                        .HasColumnName("allowance");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_date");

                    b.Property<Guid>("EmployeeGuid")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("employee_guid");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("modified_date");

                    b.Property<DateTime>("PayDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("pay_date");

                    b.Property<double>("Salary")
                        .HasColumnType("float")
                        .HasColumnName("salary");

                    b.HasKey("Guid");

                    b.HasIndex("EmployeeGuid")
                        .IsUnique();

                    b.ToTable("tb_tr_payrolls");
                });

            modelBuilder.Entity("Overtime_Payroll.Models.Role", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("guid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_date");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("modified_date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.HasKey("Guid");

                    b.ToTable("tb_m_roles");
                });

            modelBuilder.Entity("Overtime_Payroll.Models.Account", b =>
                {
                    b.HasOne("Overtime_Payroll.Models.Employee", "Employee")
                        .WithOne("Account")
                        .HasForeignKey("Overtime_Payroll.Models.Account", "Guid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Overtime_Payroll.Models.AccountRole", b =>
                {
                    b.HasOne("Overtime_Payroll.Models.Account", "Account")
                        .WithMany("AccountRoles")
                        .HasForeignKey("AccountGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Overtime_Payroll.Models.Role", "Role")
                        .WithMany("AccountRoles")
                        .HasForeignKey("RoleGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Overtime_Payroll.Models.Employee", b =>
                {
                    b.HasOne("Overtime_Payroll.Models.Employee", "Manager")
                        .WithMany("Employees")
                        .HasForeignKey("ManagerGuid")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("Overtime_Payroll.Models.HistoryOvertime", b =>
                {
                    b.HasOne("Overtime_Payroll.Models.Overtime", "Overtime")
                        .WithMany("Histories")
                        .HasForeignKey("OvertimeGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Overtime");
                });

            modelBuilder.Entity("Overtime_Payroll.Models.Overtime", b =>
                {
                    b.HasOne("Overtime_Payroll.Models.Employee", "Employee")
                        .WithMany("Overtimes")
                        .HasForeignKey("EmployeeGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Overtime_Payroll.Models.Payroll", b =>
                {
                    b.HasOne("Overtime_Payroll.Models.Employee", "Employee")
                        .WithOne("Payroll")
                        .HasForeignKey("Overtime_Payroll.Models.Payroll", "EmployeeGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Overtime_Payroll.Models.Account", b =>
                {
                    b.Navigation("AccountRoles");
                });

            modelBuilder.Entity("Overtime_Payroll.Models.Employee", b =>
                {
                    b.Navigation("Account");

                    b.Navigation("Employees");

                    b.Navigation("Overtimes");

                    b.Navigation("Payroll");
                });

            modelBuilder.Entity("Overtime_Payroll.Models.Overtime", b =>
                {
                    b.Navigation("Histories");
                });

            modelBuilder.Entity("Overtime_Payroll.Models.Role", b =>
                {
                    b.Navigation("AccountRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
