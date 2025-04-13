using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.DTOs
{
    public class CareerDTO : IModelDTO
    {
        public int Id { get; set; }
        public string Place { get; set; } = null!;
        public string Title
        {
            get => Place;
            set => Place = value;
        }
        public string Position { get; set; } = null!;
        public DateOnly BeginDate { get; set; }
        public DateOnly EndDate { get; set; }

        public string Display => $"{Id}. {Title} | {Position ?? ""}";
    }
}
