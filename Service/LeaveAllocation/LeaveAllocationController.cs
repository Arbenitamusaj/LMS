using EcommerceApplication.LeaveAllocations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceService.LeaveAllocation
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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateLeaveAllocation([FromBody] LeaveAllocation leaveAllocation)
        {
            await _leaveAllocationService.CreateLeaveAllocationAsync(leaveAllocation);
            return CreatedAtAction(nameof(GetAllLeaveAllocations), new { id = leaveAllocation.Id }, leaveAllocation);
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteLeaveAllocation(int id)
        {
            await _leaveAllocationService.DeleteLeaveAllocationAsync(id);
            return NoContent();
        }
    }
}
