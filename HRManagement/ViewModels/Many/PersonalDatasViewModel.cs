using CommunityToolkit.Mvvm.Messaging;
using HRManagement.Helpers;
using HRManagement.Models;
using HRManagement.Models.Contexts;
using HRManagement.Models.DTOs;
using HRManagement.Models.Services;
using HRManagement.ViewModels.Single;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.ViewModels.Many
{
    public class PersonalDatasViewModel : BaseManyViewModel<PersonalDataService, PersonalData, PersonalDataDTO>
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
        public string PeselFilter
        {
            get => Service.PeselFilter ?? "";
            set
            {
                if (value != Service.PeselFilter)
                {
                    Service.PeselFilter = value;
                    OnPropertyChanged(() => PeselFilter);
                }
            }
        }
        public string BirthPlaceFilter
        {
            get => Service.BirthPlaceFilter ?? "";
            set
            {
                if (value != Service.BirthPlaceFilter)
                {
                    Service.BirthPlaceFilter = value;
                    OnPropertyChanged(() => BirthPlaceFilter);
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
        public string ChildrensFrom
        {
            get => Service?.ChildrensFrom + "" ?? "";
            set
            {
                if (value != Service.ChildrensFrom + "")
                {
                    try
                    {
                        Service.ChildrensFrom = int.Parse(value);
                        OnPropertyChanged(() => ChildrensFrom);
                    }
                    catch (Exception)
                    {
                        Service.ChildrensFrom = null;
                        OnPropertyChanged(() => ChildrensFrom);
                    }
                }
            }
        }
        public string ChildrensTo
        {
            get => Service?.ChildrensTo + "" ?? "";
            set
            {
                if (value != Service.ChildrensTo + "")
                {
                    try
                    {
                        Service.ChildrensTo = int.Parse(value);
                        OnPropertyChanged(() => ChildrensTo);
                    }
                    catch (Exception)
                    {
                        Service.ChildrensTo = null;
                        OnPropertyChanged(() => ChildrensTo);
                    }
                }
            }
        }
        public string DateFrom
        {
            get => Service?.DateFrom + "" ?? "";
            set
            {
                if (value != Service.DateFrom + "")
                {
                    try
                    {
                        Service.DateFrom = int.Parse(value);
                        OnPropertyChanged(() => DateFrom);
                    }
                    catch (Exception)
                    {
                        Service.DateFrom = null;
                        OnPropertyChanged(() => DateFrom);
                    }
                }
            }
        }
        public string DateTo
        {
            get => Service?.DateTo + "" ?? "";
            set
            {
                if (value != Service.DateTo + "")
                {
                    try
                    {
                        Service.DateTo = int.Parse(value);
                        OnPropertyChanged(() => DateTo);
                    }
                    catch (Exception)
                    {
                        Service.DateTo = null;
                        OnPropertyChanged(() => DateTo);
                    }
                }
            }
        }
        #endregion

        public PersonalDatasViewModel() : base("Personal Datas")
        {

        }

        protected override void CreateNewWindow()
        {
            WeakReferenceMessenger.Default.Send<OpenViewMessage>(new OpenViewMessage()
            {
                Sender = this,
                ViewModelToBeOpened = new PersonalDataViewModel()
            });
        }
        protected override void CleanFilters()
        {
            EmployeeName = "";
            EmployeeSurname = "";
            PeselFilter = "";
            BirthPlaceFilter = "";
            EducationFilter = "";
            ChildrensFrom = "";
            ChildrensTo = "";
            DateFrom = "";
            DateTo = "";
            SortProperty = SortOptions.FirstOrDefault()?.PropertyTitle;
            IsOrderDescending = false;
            Refresh();
        }
    }
}
