using LMS.Domain.Employees;

namespace EcommerceApplication.Employees
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllCustomers();
        Task CreateEmployee(Employee employee);
        Task UpdateEmployee(Employee employee);
        Task DeleteEmployee(string id);

    }
}
