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
    public class LeaveService : BaseService<Leave, LeaveDTO>
    {
        #region Filtration Properties
        public string? EmployeeName { get; set; }
        public string? EmployeeSurname { get; set; }
        public string? ReasonFilter { get; set; }
        public string? LeaveTypeFilter { get; set; }
        public string? LeaveStatusFilter { get; set; }
        public DateOnly? DateFrom { get; set; }
        public DateOnly? DateTo { get; set; }
        #endregion

        public override void AddModel(Leave model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Reason))
                {
                    model.Reason = null;
                }
                Database.Leaves.Add(model);
                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program... {ex}", "Error");
            }
        }

        public override void DeleteModel(LeaveDTO model)
        {
            try
            {
                Leave leave = Database.Leaves.First(item => item.Id == model.Id);
                leave.IsActive = false;
                leave.DeleteDateTime = DateTime.Now;
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override Leave GetModel(int id) => Database.Leaves.First(item => item.Id == id);

        public override List<LeaveDTO> GetModels()
        {
            IQueryable<Leave> leaves = Database.Leaves.Include(item => item.Employee).Include(item => item.LeaveType).Include(item => item.LeaveStatus).Where(item => item.IsActive);

            if (!string.IsNullOrEmpty(EmployeeName))
            {
                leaves = leaves.Where(item => EF.Functions.Like(item.Employee.Firstname, $"%{EmployeeName}%"));
            }
            if (!string.IsNullOrEmpty(EmployeeSurname))
            {
                leaves = leaves.Where(item => EF.Functions.Like(item.Employee.Surname, $"%{EmployeeSurname}%"));
            }
            if (!string.IsNullOrEmpty(ReasonFilter))
            {
                leaves = leaves.Where(item => EF.Functions.Like(item.Reason, $"%{ReasonFilter}%"));
            }
            if (!string.IsNullOrEmpty(LeaveStatusFilter) && !LeaveStatusFilter.Equals("No filter"))
            {
                leaves = leaves.Where(item => item.LeaveStatus.Title.Equals(LeaveStatusFilter));
            }
            if (!string.IsNullOrEmpty(LeaveTypeFilter) && !LeaveTypeFilter.Equals("No filter"))
            {
                leaves = leaves.Where(item => item.LeaveStatus.Title.Equals(LeaveTypeFilter));
            }
            if (DateFrom != null)
            {
                leaves = leaves.Where(item => item.BeginDate >= DateFrom || item.EndDate >= DateFrom);
            }
            if (DateTo != null)
            {
                leaves = leaves.Where(item => item.BeginDate <= DateTo && item.EndDate <= DateTo);
            }

            if (!string.IsNullOrEmpty(OrderProperty))
            {
                switch (OrderProperty)
                {
                    case nameof(Leave.Employee.Surname):
                        leaves = IsOrderDescending ? leaves.OrderByDescending(item => item.Employee.Surname) : leaves.OrderBy(item => item.Employee.Surname);
                        break;
                    case nameof(Leave.Employee.Firstname):
                        leaves = IsOrderDescending ? leaves.OrderByDescending(item => item.Employee.Firstname) : leaves.OrderBy(item => item.Employee.Firstname);
                        break;
                    case nameof(Leave.LeaveType):
                        leaves = IsOrderDescending ? leaves.OrderByDescending(item => item.LeaveType.Title) : leaves.OrderBy(item => item.LeaveType.Title);
                        break;
                    case nameof(Leave.LeaveStatus):
                        leaves = IsOrderDescending ? leaves.OrderByDescending(item => item.LeaveStatus.Title) : leaves.OrderBy(item => item.LeaveStatus.Title);
                        break;
                    case nameof(Leave.Reason):
                        leaves = IsOrderDescending ? leaves.OrderByDescending(item => item.Reason) : leaves.OrderBy(item => item.Reason);
                        break;
                    case nameof(Leave.BeginDate):
                        leaves = IsOrderDescending ? leaves.OrderByDescending(item => item.BeginDate) : leaves.OrderBy(item => item.BeginDate);
                        break;
                    case nameof(Leave.EndDate):
                        leaves = IsOrderDescending ? leaves.OrderByDescending(item => item.EndDate) : leaves.OrderBy(item => item.EndDate);
                        break;
                    case nameof(Leave.Id):
                        leaves = IsOrderDescending ? leaves.OrderByDescending(item => item.Id) : leaves.OrderBy(item => item.Id);
                        break;
                }
            }

            return leaves.Select(item => new LeaveDTO()
            {
                Id = item.Id,
                EmployeeName = $"{item.Employee.Firstname} {item.Employee.Surname}",
                LeaveType = item.LeaveType.Title,
                Reason = item.Reason,
                BeginDate = item.BeginDate,
                EndDate = item.EndDate,
                LeaveStatus = item.LeaveStatus.Title,
            }).ToList();
        }

        public List<Leave> GetModelsOfEmployeesInDesiredYear(int employeeId, int year)
        {
            IQueryable<Leave> leaves = Database.Leaves.Where(item => item.IsActive);

            // Search for desired employee Leaves
            leaves = leaves.Where(item => item.EmployeeId == employeeId);

            // Search for leaves of employee in desired year
            leaves = leaves.Where(item => item.BeginDate.Year == year && item.EndDate.Year == year);

            return leaves.ToList();
        }

        public override void UpdateModel(Leave model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Reason))
                {
                    model.Reason = null;
                }
                model.EditDateTime = DateTime.Now;
                Database.Leaves.Update(model);
                Database.SaveChanges();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"Oops... Something went wrong...\nContant the administrator of the program...", "Error");
            }
        }

        public override Leave CreateModel()
        {
            return new Leave()
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
                    DisplayName = "Employee Surname",
                    PropertyTitle = nameof(Leave.Employee.Surname)
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Employee Firstname",
                    PropertyTitle = nameof(Leave.Employee.Firstname)
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Leave Type",
                    PropertyTitle = nameof(Leave.LeaveType)
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Reason",
                    PropertyTitle = nameof(Leave.Reason)
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Begin Date",
                    PropertyTitle = nameof(Leave.BeginDate)
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "End Date",
                    PropertyTitle = nameof(Leave.EndDate)
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Leave Status",
                    PropertyTitle = nameof(Leave.LeaveStatus),
                },
                new SearchComboBoxDTO
                {
                    DisplayName = "Leave Id",
                    PropertyTitle = nameof(Leave.Id),
                },
            };
        }

        public int GetQuantityOfAvailableLeaveDays(Employee employee)
        {
            return new AvailableLeavesBL(employee).GetAvailableDays;
        }

        public int GetQuantityOfAvailableLeaveDaysAfterLeft(Employee employee, DateOnly beginDate, DateOnly endDate)
        {
            return new AvailableLeavesBL(employee).GetCalculatedAvailableDaysAfterLeft(beginDate, endDate);
        }
        public int GetQuantityOfAvailableLeaveDaysAfterLeftWhenEditing(Employee employee, int leaveId, DateOnly beginDate, DateOnly endDate)
        {
            return new AvailableLeavesBL(employee).GetEditingCalculatedAvailableDaysAfterLeft(leaveId, beginDate, endDate);
        }
    }
}
