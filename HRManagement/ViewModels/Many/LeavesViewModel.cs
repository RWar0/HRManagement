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
using Microsoft.EntityFrameworkCore;
using HRManagement.Models.Services;
using CommunityToolkit.Mvvm.Messaging;
using HRManagement.ViewModels.Single;

namespace HRManagement.ViewModels.Many
{
    public class LeavesViewModel : ManyWithEditingViewModel<LeaveService, Leave, LeaveDTO>
    {
        #region Filtration Properties
        public string EmployeeName
        {
            get => Service.EmployeeName ?? "";
            set
            {
                if (value != Service.EmployeeName)
                {
                    Service.EmployeeName = value;
                    OnPropertyChanged(() => EmployeeName);
                }
            }
        }
        public string EmployeeSurname
        {
            get => Service.EmployeeSurname ?? "";
            set
            {
                if (value != Service.EmployeeSurname)
                {
                    Service.EmployeeSurname = value;
                    OnPropertyChanged(() => EmployeeSurname);
                }
            }
        }
        public string ReasonFilter
        {
            get => Service.ReasonFilter ?? "";
            set
            {
                if (value != Service.ReasonFilter)
                {
                    Service.ReasonFilter = value;
                    OnPropertyChanged(() => ReasonFilter);
                }
            }
        }
        public string LeaveTypeFilter
        {
            get => Service.LeaveTypeFilter ?? NO_FILTER;
            set
            {
                if(value != Service.LeaveTypeFilter)
                {
                    Service.LeaveTypeFilter = value;
                    OnPropertyChanged(() => LeaveTypeFilter);
                }
            }
        }
        public string LeaveStatusFilter
        {
            get => Service.LeaveStatusFilter ?? NO_FILTER;
            set
            {
                if(value != Service.LeaveStatusFilter)
                {
                    Service.LeaveStatusFilter = value;
                    OnPropertyChanged(() => LeaveStatusFilter);
                }
            }
        }
        public DateTime? DateFrom
        {
            get => Service.DateFrom.HasValue ? ((DateOnly)Service.DateFrom).ToDateTime(TimeOnly.MinValue) : null;
            set
            {
                Service.DateFrom = value != null ? DateOnly.FromDateTime((DateTime)value) : null;
                OnPropertyChanged(() => DateFrom);
            }
        }
        public DateTime? DateTo
        {
            get => Service.DateTo.HasValue ? ((DateOnly)Service.DateTo).ToDateTime(TimeOnly.MinValue) : null;
            set
            {
                Service.DateTo = value != null ? DateOnly.FromDateTime((DateTime)value) : null;
                OnPropertyChanged(() => DateTo);
            }
        }
        #endregion

        #region Additional Models
        private const string NO_FILTER = "No filter";
        public List<string> LeaveTypeModels { get; set; } = default!;
        public List<string> LeaveStatusModels { get; set; } = default!;
        #endregion

        #region Additional Services
        private LeaveTypeService LeaveTypeService { get; set; }
        private LeaveStatusService LeaveStatusService { get; set; }
        #endregion

        public LeavesViewModel() : base("Leaves")
        {
            LeaveTypeService = new LeaveTypeService();
            LeaveStatusService = new LeaveStatusService();
            InitializeModels();
        }

        #region Overrided Abstract Methods
        protected override void CreateNewWindow()
        {
            WeakReferenceMessenger.Default.Send<OpenViewMessage>(new OpenViewMessage()
            {
                Sender = this,
                ViewModelToBeOpened = new LeaveViewModel()
            });
        }
        protected override void CleanFilters()
        {
            EmployeeName = "";
            EmployeeSurname = "";
            ReasonFilter = "";
            DateFrom = null;
            DateTo = null;
            LeaveTypeFilter = NO_FILTER;
            LeaveStatusFilter = NO_FILTER;
            SortProperty = SortOptions.FirstOrDefault()?.PropertyTitle;
            IsOrderDescending = false;
            Refresh();
        }
        #endregion

        #region Methods
        private void InitializeModels()
        {
            LeaveTypeModels = LeaveTypeService.GetModelsTitles();
            LeaveTypeModels.Insert(0, NO_FILTER);
            LeaveTypeFilter = NO_FILTER;

            LeaveStatusModels = LeaveStatusService.GetModelsTitles();
            LeaveStatusModels.Insert(0, NO_FILTER);
            LeaveStatusFilter = NO_FILTER;
        }
        #endregion
    }
}
