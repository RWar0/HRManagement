using HRManagement.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.Services
{
    public class EmployeeTrainingService : BaseManyToManyTableService<EmployeeTraining, EmployeeTrainingDTO, Training>
    {
        public override void AddModel(EmployeeTraining model)
        {
            try
            {
                Database.EmployeeTrainings.Add(model);
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override void DeleteModel(EmployeeTraining model)
        {
            try
            {
                EmployeeTraining employeeTraining = Database.EmployeeTrainings.First(item => item.Id == model.Id);
                employeeTraining.IsActive = false;
                employeeTraining.DeleteDateTime = DateTime.Now;
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override void DeleteModel(EmployeeTrainingDTO model)
        {
            try
            {
                EmployeeTraining employeeTraining = Database.EmployeeTrainings.First(item => item.Id == model.Id);
                employeeTraining.IsActive = false;
                employeeTraining.DeleteDateTime = DateTime.Now;
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override EmployeeTraining GetModel(int id) => Database.EmployeeTrainings.First(item => item.Id == id);

        public override EmployeeTraining GetModelForSpecificParentAndEmployee(Training parentModel, Employee employee)
        {
            return Database.EmployeeTrainings.Where(item => item.IsActive && item.Training == parentModel && item.Employee == employee).First();
        }

        public override List<EmployeeTrainingDTO> GetModels()
        {
            return Database.EmployeeTrainings.Where(item => item.IsActive).Select(item => new EmployeeTrainingDTO()
            {
                Id = item.Id,
                Property = item.Training,
                Employee = item.Employee,
            }).Distinct().ToList();
        }

        public override void UpdateModel(EmployeeTraining model)
        {
            try
            {
                model.EditDateTime = DateTime.Now;
                Database.EmployeeTrainings.Update(model);
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override EmployeeTraining CreateModel()
        {
            return new EmployeeTraining()
            {
                IsActive = true,
                CreationDateTime = DateTime.Now,
            };
        }
    }
}
