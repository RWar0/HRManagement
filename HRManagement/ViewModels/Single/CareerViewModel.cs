using HRManagement.Helpers;
using HRManagement.Models;
using HRManagement.Models.Contexts;
using HRManagement.Models.DTOs;
using HRManagement.Models.ModalWindows;
using HRManagement.Models.Services;
using HRManagement.ViewModels.Many;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRManagement.ViewModels.Single
{
    public class CareerViewModel : PropertySelectorWithEmployeesViewModel<CareerService, EmployeeCareerService, EmployeeCareer, EmployeeCareerDTO, Career, CareerDTO>
    {

        #region FieldsAndProperties
        public string Position
        {
            get => Model.Position;
            set
            {
                if(value != Model.Position)
                {
                    Model.Position = value;
                    OnPropertyChanged(() => Position);
                }
            }
        }
        public string Place
        {
            get => Model.Title;
            set
            {
                if (value != Model.Title)
                {
                    Model.Title = value;
                    OnPropertyChanged(() => Place);
                }
            }
        }
        public DateTime BeginDate
        {
            get => Model.BeginDate.ToDateTime(TimeOnly.MinValue);
            set
            {
                if (value != Model.BeginDate.ToDateTime(TimeOnly.MinValue))
                {
                    Model.BeginDate = DateOnly.FromDateTime(value);
                    OnPropertyChanged(() => BeginDate);
                }
            }
        }
        public DateTime EndDate
        {
            get => Model.EndDate.ToDateTime(TimeOnly.MinValue);
            set
            {
                if (value != Model.EndDate.ToDateTime(TimeOnly.MinValue))
                {
                    Model.EndDate = DateOnly.FromDateTime(value);
                    OnPropertyChanged(() => EndDate);
                }
            }
        }
        #endregion


        public CareerViewModel() : base("Career")
        {
            SelectPropertyModelCommand = new BaseCommand(() => WindowManager.OpenWindow(new CareersWithCallbackViewModel(this)));
        }

        #region Overrided Abstract Methods
        protected override void UpdateFormFields()
        {
            OnPropertyChanged(() => Position);
            OnPropertyChanged(() => Place);
            OnPropertyChanged(() => BeginDate);
            OnPropertyChanged(() => EndDate);
        }
        protected override void ValidationOfData()
        {
            if (string.IsNullOrEmpty(Place))
            {
                MessageLabel = "Enter Career Place";
                return;
            }
            if (string.IsNullOrEmpty(Position))
            {
                MessageLabel = "Enter Position of Career";
                return;
            }
            if (BeginDate > EndDate)
            {
                MessageLabel = "Begin Date cannot be greater than End Date!";
                return;
            }
        }
        protected override void UpdatePropertyDisplay()
        {
            PropertyDisplay = Model.Id > 0 ? $"{Model.Title} | {Model.Position}" : "Creating new";
        }
        #endregion
    }
}
