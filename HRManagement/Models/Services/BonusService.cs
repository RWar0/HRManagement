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
    public class BonusService : BaseServiceWithEmployeeSearch<Bonus, BonusDTO>
    {
        #region Filtration Properties
        public string? TitleName { get; set; }
        public string? DescriptionName { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public DateOnly? DateFrom { get; set; }
        public DateOnly? DateTo { get; set; }
        #endregion

        public override void AddModel(Bonus model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Description) || model.Description.Equals("-----"))
                {
                    model.Description = null;
                }
                Database.Bonuses.Add(model);
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override void DeleteModel(BonusDTO model)
        {
            try
            {
                Bonus bonus = Database.Bonuses.First(item => item.Id == model.Id);
                bonus.IsActive = false;
                bonus.DeleteDateTime = DateTime.Now;
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override List<EmployeeDTO> GetEmployeeModelsOfProperty(Bonus model)
        {
            return Database.EmployeeBonuses
                .Include(item => item.Employee)
                .Include(item => item.Employee.PersonalData)
                .Include(item => item.Employee.Salary)
                .Include(item => item.Employee.Position)
                .Where(item => item.IsActive && item.Bonus == model)
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

        public override Bonus GetModel(int id) => Database.Bonuses.First(item => item.Id == id);

        public override List<BonusDTO> GetModels()
        {
            IQueryable<Bonus> bonuses = Database.Bonuses.Where(item => item.IsActive);

            if (!string.IsNullOrEmpty(TitleName))
            {
                bonuses = bonuses.Where(item => EF.Functions.Like(item.Title, $"%{TitleName}%"));
            }
            if (!string.IsNullOrEmpty(DescriptionName))
            {
                bonuses = bonuses.Where(item => EF.Functions.Like(item.Description, $"%{DescriptionName}%"));
            }
            if (PriceFrom != null)
            {
                bonuses = bonuses.Where(item => item.Price >= PriceFrom);
            }
            if (PriceTo != null)
            {
                bonuses = bonuses.Where(item => item.Price <= PriceTo);
            }
            if (DateFrom != null)
            {
                bonuses = bonuses.Where(item => item.BeginDate >= DateFrom || item.EndDate >= DateFrom);
            }
            if (DateTo != null)
            {
                bonuses = bonuses.Where(item => item.BeginDate <= DateTo && item.EndDate <= DateTo);
            }

            if(!string.IsNullOrEmpty(OrderProperty))
            {
                switch (OrderProperty)
                {
                    case nameof(Bonus.Title):
                        bonuses = IsOrderDescending ? bonuses.OrderByDescending(item => item.Title) : bonuses.OrderBy(item => item.Title);
                        break;
                    case nameof(Bonus.Description):
                        bonuses = IsOrderDescending ? bonuses.OrderByDescending(item => item.Description) : bonuses.OrderBy(item => item.Description);
                        break;
                    case nameof(Bonus.Price):
                        bonuses = IsOrderDescending ? bonuses.OrderByDescending(item => item.Price) : bonuses.OrderBy(item => item.Price);
                        break;
                    case nameof(Bonus.BeginDate):
                        bonuses = IsOrderDescending ? bonuses.OrderByDescending(item => item.BeginDate) : bonuses.OrderBy(item => item.BeginDate);
                        break;
                    case nameof(Bonus.EndDate):
                        bonuses = IsOrderDescending ? bonuses.OrderByDescending(item => item.EndDate) : bonuses.OrderBy(item => item.EndDate);
                        break;
                }
            }

            return bonuses.Select(item => new BonusDTO()
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                Price = item.Price,
                BeginDate = item.BeginDate,
                EndDate = item.EndDate,
            }).ToList();
        }

        public override void UpdateModel(Bonus model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Description) || model.Description.Equals("-----"))
                {
                    model.Description = null;
                }

                model.EditDateTime = DateTime.Now;
                Database.Bonuses.Update(model);
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override Bonus CreateModel()
        {
            return new Bonus()
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
                    PropertyTitle = nameof(Bonus.Title)
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Description",
                    PropertyTitle = nameof(Bonus.Description),
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Price",
                    PropertyTitle = nameof(Bonus.Price),
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Begin date",
                    PropertyTitle = nameof(Bonus.BeginDate),
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "End date",
                    PropertyTitle = nameof(Bonus.EndDate),
                },
            };
        }
    }
}
