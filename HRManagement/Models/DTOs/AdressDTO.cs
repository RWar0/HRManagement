using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.DTOs
{
    public class AdressDTO
    {
        public int Id { get; set; }
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;

        private string _postalCode = null!;
        public string? PostalCode
        {
            get => _postalCode;
            set
            {
                _postalCode = value ?? "-----";
            }
        }

        private string _street = null!;
        public string? Street
        {
            get => _street;
            set
            {
                _street = value ?? "-----";
            }
        }

        public string HouseNumber { get; set; } = null!;


        private string _flatNumber = null!;
        public string? FlatNumber
        {
            get => _flatNumber;
            set
            {
                _flatNumber = value ?? "-----";
            }
        }

        public string ShortAdressDisplay
        {
            get
            {
                return $"{Country}, {City}, {Street ?? City} {HouseNumber}"
                    + (_flatNumber.Equals("-----") ? "" : $"/{FlatNumber}");
            }
        }
    }
}
