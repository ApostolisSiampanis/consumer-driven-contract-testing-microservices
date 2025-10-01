using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PactNet;

namespace OrdersService.Tests;

public sealed class DiscountSvcMock
{
    private readonly PactConfig _config;
    private const string ConsumerName = "OrdersService";
    private const string ProviderName = "DiscountService";

    public DiscountSvcMock()
    {
        _config = new PactConfig
        {
            PactDir = Path.Combine("..", "..", "..", "pact"),
            LogLevel = PactLogLevel.Debug
        };
    }

    private Task VerifyAsync(Action<dynamic> defineInteractions, Func<Uri, Task> test)
    {
        var pact = Pact.V2(ConsumerName, ProviderName, _config);
        var api = pact.WithHttpInteractions();
        defineInteractions(api);

        return api.VerifyAsync(async ctx => { await test(ctx.MockServerUri); });
    }

    public Task VerifyAsync(Action<dynamic> defineInteractions, Func<HttpClient, Task> test)
    {
        return VerifyAsync(defineInteractions, async baseUri =>
        {
            using var client = new HttpClient();
            client.BaseAddress = baseUri;
            await test(client);
        });
    }
}