using EcommerceDomain.LeaveRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApplication.LeaveRequests
{
    public interface ILeaveRequestService
    {
        Task<List<LeaveRequest>> GetAllLeaveRequestsAsync();
        Task CreateLeaveRequestAsync(string employeeId, DateTime startDate, DateTime endDate, int leaveTypeId, string requestComments);
        Task<bool> ApproveLeaveRequestAsync(int leaveRequestId, string approvedById);
        Task<bool> CancelLeaveRequestAsync(int leaveRequestId);
    }
}
