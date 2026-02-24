using System;
using System.Globalization;

namespace Fenceline.Core;

public static class NmeaParser
{
    public static bool IsChecksumValid(string sentence)
    {
        if (string.IsNullOrWhiteSpace(sentence) || sentence[0] != '$')
        {
            return false;
        }

        var star = sentence.IndexOf('*');
        if (star <= 1 || star >= sentence.Length - 2)
        {
            return false;
        }

        var payload = sentence[1..star];
        var checksumText = sentence[(star + 1)..].Trim();

        var checksum = 0;
        foreach (var ch in payload)
        {
            checksum ^= ch;
        }

        return int.TryParse(checksumText, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var expected)
               && checksum == expected;
    }

    public static bool TryParseGga(string sentence, out (double lat, double lon, int fixQuality, double? altitude, double? hdop, int? sats) result)
    {
        result = default;
        if (!IsChecksumValid(sentence))
        {
            return false;
        }

        var fields = sentence.Split(',');
        if (fields.Length < 10 || !fields[0].EndsWith("GGA", StringComparison.Ordinal))
        {
            return false;
        }

        if (!TryParseLatLon(fields[2], fields[3], fields[4], fields[5], out var lat, out var lon))
        {
            return false;
        }

        if (!int.TryParse(fields[6], NumberStyles.Integer, CultureInfo.InvariantCulture, out var fixQuality))
        {
            return false;
        }

        _ = int.TryParse(fields[7], NumberStyles.Integer, CultureInfo.InvariantCulture, out var sats);
        _ = double.TryParse(fields[8], NumberStyles.Float, CultureInfo.InvariantCulture, out var hdop);
        _ = double.TryParse(fields[9], NumberStyles.Float, CultureInfo.InvariantCulture, out var altitude);

        result = (lat, lon, fixQuality, altitude, hdop, sats);
        return true;
    }

    private static bool TryParseLatLon(string latRaw, string latDir, string lonRaw, string lonDir, out double lat, out double lon)
    {
        lat = default;
        lon = default;

        if (!double.TryParse(latRaw, NumberStyles.Float, CultureInfo.InvariantCulture, out var latNmea)
            || !double.TryParse(lonRaw, NumberStyles.Float, CultureInfo.InvariantCulture, out var lonNmea))
        {
            return false;
        }

        lat = NmeaToDegrees(latNmea);
        lon = NmeaToDegrees(lonNmea);

        if (latDir.Equals("S", StringComparison.OrdinalIgnoreCase))
        {
            lat = -lat;
        }

        if (lonDir.Equals("W", StringComparison.OrdinalIgnoreCase))
        {
            lon = -lon;
        }

        return true;
    }

    private static double NmeaToDegrees(double nmea)
    {
        var deg = Math.Floor(nmea / 100);
        var minutes = nmea - (deg * 100);
        return deg + (minutes / 60.0);
    }
}
