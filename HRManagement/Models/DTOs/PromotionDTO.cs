using HRManagement.Models.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.DTOs
{
    public class PromotionDTO : IBaseModelDTO
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; } = null!;
        public DateOnly PromotionDate { get; set; }
        public string OldPosition { get; set; } = null!;
        public string NewPosition { get; set; } = null!;
        public SalaryBL OldSalary { get; set; } = null!;
        public SalaryBL NewSalary { get; set; } = null!;
    }
}
