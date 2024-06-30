using EcommerceDomain.LeaveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApplication.LeaveTypes
{
    public interface ILeaveTypeService
    {
        Task<List<LeaveType>> GetAllLeaveTypesAsync();
        Task InitializeLeaveTypesAsync();
    }
}
