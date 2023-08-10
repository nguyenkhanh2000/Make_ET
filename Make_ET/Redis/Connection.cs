using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Make_ET.Redis
{
    public class Connection
    {
        private static ConnectionMultiplexer _redisConnection;
        public static void ConnectionRedis()
        {
            //Create a configuration obj
            ConfigurationOptions config = new ConfigurationOptions
            {
                EndPoints = { "localhost:6379" },
                Password = "123456",
            };
            //Create a connection to redis
            _redisConnection = ConnectionMultiplexer.Connect(config);

            // Get a reference to the Redis database
            //IDatabase db = _redisConnection.GetDatabase();
        }
        public static IDatabase GetRedisDatabase()
        {
            return _redisConnection.GetDatabase();
        }
        public static void RedisClose()
        {
            _redisConnection.Close();
        }

    }
}
