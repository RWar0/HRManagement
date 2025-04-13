using HRManagement.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.ViewModels.Many
{
    public class EmployeesOnLeaveViewModel : EmployeesViewModel
    {
        public EmployeesOnLeaveViewModel() : base("Employee On Leave") { }

        #region Overrided Abstract Methods
        protected override void Refresh()
        {
            Models = new ObservableCollection<EmployeeDTO>(Service.GetModelsOnLeave());
        }
        #endregion
    }
}
