using System;

using System.Net.Mqtt;

namespace HermesBroker
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Started Program to start broker...");

			var mqttOptions = new MqttConfiguration {

				BufferSize = 128 * 1024,
				Port = 1883,
				KeepAliveSecs = 10,
				WaitTimeoutSecs = 2,
				MaximumQualityOfService = MqttQualityOfService.AtMostOnce,
				AllowWildcardsInTopicFilters = true,
				AllowLocationSubscription = true

			};

			var broker = MqttServer.Create(mqttOptions);
			try { 
				broker.Start();
				Console.WriteLine("Press CTRL + C exit.");
				while(true)
				{
					System.Threading.Thread.Sleep(1000);
				}
			}
			catch(Exception ex) {
				Console.WriteLine(ex);
			}
		}
	}
}
