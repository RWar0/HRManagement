using HRManagement.Helpers;
using HRManagement.Models.Contexts;
using HRManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HRManagement.Models.DTOs;
using HRManagement.Models.Services;
using CommunityToolkit.Mvvm.Messaging;
using HRManagement.ViewModels.Single;

namespace HRManagement.ViewModels.Many
{
    public class LeaveStatusesViewModel : ManyWithEditingViewModel<LeaveStatusService, LeaveStatus, LeaveStatusDTO>
    {
        #region Filtration Properties
        public string TitleName
        {
            get => Service.TitleName ?? "";
            set
            {
                if(value !=  Service.TitleName)
                {
                    Service.TitleName = value;
                    OnPropertyChanged(() => TitleName);
                }
            }
        }
        #endregion

        public LeaveStatusesViewModel() : base("Leave Statuses") 
        {

        }

        protected override void CreateNewWindow()
        {
            WeakReferenceMessenger.Default.Send<OpenViewMessage>(new OpenViewMessage()
            {
                Sender = this,
                ViewModelToBeOpened = new LeaveStatusViewModel()
            });
        }
        protected override void CleanFilters()
        {
            TitleName = "";
            SortProperty = SortOptions.FirstOrDefault()?.PropertyTitle;
            IsOrderDescending = false;
            Refresh();
        }

    }
}
