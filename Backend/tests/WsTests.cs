namespace Pidgin.Tests;

using Pidgin.Util;
using Xunit;

public static class WsTests
{
	[Fact]
	public static void TestDirectKey()
	{
		Dictionary<WebSocketDirectKey, string> dict = new();
		var key1 = new WebSocketDirectKey(1, 2);
		var key2 = new WebSocketDirectKey(2, 1);

		dict[key1] = "key1result";
		Assert.Equal("key1result", dict[key1]);
		Assert.Equal("key1result", dict[key2]);
		Assert.Throws<KeyNotFoundException>(() => dict[new WebSocketDirectKey(1, 3)]);
	}
}