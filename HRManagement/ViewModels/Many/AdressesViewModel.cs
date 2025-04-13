using CommunityToolkit.Mvvm.Messaging;
using HRManagement.Helpers;
using HRManagement.Helpers.Enums;
using HRManagement.Models;
using HRManagement.Models.Contexts;
using HRManagement.Models.DTOs;
using HRManagement.Models.ModalWindows;
using HRManagement.Models.Services;
using HRManagement.ViewModels.Single;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.ViewModels.Many
{
    public class AdressesViewModel : BaseManyViewModel<AdressService, Adress, AdressDTO>
    {
        #region Fitration Properties
        public string CountryName
        {
            get => Service.CountryName ?? "";
            set
            {
                if(value != Service.CountryName)
                {
                    Service.CountryName = value;
                    OnPropertyChanged(() => CountryName);
                }
            }
        }
        public string CityName
        {
            get => Service?.CityName ?? "";
            set
            {
                if(value != Service.CityName)
                {
                    Service.CityName = value;
                    OnPropertyChanged(() => CityName);
                }
            }
        }
        public string StreetName
        {
            get => Service?.StreetName ?? "";
            set
            {
                if(value != Service.StreetName)
                {
                    Service.StreetName = value;
                    OnPropertyChanged(() => StreetName);
                }
            }
        }
        public List<YesNoFilterComboBoxDTO> SelectingFilterModels { get; set; } = default!;
        public YesNoFilterEnum HasStreetFilter 
        {
            get => Service.HasStreetFilter;
            set
            {
                if(value != Service.HasStreetFilter)
                {
                    Service.HasStreetFilter = value;
                    OnPropertyChanged(() => HasStreetFilter);
                }
            }
        }
        public YesNoFilterEnum HasPostalCodeFilter
        {
            get => Service.HasPostalCodeFilter;
            set
            {
                if (value != Service.HasPostalCodeFilter)
                {
                    Service.HasPostalCodeFilter = value;
                    OnPropertyChanged(() => HasPostalCodeFilter);
                }
            }
        }
        public YesNoFilterEnum HasFlatNumberFilter
        {
            get => Service.HasFlatNumberFilter;
            set
            {
                if (value != Service.HasFlatNumberFilter)
                {
                    Service.HasFlatNumberFilter = value;
                    OnPropertyChanged(() => HasFlatNumberFilter);
                }
            }
        }

        #endregion

        public AdressesViewModel() : base("Adresses") 
        {
            InitializeFiltersFields();
        }

        protected override void CreateNewWindow()
        {
            WeakReferenceMessenger.Default.Send<OpenViewMessage>(new OpenViewMessage()
            {
                Sender = this,
                ViewModelToBeOpened = new AdressViewModel()
            });
        }
        protected override void CleanFilters()
        {
            HasStreetFilter = SelectingFilterModels.First(item => item.SelectedOption == YesNoFilterEnum.NoFilter).SelectedOption;
            HasPostalCodeFilter = SelectingFilterModels.First(item => item.SelectedOption == YesNoFilterEnum.NoFilter).SelectedOption;
            HasFlatNumberFilter = SelectingFilterModels.First(item => item.SelectedOption == YesNoFilterEnum.NoFilter).SelectedOption;
            CountryName = "";
            CityName = "";
            StreetName = "";
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
                    OptionName = "Is assigned",
                    SelectedOption = YesNoFilterEnum.Yes,
                },
                new()
                {
                    OptionName = "Not assigned",
                    SelectedOption = YesNoFilterEnum.No,
                }
            };
            HasStreetFilter = SelectingFilterModels.First(item => item.SelectedOption == YesNoFilterEnum.NoFilter).SelectedOption;
            HasPostalCodeFilter = SelectingFilterModels.First(item => item.SelectedOption == YesNoFilterEnum.NoFilter).SelectedOption;
            HasFlatNumberFilter = SelectingFilterModels.First(item => item.SelectedOption == YesNoFilterEnum.NoFilter).SelectedOption;

            Refresh();
        }
    }
}
