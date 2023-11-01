using pidgin.models;

namespace pidgin.services
{
    public interface IUserService
    {
        Task<User> GetUserById(int id);
        Task<User> GetUserByEmail(string email);
        int CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
    }
}