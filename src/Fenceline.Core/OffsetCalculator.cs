using System;

namespace Fenceline.Core;

public static class OffsetCalculator
{
    private const double EarthRadiusMeters = 6378137.0;
    private const double FeetToMeters = 0.3048;

    public static OffsetPoint Apply(double latitude, double longitude, double headingDegrees, OffsetConfig config)
    {
        var lateralMeters = config.LeftRightFeet * FeetToMeters;
        var forwardMeters = config.ForwardBackFeet * FeetToMeters;

        var headingRad = DegreesToRadians(headingDegrees);

        // positive left is heading - 90°, positive forward follows heading.
        var northMeters = (forwardMeters * Math.Cos(headingRad)) + (lateralMeters * Math.Cos(headingRad - (Math.PI / 2.0)));
        var eastMeters = (forwardMeters * Math.Sin(headingRad)) + (lateralMeters * Math.Sin(headingRad - (Math.PI / 2.0)));

        var latRad = DegreesToRadians(latitude);
        var dLat = northMeters / EarthRadiusMeters;
        var dLon = eastMeters / (EarthRadiusMeters * Math.Cos(latRad));

        var shiftedLat = latitude + RadiansToDegrees(dLat);
        var shiftedLon = longitude + RadiansToDegrees(dLon);
        return new OffsetPoint(shiftedLat, shiftedLon);
    }

    private static double DegreesToRadians(double value) => value * Math.PI / 180.0;

    private static double RadiansToDegrees(double value) => value * 180.0 / Math.PI;
}
