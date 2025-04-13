using HRManagement.Models.BusinessLogic;
using HRManagement.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.Services
{
    public class SkillService : BaseServiceWithEmployeeSearch<Skill, SkillDTO>
    {
        #region Filtration Properties
        public string? TitleName { get; set; }
        #endregion

        public override void AddModel(Skill model)
        {
            try
            {
                Database.Skills.Add(model);
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override void DeleteModel(SkillDTO model)
        {
            try
            {
                Skill skill = Database.Skills.First(item => item.Id == model.Id);
                skill.IsActive = false;
                skill.DeleteDateTime = DateTime.Now;
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override Skill GetModel(int id) => Database.Skills.First(item => item.Id == id);

        public override List<SkillDTO> GetModels()
        {
            IQueryable<Skill> skills = Database.Skills.Where(item => item.IsActive);

            if (!string.IsNullOrEmpty(TitleName))
            {
                skills = skills.Where(item => EF.Functions.Like(item.Title, $"%{TitleName}%"));
            }

            if (!string.IsNullOrEmpty(OrderProperty))
            {
                switch (OrderProperty)
                {
                    case nameof(Skill.Title):
                        skills = IsOrderDescending ? skills.OrderByDescending(item => item.Title) : skills.OrderBy(item => item.Title);
                        break;
                    case nameof(Skill.Id):
                        skills = IsOrderDescending ? skills.OrderByDescending(item => item.Id) : skills.OrderBy(item => item.Id);
                        break;
                }
            }

            return skills.Select(item => new SkillDTO()
            {
                Id = item.Id,
                Title = item.Title,
            }).ToList();
        }

        public override List<EmployeeDTO> GetEmployeeModelsOfProperty(Skill model)
        {
            return Database.EmployeSkills
                .Include(item => item.Employee)
                .Include(item => item.Employee.PersonalData)
                .Include(item => item.Employee.Salary)
                .Include(item => item.Employee.Position)
                .Where(item => item.IsActive && item.Skill == model)
                .Select(item => new EmployeeDTO
                {
                    Id = item.Employee.Id,
                    Firstname = item.Employee.Firstname,
                    Surname = item.Employee.Surname,
                    Gender = item.Employee.Gender,
                    EmploymentType = item.Employee.EmploymentType,
                    PositionName = item.Employee.Position.Title,
                    Salary = new SalaryBL
                    {
                        Id = item.Employee.Salary.Id,
                        BruttoPrice = item.Employee.Salary.BruttoPrice,
                        Declusions = item.Employee.Salary.Declusions,
                        TaxRate = item.Employee.Salary.TaxRate,
                        ZusTaxRate = item.Employee.Salary.ZusTaxRate,
                    },
                    Education = item.Employee.PersonalData.First().Education,
                }).ToList();
        }

        public override void UpdateModel(Skill model)
        {
            try
            {
                model.EditDateTime = DateTime.Now;
                Database.Skills.Update(model);
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override Skill CreateModel()
        {
            return new Skill() 
            {
                IsActive = true,
                CreationDateTime = DateTime.Now,
            };
        }

        public override List<SearchComboBoxDTO> GetOrderByComboBoxDtos()
        {
            return new List<SearchComboBoxDTO>
            {
                new SearchComboBoxDTO
                {
                    DisplayName = "Title",
                    PropertyTitle = nameof(Skill.Title),
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Skill Id",
                    PropertyTitle = nameof(Skill.Id),
                },
            };
        }
    }
}
