using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(WebStorageSystem.Areas.Identity.IdentityHostingStartup))]
namespace WebStorageSystem.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}