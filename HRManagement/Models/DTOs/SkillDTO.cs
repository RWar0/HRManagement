using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.DTOs
{
    public class SkillDTO : IModelDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public string Display => $"{Id}. {Title}";
    }
}
