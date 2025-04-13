using HRManagement.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRManagement.ViewModels
{
    public class WorkspaceViewModel : BaseViewModel
    {
        #region FieldsAndProperties
        public string DisplayName { get; set; }
        private BaseCommand _CloseCommand = default!;
        public ICommand CloseCommand
        {
            get
            {
                if (_CloseCommand == null)
                    _CloseCommand = new BaseCommand(() => this.OnRequestClose()); //this command calls method to close a tab
                return _CloseCommand;
            }
        }
        #endregion

        public WorkspaceViewModel(String displayName)
        {
            DisplayName = displayName;
        }


        #region RequestClose [event]
        public event EventHandler RequestClose = default!;
        public void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
        #endregion 

    }
}
