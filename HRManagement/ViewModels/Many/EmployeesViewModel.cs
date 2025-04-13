using CommunityToolkit.Mvvm.Messaging;
using HRManagement.Helpers;
using HRManagement.Models;
using HRManagement.Models.BusinessLogic;
using HRManagement.Models.Contexts;
using HRManagement.Models.DTOs;
using HRManagement.Models.Services;
using HRManagement.ViewModels.Single;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRManagement.ViewModels.Many
{
    public class EmployeesViewModel : ManyWithEditingViewModel<EmployeeService, Employee, EmployeeDTO>
    {
        #region Filtration Properties
        public string EmployeeName 
        {
            get => Service.EmployeeName ?? "";
            set
            {
                if(value != Service.EmployeeName)
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
        public string SelectedGender
        {
            get => Service.SelectedGender ?? "No filter";
            set
            {
                if (value != Service.SelectedGender)
                {
                    Service.SelectedGender = value;
                    OnPropertyChanged(() => SelectedGender);
                }
            }
        }
        public string EducationFilter
        {
            get => Service.EducationFilter ?? "";
            set
            {
                if (value != Service.EducationFilter)
                {
                    Service.EducationFilter = value;
                    OnPropertyChanged(() => EducationFilter);
                }
            }
        }
        public string EmploymentFilter
        {
            get => Service.EmploymentFilter ?? "";
            set
            {
                if (value != Service.EmploymentFilter)
                {
                    Service.EmploymentFilter = value;
                    OnPropertyChanged(() => EmploymentFilter);
                }
            }
        }
        public string PositionFilter
        {
            get => Service.PositionFilter ?? "";
            set
            {
                if (value != Service.PositionFilter)
                {
                    Service.PositionFilter = value;
                    OnPropertyChanged(() => PositionFilter);
                }
            }
        }
        public string PriceFrom
        {
            get => Service?.PriceFrom + "" ?? "";
            set
            {
                if (value != Service.PriceFrom + "")
                {
                    try
                    {
                        Service.PriceFrom = decimal.Parse(value);
                        OnPropertyChanged(() => PriceFrom);
                    }
                    catch (Exception)
                    {
                        Service.PriceFrom = null;
                        OnPropertyChanged(() => PriceFrom);
                    }
                }
            }
        }
        public string PriceTo
        {
            get => Service?.PriceTo + "" ?? "";
            set
            {
                if (value != Service.PriceTo + "")
                {
                    try
                    {
                        Service.PriceTo = decimal.Parse(value);
                        OnPropertyChanged(() => PriceTo);
                    }
                    catch (Exception)
                    {
                        Service.PriceTo = null;
                        OnPropertyChanged(() => PriceTo);
                    }
                }
            }
        }
        #endregion

        #region Additional Models
        public List<string> GenderModels { get; set; }
        #endregion

        public EmployeesViewModel() : base("Employees")
        {
            GenderModels = new List<string>() { "No filter", "Male", "Female" };
            SelectedGender = "No filter";
        }

        public EmployeesViewModel(string displayName) : base(displayName)
        {
            GenderModels = new List<string>() { "No filter", "Male", "Female" };
            SelectedGender = "No filter";
        }

        protected override void CreateNewWindow()
        {
            WeakReferenceMessenger.Default.Send<OpenViewMessage>(new OpenViewMessage()
            {
                Sender = this,
                ViewModelToBeOpened = new EmployeeViewModel()
            });
        }
        protected override void CleanFilters()
        {
            EmployeeName = "";
            EmployeeSurname = "";
            SelectedGender = "No filter";
            EducationFilter = "";
            EmploymentFilter = "";
            PositionFilter = "";
            PriceFrom = "";
            PriceTo = "";
            SortProperty = SortOptions.FirstOrDefault()?.PropertyTitle;
            IsOrderDescending = false;
            Refresh();
        }

    }
}
