using CommunityToolkit.Mvvm.Messaging;
using HRManagement.Helpers;
using HRManagement.Models;
using HRManagement.Models.BusinessLogic;
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
    public class PromotionViewModel : CreationAndEditionWithEmployeeSelectorViewModel<PromotionService, Promotion, PromotionDTO>
    {

        #region FieldsAndProperties

        #region Commands
        public ICommand SelectPositionCommand { get; set; }
        #endregion

        #region Additional Services
        private PositionService OldPositionService { get; set; }
        private PositionService NewPositionService { get; set; }
        private SalaryService OldSalaryService { get; set; }
        private SalaryService NewSalaryService { get; set; }
        #endregion

        #region Additional Messages
        private string _employeeMessageColor = "Red";
        public string EmployeeMessageColor
        {
            get => _employeeMessageColor;
            set
            {
                if (value != _employeeMessageColor)
                {
                    _employeeMessageColor = value;
                    OnPropertyChanged(() => EmployeeMessageColor);
                }
            }
        }
        private string _employeeMessageLabel = "";
        public string EmployeeMessageLabel
        {
            get => _employeeMessageLabel;
            set
            {
                if (value != _employeeMessageLabel)
                {
                    _employeeMessageLabel = value;
                    OnPropertyChanged(() => EmployeeMessageLabel);
                }
            }
        }
        #endregion

        #region UserControl Fields And Properties

        // New Position
        private bool _isSelectedAssignNewPosition = false;
        public bool IsSelectedAssignNewPosition
        {
            get => _isSelectedAssignNewPosition;
            set
            {
                if (value != _isSelectedAssignNewPosition)
                {
                    _isSelectedAssignNewPosition = value;
                    OnPropertyChanged(() => IsSelectedAssignNewPosition);
                    OnPropertyChanged(() => IsAssignNewPositionEnable);
                    OnPropertyChanged(() => AssignNewPositionOppacity);
                    if (!value)
                    {
                        IsSelectedCreateNewPosition = false;
                        PositionName = "";
                        DepartmentName = "";
                        NewPositionModel = NewPositionService.CreateModel();
                    }
                }
            }
        }

        public bool IsAssignNewPositionEnable => IsSelectedAssignNewPosition;
        public string AssignNewPositionOppacity => IsAssignNewPositionEnable ? "1" : "0.5";

        // New Salary
        private bool _isSelectedAssignNewSalary = false;
        public bool IsSelectedAssignNewSalary
        {
            get => _isSelectedAssignNewSalary;
            set
            {
                if (value != _isSelectedAssignNewSalary)
                {
                    _isSelectedAssignNewSalary = value;
                    OnPropertyChanged(() => IsSelectedAssignNewSalary);
                    OnPropertyChanged(() => IsAssignNewSalaryEnable);
                    OnPropertyChanged(() => AssignNewSalaryOppacity);
                }
            }
        }
        public bool IsAssignNewSalaryEnable => IsSelectedAssignNewSalary;
        public string AssignNewSalaryOppacity => IsAssignNewSalaryEnable ? "1" : "0.5";
        #endregion

        #region Old Position
        private string _oldPositionTitle = default!;
        public string OldPositionTitle 
        {
            get => _oldPositionTitle;
            set
            {
                if(value != _oldPositionTitle)
                {
                    _oldPositionTitle = value;
                    OnPropertyChanged(() => OldPositionTitle);
                }
            }
        }
        private string _oldPositionDepartment = default!;
        public string? OldPositionDepartment
        {
            get => _oldPositionDepartment;
            set
            {
                if (value != _oldPositionDepartment)
                {
                    _oldPositionDepartment = value ?? "-----";
                    OnPropertyChanged(() => OldPositionDepartment);
                }
            }
        }
        #endregion

        #region Old Salary
        public Salary OldSalaryModel
        {
            get => Model.OldSalary;
            set
            {
                if (value != Model.OldSalary)
                {
                    Model.OldSalary = value;
                    OnPropertyChanged(() => OldSalaryModel);
                }
            }
        }

        private SalaryBL _oldSalaryBL = default!;
        public SalaryBL OldSalaryBL
        {
            get => _oldSalaryBL;
            set
            {
                if (value != _oldSalaryBL)
                {
                    _oldSalaryBL = value;
                    OnPropertyChanged(() => OldSalaryBL);
                }
            }
        }
        #endregion

        #region NewSalary Properties
        private Salary NewSalaryModel { get; set; }
        public string BruttoPrice
        {
            get => Decimal.Round(NewSalaryModel.BruttoPrice, 2) + "";
            set
            {
                if (value != NewSalaryModel.BruttoPrice + "")
                {
                    MessageColor = "Red";
                    if (string.IsNullOrEmpty(value))
                    {
                        NewSalaryModel.BruttoPrice = 0.00m;
                        OnPropertyChanged(() => BruttoPrice);
                    }
                    if (Regex.IsMatch(value, @"^\d+([\.,]\d{1,2})?$"))
                    {
                        try
                        {
                            Decimal brutto = Decimal.Parse(value);
                            if (brutto >= 0)
                            {
                                NewSalaryModel.BruttoPrice = brutto;
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
                        NewSalaryModel.BruttoPrice = 0.00m;
                    }
                }
            }
        }

        public string TaxRate
        {
            get => Double.Round(NewSalaryModel.TaxRate * 100, 2) + "";
            set
            {
                if (value != NewSalaryModel.TaxRate + "")
                {
                    MessageColor = "Red";
                    if (string.IsNullOrEmpty(value))
                    {
                        NewSalaryModel.TaxRate = 0.00d;
                        OnPropertyChanged(() => TaxRate);
                    }
                    if (Regex.IsMatch(value, @"^\d+([\.,]\d{1,2})?$"))
                    {
                        try
                        {
                            Double tax = Double.Parse(value);
                            if (tax >= 0 && tax <= 100)
                            {
                                NewSalaryModel.TaxRate = tax / 100;
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
                        NewSalaryModel.TaxRate = 0.00d;
                    }
                }
            }
        }

        public string ZusTaxRate
        {
            get => Double.Round(NewSalaryModel.ZusTaxRate * 100, 2) + "";
            set
            {
                if (value != NewSalaryModel.ZusTaxRate + "")
                {
                    MessageColor = "Red";
                    if (string.IsNullOrEmpty(value))
                    {
                        NewSalaryModel.ZusTaxRate = 0.00d;
                        OnPropertyChanged(() => ZusTaxRate);
                    }
                    if (Regex.IsMatch(value, @"^\d+([\.,]\d{1,2})?$"))
                    {
                        try
                        {
                            Double zus = Double.Parse(value);
                            if (zus >= 0 && zus <= 100)
                            {
                                NewSalaryModel.ZusTaxRate = zus / 100;
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
                        NewSalaryModel.ZusTaxRate = 0.00d;
                    }
                }
            }
        }

        public string Declusions
        {
            get => Decimal.Round(NewSalaryModel.Declusions, 2) + "";
            set
            {
                if (value != NewSalaryModel.Declusions + "")
                {
                    MessageColor = "Red";
                    if (string.IsNullOrEmpty(value))
                    {
                        NewSalaryModel.Declusions = 0.00m;
                        OnPropertyChanged(() => Declusions);
                    }
                    if (Regex.IsMatch(value, @"^\d+([\.,]\d{1,2})?$"))
                    {
                        try
                        {
                            Decimal declusions = Decimal.Parse(value);
                            if (declusions >= 0)
                            {
                                NewSalaryModel.Declusions = declusions;
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
                        NewSalaryModel.Declusions = 0.00m;
                    }
                }
            }
        }

        public string? AdditionalDescription
        {
            get => NewSalaryModel.Description;
            set
            {
                if (value != NewSalaryModel.Description)
                {
                    NewSalaryModel.Description = value;
                    OnPropertyChanged(() => AdditionalDescription);
                }
            }
        }
        public string CalculatedNetto
        {
            get
            {
                if (!string.IsNullOrEmpty(BruttoPrice) && !string.IsNullOrEmpty(TaxRate) && !string.IsNullOrEmpty(ZusTaxRate) && !string.IsNullOrEmpty(Declusions))
                {
                    try
                    {
                        Decimal brutto = Decimal.Parse(BruttoPrice);
                        Decimal tax = Decimal.Parse(TaxRate);
                        Decimal zusTax = Decimal.Parse(ZusTaxRate);
                        Decimal declusions = Decimal.Parse(Declusions);

                        Decimal netto = (brutto * (1 - zusTax / 100)) * (1 - tax / 100) - declusions;
                        return Decimal.Round(netto, 2) + "";
                    }
                    catch
                    {
                        return "N/A";
                    }
                }
                return "N/A";
            }
        }
        #endregion

        #region New Position - Properties
        public Position NewPositionModel { get; set; }
        public string PositionName
        {
            get => NewPositionModel.Title;
            set
            {
                if (value != NewPositionModel.Title)
                {
                    NewPositionModel.Title = value;
                    OnPropertyChanged(() => PositionName);
                }
            }
        }
        public string? DepartmentName
        {
            get => NewPositionModel.DepartmentName;
            set
            {
                if (value != NewPositionModel.DepartmentName)
                {
                    NewPositionModel.DepartmentName = value;
                    OnPropertyChanged(() => DepartmentName);
                }
            }
        }

        private bool _isCheckedDepartmentCheckBox = false;
        public bool IsCheckedDepartmentCheckBox
        {
            get => _isCheckedDepartmentCheckBox;
            set
            {
                if (value != _isCheckedDepartmentCheckBox)
                {
                    _isCheckedDepartmentCheckBox = value;
                    OnPropertyChanged(() => IsCheckedDepartmentCheckBox);
                    OnPropertyChanged(() => IsActiveDepartmentName);
                    OnPropertyChanged(() => DepartmentOpacity);

                    if (IsCheckedDepartmentCheckBox)
                    {
                        DepartmentName = "";
                    }

                }
            }
        }

        public bool IsActiveDepartmentName => !IsCheckedDepartmentCheckBox;
        public string DepartmentOpacity => IsActiveDepartmentName ? "1" : "0.5";
        #endregion

        #region PositionsModels
        private PositionDTO? _selectedPositionModel;
        public PositionDTO? SelectedPositionModel
        {
            get => _selectedPositionModel;
            set
            {
                if (value != _selectedPositionModel && value != null)
                {
                    _selectedPositionModel = value;
                    OnPropertyChanged(() => SelectedPositionModel);
                    NewPositionModel = NewPositionService.GetModel(value.Id);
                }
            }
        }

        private string? _selectedPositionName;
        public string? SelectedPositionName
        {
            get => _selectedPositionName ?? "Position not selected";
            set
            {
                if (value != _selectedPositionName)
                {
                    _selectedPositionName = value;
                    OnPropertyChanged(() => SelectedPositionName);
                }
            }
        }
        #endregion

        #region New Position - Additional Properties
        private bool _isSelectedCreateNewPosition = false;
        public bool IsSelectedCreateNewPosition
        {
            get => _isSelectedCreateNewPosition;
            set
            {
                if (value != _isSelectedCreateNewPosition)
                {
                    _isSelectedCreateNewPosition = value;
                    OnPropertyChanged(() => IsSelectedCreateNewPosition);
                    OnPropertyChanged(() => IsReverseSelectedCreateNewPosition);
                    OnPropertyChanged(() => PositionSelectionCheckBoxOpacity);
                    OnPropertyChanged(() => PositionSelectionCreatorOpacity);

                    if (IsSelectedCreateNewPosition)
                    {
                        SelectedPositionModel = null;
                    }
                    else
                    {
                        PositionName = "";
                        DepartmentName = "";
                    }
                }
            }
        }

        public bool IsReverseSelectedCreateNewPosition => !_isSelectedCreateNewPosition;
        public string PositionSelectionCheckBoxOpacity => IsReverseSelectedCreateNewPosition ? "1" : "0.3";
        public string PositionSelectionCreatorOpacity => IsSelectedCreateNewPosition ? "1" : "0.3";
        #endregion

        public DateTime PromotionDate
        {
            get => Model.PromotionDate.ToDateTime(TimeOnly.MinValue);
            set
            {
                if (value != Model.PromotionDate.ToDateTime(TimeOnly.MinValue))
                {
                    Model.PromotionDate = DateOnly.FromDateTime(value);
                    OnPropertyChanged(() => PromotionDate);
                }
            }
        }

        #endregion

        public PromotionViewModel() : base("Promotion")
        {
            OldSalaryService = new SalaryService();
            NewSalaryService = new SalaryService();
            OldPositionService = new PositionService();
            NewPositionService = new PositionService();

            NewSalaryModel = NewSalaryService.CreateModel();
            NewPositionModel = NewPositionService.CreateModel();
            OldSalaryBL = OldSalaryService.GetEmptyBLModel();

            // Handling Selecting and Recieving of Position
            SelectPositionCommand = new BaseCommand(() => WindowManager.OpenWindow(new PositionsWithCallbackViewModel(this)));
            WeakReferenceMessenger.Default.Register<SelectedObjectMessage<PositionDTO>>(this, (recipient, message) => GetSelectedPosition(message));
        }

        #region Overrided Abstract methods
        protected override void UpdateFormFields()
        {
            if (SelectedEmployeeModel != null && EmployeeModel != null)
            {
                NewSalaryModel = NewSalaryService.CreateModel();
                NewPositionModel = NewPositionService.CreateModel();

                Model.EmployeeId = EmployeeModel.Id;

                Position oldPositionTemp = OldPositionService.GetModel(EmployeeModel.PositionId);
                Model.OldPositionId = oldPositionTemp.Id;
                OldPositionTitle = oldPositionTemp.Title;
                OldPositionDepartment = oldPositionTemp.DepartmentName;

                Model.OldSalaryId = OldSalaryService.GetModel(EmployeeModel.SalaryId).Id;
                OldSalaryBL = OldSalaryService.GetModelOfBL(EmployeeModel.SalaryId);

                OnPropertyChanged(() => PromotionDate);
                OnPropertyChanged(() => OldSalaryModel);
                OnPropertyChanged(() => OldSalaryBL);
            }
            else
            {
                CleanForm();
            }
        }
        public override void Save()
        {
            MessageColor = "Red";
            MessageLabel = "";

            // Checking if employee is selected
            if (SelectedEmployeeModel == null || EmployeeModel == null)
            {
                MessageLabel = "Select employee firstly";
                return;
            }

            // Checking if entered data in position when selected
            if (IsSelectedAssignNewPosition)
            {
                if (!IsSelectedCreateNewPosition)
                {
                    if (SelectedPositionModel == null)
                    {
                        MessageLabel = "Select Position or Create New or Unselect 'Assign New Position'!";
                        return;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(PositionName))
                    {
                        MessageLabel = "Enter Position Name or unselect 'Create new position' and Choose existing one or Unselect 'Assign new position'!";
                        return;
                    }
                    if (IsActiveDepartmentName && string.IsNullOrEmpty(DepartmentName))
                    {
                        MessageLabel = "Enter Department Name or Select 'Do not assign department'!";
                        return;
                    }
                }
            }
            // Checking if entered data in salary when selected
            if (IsSelectedAssignNewSalary)
            {
                if (string.IsNullOrEmpty(BruttoPrice))
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
            }

            // Checking if anything done
            if (!(IsSelectedAssignNewPosition || IsSelectedAssignNewSalary))
            {
                MessageLabel = "To create promotion it is necessary to select at least one of assign new data!";
                return;
            }

            // Assigning new Position or continue with old one
            if (IsSelectedAssignNewPosition)
            {
                if(IsSelectedCreateNewPosition)
                {
                    NewPositionService.AddModel(NewPositionModel);
                }
                Model.NewPositionId = NewPositionModel.Id;
                EmployeeModel.Position = NewPositionModel;
            }
            else
            {
                Model.NewPositionId = Model.OldPositionId;
            }

            // Assigning new Salary or continue with old one
            if (IsSelectedAssignNewSalary)
            {
                NewSalaryService.AddModel(NewSalaryModel);
                Model.NewSalaryId = NewSalaryModel.Id;
                EmployeeModel.Salary = NewSalaryModel;
            }
            else
            {
                Model.NewSalaryId = Model.OldSalaryId;
            }

            if (IsEditing)
            {
                CheckAndUpdateSalary();

                Service.UpdateModel(Model);
                OnRequestClose();
                WeakReferenceMessenger.Default.Send<OpenViewMessage>(new OpenViewMessage()
                {
                    Sender = this,
                    ViewModelToBeOpened = new PromotionsViewModel()
                });
            }
            else
            {
                Service.AddModel(Model);
                EmployeeService.UpdateModel(EmployeeModel);

                IsSelectedAssignNewPosition = false;
                IsSelectedAssignNewSalary = false;

                InitializeNewModelAndService();
                UpdateFormFields();
                MessageColor = "Green";
                MessageLabel = "Promotion added successfully";
            }
        }

        protected override void EditionFieldsUpdating()
        {
            SelectedEmployeeModel = EmployeeService.GetDtoModel(Model.EmployeeId);
            EmployeeDisplay = $"{SelectedEmployeeModel.Firstname} {SelectedEmployeeModel.Surname}";

            // Getting model again due to issue after changing selected employee model
            Model = Service.GetModel(Model.Id);

            OnPropertyChanged(() => PromotionDate);

            // Setting old position
            Position oldPositionTemp = OldPositionService.GetModel(Model.OldPositionId);
            OldPositionTitle = oldPositionTemp.Title;
            OldPositionDepartment = oldPositionTemp.DepartmentName;

            // Setting old salary
            OldSalaryBL = OldSalaryService.GetModelOfBL(Model.OldSalaryId);
            OnPropertyChanged(() => OldSalaryModel);
            OnPropertyChanged(() => OldSalaryBL);

            // Checking if new possition assigned
            IsSelectedAssignNewPosition = Model.NewPositionId != Model.OldPositionId;
            if(IsSelectedAssignNewPosition)
            {
                NewPositionModel = NewPositionService.GetModel(Model.NewPositionId);
                SelectedPositionModel = NewPositionService.GetDtoModel(Model.NewPositionId);
                SelectedPositionName = $"{SelectedPositionModel.Title} | {SelectedPositionModel.DepartmentName}";
            }

            // Ckecking if new salary assigned
            IsSelectedAssignNewSalary = Model.NewSalaryId != Model.OldSalaryId;
            if(IsSelectedAssignNewSalary)
            {
                NewSalaryModel = NewSalaryService.GetModel(Model.NewSalaryId);
                OnPropertyChanged(() => BruttoPrice);
                OnPropertyChanged(() => TaxRate);
                OnPropertyChanged(() => ZusTaxRate);
                OnPropertyChanged(() => Declusions);
                OnPropertyChanged(() => AdditionalDescription);
                OnPropertyChanged(() => CalculatedNetto);
            }

        }
        #endregion

        #region Methods
        private void GetSelectedPosition(SelectedObjectMessage<PositionDTO> message)
        {
            if (message.WhoRequestedToSelect == this)
            {
                SelectedPositionModel = message.SelectedObject;
                SelectedPositionName = $"{SelectedPositionModel.Title} | {SelectedPositionModel.DepartmentName}";
            }
        }
        private void CleanForm()
        {
            NewSalaryModel = NewSalaryService.CreateModel();
            NewPositionModel = NewPositionService.CreateModel();
            OldSalaryBL = OldSalaryService.GetEmptyBLModel();
            OldPositionTitle = "";
            OldPositionDepartment = "";
            SelectedPositionName = null;
        }

        // Method checks if new salary was changed and update if necessary
        private void CheckAndUpdateSalary()
        {
            Salary baseNewSalary = NewSalaryService.GetModel(Model.NewSalaryId); 
            bool doUpdate = false;

            if (baseNewSalary.BruttoPrice != convertSalaryDecimalField(BruttoPrice))
            {
                doUpdate = true;
            }
            if (!doUpdate && baseNewSalary.TaxRate != convertSalaryDoubleField(TaxRate))
            {
                doUpdate = true;
            }
            if (!doUpdate && baseNewSalary.ZusTaxRate != convertSalaryDoubleField(ZusTaxRate))
            {
                doUpdate = true;
            }
            if (!doUpdate && baseNewSalary.Declusions != convertSalaryDecimalField(Declusions))
            {
                doUpdate = true;
            }
            if (!doUpdate)
            {
                if(!string.IsNullOrEmpty(baseNewSalary.Description) && baseNewSalary.Description.Equals(AdditionalDescription)) 
                {
                    doUpdate = true;
                }
                if (string.IsNullOrEmpty(baseNewSalary.Description) && !string.IsNullOrEmpty(AdditionalDescription))
                {
                    doUpdate = true;
                }
            }

            if (doUpdate)
            {
                NewSalaryService.UpdateModel(NewSalaryModel);
            }
        }

        private decimal convertSalaryDecimalField(string value)
        {
            try
            {
                return decimal.Parse(value);
            }
            catch(Exception)
            {
                return -1;
            }
        }
        private double convertSalaryDoubleField(string value)
        {
            try
            {
                return double.Parse(value);
            }
            catch (Exception)
            {
                return -1;
            }
        }
        #endregion

    }
}
