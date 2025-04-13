using HRManagement.ViewModels;
using HRManagement.ViewModels.Many;
using HRManagement.Views.Many;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HRManagement.Models.ModalWindows
{
    public static class WindowManager
    {

        public static void OpenWindow(WorkspaceViewModel workspaceViewModel)
        {
            Window window = new Window()
            {
                // Setting parent Window
                Owner = App.Current.MainWindow,
                Title = "HR Management - element selector",
                Width = 1280,
                Height = 720,
            };

            #region Performing opening of appropriate view
            if (workspaceViewModel.GetType() == typeof(EmployeesWithCallbackViewModel))
            {
                window.Content = new EmployeesView() { DataContext = workspaceViewModel };
            }
            if (workspaceViewModel.GetType() == typeof(AvailableEmployeesWithCallbackViewModel))
            {
                window.Content = new EmployeesView() { DataContext = workspaceViewModel };
            }
            if (workspaceViewModel.GetType() == typeof(BonusesWithCallbackViewModel))
            {
                window.Content = new BonusesView() { DataContext = workspaceViewModel };
            }
            if (workspaceViewModel.GetType() == typeof(CareersWithCallbackViewModel))
            {
                window.Content = new CareersView() { DataContext = workspaceViewModel };
            }
            if (workspaceViewModel.GetType() == typeof(SkillsWithCallbackViewModel))
            {
                window.Content = new SklillsView() { DataContext = workspaceViewModel };
            }
            if (workspaceViewModel.GetType() == typeof(TrainingsWithCallbackViewModel))
            {
                window.Content = new TrainingsView() { DataContext = workspaceViewModel };
            }
            if (workspaceViewModel.GetType() == typeof(PositionsWithCallbackViewModel))
            {
                window.Content = new PositionsView() { DataContext = workspaceViewModel };
            }
            if (workspaceViewModel.GetType() == typeof(LeaveStatusesWithCallbackViewModel))
            {
                window.Content = new LeaveStatusesView() { DataContext = workspaceViewModel };
            }
            if (workspaceViewModel.GetType() == typeof(LeaveTypesWithCallbackViewModel))
            {
                window.Content = new LeaveTypesView() { DataContext = workspaceViewModel };
            } 
            #endregion

            // executing closing window when OnRequestClose()
            workspaceViewModel.RequestClose += (sender, e) =>
            {
                window.Close();
            };

            App.Current.MainWindow.Opacity = 0.6;

            // executing setting Opacity of Parent window to 1 when closed
            window.Closing += (sender, e) =>
            {
                App.Current.MainWindow.Opacity = 1;
            };

            window.ShowDialog();
        }
    }
}
