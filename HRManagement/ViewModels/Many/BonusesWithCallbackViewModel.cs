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
    public class BonusesWithCallbackViewModel : BonusesViewModel
    {
        public object WhoRequestedToSelect { get; set; }

        public BonusesWithCallbackViewModel(object whoRequestedToSelect)
        {
            WhoRequestedToSelect = whoRequestedToSelect;
        }

        protected override void HandleSelect()
        {
            if (SelectedModel != null)
            {
                WeakReferenceMessenger.Default.Send(new SelectedObjectMessage<BonusDTO>(WhoRequestedToSelect, SelectedModel));
                OnRequestClose();
            }
        }
    }
}
