namespace Pidgin.Services;


public interface IPasswordService
{
	public string HashPassword(string password);
	public bool IsValidPassword(string password);
	public Task<bool> EmailExists(string email);
	public Task<int> ValidateCredentials(string email, string password);
}