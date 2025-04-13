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
    public class PositionsWithCallbackViewModel : PositionsViewModel
    {
        public object WhoRequestedToSelect { get; set; } = default!;

        public PositionsWithCallbackViewModel(object whoRequestedToSelect)
        {
            WhoRequestedToSelect = whoRequestedToSelect;
        }
        public PositionsWithCallbackViewModel() { }

        protected override void HandleSelect()
        {
            if (SelectedModel != null)
            {
                WeakReferenceMessenger.Default.Send(new SelectedObjectMessage<PositionDTO>(WhoRequestedToSelect, SelectedModel));
                OnRequestClose();
            }
        }
    }
}
