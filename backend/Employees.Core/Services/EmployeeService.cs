using Employees.Core.Domain.Models;
using Employees.Core.Domain.Repositories;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Employees.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUserService _userService;
        private readonly IBaseRepository<Employee> _employeeRepository;

        public EmployeeService(IUserService userService, IBaseRepository<Employee> employeeRepository)
        {
            _userService = userService;
            _employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<Employee>> SearchAsync(string search, CancellationToken cancellationToken)
        {
            IEnumerable<Employee> employees;

            if (_userService.isAdmin() && !search.IsNullOrEmpty())
            {
                employees = await _employeeRepository.SearchAsync(x => x.FirstName.Contains(search) ||
                    x.LastName.Contains(search) || x.JobTitle.Contains(search), cancellationToken);
            }
            else
            {
                employees = await _employeeRepository.GetAsync(cancellationToken);
            }

            return employees;
        }
    }
}
