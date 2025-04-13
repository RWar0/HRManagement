using HRManagement.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.DTOs
{
    public class YesNoFilterComboBoxDTO
    {
        public string OptionName { get; set; } = null!;
        public YesNoFilterEnum SelectedOption { get; set; }
    }
}
