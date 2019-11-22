using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mqtt.Server.Sdk;


namespace System.Net.Mqtt.Sdk
{
	internal class DistanceCalculator
	{

		private class Units
		{
			public static readonly string Meters = "m";
			public static readonly string KiloMeters = "km";
		}


		private static readonly double EarthRadius = 6371e3; //meters

		/// <summary>
		/// Wrapper around the method P2PDistanceBetweenLatLon that uses Tuple for lat lon.
		/// It is suggested to use this wrapper.
		/// </summary>
		/// <param name="subscription">The GeoInfos about the subscription</param>
		/// <param name="publish">The GeoInfos about the publish</param>
		/// <returns>The distance in meter or kilometers</returns>
		public static double P2PDistanceBetweenLatLon(GeoInfos subscription, GeoInfos publish)
		{
			return P2PDistanceBetweenLatLon(new Tuple<double, double>(subscription.Latitude, subscription.Longitude),
												new Tuple<double, double>(publish.Latitude, publish.Longitude),
												subscription.ShapeParametersUnit);
		}

		public static double P2PDistanceBetweenLatLon(Tuple<double, double> first, Tuple<double, double> second, string unit)
		{
			unit = unit.ToLower();
			double multiplicator; // Meters

            if (!IsCorrectMeasureUnit(unit, out multiplicator))
                Console.WriteLine($"Something is wrong with measure unit: ${unit}. " +
                    $"The System understands only ${Units.KiloMeters} and ${Units.Meters}.\n" +
                    $"Will consider meters as default.");

			var first_latToRad = second.Item1.ToRadians();
			var second_latToRad = second.Item1.ToRadians();

			var delta_lat = (second.Item1 - first.Item1).ToRadians();
			var delta_lon = (second.Item2 - first.Item2).ToRadians();

			var a = (Math.Sin(delta_lat / 2) * Math.Sin(delta_lat / 2)) +
				(Math.Cos(first_latToRad) * Math.Cos(second_latToRad) *
				Math.Sin(delta_lon / 2) * Math.Sin(delta_lon / 2));

			var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

			var distance = EarthRadius * c; // in Meters.

            return distance * multiplicator;
		}

		private static bool IsCorrectMeasureUnit(string unit, out double multiplicator)
        {
            unit = unit.ToLower();
			multiplicator = 1f;
            
			if (Convert.ToBoolean(multiplicator))
                multiplicator = !Convert.ToBoolean(unit.CompareTo(Units.KiloMeters)) ? (1f / 1000f) : 1f;

            return Convert.ToBoolean(unit.CompareTo(Units.KiloMeters)) || Convert.ToBoolean(unit.CompareTo(Units.Meters));
        }

	}

	internal static class DoubleExtension
	{
		public static double ToRadians(this double number)
		{
			return number * (Math.PI / 180);
		}
	}
}
