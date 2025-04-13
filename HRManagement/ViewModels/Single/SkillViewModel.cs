using HRManagement.Helpers;
using HRManagement.Models;
using HRManagement.Models.Contexts;
using HRManagement.Models.DTOs;
using HRManagement.Models.ModalWindows;
using HRManagement.Models.Services;
using HRManagement.ViewModels.Many;
using Microsoft.EntityFrameworkCore;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRManagement.ViewModels.Single
{
    public class SkillViewModel : PropertySelectorWithEmployeesViewModel<SkillService, EmployeeSkillService, EmployeSkill, EmployeeSkillDTO, Skill, SkillDTO>
    {

        #region FieldsAndProperties
        public string SkillName
        {
            get => Model.Title;
            set
            {
                if (value != Model.Title)
                {
                    Model.Title = value;
                    OnPropertyChanged(() => SkillName);
                }
            }
        }
        #endregion

        public SkillViewModel() : base("Skill")
        {
            SelectPropertyModelCommand = new BaseCommand(() => WindowManager.OpenWindow(new SkillsWithCallbackViewModel(this)));
        }


        #region Overrided Abstract Methods
        protected override void UpdateFormFields()
        {
            OnPropertyChanged(() => SkillName);
        }
        protected override void ValidationOfData()
        {
            MessageColor = "Red";
            if (string.IsNullOrEmpty(SkillName))
            {
                MessageLabel = "Enter Skill Name!";
                return;
            }
        }
        protected override void UpdatePropertyDisplay()
        {
            PropertyDisplay = Model.Title;
        }

        #endregion

    }
}
