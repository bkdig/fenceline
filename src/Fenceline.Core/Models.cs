using System;

namespace Fenceline.Core;

public enum HeadingSource
{
    Vtg,
    Calculated,
    Held
}

public sealed record GeoSample(
    DateTime TimestampUtc,
    double Latitude,
    double Longitude,
    double? AltitudeMeters,
    int FixQuality,
    double? Hdop,
    int? Satellites,
    double? CorrectionAgeSeconds,
    double? HeadingDegrees,
    double? SpeedMph,
    HeadingSource HeadingSource);

public sealed record OffsetConfig(
    double LeftRightFeet,
    double ForwardBackFeet,
    double MinSpeedMph = 1.0);

public sealed record OffsetPoint(double Latitude, double Longitude);
