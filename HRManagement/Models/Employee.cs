using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Models;

public partial class Employee
{
    [Key]
    public int Id { get; set; }

    [StringLength(60)]
    public string Firstname { get; set; } = null!;

    [StringLength(80)]
    public string Surname { get; set; } = null!;

    [StringLength(15)]
    public string Gender { get; set; } = null!;

    [StringLength(100)]
    public string EmploymentType { get; set; } = null!;

    public int SalaryId { get; set; }

    public int PositionId { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EditDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteDateTime { get; set; }

    [InverseProperty("Employee")]
    public virtual ICollection<EmployeSkill> EmployeSkills { get; set; } = new List<EmployeSkill>();

    [InverseProperty("Employee")]
    public virtual ICollection<EmployeeBonus> EmployeeBonuses { get; set; } = new List<EmployeeBonus>();

    [InverseProperty("Employee")]
    public virtual ICollection<EmployeeCareer> EmployeeCareers { get; set; } = new List<EmployeeCareer>();

    [InverseProperty("Employee")]
    public virtual ICollection<EmployeeTraining> EmployeeTrainings { get; set; } = new List<EmployeeTraining>();

    [InverseProperty("Employee")]
    public virtual ICollection<Leave> Leaves { get; set; } = new List<Leave>();

    [InverseProperty("Employee")]
    public virtual ICollection<PersonalData> PersonalData { get; set; } = new List<PersonalData>();

    [ForeignKey("PositionId")]
    [InverseProperty("Employees")]
    public virtual Position Position { get; set; } = null!;

    [InverseProperty("Employee")]
    public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();

    [ForeignKey("SalaryId")]
    [InverseProperty("Employees")]
    public virtual Salary Salary { get; set; } = null!;
}
