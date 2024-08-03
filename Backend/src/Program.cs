using System.Collections;
using Microsoft.AspNetCore.Hosting;

namespace Pidgin;

public class Program
{
    public static async Task Main(string[] args)
    {
		Console.WriteLine("Environment Variables: " + string.Join(Environment.NewLine, 
    Environment.GetEnvironmentVariables()
       .Cast<DictionaryEntry>()
       .Select(de => $"{de.Key}={de.Value}")));
		Console.CancelKeyPress += (sender, e) => Environment.Exit(0);
        await Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => {
			webBuilder.UseWebRoot("wwwroot");
            webBuilder.UseStartup<Startup>();
            webBuilder.UseUrls("http://0.0.0.0:5000");
        }).Build().RunAsync();
    }
}
