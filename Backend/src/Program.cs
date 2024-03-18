using Microsoft.AspNetCore.Hosting;

namespace Pidgin;

public class Program
{
    public static async Task Main(string[] args)
    {
        await Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => {
            webBuilder.UseStartup<Startup>();
            webBuilder.UseUrls("http://localhost:5000", "https://localhost:5001");
        }).Build().RunAsync();
    }
}