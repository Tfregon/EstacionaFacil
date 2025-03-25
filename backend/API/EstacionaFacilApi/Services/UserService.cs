using EstacionaFacilAPI.Data;
using EstacionaFacilAPI.Models;
using MongoDB.Driver;

namespace EstacionaFacilAPI.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(MongoDBContext context)
        {
            _users = context.Users;
        }

        public async Task<User> RegisterAsync(User user)
        {
            var existingUser = await _users.Find(u => u.Username == user.Username).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                return null; // Já existe um usuário com esse username
            }

            await _users.InsertOneAsync(user);
            return user;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            return await _users.Find(u => u.Username == username && u.Password == password).FirstOrDefaultAsync();
        }
    }
}
