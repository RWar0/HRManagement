using CommunityToolkit.Mvvm.Messaging;
using HRManagement.Helpers;
using HRManagement.Models;
using HRManagement.Models.Contexts;
using HRManagement.Models.DTOs;
using HRManagement.Models.Services;
using HRManagement.ViewModels.Single;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.ViewModels.Many
{
    public class SkillsViewModel : BaseManyViewModel<SkillService, Skill, SkillDTO>
    {
        #region Filtration Properties
        public string TitleName
        {
            get => Service.TitleName ?? "";
            set
            {
                if(value != Service.TitleName)
                {
                    Service.TitleName = value;
                    OnPropertyChanged(() => TitleName);
                }
            }
        }
        #endregion

        public SkillsViewModel() : base("Skills")
        {

        }

        protected override void CreateNewWindow()
        {
            WeakReferenceMessenger.Default.Send<OpenViewMessage>(new OpenViewMessage()
            {
                Sender = this,
                ViewModelToBeOpened = new SkillViewModel()
            });
        }
        protected override void CleanFilters()
        {
            TitleName = "";
            SortProperty = SortOptions.FirstOrDefault()?.PropertyTitle;
            IsOrderDescending = false;
            Refresh();
        }
    }
}
