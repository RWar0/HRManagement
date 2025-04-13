using HRManagement.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.ViewModels.Many
{
    public class AvailableEmployeesWithCallbackViewModel : EmployeesWithCallbackViewModel
    {
        public List<EmployeeDTO> UsedEmployees { get; set; } = default!;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="whoRequestedToSelect">Window object which requested opening this ViewModel</param>
        /// <param name="usedEmployees">List of the arleady used employees, that should be omitted</param>
        public AvailableEmployeesWithCallbackViewModel(object whoRequestedToSelect, ObservableCollection<EmployeeDTO> usedEmployees) : base(whoRequestedToSelect) 
        {
            UsedEmployees = usedEmployees.ToList();
            Refresh();
        }

        #region Overrided methods
        protected override void Refresh()
        {
            if(UsedEmployees != null)
            {
                Models = new ObservableCollection<EmployeeDTO>(Service.GetAvailableModels(UsedEmployees.ToList()));
            }
            else
            {
                Models = new ObservableCollection<EmployeeDTO>(Service.GetModels());
            }
        }
        #endregion
    }
}
