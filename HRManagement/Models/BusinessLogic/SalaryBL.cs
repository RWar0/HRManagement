using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.BusinessLogic
{
    public class SalaryBL
    {
        public int Id { get; set; }
        private decimal _bruttoPrice;
        public decimal BruttoPrice
        {
            get => Decimal.Round(_bruttoPrice, 2);
            set
            {
                _bruttoPrice = value;
            }
        }
        public decimal _declusions;
        public decimal Declusions
        {
            get => Decimal.Round(_declusions, 2);
            set
            {
                _declusions = value;
            }
        }
        private double _taxRate;
        public double TaxRate
        {
            get => _taxRate;
            set
            {
                _taxRate = value;
            }
        }
        public double TaxRatePercent
        {
            get => _taxRate * 100;
            set
            {
                _taxRate = value / 100;
            }
        }

        private double _zusTaxRate;
        public double ZusTaxRate 
        { 
            get => _zusTaxRate;
            set
            {
                _zusTaxRate = value;
            }
        }
        public double ZusTaxRatePercent
        {
            get => _zusTaxRate * 100;
            set
            {
                _zusTaxRate = value/100;
            }
        }
        public virtual decimal NettoPrice
        {
            get
            {
                Decimal netto = (_bruttoPrice * (decimal)(1 - ZusTaxRate)) * (decimal)(1 - TaxRate) - _declusions;
                return Decimal.Round(netto, 2);
            }
        }


        private string? _additionalDescription;
        public string? AdditionalDescription 
        {
            get => _additionalDescription ?? "-----";
            set => _additionalDescription = value;
        }


        public decimal CalculateBruttoFromNetto(decimal netto)
        {
            Decimal brutto = (netto / ((decimal)(1 - ZusTaxRate) * (decimal)(1 - TaxRate))) + _declusions;
            return Decimal.Round(brutto, 2);
        }

    }
}
