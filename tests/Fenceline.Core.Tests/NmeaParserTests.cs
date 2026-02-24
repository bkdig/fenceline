using Fenceline.Core;
using Xunit;

namespace Fenceline.Core.Tests;

public sealed class NmeaParserTests
{
    [Fact]
    public void ValidatesChecksumForKnownSentence()
    {
        const string sentence = "$GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,47.0,M,,*47";
        Assert.True(NmeaParser.IsChecksumValid(sentence));
    }

    [Fact]
    public void ParsesBasicGgaSentence()
    {
        const string sentence = "$GPGGA,123519,4807.038,N,01131.000,E,4,12,0.8,545.4,M,47.0,M,,*40";

        var ok = NmeaParser.TryParseGga(sentence, out var result);

        Assert.True(ok);
        Assert.Equal(4, result.fixQuality);
        Assert.Equal(48.1173, result.lat, 4);
        Assert.Equal(11.5167, result.lon, 4);
    }
}
