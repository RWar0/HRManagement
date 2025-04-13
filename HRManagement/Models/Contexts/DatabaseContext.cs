using System;
using System.Collections.Generic;
using System.Diagnostics;
using HRManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Models.Contexts;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Adress> Adresses { get; set; }

    public virtual DbSet<Bonus> Bonuses { get; set; }

    public virtual DbSet<Career> Careers { get; set; }

    public virtual DbSet<EmployeSkill> EmployeSkills { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeBonus> EmployeeBonuses { get; set; }

    public virtual DbSet<EmployeeCareer> EmployeeCareers { get; set; }

    public virtual DbSet<EmployeeTraining> EmployeeTrainings { get; set; }

    public virtual DbSet<Leave> Leaves { get; set; }

    public virtual DbSet<LeaveStatus> LeaveStatuses { get; set; }

    public virtual DbSet<LeaveType> LeaveTypes { get; set; }

    public virtual DbSet<PersonalData> PersonalData { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<Salary> Salaries { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<Training> Trainings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=RAFA-KOMPUTER;Integrated Security=True;Trust Server Certificate=True;Database=HRManagementProject");
        optionsBuilder.LogTo(item => Debug.WriteLine(item));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Adress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Adresses__3214EC07D1C25E34");
        });

        modelBuilder.Entity<Bonus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Bonuses__3214EC0799ADEC89");
        });

        modelBuilder.Entity<Career>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Careers__3214EC073AC1BB81");
        });

        modelBuilder.Entity<EmployeSkill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EmployeS__3214EC07BF0E174E");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeSkills)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EmployeSk__Emplo__4D94879B");

            entity.HasOne(d => d.Skill).WithMany(p => p.EmployeSkills)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EmployeSk__Skill__4E88ABD4");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC07A38A06FA");

            entity.HasOne(d => d.Position).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employees__Posit__3C69FB99");

            entity.HasOne(d => d.Salary).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employees__Salar__3B75D760");
        });

        modelBuilder.Entity<EmployeeBonus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC079773374E");

            entity.HasOne(d => d.Bonus).WithMany(p => p.EmployeeBonuses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EmployeeB__Bonus__693CA210");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeBonuses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EmployeeB__Emplo__68487DD7");
        });

        modelBuilder.Entity<EmployeeCareer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC07DC9F20B0");

            entity.HasOne(d => d.Career).WithMany(p => p.EmployeeCareers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EmployeeC__Caree__48CFD27E");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeCareers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EmployeeC__Emplo__47DBAE45");
        });

        modelBuilder.Entity<EmployeeTraining>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC07ECB753F6");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeTrainings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EmployeeT__Emplo__628FA481");

            entity.HasOne(d => d.Training).WithMany(p => p.EmployeeTrainings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EmployeeT__Train__6383C8BA");
        });

        modelBuilder.Entity<Leave>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Leaves__3214EC07B4111508");

            entity.HasOne(d => d.Employee).WithMany(p => p.Leaves)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Leaves__Employee__5535A963");

            entity.HasOne(d => d.LeaveStatus).WithMany(p => p.Leaves)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Leaves__LeaveSta__571DF1D5");

            entity.HasOne(d => d.LeaveType).WithMany(p => p.Leaves)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Leaves__LeaveTyp__5629CD9C");
        });

        modelBuilder.Entity<LeaveStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LeaveSta__3214EC07AC376C8C");
        });

        modelBuilder.Entity<LeaveType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LeaveTyp__3214EC07DD1BD6AA");
        });

        modelBuilder.Entity<PersonalData>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Personal__3214EC07C5BF1445");

            entity.Property(e => e.Pesel).IsFixedLength();

            entity.HasOne(d => d.Employee).WithMany(p => p.PersonalData)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PersonalD__Emplo__412EB0B6");

            entity.HasOne(d => d.RegistrationAdress).WithMany(p => p.PersonalDatumRegistrationAdresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PersonalD__Regis__4316F928");

            entity.HasOne(d => d.ResidenceAdress).WithMany(p => p.PersonalDatumResidenceAdresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PersonalD__Resid__4222D4EF");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Position__3214EC07BE8E2734");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Promotio__3214EC07D10705A8");

            entity.HasOne(d => d.Employee).WithMany(p => p.Promotions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Promotion__Emplo__59FA5E80");

            entity.HasOne(d => d.NewPosition).WithMany(p => p.PromotionNewPositions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Promotion__NewPo__5BE2A6F2");

            entity.HasOne(d => d.NewSalary).WithMany(p => p.PromotionNewSalaries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Promotion__NewSa__5DCAEF64");

            entity.HasOne(d => d.OldPosition).WithMany(p => p.PromotionOldPositions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Promotion__OldPo__5AEE82B9");

            entity.HasOne(d => d.OldSalary).WithMany(p => p.PromotionOldSalaries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Promotion__OldSa__5CD6CB2B");
        });

        modelBuilder.Entity<Salary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Salaries__3214EC078AF00C5C");
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Skills__3214EC07E2E9F6BB");
        });

        modelBuilder.Entity<Training>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Training__3214EC07C37A824F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
