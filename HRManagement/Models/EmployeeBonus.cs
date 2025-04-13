using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Models;

public partial class EmployeeBonus : IManyModel
{
    [Key]
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public int BonusId { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EditDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteDateTime { get; set; }

    [ForeignKey("BonusId")]
    [InverseProperty("EmployeeBonuses")]
    public virtual Bonus Bonus { get; set; } = null!;

    [ForeignKey("EmployeeId")]
    [InverseProperty("EmployeeBonuses")]
    public virtual Employee Employee { get; set; } = null!;

    int IManyModel.PropertyId
    {
        get => BonusId;
        set => BonusId = value;
    }
}
