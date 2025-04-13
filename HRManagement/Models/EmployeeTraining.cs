using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Models;

public partial class EmployeeTraining : IManyModel
{
    [Key]
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public int TrainingId { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EditDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteDateTime { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("EmployeeTrainings")]
    public virtual Employee Employee { get; set; } = null!;

    [ForeignKey("TrainingId")]
    [InverseProperty("EmployeeTrainings")]
    public virtual Training Training { get; set; } = null!;

    int IManyModel.PropertyId
    {
        get => TrainingId;
        set => TrainingId = value;
    }
}
