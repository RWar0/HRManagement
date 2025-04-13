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
    public class PromotionsViewModel : ManyWithEditingViewModel<PromotionService, Promotion, PromotionDTO>
    {
        #region FiltrationProperties
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

        public List<YesNoFilterComboBoxDTO> SelectingFilterModels { get; set; } = default!;

        public YesNoFilterEnum PositionFilter
        {
            get => Service.PositionFilter;
            set
            {
                if(value != Service.PositionFilter)
                {
                    Service.PositionFilter = value;
                    OnPropertyChanged(() => PositionFilter);
                }
            }
        }
        public YesNoFilterEnum SalaryFilter
        {
            get => Service.SalaryFilter;
            set
            {
                if(value != Service.SalaryFilter)
                {
                    Service.SalaryFilter = value;
                    OnPropertyChanged(() => SalaryFilter);
                }
            }
        }
        #endregion

        public PromotionsViewModel() : base("Promotions") 
        {
            InitializeFiltersForSelectingOptions();
        }

        #region Overrided Abstract Methods
        protected override void CreateNewWindow()
        {
            WeakReferenceMessenger.Default.Send<OpenViewMessage>(new OpenViewMessage()
            {
                Sender = this,
                ViewModelToBeOpened = new PromotionViewModel()
            });
        }

        protected override void CleanFilters()
        {
            EmployeeName = "";
            EmployeeSurname = "";
            PositionFilter = SelectingFilterModels.First(item => item.SelectedOption == YesNoFilterEnum.NoFilter).SelectedOption;
            SalaryFilter = SelectingFilterModels.First(item => item.SelectedOption == YesNoFilterEnum.NoFilter).SelectedOption;
            DateFrom = null;
            DateTo = null;
            SortProperty = SortOptions.FirstOrDefault()?.PropertyTitle;
            IsOrderDescending = false;
            Refresh();
        }
        #endregion

        #region Methods
        private void InitializeFiltersForSelectingOptions()
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
                    OptionName = "Assigned new",
                    SelectedOption = YesNoFilterEnum.Yes,
                },
                new()
                {
                    OptionName = "Not changed",
                    SelectedOption = YesNoFilterEnum.No,
                }
            };
            PositionFilter = SelectingFilterModels.First(item => item.SelectedOption == YesNoFilterEnum.NoFilter).SelectedOption;
            SalaryFilter = SelectingFilterModels.First(item => item.SelectedOption == YesNoFilterEnum.NoFilter).SelectedOption;

            Refresh();
        }
        #endregion

    }
}
