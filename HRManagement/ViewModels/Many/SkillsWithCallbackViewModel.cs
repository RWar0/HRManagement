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
    public class SkillsWithCallbackViewModel : SkillsViewModel
    {
        public object WhoRequestedToSelect { get; set; }

        public SkillsWithCallbackViewModel(object whoRequestedToSelect)
        {
            WhoRequestedToSelect = whoRequestedToSelect;
        }

        protected override void HandleSelect()
        {
            if (SelectedModel != null)
            {
                WeakReferenceMessenger.Default.Send(new SelectedObjectMessage<SkillDTO>(WhoRequestedToSelect, SelectedModel));
                OnRequestClose();
            }
        }
    }
}
