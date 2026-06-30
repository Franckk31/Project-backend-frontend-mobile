using ProductApi.Data;
using ProductApi.Models;

namespace ProductApi.Services
{
    public class UserDbService : IUsersService
    {
        private readonly AppDbContext _context;

        public UserDbService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Users> GetAll()
        {
            return _context.Users.ToList();
        }

        public Users? GetById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == id);
        }

        public Users Create(Users user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public bool Update(int id, Users user)
        {
            var existing = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (existing == null) return false;

            existing.Username = user.Username;
            existing.Password = user.Password;
            existing.Name = user.Name;
            existing.Role = user.Role;
            existing.ContactInfo = user.ContactInfo;

            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var existing = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (existing == null) return false;

            _context.Users.Remove(existing);
            _context.SaveChanges();
            return true;
        }
    }
}