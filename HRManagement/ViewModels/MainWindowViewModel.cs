using CommunityToolkit.Mvvm.Messaging;
using HRManagement.Helpers;
using HRManagement.Models.SideMenu;
using HRManagement.ViewModels.Many;
using HRManagement.ViewModels.SideMenu;
using HRManagement.ViewModels.Single;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace HRManagement.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region TopAndSideMenuCommand

        // Position Single and Many Views
        public ICommand OpenPositionsViewCommand { get => new BaseCommand(() => CreateView(new PositionsViewModel())); }
        public ICommand OpenPositionViewCommand { get => new BaseCommand(() => CreateView(new PositionViewModel())); }

        // Employee Single and Many Views
        public ICommand OpenEmployeesViewCommand { get => new BaseCommand(() => CreateView(new EmployeesViewModel())); }
        public ICommand OpenEmployeesOnLeaveViewCommand { get => new BaseCommand(() => CreateView(new EmployeesOnLeaveViewModel())); }
        public ICommand OpenEmployeeViewCommand { get => new BaseCommand(() => CreateView(new EmployeeViewModel())); }

        // Leave Type Single and Many Views
        public ICommand OpenLeaveTypesViewCommand { get => new BaseCommand(() => CreateView(new LeaveTypesViewModel())); }
        public ICommand OpenLeaveTypeViewCommand { get => new BaseCommand(() => CreateView(new LeaveTypeViewModel())); }

        // Leave Status Single and Many Views
        public ICommand OpenLeaveStatusesViewCommand { get => new BaseCommand(() => CreateView(new LeaveStatusesViewModel())); }
        public ICommand OpenLeaveStatusViewCommand { get => new BaseCommand(() => CreateView(new LeaveStatusViewModel())); }

        // Leaves Single and Many Views
        public ICommand OpenLeavesViewCommand { get => new BaseCommand(() => CreateView(new LeavesViewModel())); }
        public ICommand OpenLeaveViewCommand { get => new BaseCommand(() => CreateView(new LeaveViewModel())); }

        // Skills Single and Many Views
        public ICommand OpenSkillsViewCommand { get => new BaseCommand(() => CreateView(new SkillsViewModel())); }
        public ICommand OpenSkillViewCommand { get => new BaseCommand(() => CreateView(new SkillViewModel())); }

        // Trainings Single and Many Views
        public ICommand OpenTrainingsViewCommand { get => new BaseCommand(() => CreateView(new TrainingsViewModel())); }
        public ICommand OpenTrainingViewCommand { get => new BaseCommand(() => CreateView(new TrainingViewModel())); }

        // Personal Data Single and Many Views
        public ICommand OpenPersonalDatasViewCommand { get => new BaseCommand(() => CreateView(new PersonalDatasViewModel())); }
        public ICommand OpenPersonalDataViewCommand { get => new BaseCommand(() => CreateView(new PersonalDataViewModel())); }

        // Adresses Single and Many Views
        public ICommand OpenAdressesViewCommand { get => new BaseCommand(() => CreateView(new AdressesViewModel())); }
        public ICommand OpenAdressViewCommand { get => new BaseCommand(() => CreateView(new AdressViewModel())); }

        // Salary Single and Many Views
        public ICommand OpenSalariesViewCommand { get => new BaseCommand(() => CreateView(new SalariesViewModel())); }
        public ICommand OpenSalaryViewCommand { get => new BaseCommand(() => CreateView(new SalaryViewModel())); }
        public ICommand OpenSalaryCalculatorViewCommand { get => new BaseCommand(() => CreateView(new SalaryCalculatorViewModel())); }

        // Promotions Single and Many Views
        public ICommand OpenPromotionsViewCommand { get => new BaseCommand(() => CreateView(new PromotionsViewModel())); }
        public ICommand OpenPromotionViewCommand { get => new BaseCommand(() => CreateView(new PromotionViewModel())); }

        // Bonuses Single and Many Views
        public ICommand OpenBonusesViewCommand { get => new BaseCommand(() => CreateView(new BonusesViewModel())); }
        public ICommand OpenBonusViewCommand { get => new BaseCommand(() => CreateView(new BonusViewModel())); }

        // Careers Single and Many Views
        public ICommand OpenCareersViewCommand { get => new BaseCommand(() => CreateView(new CareersViewModel())); }
        public ICommand OpenCareerViewCommand { get => new BaseCommand(() => CreateView(new CareerViewModel())); }

        #endregion

        public MainWindowViewModel()
        {
            MenuCategories = new(CreateMenuCategories());
            Workspaces = new();
            Workspaces.CollectionChanged += OnWorkspacesChanged;
            WeakReferenceMessenger.Default.Register<OpenViewMessage>(this, (recipent, message) => OpenViewMessagesHandler(message));
        }

        #region Buttons in side bar

        public ReadOnlyCollection<MenuCategoryViewModel> MenuCategories { get; set; }
        private List<MenuCategoryViewModel> CreateMenuCategories()
        {
            return new ()
            {
                new MenuCategoryViewModel()
                {
                    CategoryTitle = "Employees",
                    SubmenuItems = new List<SubMenuItem>()
                    {
                        new("List all", OpenEmployeesViewCommand),
                        new("Create new", OpenEmployeeViewCommand),

                        new("Personal datas list", OpenPersonalDatasViewCommand),
                        new("Edit Personal data", OpenPersonalDataViewCommand),
                    }
                },
                new MenuCategoryViewModel()
                {
                    CategoryTitle = "Employees Details",
                    SubmenuItems = new List<SubMenuItem>()
                    {
                        new("List adresses", OpenAdressesViewCommand),
                        new("Edit adress", OpenAdressViewCommand),

                        new("List careers", OpenCareersViewCommand),
                        new("Create/Edit careers", OpenCareerViewCommand),

                        new("List skills", OpenSkillsViewCommand),
                        new("Create/Edit skills", OpenSkillViewCommand),
                    }
                },
                new MenuCategoryViewModel()
                {
                    CategoryTitle = "Leaves",
                    SubmenuItems = new List<SubMenuItem>()
                    {
                        new("List all", OpenLeavesViewCommand),
                        new("Create new", OpenLeaveViewCommand),

                        new("Employees on Leave", OpenEmployeesOnLeaveViewCommand),

                        new("Leave types", OpenLeaveTypesViewCommand),
                        new("Create leave types", OpenLeaveTypeViewCommand),

                        new("Leave statuses", OpenLeaveStatusesViewCommand),
                        new("Create leave statuses", OpenLeaveStatusViewCommand),
                    }
                },
                new MenuCategoryViewModel()
                {
                    CategoryTitle = "Finances",
                    SubmenuItems = new List<SubMenuItem>()
                    {
                        new("Salary calculator", OpenSalaryCalculatorViewCommand),
                        new("List salaries", OpenSalariesViewCommand),
                        new("Change salary", OpenSalaryViewCommand),

                        new("List bonuses", OpenBonusesViewCommand),
                        new("Create bonus", OpenBonusViewCommand),
                    }
                },
                new MenuCategoryViewModel()
                {
                    CategoryTitle = "Workflow",
                    SubmenuItems = new List<SubMenuItem>()
                    {
                        new("List positions", OpenPositionsViewCommand),
                        new("Create position", OpenPositionViewCommand),

                        new("List trainings", OpenTrainingsViewCommand),
                        new("Create training", OpenTrainingViewCommand),
                        
                        new("List promotions", OpenPromotionsViewCommand),
                        new("Create promotion", OpenPromotionViewCommand),
                    }
                },
            };
        }
        #endregion

        #region Tabs

        public ObservableCollection<WorkspaceViewModel> Workspaces { get; set; }
        private void OnWorkspacesChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.NewItems)
                    workspace.RequestClose += onWorkspaceRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.OldItems)
                    workspace.RequestClose -= onWorkspaceRequestClose;
        }

        private void onWorkspaceRequestClose(object? sender, EventArgs e)
        {
            if (sender is WorkspaceViewModel workspace && workspace != null)
            {
                Workspaces.Remove(workspace);
            }
        }

        #endregion

        #region Helepers
        private void OpenViewMessagesHandler(OpenViewMessage message)
        {
            CreateView(message.ViewModelToBeOpened);
        }

        private void CreateView(WorkspaceViewModel workspace)
        {
            Workspaces.Add(workspace);
            SetActiveWorkspace(workspace);
        }

        private void CreateListView<WorkspaceViewModelType>() where WorkspaceViewModelType : WorkspaceViewModel, new()
        {
            WorkspaceViewModel? workspace = Workspaces.FirstOrDefault(vm => vm is WorkspaceViewModelType);
            if (workspace == null)
            {
                workspace = new WorkspaceViewModelType();
                Workspaces.Add(workspace);
            }
            SetActiveWorkspace(workspace);
        }

        private void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            Debug.Assert(Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }

        #endregion
    }
}
