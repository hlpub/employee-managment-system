
namespace Employees.Core.Domain.Models
{
    public class Employee : ModelBase
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string JobTitle { get; set; }
        public DateTime DateOfJoining { get; set; }
        public int? YearsOfService { get => (DateTime.Now - DateOfJoining).Days / 365; }
    }
}
