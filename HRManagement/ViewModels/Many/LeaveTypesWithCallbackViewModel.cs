using CommunityToolkit.Mvvm.Messaging;
using HRManagement.Helpers;
using HRManagement.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.ViewModels.Many
{
    public class LeaveTypesWithCallbackViewModel : LeaveTypesViewModel
    {
        public object WhoRequestedToSelect { get; set; } = default!;

        public LeaveTypesWithCallbackViewModel(object whoRequestedToSelect)
        {
            WhoRequestedToSelect = whoRequestedToSelect;
        }
        public LeaveTypesWithCallbackViewModel() { }

        protected override void HandleSelect()
        {
            if (SelectedModel != null)
            {
                WeakReferenceMessenger.Default.Send(new SelectedObjectMessage<LeaveTypeDTO>(WhoRequestedToSelect, SelectedModel));
                OnRequestClose();
            }
        }
    }
}
