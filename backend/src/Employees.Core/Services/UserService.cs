

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Employees.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;

        public UserService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool isAdmin(string adminKey)
        {
            return !adminKey.IsNullOrEmpty() && 
                _configuration.GetSection("AdminKey").Value == adminKey;
        }
    }
}
