using HRManagement.Helpers;
using HRManagement.Models.SideMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace HRManagement.ViewModels.SideMenu
{
    public class MenuCategoryViewModel : BaseViewModel
    {
        public string CategoryTitle { get; set; } = null!;
        public List<SubMenuItem> SubmenuItems { get; set; }

        public bool IsCollapsed { get; set; }
        public string CollapsedProperty => IsCollapsed ? "Collapsed" : "Visible";
        public ICommand CollapseCommand { get; set; } = default!;

        public MenuCategoryViewModel() 
        {
            IsCollapsed = true;
            CollapseCommand = new BaseCommand(() => Collapse());
            SubmenuItems = new List<SubMenuItem>();
        }

        private void Collapse()
        {
            IsCollapsed = !IsCollapsed;
            OnPropertyChanged(() => CollapsedProperty);
        }
    }
}
