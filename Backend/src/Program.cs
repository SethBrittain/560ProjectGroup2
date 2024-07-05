using Microsoft.AspNetCore.Hosting;

namespace Pidgin;

public class Program
{
    public static async Task Main(string[] args)
    {
        await Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => {
            webBuilder.UseStartup<Startup>();
            webBuilder.UseUrls("http://localhost:5000", "https://localhost:5001", "http://192.168.1.106:5000");
        }).Build().RunAsync();
    }
}