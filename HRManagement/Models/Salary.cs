using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Models;

public partial class Salary
{
    [Key]
    public int Id { get; set; }

    public string? Description { get; set; }

    [Column(TypeName = "money")]
    public decimal BruttoPrice { get; set; }

    public double TaxRate { get; set; }

    public double ZusTaxRate { get; set; }

    [Column(TypeName = "money")]
    public decimal Declusions { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EditDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteDateTime { get; set; }

    [InverseProperty("Salary")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    [InverseProperty("NewSalary")]
    public virtual ICollection<Promotion> PromotionNewSalaries { get; set; } = new List<Promotion>();

    [InverseProperty("OldSalary")]
    public virtual ICollection<Promotion> PromotionOldSalaries { get; set; } = new List<Promotion>();
}
