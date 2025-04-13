using HRManagement.Models.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.DTOs
{
    public class SalaryWithEmployeeDTO
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; } = null!;
        public SalaryBL Salary {  get; set; } = null!;
    }
}
