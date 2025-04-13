using HRManagement.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.Services
{
    public class EmployeeCareerService : BaseManyToManyTableService<EmployeeCareer, EmployeeCareerDTO, Career>
    {
        public override void AddModel(EmployeeCareer model)
        {
            try
            {
                Database.EmployeeCareers.Add(model);
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override void DeleteModel(EmployeeCareerDTO model)
        {
            try
            {
                EmployeeCareer employeCareer = Database.EmployeeCareers.First(item => item.Id == model.Id);
                employeCareer.IsActive = false;
                employeCareer.DeleteDateTime = DateTime.Now;
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }
        public override void DeleteModel(EmployeeCareer model)
        {
            try
            {
                EmployeeCareer employeCareer = Database.EmployeeCareers.First(item => item.Id == model.Id);
                employeCareer.IsActive = false;
                employeCareer.DeleteDateTime = DateTime.Now;
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override EmployeeCareer GetModel(int id) => Database.EmployeeCareers.First(item => item.Id == id);

        public override EmployeeCareer GetModelForSpecificParentAndEmployee(Career parentModel, Employee employee)
        {
            return Database.EmployeeCareers.Where(item => item.IsActive && item.Career == parentModel && item.Employee == employee).First();
        }

        public override List<EmployeeCareerDTO> GetModels()
        {
            return Database.EmployeeCareers.Where(item => item.IsActive).Select(item => new EmployeeCareerDTO()
            {
                Id = item.Id,
                Property = item.Career,
                Employee = item.Employee,
            }).Distinct().ToList();
        }

        public List<Career> GetCareersOfEmployee(int id)
        {
            return Database.EmployeeCareers.Include(item => item.Career)
                .Where(item => item.IsActive && item.EmployeeId == id)
                .Select(item => item.Career).ToList();
        }

        public override void UpdateModel(EmployeeCareer model)
        {
            try
            {
                model.EditDateTime = DateTime.Now;
                Database.EmployeeCareers.Update(model);
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override EmployeeCareer CreateModel()
        {
            return new EmployeeCareer()
            {
                IsActive = true,
                CreationDateTime = DateTime.Now,
            };
        }
    }
}