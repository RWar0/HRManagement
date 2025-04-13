using HRManagement.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.Services
{
    public class PositionService : BaseService<Position, PositionDTO>
    {
        #region Filtration Properties
        public string? TitleName { get; set; }
        public bool HasDepartment { get; set; }
        public string? DepartmentName { get; set; }
        #endregion

        public override void AddModel(Position model)
        {
            try
            {
                if(string.IsNullOrEmpty(model.DepartmentName)) 
                {
                    model.DepartmentName = null;
                }
                Database.Positions.Add(model);
                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...\n{ex}", "Error");
            }
        }

        public override void DeleteModel(PositionDTO model)
        {
            try
            {
                Position position = Database.Positions.First(item => item.Id == model.Id);
                position.IsActive = false;
                position.DeleteDateTime = DateTime.Now;
                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...\n{ex}", "Error");
            }
        }

        public override Position GetModel(int id) => Database.Positions.First(item => item.Id == id);

        public PositionDTO GetDtoModel(int id)
        {
            return Database.Positions.Where(item => item.Id == id).Select(item => new PositionDTO()
            {
                Id = item.Id,
                Title = item.Title,
                DepartmentName = item.DepartmentName
            }).First();
        }

        public override List<PositionDTO> GetModels()
        {
            IQueryable<Position> positions = Database.Positions.Where(item => item.IsActive);

            if (!string.IsNullOrEmpty(TitleName))
            {
                positions = positions.Where(item => EF.Functions.Like(item.Title, $"%{TitleName}%"));
            }
            if (HasDepartment)
            {
                positions = positions.Where(item => !string.IsNullOrEmpty(item.DepartmentName));
            }
            if (!string.IsNullOrEmpty(DepartmentName))
            {
                positions = positions.Where(item => EF.Functions.Like(item.DepartmentName, $"%{DepartmentName}%"));
            }

            if(!string.IsNullOrEmpty(OrderProperty))
            {
                switch (OrderProperty)
                {
                    case nameof(Position.Title):
                        positions = IsOrderDescending ? positions.OrderByDescending(item => item.Title) : positions.OrderBy(item => item.Title);
                        break;
                    case nameof(Position.DepartmentName):
                        positions = IsOrderDescending ? positions.OrderByDescending(item => item.DepartmentName) : positions.OrderBy(item => item.DepartmentName);
                        break;
                    case nameof(Position.Id):
                        positions = IsOrderDescending ? positions.OrderByDescending(item => item.Id) : positions.OrderBy(item => item.Id);
                        break;
                }
            }

            return positions.Select(item => new PositionDTO()
            {
                Id = item.Id,
                Title = item.Title,
                DepartmentName = item.DepartmentName
            }).ToList();
        }

        public override void UpdateModel(Position model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.DepartmentName))
                {
                    model.DepartmentName = null;
                }
                model.EditDateTime = DateTime.Now;
                Database.Positions.Update(model);
                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...\n{ex}", "Error");
            }
        }

        public override Position CreateModel()
        {
            return new Position()
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
                    DisplayName = "Title",
                    PropertyTitle = nameof(Position.Title)
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Department",
                    PropertyTitle = nameof(Position.DepartmentName),
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Position Id",
                    PropertyTitle = nameof(Position.Id),
                },
            };
        }
    }
}
