using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Models;

public partial class Bonus : IModel
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    [Column(TypeName = "money")]
    public decimal Price { get; set; }

    public DateOnly BeginDate { get; set; }

    public DateOnly EndDate { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EditDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteDateTime { get; set; }

    [InverseProperty("Bonus")]
    public virtual ICollection<EmployeeBonus> EmployeeBonuses { get; set; } = new List<EmployeeBonus>();
}
