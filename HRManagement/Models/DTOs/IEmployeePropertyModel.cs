using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.DTOs
{
    public interface IEmployeePropertyModel<PropertyClass>
        where PropertyClass : class
    {
        public int Id { get; set; }
        public PropertyClass Property { get; set; }
        public Employee Employee { get; set; }
    }
}
