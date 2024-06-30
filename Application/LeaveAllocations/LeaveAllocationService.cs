using EcommerceDomain.LeaveAllovations;
using LMS.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApplication.LeaveAllocations
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveAllocationController : ControllerBase
    {
        private readonly ILeaveAllocationService _leaveAllocationService;

        public LeaveAllocationController(ILeaveAllocationService leaveAllocationService)
        {
            _leaveAllocationService = leaveAllocationService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<LeaveAllocation>>> GetAllLeaveAllocations()
        {
            var leaveAllocations = await _leaveAllocationService.GetAllLeaveAllocationsAsync();
            return Ok(leaveAllocations);
        }

        [HttpPost("allocate")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> AllocateLeaveDays([FromBody] AllocateLeaveDaysDto allocateLeaveDaysDto)
        {
            await _leaveAllocationService.AllocateLeaveDaysAsync(
                allocateLeaveDaysDto.EmployeeId,
                allocateLeaveDaysDto.LeaveTypeId,
                allocateLeaveDaysDto.NumberOfDays,
                allocateLeaveDaysDto.Period);

            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> AdjustLeaveAllocation(int id, [FromBody] AdjustLeaveAllocationDto adjustLeaveAllocationDto)
        {
            var success = await _leaveAllocationService.AdjustLeaveAllocationAsync(id, adjustLeaveAllocationDto.NewNumberOfDays);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> DeleteLeaveAllocation(int id)
        {
            var success = await _leaveAllocationService.DeleteLeaveAllocationAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}

