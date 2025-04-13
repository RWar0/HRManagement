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
    public class EmployeeService : BaseService<Employee, EmployeeDTO>
    {
        #region Filtration Properties
        public string? EmployeeName { get; set; }
        public string? EmployeeSurname { get; set; }
        public string? SelectedGender { get; set; }
        public string? EducationFilter { get; set; }
        public string? EmploymentFilter { get; set; }
        public string? PositionFilter { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        #endregion

        public override void AddModel(Employee model)
        {
            try
            {
                Database.Employees.Add(model);
                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...\n{ex}", "Error");
            }
        }

        public override void DeleteModel(EmployeeDTO model)
        {
            try
            {
                Employee employee = Database.Employees.First(item => item.Id == model.Id);
                employee.IsActive = false;
                employee.DeleteDateTime = DateTime.Now;
                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...\n{ex}", "Error");
            }
        }

        public override Employee GetModel(int id) => Database.Employees.First(item => item.Id == id);
        public Employee GetAdvancedModel(int id) => Database.Employees.Include(item => item.Salary).Include(item => item.Position).First(item => item.Id == id);

        public EmployeeDTO GetDtoModel(int id)
        {
            return Database.Employees.Include(item => item.PersonalData).Include(item => item.Salary).Include(item => item.Position).Where(item => item.Id == id).Select(item => new EmployeeDTO
            {
                Id = item.Id,
                Firstname = item.Firstname,
                Surname = item.Surname,
                Gender = item.Gender,
                EmploymentType = item.EmploymentType,
                PositionName = item.Position.Title,
                Salary = new SalaryBL
                {
                    Id = item.Salary.Id,
                    BruttoPrice = item.Salary.BruttoPrice,
                    Declusions = item.Salary.Declusions,
                    TaxRate = item.Salary.TaxRate,
                    ZusTaxRate = item.Salary.ZusTaxRate,
                },
                Education = item.PersonalData.First().Education,
            }).First();
        }

        public override List<EmployeeDTO> GetModels()
        {
            IQueryable<Employee> employees = Database.Employees.Include(item => item.PersonalData).Include(item => item.Salary).Include(item => item.Position).Where(item => item.IsActive);

            if (!string.IsNullOrEmpty(EmployeeName))
            {
                employees = employees.Where(item => EF.Functions.Like(item.Firstname, $"%{EmployeeName}%"));
            }
            if (!string.IsNullOrEmpty(EmployeeSurname))
            {
                employees = employees.Where(item => EF.Functions.Like(item.Surname, $"%{EmployeeSurname}%"));
            }
            switch (SelectedGender)
            {
                case "Male":
                    employees = employees.Where(item => item.Gender.Equals("Male"));
                    break;
                case "Female":
                    employees = employees.Where(item => item.Gender.Equals("Female"));
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(EducationFilter))
            {
                employees = employees.Where(item => EF.Functions.Like(item.PersonalData.First().Education, $"%{EducationFilter}%"));
            }
            if (!string.IsNullOrEmpty(EmploymentFilter))
            {
                employees = employees.Where(item => EF.Functions.Like(item.EmploymentType, $"%{EmploymentFilter}%"));
            }
            if (!string.IsNullOrEmpty(PositionFilter))
            {
                employees = employees.Where(item => EF.Functions.Like(item.Position.Title, $"%{PositionFilter}%"));
            }
            if (PriceFrom != null)
            {
                employees = employees.Where(item => item.Salary.BruttoPrice >= PriceFrom);
            }
            if (PriceTo != null)
            {
                employees = employees.Where(item => item.Salary.BruttoPrice <= PriceTo);
            }

            if (!string.IsNullOrEmpty(OrderProperty))
            {
                switch (OrderProperty)
                {
                    case nameof(Employee.Firstname):
                        employees = IsOrderDescending ? employees.OrderByDescending(item => item.Firstname) : employees.OrderBy(item => item.Firstname);
                        break;
                    case nameof(Employee.Surname):
                        employees = IsOrderDescending ? employees.OrderByDescending(item => item.Surname) : employees.OrderBy(item => item.Surname);
                        break;
                    case "Education":
                        try
                        {
                            employees = IsOrderDescending ? employees.OrderByDescending(item => item.PersonalData.First().Education) : employees.OrderBy(item => item.PersonalData.First().Education);
                        }
                        catch (Exception) { }
                        break;
                    case nameof(Employee.Position):
                        employees = IsOrderDescending ? employees.OrderByDescending(item => item.Position.Title) : employees.OrderBy(item => item.Position.Title);
                        break;
                    case nameof(Employee.Salary.BruttoPrice):
                        employees = IsOrderDescending ? employees.OrderByDescending(item => item.Salary.BruttoPrice) : employees.OrderBy(item => item.Salary.BruttoPrice);
                        break;
                    case nameof(Employee.Id):
                        employees = IsOrderDescending ? employees.OrderByDescending(item => item.Id) : employees.OrderBy(item => item.Id);
                        break;
                }
            }

            return employees
                .Select(item => new EmployeeDTO
                {
                    Id = item.Id,
                    Firstname = item.Firstname,
                    Surname = item.Surname,
                    Gender = item.Gender,
                    EmploymentType = item.EmploymentType,
                    PositionName = item.Position.Title,
                    Salary = new SalaryBL
                    {
                        Id = item.Salary.Id,
                        BruttoPrice = item.Salary.BruttoPrice,
                        Declusions = item.Salary.Declusions,
                        TaxRate = item.Salary.TaxRate,
                        ZusTaxRate = item.Salary.ZusTaxRate,
                    },
                    Education = item.PersonalData.First().Education,
                }).ToList();
        }

        /// <summary>
        /// Method returns the list of employees except these from list passed as a parameter
        /// </summary>
        /// <param name="usedEmployees">List of Employees to be omitted</param>
        /// <returns>List of Employees except these passed as a parameter</returns>
        public List<EmployeeDTO> GetAvailableModels(List<EmployeeDTO> usedEmployees)
        {
            IQueryable<Employee> employees = Database.Employees.Include(item => item.PersonalData).Include(item => item.Salary).Include(item => item.Position).Where(item => item.IsActive);

            employees = employees.Where(emp => !usedEmployees.Select(item => item.Id).Contains(emp.Id));

            if (!string.IsNullOrEmpty(EmployeeName))
            {
                employees = employees.Where(item => EF.Functions.Like(item.Firstname, $"%{EmployeeName}%"));
            }
            if (!string.IsNullOrEmpty(EmployeeSurname))
            {
                employees = employees.Where(item => EF.Functions.Like(item.Surname, $"%{EmployeeSurname}%"));
            }
            switch (SelectedGender)
            {
                case "Male":
                    employees = employees.Where(item => item.Gender.Equals("Male"));
                    break;
                case "Female":
                    employees = employees.Where(item => item.Gender.Equals("Female"));
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(EducationFilter))
            {
                employees = employees.Where(item => EF.Functions.Like(item.PersonalData.First().Education, $"%{EducationFilter}%"));
            }
            if (!string.IsNullOrEmpty(EmploymentFilter))
            {
                employees = employees.Where(item => EF.Functions.Like(item.EmploymentType, $"%{EmploymentFilter}%"));
            }
            if (!string.IsNullOrEmpty(PositionFilter))
            {
                employees = employees.Where(item => EF.Functions.Like(item.Position.Title, $"%{PositionFilter}%"));
            }
            if (PriceFrom != null)
            {
                employees = employees.Where(item => item.Salary.BruttoPrice >= PriceFrom);
            }
            if (PriceTo != null)
            {
                employees = employees.Where(item => item.Salary.BruttoPrice <= PriceTo);
            }

            if (!string.IsNullOrEmpty(OrderProperty))
            {
                switch (OrderProperty)
                {
                    case nameof(Employee.Firstname):
                        employees = IsOrderDescending ? employees.OrderByDescending(item => item.Firstname) : employees.OrderBy(item => item.Firstname);
                        break;
                    case nameof(Employee.Surname):
                        employees = IsOrderDescending ? employees.OrderByDescending(item => item.Surname) : employees.OrderBy(item => item.Surname);
                        break;
                    case "Education":
                        try
                        {
                            employees = IsOrderDescending ? employees.OrderByDescending(item => item.PersonalData.First().Education) : employees.OrderBy(item => item.PersonalData.First().Education);
                        }
                        catch (Exception) { }
                        break;
                    case nameof(Employee.Position):
                        employees = IsOrderDescending ? employees.OrderByDescending(item => item.Position.Title) : employees.OrderBy(item => item.Position.Title);
                        break;
                    case nameof(Employee.Salary.BruttoPrice):
                        employees = IsOrderDescending ? employees.OrderByDescending(item => item.Salary.BruttoPrice) : employees.OrderBy(item => item.Salary.BruttoPrice);
                        break;
                    case nameof(Employee.Id):
                        employees = IsOrderDescending ? employees.OrderByDescending(item => item.Id) : employees.OrderBy(item => item.Id);
                        break;
                }
            }

            return employees
                .Select(item => new EmployeeDTO
                {
                    Id = item.Id,
                    Firstname = item.Firstname,
                    Surname = item.Surname,
                    Gender = item.Gender,
                    EmploymentType = item.EmploymentType,
                    PositionName = item.Position.Title,
                    Salary = new SalaryBL
                    {
                        Id = item.Salary.Id,
                        BruttoPrice = item.Salary.BruttoPrice,
                        Declusions = item.Salary.Declusions,
                        TaxRate = item.Salary.TaxRate,
                        ZusTaxRate = item.Salary.ZusTaxRate,
                    },
                    Education = item.PersonalData.First().Education,
                }).ToList();
        }
        
        public List<EmployeeDTO> GetModelsOnLeave()
        {
            DateOnly todayDate = DateOnly.FromDateTime(DateTime.Now);

            HashSet<int> setOfIds = new(Database.Leaves.Where(item => item.IsActive && item.BeginDate <= todayDate && item.EndDate >= todayDate).Select(item => item.Id));

            IQueryable<Employee> employees = Database.Employees.Include(item => item.PersonalData).Include(item => item.Salary).Include(item => item.Position).Where(item => item.IsActive);
            employees = Database.Employees.Where(item => setOfIds.Contains(item.Id));

            if (!string.IsNullOrEmpty(EmployeeName))
            {
                employees = employees.Where(item => EF.Functions.Like(item.Firstname, $"%{EmployeeName}%"));
            }
            if (!string.IsNullOrEmpty(EmployeeSurname))
            {
                employees = employees.Where(item => EF.Functions.Like(item.Surname, $"%{EmployeeSurname}%"));
            }
            switch (SelectedGender)
            {
                case "Male":
                    employees = employees.Where(item => item.Gender.Equals("Male"));
                    break;
                case "Female":
                    employees = employees.Where(item => item.Gender.Equals("Female"));
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(EducationFilter))
            {
                employees = employees.Where(item => EF.Functions.Like(item.PersonalData.First().Education, $"%{EducationFilter}%"));
            }
            if (!string.IsNullOrEmpty(EmploymentFilter))
            {
                employees = employees.Where(item => EF.Functions.Like(item.EmploymentType, $"%{EmploymentFilter}%"));
            }
            if (!string.IsNullOrEmpty(PositionFilter))
            {
                employees = employees.Where(item => EF.Functions.Like(item.Position.Title, $"%{PositionFilter}%"));
            }
            if (PriceFrom != null)
            {
                employees = employees.Where(item => item.Salary.BruttoPrice >= PriceFrom);
            }
            if (PriceTo != null)
            {
                employees = employees.Where(item => item.Salary.BruttoPrice <= PriceTo);
            }

            if (!string.IsNullOrEmpty(OrderProperty))
            {
                switch (OrderProperty)
                {
                    case nameof(Employee.Firstname):
                        employees = IsOrderDescending ? employees.OrderByDescending(item => item.Firstname) : employees.OrderBy(item => item.Firstname);
                        break;
                    case nameof(Employee.Surname):
                        employees = IsOrderDescending ? employees.OrderByDescending(item => item.Surname) : employees.OrderBy(item => item.Surname);
                        break;
                    case "Education":
                        try
                        {
                            employees = IsOrderDescending ? employees.OrderByDescending(item => item.PersonalData.First().Education) : employees.OrderBy(item => item.PersonalData.First().Education);
                        }
                        catch (Exception) { }
                        break;
                    case nameof(Employee.Position):
                        employees = IsOrderDescending ? employees.OrderByDescending(item => item.Position.Title) : employees.OrderBy(item => item.Position.Title);
                        break;
                    case nameof(Employee.Salary.BruttoPrice):
                        employees = IsOrderDescending ? employees.OrderByDescending(item => item.Salary.BruttoPrice) : employees.OrderBy(item => item.Salary.BruttoPrice);
                        break;
                    case nameof(Employee.Id):
                        employees = IsOrderDescending ? employees.OrderByDescending(item => item.Id) : employees.OrderBy(item => item.Id);
                        break;
                }
            }

            return employees
                .Select(item => new EmployeeDTO
                {
                    Id = item.Id,
                    Firstname = item.Firstname,
                    Surname = item.Surname,
                    Gender = item.Gender,
                    EmploymentType = item.EmploymentType,
                    PositionName = item.Position.Title,
                    Salary = new SalaryBL
                    {
                        Id = item.Salary.Id,
                        BruttoPrice = item.Salary.BruttoPrice,
                        Declusions = item.Salary.Declusions,
                        TaxRate = item.Salary.TaxRate,
                        ZusTaxRate = item.Salary.ZusTaxRate,
                    },
                    Education = item.PersonalData.First().Education,
                }).ToList();
        }

        public override void UpdateModel(Employee model)
        {
            try
            {
                model.EditDateTime = DateTime.Now;
                Database.Employees.Update(model);
                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...\n{ex}", "Error");
            }
        }

        public override Employee CreateModel()
        {
            return new Employee()
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
                    DisplayName = "Surname",
                    PropertyTitle = nameof(Employee.Surname)
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Firstname",
                    PropertyTitle = nameof(Employee.Firstname),
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Education",
                    PropertyTitle = "Education",
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Position",
                    PropertyTitle = nameof(Employee.Position.Title),
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Brutto salary",
                    PropertyTitle = nameof(Employee.Salary.BruttoPrice),
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Employee Id",
                    PropertyTitle = nameof(Employee.Id),
                },
            };
        }
    }
}
