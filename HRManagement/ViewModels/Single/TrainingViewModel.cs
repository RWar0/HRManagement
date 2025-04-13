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
    public class TrainingViewModel : PropertySelectorWithEmployeesViewModel<TrainingService, EmployeeTrainingService, EmployeeTraining, EmployeeTrainingDTO, Training, TrainingDTO>
    {

        #region FieldsAndProperties

        #region Training Properties
        public string Title
        {
            get => Model.Title;
            set
            {
                if (value != Model.Title)
                {
                    Model.Title = value;
                    OnPropertyChanged(() => Title);
                }
            }
        }
        public string Description
        {
            get => Model.Description;
            set
            {
                if (value != Model.Description)
                {
                    Model.Description = value;
                    OnPropertyChanged(() => Description);
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

        #endregion

        public TrainingViewModel() : base("Training")
        {
            SelectPropertyModelCommand = new BaseCommand(() => WindowManager.OpenWindow(new TrainingsWithCallbackViewModel(this)));
        }

        #region Overrided Abstract Methods

        protected override void UpdateFormFields()
        {
            OnPropertyChanged(() => Title);
            OnPropertyChanged(() => Description);
            OnPropertyChanged(() => BeginDate);
            OnPropertyChanged(() => EndDate);
        }

        protected override void ValidationOfData()
        {
            if (string.IsNullOrEmpty(Title))
            {
                MessageLabel = "Enter Title of the training!";
                return;
            }
            if (string.IsNullOrEmpty(Description))
            {
                MessageLabel = "Enter Description of the training!";
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
            PropertyDisplay = Model.Title;
        }

        #endregion

    }
}
