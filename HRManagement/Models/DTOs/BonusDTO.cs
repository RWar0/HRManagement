using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.DTOs
{
    public class BonusDTO : IModelDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string _description = null!;
        public string? Description
        {
            get => _description;
            set
            {
                _description = value ?? "-----";
            }
        }
        private decimal _price;
        public decimal Price
        {
            get => Decimal.Round(_price, 2);
            set => _price = value;
        }
        public DateOnly BeginDate {  get; set; }
        public DateOnly EndDate {  get; set; }

        public string Display => $"{Id}. {Title} | {Description}";
    }
}
