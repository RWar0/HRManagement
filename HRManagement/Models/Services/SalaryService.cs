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
    public class SalaryService : BaseService<Salary, SalaryBL>
    {
        public override void AddModel(Salary model)
        {
            try
            {
                Database.Salaries.Add(model);
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override void DeleteModel(SalaryBL model)
        {
            try
            {
                Salary? salary = Database.Salaries.FirstOrDefault(item => item.Id == model.Id);
                if (salary == null)
                {
                    System.Windows.MessageBox.Show($"Oops... Something went wrong...\nSalary cannot be deleted, because cannot be found in database!", "Error");
                    return;
                }
                salary.IsActive = false;
                salary.DeleteDateTime = DateTime.Now;
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public void DeleteById(int id)
        {
            try
            {
                Salary? salary = Database.Salaries.FirstOrDefault(item => item.Id == id);
                if (salary == null)
                {
                    System.Windows.MessageBox.Show($"Oops... Something went wrong...\nSalary cannot be deleted, because cannot be found in database!", "Error");
                    return;
                }
                salary.IsActive = false;
                salary.DeleteDateTime = DateTime.Now;
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override Salary GetModel(int id) => Database.Salaries.First(item => item.Id == id);

        public SalaryBL GetModelOfBL(int id)
        {
            return Database.Salaries.Where(item => item.Id == id).Select(item => new SalaryBL()
            {
                Id = item.Id,
                BruttoPrice = item.BruttoPrice,
                TaxRate = item.TaxRate,
                ZusTaxRate = item.ZusTaxRate,
                Declusions = item.Declusions,
                AdditionalDescription = item.Description,
            }).First();
        }

        public SalaryBL GetEmptyBLModel() => new SalaryBL();

        public override List<SalaryBL> GetModels()
        {
            return Database.Salaries.Where(item => item.IsActive).Select(item => new SalaryBL()
            {
                Id = item.Id,
                BruttoPrice = item.BruttoPrice,
                Declusions = item.Declusions,
                TaxRate = item.TaxRate,
                ZusTaxRate = item.ZusTaxRate,
                AdditionalDescription = item.Description,
            }).ToList();
        }

        public override void UpdateModel(Salary model)
        {
            try
            {
                model.EditDateTime = DateTime.Now;
                Database.Salaries.Update(model);
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override Salary CreateModel()
        {
            return new Salary()
            {
                IsActive = true,
                CreationDateTime = DateTime.Now,
            };
        }

        public string GetStringOfCalculatedNetto(Salary model)
        {
            try
            {
                return new SalaryBL()
                {
                    Id = model.Id,
                    BruttoPrice = model.BruttoPrice,
                    Declusions = model.Declusions,
                    TaxRate = model.TaxRate,
                    ZusTaxRate = model.ZusTaxRate,
                    AdditionalDescription = model.Description,
                }.NettoPrice + "";
            }
            catch (Exception)
            {
                return "N/A";
            }
        }
        public decimal GetCalculatedNetto(Salary model)
        {
            try
            {
                return new SalaryBL()
                {
                    Id = model.Id,
                    BruttoPrice = model.BruttoPrice,
                    Declusions = model.Declusions,
                    TaxRate = model.TaxRate,
                    ZusTaxRate = model.ZusTaxRate,
                    AdditionalDescription = model.Description,
                }.NettoPrice;
            }
            catch(Exception)
            {
                return -1;
            }
        }

        public decimal GetCalculatedBrutto(Salary model, decimal nettoValue)
        {
            try
            {
                SalaryBL tempSalary = new SalaryBL()
                {
                    Id = model.Id,
                    BruttoPrice = model.BruttoPrice,
                    Declusions = model.Declusions,
                    TaxRate = model.TaxRate,
                    ZusTaxRate = model.ZusTaxRate,
                    AdditionalDescription = model.Description,
                };

                return tempSalary.CalculateBruttoFromNetto(nettoValue);
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
