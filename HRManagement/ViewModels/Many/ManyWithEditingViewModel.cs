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
using System.Windows.Input;

namespace HRManagement.ViewModels.Many
{
    public abstract class ManyWithEditingViewModel<ServiceClass, ModelClass, DtoClass> : BaseManyViewModel<ServiceClass, ModelClass, DtoClass>
        where ServiceClass : BaseService<ModelClass, DtoClass>, new()
        where ModelClass : class, new()
        where DtoClass : class, IBaseModelDTO
    {

        #region Fields And Properties
        public ICommand EditCommand { get; set; }
        #endregion

        public ManyWithEditingViewModel(string displayName) : base(displayName)
        {
            EditCommand = new BaseCommand(() => HandleEdit());
        }

        #region Methods
        protected void HandleEdit()
        {
            if (SelectedModel != null)
            {
                ModelClass modelToSend = Service.GetModel(SelectedModel.Id);
                CreateNewWindow();
                WeakReferenceMessenger.Default.Send<EditModelMessage<ModelClass>>(new EditModelMessage<ModelClass>(this, modelToSend));
                OnRequestClose();
            }
        }
        #endregion

    }
}
