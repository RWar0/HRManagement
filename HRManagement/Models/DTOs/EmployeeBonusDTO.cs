using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.DTOs
{
    public class EmployeeBonusDTO : IEmployeePropertyModel<Bonus>
    {
        public int Id { get; set; }
        public Bonus Property { get; set; } = default!;
        public Employee Employee { get; set; } = default!;
    }
}
