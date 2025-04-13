using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRManagement.Models.BusinessLogic;

namespace HRManagement.Models.DTOs
{
    public class EmployeeDTO : EmployeeBasicDTO
    {
        public string Gender { get; set; } = null!;
        public string EmploymentType { get; set; } = null!;
        public string PositionName { get; set; } = null!;
        public string Education { get; set; } = null!;
        public SalaryBL Salary { get; set; } = null!;

    }
}
