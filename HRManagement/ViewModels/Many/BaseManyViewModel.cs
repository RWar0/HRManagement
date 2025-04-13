using HRManagement.Helpers;
using HRManagement.Models.DTOs;
using HRManagement.Models.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRManagement.ViewModels.Many
{
    public abstract class BaseManyViewModel<ServiceClass, ModelClass, DtoClass>
        : BaseServiceViewModel<ServiceClass, ModelClass, DtoClass>
        where ServiceClass : BaseService<ModelClass, DtoClass>, new()
        where ModelClass : class, new()
        where DtoClass : class
    {
        #region Commands
        public ICommand RefreshCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand CreateNewWindowCommand { get; set; }
        public ICommand CleanFiltersCommand {  get; set; }
        #endregion

        #region Properties
        private ObservableCollection<DtoClass> _models = default!;
        public ObservableCollection<DtoClass> Models
        {
            get => _models;
            set
            {
                if (value != _models)
                {
                    _models = value;
                    OnPropertyChanged(() => Models);
                }
            }
        }

        private DtoClass? _selectedModel;
        public virtual DtoClass? SelectedModel
        {
            get => _selectedModel;
            set
            {
                if (value != _selectedModel)
                {
                    _selectedModel = value;
                    OnPropertyChanged(() => SelectedModel);
                    HandleSelect();
                }
            }
        }

        // Sorting
        public List<SearchComboBoxDTO> SortOptions { get; set; } = default!;
        public string? SortProperty
        {
            get => Service.OrderProperty;
            set
            {
                if (value != Service.OrderProperty)
                {
                    Service.OrderProperty = value;
                    OnPropertyChanged(() => SortProperty);
                }
            }
        }
        public bool IsOrderDescending
        {
            get => Service.IsOrderDescending;
            set
            {
                if (value != Service.IsOrderDescending)
                {
                    Service.IsOrderDescending = value;
                    OnPropertyChanged(() => IsOrderDescending);
                }
            }
        }
        #endregion

        public BaseManyViewModel(string displayName) : base(displayName)
        {
            RefreshCommand = new BaseCommand(() => Refresh());
            DeleteCommand = new BaseCommand(() => Delete());
            CreateNewWindowCommand = new BaseCommand(() => CreateNewWindow());
            CleanFiltersCommand = new BaseCommand(() => CleanFilters());

            SortOptions = Service.GetOrderByComboBoxDtos();
            SortProperty = SortOptions.FirstOrDefault()?.PropertyTitle;
            Refresh();
        }

        #region Methods
        protected virtual void Refresh()
        {
            Models = new ObservableCollection<DtoClass>(Service.GetModels());
        }
        private void Delete()
        {
            if (SelectedModel != null)
            {
                Service.DeleteModel(SelectedModel);
                Models.Remove(SelectedModel);
            }
        }
        #endregion

        #region AbstractMethods
        protected abstract void CreateNewWindow();
        protected abstract void CleanFilters();
        protected virtual void HandleSelect() { }
        #endregion
    }
}
