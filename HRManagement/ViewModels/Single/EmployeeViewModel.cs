using CommunityToolkit.Mvvm.Messaging;
using HRManagement.Helpers;
using HRManagement.Models;
using HRManagement.Models.Contexts;
using HRManagement.Models.DTOs;
using HRManagement.Models.ModalWindows;
using HRManagement.Models.Services;
using HRManagement.ViewModels.Many;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HRManagement.ViewModels.Single
{
    public class EmployeeViewModel : BaseCreateWithEditingViewModel<EmployeeService, Employee, EmployeeDTO>
    {

        #region FieldsAndProperties

        #region Visibility properties
        public string AllFormVisibility => IsEditing ? "Collapsed" : "Visible";
        #endregion

        #region Additional Services
        private PersonalDataService PersonalDataService { get; set; } = default!;
        private AdressService RegistrationAdressService { get; set; } = default!;
        private AdressService ResidentialAdressService { get; set; } = default!;
        private CareerService CareerService { get; set; } = default!;
        private EmployeeCareerService EmployeeCareerService { get; set; } = default!;
        private SalaryService SalaryService { get; set; } = default!;
        private PositionService PositionService { get; set; } = default!;
        #endregion

        #region Additional Models
        private PersonalData PersonalDataModel { get; set; } = default!;
        private Adress RegistrationAdressModel { get; set; } = default!;
        private Adress ResidenceAdressModel { get; set; } = default!;
        private Career CareerModel { get; set; } = default!;
        private EmployeeCareer EmployeeCareerModel { get; set; } = default!;
        private Salary SalaryModel { get; set; } = default!;
        private Position PositionModel { get; set; } = default!;
        #endregion

        #region Commands
        public ICommand CleanFormCommand { get; set; }
        public ICommand AddCarrerToModelCommand { get; set; }
        public ICommand DeleteCareerFromModelCommand { get; set; }
        public ICommand SelectPositionCommand { get; set; }
        #endregion

        #region Collections And Models
        private ObservableCollection<Career> _CareerModels = default!;
        public ObservableCollection<Career> CareerModels
        {
            get => _CareerModels;
            set
            {
                if (value != _CareerModels)
                {
                    _CareerModels = value;
                    OnPropertyChanged(() => CareerModels);
                }
            }
        }

        private Career? _SelectedCareerModel;
        public Career? SelectedCarrerModel
        {
            get => _SelectedCareerModel;
            set
            {
                if (value != _SelectedCareerModel)
                {
                    _SelectedCareerModel = value;
                    OnPropertyChanged(() => SelectedCarrerModel);
                }
            }
        }

        // positions
        private PositionDTO? _selectedPositionModel;
        public PositionDTO? SelectedPositionModel
        {
            get => _selectedPositionModel;
            set
            {
                if (value != _selectedPositionModel)
                {
                    _selectedPositionModel = value;
                    OnPropertyChanged(() => SelectedPositionModel);
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

        private ObservableCollection<string> _genderModels = default!;
        public ObservableCollection<string> GenderModels
        {
            get => _genderModels;
            set
            {
                if (value != _genderModels)
                {
                    _genderModels = value;
                    OnPropertyChanged(() => GenderModels);
                }
            }
        }
        #endregion

        #region ApplicationFields
        private string _employmentMessageLabel = "";
        public string EmploymentMessageLabel
        {
            get => _employmentMessageLabel;
            set
            {
                if (value != _employmentMessageLabel)
                {
                    _employmentMessageLabel = value;
                    OnPropertyChanged(() => EmploymentMessageLabel);
                }
            }
        }

        private string _careerMessageLabel = "";
        public string CareerMessageLabel
        {
            get => _careerMessageLabel;
            set
            {
                if (value != _careerMessageLabel)
                {
                    _careerMessageLabel = value;
                    OnPropertyChanged(() => CareerMessageLabel);
                }
            }
        }

        private bool _isSelectedResidenceAddress = false;
        public bool IsSelectedResidenceAddress
        {
            get => _isSelectedResidenceAddress;
            set
            {
                if (value != _isSelectedResidenceAddress)
                {
                    _isSelectedResidenceAddress = value;
                    OnPropertyChanged(() => IsSelectedResidenceAddress);
                    OnPropertyChanged(() => ResidenceAddressOpacity);
                    if (value)
                    {
                        ResidenceCountry = "";
                        ResidenceCity = "";
                        ResidencePostalCode = "";
                        ResidenceStreet = "";
                        ResidenceHouseNo = "";
                        ResidenceFlatNo = "";
                    }
                    else
                    {
                        ResidenceCountry = RegistrationCountry;
                        ResidenceCity = RegistrationCity;
                        ResidencePostalCode = RegistrationPostalCode;
                        ResidenceStreet = RegistrationStreet;
                        ResidenceHouseNo = RegistrationHouseNo;
                        ResidenceFlatNo = RegistrationFlatNo;
                    }
                }
            }
        }
        public string ResidenceAddressOpacity => IsSelectedResidenceAddress ? "1" : "0.3";

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

        #region Employee Basic Information
        public string Firstname
        {
            get => Model.Firstname;
            set
            {
                if (value != Model.Firstname)
                {
                    Model.Firstname = value;
                    OnPropertyChanged(() => Firstname);
                }
            }
        }
        public string Lastname
        {
            get => Model.Surname;
            set
            {
                if (value != Model.Surname)
                {
                    Model.Surname = value;
                    OnPropertyChanged(() => Lastname);
                }
            }
        }
        public string Gender
        {
            get => Model.Gender;
            set
            {
                if (value != Model.Gender)
                {
                    Model.Gender = value;
                    OnPropertyChanged(() => Gender);
                }
            }
        }
        public string Education
        {
            get => PersonalDataModel.Education;
            set
            {
                if (value != PersonalDataModel.Education)
                {
                    PersonalDataModel.Education = value;
                    OnPropertyChanged(() => Education);
                }
            }
        }
        #endregion

        #region Employee Personal Data
        public string Pesel
        {
            get => PersonalDataModel.Pesel;
            set
            {
                if (value != PersonalDataModel.Pesel)
                {
                    if (value.Length == 11)
                    {
                        PersonalDataModel.Pesel = value;
                        OnPropertyChanged(() => Pesel);
                    }
                    else
                    {
                        MessageColor = "Red";
                        MessageLabel = "PESEL has to contain 11 characters!";
                        PersonalDataModel.Pesel = "";
                        OnPropertyChanged(() => Pesel);
                    }
                }
            }
        }
        public string PhoneNo
        {
            get => PersonalDataModel.PhoneNumber;
            set
            {
                if (value != PersonalDataModel.PhoneNumber)
                {
                    PersonalDataModel.PhoneNumber = value;
                    OnPropertyChanged(() => PhoneNo);
                }
            }
        }
        public DateTime DateOfBirth
        {
            get => PersonalDataModel.DateOfBirth.ToDateTime(TimeOnly.MinValue);
            set
            {
                if (value != PersonalDataModel.DateOfBirth.ToDateTime(TimeOnly.MinValue))
                {
                    PersonalDataModel.DateOfBirth = DateOnly.FromDateTime(value);
                    OnPropertyChanged(() => DateOfBirth);
                }
            }
        }
        public string PlaceOfBirth
        {
            get => PersonalDataModel.PlaceOfBirth;
            set
            {
                if (value != PersonalDataModel.PlaceOfBirth)
                {
                    PersonalDataModel.PlaceOfBirth = value;
                    OnPropertyChanged(() => PlaceOfBirth);
                }
            }
        }
        public string ChildrenQuantity
        {
            get => PersonalDataModel.ChildrenQuantity + "";
            set
            {
                if (value != PersonalDataModel.ChildrenQuantity + "")
                {
                    MessageColor = "Red";
                    if (Regex.IsMatch(value, @"^\d*$"))
                    {
                        try
                        {
                            PersonalDataModel.ChildrenQuantity = int.Parse(value);
                            OnPropertyChanged(() => ChildrenQuantity);
                        }
                        catch (Exception)
                        {
                            PersonalDataModel.ChildrenQuantity = 0;
                            OnPropertyChanged(() => ChildrenQuantity);
                            MessageLabel = "Incorrect children quantity! Setting value to 0!";
                        }
                    }
                    else
                    {
                        MessageLabel = "Children quantity has to be a number!";
                    }
                }
            }
        }
        #endregion

        #region Employee Addresses
        public string RegistrationCountry
        {
            get => RegistrationAdressModel.Country;
            set
            {
                if (value != RegistrationAdressModel.Country)
                {
                    RegistrationAdressModel.Country = value;
                    OnPropertyChanged(() => RegistrationCountry);
                    if (!IsSelectedResidenceAddress)
                    {
                        ResidenceCountry = RegistrationCountry;
                        OnPropertyChanged(() => ResidenceCountry);
                    }
                }
            }
        }
        public string RegistrationCity
        {
            get => RegistrationAdressModel.City;
            set
            {
                if (value != RegistrationAdressModel.City)
                {
                    RegistrationAdressModel.City = value;
                    OnPropertyChanged(() => RegistrationCity);
                    if (!IsSelectedResidenceAddress)
                    {
                        ResidenceCity = RegistrationCity;
                        OnPropertyChanged(() => ResidenceCity);
                    }
                }
            }
        }
        public string RegistrationPostalCode
        {
            get => RegistrationAdressModel.PostalCode ?? "";
            set
            {
                if (value != RegistrationAdressModel.PostalCode)
                {
                    RegistrationAdressModel.PostalCode = value;
                    OnPropertyChanged(() => RegistrationPostalCode);
                    if (!IsSelectedResidenceAddress)
                    {
                        ResidencePostalCode = RegistrationPostalCode;
                        OnPropertyChanged(() => ResidencePostalCode);
                    }
                }
            }
        }
        public string RegistrationStreet
        {
            get => RegistrationAdressModel.Street ?? "";
            set
            {
                if (value != RegistrationAdressModel.Street)
                {
                    RegistrationAdressModel.Street = value;
                    OnPropertyChanged(() => RegistrationStreet);
                    if (!IsSelectedResidenceAddress)
                    {
                        ResidenceStreet = RegistrationStreet;
                        OnPropertyChanged(() => ResidenceStreet);
                    }
                }
            }
        }
        public string RegistrationHouseNo
        {
            get => RegistrationAdressModel.HouseNumber;
            set
            {
                if (value != RegistrationAdressModel.HouseNumber)
                {
                    RegistrationAdressModel.HouseNumber = value;
                    OnPropertyChanged(() => RegistrationHouseNo);
                    if (!IsSelectedResidenceAddress)
                    {
                        ResidenceHouseNo = RegistrationHouseNo;
                        OnPropertyChanged(() => ResidenceHouseNo);
                    }
                }
            }
        }
        public string RegistrationFlatNo
        {
            get => RegistrationAdressModel.FlatNumber ?? "";
            set
            {
                if (value != RegistrationAdressModel.FlatNumber)
                {
                    RegistrationAdressModel.FlatNumber = value;
                    OnPropertyChanged(() => RegistrationFlatNo);
                    if (!IsSelectedResidenceAddress)
                    {
                        ResidenceFlatNo = RegistrationFlatNo;
                        OnPropertyChanged(() => ResidenceFlatNo);
                    }
                }
            }
        }
        // Residence address
        public string ResidenceCountry
        {
            get => ResidenceAdressModel.Country;
            set
            {
                if (value != ResidenceAdressModel.Country)
                {
                    ResidenceAdressModel.Country = value;
                    OnPropertyChanged(() => ResidenceCountry);
                }
            }
        }
        public string ResidenceCity
        {
            get => ResidenceAdressModel.City;
            set
            {
                if (value != ResidenceAdressModel.City)
                {
                    ResidenceAdressModel.City = value;
                    OnPropertyChanged(() => ResidenceCity);
                }
            }
        }
        public string ResidencePostalCode
        {
            get => ResidenceAdressModel.PostalCode ?? "";
            set
            {
                if (value != ResidenceAdressModel.PostalCode)
                {
                    ResidenceAdressModel.PostalCode = value;
                    OnPropertyChanged(() => ResidencePostalCode);
                }
            }
        }
        public string ResidenceStreet
        {
            get => ResidenceAdressModel.Street ?? "";
            set
            {
                if (value != ResidenceAdressModel.Street)
                {
                    ResidenceAdressModel.Street = value;
                    OnPropertyChanged(() => ResidenceStreet);
                }
            }
        }
        public string ResidenceHouseNo
        {
            get => ResidenceAdressModel.HouseNumber;
            set
            {
                if (value != ResidenceAdressModel.HouseNumber)
                {
                    ResidenceAdressModel.HouseNumber = value;
                    OnPropertyChanged(() => ResidenceHouseNo);
                }
            }
        }
        public string ResidenceFlatNo
        {
            get => ResidenceAdressModel.FlatNumber ?? "";
            set
            {
                if (value != ResidenceAdressModel.FlatNumber)
                {
                    ResidenceAdressModel.FlatNumber = value;
                    OnPropertyChanged(() => ResidenceFlatNo);
                }
            }
        }
        #endregion

        #region Employee Carrer
        public string CareerTitle
        {
            get => CareerModel.Position;
            set
            {
                if (value != CareerModel.Position)
                {
                    CareerModel.Position = value;
                    OnPropertyChanged(() => CareerTitle);
                }
            }
        }
        public string CareerPlace
        {
            get => CareerModel.Title;
            set
            {
                if (value != CareerModel.Title)
                {
                    CareerModel.Title = value;
                    OnPropertyChanged(() => CareerPlace);
                }
            }
        }
        public DateTime CareerBeginDate
        {
            get => CareerModel.BeginDate.ToDateTime(TimeOnly.MinValue);
            set
            {
                if (value != CareerModel.BeginDate.ToDateTime(TimeOnly.MinValue))
                {
                    CareerModel.BeginDate = DateOnly.FromDateTime(value);
                    OnPropertyChanged(() => CareerBeginDate);
                }
            }
        }
        public DateTime CareerEndDate
        {
            get => CareerModel.EndDate.ToDateTime(TimeOnly.MinValue);
            set
            {
                if (value != CareerModel.EndDate.ToDateTime(TimeOnly.MinValue))
                {
                    CareerModel.EndDate = DateOnly.FromDateTime(value);
                    OnPropertyChanged(() => CareerEndDate);
                }
            }
        }
        #endregion

        #region Employee Employment
        public string EmploymentType
        {
            get => Model.EmploymentType;
            set
            {
                if (value != Model.EmploymentType)
                {
                    Model.EmploymentType = value;
                    OnPropertyChanged(() => EmploymentType);
                }
            }
        }

        #region Salary
        public string BruttoPrice
        {
            get => Decimal.Round(SalaryModel.BruttoPrice, 2) + "";
            set
            {
                if (value != SalaryModel.BruttoPrice + "")
                {
                    MessageColor = "Red";
                    if (string.IsNullOrEmpty(value))
                    {
                        SalaryModel.BruttoPrice = 0.00m;
                        OnPropertyChanged(() => BruttoPrice);
                    }
                    if (Regex.IsMatch(value, @"^\d+([\.,]\d{1,2})?$"))
                    {
                        try
                        {
                            Decimal brutto = Decimal.Parse(value);
                            if (brutto >= 0)
                            {
                                SalaryModel.BruttoPrice = brutto;
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
                        SalaryModel.BruttoPrice = 0.00m;
                    }
                }
            }
        }
        public string TaxRate
        {
            get => Double.Round(SalaryModel.TaxRate * 100, 2) + "";
            set
            {
                if (value != SalaryModel.TaxRate + "")
                {
                    MessageColor = "Red";
                    if (string.IsNullOrEmpty(value))
                    {
                        SalaryModel.TaxRate = 0.00d;
                        OnPropertyChanged(() => TaxRate);
                    }
                    if (Regex.IsMatch(value, @"^\d+([\.,]\d{1,2})?$"))
                    {
                        try
                        {
                            Double tax = Double.Parse(value);
                            if (tax >= 0 && tax <= 100)
                            {
                                SalaryModel.TaxRate = tax / 100;
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
                        SalaryModel.TaxRate = 0.00d;
                    }
                }
            }
        }
        public string ZusTaxRate
        {
            get => Double.Round(SalaryModel.ZusTaxRate * 100, 2) + "";
            set
            {
                if (value != SalaryModel.ZusTaxRate + "")
                {
                    MessageColor = "Red";
                    if (string.IsNullOrEmpty(value))
                    {
                        SalaryModel.ZusTaxRate = 0.00d;
                        OnPropertyChanged(() => ZusTaxRate);
                    }
                    if (Regex.IsMatch(value, @"^\d+([\.,]\d{1,2})?$"))
                    {
                        try
                        {
                            Double zus = Double.Parse(value);
                            if (zus >= 0 && zus <= 100)
                            {
                                SalaryModel.ZusTaxRate = zus / 100;
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
                        SalaryModel.ZusTaxRate = 0.00d;
                    }
                }
            }
        }
        public string Declusions
        {
            get => Decimal.Round(SalaryModel.Declusions, 2) + "";
            set
            {
                if (value != SalaryModel.Declusions + "")
                {
                    MessageColor = "Red";
                    if (string.IsNullOrEmpty(value))
                    {
                        SalaryModel.Declusions = 0.00m;
                        OnPropertyChanged(() => Declusions);
                    }
                    if (Regex.IsMatch(value, @"^\d+([\.,]\d{1,2})?$"))
                    {
                        try
                        {
                            Decimal declusions = Decimal.Parse(value);
                            if (declusions >= 0)
                            {
                                SalaryModel.Declusions = declusions;
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
                        SalaryModel.Declusions = 0.00m;
                    }
                }
            }
        }
        public string? AdditionalDescription
        {
            get => SalaryModel.Description;
            set
            {
                if (value != SalaryModel.Description)
                {
                    SalaryModel.Description = value;
                    OnPropertyChanged(() => AdditionalDescription);
                }
            }
        }
        public string CalculatedNetto => SalaryService.GetStringOfCalculatedNetto(SalaryModel);

        #endregion

        #region Position
        public string PositionName
        {
            get => PositionModel.Title;
            set
            {
                if (value != PositionModel.Title)
                {
                    PositionModel.Title = value;
                    OnPropertyChanged(() => PositionName);
                }
            }
        }
        public string DepartmentName
        {
            get => PositionModel.DepartmentName ?? "";
            set
            {
                if (value != PositionModel.DepartmentName)
                {
                    PositionModel.DepartmentName = value;
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

        #endregion

        #endregion

        public EmployeeViewModel() : base("Employee")
        {
            CleanFormCommand = new BaseCommand(() => clearForm());
            AddCarrerToModelCommand = new BaseCommand(() => addCareerToModel());
            DeleteCareerFromModelCommand = new BaseCommand(() => deleteCareerFromModel());

            // Handling Selecting and Recieving of Position
            SelectPositionCommand = new BaseCommand(() => WindowManager.OpenWindow(new PositionsWithCallbackViewModel(this)));
            WeakReferenceMessenger.Default.Register<SelectedObjectMessage<PositionDTO>>(this, (recipient, message) => GetSelectedPosition(message));

            clearForm();
        }

        #region Methods
        // Initializations
        private void InitializeServices()
        {
            PersonalDataService = new PersonalDataService();
            RegistrationAdressService = new AdressService();
            ResidentialAdressService = new AdressService();
            SalaryService = new SalaryService();
            PositionService = new PositionService();
            CareerService = new CareerService();
            EmployeeCareerService = new EmployeeCareerService();
        }
        private void InitializeModels()
        {
            Model = Service.CreateModel();
            PersonalDataModel = PersonalDataService.CreateModel();
            RegistrationAdressModel = RegistrationAdressService.CreateModel();
            ResidenceAdressModel = ResidentialAdressService.CreateModel();
            CareerModel = CareerService.CreateModel();
            EmployeeCareerModel = EmployeeCareerService.CreateModel();
            SalaryModel = SalaryService.CreateModel();
            PositionModel = PositionService.CreateModel();

            CareerModels = new ObservableCollection<Career>();

            GenderModels = new ObservableCollection<string> { "Male", "Femaile" };
        }

        // Adding and Deleting operation on List of Careers
        private void addCareerToModel()
        {
            if (string.IsNullOrEmpty(CareerTitle))
            {
                CareerMessageLabel = "Enter Career Title!";
                return;
            }
            if (string.IsNullOrEmpty(CareerPlace))
            {
                CareerMessageLabel = "Enter Career Place!";
                return;
            }
            if (CareerBeginDate > CareerEndDate)
            {
                CareerMessageLabel = "Begin date cannot be greater than End date!";
                return;
            }
            if (CareerEndDate > DateTime.Now)
            {
                CareerMessageLabel = "End date cannot be set to the future!";
                return;
            }

            CareerModels.Add(CareerModel);

            CareerModel = CareerService.CreateModel();
            CareerMessageLabel = "";
            OnPropertyChanged(() => CareerTitle);
            OnPropertyChanged(() => CareerPlace);
            OnPropertyChanged(() => CareerBeginDate);
            OnPropertyChanged(() => CareerEndDate);
        }
        private void deleteCareerFromModel()
        {
            if (SelectedCarrerModel != null)
            {
                CareerModels.Remove(SelectedCarrerModel);
                SelectedCarrerModel = null;
            }
        }

        // Assigning recieved Selected Position
        private void GetSelectedPosition(SelectedObjectMessage<PositionDTO> message)
        {
            if (message.WhoRequestedToSelect == this)
            {
                SelectedPositionModel = message.SelectedObject;
                SelectedPositionName = $"{SelectedPositionModel.Title} | {SelectedPositionModel.DepartmentName}";
            }
        }

        // Saving and cleaning
        public override void Save()
        {
            MessageColor = "Red";

            #region Validation of Base employee fields
            if (string.IsNullOrEmpty(Firstname))
            {
                MessageLabel = "Enter Firstname In Basic Information Section!";
                return;
            }
            if (string.IsNullOrEmpty(Lastname))
            {
                MessageLabel = "Enter Lastname In Basic Information Section!";
                return;
            }
            if (string.IsNullOrEmpty(Gender))
            {
                MessageLabel = "Enter Gender In Basic Information Section!";
                return;
            }
            if (string.IsNullOrEmpty(EmploymentType))
            {
                MessageLabel = "Enter Gender In Employment Section!";
                return;
            }

            // Update Employee Basic Information when editing
            if(IsEditing)
            {
                if (string.IsNullOrEmpty(Education))
                {
                    MessageLabel = "Enter Gender In Basic Information Section!";
                    return;
                }
                PersonalDataService.UpdateModel(PersonalDataModel);
                Service.UpdateModel(Model);
                WeakReferenceMessenger.Default.Send<OpenViewMessage>(new OpenViewMessage()
                {
                    Sender = this,
                    ViewModelToBeOpened = new EmployeesViewModel()
                });
                OnRequestClose();
                return;
            }

            #endregion

            #region Adding and Validation of Position
            if (!IsSelectedCreateNewPosition)
            {
                if (SelectedPositionModel == null)
                {
                    MessageLabel = "Select Position or create new In Position Section!";
                    return;
                }
                else
                {
                    PositionModel = PositionService.GetModel(SelectedPositionModel.Id);
                    if (PositionModel == null)
                    {
                        MessageLabel = "Cannot set selected position from list!";
                        return;
                    }
                }
            }
            else
            {
                if (IsActiveDepartmentName && string.IsNullOrEmpty(DepartmentName))
                {
                    MessageLabel = "Enter Department name or select 'Do not assign department' In Position Section!";
                    return;
                }
                PositionService.AddModel(PositionModel);
            }
            #endregion

            #region Adding and Validation of Salary
            if (string.IsNullOrEmpty(BruttoPrice))
            {
                MessageLabel = "Enter BruttoPrice In Salary Section!";
                return;
            }
            if (string.IsNullOrEmpty(TaxRate))
            {
                MessageLabel = "Enter Tax Rate In Salary Section!";
                return;
            }
            if (string.IsNullOrEmpty(ZusTaxRate))
            {
                MessageLabel = "Enter Zus Tax Rate In Salary Section!";
                return;
            }
            if (string.IsNullOrEmpty(Declusions))
            {
                MessageLabel = "Enter Declusions In Salary Section!";
                return;
            }
            if (SalaryService.GetCalculatedNetto(SalaryModel) < 0)
            {
                MessageLabel = "Netto value has to be equal or greater than 0!";
                return;
            }

            SalaryService.AddModel(SalaryModel);
            #endregion

            #region Adding Employee
            
            Model.PositionId = PositionModel.Id;
            Model.SalaryId = SalaryModel.Id;
            Service.AddModel(Model);
            #endregion

            #region Adding and Validation of Adresses
            // Adding and validation of Registration Adress
            if (string.IsNullOrEmpty(RegistrationCountry))
            {
                MessageLabel = "Enter Country In Adresses Section!";
                return;
            }
            if (string.IsNullOrEmpty(RegistrationCity))
            {
                MessageLabel = "Enter City In Adresses Section!";
                return;
            }
            if (string.IsNullOrEmpty(RegistrationHouseNo))
            {
                MessageLabel = "Enter House Numer In Adresses Section!";
                return;
            }
            RegistrationAdressService.AddModel(RegistrationAdressModel);
            PersonalDataModel.RegistrationAdressId = RegistrationAdressModel.Id;

            // Assigning Residence address as Registration if selected
            if (!IsSelectedResidenceAddress)
            {
                PersonalDataModel.ResidenceAdressId = RegistrationAdressModel.Id;
            }
            // Adding and validation of Residence Adress if differ than Registration
            else
            {
                if (string.IsNullOrEmpty(ResidenceCountry))
                {
                    MessageLabel = "Enter Country In Adresses Section!";
                    return;
                }
                if (string.IsNullOrEmpty(ResidenceCity))
                {
                    MessageLabel = "Enter City In Adresses Section!";
                    return;
                }
                if (string.IsNullOrEmpty(ResidenceHouseNo))
                {
                    MessageLabel = "Enter House Numer In Adresses Section!";
                    return;
                }
                ResidentialAdressService.AddModel(ResidenceAdressModel);
                PersonalDataModel.ResidenceAdressId = ResidenceAdressModel.Id;
            }
            #endregion

            #region Adding and Validation of Personal Data
            if (string.IsNullOrEmpty(Pesel))
            {
                MessageLabel = "Enter Pesel In Personal Data Section!";
                return;
            }
            if (Pesel.Length != 11)
            {
                MessageLabel = "Pesel has to contain 11 characters!";
                return;
            }
            if (string.IsNullOrEmpty(Education))
            {
                MessageLabel = "Enter Pesel In Basic Information Section!";
                return;
            }
            if (string.IsNullOrEmpty(PhoneNo))
            {
                MessageLabel = "Enter Phone Number In Personal Data Section!";
                return;
            }
            if (DateOfBirth >= DateTime.Now)
            {
                MessageLabel = "Enter Date of cannot be greater than today!";
                return;
            }
            if (string.IsNullOrEmpty(PlaceOfBirth))
            {
                MessageLabel = "Enter Place Of Birth In Personal Data Section!";
                return;
            }
            if (string.IsNullOrEmpty(ChildrenQuantity))
            {
                MessageLabel = "Enter Children Quantity In Personal Data Section!";
                return;
            }
            PersonalDataModel.EmployeeId = Model.Id;
            PersonalDataService.AddModel(PersonalDataModel);
            #endregion

            #region Adding and Validation of Careers
            if (CareerModels.IsNullOrEmpty() || CareerModels.Count <= 0)
            {
                MessageLabel = "Career cannot be empty, at least basic school!";
                return;
            }

            foreach (Career item in CareerModels)
            {
                CareerModel = item;
                CareerService.AddModel(CareerModel);
                EmployeeCareerModel = EmployeeCareerService.CreateModel();
                EmployeeCareerModel.CareerId = CareerModel.Id;
                EmployeeCareerModel.EmployeeId = Model.Id;
            }
            #endregion

            clearForm();

            MessageColor = "Green";
            MessageLabel = $"Employee successfully created";

        }
        private void clearForm()
        {
            MessageLabel = "";
            InitializeServices();
            InitializeModels();
            OnPropertyChanged(() => Firstname);
            OnPropertyChanged(() => Lastname);
            OnPropertyChanged(() => Gender);
            OnPropertyChanged(() => Education);
            OnPropertyChanged(() => Pesel);
            OnPropertyChanged(() => PhoneNo);
            OnPropertyChanged(() => DateOfBirth);
            OnPropertyChanged(() => PlaceOfBirth);
            OnPropertyChanged(() => ChildrenQuantity);
            OnPropertyChanged(() => RegistrationCountry);
            OnPropertyChanged(() => RegistrationCity);
            OnPropertyChanged(() => RegistrationPostalCode);
            OnPropertyChanged(() => RegistrationStreet);
            OnPropertyChanged(() => RegistrationHouseNo);
            OnPropertyChanged(() => RegistrationFlatNo);
            OnPropertyChanged(() => ResidenceCountry);
            OnPropertyChanged(() => ResidenceCity);
            OnPropertyChanged(() => ResidencePostalCode);
            OnPropertyChanged(() => ResidenceStreet);
            OnPropertyChanged(() => ResidenceHouseNo);
            OnPropertyChanged(() => ResidenceFlatNo);
            OnPropertyChanged(() => CareerTitle);
            OnPropertyChanged(() => CareerPlace);
            OnPropertyChanged(() => CareerBeginDate);
            OnPropertyChanged(() => CareerEndDate);
            OnPropertyChanged(() => EmploymentType);
            OnPropertyChanged(() => BruttoPrice);
            OnPropertyChanged(() => TaxRate);
            OnPropertyChanged(() => ZusTaxRate);
            OnPropertyChanged(() => Declusions);
            OnPropertyChanged(() => AdditionalDescription);
            OnPropertyChanged(() => CalculatedNetto);
            OnPropertyChanged(() => PositionName);
            OnPropertyChanged(() => DepartmentName);
            SelectedPositionName = null;
        }

        protected override void EditionFieldsUpdating()
        {
            OnPropertyChanged(() => Firstname);
            OnPropertyChanged(() => Lastname);
            OnPropertyChanged(() => Gender);
            PersonalDataModel = PersonalDataService.GetModel(Model);
            OnPropertyChanged(() => Education);
            OnPropertyChanged(() => EmploymentType);
            OnPropertyChanged(() => AllFormVisibility);
        }
        #endregion
    }
}
