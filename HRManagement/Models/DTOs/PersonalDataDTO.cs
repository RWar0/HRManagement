using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.DTOs
{
    public class PersonalDataDTO
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; } = null!;
        public string Pesel { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; } = null!;
        public int ChildrenQuantity { get; set; }
        public string Education { get; set; } = null!;

        public AdressDTO RegistrationAdress { get; set; } = null!;
        public AdressDTO ResidenceAdress { get; set; } = null!;
    }
}
