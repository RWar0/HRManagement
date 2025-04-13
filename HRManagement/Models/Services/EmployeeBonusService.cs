using HRManagement.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.Services
{
    public class EmployeeBonusService : BaseManyToManyTableService<EmployeeBonus, EmployeeBonusDTO, Bonus>
    {
        public override void AddModel(EmployeeBonus model)
        {
            try
            {
                Database.EmployeeBonuses.Add(model);
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override void DeleteModel(EmployeeBonus model)
        {
            try
            {
                EmployeeBonus employeeBonus = Database.EmployeeBonuses.First(item => item.Id == model.Id);
                employeeBonus.IsActive = false;
                employeeBonus.DeleteDateTime = DateTime.Now;
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override void DeleteModel(EmployeeBonusDTO model)
        {
            try
            {
                EmployeeBonus employeeBonus = Database.EmployeeBonuses.First(item => item.Id == model.Id);
                employeeBonus.IsActive = false;
                employeeBonus.DeleteDateTime = DateTime.Now;
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override EmployeeBonus GetModel(int id) => Database.EmployeeBonuses.First(item => item.Id == id);

        public override EmployeeBonus GetModelForSpecificParentAndEmployee(Bonus parentModel, Employee employee)
        {
            return Database.EmployeeBonuses.Where(item => item.IsActive && item.Bonus == parentModel && item.Employee == employee).First();
        }

        public override List<EmployeeBonusDTO> GetModels()
        {
            return Database.EmployeeBonuses.Where(item => item.IsActive).Select(item => new EmployeeBonusDTO()
            {
                Id = item.Id,
                Property = item.Bonus,
                Employee = item.Employee,
            }).Distinct().ToList();
        }

        public override void UpdateModel(EmployeeBonus model)
        {
            try
            {
                model.EditDateTime = DateTime.Now;
                Database.EmployeeBonuses.Update(model);
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override EmployeeBonus CreateModel()
        {
            return new EmployeeBonus()
            {
                IsActive = true,
                CreationDateTime = DateTime.Now,
            };
        }
    }
}
