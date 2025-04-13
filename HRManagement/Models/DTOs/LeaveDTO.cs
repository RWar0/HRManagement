using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.DTOs
{
    public class LeaveDTO : IBaseModelDTO
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; } = null!;
        public string LeaveType { get; set; } = null!;
        
        private string _reason = null!;
        public string? Reason 
        { 
            get => _reason; 
            set
            {
                _reason = value ?? "-----";
            }
        }
        public DateOnly BeginDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string LeaveStatus { get; set; } = null!;

    }
}
