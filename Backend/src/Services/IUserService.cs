using pidgin.models;

namespace pidgin.services
{
    public interface IUserService
    {
		Task<User> GetUserById(long id);
		Task<User> GetUserByEmail(string email);
		Task<User> RegisterUser(int orgId, string email, string firstName, string lastName, string title, string profilePhotoUrl);
		void DeleteUser(User user);
		Task<User> UpdateUser(User user);
    }
}