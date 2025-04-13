using HRManagement.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.Services
{
    public class PersonalDataService : BaseService<PersonalData, PersonalDataDTO>
    {
        #region Filtration Properties
        public string? EmployeeName { get; set; }
        public string? EmployeeSurname { get; set; }
        public string? PeselFilter { get; set; }
        public string? BirthPlaceFilter { get; set; }
        public string? EducationFilter { get; set; }
        public int? ChildrensFrom { get; set; }
        public int? ChildrensTo { get; set; }
        public int? DateFrom { get; set; }
        public int? DateTo { get; set; }
        #endregion

        #region Additional Services
        private AdressService AdressService { get; set; }
        #endregion

        public PersonalDataService()
        {
            AdressService = new AdressService();
        }

        public override void AddModel(PersonalData model)
        {
            try
            {
                Database.PersonalData.Add(model);
                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...\n{ex}", "Error");
            }
        }

        public override void DeleteModel(PersonalDataDTO model)
        {
            try
            {
                PersonalData personalData = Database.PersonalData.First(item => item.Id == model.Id);
                personalData.IsActive = false;
                personalData.DeleteDateTime = DateTime.Now;
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override PersonalData GetModel(int id) => Database.PersonalData.First(item => item.Id == id);

        public PersonalData GetModel(Employee employee)
        {
            return Database.PersonalData.Where(item => item.IsActive && item.Employee == employee).First();
        }

        public override List<PersonalDataDTO> GetModels()
        {
            IQueryable<PersonalData> personalDatas = Database.PersonalData.Include(item => item.Employee).Include(item => item.RegistrationAdress).Include(item => item.ResidenceAdress).Where(item => item.IsActive);


            if (!string.IsNullOrEmpty(EmployeeName))
            {
                personalDatas = personalDatas.Where(item => EF.Functions.Like(item.Employee.Firstname, $"%{EmployeeName}%"));
            }
            if (!string.IsNullOrEmpty(EmployeeSurname))
            {
                personalDatas = personalDatas.Where(item => EF.Functions.Like(item.Employee.Surname, $"%{EmployeeSurname}%"));
            }
            if (!string.IsNullOrEmpty(PeselFilter))
            {
                personalDatas = personalDatas.Where(item => EF.Functions.Like(item.Pesel, $"%{PeselFilter}%"));
            }
            if (!string.IsNullOrEmpty(BirthPlaceFilter))
            {
                personalDatas = personalDatas.Where(item => EF.Functions.Like(item.PlaceOfBirth, $"%{BirthPlaceFilter}%"));
            }
            if (!string.IsNullOrEmpty(EducationFilter))
            {
                personalDatas = personalDatas.Where(item => EF.Functions.Like(item.Education, $"%{EducationFilter}%"));
            }
            if (ChildrensFrom != null)
            {
                personalDatas = personalDatas.Where(item => item.ChildrenQuantity >= ChildrensFrom);
            }
            if (ChildrensTo != null)
            {
                personalDatas = personalDatas.Where(item => item.ChildrenQuantity <= ChildrensTo);
            }
            if (DateFrom != null)
            {
                personalDatas = personalDatas.Where(item => item.DateOfBirth.Year >= DateFrom);
            }
            if (DateTo != null)
            {
                personalDatas = personalDatas.Where(item => item.DateOfBirth.Year <= DateTo);
            }

            if (!string.IsNullOrEmpty(OrderProperty))
            {
                switch (OrderProperty)
                {
                    case nameof(PersonalData.Employee.Surname):
                        personalDatas = IsOrderDescending ? personalDatas.OrderByDescending(item => item.Employee.Surname) : personalDatas.OrderBy(item => item.Employee.Surname);
                        break;
                    case nameof(PersonalData.Employee.Firstname):
                        personalDatas = IsOrderDescending ? personalDatas.OrderByDescending(item => item.Employee.Firstname) : personalDatas.OrderBy(item => item.Employee.Firstname);
                        break;
                    case nameof(PersonalData.Pesel):
                        personalDatas = IsOrderDescending ? personalDatas.OrderByDescending(item => item.Pesel) : personalDatas.OrderBy(item => item.Pesel);
                        break;
                    case nameof(PersonalData.PhoneNumber):
                        personalDatas = IsOrderDescending ? personalDatas.OrderByDescending(item => item.PhoneNumber) : personalDatas.OrderBy(item => item.PhoneNumber);
                        break;
                    case nameof(PersonalData.DateOfBirth):
                        personalDatas = IsOrderDescending ? personalDatas.OrderByDescending(item => item.DateOfBirth.Year) : personalDatas.OrderBy(item => item.DateOfBirth.Year);
                        break;
                    case nameof(PersonalData.PlaceOfBirth):
                        personalDatas = IsOrderDescending ? personalDatas.OrderByDescending(item => item.PlaceOfBirth) : personalDatas.OrderBy(item => item.PlaceOfBirth);
                        break;
                    case nameof(PersonalData.ChildrenQuantity):
                        personalDatas = IsOrderDescending ? personalDatas.OrderByDescending(item => item.ChildrenQuantity) : personalDatas.OrderBy(item => item.ChildrenQuantity);
                        break;
                    case nameof(PersonalData.Education):
                        personalDatas = IsOrderDescending ? personalDatas.OrderByDescending(item => item.Education) : personalDatas.OrderBy(item => item.Education);
                        break;
                    case nameof(PersonalData.Id):
                        personalDatas = IsOrderDescending ? personalDatas.OrderByDescending(item => item.Id) : personalDatas.OrderBy(item => item.Id);
                        break;
                }
            }

            return personalDatas.Select(item => new PersonalDataDTO()
            {
                Id = item.Id,
                EmployeeName = $"{item.Employee.Firstname} {item.Employee.Surname}",
                Pesel = item.Pesel,
                PhoneNumber = item.PhoneNumber,
                DateOfBirth = item.DateOfBirth,
                PlaceOfBirth = item.PlaceOfBirth,
                ChildrenQuantity = item.ChildrenQuantity,
                Education = item.Education,
                RegistrationAdress = AdressService.GetModelDTO(item.RegistrationAdressId),
                ResidenceAdress = AdressService.GetModelDTO(item.ResidenceAdressId),
            }).ToList();
        }

        public override void UpdateModel(PersonalData model)
        {
            try
            {
                model.EditDateTime = DateTime.Now;
                Database.PersonalData.Update(model);
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override PersonalData CreateModel()
        {
            return new PersonalData()
            {
                DateOfBirth = DateOnly.FromDateTime(DateTime.Now),
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
                    PropertyTitle = nameof(PersonalData.Employee.Surname)
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Employee Firstname",
                    PropertyTitle = nameof(PersonalData.Employee.Firstname)
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "PESEL",
                    PropertyTitle = nameof(PersonalData.Pesel)
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Phone Number",
                    PropertyTitle = nameof(PersonalData.PhoneNumber)
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Year of birth",
                    PropertyTitle = nameof(PersonalData.DateOfBirth)
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Place of birth",
                    PropertyTitle = nameof(PersonalData.PlaceOfBirth)
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Childrens",
                    PropertyTitle = nameof(PersonalData.ChildrenQuantity)
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Education",
                    PropertyTitle = nameof(PersonalData.Education),
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "PersonalData Id",
                    PropertyTitle = nameof(PersonalData.Id),
                },
            };
        }
    }
}
