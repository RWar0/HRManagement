using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models
{
    public interface IManyModel
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int EmployeeId { get; set; }
    }
}
