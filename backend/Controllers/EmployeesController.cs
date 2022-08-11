using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class EmployeesController : Controller
    {
        private readonly DatabaseContext _databaseContext;

        public EmployeesController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        [HttpGet]        
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _databaseContext.Employees.ToListAsync();
            return Ok(employees);
        }

        [HttpPost]       
        public async Task<IActionResult> AddEmployee(Employee employeerequest)
        {
            employeerequest.id = Guid.NewGuid();

            await _databaseContext.Employees.AddAsync(employeerequest);
            await _databaseContext.SaveChangesAsync();

            return Ok(employeerequest);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> getEmployee([FromRoute] Guid id) {
            var employee = await _databaseContext.Employees.FirstOrDefaultAsync(x => x.id == id);

            if (employee == null) {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> updateEmployee([FromRoute] Guid id, Employee updateEmployeeRequest) {
            var employee = await _databaseContext.Employees.FindAsync(id);

            if (employee == null) {
                return NotFound();
            }
            employee.Name = updateEmployeeRequest.Name;
            employee.Email = updateEmployeeRequest.Email;
            employee.Phone = updateEmployeeRequest.Phone;
            employee.Salary = updateEmployeeRequest.Salary;
            employee.Department = updateEmployeeRequest.Department;

            _databaseContext.SaveChangesAsync();
            return Ok(employee);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> deleteEmployee([FromRoute] Guid id) {
            var employee = await _databaseContext.Employees.FindAsync(id);

            if (employee == null) {
                return NotFound();
            }

            _databaseContext.Employees.Remove(employee);
            await _databaseContext.SaveChangesAsync();
            return Ok(employee);
        }
    }
}
