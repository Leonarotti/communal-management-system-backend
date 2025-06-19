using Microsoft.EntityFrameworkCore;
using CommunalManagementSystem.DataAccess.DAOs;

namespace CommunalManagementSystem.DataAccess.Context
{
    public class CommunalManagementSystemDbContext : DbContext
    {
        public CommunalManagementSystemDbContext(DbContextOptions<CommunalManagementSystemDbContext> options)
            : base(options)
        {
        }

        public DbSet<PersonDAO> Persons { get; set; }
        public DbSet<AuthUserDAO> AuthUsers { get; set; }
        public DbSet<QuotaDAO> Quotas { get; set; }
        public DbSet<IncomeDAO> Incomes { get; set; }
        public DbSet<ExpenseDAO> Expenses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ====== PERSON ======
            modelBuilder.Entity<PersonDAO>()
                .ToTable("persons");

            modelBuilder.Entity<PersonDAO>()
                .HasKey(p => p.id);

            modelBuilder.Entity<PersonDAO>()
                .HasIndex(p => p.dni)
                .IsUnique();

            modelBuilder.Entity<PersonDAO>()
                .Property(p => p.created_at)
                .HasDefaultValueSql("SYSDATETIME()");

            // ====== AUTH USERS ======
            modelBuilder.Entity<AuthUserDAO>()
                .ToTable("auth_users");

            modelBuilder.Entity<AuthUserDAO>()
                .HasKey(au => au.id);

            modelBuilder.Entity<AuthUserDAO>()
                .HasIndex(au => au.email)
                .IsUnique();

            modelBuilder.Entity<AuthUserDAO>()
                .HasOne(au => au.Person)
                .WithOne(p => p.AuthUser)
                .HasForeignKey<AuthUserDAO>(au => au.person_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AuthUserDAO>()
                .Property(au => au.created_at)
                .HasDefaultValueSql("SYSDATETIME()");

            modelBuilder.Entity<AuthUserDAO>()
                .HasCheckConstraint("CK_AuthUser_Role", "role IN ('admin', 'reader')");

            // ====== QUOTAS ======
            modelBuilder.Entity<QuotaDAO>()
                .ToTable("quotas");

            modelBuilder.Entity<QuotaDAO>()
                .HasKey(q => q.id);

            modelBuilder.Entity<QuotaDAO>()
                .HasOne(q => q.Person)
                .WithMany(p => p.Quotas)
                .HasForeignKey(q => q.person_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuotaDAO>()
                .HasIndex(q => new { q.person_id, q.year, q.month })
                .IsUnique();

            modelBuilder.Entity<QuotaDAO>()
                .Property(q => q.created_at)
                .HasDefaultValueSql("SYSDATETIME()");

            modelBuilder.Entity<QuotaDAO>()
                .HasCheckConstraint("CK_Quota_Status", "status IN ('paid', 'unpaid')");

            // ====== INCOMES ======
            modelBuilder.Entity<IncomeDAO>()
                .ToTable("incomes");

            modelBuilder.Entity<IncomeDAO>()
                .HasKey(i => i.id);

            modelBuilder.Entity<IncomeDAO>()
                .Property(i => i.created_at)
                .HasDefaultValueSql("SYSDATETIME()");

            // ====== EXPENSES ======
            modelBuilder.Entity<ExpenseDAO>()
                .ToTable("expenses");

            modelBuilder.Entity<ExpenseDAO>()
                .HasKey(e => e.id);

            modelBuilder.Entity<ExpenseDAO>()
                .Property(e => e.created_at)
                .HasDefaultValueSql("SYSDATETIME()");
        }
    }
}
