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
    public class TrainingService : BaseServiceWithEmployeeSearch<Training, TrainingDTO>
    {
        #region Filtration Properties
        public string? TitleName { get; set; }
        public string? DescriptionName { get; set; }
        public DateOnly? DateFrom { get; set; }
        public DateOnly? DateTo { get; set; }
        #endregion

        public override void AddModel(Training model)
        {
            try
            {
                Database.Trainings.Add(model);
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override void DeleteModel(TrainingDTO model)
        {
            try
            {
                Training training = Database.Trainings.First(item => item.Id == model.Id);
                training.IsActive = false;
                training.DeleteDateTime = DateTime.Now;
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override List<EmployeeDTO> GetEmployeeModelsOfProperty(Training model)
        {
            return Database.EmployeeTrainings
                .Include(item => item.Employee)
                .Include(item => item.Employee.PersonalData)
                .Include(item => item.Employee.Salary)
                .Include(item => item.Employee.Position)
                .Where(item => item.IsActive && item.Training == model)
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

        public override Training GetModel(int id) => Database.Trainings.First(item => item.Id == id);

        public override List<TrainingDTO> GetModels()
        {
            IQueryable<Training> trainings = Database.Trainings.Where(item => item.IsActive);

            if (!string.IsNullOrEmpty(TitleName))
            {
                trainings = trainings.Where(item => EF.Functions.Like(item.Title, $"%{TitleName}%"));
            }
            if (!string.IsNullOrEmpty(DescriptionName))
            {
                trainings = trainings.Where(item => EF.Functions.Like(item.Description, $"%{DescriptionName}%"));
            }
            if (DateFrom != null)
            {
                trainings = trainings.Where(item => item.BeginDate >= DateFrom || item.EndDate >= DateFrom);
            }
            if (DateTo != null)
            {
                trainings = trainings.Where(item => item.BeginDate <= DateTo && item.EndDate <= DateTo);
            }

            if (!string.IsNullOrEmpty(OrderProperty))
            {
                switch (OrderProperty)
                {
                    case nameof(Training.Title):
                        trainings = IsOrderDescending ? trainings.OrderByDescending(item => item.Title) : trainings.OrderBy(item => item.Title);
                        break;
                    case nameof(Training.Description):
                        trainings = IsOrderDescending ? trainings.OrderByDescending(item => item.Description) : trainings.OrderBy(item => item.Description);
                        break;
                    case nameof(Training.BeginDate):
                        trainings = IsOrderDescending ? trainings.OrderByDescending(item => item.BeginDate) : trainings.OrderBy(item => item.BeginDate);
                        break;
                    case nameof(Training.EndDate):
                        trainings = IsOrderDescending ? trainings.OrderByDescending(item => item.EndDate) : trainings.OrderBy(item => item.EndDate);
                        break;
                    case nameof(Training.Id):
                        trainings = IsOrderDescending ? trainings.OrderByDescending(item => item.Id) : trainings.OrderBy(item => item.Id);
                        break;
                }
            }

            return trainings.Select(item => new TrainingDTO()
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                BeginDate = item.BeginDate,
                EndDate = item.EndDate,
            }).ToList();
        }

        public override void UpdateModel(Training model)
        {
            try
            {
                model.EditDateTime = DateTime.Now;
                Database.Trainings.Update(model);
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override Training CreateModel()
        {
            return new Training()
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
                    PropertyTitle = nameof(Training.Title)
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Description",
                    PropertyTitle = nameof(Training.Description),
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Begin Date",
                    PropertyTitle = nameof(Training.BeginDate),
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "End Date",
                    PropertyTitle = nameof(Training.EndDate),
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Training Id",
                    PropertyTitle = nameof(Training.Id),
                },
            };
        }
    }
}
