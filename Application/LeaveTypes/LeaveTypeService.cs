using EcommerceDomain.LeaveTypes;
using LMS.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApplication.LeaveTypes
{
    public class LeaveTypeService : ILeaveTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LeaveTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<LeaveType>> GetAllLeaveTypesAsync()
        {
            return await _unitOfWork.Repository<LeaveType>().GetAll().ToListAsync();
        }

        public async Task InitializeLeaveTypesAsync()
        {
            var existingTypes = await _unitOfWork.Repository<LeaveType>().GetAll().ToListAsync();
            if (existingTypes.Any())
                return; // Leave types are already seeded

            var leaveTypes = new List<LeaveType>
            {
                new LeaveType { Name = "Annual", DefaultDays = 0 },
                new LeaveType { Name = "Sick", DefaultDays = 20 },
                new LeaveType { Name = "Replacement", DefaultDays = 0 }, 
                new LeaveType { Name = "Unpaid", DefaultDays = 10 }
            };

            foreach (var leaveType in leaveTypes)
            {
                _unitOfWork.Repository<LeaveType>().Create(leaveType);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
