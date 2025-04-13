using HRManagement.Helpers.Enums;
using HRManagement.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.Services
{
    public class AdressService : BaseService<Adress, AdressDTO>
    {
        #region FiltrationProperties
        public string? CountryName { get; set; }
        public string? CityName { get; set; }
        public string? StreetName { get; set; }

        public YesNoFilterEnum HasStreetFilter { get; set; }
        public YesNoFilterEnum HasPostalCodeFilter { get; set; }
        public YesNoFilterEnum HasFlatNumberFilter { get; set; }
        #endregion

        public override void AddModel(Adress model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.PostalCode))
                {
                    model.PostalCode = null;
                }
                if (string.IsNullOrEmpty(model.Street))
                {
                    model.Street = null;
                }
                if (string.IsNullOrEmpty(model.FlatNumber))
                {
                    model.FlatNumber = null;
                }

                Database.Adresses.Add(model);
                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program... {ex}", "Error");
            }
        }

        public override void DeleteModel(AdressDTO model)
        {
            try
            {
                Adress address = Database.Adresses.First(item => item.Id == model.Id);
                address.IsActive = false;
                address.DeleteDateTime = DateTime.Now;
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override Adress GetModel(int id) => Database.Adresses.First(item => item.Id == id);

        public AdressDTO GetModelDTO(int id)
        {
            return Database.Adresses.Where(item => item.Id == id).Select(item => new AdressDTO()
            {
                Id = item.Id,
                Country = item.Country,
                City = item.City,
                PostalCode = item.PostalCode,
                Street = item.Street,
                HouseNumber = item.HouseNumber,
                FlatNumber = item.FlatNumber,
            }).First();
        }

        public override List<AdressDTO> GetModels()
        {
            IQueryable<Adress> adresses = Database.Adresses.Where(item => item.IsActive);

            switch(HasPostalCodeFilter)
            {
                case YesNoFilterEnum.Yes:
                    adresses = adresses.Where(item => !string.IsNullOrEmpty(item.PostalCode));
                    break;
                case YesNoFilterEnum.No:
                    adresses = adresses.Where(item => string.IsNullOrEmpty(item.PostalCode));
                    break;
            }
            switch (HasStreetFilter)
            {
                case YesNoFilterEnum.Yes:
                    adresses = adresses.Where(item => !string.IsNullOrEmpty(item.Street));
                    break;
                case YesNoFilterEnum.No:
                    adresses = adresses.Where(item => string.IsNullOrEmpty(item.Street));
                    break;
            }
            switch (HasFlatNumberFilter)
            {
                case YesNoFilterEnum.Yes:
                    adresses = adresses.Where(item => !string.IsNullOrEmpty(item.FlatNumber));
                    break;
                case YesNoFilterEnum.No:
                    adresses = adresses.Where(item => string.IsNullOrEmpty(item.FlatNumber));
                    break;
            }

            if (!string.IsNullOrEmpty(CountryName))
            {
                adresses = adresses.Where(item => EF.Functions.Like(item.Country, $"%{CountryName}%"));
            }
            if (!string.IsNullOrEmpty(CityName))
            {
                adresses = adresses.Where(item => EF.Functions.Like(item.City, $"%{CityName}%"));
            }
            if (!string.IsNullOrEmpty(StreetName))
            {
                adresses = adresses.Where(item => EF.Functions.Like(item.Street, $"%{StreetName}%"));
            }

            if(!string.IsNullOrEmpty(OrderProperty))
            {
                switch (OrderProperty)
                {
                    case nameof(Adress.Country):
                        adresses = IsOrderDescending ? adresses.OrderByDescending(item => item.Country) : adresses.OrderBy(item => item.Country);
                        break;
                    case nameof(Adress.City):
                        adresses = IsOrderDescending ? adresses.OrderByDescending(item => item.City) : adresses.OrderBy(item => item.City);
                        break;
                    case nameof(Adress.Street):
                        adresses = IsOrderDescending ? adresses.OrderByDescending(item => item.Street) : adresses.OrderBy(item => item.Street);
                        break;
                    case nameof(Adress.HouseNumber):
                        adresses = IsOrderDescending ? adresses.OrderByDescending(item => item.HouseNumber) : adresses.OrderBy(item => item.HouseNumber);
                        break;
                    case nameof(Adress.Id):
                        adresses = IsOrderDescending ? adresses.OrderByDescending(item => item.Id) : adresses.OrderBy(item => item.Id);
                        break;
                }
            }

            return adresses.Select(item => new AdressDTO()
            {
                Id = item.Id,
                Country = item.Country,
                City = item.City,
                PostalCode = item.PostalCode,
                Street = item.Street,
                HouseNumber = item.HouseNumber,
                FlatNumber = item.FlatNumber,
            }).ToList();
        }

        public override void UpdateModel(Adress model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.PostalCode))
                {
                    model.PostalCode = null;
                }
                if (string.IsNullOrEmpty(model.Street))
                {
                    model.Street = null;
                }
                if (string.IsNullOrEmpty(model.FlatNumber))
                {
                    model.FlatNumber = null;
                }

                model.EditDateTime = DateTime.Now;
                Database.Adresses.Update(model);
                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program... {ex}", "Error");
            }
        }
        public override Adress CreateModel()
        {
            return new Adress()
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
                    DisplayName = "Country",
                    PropertyTitle = nameof(Adress.Country)
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "City",
                    PropertyTitle = nameof(Adress.City),
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Street",
                    PropertyTitle = nameof(Adress.Street),
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "House Number",
                    PropertyTitle = nameof(Adress.HouseNumber),
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Adress Id",
                    PropertyTitle = nameof(Adress.Id),
                },
            };
        }
    }
}
