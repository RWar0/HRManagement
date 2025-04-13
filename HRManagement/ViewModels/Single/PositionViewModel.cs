using CommunityToolkit.Mvvm.Messaging;
using HRManagement.Helpers;
using HRManagement.Models;
using HRManagement.Models.Contexts;
using HRManagement.Models.DTOs;
using HRManagement.Models.Services;
using HRManagement.ViewModels.Many;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRManagement.ViewModels.Single
{
    public class PositionViewModel : BaseCreateWithEditingViewModel<PositionService, Position, PositionDTO>
    {

        #region FieldsAndProperties
        public string PositionName 
        {  
            get => Model.Title; 
            set
            {
                if(value != Model.Title)
                {
                    Model.Title = value;
                    OnPropertyChanged(() => Model.Title);
                }
            }
        }
        public string? DepartmentName
        {
            get => Model.DepartmentName;
            set
            {
                if (value != Model.DepartmentName)
                {
                    Model.DepartmentName = value;
                    OnPropertyChanged(() => Model.DepartmentName);
                }
            }
        }

        private bool _isCkeckedDepartmentCheckBox;
        public bool IsCheckedDepartmentCheckBox
        {
            get => _isCkeckedDepartmentCheckBox;
            set
            {
                if( value != _isCkeckedDepartmentCheckBox)
                {
                    _isCkeckedDepartmentCheckBox = value;
                    OnPropertyChanged(() => IsCheckedDepartmentCheckBox);
                    OnPropertyChanged(() => IsDepartmentTextBoxEnabled);
                    if(value)
                    {
                        DepartmentName = "";
                    }
                }
            }
        }
        public bool IsDepartmentTextBoxEnabled => !_isCkeckedDepartmentCheckBox;
        #endregion

        public PositionViewModel() : base("Position")
        {

        }

        public override void Save()
        {
            MessageColor = "Red";
            if (string.IsNullOrEmpty(PositionName))
            {
                MessageLabel = "Enter Position name!";
                return;
            }
            if(!IsCheckedDepartmentCheckBox && string.IsNullOrEmpty(DepartmentName))
            {
                MessageLabel = "Enter Department name or Select 'Do not assign department'!";
                return;
            }
            if (IsCheckedDepartmentCheckBox && string.IsNullOrEmpty(DepartmentName))
            {
                DepartmentName = null;
            }

            if (IsEditing)
            {
                Service.UpdateModel(Model);
                OnRequestClose();
                WeakReferenceMessenger.Default.Send<OpenViewMessage>(new OpenViewMessage()
                {
                    Sender = this,
                    ViewModelToBeOpened = new PositionsViewModel()
                });
            }
            else
            {
                Service.AddModel(Model);

                MessageColor = "Green";
                MessageLabel = $"Successfully added position: {PositionName}";

                PositionName = "";
                DepartmentName = "";
            }
            InitializeNewModelAndService();
        }

        protected override void EditionFieldsUpdating()
        {
            OnPropertyChanged(() => PositionName);
            OnPropertyChanged(() => DepartmentName);
            if(string.IsNullOrEmpty(DepartmentName))
            {
                IsCheckedDepartmentCheckBox = true;
            }
        }
    }
}
