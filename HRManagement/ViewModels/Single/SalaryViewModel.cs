using HRManagement.Helpers;
using HRManagement.Models.BusinessLogic;
using HRManagement.Models.Contexts;
using HRManagement.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using Microsoft.IdentityModel.Tokens;
using HRManagement.Models;
using HRManagement.Models.Services;

namespace HRManagement.ViewModels.Single
{
    public class SalaryViewModel : CreationWithEmployeeSelectorViewModel<SalaryWithEmployeeService, Salary, SalaryWithEmployeeDTO>
    {

        #region FieldsAndProperties

        #region Salary Properties
        public string BruttoPrice
        {
            get => Decimal.Round(Model.BruttoPrice, 2) + "";
            set
            {
                if (value != Model.BruttoPrice + "")
                {
                    MessageColor = "Red";
                    if (string.IsNullOrEmpty(value))
                    {
                        Model.BruttoPrice = 0.00m;
                        OnPropertyChanged(() => BruttoPrice);
                    }
                    if (Regex.IsMatch(value, @"^\d+([\.,]\d{1,2})?$"))
                    {
                        try
                        {
                            Decimal brutto = Decimal.Parse(value);
                            if (brutto >= 0)
                            {
                                Model.BruttoPrice = brutto;
                                OnPropertyChanged(() => BruttoPrice);
                                OnPropertyChanged(() => CalculatedNetto);
                                MessageLabel = "";
                            }
                            else
                            {
                                MessageLabel = "Brutto price cannot be negative";
                                BruttoPrice = "";
                            }
                        }
                        catch
                        {
                            MessageLabel = "Invalid Brutto price value!";
                            BruttoPrice = "";
                        }
                    }
                    else
                    {
                        MessageLabel = "Invalid Brutto price! - The value has to be a number! Brutto price is set to 0 now!";
                        Model.BruttoPrice = 0.00m;
                    }
                }
            }
        }
        public string TaxRate
        {
            get => Double.Round(Model.TaxRate*100,2) + "";
            set
            {
                if (value != Model.TaxRate + "")
                {
                    MessageColor = "Red";
                    if (string.IsNullOrEmpty(value))
                    {
                        Model.TaxRate = 0.00d;
                        OnPropertyChanged(() => TaxRate);
                    }
                    if (Regex.IsMatch(value, @"^\d+([\.,]\d{1,2})?$"))
                    {
                        try
                        {
                            Double tax = Double.Parse(value);
                            if (tax >= 0 && tax <= 100)
                            {
                                Model.TaxRate = tax/100;
                                OnPropertyChanged(() => TaxRate);
                                OnPropertyChanged(() => CalculatedNetto);
                                MessageLabel = "";
                            }
                            else
                            {
                                MessageLabel = "Tax rate has to be between 0 to 100% !";
                                TaxRate = "";
                            }
                        }
                        catch
                        {
                            MessageLabel = "Invalid Tax rate value!";
                            TaxRate = "";
                        }
                    }
                    else
                    {
                        MessageLabel = "Invalid Tax rate value! - The value has to be a number between 0 and 100%. Tax rate is set to 0 now!";
                        Model.TaxRate = 0.00d;
                    }
                }
            }
        }
        public string ZusTaxRate
        {
            get => Double.Round(Model.ZusTaxRate*100, 2) + "";
            set
            {
                if (value != Model.ZusTaxRate + "")
                {
                    MessageColor = "Red";
                    if (string.IsNullOrEmpty(value))
                    {
                        Model.ZusTaxRate = 0.00d;
                        OnPropertyChanged(() => ZusTaxRate);
                    }
                    if (Regex.IsMatch(value, @"^\d+([\.,]\d{1,2})?$"))
                    {
                        try
                        {
                            Double zus = Double.Parse(value);
                            if (zus >= 0 && zus <= 100)
                            {
                                Model.ZusTaxRate = zus/100;
                                OnPropertyChanged(() => ZusTaxRate);
                                OnPropertyChanged(() => CalculatedNetto);
                                MessageLabel = "";
                            }
                            else
                            {
                                MessageLabel = "Zus tax rate has to be between 0 to 100% !";
                                ZusTaxRate = "";
                            }
                        }
                        catch
                        {
                            MessageLabel = "Invalid Zus tax rate value!";
                            ZusTaxRate = "";
                        }
                    }
                    else
                    {
                        MessageLabel = "Invalid Zus Tax rate value! - The value has to be a number between 0 and 100%. Zus Tax rate is set to 0 now!";
                        Model.ZusTaxRate = 0.00d;
                    }
                }
            }
        }
        public string Declusions
        {
            get => Decimal.Round(Model.Declusions, 2) + "";
            set
            {
                if (value != Model.Declusions + "")
                {
                    MessageColor = "Red";
                    if (string.IsNullOrEmpty(value))
                    {
                        Model.Declusions = 0.00m;
                        OnPropertyChanged(() => Declusions);
                    }
                    if (Regex.IsMatch(value, @"^\d+([\.,]\d{1,2})?$"))
                    {
                        try
                        {
                            Decimal declusions = Decimal.Parse(value);
                            if (declusions >= 0)
                            {
                                Model.Declusions = declusions;
                                OnPropertyChanged(() => Declusions);
                                OnPropertyChanged(() => CalculatedNetto);
                                MessageLabel = "";
                            }
                            else
                            {
                                MessageLabel = "Declusions cannot be negative";
                                Declusions = "";
                            }
                        }
                        catch
                        {
                            MessageLabel = "Invalid declusions value!";
                            Declusions = "";
                        }
                    }
                    else
                    {
                        MessageLabel = "Invalid Declusions! - The value has to be a number! Declusions are set to 0 now!";
                        Model.Declusions = 0.00m;
                    }
                }
            }
        }
        public string? AdditionalDescription
        {
            get => Model.Description;
            set
            {
                if (value != Model.Description)
                {
                    Model.Description = value;
                    OnPropertyChanged(() => AdditionalDescription);
                }
            }
        }
        public string CalculatedNetto => Service.GetStringOfCalculatedNetto(Model);
        #endregion

        #endregion

        public SalaryViewModel() : base("Salary")
        {

        }

        #region Abstract functions

        public override void Save()
        {
            MessageColor = "Red";
            if (SelectedEmployeeModel == null)
            {
                MessageLabel = "Select employee firstly!";
                return;
            }

            if (string.IsNullOrEmpty(BruttoPrice) || BruttoPrice.Equals("0,00") || Declusions.Equals("0"))
            {
                MessageLabel = "Enter Brutto Price!";
                return;
            }

            if (string.IsNullOrEmpty(TaxRate))
            {
                MessageLabel = "Enter Tax Rate!";
                return;
            }

            if (string.IsNullOrEmpty(ZusTaxRate))
            {
                MessageLabel = "Enter Zus Tax Rate!";
                return;
            }

            if (string.IsNullOrEmpty(Declusions))
            {
                MessageLabel = "Enter Declusions!";
                return;
            }
            if (Service.GetCalculatedNetto(Model) < 0)
            {
                MessageLabel = "Netto value has to be equal or greater than 0!";
                return;
            }

            Service.UpdateModel(Model);

            MessageColor = "Green";
            MessageLabel = "Changed successfully";

        }
        protected override void UpdateFormFields()
        {
            if (SelectedEmployeeModel != null && EmployeeModel != null)
            {
                Model = Service.GetModel(EmployeeModel.SalaryId);
                OnPropertyChanged(() => BruttoPrice);
                OnPropertyChanged(() => TaxRate);
                OnPropertyChanged(() => ZusTaxRate);
                OnPropertyChanged(() => Declusions);
                OnPropertyChanged(() => AdditionalDescription);
                OnPropertyChanged(() => CalculatedNetto);
            }
        }

        #endregion
    }
}
