using HRManagement.Helpers;
using HRManagement.Models;
using HRManagement.Models.Contexts;
using HRManagement.Models.DTOs;
using HRManagement.Models.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace HRManagement.ViewModels.Single
{
    /// <summary>
    /// AdressService that is given as Generic type is for Registration Adress
    /// Model is for Registration Adress
    /// </summary>
    public class AdressViewModel : CreationWithEmployeeSelectorViewModel<AdressService, Adress, AdressDTO>
    {

        #region FieldsAndProperties

        private bool _updateResidenceAdress;
        private PersonalData? _personalData;

        #region AdditionalServices
        private AdressService ResidenceAdressService { get; set; } = default!;
        private PersonalDataService PersonalDataService { get; set; } = default!;
        #endregion

        #region UserControl Fields
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
                        _updateResidenceAdress = false;
                        ResidenceModel = ResidenceAdressService.CreateModel();
                        ResidenceCountry = "";
                        ResidenceCity = "";
                        ResidencePostalCode = "";
                        ResidenceStreet = "";
                        ResidenceHouseNo = "";
                        ResidenceFlatNo = "";
                    }

                    else
                    {
                        ResidenceModel = Model;
                        ResidenceCountry = RegistrationCountry;
                        ResidenceCity = RegistrationCity;
                        ResidencePostalCode = RegistrationPostalCode;
                        ResidenceStreet = RegistrationStreet;
                        ResidenceHouseNo = RegistrationHouseNo;
                        ResidenceFlatNo = RegistrationFlatNo;

                        OnPropertyChanged(() => ResidenceCountry);
                        OnPropertyChanged(() => ResidenceCity);
                        OnPropertyChanged(() => ResidencePostalCode);
                        OnPropertyChanged(() => ResidenceStreet);
                        OnPropertyChanged(() => ResidenceHouseNo);
                        OnPropertyChanged(() => ResidenceFlatNo);
                    }
                }
            }
        }
        public string ResidenceAddressOpacity => IsSelectedResidenceAddress ? "1" : "0.3";
        #endregion

        #region Registration Adress Property
        public string RegistrationCountry
        {
            get => Model.Country;
            set
            {
                if (value != Model.Country)
                {
                    Model.Country = value;
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
            get => Model.City;
            set
            {
                if (value != Model.City)
                {
                    Model.City = value;
                    OnPropertyChanged(() => RegistrationCity);
                    if (!IsSelectedResidenceAddress)
                    {
                        ResidenceCity = RegistrationCity;
                        OnPropertyChanged(() => ResidenceCity);
                    }
                }
            }
        }
        public string? RegistrationPostalCode
        {
            get => Model.PostalCode;
            set
            {
                if (value != Model.PostalCode)
                {
                    Model.PostalCode = value;
                    OnPropertyChanged(() => RegistrationPostalCode);
                    if (!IsSelectedResidenceAddress)
                    {
                        ResidencePostalCode = RegistrationPostalCode;
                        OnPropertyChanged(() => ResidencePostalCode);
                    }
                }
            }
        }
        public string? RegistrationStreet
        {
            get => Model.Street;
            set
            {
                if (value != Model.Street)
                {
                    Model.Street = value;
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
            get => Model.HouseNumber;
            set
            {
                if (value != Model.HouseNumber)
                {
                    Model.HouseNumber = value;
                    OnPropertyChanged(() => RegistrationHouseNo);
                    if (!IsSelectedResidenceAddress)
                    {
                        ResidenceHouseNo = RegistrationHouseNo;
                        OnPropertyChanged(() => ResidenceHouseNo);
                    }
                }
            }
        }
        public string? RegistrationFlatNo
        {
            get => Model.FlatNumber;
            set
            {
                if (value != Model.FlatNumber)
                {
                    Model.FlatNumber = value;
                    OnPropertyChanged(() => RegistrationFlatNo);
                    if (!IsSelectedResidenceAddress)
                    {
                        ResidenceFlatNo = RegistrationFlatNo;
                        OnPropertyChanged(() => ResidenceFlatNo);
                    }
                }
            }
        }
        #endregion

        #region Residential Adress Property
        private Adress ResidenceModel { get; set; } = default!;
        public string ResidenceCountry
        {
            get => ResidenceModel.Country;
            set
            {
                if (value != ResidenceModel.Country)
                {
                    ResidenceModel.Country = value;
                    OnPropertyChanged(() => ResidenceCountry);
                }
            }
        }
        public string ResidenceCity
        {
            get => ResidenceModel.City;
            set
            {
                if (value != ResidenceModel.City)
                {
                    ResidenceModel.City = value;
                    OnPropertyChanged(() => ResidenceCity);
                }
            }
        }
        public string? ResidencePostalCode
        {
            get => ResidenceModel.PostalCode;
            set
            {
                if (value != ResidenceModel.PostalCode)
                {
                    ResidenceModel.PostalCode = value;
                    OnPropertyChanged(() => ResidencePostalCode);
                }
            }
        }
        public string? ResidenceStreet
        {
            get => ResidenceModel.Street;
            set
            {
                if (value != ResidenceModel.Street)
                {
                    ResidenceModel.Street = value;
                    OnPropertyChanged(() => ResidenceStreet);
                }
            }
        }
        public string ResidenceHouseNo
        {
            get => ResidenceModel.HouseNumber;
            set
            {
                if (value != ResidenceModel.HouseNumber)
                {
                    ResidenceModel.HouseNumber = value;
                    OnPropertyChanged(() => ResidenceHouseNo);
                }
            }
        }
        public string? ResidenceFlatNo
        {
            get => ResidenceModel.FlatNumber;
            set
            {
                if (value != ResidenceModel.FlatNumber)
                {
                    ResidenceModel.FlatNumber = value;
                    OnPropertyChanged(() => ResidenceFlatNo);
                }
            }
        }
        #endregion

        #endregion

        public AdressViewModel() : base("Adress")
        {
            ResidenceAdressService = new AdressService();
            PersonalDataService = new PersonalDataService();
            ResidenceModel = ResidenceAdressService.CreateModel();
            _updateResidenceAdress = false;
        }


        #region Overrided Abstract Methods
        protected override void UpdateFormFields()
        {
            if (SelectedEmployeeModel != null && EmployeeModel != null)
            {
                _personalData = PersonalDataService.GetModel(EmployeeModel);
                if (_personalData != null)
                {
                    Model = Service.GetModel(_personalData.RegistrationAdressId);
                    if (_personalData.ResidenceAdressId == _personalData.RegistrationAdressId)
                    {
                        ResidenceModel = Model;
                        IsSelectedResidenceAddress = false;
                    }
                    else
                    {
                        IsSelectedResidenceAddress = true;
                        ResidenceModel = Service.GetModel(_personalData.ResidenceAdressId);
                        _updateResidenceAdress = true;
                    }
                }

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
            }
        }
        public override void Save()
        {
            MessageColor = "Red";

            if (SelectedEmployeeModel == null || SelectedEmployeeModel?.Id == null)
            {
                MessageLabel = "Cannot find Employee In Database!";
                return;
            }

            if (_personalData == null)
            {
                MessageLabel = "Invalid Personal Data of the Employee! Cannot Save!";
                return;
            }

            if (Model == null || ResidenceModel == null)
            {
                MessageLabel = "Cannot find Adresses of The Employee In Database!";
                return;
            }

            // Validation of Registration Adress
            if (string.IsNullOrEmpty(RegistrationCountry))
            {
                MessageLabel = "Enter Registration Country!";
                return;
            }
            if (string.IsNullOrEmpty(RegistrationCity))
            {
                MessageLabel = "Enter Registration City!";
                return;
            }
            if (string.IsNullOrEmpty(RegistrationHouseNo))
            {
                MessageLabel = "Enter Registration House Number!";
                return;
            }

            // Validation of Residence Adress if is different than Registration Adress
            if (IsSelectedResidenceAddress)
            {
                if (string.IsNullOrEmpty(ResidenceCountry))
                {
                    MessageLabel = "Enter Residence Country!";
                    return;
                }
                if (string.IsNullOrEmpty(ResidenceCity))
                {
                    MessageLabel = "Enter Residence City!";
                    return;
                }
                if (string.IsNullOrEmpty(ResidenceHouseNo))
                {
                    MessageLabel = "Enter Residence House Number!";
                    return;
                }
            }

            // Performing Saving of Residence Adress if new Selected
            if (IsSelectedResidenceAddress)
            {
                // Update existing model of Residential Adress
                if (_updateResidenceAdress)
                {
                    ResidenceAdressService.UpdateModel(ResidenceModel);
                }
                //Add new Model of Residential Adress if newly created
                else
                {
                    ResidenceAdressService.AddModel(ResidenceModel);
                    _personalData.ResidenceAdress = ResidenceModel;
                    PersonalDataService.UpdateModel(_personalData);
                }
            }
            else
            {
                _personalData.ResidenceAdress = Model;
                PersonalDataService.UpdateModel(_personalData);
            }

            Service.UpdateModel(Model);
            UpdateFormFields();

            MessageColor = "Green";
            MessageLabel = "Adresses updated sucessfully!";
        } 
        #endregion

    }
}
