using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Models;

public partial class Leave
{
    [Key]
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public int LeaveTypeId { get; set; }

    public string? Reason { get; set; }

    public DateOnly BeginDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int LeaveStatusId { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EditDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteDateTime { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("Leaves")]
    public virtual Employee Employee { get; set; } = null!;

    [ForeignKey("LeaveStatusId")]
    [InverseProperty("Leaves")]
    public virtual LeaveStatus LeaveStatus { get; set; } = null!;

    [ForeignKey("LeaveTypeId")]
    [InverseProperty("Leaves")]
    public virtual LeaveType LeaveType { get; set; } = null!;
}
