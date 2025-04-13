using CommunityToolkit.Mvvm.Messaging;
using HRManagement.Helpers;
using HRManagement.Models;
using HRManagement.Models.Contexts;
using HRManagement.Models.DTOs;
using HRManagement.Models.Services;
using HRManagement.ViewModels.Single;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.ViewModels.Many
{
    public class BonusesViewModel : BaseManyViewModel<BonusService, Bonus, BonusDTO>
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

        public BonusesViewModel() : base("Bonuses") { }

        protected override void CreateNewWindow()
        {
            WeakReferenceMessenger.Default.Send<OpenViewMessage>(new OpenViewMessage()
            {
                Sender = this,
                ViewModelToBeOpened = new BonusViewModel()
            });
        }

        protected override void CleanFilters()
        {
            TitleName = "";
            DescriptionName = "";
            PriceTo = "";
            PriceFrom = "";
            DateFrom = null;
            DateTo = null;
            SortProperty = SortOptions.FirstOrDefault()?.PropertyTitle;
            IsOrderDescending = false;
            Refresh();
        }
    }
}
