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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRManagement.ViewModels.Single
{
    public class BonusViewModel : PropertySelectorWithEmployeesViewModel<BonusService, EmployeeBonusService, EmployeeBonus, EmployeeBonusDTO, Bonus, BonusDTO>
    {
        #region FieldsAndProperties

        #region Bonus Properties
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
        public string? Description
        {
            get => Model.Description ?? "-----";
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
        public string Price
        {
            get => Decimal.Round(Model.Price, 2) + "";
            set
            {
                if (value != Model.Price + "")
                {
                    MessageColor = "Red";
                    if (string.IsNullOrEmpty(value))
                    {
                        Model.Price = 0.00m;
                        OnPropertyChanged(() => Price);
                    }
                    if (Regex.IsMatch(value, @"^\d+([\.,]\d{1,2})?$"))
                    {
                        try
                        {
                            Decimal brutto = Decimal.Parse(value);
                            if (brutto >= 0)
                            {
                                Model.Price = brutto;
                                OnPropertyChanged(() => Price);
                                MessageLabel = "";
                            }
                            else
                            {
                                MessageLabel = "Price cannot be negative";
                                Price = "";
                            }
                        }
                        catch
                        {
                            MessageLabel = "Invalid Price value!";
                            Price = "";
                        }
                    }
                    else
                    {
                        MessageLabel = "Invalid Price! - The value has to be a number! Price is set to 0 now!";
                        Model.Price = 0.00m;
                    }
                }
            }
        }
        #endregion

        #endregion
        public BonusViewModel() : base("Bonus")
        {
            SelectPropertyModelCommand = new BaseCommand(() => WindowManager.OpenWindow(new BonusesWithCallbackViewModel(this)));
        }

        #region Overrided Abstract Methods

        protected override void UpdateFormFields()
        {
            OnPropertyChanged(() => Title);
            OnPropertyChanged(() => Description);
            OnPropertyChanged(() => BeginDate);
            OnPropertyChanged(() => EndDate);
            OnPropertyChanged(() => Price);
        }

        protected override void ValidationOfData()
        {
            if (string.IsNullOrEmpty(Title))
            {
                MessageLabel = "Enter Bonus Title";
                return;
            }
            if (string.IsNullOrEmpty(Price))
            {
                MessageLabel = "Enter Price of the bonus";
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
