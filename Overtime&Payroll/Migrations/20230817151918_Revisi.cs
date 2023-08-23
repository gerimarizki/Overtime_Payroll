using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class Revisi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                table: "tb_m_employees");

            migrationBuilder.AlterColumn<string>(
                name: "last_name",
                table: "tb_m_employees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nchar(50)");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                table: "tb_m_employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "manager_guid",
                table: "tb_m_employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "modified_date",
                table: "tb_m_employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "tb_m_accounts",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    email = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    otp = table.Column<int>(type: "int", nullable: true),
                    is_used = table.Column<bool>(type: "bit", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    expired_time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_accounts", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_m_accounts_tb_m_employees_guid",
                        column: x => x.guid,
                        principalTable: "tb_m_employees",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_overtimes",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    overtime_Id = table.Column<string>(type: "nchar(8)", nullable: false),
                    start_overtime_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_overtime_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    remarks = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    paid = table.Column<double>(type: "float", nullable: false),
                    overtime_remaining = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    employee_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_overtimes", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_m_overtimes_tb_m_employees_employee_guid",
                        column: x => x.employee_guid,
                        principalTable: "tb_m_employees",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_roles",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_roles", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "tb_tr_payrolls",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    pay_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    allowance = table.Column<double>(type: "float", nullable: false),
                    salary = table.Column<double>(type: "float", nullable: false),
                    employee_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_tr_payrolls", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_tr_payrolls_tb_m_employees_employee_guid",
                        column: x => x.employee_guid,
                        principalTable: "tb_m_employees",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_tr_histories_overtimes",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    overtime_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_tr_histories_overtimes", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_tr_histories_overtimes_tb_m_overtimes_overtime_guid",
                        column: x => x.overtime_guid,
                        principalTable: "tb_m_overtimes",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_tr_account_roles",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    account_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    role_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                name: "IX_tb_m_employees_manager_guid",
                table: "tb_m_employees",
                column: "manager_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_employees_nik_phone_number",
                table: "tb_m_employees",
                columns: new[] { "nik", "phone_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_accounts_email",
                table: "tb_m_accounts",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_overtimes_employee_guid",
                table: "tb_m_overtimes",
                column: "employee_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_overtimes_overtime_Id",
                table: "tb_m_overtimes",
                column: "overtime_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_account_roles_account_guid",
                table: "tb_tr_account_roles",
                column: "account_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_account_roles_role_guid",
                table: "tb_tr_account_roles",
                column: "role_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_histories_overtimes_overtime_guid",
                table: "tb_tr_histories_overtimes",
                column: "overtime_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_payrolls_employee_guid",
                table: "tb_tr_payrolls",
                column: "employee_guid",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_m_employees_tb_m_employees_manager_guid",
                table: "tb_m_employees",
                column: "manager_guid",
                principalTable: "tb_m_employees",
                principalColumn: "guid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_m_employees_tb_m_employees_manager_guid",
                table: "tb_m_employees");

            migrationBuilder.DropTable(
                name: "tb_tr_account_roles");

            migrationBuilder.DropTable(
                name: "tb_tr_histories_overtimes");

            migrationBuilder.DropTable(
                name: "tb_tr_payrolls");

            migrationBuilder.DropTable(
                name: "tb_m_accounts");

            migrationBuilder.DropTable(
                name: "tb_m_roles");

            migrationBuilder.DropTable(
                name: "tb_m_overtimes");

            migrationBuilder.DropIndex(
                name: "IX_tb_m_employees_manager_guid",
                table: "tb_m_employees");

            migrationBuilder.DropIndex(
                name: "IX_tb_m_employees_nik_phone_number",
                table: "tb_m_employees");

            migrationBuilder.DropColumn(
                name: "created_date",
                table: "tb_m_employees");

            migrationBuilder.DropColumn(
                name: "manager_guid",
                table: "tb_m_employees");

            migrationBuilder.DropColumn(
                name: "modified_date",
                table: "tb_m_employees");

            migrationBuilder.AlterColumn<string>(
                name: "last_name",
                table: "tb_m_employees",
                type: "nchar(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "tb_m_employees",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");
        }
    }
}
