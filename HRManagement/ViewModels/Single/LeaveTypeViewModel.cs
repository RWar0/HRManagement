using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using HRManagement.Helpers;
using HRManagement.Models;
using HRManagement.Models.Contexts;
using HRManagement.Models.DTOs;
using HRManagement.Models.Services;
using HRManagement.ViewModels.Many;

namespace HRManagement.ViewModels.Single
{
    public class LeaveTypeViewModel : BaseCreateWithEditingViewModel<LeaveTypeService, LeaveType, LeaveTypeDTO>
    {

        #region FieldsAndProperties
        public string LeaveTypeTitle
        {
            get => Model.Title;
            set
            {
                if (value != Model.Title)
                {
                    Model.Title = value;
                    OnPropertyChanged(() => LeaveTypeTitle);
                }
            }
        }
        public string? LeaveTypeDescription
        {
            get => Model.Description;
            set
            {
                if (value != Model.Description)
                {
                    Model.Description = value;
                    OnPropertyChanged(() => LeaveTypeDescription);
                }
            }
        }

        private bool _isCkeckedDescriptionCheckBox = false;
        public bool IsCheckedDescriptionCheckBox
        {
            get => _isCkeckedDescriptionCheckBox;
            set
            {
                if (value != _isCkeckedDescriptionCheckBox)
                {
                    _isCkeckedDescriptionCheckBox = value;
                    OnPropertyChanged(() => IsCheckedDescriptionCheckBox);
                    OnPropertyChanged(() => IsDescriptionTextBoxEnabled);
                    if (value)
                    {
                        LeaveTypeDescription = "";
                    }
                }
            }
        }
        public bool IsDescriptionTextBoxEnabled => !_isCkeckedDescriptionCheckBox;

        #endregion


        public LeaveTypeViewModel() : base("Leave Type")
        {

        }


        #region Methods

        public override void Save()
        {
            MessageColor = "Red";
            if (string.IsNullOrEmpty(LeaveTypeTitle))
            {
                MessageLabel = "Enter Title of Leave Type!";
                return;
            }

            if(IsDescriptionTextBoxEnabled && string.IsNullOrEmpty(LeaveTypeDescription))
            {
                MessageLabel = "Enter Description or set 'No description' option!";
                return;
            }
            if(!IsDescriptionTextBoxEnabled && string.IsNullOrEmpty(LeaveTypeDescription))
            {
                LeaveTypeDescription = null;
            }

            if (IsEditing)
            {
                Service.UpdateModel(Model);
                OnRequestClose();
                WeakReferenceMessenger.Default.Send<OpenViewMessage>(new OpenViewMessage()
                {
                    Sender = this,
                    ViewModelToBeOpened = new LeaveTypesViewModel()
                });
            }
            else
            {
                Service.AddModel(Model);

                MessageColor = "Green";
                MessageLabel = $"Successfully added Leave type: {LeaveTypeTitle}";

                LeaveTypeTitle = "";
                LeaveTypeDescription = "";
            }

            InitializeNewModelAndService();
        }

        protected override void EditionFieldsUpdating()
        {
            OnPropertyChanged(() => LeaveTypeTitle);
            OnPropertyChanged(() => LeaveTypeDescription);
            if (string.IsNullOrEmpty(LeaveTypeDescription))
            {
                IsCheckedDescriptionCheckBox = true;
            }
        }

        #endregion
    }
}
