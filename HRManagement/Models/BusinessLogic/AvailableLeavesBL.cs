using HRManagement.Models.DTOs;
using HRManagement.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.BusinessLogic
{
    public class AvailableLeavesBL
    {
        // Services
        private EmployeeCareerService EmployeeCareerService { get; set; }
        private LeaveService LeaveService { get; set; }

        // Fields And Properties
        private Employee Employee { get; set; } = default!;
        public int GetAvailableDays => GetCalculatedEligibleDays() - GetUsedDaysInThisYear() + GetUnusedDaysFromPeviousYear();
        public AvailableLeavesBL(Employee employee)
        {
            EmployeeCareerService = new EmployeeCareerService();
            LeaveService = new LeaveService();
            Employee = employee;
        }

        // Education levels and the appropriate amount of the experience years
        private readonly Dictionary<string, int> educationLevels = new()
        {
            { "University", 8 },
            { "Academy", 8 },
            { "Post-Secondary School", 6 },
            { "High School", 4 },
            { "Vocational School", 3 }
        };

        // Methods
        private int GetWorkExperience()
        {
            int experience = 0;
            bool isEducationFound = false;

            List<Career> careerList = EmployeeCareerService.GetCareersOfEmployee(Employee.Id);

            foreach (Career item in careerList)
            {
                string title = item.Title;

                // Searching for experience years due after chool
                if (!isEducationFound)
                {
                    foreach (KeyValuePair<string, int> level in educationLevels)
                    {
                        if (title.Contains(level.Key, StringComparison.OrdinalIgnoreCase))
                        {
                            experience += level.Value;
                            isEducationFound = true;
                            break;
                        }
                    }
                }

                // Checking for work experience
                if (!title.Contains("University", StringComparison.OrdinalIgnoreCase) && !title.Contains("Post-Secondary School", StringComparison.OrdinalIgnoreCase)
                    && !title.Contains("High School", StringComparison.OrdinalIgnoreCase) && !title.Contains("Vocational School", StringComparison.OrdinalIgnoreCase))
                {
                    int years = item.EndDate.Year - item.BeginDate.Year;
                    if (years > 0)
                    {
                        experience += years;
                    }
                }
            }

            // Adding experience in this company
            int employeeCurrentEmploymentYears = DateTime.Now.Year - Employee.CreationDateTime.Year;
            if (employeeCurrentEmploymentYears > 0)
            {
                experience += employeeCurrentEmploymentYears;
            }

            return experience;
        }
        private int GetCalculatedEligibleDays()
        {
            return GetWorkExperience() >= 10 ? 26 : 20;
        }
        private int GetUsedDaysInThisYear()
        {
            List<Leave> leaves = LeaveService.GetModelsOfEmployeesInDesiredYear(Employee.Id, DateTime.Now.Year);

            return GetWorkingDaysLeaves(leaves);
        }
        private int GetUnusedDaysFromPeviousYear()
        {
            List<Leave> leaves = LeaveService.GetModelsOfEmployeesInDesiredYear(Employee.Id, DateTime.Now.Year - 1);

            return GetCalculatedEligibleDays() - GetWorkingDaysLeaves(leaves);
        }
        private int GetWorkingDaysLeaves(List<Leave> leavesList)
        {
            int usedDays = 0;

            foreach (Leave item in leavesList)
            {
                usedDays += GetWorkingDays(item.BeginDate, item.EndDate);
            }

            return usedDays;
        }
        private int GetWorkingDays(DateOnly begin, DateOnly end)
        {
            int workingDays = 0;
            for (DateOnly date = begin; date <= end; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    workingDays++;
                }
            }
            return workingDays;
        }
        public int GetCalculatedAvailableDaysAfterLeft(DateOnly beginDate, DateOnly endDate)
        {
            return GetAvailableDays - GetWorkingDays(beginDate, endDate);
        }
        public int GetEditingCalculatedAvailableDaysAfterLeft(int leaveId, DateOnly beginDate, DateOnly endDate)
        {
            Leave currentLeave = LeaveService.GetModel(leaveId);
            return GetAvailableDays + GetWorkingDays(currentLeave.BeginDate, currentLeave.EndDate) - GetWorkingDays(beginDate, endDate);
        }
    }
}
