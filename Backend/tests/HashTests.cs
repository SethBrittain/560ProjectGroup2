using BCrypt.Net;
using Xunit;
using System.Text;

namespace Pidgin.Tests;

public static class HashTests
{
	[Fact]
	public static void TestHash()
	{
		string testPassword = "password";
		string testPasswordHash = BCrypt.Net.BCrypt.HashPassword(testPassword, workFactor: 12);
		Assert.True(BCrypt.Net.BCrypt.Verify(testPassword, testPasswordHash));

		byte[] hashBytes = Encoding.UTF8.GetBytes(testPasswordHash);
		string hashString = Encoding.UTF8.GetString(hashBytes);

		Console.WriteLine(testPassword);
		Console.WriteLine(hashString);
		Console.WriteLine(hashBytes.Length);

		bool verified = BCrypt.Net.BCrypt.Verify(testPassword, hashString);
		Console.WriteLine("Verify: " + verified);
		Assert.True(verified);
	}
}