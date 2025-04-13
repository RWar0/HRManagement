using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Models;

public partial class Promotion
{
    [Key]
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public DateOnly PromotionDate { get; set; }

    public int OldPositionId { get; set; }

    public int NewPositionId { get; set; }

    public int OldSalaryId { get; set; }

    public int NewSalaryId { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EditDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteDateTime { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("Promotions")]
    public virtual Employee Employee { get; set; } = null!;

    [ForeignKey("NewPositionId")]
    [InverseProperty("PromotionNewPositions")]
    public virtual Position NewPosition { get; set; } = null!;

    [ForeignKey("NewSalaryId")]
    [InverseProperty("PromotionNewSalaries")]
    public virtual Salary NewSalary { get; set; } = null!;

    [ForeignKey("OldPositionId")]
    [InverseProperty("PromotionOldPositions")]
    public virtual Position OldPosition { get; set; } = null!;

    [ForeignKey("OldSalaryId")]
    [InverseProperty("PromotionOldSalaries")]
    public virtual Salary OldSalary { get; set; } = null!;
}
