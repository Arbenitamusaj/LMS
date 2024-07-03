using EcommerceApplication.Email;
using EcommerceDomain.LeaveAllovations;
using EcommerceDomain.LeaveRequests;
using LMS.Application.Interfaces;
using LMS.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApplication.LeaveRequests
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public LeaveRequestService(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<List<LeaveRequest>> GetAllLeaveRequestsAsync()
        {
            return await _unitOfWork.Repository<LeaveRequest>().GetAll().ToListAsync();
        }

        public async Task CreateLeaveRequestAsync(string employeeId, DateTime startDate, DateTime endDate, int leaveTypeId, string requestComments)
        {
            var leaveRequest = new LeaveRequest
            {
                EmployeeId = employeeId,
                LeaveTypeId = leaveTypeId,
                StartDate = startDate,
                EndDate = endDate,
                RequestComments = requestComments,
                Approved = false,
                RequestDate = DateTime.UtcNow
            };

            _unitOfWork.Repository<LeaveRequest>().Create(leaveRequest);
            _unitOfWork.Complete();

            // Send email notification to lead
            await SendLeaveRequestNotificationAsync(employeeId, leaveRequest.Id);
        }

        public async Task<bool> ApproveLeaveRequestAsync(int leaveRequestId, string approvedById)
        {
            var leaveRequest = await _unitOfWork.Repository<LeaveRequest>().GetByCondition(lr => lr.Id == leaveRequestId).FirstOrDefaultAsync();
            if (leaveRequest == null)
                return false;

            leaveRequest.Approved = true;
            leaveRequest.ApprovedById = approvedById;

            _unitOfWork.Repository<LeaveRequest>().Update(leaveRequest);
            _unitOfWork.Complete();

            // Subtract approved days from leave allocation
            await SubtractLeaveDaysAsync(leaveRequest.EmployeeId, leaveRequest.LeaveTypeId, (leaveRequest.EndDate - leaveRequest.StartDate).Days);

            return true;
        }

        public async Task<bool> CancelLeaveRequestAsync(int leaveRequestId)
        {
            var leaveRequest = await _unitOfWork.Repository<LeaveRequest>().GetByCondition(lr => lr.Id == leaveRequestId).FirstOrDefaultAsync();
            if (leaveRequest == null)
                return false;

            _unitOfWork.Repository<LeaveRequest>().Delete(leaveRequest);
            _unitOfWork.Complete();

            return true;
        }

        private async Task SendLeaveRequestNotificationAsync(string employeeId, int leaveRequestId)
        {
            var employee = await _unitOfWork.Repository<Employee>().GetByCondition(e => e.Id == employeeId).FirstOrDefaultAsync();
            if (employee != null && !string.IsNullOrEmpty(employee.LeadId))
            {
                var lead = await _unitOfWork.Repository<Employee>().GetByCondition(e => e.Id == employee.LeadId).FirstOrDefaultAsync();
                if (lead != null)
                {
                    var emailSubject = $"Leave Request Submission - Employee: {employee.Name}";
                    var emailBody = $"Dear {lead.Name},\n\nAn employee under your supervision has submitted a leave request. Please review it in the application.\n\nThank you.";
                    await _emailService.SendEmailAsync(lead.Email, emailSubject, emailBody);
                }
            }
        }

        private async Task SubtractLeaveDaysAsync(string employeeId, int leaveTypeId, int days)
        {
            var allocation = await _unitOfWork.Repository<LeaveAllocation>().GetByCondition(a => a.EmployeeId == employeeId && a.LeaveTypeId == leaveTypeId).FirstOrDefaultAsync();
            if (allocation != null)
            {
                allocation.NumberOfDays -= days;
                _unitOfWork.Repository<LeaveAllocation>().Update(allocation);
                _unitOfWork.Complete();
            }
        }
    }

}
