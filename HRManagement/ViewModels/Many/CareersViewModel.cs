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
    public class CareersViewModel : BaseManyViewModel<CareerService, Career, CareerDTO>
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
        public string PositionName
        {
            get => Service.PositionName ?? "";
            set
            {
                if (value != Service.PositionName)
                {
                    Service.PositionName = value;
                    OnPropertyChanged(() => PositionName);
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

        public CareersViewModel() : base("Careers") { }


        protected override void CreateNewWindow()
        {
            WeakReferenceMessenger.Default.Send<OpenViewMessage>(new OpenViewMessage()
            {
                Sender = this,
                ViewModelToBeOpened = new CareerViewModel()
            });
        }
        protected override void CleanFilters()
        {
            TitleName = "";
            PositionName = "";
            DateFrom = null;
            DateTo = null;
            SortProperty = SortOptions.FirstOrDefault()?.PropertyTitle;
            IsOrderDescending = false;
            Refresh();
        }

    }
}
