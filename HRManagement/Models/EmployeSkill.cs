using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Models;

public partial class EmployeSkill : IManyModel
{
    [Key]
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public int SkillId { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EditDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteDateTime { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("EmployeSkills")]
    public virtual Employee Employee { get; set; } = null!;

    [ForeignKey("SkillId")]
    [InverseProperty("EmployeSkills")]
    public virtual Skill Skill { get; set; } = null!;

    int IManyModel.PropertyId
    {
        get => SkillId;
        set => SkillId = value;
    }
}
