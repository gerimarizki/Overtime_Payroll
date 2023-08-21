using Microsoft.EntityFrameworkCore;
using Overtime_Payroll.DTOs.Roles;
using Overtime_Payroll.Models;
using System.Reflection.Emit;
using System.Security.Principal;

namespace Overtime_Payroll.Data
{
    public class OvertimeDbContext : DbContext
    {
        public OvertimeDbContext(DbContextOptions<OvertimeDbContext> options) : base(options)
        {

        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<HistoryOvertime> Histories { get; set; }
        public DbSet<Overtime> Overtimes { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<Role> Roles { get; set; }

        //Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>().HasData(
            new NewRoleDefaultDto
            {
                Guid = Guid.Parse("d45a2669-59da-4998-69f2-08db92586f59"),
                Name = "Employee"
            });

            //Unique
            modelBuilder.Entity<Employee>()
                        .HasIndex(e => new
                        {
                            e.NIK,
                            e.PhoneNumber
                        }).IsUnique();

            modelBuilder.Entity<Employee>()
                .HasKey(e => e.Guid);

            modelBuilder.Entity<Account>()
                        .HasIndex(a => new
                        {
                            a.Email,
                        }).IsUnique();

            modelBuilder.Entity<Overtime>()
                        .HasIndex(o => new
                        {
                            o.OvertimeId,
                        }).IsUnique();

            // All Relationship
               
            // Emp - Acc
            modelBuilder.Entity<Account>().HasOne(account => account.Employee).WithOne(employee => employee.Account)
                        .HasForeignKey<Account>(account => account.Guid);

            // Acc - AccRole 
            modelBuilder.Entity<Account>().HasMany(account => account.AccountRoles).WithOne(accountRole => accountRole.Account)
                        .HasForeignKey(accountRole => accountRole.AccountGuid);

            // Emp - Ovt
            modelBuilder.Entity<Employee>().HasMany(employee => employee.Overtimes).WithOne(overtime => overtime.Employee)
                        .HasForeignKey(overtime => overtime.EmployeeGuid);

            // Hty - Ovt
            modelBuilder.Entity<HistoryOvertime>().HasOne(history => history.Overtime).WithMany(overtime => overtime.Histories)
                        .HasForeignKey(history => history.OvertimeGuid);

            // Payroll - Emp
            modelBuilder.Entity<Payroll>().HasOne(payslip => payslip.Employee).WithOne(employee => employee.Payroll)
                        .HasForeignKey<Payroll>(payslip => payslip.EmployeeGuid);

            // AccRole - Role
            modelBuilder.Entity<Role>().HasMany(role => role.AccountRoles).WithOne(accountRole => accountRole.Role)
                        .HasForeignKey(AccountRole => AccountRole.RoleGuid);

            // Emp - self relation
            modelBuilder.Entity<Employee>().HasOne(employee => employee.Manager).WithMany(employee => employee.Employees).HasForeignKey(employee => employee.ManagerGuid)
                        .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
