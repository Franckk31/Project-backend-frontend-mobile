using ProductApi.Models;

namespace ProductApi.Services
{
    public interface IUsersService
    {
        IEnumerable<Users> GetAll();
        Users? GetById(int id);
        Users Create(Users user);
        bool Update(int id, Users user);
        bool Delete(int id);
    }
}