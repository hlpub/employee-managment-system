namespace Employees.Core.Services
{
    public interface IUserService
    {
        bool isAdmin(string adminKey);
    }
}