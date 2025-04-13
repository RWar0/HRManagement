using HRManagement.Helpers;
using HRManagement.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRManagement.ViewModels.Single
{
    public class BaseCreateViewModel<ServiceClass, ModelClass, DtoClass>
        : BaseServiceViewModel<ServiceClass, ModelClass, DtoClass>
        where ServiceClass : BaseService<ModelClass, DtoClass>, new()
        where ModelClass : class, new()
        where DtoClass : class
    {

        #region FieldsAndProperties

        #region Commands
        public ICommand SaveCommand { get; set; }
        #endregion

        #region Messages
        private string _messageLabel = "";
        public string MessageLabel
        {
            get => _messageLabel;
            set
            {
                if (value != _messageLabel)
                {
                    _messageLabel = value;
                    OnPropertyChanged(() => MessageLabel);
                }
            }
        }
        private string _messageColor = "Red";
        public string MessageColor
        {
            get => _messageColor;
            set
            {
                if (value != _messageColor)
                {
                    _messageColor = value;
                    OnPropertyChanged(() => MessageColor);
                }
            }
        }
        #endregion

        #region Model
        private ModelClass _model = default!;
        public ModelClass Model
        {
            get => _model;
            set
            {
                if (value != _model)
                {
                    _model = value;
                    OnPropertyChanged(() => Model);
                }
            }
        }
        #endregion

        #endregion

        public BaseCreateViewModel(string displayName) : base(displayName)
        {
            _model = Service.CreateModel();
            SaveCommand = new BaseCommand(() => Save());
        }

        #region Methods
        public virtual void Save()
        {
            Service.AddModel(Model);
        }

        public virtual void InitializeNewModelAndService()
        {
            Service = new ServiceClass();
            _model = Service.CreateModel();
        }
        #endregion
    }
}
