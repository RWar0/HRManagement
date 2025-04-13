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
    public class PromotionService : BaseService<Promotion, PromotionDTO>
    {
        #region Filtration Properties
        public string? EmployeeName { get; set; }
        public string? EmployeeSurname { get; set; }
        public YesNoFilterEnum PositionFilter { get; set; }
        public YesNoFilterEnum SalaryFilter { get; set; }
        public DateOnly? DateFrom { get; set; }
        public DateOnly? DateTo { get; set; }
        #endregion

        public override void AddModel(Promotion model)
        {
            try
            {
                Database.Promotions.Add(model);
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...\n", "Error");
            }
        }

        public override void DeleteModel(PromotionDTO model)
        {
            try
            {
                Promotion promotion = Database.Promotions.First(item => item.Id == model.Id);
                promotion.IsActive = false;
                promotion.DeleteDateTime = DateTime.Now;
                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...\n{ex}", "Error");
            }
        }

        public override Promotion GetModel(int id) => Database.Promotions.First(item => item.Id == id);

        public override List<PromotionDTO> GetModels()
        {
            IQueryable<Promotion> promotions = Database.Promotions.Include(item => item.Employee).Where(item => item.IsActive);

            if (!string.IsNullOrEmpty(EmployeeName))
            {
                promotions = promotions.Where(item => EF.Functions.Like(item.Employee.Firstname, $"%{EmployeeName}%"));
            }
            if (!string.IsNullOrEmpty(EmployeeSurname))
            {
                promotions = promotions.Where(item => EF.Functions.Like(item.Employee.Surname, $"%{EmployeeSurname}%"));
            }
            switch (PositionFilter)
            {
                case YesNoFilterEnum.Yes:
                    promotions = promotions.Where(item => item.OldPositionId != item.NewPositionId);
                    break;
                case YesNoFilterEnum.No:
                    promotions = promotions.Where(item => item.OldPositionId == item.NewPositionId);
                    break;
            }
            switch (SalaryFilter)
            {
                case YesNoFilterEnum.Yes:
                    promotions = promotions.Where(item => item.OldSalaryId != item.NewSalaryId);
                    break;
                case YesNoFilterEnum.No:
                    promotions = promotions.Where(item => item.OldSalaryId == item.NewSalaryId);
                    break;
            }
            if (DateFrom != null)
            {
                promotions = promotions.Where(item => item.PromotionDate >= DateFrom);
            }
            if (DateTo != null)
            {
                promotions = promotions.Where(item => item.PromotionDate <= DateTo);
            }

            if (!string.IsNullOrEmpty(OrderProperty))
            {
                switch (OrderProperty)
                {
                    case nameof(Promotion.Employee.Surname):
                        promotions = IsOrderDescending ? promotions.OrderByDescending(item => item.Employee.Surname) : promotions.OrderBy(item => item.Employee.Surname);
                        break;
                    case nameof(Promotion.Employee.Firstname):
                        promotions = IsOrderDescending ? promotions.OrderByDescending(item => item.Employee.Firstname) : promotions.OrderBy(item => item.Employee.Firstname);
                        break;
                    case nameof(Promotion.OldPosition):
                        promotions = IsOrderDescending ? promotions.OrderByDescending(item => item.OldPosition.Title) : promotions.OrderBy(item => item.OldPosition.Title);
                        break;
                    case nameof(Promotion.NewPosition):
                        promotions = IsOrderDescending ? promotions.OrderByDescending(item => item.NewPosition.Title) : promotions.OrderBy(item => item.NewPosition.Title);
                        break;
                    case nameof(Promotion.OldSalary):
                        promotions = IsOrderDescending ? promotions.OrderByDescending(item => item.OldSalary.BruttoPrice) : promotions.OrderBy(item => item.OldSalary.BruttoPrice);
                        break;
                    case nameof(Promotion.NewSalary):
                        promotions = IsOrderDescending ? promotions.OrderByDescending(item => item.NewSalary.BruttoPrice) : promotions.OrderBy(item => item.NewSalary.BruttoPrice);
                        break;
                    case nameof(Promotion.Id):
                        promotions = IsOrderDescending ? promotions.OrderByDescending(item => item.Id) : promotions.OrderBy(item => item.Id);
                        break;
                }
            }

            return promotions.Select(item => new PromotionDTO()
            {
                Id = item.Id,
                EmployeeName = $"{item.Employee.Firstname} {item.Employee.Surname}",
                PromotionDate = item.PromotionDate,
                OldPosition = item.OldPosition.Title,
                NewPosition = item.NewPosition.Title,
                OldSalary = new SalaryBL()
                {
                    Id = item.OldSalary.Id,
                    BruttoPrice = item.OldSalary.BruttoPrice,
                    TaxRate = item.OldSalary.TaxRate,
                    ZusTaxRate = item.OldSalary.ZusTaxRate,
                    Declusions = item.OldSalary.Declusions
                },
                NewSalary = new SalaryBL()
                {
                    Id = item.NewSalary.Id,
                    BruttoPrice = item.NewSalary.BruttoPrice,
                    TaxRate = item.NewSalary.TaxRate,
                    ZusTaxRate = item.NewSalary.ZusTaxRate,
                    Declusions = item.NewSalary.Declusions
                },
            }).ToList();
        }

        public override void UpdateModel(Promotion model)
        {
            try
            {
                model.EditDateTime = DateTime.Now;
                Database.Promotions.Update(model);
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...\n", "Error");
            }
        }

        public override Promotion CreateModel()
        {
            return new Promotion()
            {
                PromotionDate = DateOnly.FromDateTime(DateTime.Now),
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
                    DisplayName = "Employee Surname",
                    PropertyTitle = nameof(Promotion.Employee.Surname)
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Employee Firstname",
                    PropertyTitle = nameof(Promotion.Employee.Firstname),
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Old Position Title",
                    PropertyTitle = nameof(Promotion.OldPosition),
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "New Position",
                    PropertyTitle = nameof(Promotion.NewPosition),
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Old Brutto Price",
                    PropertyTitle = nameof(Promotion.OldSalary),
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "New Brutto",
                    PropertyTitle = nameof(Promotion.NewSalary),
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Promotion Id",
                    PropertyTitle = nameof(Promotion.Id),
                },
            };
        }
    }
}
