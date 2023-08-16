using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Overtime_Payroll.Migrations
{
    public partial class Models : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "last_name",
                table: "tb_m_employees",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "gender",
                table: "tb_m_employees",
                type: "char(1)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "first_name",
                table: "tb_m_employees",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "tb_m_employees",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AddColumn<Guid>(
                name: "department_id",
                table: "tb_m_employees",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "employee_level_id",
                table: "tb_m_employees",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "overtime_id",
                table: "tb_m_employees",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "report_to",
                table: "tb_m_employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tb_m_accounts",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    password = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    otp = table.Column<int>(type: "int", nullable: false),
                    is_used = table.Column<bool>(type: "bit", nullable: true),
                    expired_time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    employee_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_accounts", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_m_accounts_tb_m_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tb_m_employees",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_overtimes",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    startOvertime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endOvertime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    submitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    deskripsi = table.Column<string>(type: "varchar(100)", nullable: false),
                    Paid = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    employee_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_overtimes", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_m_overtimes_tb_m_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tb_m_employees",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_roles",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_roles", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "tb_tr_departments",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_tr_departments", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "tb_tr_employee_levels",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    level = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    salary = table.Column<int>(type: "int", nullable: false),
                    allowance = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_tr_employee_levels", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "tb_tr_payrolls",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    pay_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    payroll_cut = table.Column<int>(type: "int", nullable: false),
                    total_salary = table.Column<int>(type: "int", nullable: false),
                    employee_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_tr_payrolls", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_tr_payrolls_tb_m_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tb_m_employees",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_tr_account_roles",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    account_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    role_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_tr_account_roles", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_tr_account_roles_tb_m_accounts_account_guid",
                        column: x => x.account_guid,
                        principalTable: "tb_m_accounts",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_tr_account_roles_tb_m_roles_role_guid",
                        column: x => x.role_guid,
                        principalTable: "tb_m_roles",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_employees_department_id",
                table: "tb_m_employees",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_employees_employee_level_id",
                table: "tb_m_employees",
                column: "employee_level_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_employees_nik_email_phone_number",
                table: "tb_m_employees",
                columns: new[] { "nik", "email", "phone_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_accounts_employee_id",
                table: "tb_m_accounts",
                column: "employee_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_overtimes_employee_id",
                table: "tb_m_overtimes",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_account_roles_account_guid",
                table: "tb_tr_account_roles",
                column: "account_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_account_roles_role_guid",
                table: "tb_tr_account_roles",
                column: "role_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_payrolls_employee_id",
                table: "tb_tr_payrolls",
                column: "employee_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_m_employees_tb_tr_departments_department_id",
                table: "tb_m_employees",
                column: "department_id",
                principalTable: "tb_tr_departments",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_m_employees_tb_tr_employee_levels_employee_level_id",
                table: "tb_m_employees",
                column: "employee_level_id",
                principalTable: "tb_tr_employee_levels",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_m_employees_tb_tr_departments_department_id",
                table: "tb_m_employees");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_m_employees_tb_tr_employee_levels_employee_level_id",
                table: "tb_m_employees");

            migrationBuilder.DropTable(
                name: "tb_m_overtimes");

            migrationBuilder.DropTable(
                name: "tb_tr_account_roles");

            migrationBuilder.DropTable(
                name: "tb_tr_departments");

            migrationBuilder.DropTable(
                name: "tb_tr_employee_levels");

            migrationBuilder.DropTable(
                name: "tb_tr_payrolls");

            migrationBuilder.DropTable(
                name: "tb_m_accounts");

            migrationBuilder.DropTable(
                name: "tb_m_roles");

            migrationBuilder.DropIndex(
                name: "IX_tb_m_employees_department_id",
                table: "tb_m_employees");

            migrationBuilder.DropIndex(
                name: "IX_tb_m_employees_employee_level_id",
                table: "tb_m_employees");

            migrationBuilder.DropIndex(
                name: "IX_tb_m_employees_nik_email_phone_number",
                table: "tb_m_employees");

            migrationBuilder.DropColumn(
                name: "department_id",
                table: "tb_m_employees");

            migrationBuilder.DropColumn(
                name: "employee_level_id",
                table: "tb_m_employees");

            migrationBuilder.DropColumn(
                name: "overtime_id",
                table: "tb_m_employees");

            migrationBuilder.DropColumn(
                name: "report_to",
                table: "tb_m_employees");

            migrationBuilder.AlterColumn<string>(
                name: "last_name",
                table: "tb_m_employees",
                type: "nchar(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "gender",
                table: "tb_m_employees",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(1)");

            migrationBuilder.AlterColumn<string>(
                name: "first_name",
                table: "tb_m_employees",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "tb_m_employees",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");
        }
    }
}
