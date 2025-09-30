using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiscountService.Tests;

public class ProviderApiTestServer : IDisposable
{
    private readonly IHost _host;
    public Uri BaseAddress { get; } = new Uri("http://localhost:6001");

    public ProviderApiTestServer()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseKestrel();
                webBuilder.UseUrls(BaseAddress.ToString());
                webBuilder.ConfigureServices(services =>
                {
                    services.AddControllers()
                        .AddApplicationPart(typeof(DiscountService.Controllers.DiscountController).Assembly)
                        .AddControllersAsServices();
                });
                webBuilder.Configure(app =>
                {
                    var env = app.ApplicationServices.GetRequiredService<IHostEnvironment>();
                    if (env.IsDevelopment())
                    {
                        // No Swagger in tests; keep pipeline minimal
                    }

                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });
                });
            })
            .Build();
        _host.Start();
    }

    public void Dispose()
    {
        _host.StopAsync().GetAwaiter().GetResult();
        _host.Dispose();
    }
}