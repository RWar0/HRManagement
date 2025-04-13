using CommunityToolkit.Mvvm.Messaging;
using HRManagement.Helpers;
using HRManagement.Models;
using HRManagement.Models.DTOs;
using HRManagement.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.ViewModels.Single
{
    public abstract class CreationAndEditionWithEmployeeSelectorViewModel<ServiceClass, ModelClass, DtoClass> : CreationWithEmployeeSelectorViewModel<ServiceClass, ModelClass, DtoClass>
        where ServiceClass : BaseService<ModelClass, DtoClass>, new()
        where ModelClass : class, new()
        where DtoClass : class
    {

        #region Saving And Editing Properties
        protected bool IsEditing { get; set; }
        private const string _CREATE = "Create new";
        private const string _CHANGE = "Save changes";
        public string AcceptButtonMessage => IsEditing ? _CHANGE : _CREATE;

        public string CollapsedButtonProperty => IsEditing ? "Collapsed" : "Visible";
        #endregion

        public CreationAndEditionWithEmployeeSelectorViewModel(string displayName) : base(displayName) 
        {
            IsEditing = false;
            WeakReferenceMessenger.Default.Register<EditModelMessage<ModelClass>>(this, (recipient, message) => HandleEditing(message));
        }

        protected void HandleEditing(EditModelMessage<ModelClass> message)
        {
            IsEditing = true;
            Model = message.ModelToEdit;
            EditionFieldsUpdating();
            OnPropertyChanged(() => AcceptButtonMessage);
        }

        /// <summary>
        /// This Method should contains each Property that is visible and is need to be set and updated | OnPropertyChange(() => ____)
        /// </summary>
        protected abstract void EditionFieldsUpdating();

    }
}
