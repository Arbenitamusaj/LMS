using EcommerceApplication.Interfaces;
using EcommerceDomain.LeaveAllovations;
using Hangfire;
using LMS.Application.Interfaces;
using LMS.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceApplication.BackgroundJobs
{
    public class LeaveBackgroundService : ILeaveBackgroundService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LeaveBackgroundService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddDaysToAnnualLeaveMonthly()
        {
            RecurringJob.AddOrUpdate(() => AddDaysToAnnualLeaveJob(), Cron.Monthly);
        }

        public void ResetAnnualLeaveOnJulyFirst()
        {
            RecurringJob.AddOrUpdate(() => ResetAnnualLeaveJob(), Cron.Yearly(7, 1));
        }

        public async Task AddDaysToAnnualLeaveJob()
        {
            var employees = await _unitOfWork.Repository<Employee>().GetAll().ToListAsync();

            foreach (var employee in employees)
            {
                var allocation = await _unitOfWork.Repository<LeaveAllocation>()
                    .GetByCondition(a => a.EmployeeId == employee.Id && a.LeaveType.Name == "Annual" && a.Period == DateTime.Now.Year)
                    .FirstOrDefaultAsync();

                if (allocation != null)
                {
                    allocation.NumberOfDays += 2;
                    _unitOfWork.Repository<LeaveAllocation>().Update(allocation);
                }
                else
                {
                    var newAllocation = new LeaveAllocation
                    {
                        EmployeeId = employee.Id,
                        LeaveTypeId = 1,
                        NumberOfDays = 2,
                        Period = DateTime.Now.Year
                    };
                    _unitOfWork.Repository<LeaveAllocation>().Create(newAllocation);
                }
            }

            await _unitOfWork.CompleteAsync();
        }

        public async Task ResetAnnualLeaveJob()
        {
            var employees = await _unitOfWork.Repository<Employee>().GetAll().ToListAsync();

            foreach (var employee in employees)
            {
                var allocations = await _unitOfWork.Repository<LeaveAllocation>()
                    .GetByCondition(a => a.EmployeeId == employee.Id && a.LeaveType.Name == "Annual")
                    .ToListAsync();

                foreach (var allocation in allocations)
                {
                    if (allocation.Period < DateTime.Now.Year)
                    {
                        allocation.NumberOfDays = 0;
                        _unitOfWork.Repository<LeaveAllocation>().Update(allocation);
                    }
                }
            }

            await _unitOfWork.CompleteAsync();
        }
    }
}
