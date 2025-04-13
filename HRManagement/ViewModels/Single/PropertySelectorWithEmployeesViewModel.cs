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
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="ServiceClass">Class of main [Property]service - not service that begins with Employee..., eg. SkillService </typeparam>
    /// <typeparam name="ManyServiceClass">Service of Employee[Property]Service that begins with Employee..., eg. EmployeeSkillService</typeparam>
    /// <typeparam name="ParentPartialClass">Class of Employee[Property] that begins with Employee..., eg. EmployeeSkill</typeparam>
    /// <typeparam name="ParentPartialClassDTO">DTO class of the ParentPartialClass, does start with Employee..., eg. EmployeeSkillDTO</typeparam>
    /// <typeparam name="ModelClass">Class of single element, that does not start with Employee..., eg. Skill </typeparam>
    /// <typeparam name="DtoClass">DTO class of the Model Class, does not start with Employee..., eg. SkillDTO</typeparam>
    ///
    public abstract class PropertySelectorWithEmployeesViewModel<ServiceClass, ManyServiceClass, ParentPartialClass, ParentPartialClassDTO, ModelClass, DtoClass>
        : CreationWithEmployeeSelectorViewModel<ServiceClass, ModelClass, DtoClass>
        where ServiceClass : BaseServiceWithEmployeeSearch<ModelClass, DtoClass>, new()
        where ManyServiceClass : BaseManyToManyTableService<ParentPartialClass, ParentPartialClassDTO, ModelClass>, new()
        where ParentPartialClass : class, IManyModel, new()
        where ParentPartialClassDTO : class, new()
        where ModelClass : class, IModel, new()
        where DtoClass : class, IModelDTO, new()
    {

        #region FieldsAndProperties

        #region Additional Service
        public ManyServiceClass AdditionalService { get; set; }
        #endregion

        #region Commands
        public ICommand AddSelectedEmployeeToPropertyCommand { get; set; }
        public ICommand RemoveSelectedEmployeeFromPropertyCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand CreateNewModelCommand { get; set; }
        public ICommand SelectPropertyModelCommand { get; set; } = default!;
        public ICommand SelectAvailableEmployeeCommand { get; set; } = default!;
        #endregion

        #region Accept/Saving Button Text

        public const string _CREATE = "Create new";
        public const string _CHANGE = "Save changes";
        public string AcceptButtonMessage => SelectedPropertyModel == BaseCreationModel ? _CREATE : _CHANGE;

        #endregion

        #region Employee Model
        private ObservableCollection<EmployeeDTO> _employeeModels = default!;
        public ObservableCollection<EmployeeDTO> EmployeeModels
        {
            get => _employeeModels;
            set
            {
                if (value != _employeeModels)
                {
                    _employeeModels = value;
                    OnPropertyChanged(() => EmployeeModels);
                }
            }
        }
        #endregion

        #region Current Property
        public DtoClass BaseCreationModel { get; set; } = default!;

        private DtoClass? _selectedPropertyModel;
        public DtoClass? SelectedPropertyModel
        {
            get => _selectedPropertyModel;
            set
            {
                if (value != _selectedPropertyModel)
                {
                    _selectedPropertyModel = value;

                    if (value != null)
                    {
                        if (value == BaseCreationModel)
                        {
                            Model = Service.CreateModel();
                        }
                        else
                        {
                            Model = Service.GetModel(value.Id);
                        }
                    }
                    UpdatePropertyDisplay();
                    OnPropertyChanged(() => SelectedPropertyModel);
                    OnPropertyChanged(() => AcceptButtonMessage);
                    UpdateFormFields();
                    UpdateEmployeesForSelectedProperty();
                }
            }
        }

        private string? _propertyDisplay;
        public string PropertyDisplay
        {
            get => _propertyDisplay ?? "Creating new";
            set
            {
                if(value != _propertyDisplay)
                {
                    _propertyDisplay = value;
                    OnPropertyChanged(() => PropertyDisplay);
                }
            }
        }
        #endregion

        #region EmployeeModels For New Property
        private ObservableCollection<EmployeeDTO> _currentPropertyEmployeeModels = default!;
        public ObservableCollection<EmployeeDTO> CurrentPropertyEmployeeModels
        {
            get => _currentPropertyEmployeeModels;
            set
            {
                if (value != _currentPropertyEmployeeModels)
                {
                    _currentPropertyEmployeeModels = value;
                    OnPropertyChanged(() => CurrentPropertyEmployeeModels);
                }
            }
        }

        private EmployeeDTO? _currentPropertySelectedEmployeeModel;
        public EmployeeDTO? CurrentPropertySelectedEmployeeModel
        {
            get => _currentPropertySelectedEmployeeModel;
            set
            {
                if (value != _currentPropertySelectedEmployeeModel)
                {
                    _currentPropertySelectedEmployeeModel = value;
                    OnPropertyChanged(() => CurrentPropertySelectedEmployeeModel);
                }
            }
        }
        #endregion

        #region EmployeeModels From Old Property
        private ObservableCollection<EmployeeDTO> _oldPropertyEmployeeModels = default!;
        public ObservableCollection<EmployeeDTO> OldPropertyEmployeeModels
        {
            get => _oldPropertyEmployeeModels;
            set
            {
                if (value != _oldPropertyEmployeeModels)
                {
                    _oldPropertyEmployeeModels = value;
                    OnPropertyChanged(() => OldPropertyEmployeeModels);
                }
            }
        }
        #endregion

        #endregion

        public PropertySelectorWithEmployeesViewModel(string displayName) : base(displayName)
        {
            AdditionalService = new ManyServiceClass();
            BaseCreationModel = new DtoClass() { Id = 0, Title = "Create NEW" };

            CreateNewModelCommand = new BaseCommand(() => SelectedPropertyModel = BaseCreationModel);
            SelectAvailableEmployeeCommand = new BaseCommand(() => WindowManager.OpenWindow(new AvailableEmployeesWithCallbackViewModel(this, CurrentPropertyEmployeeModels)));

            AddSelectedEmployeeToPropertyCommand = new BaseCommand(() => AddSelectedEmployeeToProperty());
            RemoveSelectedEmployeeFromPropertyCommand = new BaseCommand(() => DeleteCurrentSelectedEmployeeFromProperty());
            RefreshCommand = new BaseCommand(() => Refresh());
            EmployeeModels = new ObservableCollection<EmployeeDTO>(EmployeeService.GetModels());
            InitializeAllPropertiesAndModels();

            WeakReferenceMessenger.Default.Register<SelectedObjectMessage<DtoClass>>(this, (recipient, message) => GetSelectedProperty(message));
        }

        #region Overrided Methods
        public override void Save()
        {
            // Create new Property and additional Employee...
            if (AcceptButtonMessage.Equals(_CREATE))
            {
                ValidationOfData();
                CreateModelAndAddAllEmployees();

                Refresh();

                MessageColor = "Green";
                MessageLabel = $"{Model.GetType().Name} added successfully!";
            }

            // Update Property and additionally Employee...
            if (AcceptButtonMessage.Equals(_CHANGE))
            {
                ValidationOfData();
                UpdateModelAndAllItsEmployees();

                UpdateFormFields();
                UpdateEmployeesForSelectedProperty();

                MessageColor = "Green";
                MessageLabel = $"{Model.GetType().Name} updated successfully!";
            }
        }
        protected override void UpdateSelectedModel()
        {
            if (SelectedEmployeeModel != null)
            {
                EmployeeModel = EmployeeService.GetModel(SelectedEmployeeModel.Id);
            }
        }
        protected override void GetSelectedEmployee(SelectedObjectMessage<EmployeeDTO> message)
        {
            if (message.WhoRequestedToSelect == this && message.SelectedObject.GetType() == typeof(EmployeeDTO))
            {
                SelectedEmployeeModel = message.SelectedObject;
                EmployeeDisplay = $"{SelectedEmployeeModel.Firstname} {SelectedEmployeeModel.Surname}";
            }
        }
        #endregion

        #region Abstract Methods
        protected abstract void ValidationOfData();

        protected abstract void UpdatePropertyDisplay();
        #endregion

        #region Methods
        private void GetSelectedProperty(SelectedObjectMessage<DtoClass> message)
        {
            if (message.WhoRequestedToSelect == this && message.SelectedObject.GetType() != typeof(EmployeeDTO))
            {
                SelectedPropertyModel = message.SelectedObject;
            }
        }

        protected void InitializeAllPropertiesAndModels()
        {
            SelectedPropertyModel = BaseCreationModel;
            
            CurrentPropertyEmployeeModels = new ObservableCollection<EmployeeDTO>();
            OldPropertyEmployeeModels = new ObservableCollection<EmployeeDTO>();

        }

        protected void UpdateEmployeesForSelectedProperty()
        {
            if(SelectedPropertyModel == BaseCreationModel)
            {
                CurrentPropertyEmployeeModels = new ObservableCollection<EmployeeDTO>();
                OldPropertyEmployeeModels = new ObservableCollection<EmployeeDTO>();
                EmployeeModels = new ObservableCollection<EmployeeDTO>(EmployeeService.GetModels());
                MessageLabel = "";
                return;
            }
            if (SelectedPropertyModel == null)
            {
                MessageColor = "Red";
                MessageLabel = "Cannot get Employees for current Property!";
                return;
            }
            else
            {
                MessageLabel = "";
            }

            CurrentPropertyEmployeeModels = new ObservableCollection<EmployeeDTO>(Service.GetEmployeeModelsOfProperty(Model));
            OldPropertyEmployeeModels = new ObservableCollection<EmployeeDTO>(CurrentPropertyEmployeeModels);

            List<EmployeeDTO> tempEmployeesList = new List<EmployeeDTO>(EmployeeService.GetModels());
            foreach (EmployeeDTO item in CurrentPropertyEmployeeModels)
            {
                EmployeeDTO? searchedItem = tempEmployeesList.Find(emp => emp.Id == item.Id);
                if (searchedItem != null)
                {
                    tempEmployeesList.Remove(searchedItem);
                }
            }
            EmployeeModels = new ObservableCollection<EmployeeDTO>(tempEmployeesList);

            CurrentPropertySelectedEmployeeModel = null;
            SelectedEmployeeModel = null;
        }

        private void AddSelectedEmployeeToProperty()
        {
            if (SelectedEmployeeModel != null)
            {
                CurrentPropertyEmployeeModels.Add(SelectedEmployeeModel);
                SelectedEmployeeModel = null;
                EmployeeDisplay = null;
            }
        }

        private void DeleteCurrentSelectedEmployeeFromProperty()
        {
            if (CurrentPropertySelectedEmployeeModel != null)
            {
                CurrentPropertyEmployeeModels.Remove(CurrentPropertySelectedEmployeeModel);
            }
        }

        private void Refresh()
        {
            InitializeNewModelAndService();
            SelectedPropertyModel = BaseCreationModel;
            InitializeAllPropertiesAndModels();
        }

        public void CreateModelAndAddAllEmployees()
        {
            Service.AddModel(Model);

            foreach (EmployeeDTO empItem in CurrentPropertyEmployeeModels)
            {
                Employee? emp = EmployeeService.GetModel(empItem.Id);
                if (emp != null)
                {
                    ParentPartialClass empProp = AdditionalService.CreateModel();

                    empProp.PropertyId = Model.Id;
                    empProp.EmployeeId = emp.Id;

                    AdditionalService.AddModel(empProp);
                }
                else
                {
                    MessageLabel = "Cannot add employee!";
                }
            }
        }

        public void UpdateModelAndAllItsEmployees()
        {
            Service.UpdateModel(Model);

            List<EmployeeDTO> currentEmpList = CurrentPropertyEmployeeModels.ToList();
            List<EmployeeDTO> oldEmpList = OldPropertyEmployeeModels.ToList();

            // Deleting EmployeeProperties that have been removed from list
            oldEmpList.ForEach(item =>
            {
                if (!currentEmpList.Contains(item))
                {
                    Employee emp = EmployeeService.GetModel(item.Id);
                    ParentPartialClass empProp = AdditionalService.GetModelForSpecificParentAndEmployee(Model, emp);

                    AdditionalService.DeleteModel(empProp);
                }
            });

            // Adding EmployeeProperties that have been added to list
            CurrentPropertyEmployeeModels.ToList().ForEach(item =>
            {
                if (!OldPropertyEmployeeModels.Contains(item))
                {
                    ParentPartialClass empProp = AdditionalService.CreateModel();
                    empProp.PropertyId = Model.Id;
                    empProp.EmployeeId = item.Id;

                    AdditionalService.AddModel(empProp);
                }
            });
        }

        #endregion

    }
}
