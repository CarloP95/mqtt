using System;
using System.Linq;

namespace System.Net.Mqtt.Server.Sdk
{
	/// <summary>
	/// The topic will be expressed in the following way: 
	/// topics/<b>loc</b>/ll=xx.xx,yy.yy&s=circle&r=15&u=(m|km)&t=(timestamp)
	/// So this class will unmarshall into a GeoInfos object all of the attributes that can be found
	/// into the topic.
	/// </summary>
	internal class GeoTopicUnmashaller {

		class TopicFieldProperties
		{
			public static readonly string Location = "loc";
			public static readonly string LatLon = "ll";
			public static readonly string Shape = "s";
			public static readonly string Radius = "r";
			public static readonly string Unit = "u";
			public static readonly string Timestamp = "t";
		}

		
		static public GeoInfos Unmarshal (string topic) {

			if (!topic.Contains(TopicFieldProperties.Location))
				throw new FormatException($"The following topic {topic} is not a location based topic. Example: topics/loc/ll=xx.xx,yy.yy&...&...");

			var provider = new Globalization.NumberFormatInfo();
			provider.NumberDecimalSeparator = ".";

			var topicParts = topic.Split('/');
			var splittedTopicParts = topicParts.SelectMany( part => part.Split(separator));

			double latitude, longitude = latitude = 0.0;
			string shape, radius, unit, timestamp = unit = radius = shape = String.Empty;

			foreach(var part in splittedTopicParts) {

				var lower_part = part.ToLower();

				if (lower_part.StartsWith(TopicFieldProperties.LatLon)) {
					var lat_lon = lower_part.Split(new Char[]{equals, comma});
					latitude = Convert.ToDouble(lat_lon[1], provider);
					longitude = Convert.ToDouble(lat_lon[2], provider);
					continue;
				}

				if (lower_part.StartsWith(TopicFieldProperties.Shape))
				{
					SplitAndPopulate(lower_part, equals, out shape);
					continue;
				}

				if (lower_part.StartsWith(TopicFieldProperties.Radius))
				{
					SplitAndPopulate(lower_part, equals, out radius);
					continue;
				}

				if (lower_part.StartsWith(TopicFieldProperties.Unit))
				{
					SplitAndPopulate(lower_part, equals, out unit);
					continue;
				}

				if (lower_part.StartsWith(TopicFieldProperties.Timestamp))
				{
					SplitAndPopulate(lower_part, equals, out timestamp);
					continue;
				}

			}

			return new GeoInfos {

				Latitude = latitude,
				Longitude = longitude,
				Shape = shape,
				ShapeParameters = Convert.ToDouble(radius), //Must support different Polygon
				ShapeParametersUnit = unit,
				Timestamp = timestamp

			};
		}

		static private void SplitAndPopulate(string part, char separator, out string populateVar)
		{
			var part_splitted = part.Split(separator);
			populateVar = part_splitted[1];
		}

		public static char equals = '=';
		public static char comma = ',';
		public static char separator = '&';
	}
}
