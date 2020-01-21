namespace System.Net.Mqtt.Sdk
{
	using StackExchange.Redis;
	public class RedisAuthenticationProvider : IMqttAuthenticationProvider
    {
        static readonly Lazy<RedisAuthenticationProvider> instance;
		static ConnectionMultiplexer redis;
		static IDatabase sessionStorage;

        static RedisAuthenticationProvider()
        {
            instance = new Lazy<RedisAuthenticationProvider>(() => new RedisAuthenticationProvider());
        }

        RedisAuthenticationProvider()
        {
			ConfigurationOptions opts = new ConfigurationOptions
			{
				EndPoints =
				{
					{ Environment.GetEnvironmentVariable("REDIS_HOST"), Int32.Parse(Environment.GetEnvironmentVariable("REDIS_PORT")) }
				},
				ResolveDns = true,
				Password = Environment.GetEnvironmentVariable("REDIS_KEY")
			};

			redis = ConnectionMultiplexer.Connect(opts);
			sessionStorage = redis.GetDatabase(0);
		}

        public static IMqttAuthenticationProvider Instance { get { return instance.Value; } }

        public bool Authenticate(string clientId, string username, string password)
        {
			var sessionId	 = username;
			var token		 = password;

			var retrievedToken = sessionStorage.StringGet(sessionId);

			if (retrievedToken.CompareTo(token) == 0)
				return true;
			else
				return false;
        }
    }
}
