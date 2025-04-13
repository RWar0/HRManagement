using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.DTOs
{
    public class EmployeeCareerDTO : IEmployeePropertyModel<Career>
    {
        public int Id { get; set; }
        public Career Property { get; set; } = default!;
        public Employee Employee { get; set; } = default!;
    }
}
