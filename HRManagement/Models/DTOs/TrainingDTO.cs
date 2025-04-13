using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.DTOs
{
    public class TrainingDTO : IModelDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateOnly BeginDate { get; set; }
        public DateOnly EndDate { get; set; }

        public string Display => $"{Id}. {Title}";
    }
}
