using HRManagement.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.Services
{
    public class EmployeeSkillService : BaseManyToManyTableService<EmployeSkill, EmployeeSkillDTO, Skill>
    {
        public override void AddModel(EmployeSkill model)
        {
            try
            {
                Database.EmployeSkills.Add(model);
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override void DeleteModel(EmployeeSkillDTO model)
        {
            try
            {
                EmployeSkill employeSkill = Database.EmployeSkills.First(item => item.Id == model.Id);
                employeSkill.IsActive = false;
                employeSkill.DeleteDateTime = DateTime.Now;
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }
        public override void DeleteModel(EmployeSkill model)
        {
            try
            {
                EmployeSkill employeSkill = Database.EmployeSkills.First(item => item.Id == model.Id);
                employeSkill.IsActive = false;
                employeSkill.DeleteDateTime = DateTime.Now;
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override EmployeSkill GetModel(int id) => Database.EmployeSkills.First(item => item.Id == id);

        public override EmployeSkill GetModelForSpecificParentAndEmployee(Skill parentModel, Employee employee)
        {
            return Database.EmployeSkills.Where(item => item.IsActive && item.Skill == parentModel && item.Employee == employee).First();
        }

        public override List<EmployeeSkillDTO> GetModels()
        {
            return Database.EmployeSkills.Where(item => item.IsActive).Select(item => new EmployeeSkillDTO()
            {
                Id = item.Id,
                Property = item.Skill,
                Employee = item.Employee,
            }).Distinct().ToList();
        }

        public override void UpdateModel(EmployeSkill model)
        {
            try
            {
                model.EditDateTime = DateTime.Now;
                Database.EmployeSkills.Update(model);
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override EmployeSkill CreateModel()
        {
            return new EmployeSkill()
            {
                IsActive = true,
                CreationDateTime = DateTime.Now,
            };
        }
    }
}
