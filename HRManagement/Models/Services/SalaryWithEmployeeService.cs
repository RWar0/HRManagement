using HRManagement.Helpers.Enums;
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
    public class SalaryWithEmployeeService : BaseService<Salary, SalaryWithEmployeeDTO>
    {
        #region Filtration Properties
        public string? EmployeeName { get; set; }
        public string? EmployeeSurname { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public decimal? DeclusionsFrom { get; set; }
        public decimal? DeclusionsTo { get; set; }
        public double? TaxFrom { get; set; }
        public double? TaxTo { get; set; }
        public double? ZusTaxFrom { get; set; }
        public double? ZusTaxTo { get; set; }

        public YesNoFilterEnum DescriptionFilter { get; set; }
        #endregion

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

        public override void DeleteModel(SalaryWithEmployeeDTO model)
        {
            try
            {
                Salary salary = Database.Salaries.First(item => item.Id == model.Id);
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

        public override List<SalaryWithEmployeeDTO> GetModels()
        {
            IQueryable<Employee> employeeWithSalary = Database.Employees.Include(item => item.Salary).Where(item => item.IsActive);

            if (!string.IsNullOrEmpty(EmployeeName))
            {
                employeeWithSalary = employeeWithSalary.Where(item => EF.Functions.Like(item.Firstname, $"%{EmployeeName}%"));
            }
            if (!string.IsNullOrEmpty(EmployeeSurname))
            {
                employeeWithSalary = employeeWithSalary.Where(item => EF.Functions.Like(item.Surname, $"%{EmployeeSurname}%"));
            }
            if (PriceFrom != null)
            {
                employeeWithSalary = employeeWithSalary.Where(item => item.Salary.BruttoPrice >= PriceFrom);
            }
            if (PriceTo != null)
            {
                employeeWithSalary = employeeWithSalary.Where(item => item.Salary.BruttoPrice <= PriceTo);
            }
            if (TaxFrom != null)
            {
                employeeWithSalary = employeeWithSalary.Where(item => item.Salary.TaxRate >= TaxFrom / 100);
            }
            if (TaxTo != null)
            {
                employeeWithSalary = employeeWithSalary.Where(item => item.Salary.TaxRate <= TaxTo / 100);
            }
            if (ZusTaxFrom != null)
            {
                employeeWithSalary = employeeWithSalary.Where(item => item.Salary.ZusTaxRate >= ZusTaxFrom / 100);
            }
            if (ZusTaxTo != null)
            {
                employeeWithSalary = employeeWithSalary.Where(item => item.Salary.ZusTaxRate <= ZusTaxTo / 100);
            }
            if (DeclusionsFrom != null)
            {
                employeeWithSalary = employeeWithSalary.Where(item => item.Salary.Declusions >= DeclusionsFrom);
            }
            if (DeclusionsTo != null)
            {
                employeeWithSalary = employeeWithSalary.Where(item => item.Salary.Declusions <= DeclusionsTo);
            }

            switch(DescriptionFilter)
            {
                case YesNoFilterEnum.Yes:
                    employeeWithSalary = employeeWithSalary.Where(item => !string.IsNullOrEmpty(item.Salary.Description));
                    break;
                case YesNoFilterEnum.No:
                    employeeWithSalary = employeeWithSalary.Where(item => string.IsNullOrEmpty(item.Salary.Description));
                    break;
            }

            if (!string.IsNullOrEmpty(OrderProperty))
            {
                switch (OrderProperty)
                {
                    case nameof(Employee.Surname):
                        employeeWithSalary = IsOrderDescending ? employeeWithSalary.OrderByDescending(item => item.Surname) : employeeWithSalary.OrderBy(item => item.Surname);
                        break;
                    case nameof(Employee.Firstname):
                        employeeWithSalary = IsOrderDescending ? employeeWithSalary.OrderByDescending(item => item.Firstname) : employeeWithSalary.OrderBy(item => item.Firstname);
                        break;
                    case nameof(Salary.BruttoPrice):
                        employeeWithSalary = IsOrderDescending ? employeeWithSalary.OrderByDescending(item => item.Salary.BruttoPrice) : employeeWithSalary.OrderBy(item => item.Salary.BruttoPrice);
                        break;
                    case nameof(Salary.TaxRate):
                        employeeWithSalary = IsOrderDescending ? employeeWithSalary.OrderByDescending(item => item.Salary.TaxRate) : employeeWithSalary.OrderBy(item => item.Salary.TaxRate);
                        break;
                    case nameof(Salary.ZusTaxRate):
                        employeeWithSalary = IsOrderDescending ? employeeWithSalary.OrderByDescending(item => item.Salary.ZusTaxRate) : employeeWithSalary.OrderBy(item => item.Salary.ZusTaxRate);
                        break;
                    case nameof(Salary.Description):
                        employeeWithSalary = IsOrderDescending ? employeeWithSalary.OrderByDescending(item => item.Salary.Description) : employeeWithSalary.OrderBy(item => item.Salary.Description);
                        break;
                    case nameof(Salary.Id):
                        employeeWithSalary = IsOrderDescending ? employeeWithSalary.OrderByDescending(item => item.Salary.Id) : employeeWithSalary.OrderBy(item => item.Salary.Id);
                        break;
                }
            }

            return employeeWithSalary.Select(item => new SalaryWithEmployeeDTO()
            {
                Id = item.Id,
                EmployeeName = $"{item.Firstname} {item.Surname}",
                Salary = new SalaryBL()
                {
                    Id = item.Salary.Id,
                    BruttoPrice = item.Salary.BruttoPrice,
                    Declusions = item.Salary.Declusions,
                    TaxRate = item.Salary.TaxRate,
                    ZusTaxRate = item.Salary.ZusTaxRate,
                    AdditionalDescription = item.Salary.Description,
                }
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
            catch (Exception)
            {
                return -1;
            }
        }

        public override List<SearchComboBoxDTO> GetOrderByComboBoxDtos()
        {
            return new List<SearchComboBoxDTO>
            {
                new SearchComboBoxDTO
                {
                    DisplayName = "Employee Surname",
                    PropertyTitle = nameof(Employee.Surname)
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Employee Firstname",
                    PropertyTitle = nameof(Employee.Firstname)
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Brutto price",
                    PropertyTitle = nameof(Salary.BruttoPrice)
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Tax Rate",
                    PropertyTitle = nameof(Salary.TaxRate),
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Zus Tax Rate",
                    PropertyTitle = nameof(Salary.ZusTaxRate),
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Description",
                    PropertyTitle = nameof(Salary.Description),
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Salary Id",
                    PropertyTitle = nameof(Salary.Id),
                },
            };
        }
    }
}
