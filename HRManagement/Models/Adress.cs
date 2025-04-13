using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Models;

public partial class Adress
{
    [Key]
    public int Id { get; set; }

    [StringLength(128)]
    public string Country { get; set; } = null!;

    [StringLength(128)]
    public string City { get; set; } = null!;

    [StringLength(16)]
    public string? PostalCode { get; set; }

    [StringLength(128)]
    public string? Street { get; set; }

    [StringLength(16)]
    public string HouseNumber { get; set; } = null!;

    [StringLength(16)]
    public string? FlatNumber { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EditDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteDateTime { get; set; }

    [InverseProperty("RegistrationAdress")]
    public virtual ICollection<PersonalData> PersonalDatumRegistrationAdresses { get; set; } = new List<PersonalData>();

    [InverseProperty("ResidenceAdress")]
    public virtual ICollection<PersonalData> PersonalDatumResidenceAdresses { get; set; } = new List<PersonalData>();
}
