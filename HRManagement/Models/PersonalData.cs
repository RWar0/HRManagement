using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Models;

public partial class PersonalData
{
    [Key]
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    [Column("PESEL")]
    [StringLength(11)]
    [Unicode(false)]
    public string Pesel { get; set; } = null!;

    [StringLength(16)]
    [Unicode(false)]
    public string PhoneNumber { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    [StringLength(60)]
    public string PlaceOfBirth { get; set; } = null!;

    public int ChildrenQuantity { get; set; }

    [StringLength(40)]
    public string Education { get; set; } = null!;

    public int ResidenceAdressId { get; set; }

    public int RegistrationAdressId { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EditDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteDateTime { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("PersonalData")]
    public virtual Employee Employee { get; set; } = null!;

    [ForeignKey("RegistrationAdressId")]
    [InverseProperty("PersonalDatumRegistrationAdresses")]
    public virtual Adress RegistrationAdress { get; set; } = null!;

    [ForeignKey("ResidenceAdressId")]
    [InverseProperty("PersonalDatumResidenceAdresses")]
    public virtual Adress ResidenceAdress { get; set; } = null!;
}
