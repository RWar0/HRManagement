using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.DTOs
{
    public class LeaveTypeDTO : IModelDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        private string? _description;
        public string? Description 
        { 
            get => _description ?? "-----"; 
            set
            {
                _description = value;
            }
        }
    }
}
