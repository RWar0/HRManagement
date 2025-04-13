using HRManagement.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.Services
{
    public class LeaveTypeService : BaseService<LeaveType, LeaveTypeDTO>
    {
        #region Filtration Properties
        public string? TitleName { get; set; }
        public bool HasDescription { get; set; }
        public string? DescriptionName { get; set; }
        #endregion

        public override void AddModel(LeaveType model)
        {
            try
            {
                Database.LeaveTypes.Add(model);
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override void DeleteModel(LeaveTypeDTO model)
        {
            try
            {
                LeaveType leaveType = Database.LeaveTypes.First(item => item.Id == model.Id);
                leaveType.IsActive = false;
                leaveType.DeleteDateTime = DateTime.Now;
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override LeaveType GetModel(int id) => Database.LeaveTypes.First(item => item.Id == id);

        public LeaveTypeDTO GetDtoModel(int id)
        {
            return Database.LeaveTypes.Where(item => item.Id == id).Select(item => new LeaveTypeDTO()
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description
            }).First();
        }

        public override List<LeaveTypeDTO> GetModels()
        {
            IQueryable<LeaveType> types = Database.LeaveTypes.Where(item => item.IsActive);

            if (!string.IsNullOrEmpty(TitleName))
            {
                types = types.Where(item => EF.Functions.Like(item.Title, $"%{TitleName}%"));
            }
            if (HasDescription)
            {
                types = types.Where(item => !string.IsNullOrEmpty(item.Description));
            }
            if (!string.IsNullOrEmpty(DescriptionName))
            {
                types = types.Where(item => EF.Functions.Like(item.Description, $"%{DescriptionName}%"));
            }

            if (!string.IsNullOrEmpty(OrderProperty))
            {
                switch (OrderProperty)
                {
                    case nameof(LeaveType.Title):
                        types = IsOrderDescending ? types.OrderByDescending(item => item.Title) : types.OrderBy(item => item.Title);
                        break;
                    case nameof(LeaveType.Description):
                        types = IsOrderDescending ? types.OrderByDescending(item => item.Description) : types.OrderBy(item => item.Description);
                        break;
                    case nameof(LeaveType.Id):
                        types = IsOrderDescending ? types.OrderByDescending(item => item.Id) : types.OrderBy(item => item.Id);
                        break;
                }
            }

            return types.Select(item => new LeaveTypeDTO()
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description
            }).ToList();
        }
        public List<string> GetModelsTitles()
        {
            return Database.LeaveTypes.Where(item => item.IsActive).Select(item => item.Title).ToList();
        }

        public override LeaveType CreateModel()
        {
            return new LeaveType()
            {
                IsActive = true,
                CreationDateTime = DateTime.Now,
            };
        }

        public override void UpdateModel(LeaveType model)
        {
            try
            {
                model.EditDateTime = DateTime.Now;
                Database.LeaveTypes.Update(model);
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override List<SearchComboBoxDTO> GetOrderByComboBoxDtos()
        {
            return new List<SearchComboBoxDTO>
            {
                new SearchComboBoxDTO
                {
                    DisplayName = "Title",
                    PropertyTitle = nameof(LeaveType.Title)
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Description",
                    PropertyTitle = nameof(LeaveType.Description)
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "LeaveType Id",
                    PropertyTitle = nameof(LeaveType.Id),
                },
            };
        }
    }
}
