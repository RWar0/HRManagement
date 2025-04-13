using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.DTOs
{
    public class EmployeeBasicDTO : IBaseModelDTO
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = null!;
        public string Surname { get; set; } = null!;

        public string DisplayBasics => $"{Id} - {Firstname} {Surname}";
    }
}
