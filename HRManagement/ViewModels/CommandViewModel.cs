using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRManagement.ViewModels
{
    public class CommandViewModel : BaseViewModel
    {
        #region FieldAndProperties

        public string DisplayName { get; set; }
        public ICommand Command { get; set; }

        #endregion

        #region Konstruktor

        public CommandViewModel(string displayName, ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException("Command");
            this.DisplayName = displayName;
            this.Command = command;
        }

        #endregion
    }
}
