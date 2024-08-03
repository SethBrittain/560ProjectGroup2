namespace Pidgin.Exceptions;

public sealed class UserNotFoundException : Exception
{
	public UserNotFoundException() : base("User not found"){}
}