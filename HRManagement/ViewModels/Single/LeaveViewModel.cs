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
using System.Windows;
using System.Windows.Input;

namespace HRManagement.ViewModels.Single
{
    public class LeaveViewModel : CreationAndEditionWithEmployeeSelectorViewModel<LeaveService, Leave, LeaveDTO>
    {
        #region FieldsAndProperties

        #region Additional Services
        private LeaveTypeService LeaveTypeService { get; set; } = default!;
        private LeaveStatusService LeaveStatusService { get; set; } = default!;
        #endregion

        #region Commands
        public ICommand RefreshCommand { get; set; }
        public ICommand SelectLeaveTypeCommand { get; set; }
        public ICommand SelectLeaveStatusCommand { get; set; }
        #endregion

        #region LeaveTypesModels
        private LeaveTypeDTO? _leaveTypeSelectedModel;
        public LeaveTypeDTO? LeaveTypeSelectedModel
        {
            get => _leaveTypeSelectedModel;
            set
            {
                if (value != _leaveTypeSelectedModel)
                {
                    _leaveTypeSelectedModel = value;
                    OnPropertyChanged(() => LeaveTypeSelectedModel);
                }
            }
        }
        private string? _leaveTypeSelectedName;
        public string? LeaveTypeSelectedName
        {
            get => _leaveTypeSelectedName ?? "Leave type not selected";
            set
            {
                if (value != _leaveTypeSelectedName)
                {
                    _leaveTypeSelectedName = value;
                    OnPropertyChanged(() => LeaveTypeSelectedName);
                }
            }
        }
        #endregion

        #region LeaveStatusesModels
        private LeaveStatusDTO? _leaveStatusSelectedModel;
        public LeaveStatusDTO? LeaveStatusSelectedModel
        {
            get => _leaveStatusSelectedModel;
            set
            {
                if (value != _leaveStatusSelectedModel)
                {
                    _leaveStatusSelectedModel = value;
                    OnPropertyChanged(() => LeaveStatusSelectedModel);
                }
            }
        }
        private string? _leaveStatusSelectedName;
        public string? LeaveStatusSelectedName
        {
            get => _leaveStatusSelectedName ?? "Leave status not selected";
            set
            {
                if (value != _leaveStatusSelectedName)
                {
                    _leaveStatusSelectedName = value;
                    OnPropertyChanged(() => LeaveStatusSelectedName);
                }
            }
        }
        #endregion

        #region LeaveProperties
        public string? Reason
        {
            get => Model.Reason;
            set
            {
                if (value != Model.Reason)
                {
                    Model.Reason = value;
                    OnPropertyChanged(() => Reason);
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
                    OnPropertyChanged(() => AvailableDaysAfterLeave);
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
                    OnPropertyChanged(() => AvailableDaysAfterLeave);
                }
            }
        }

        #endregion

        #region CheckBoxes
        private bool _isCheckedReasonCheckBox;
        public bool IsCheckedReasonCheckBox
        {
            get => _isCheckedReasonCheckBox;
            set
            {
                if (value != _isCheckedReasonCheckBox)
                {
                    _isCheckedReasonCheckBox = value;
                    OnPropertyChanged(() => IsCheckedReasonCheckBox);
                    OnPropertyChanged(() => IsReasonTextBoxEnabled);
                    OnPropertyChanged(() => ReasonTextBoxOpacity);
                    if (value)
                    {
                        Reason = "";
                    }
                }
            }
        }
        public bool IsReasonTextBoxEnabled => !IsCheckedReasonCheckBox;
        public string ReasonTextBoxOpacity => IsReasonTextBoxEnabled ? "1" : "0.3";

        #endregion

        #region Available days of leaves
        public string AvailableLeaveDays => EmployeeModel != null ? Service.GetQuantityOfAvailableLeaveDays(EmployeeModel) + "" : "---";
        public string AvailableDaysAfterLeave
        {
            get
            {
                if (EmployeeModel != null)
                {
                    if(IsEditing)
                    {
                        return Service.GetQuantityOfAvailableLeaveDaysAfterLeftWhenEditing(EmployeeModel, Model.Id, DateOnly.FromDateTime(BeginDate), DateOnly.FromDateTime(EndDate)) + "";
                    }
                    else
                    {
                        return Service.GetQuantityOfAvailableLeaveDaysAfterLeft(EmployeeModel, DateOnly.FromDateTime(BeginDate), DateOnly.FromDateTime(EndDate)) + "";
                    }
                }
                return "---";
            }
        }
        #endregion

        #endregion

        public LeaveViewModel() : base("Leave")
        {
            LeaveTypeService = new LeaveTypeService();
            LeaveStatusService = new LeaveStatusService();
            RefreshCommand = new BaseCommand(() => UpdateFormFields());

            // Handling Selecting and Recieving of LeaveType
            SelectLeaveTypeCommand = new BaseCommand(() => WindowManager.OpenWindow(new LeaveTypesWithCallbackViewModel(this)));
            WeakReferenceMessenger.Default.Register<SelectedObjectMessage<LeaveTypeDTO>>(this, (recipient, message) => GetSelectedLeaveType(message));

            // Handling Selecting and Recieving of LeaveStatus
            SelectLeaveStatusCommand = new BaseCommand(() => WindowManager.OpenWindow(new LeaveStatusesWithCallbackViewModel(this)));
            WeakReferenceMessenger.Default.Register<SelectedObjectMessage<LeaveStatusDTO>>(this, (recipient, message) => GetSelectedLeaveStatus(message));
        }

        #region Overrided Abstract Methods
        protected override void UpdateFormFields()
        {

            if (EmployeeModel == null)
            {
                MessageColor = "Red";
                MessageLabel = "Cannot parse Employee to Leave! Employee was not downloaded correctly!";
                return;
            }

            OnPropertyChanged(() => AvailableLeaveDays);
            OnPropertyChanged(() => AvailableDaysAfterLeave);
        }
        public override void Save()
        {
            MessageColor = "Red";
            if (SelectedEmployeeModel == null || EmployeeModel == null)
            {
                MessageLabel = "Select Employee!";
                return;
            }
            Model.EmployeeId = EmployeeModel.Id;

            if (LeaveTypeSelectedModel == null)
            {
                MessageLabel = "Select Leave Type!";
                return;
            }
            Model.LeaveTypeId = LeaveTypeSelectedModel.Id;

            if (IsReasonTextBoxEnabled && string.IsNullOrEmpty(Reason))
            {
                MessageLabel = "Enter Reason or select 'No reason'";
                return;
            }
            if (BeginDate > EndDate)
            {
                MessageLabel = "Begin Date cannot be later than End Date!";
                return;
            }
            if (LeaveStatusSelectedModel == null)
            {
                MessageLabel = "Select Leave Status!";
                return;
            }
            Model.LeaveStatusId = LeaveStatusSelectedModel.Id;

            if (AvailableLeaveDays.Equals("---"))
            {
                MessageLabel = "Available leave days was not correctly downloaded! Refresh and try again!";
                return;
            }
            if (AvailableDaysAfterLeave.Equals("---"))
            {
                MessageLabel = "Available leave days after adding this are not correctly calculated! Refresh and try again!";
                return;
            }

            // Checking for available days left after this
            // If negative - ask for confirmation
            try
            {
                int availableDays = int.Parse(AvailableDaysAfterLeave);

                if (availableDays < 0)
                {
                    MessageBoxResult result = MessageBox.Show(
                        "This employee exceeded number of available leaving days!\n" +
                        "Do you want to confirm and assign leave?\n\n" +
                        "The quantity of available days will be negative!",
                        "Confirmation",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.No)
                    {
                        MessageLabel = "Adding was aborted!";
                        return;
                    }
                }
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong during creating leave...\nContant the administrator of the program...", "Error");
            }


            if (IsEditing)
            {
                Service.UpdateModel(Model);
                OnRequestClose();
                WeakReferenceMessenger.Default.Send<OpenViewMessage>(new OpenViewMessage()
                {
                    Sender = this,
                    ViewModelToBeOpened = new LeavesViewModel()
                });
            }
            else
            {
                Service.AddModel(Model);

                SelectedEmployeeModel = null;
                LeaveTypeSelectedModel = null;
                LeaveStatusSelectedModel = null;
                IsCheckedReasonCheckBox = false;
                Reason = null;
                InitializeNewModelAndService();
                LeaveTypeSelectedModel = null;
                LeaveTypeSelectedName = null;
                LeaveStatusSelectedModel = null;
                LeaveStatusSelectedName = null;
                OnPropertyChanged(() => AvailableLeaveDays);
                OnPropertyChanged(() => AvailableDaysAfterLeave);
            }

            MessageColor = "Green";
            MessageLabel = "Leave added successfully!";
        }
        protected override void EditionFieldsUpdating()
        {
            SelectedEmployeeModel = EmployeeService.GetDtoModel(Model.EmployeeId);
            EmployeeDisplay = $"{SelectedEmployeeModel.Firstname} {SelectedEmployeeModel.Surname}";

            LeaveTypeSelectedModel = LeaveTypeService.GetDtoModel(Model.LeaveTypeId);
            LeaveTypeSelectedName = LeaveTypeSelectedModel.Title;

            LeaveStatusSelectedModel = LeaveStatusService.GetDtoModel(Model.LeaveStatusId);
            LeaveStatusSelectedName = LeaveStatusSelectedModel.Title;

            IsCheckedReasonCheckBox = string.IsNullOrEmpty(Reason);
            OnPropertyChanged(() => Reason);

            OnPropertyChanged(() => AvailableDaysAfterLeave);
            OnPropertyChanged(() => AvailableLeaveDays);
            OnPropertyChanged(() => BeginDate);
            OnPropertyChanged(() => EndDate);
        }

        #endregion

        #region Methods
        private void GetSelectedLeaveType(SelectedObjectMessage<LeaveTypeDTO> message)
        {
            if (message.WhoRequestedToSelect == this)
            {
                LeaveTypeSelectedModel = message.SelectedObject;
                LeaveTypeSelectedName = LeaveTypeSelectedModel.Title;
            }
        }
        private void GetSelectedLeaveStatus(SelectedObjectMessage<LeaveStatusDTO> message)
        {
            if (message.WhoRequestedToSelect == this)
            {
                LeaveStatusSelectedModel = message.SelectedObject;
                LeaveStatusSelectedName = LeaveStatusSelectedModel.Title;
            }
        }

        #endregion
    }
}
