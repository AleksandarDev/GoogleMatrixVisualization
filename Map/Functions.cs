using Microsoft.Maps.MapControl.WPF;
using System;

namespace WpfApplication1.Map
{
    /// <summary>
    /// The helper class that contains some map functions.
    /// </summary>
    public static class MapFunctions
    {
        private const double EarthRadius = 6371;

        public static Location CalculateUsingHaversine(Location startLocation, double distance, double bearing)
        {
            var lat1 = DegreeToRadian(startLocation.Latitude);
            var long1 = DegreeToRadian(startLocation.Longitude);

            var bar1 = DegreeToRadian(bearing);
            var angularDistance = distance / EarthRadius;

            var lat2 = Math.Asin(Math.Sin(lat1) * Math.Cos(angularDistance) + Math.Cos(lat1) * Math.Sin(angularDistance) * Math.Cos(bar1));

            var lon2 = long1 + Math.Atan2(Math.Sin(bar1) * Math.Sin(angularDistance) * Math.Cos(lat1),
                                            Math.Cos(angularDistance) - Math.Sin(lat1) * Math.Sin(lat2));

            var destinationLocation = new Location(RadianToDegree(lat2), RadianToDegree(lon2));

            return destinationLocation;
        }

        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }
    }
}
