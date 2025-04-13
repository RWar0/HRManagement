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
using System.Xml.Linq;

namespace HRManagement.ViewModels.Many
{
    public class PositionsViewModel : ManyWithEditingViewModel<PositionService, Position, PositionDTO>
    {
        #region Filtration Properties
        public string TitleName
        {
            get => Service.TitleName ?? "";
            set
            {
                if(value != Service.TitleName)
                {
                    Service.TitleName = value;
                    OnPropertyChanged(() => TitleName);
                }
            }
        }
        public string DepartmentName
        {
            get => Service.DepartmentName ?? "";
            set
            {
                if (value != Service.DepartmentName)
                {
                    Service.DepartmentName = value;
                    OnPropertyChanged(() => DepartmentName);
                }
            }
        }
        public bool HasDepartment
        {
            get => Service.HasDepartment;
            set
            {
                if(value != Service.HasDepartment)
                {
                    Service.HasDepartment = value;
                    OnPropertyChanged(() => HasDepartment);
                }
            }
        }
        #endregion

        public PositionsViewModel() : base("Positions")
        {

        }

        protected override void CreateNewWindow()
        {
            WeakReferenceMessenger.Default.Send<OpenViewMessage>(new OpenViewMessage()
            {
                Sender = this,
                ViewModelToBeOpened = new PositionViewModel()
            });
        }
        protected override void CleanFilters()
        {
            TitleName = "";
            DepartmentName = "";
            HasDepartment = false;
            SortProperty = SortOptions.FirstOrDefault()?.PropertyTitle;
            IsOrderDescending = false;
            Refresh();
        }
    }
}
