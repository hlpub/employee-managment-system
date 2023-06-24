using System.ComponentModel.DataAnnotations;

namespace Employees.Core.Domain.Models
{
    public abstract class ModelBase
    {
        [Key]
        public int Id { get; private set; }
    }
}