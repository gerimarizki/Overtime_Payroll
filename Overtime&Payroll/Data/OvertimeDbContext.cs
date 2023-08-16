using Microsoft.EntityFrameworkCore;
using Overtime_Payroll.Models;
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
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeLevel> EmployeeLevels { get; set; }
        public DbSet<Overtime> Overtimes { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Employee>().HasIndex(e => new { e.NIK, e.Email, e.PhoneNumber }).IsUnique();

            builder.Entity<Overtime>().HasOne(u => u.Employee).WithMany(e => e.Overtimes)
                .HasForeignKey(e => e.EmployeeId);

            builder.Entity<Payroll>().HasOne(u => u.Employee).WithMany(e => e.Payrolls)
                .HasForeignKey(e => e.EmployeeId);

            builder.Entity<Department>().HasMany(u => u.Employee).WithOne(e => e.Department)
                .HasForeignKey(e => e.DepartmentId);

            builder.Entity<EmployeeLevel>().HasMany(u => u.Employees).WithOne(e => e.EmployeeLevel)
                .HasForeignKey(e => e.EmployeeLevelId);

            builder.Entity<Role>().HasMany(u => u.AccountRoles).WithOne(e => e.Role)
                .HasForeignKey(e => e.RoleGuid);

            builder.Entity<AccountRole>().HasOne(u => u.Account).WithMany(e => e.AccountRoles)
                .HasForeignKey(e => e.AccountGuid);

            builder.Entity<Account>().HasOne(u => u.Employee).WithOne(e => e.Account)
                .HasForeignKey<Account>(e => e.EmployeeId);
        }
    }
}
