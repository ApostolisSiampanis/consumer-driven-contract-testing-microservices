using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit.Abstractions;
using IOutput = PactNet.Infrastructure.Outputters.IOutput;

namespace ProviderTests;

public class XUnitOutput : IOutput
{
    private readonly ITestOutputHelper _output;
    
    public XUnitOutput(ITestOutputHelper output)
    {
        _output = output;
    }

    public void Write(string message, OutputLevel level)
    {
        _output.WriteLine(message);
    }

    public void WriteLine(string line)
    {
        _output.WriteLine(line);
    }

    public void WriteLine(string message, OutputLevel level)
    {
        _output.WriteLine(message);
    }
}