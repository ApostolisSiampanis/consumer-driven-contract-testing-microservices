using PactNet;
using PactNet.Verifier;
using Xunit.Abstractions;

namespace DiscountService.Tests;

public class ProviderPactTests : IClassFixture<ProviderApiTestServer>
{
    private readonly ProviderApiTestServer _testServer;
    private readonly ITestOutputHelper _output;

    public ProviderPactTests(ProviderApiTestServer fixture, ITestOutputHelper output)
    {
        _testServer = fixture;
        _output = output;
    }

    [Fact]
    public void VerifyConsumerPact()
    {
        var config = new PactVerifierConfig
        {
            LogLevel = PactLogLevel.Debug
        };

        var pactPath = Path.Combine("..", "..", "..", "..", "OrdersSrv-Consumer.Tests", "pacts", "OrdersService-DiscountService.json");

        IPactVerifier verifier = new PactVerifier(_testServer.BaseAddress.ToString(), config);

        verifier
            .WithHttpEndpoint(_testServer.BaseAddress)
            .WithFileSource(new FileInfo(pactPath))
            .Verify();
    }
}