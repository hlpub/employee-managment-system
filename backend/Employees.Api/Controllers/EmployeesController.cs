
using Employees.Core.Domain.Models;
using Employees.Core.Domain.Repositories;
using Employees.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {

        private readonly ILogger<EmployeesController> _logger;
        private readonly IBaseRepository<Employee> _employeeRepository;
        private readonly IEmployeeService _employeeService;

        public EmployeesController(ILogger<EmployeesController> logger,
            IBaseRepository<Employee> employeeRepository, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeRepository = employeeRepository;
            _employeeService = employeeService;
        }

        [HttpGet]
        [HttpGet("{search}")]
        public async Task<IActionResult> Get(string search, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching Employees");

            try
            {
                var employees = await _employeeService.SearchAsync(search, cancellationToken);
                return Ok(employees);
            }

            catch (Exception ex)
            {
                _logger.LogInformation("Unable to fetch Employees: {0}", ex);
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching Employee ID: {0}", id);

            try
            {
                var employee = await _employeeRepository.GetByIdAsync(id, cancellationToken);

                if (employee is null)
                    return NotFound();

                return Ok(employee);
            }

            catch (Exception ex)
            {
                _logger.LogError("Unable to fetch Employee ID: {0},{1}", id, ex);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Employee employee, CancellationToken cancellationToken)
        {
            if (employee is null)
                return BadRequest();

            try
            {
                _logger.LogInformation("Creating employee {0}", employee.Email);

                var newId = await _employeeRepository.CreateAsync(employee, cancellationToken);

                return Ok(new { id = newId });
            }

            catch (Exception ex)
            {
                _logger.LogError("Unable to create new employee. {0}", ex);
                throw;
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Employee updatedEmployee, CancellationToken cancellationToken)
        {
            if (updatedEmployee is null || updatedEmployee.Id == 0)
                return BadRequest();

            try
            {
                var employee = await _employeeRepository.GetByIdAsync(updatedEmployee.Id, cancellationToken);

                if(employee is null)
                    return NotFound();

                _logger.LogInformation("Updating employee {0}", updatedEmployee.Email);

                employee.Email = updatedEmployee.Email;
                employee.JobTitle = updatedEmployee.JobTitle;
                employee.DateOfJoining = updatedEmployee.DateOfJoining;
                employee.FirstName = updatedEmployee.FirstName;
                employee.LastName = updatedEmployee.LastName;

                await _employeeRepository.SaveChangesAsync(cancellationToken);

                return Ok(employee);
            }

            catch (Exception ex)
            {
                _logger.LogError("Unable to update employee. {0}", ex);
                throw;
            }
        }

    }
}