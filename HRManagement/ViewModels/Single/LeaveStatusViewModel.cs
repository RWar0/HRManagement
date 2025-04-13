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
    public class LeaveStatusViewModel : BaseCreateWithEditingViewModel<LeaveStatusService, LeaveStatus, LeaveStatusDTO>
    {

        #region Model Property
        public string LeaveStatusTitle
        {
            get => Model.Title;
            set
            {
                if (value != Model.Title)
                {
                    Model.Title = value;
                    OnPropertyChanged(() => LeaveStatusTitle);
                }
            }
        }
        #endregion

        public LeaveStatusViewModel() : base("Leave Status") { }

        public override void Save()
        {
            if (string.IsNullOrEmpty(LeaveStatusTitle))
            {
                MessageLabel = "Enter Title of Leave Type!";
                return;
            }

            if(IsEditing)
            {
                Service.UpdateModel(Model);
                OnRequestClose();
                WeakReferenceMessenger.Default.Send<OpenViewMessage>(new OpenViewMessage()
                {
                    Sender = this,
                    ViewModelToBeOpened = new LeaveStatusesViewModel()
                });
            }
            else
            {
                Service.AddModel(Model);

                MessageColor = "Green";
                MessageLabel = $"Successfully added leave status: {LeaveStatusTitle}";

                LeaveStatusTitle = "";
            }

            InitializeNewModelAndService();
        }

        protected override void EditionFieldsUpdating()
        {
            OnPropertyChanged(() => LeaveStatusTitle);
        }
    }
}
