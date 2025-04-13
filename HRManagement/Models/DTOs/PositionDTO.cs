using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.DTOs
{
    public class PositionDTO : IModelDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public string _DepartmentName = null!;
        public string? DepartmentName
        {
            get => _DepartmentName;
            set
            {
                _DepartmentName = value ?? "-----";
            }
        }

        public string DisplayTitleAndDepartment => $"{Title} | {DepartmentName}";

    }
}
