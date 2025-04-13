using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Models;

public partial class Skill : IModel
{
    [Key]
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EditDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteDateTime { get; set; }

    [InverseProperty("Skill")]
    public virtual ICollection<EmployeSkill> EmployeSkills { get; set; } = new List<EmployeSkill>();
}
