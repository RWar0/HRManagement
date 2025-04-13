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
    public class CareerService : BaseServiceWithEmployeeSearch<Career, CareerDTO>
    {
        #region Filtration Properties
        public string? TitleName { get; set; }
        public string? PositionName { get; set; }
        public DateOnly? DateFrom { get; set; }
        public DateOnly? DateTo { get; set; }
        #endregion

        public override void AddModel(Career model)
        {
            try
            {
                Database.Careers.Add(model);
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override void DeleteModel(CareerDTO model)
        {
            try
            {
                Career career = Database.Careers.First(item => item.Id == model.Id);
                career.IsActive = false;
                career.DeleteDateTime = DateTime.Now;
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override Career GetModel(int id) => Database.Careers.First(item => item.Id == id);

        public override List<CareerDTO> GetModels()
        {
            IQueryable<Career> careers = Database.Careers.Where(item => item.IsActive);

            if (!string.IsNullOrEmpty(TitleName))
            {
                careers = careers.Where(item => EF.Functions.Like(item.Title, $"%{TitleName}%"));
            }
            if (!string.IsNullOrEmpty(PositionName))
            {
                careers = careers.Where(item => EF.Functions.Like(item.Position, $"%{PositionName}%"));
            }
            if (DateFrom != null)
            {
                careers = careers.Where(item => item.BeginDate >= DateFrom || item.EndDate >= DateFrom);
            }
            if (DateTo != null)
            {
                careers = careers.Where(item => item.BeginDate <= DateTo && item.EndDate <= DateTo);
            }

            if (!string.IsNullOrEmpty(OrderProperty))
            {
                switch (OrderProperty)
                {
                    case nameof(Career.Title):
                        careers = IsOrderDescending ? careers.OrderByDescending(item => item.Title) : careers.OrderBy(item => item.Title);
                        break;
                    case nameof(Career.Position):
                        careers = IsOrderDescending ? careers.OrderByDescending(item => item.Position) : careers.OrderBy(item => item.Position);
                        break;
                    case nameof(Career.BeginDate):
                        careers = IsOrderDescending ? careers.OrderByDescending(item => item.BeginDate) : careers.OrderBy(item => item.BeginDate);
                        break;
                    case nameof(Career.EndDate):
                        careers = IsOrderDescending ? careers.OrderByDescending(item => item.EndDate) : careers.OrderBy(item => item.EndDate);
                        break;
                    case nameof(Career.Id):
                        careers = IsOrderDescending ? careers.OrderByDescending(item => item.Id) : careers.OrderBy(item => item.Id);
                        break;
                }
            }

            return careers.Select(item => new CareerDTO()
            {
                Id = item.Id,
                Title = item.Title,
                Position = item.Position,
                BeginDate = item.BeginDate,
                EndDate = item.EndDate
            }).ToList();
        }

        public override List<EmployeeDTO> GetEmployeeModelsOfProperty(Career model)
        {
            return Database.EmployeeCareers
                .Include(item => item.Employee)
                .Include(item => item.Employee.PersonalData)
                .Include(item => item.Employee.Salary)
                .Include(item => item.Employee.Position)
                .Where(item => item.IsActive && item.Career == model)
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

        public override void UpdateModel(Career model)
        {
            try
            {
                model.EditDateTime = DateTime.Now;
                Database.Careers.Update(model);
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override Career CreateModel()
        {
            return new Career()
            {
                BeginDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now),
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
                    PropertyTitle = nameof(Career.Title)
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Position",
                    PropertyTitle = nameof(Career.Position),
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Begin date",
                    PropertyTitle = nameof(Career.BeginDate),
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "End date",
                    PropertyTitle = nameof(Career.EndDate),
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Career Id",
                    PropertyTitle = nameof(Career.Id),
                },
            };
        }
    }
}
