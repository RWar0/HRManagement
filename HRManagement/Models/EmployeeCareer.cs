using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Models;

[Table("EmployeeCareer")]
public partial class EmployeeCareer : IManyModel
{
    [Key]
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public int CareerId { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EditDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteDateTime { get; set; }

    [ForeignKey("CareerId")]
    [InverseProperty("EmployeeCareers")]
    public virtual Career Career { get; set; } = null!;

    [ForeignKey("EmployeeId")]
    [InverseProperty("EmployeeCareers")]
    public virtual Employee Employee { get; set; } = null!;

    int IManyModel.PropertyId
    {
        get => CareerId;
        set => CareerId = value;
    }
}
