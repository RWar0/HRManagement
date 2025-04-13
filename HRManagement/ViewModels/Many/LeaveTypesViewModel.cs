using CommunityToolkit.Mvvm.Messaging;
using HRManagement.Helpers;
using HRManagement.Models;
using HRManagement.Models.Contexts;
using HRManagement.Models.DTOs;
using HRManagement.Models.Services;
using HRManagement.ViewModels.Single;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRManagement.ViewModels.Many
{
    public class LeaveTypesViewModel : ManyWithEditingViewModel<LeaveTypeService, LeaveType, LeaveTypeDTO>
    {
        #region Filtration Properties
        public string TitleName
        {
            get => Service.TitleName ?? "";
            set
            {
                if (value != Service.TitleName)
                {
                    Service.TitleName = value;
                    OnPropertyChanged(() => TitleName);
                }
            }
        }
        public string DescriptionName
        {
            get => Service.DescriptionName ?? "";
            set
            {
                if (value != Service.DescriptionName)
                {
                    Service.DescriptionName = value;
                    OnPropertyChanged(() => DescriptionName);
                }
            }
        }
        public bool HasDescription
        {
            get => Service.HasDescription;
            set
            {
                if(value != Service.HasDescription)
                {
                    Service.HasDescription = value;
                    OnPropertyChanged(() => HasDescription);
                }
            }
        }
        #endregion

        public LeaveTypesViewModel() : base("Leave Types") { }

        protected override void CreateNewWindow()
        {
            WeakReferenceMessenger.Default.Send<OpenViewMessage>(new OpenViewMessage()
            {
                Sender = this,
                ViewModelToBeOpened = new LeaveTypeViewModel()
            });
        }
        protected override void CleanFilters()
        {
            HasDescription = false;
            TitleName = "";
            DescriptionName = "";
            SortProperty = SortOptions.FirstOrDefault()?.PropertyTitle;
            IsOrderDescending = false;
            Refresh();
        }
    }
}
