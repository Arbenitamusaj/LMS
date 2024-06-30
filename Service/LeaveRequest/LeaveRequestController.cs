using EcommerceApplication.LeaveRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceService.LeaveRequest
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveRequestController(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<List<LeaveRequest>>> GetAllLeaveRequests()
        {
            var leaveRequests = await _leaveRequestService.GetAllLeaveRequestsAsync();
            return Ok(leaveRequests);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateLeaveRequest([FromBody] LeaveRequestDto requestDto)
        {
            await _leaveRequestService.CreateLeaveRequestAsync(requestDto.EmployeeId, requestDto.StartDate, requestDto.EndDate, requestDto.LeaveTypeId, requestDto.RequestComments);
            return CreatedAtAction(nameof(GetAllLeaveRequests), new { id = requestDto.EmployeeId }, requestDto);
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> ApproveLeaveRequest(int id, [FromBody] ApproveLeaveRequestDto approveDto)
        {
            var success = await _leaveRequestService.ApproveLeaveRequestAsync(id, approveDto.ApprovedById);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPut("{id}/cancel")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> CancelLeaveRequest(int id)
        {
            var success = await _leaveRequestService.CancelLeaveRequestAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
