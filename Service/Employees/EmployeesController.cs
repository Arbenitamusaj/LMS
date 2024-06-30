using EcommerceApplication.Employees;
using LMS.Domain.Employees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Service.Employees
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<List<Employee>>> GetAllEmployees()
        {
            var employees = await _employeeService.GetAllCustomers();
            return Ok(employees);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateEmployee([FromBody] Employee employee)
        {
            await _employeeService.CreateEmployee(employee);
            return CreatedAtAction(nameof(GetAllEmployees), new { id = employee.Id }, employee);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateEmployee(int id, [FromBody] Employee employee)
        {
            employee.Id = id;
            await _employeeService.UpdateEmployee(employee);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteEmployee(string id)
        {
            await _employeeService.DeleteEmployee(id);
            return NoContent();
        }
    }
}