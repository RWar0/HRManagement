using HRManagement.Helpers;
using HRManagement.Models;
using HRManagement.Models.BusinessLogic;
using HRManagement.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRManagement.ViewModels.Single
{
    public class SalaryCalculatorViewModel : BaseServiceViewModel<SalaryService, Salary, SalaryBL>
    {
        #region FieldsAndProperties

        #region Messages
        private string _messageLabel = "";
        public string MessageLabel
        {
            get => _messageLabel;
            set
            {
                if (value != _messageLabel)
                {
                    _messageLabel = value;
                    OnPropertyChanged(() => MessageLabel);
                }
            }
        }
        private string _messageColor = "Red";
        public string MessageColor
        {
            get => _messageColor;
            set
            {
                if (value != _messageColor)
                {
                    _messageColor = value;
                    OnPropertyChanged(() => MessageColor);
                }
            }
        }
        #endregion

        #region Commands
        public ICommand CleanButton { get; set; }
        public ICommand ChangeValueButton { get; set; }
        #endregion

        #region Model
        public Salary Model { get; set; } = default!;
        #endregion

        #region Model Properties
        public string EnteringValue
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
                        OnPropertyChanged(() => EnteringValue);
                    }
                    if (Regex.IsMatch(value, @"^\d+([\.,]\d{1,2})?$"))
                    {
                        try
                        {
                            Decimal brutto = Decimal.Parse(value);
                            if (brutto >= 0)
                            {
                                Model.BruttoPrice = brutto;
                                OnPropertyChanged(() => EnteringValue);
                                OnPropertyChanged(() => CalculatedValue);
                                MessageLabel = "";
                            }
                            else
                            {
                                MessageLabel = $"{EnteringValueTitle} cannot be negative";
                                EnteringValue = "";
                            }
                        }
                        catch
                        {
                            MessageLabel = $"Invalid {EnteringValueTitle} value!";
                            EnteringValue = "";
                        }
                    }
                    else
                    {
                        MessageLabel = $"Invalid {EnteringValueTitle}! - The value has to be a number! {EnteringValueTitle} is set to 0 now!";
                        Model.BruttoPrice = 0.00m;
                    }
                }
            }
        }
        public string TaxRate
        {
            get => Double.Round(Model.TaxRate * 100, 2) + "";
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
                                Model.TaxRate = tax / 100;
                                OnPropertyChanged(() => TaxRate);
                                OnPropertyChanged(() => CalculatedValue);
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
            get => Double.Round(Model.ZusTaxRate * 100, 2) + "";
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
                                Model.ZusTaxRate = zus / 100;
                                OnPropertyChanged(() => ZusTaxRate);
                                OnPropertyChanged(() => CalculatedValue);
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
                                OnPropertyChanged(() => CalculatedValue);
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
        public string CalculatedValue
        {
            get
            {
                if(IsBrutto)
                {
                    return Service.GetStringOfCalculatedNetto(Model);
                }
                else
                {
                    try
                    {
                        decimal tempNetto = decimal.Parse(EnteringValue);
                        return Service.GetCalculatedBrutto(Model, tempNetto) + "";
                    }
                    catch (Exception)
                    {
                        return "-1";
                    }
                }
            }
        }
        #endregion

        #region Additional fields
        public bool IsBrutto { get; set; }
        public string NettoOpacity => IsBrutto ? "0.3" : "1";
        public bool IsNettoEnable => !IsBrutto;
        public string BruttoOpacity => IsBrutto ? "1" : "0.3";
        public bool IsBruttoEnable => IsBrutto;
        public string EnteringValueTitle => IsBrutto ? "Brutto price" : "Netto price";
        public string CalculatedValueTitle => IsBrutto ? "Calculated netto:" : "Calculated brutto:";
        #endregion

        #endregion

        public SalaryCalculatorViewModel() : base("Salary Calculator")
        {
            IsBrutto = true;
            Model = Service.CreateModel();
            CleanButton = new BaseCommand(() => CleanFields());
            ChangeValueButton = new BaseCommand(() => ChangeValue());
        }

        #region Methods
        private void CleanFields()
        {
            EnteringValue = "0";
            TaxRate = "0";
            ZusTaxRate = "0";
            Declusions = "0";
            MessageLabel = "";
        }
        private void ChangeValue()
        {
            IsBrutto = !IsBrutto;
            OnPropertyChanged(() => IsBrutto);
            OnPropertyChanged(() => NettoOpacity);
            OnPropertyChanged(() => IsNettoEnable);
            OnPropertyChanged(() => BruttoOpacity);
            OnPropertyChanged(() => IsBruttoEnable);
            OnPropertyChanged(() => EnteringValueTitle);
            OnPropertyChanged(() => CalculatedValueTitle);
            CleanFields();
        }
        #endregion

    }
}
