using System.Text.Json.Serialization;
using DiscountSvc_Provider.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PactNet;
using PactNet.Verifier;
using Xunit.Abstractions;
using System.Linq;

namespace ProviderTests;

public class DiscountServiceTests : IDisposable
{
    private readonly ITestOutputHelper _output;
    private bool _disposed;
    private string _serviceUri;
    private IHost? _host;

    public DiscountServiceTests(ITestOutputHelper output)
    {
        _output = output;
        _serviceUri = "http://localhost:5100";
    }

    [Fact]
    public void PactWithOrderSvcShouldBeVerified()
    {
        var config = new PactVerifierConfig
        {
            LogLevel = PactLogLevel.Debug
        };

        // Point to the pact produced by the consumer tests
        var pactPath = Path.Combine("..", "..", "..", "..", "OrdersSrv-Consumer.Tests", "pact", "Orders-Discounts.json");
        _output.WriteLine($"Verifying pact at: {Path.GetFullPath(pactPath)} against provider: {_serviceUri}");

        IPactVerifier verifier = new PactVerifier(_serviceUri, config);

        verifier
            .WithHttpEndpoint(new Uri(_serviceUri))
            .WithFileSource(new FileInfo(pactPath))
            .Verify();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _host?.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}