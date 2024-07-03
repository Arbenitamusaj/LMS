using EcommerceApplication.LeaveTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceService.LeaveType
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveTypeController : ControllerBase
    {
        private readonly ILeaveTypeService _leaveTypeService;

        public LeaveTypeController(ILeaveTypeService leaveTypeService)
        {
            _leaveTypeService = leaveTypeService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<LeaveType>>> GetAllLeaveTypes()
        {
            var leaveTypes = await _leaveTypeService.GetAllLeaveTypesAsync();
            return Ok(leaveTypes);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateLeaveType([FromBody] LeaveType leaveType)
        {
            await _leaveTypeService.CreateLeaveTypeAsync(leaveType);
            return CreatedAtAction(nameof(GetAllLeaveTypes), new { id = leaveType.Id }, leaveType);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateLeaveType(int id, [FromBody] LeaveType leaveType)
        {
            leaveType.Id = id;
            await _leaveTypeService.UpdateLeaveTypeAsync(leaveType);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteLeaveType(int id)
        {
            await _leaveTypeService.DeleteLeaveTypeAsync(id);
            return NoContent();
        }
    }
}
