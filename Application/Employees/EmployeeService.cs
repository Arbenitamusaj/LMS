using LMS.Application.Interfaces;
using LMS.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceApplication.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Employee>> GetAllCustomers()
        {
            return await _unitOfWork.Repository<Employee>().GetAll().ToListAsync();
        }

        public async Task CreateEmployee(Employee employee)
        {
            _unitOfWork.Repository<Employee>().Create(employee);
            _unitOfWork.Complete();
        }

        public async Task UpdateEmployee(Employee employee)
        {
            _unitOfWork.Repository<Employee>().Update(employee);
            _unitOfWork.Complete();
        }

        public async Task DeleteEmployee(string id)
        {
            var employee = await _unitOfWork.Repository<Employee>().GetByCondition(e => e.Id == id).FirstOrDefaultAsync();
            if (employee != null)
            {
                _unitOfWork.Repository<Employee>().Delete(employee);
                _unitOfWork.Complete();
            }
        }
    }
}
