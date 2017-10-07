using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Helpers.IHelpers;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Common.Helpers
{
    public class RedisHelper : ICacheHelper
    {
        private IApplicationConfig _applicationConfig = null;
        private ILogger _logger = null;


        public RedisHelper(IApplicationConfig applicationConfig, ILogger logger)
        {
            this._applicationConfig = applicationConfig;
            this._logger = logger;
        }

        public IDatabase GetConnection()
        {
            var serverName = _applicationConfig.RedisServerName;
            var connection = ConnectionMultiplexer.Connect($"{serverName},allowAdmin=true");

            var server = connection.GetServer(serverName);
            var database = connection.GetDatabase();

            return database;
        }

        public T GetValue<T>(string key)
        {
            try
            {
                var database = GetConnection();
                var data = database.StringGet(key);

                if (!data.IsNull)
                {
                    var result = JsonConvert.DeserializeObject<T>(data);
                    return result;
                }

                return default(T);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to start get value from Redis", ex);
                return default(T);
            }
        }

        public bool SetValue<T>(string key, T value)
        {
            try
            {
                var database = GetConnection();
                var valueToCache = JsonConvert.SerializeObject(value, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                var result = database.StringSet(key, JsonConvert.SerializeObject(valueToCache));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to start set value in Redis", ex);
                return false;
            }
        }
    }
}
