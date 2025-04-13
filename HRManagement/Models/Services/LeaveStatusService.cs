using HRManagement.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.Services
{
    public class LeaveStatusService : BaseService<LeaveStatus, LeaveStatusDTO>
    {
        #region Filtration Properties
        public string? TitleName { get; set; }
        #endregion

        public override void AddModel(LeaveStatus model)
        {
            try
            {
                Database.LeaveStatuses.Add(model);
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override void DeleteModel(LeaveStatusDTO model)
        {
            try
            {
                LeaveStatus leaveStatus = Database.LeaveStatuses.First(item => item.Id == model.Id);
                leaveStatus.IsActive = false;
                leaveStatus.DeleteDateTime = DateTime.Now;
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override LeaveStatus GetModel(int id) => Database.LeaveStatuses.First(item => item.Id == id);

        public LeaveStatusDTO GetDtoModel(int id)
        {
            return Database.LeaveStatuses.Where(item => item.Id == id).Select(item => new LeaveStatusDTO()
            {
                Id = item.Id,
                Title = item.Title,
            }).First();
        }

        public override List<LeaveStatusDTO> GetModels()
        {
            IQueryable<LeaveStatus> statuses = Database.LeaveStatuses.Where(item => item.IsActive);

            if (!string.IsNullOrEmpty(TitleName))
            {
                statuses = statuses.Where(item => EF.Functions.Like(item.Title, $"%{TitleName}%"));
            }

            if (!string.IsNullOrEmpty(OrderProperty))
            {
                switch (OrderProperty)
                {
                    case nameof(LeaveStatus.Title):
                        statuses = IsOrderDescending ? statuses.OrderByDescending(item => item.Title) : statuses.OrderBy(item => item.Title);
                        break;
                    case nameof(LeaveStatus.Id):
                        statuses = IsOrderDescending ? statuses.OrderByDescending(item => item.Id) : statuses.OrderBy(item => item.Id);
                        break;
                }
            }

            return statuses.Select(item => new LeaveStatusDTO()
            {
                Id = item.Id,
                Title = item.Title,
            }).ToList();
        }
        public List<string> GetModelsTitles()
        {
            return Database.LeaveStatuses.Where(item => item.IsActive).Select(item => item.Title).ToList();
        }

        public override LeaveStatus CreateModel()
        {
            return new LeaveStatus()
            {
                IsActive = true,
                CreationDateTime = DateTime.Now,
            };
        }

        public override void UpdateModel(LeaveStatus model)
        {
            try
            {
                model.EditDateTime = DateTime.Now;
                Database.LeaveStatuses.Update(model);
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
                    PropertyTitle = nameof(LeaveStatus.Title)
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "LeaveStatus Id",
                    PropertyTitle = nameof(LeaveStatus.Id),
                },
            };
        }
    }
}
