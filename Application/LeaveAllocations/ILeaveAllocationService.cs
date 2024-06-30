using EcommerceDomain.LeaveAllovations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApplication.LeaveAllocations
{
    public interface ILeaveAllocationService
    {
        Task<List<LeaveAllocation>> GetAllLeaveAllocationsAsync();
        Task AllocateLeaveDaysAsync(string employeeId, int leaveTypeId, int numberOfDays, int period);
        Task<bool> AdjustLeaveAllocationAsync(int leaveAllocationId, int newNumberOfDays);
        Task<bool> DeleteLeaveAllocationAsync(int leaveAllocationId);
    }
}
