using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRManagement.Models.SideMenu
{
    public class SubMenuItem
    {
        public string Title { get; set; } = null!;
        public ICommand OpenViewCommand { get; set; } = null!;

        public SubMenuItem(string title, ICommand openViewCommand) 
        {
            Title = title;
            OpenViewCommand = openViewCommand;
        }

    }
}
