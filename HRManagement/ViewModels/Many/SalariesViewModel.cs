using CommunityToolkit.Mvvm.Messaging;
using HRManagement.Helpers;
using HRManagement.Helpers.Enums;
using HRManagement.Models;
using HRManagement.Models.BusinessLogic;
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
    public class SalariesViewModel : BaseManyViewModel<SalaryWithEmployeeService, Salary, SalaryWithEmployeeDTO>
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
        public string DeclusionsFrom
        {
            get => Service?.DeclusionsFrom + "" ?? "";
            set
            {
                if (value != Service.DeclusionsFrom + "")
                {
                    try
                    {
                        Service.DeclusionsFrom = decimal.Parse(value);
                        OnPropertyChanged(() => DeclusionsFrom);
                    }
                    catch (Exception)
                    {
                        Service.DeclusionsFrom = null;
                        OnPropertyChanged(() => DeclusionsFrom);
                    }
                }
            }
        }
        public string DeclusionsTo
        {
            get => Service?.DeclusionsTo + "" ?? "";
            set
            {
                if (value != Service.DeclusionsTo + "")
                {
                    try
                    {
                        Service.DeclusionsTo = decimal.Parse(value);
                        OnPropertyChanged(() => DeclusionsTo);
                    }
                    catch (Exception)
                    {
                        Service.DeclusionsTo = null;
                        OnPropertyChanged(() => DeclusionsTo);
                    }
                }
            }
        }
        public string TaxFrom
        {
            get => Service?.TaxFrom + "" ?? "";
            set
            {
                if (value != Service.TaxFrom + "")
                {
                    try
                    {
                        Service.TaxFrom = double.Parse(value);
                        OnPropertyChanged(() => TaxFrom);
                    }
                    catch (Exception)
                    {
                        Service.TaxFrom = null;
                        OnPropertyChanged(() => TaxFrom);
                    }
                }
            }
        }
        public string TaxTo
        {
            get => Service?.TaxTo + "" ?? "";
            set
            {
                if (value != Service.TaxTo + "")
                {
                    try
                    {
                        Service.TaxTo = double.Parse(value);
                        OnPropertyChanged(() => TaxTo);
                    }
                    catch (Exception)
                    {
                        Service.TaxTo = null;
                        OnPropertyChanged(() => TaxTo);
                    }
                }
            }
        }
        public string ZusTaxFrom
        {
            get => Service?.ZusTaxFrom + "" ?? "";
            set
            {
                if (value != Service.ZusTaxFrom + "")
                {
                    try
                    {
                        Service.ZusTaxFrom = double.Parse(value);
                        OnPropertyChanged(() => ZusTaxFrom);
                    }
                    catch (Exception)
                    {
                        Service.ZusTaxFrom = null;
                        OnPropertyChanged(() => ZusTaxFrom);
                    }
                }
            }
        }
        public string ZusTaxTo
        {
            get => Service?.ZusTaxTo + "" ?? "";
            set
            {
                if (value != Service.ZusTaxTo + "")
                {
                    try
                    {
                        Service.ZusTaxTo = double.Parse(value);
                        OnPropertyChanged(() => ZusTaxTo);
                    }
                    catch (Exception)
                    {
                        Service.ZusTaxTo = null;
                        OnPropertyChanged(() => ZusTaxTo);
                    }
                }
            }
        }

        public List<YesNoFilterComboBoxDTO> SelectingFilterModels { get; set; } = default!;
        public YesNoFilterEnum DescriptionFilter 
        {
            get => Service.DescriptionFilter;
            set
            {
                if(value != Service.DescriptionFilter)
                {
                    Service.DescriptionFilter = value;
                    OnPropertyChanged(() => DescriptionFilter);
                }
            }
        }
        #endregion

        public SalariesViewModel() : base("Salaries")
        {
            InitializeFiltersFields();
        }

        protected override void CreateNewWindow()
        {
            WeakReferenceMessenger.Default.Send<OpenViewMessage>(new OpenViewMessage()
            {
                Sender = this,
                ViewModelToBeOpened = new SalaryViewModel()
            });
        }

        protected override void CleanFilters()
        {
            EmployeeName = "";
            EmployeeSurname = "";
            PriceFrom = "";
            PriceTo = "";
            PriceTo = "";
            DeclusionsFrom = "";
            DeclusionsTo = "";
            TaxFrom = "";
            TaxTo = "";
            ZusTaxFrom = "";
            ZusTaxTo = "";
            DescriptionFilter = SelectingFilterModels.First(item => item.SelectedOption == YesNoFilterEnum.NoFilter).SelectedOption;
            SortProperty = SortOptions.FirstOrDefault()?.PropertyTitle;
            IsOrderDescending = false;
            Refresh();
        }

        private void InitializeFiltersFields()
        {
            SelectingFilterModels = new List<YesNoFilterComboBoxDTO>()
            {
                new()
                {
                    OptionName = "No filter",
                    SelectedOption = YesNoFilterEnum.NoFilter,
                },
                new()
                {
                    OptionName = "Not empty",
                    SelectedOption = YesNoFilterEnum.Yes,
                },
                new()
                {
                    OptionName = "Empty",
                    SelectedOption = YesNoFilterEnum.No,
                }
            };
            DescriptionFilter = SelectingFilterModels.First(item => item.SelectedOption == YesNoFilterEnum.NoFilter).SelectedOption;

            Refresh();
        }
    }
}
