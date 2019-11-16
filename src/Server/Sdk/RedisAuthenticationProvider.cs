namespace System.Net.Mqtt.Sdk
{
    internal class RedisAuthenticationProvider : IMqttAuthenticationProvider
    {
        static readonly Lazy<RedisAuthenticationProvider> instance;

        static RedisAuthenticationProvider()
        {
            instance = new Lazy<RedisAuthenticationProvider>(() => new RedisAuthenticationProvider());
        }

        RedisAuthenticationProvider()
        {
        }

        public static IMqttAuthenticationProvider Instance { get { return instance.Value; } }

        public bool Authenticate(string clientId, string username, string password)
        {
			/** Check if clientid key is on REDIS and then check the result with username@password 
			 *  that will be nickname@token
			 */
            return true;
        }
    }
}
