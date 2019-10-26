using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Net.Mqtt.Server.Sdk
{
	/// <summary>
	/// This class is used to encapsulate all of the information that are used 
	/// from the broker to understand what information to publish towards 
	/// subscripted clients.
	/// </summary>
	internal class GeoInfos
	{
		
		public double Latitude { get; set; }

		public double Longitude { get; set; }

		public string Shape { get; set; }

		public double ShapeParameters { get; set; }

		public string ShapeParametersUnit { get; set; }

		public string Timestamp { get; set; }
	}
}
