using CommunityToolkit.Mvvm.Messaging;
using HRManagement.Helpers;
using HRManagement.Models;
using HRManagement.Models.DTOs;
using HRManagement.Models.ModalWindows;
using HRManagement.Models.Services;
using HRManagement.ViewModels.Many;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRManagement.ViewModels.Single
{
    public abstract class CreationWithEmployeeSelectorViewModel<ServiceClass, ModelClass, DtoClass>
        : BaseCreateViewModel<ServiceClass, ModelClass, DtoClass>
        where ServiceClass : BaseService<ModelClass, DtoClass>, new()
        where ModelClass : class, new()
        where DtoClass : class
    {

        #region FieldsAndProperties

        #region Commands
        public ICommand SelectEmployeeCommand { get; set; }
        #endregion

        #region Employees Service And Collection
        public EmployeeService EmployeeService { get; set; }

        private EmployeeDTO? _selectedEmployeeModel;
        public EmployeeDTO? SelectedEmployeeModel
        {
            get => _selectedEmployeeModel;
            set
            {
                if (value != _selectedEmployeeModel)
                {
                    _selectedEmployeeModel = value;
                    OnPropertyChanged(() => SelectedEmployeeModel);

                    UpdateSelectedModel();
                    UpdateFormFields();
                }
            }
        }

        private string? _employeeDisplay;
        public string? EmployeeDisplay
        {
            get => _employeeDisplay ?? "Employee not selected";
            set
            {
                if (value != _employeeDisplay)
                {
                    _employeeDisplay = value;
                    OnPropertyChanged(() => EmployeeDisplay);
                }
            }
        }

        private Employee? _employeeModel;
        public Employee? EmployeeModel 
        { 
            get => _employeeModel;
            set
            {
                if(value != _employeeModel)
                {
                    _employeeModel = value;
                    OnPropertyChanged(() => EmployeeModel);
                }
            }
        }
        #endregion

        #endregion

        public CreationWithEmployeeSelectorViewModel(string displayName) : base(displayName)
        {
            SelectEmployeeCommand = new BaseCommand(() => WindowManager.OpenWindow(new EmployeesWithCallbackViewModel(this)));
            EmployeeService = new EmployeeService();
            WeakReferenceMessenger.Default.Register<SelectedObjectMessage<EmployeeDTO>>(this, (recipient, message) => GetSelectedEmployee(message));
        }


        protected virtual void GetSelectedEmployee(SelectedObjectMessage<EmployeeDTO> message)
        {
            if (message.WhoRequestedToSelect == this)
            {
                SelectedEmployeeModel = message.SelectedObject;
            }
        }

        protected virtual void UpdateSelectedModel()
        {
            if(SelectedEmployeeModel != null)
            {
                EmployeeModel = EmployeeService.GetModel(SelectedEmployeeModel.Id);
                EmployeeDisplay = $"{EmployeeModel.Firstname} {EmployeeModel.Surname}";
            }
            else
            {
                EmployeeModel = null;
                MessageColor = "Red";
                MessageLabel = "Cannot download selected model from database";
            }
        }
        protected abstract void UpdateFormFields();

        public override void InitializeNewModelAndService()
        {
            base.InitializeNewModelAndService();
            SelectedEmployeeModel = null;
            EmployeeModel = null;
            EmployeeDisplay = null;
        }
    }
}
