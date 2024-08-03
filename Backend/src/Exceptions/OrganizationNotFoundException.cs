namespace Pidgin.Exceptions;

public sealed class OrganizationNotFoundException : Exception
{
	public OrganizationNotFoundException(string message) : base(message){}
}