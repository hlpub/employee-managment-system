using Employees.Core.Domain.Models;
using Employees.Core.Domain.Repositories;
using Employees.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Employees.Controllers
{
    [ApiController]
    [Route("employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly ILogger<EmployeesController> _logger;
        private readonly IBaseRepository<Employee> _employeeRepository;
        private readonly IEmployeeService _employeeService;
        private readonly IUserService _userService;
        public EmployeesController(ILogger<EmployeesController> logger,
            IBaseRepository<Employee> employeeRepository, IEmployeeService employeeService,
            IUserService userService)
        {
            _logger = logger;
            _employeeRepository = employeeRepository;
            _employeeService = employeeService;
            _userService = userService;
        }

        [HttpGet]
        [HttpGet("{search?}")]
        public async Task<IActionResult> Get([FromRoute] string? search,
            [FromHeader(Name = "x-admin-key")] string adminKey, CancellationToken cancellationToken)
        {
            try
            {
                if (!search.IsNullOrEmpty() && !_userService.isAdmin(adminKey))
                    return Unauthorized();

                _logger.LogInformation("Fetching Employees");

                var employees = await _employeeService.SearchAsync(search, adminKey, cancellationToken);

                return Ok(employees);
            }

            catch (Exception ex)
            {
                _logger.LogInformation("Unable to fetch Employees: {0}", ex);
                throw;
            }
        }

        [HttpGet("employee/{id}")]
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

        [HttpPost("employee")]
        public async Task<IActionResult> Create([FromBody] Employee employee, CancellationToken cancellationToken)
        {
            if (employee is null || !_employeeService.IsValid(employee))
                return BadRequest("All fields should be provided");

            try
            {
                _logger.LogInformation("Creating employee {0}", employee.Email);

                if (await _employeeRepository.AnyAsync(x => x.Email == employee.Email, cancellationToken))
                    return BadRequest("There's another employee with this email");

                var newId = await _employeeRepository.CreateAsync(employee, cancellationToken);

                return Ok(new { id = newId });
            }

            catch (Exception ex)
            {
                _logger.LogError("Unable to create new employee. {0}", ex);
                throw;
            }
        }

        [HttpPut("employee/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id,
            [FromBody] Employee updatedEmployee, CancellationToken cancellationToken)
        {
            if (updatedEmployee is null || id == 0 || !_employeeService.IsValid(updatedEmployee))
                return BadRequest("All fields should be provided");

            try
            {
                var employee = await _employeeRepository.GetByIdAsync(id, cancellationToken);

                if (employee is null)
                    return NotFound();

                if (await _employeeRepository.AnyAsync(x => x.Id != id &&
                        x.Email == employee.Email, cancellationToken))
                    return BadRequest("There's another employee with this email");

                _logger.LogInformation("Updating employee {0}", updatedEmployee.Email);

                await _employeeService.UpdateEmployeeAsync(employee, updatedEmployee, cancellationToken);

                return Ok();
            }

            catch (Exception ex)
            {
                _logger.LogError("Unable to update employee. {0}", ex);
                throw;
            }
        }

    }
}