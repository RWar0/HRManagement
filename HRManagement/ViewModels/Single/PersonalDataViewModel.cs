using HRManagement.Helpers;
using HRManagement.Models;
using HRManagement.Models.Contexts;
using HRManagement.Models.DTOs;
using HRManagement.Models.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRManagement.ViewModels.Single
{
    public class PersonalDataViewModel : CreationWithEmployeeSelectorViewModel<PersonalDataService, PersonalData, PersonalDataDTO>
    {

        #region FieldsAndProperties
        public string Pesel
        {
            get => Model.Pesel;
            set
            {
                if (value != Model.Pesel)
                {
                    Model.Pesel = value;
                    OnPropertyChanged(() => Pesel);
                }
            }
        }
        public string PhoneNo
        {
            get => Model.PhoneNumber;
            set
            {
                if (value != Model.PhoneNumber)
                {
                    Model.PhoneNumber = value;
                    OnPropertyChanged(() => PhoneNo);
                }
            }
        }
        public DateTime DateOfBirth
        {
            get => Model.DateOfBirth.ToDateTime(TimeOnly.MinValue);
            set
            {
                if (value != Model.DateOfBirth.ToDateTime(TimeOnly.MinValue))
                {
                    Model.DateOfBirth = DateOnly.FromDateTime(value);
                    OnPropertyChanged(() => DateOfBirth);
                }
            }
        }
        public string PlaceOfBirth
        {
            get => Model.PlaceOfBirth;
            set
            {
                if (value != Model.PlaceOfBirth)
                {
                    Model.PlaceOfBirth = value;
                    OnPropertyChanged(() => PlaceOfBirth);
                }
            }
        }
        public string ChildrenQuantity
        {
            get => Model.ChildrenQuantity + "";
            set
            {
                if (value != Model.ChildrenQuantity + "")
                {
                    MessageColor = "Red";
                    if (Regex.IsMatch(value, @"^\d*$"))
                    {
                        try
                        {
                            Model.ChildrenQuantity = int.Parse(value);
                            MessageLabel = "";
                        }
                        catch
                        {
                            Model.ChildrenQuantity = 0;
                            MessageLabel = "Incorrect value of children quantity! Setting value to 0!";
                        }
                    }
                    else
                    {
                        Model.ChildrenQuantity = 0;
                        MessageLabel = "Children quantity has to be a number! Setting value to 0!";
                    }
                    OnPropertyChanged(() => ChildrenQuantity);
                }
            }
        }
        public string Education
        {
            get => Model.Education;
            set
            {
                if (value != Model.Education)
                {
                    Model.Education = value;
                    OnPropertyChanged(() => Education);
                }
            }
        }
        #endregion

        public PersonalDataViewModel() : base("Personal Data")
        {

        }

        #region Overrided Abstract Methods
        protected override void UpdateFormFields()
        {
            MessageLabel = "";
            if (SelectedEmployeeModel != null && EmployeeModel != null)
            {
                Model = Service.GetModel(EmployeeModel);
                OnPropertyChanged(() => Pesel);
                OnPropertyChanged(() => PhoneNo);
                OnPropertyChanged(() => DateOfBirth);
                OnPropertyChanged(() => PlaceOfBirth);
                OnPropertyChanged(() => ChildrenQuantity);
                OnPropertyChanged(() => Education);
            }
        }
        public override void Save()
        {
            MessageColor = "Red";
            if (SelectedEmployeeModel == null || SelectedEmployeeModel?.Id == null)
            {
                MessageLabel = "Select employee firstly!";
                return;
            }
            if (Model == null)
            {
                MessageLabel = "Select employee firstly - there is not assigned Personal Data!";
                return;
            }

            if (string.IsNullOrEmpty(Pesel))
            {
                MessageLabel = "Enter Pesel!";
                return;
            }

            if (string.IsNullOrEmpty(PhoneNo))
            {
                MessageLabel = "Enter Phone Number!";
                return;
            }
            if (DateOfBirth > DateTime.Now)
            {
                MessageLabel = "Date Of Birth cannot be higher than today!";
                return;
            }
            if (string.IsNullOrEmpty(PlaceOfBirth))
            {
                MessageLabel = "Enter Place Of Birth!";
                return;
            }

            if (string.IsNullOrEmpty(ChildrenQuantity))
            {
                MessageLabel = "Enter Children Quantity!";
                return;
            }

            Service.UpdateModel(Model);
            UpdateFormFields();

            MessageColor = "Green";
            MessageLabel = "Personal Data updated successfully!";
        }

        #endregion
    }
}
