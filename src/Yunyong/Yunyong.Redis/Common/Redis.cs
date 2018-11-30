using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using Yunyong.Cache;

namespace Yunyong.Redis.Common
{
    internal class Redis
    {
        private static ConnectionMultiplexer _multiRoute = default(ConnectionMultiplexer);
        private static ConfigurationOptions _redisConfig = default(ConfigurationOptions);
        private static string ServerIP { get; set; }
        private static int ServerPort { get; set; }
        private static string ServerPassword { get; set; }
        private static int Db { get; set; }
        private static CacheServiceConfig Config { get; set; }
        private static void InitRedisConfig()
        {
            //
            if (string.IsNullOrWhiteSpace(ServerIP))
            {
                ServerIP = Config.ConnectionString;
            }
            if (ServerPort == 0)
            {
                ServerPort = Config.Port;
            }
            if (string.IsNullOrWhiteSpace(ServerPassword))
            {
                ServerPassword = Config.Password;
            }
            if (Db == 0)
            {
                Db = Config.DB;
            }
            if(string.IsNullOrWhiteSpace(Main))
            {
                Main = Config.Module;
            }
            if(string.IsNullOrWhiteSpace(Sub))
            {
                Sub = Config.Function;
            }
            if (DefaultValidTimeSpan == 0)
            {
                DefaultValidTimeSpan = 24 * 60;
            }

            //
            _redisConfig = new ConfigurationOptions
            {
                EndPoints =
                {
                    {ServerIP,ServerPort}
                }
            };

            //
            _redisConfig.Password = ServerPassword;

            //
            _redisConfig.AbortOnConnectFail = true;
            _redisConfig.AllowAdmin = false;
            _redisConfig.ConnectRetry = 3;
            _redisConfig.ConnectTimeout = 500;
            _redisConfig.DefaultDatabase = 0;
            _redisConfig.KeepAlive = 1;
            _redisConfig.SyncTimeout = 5000;
        }
        private static void CheckMultiRoute()
        {
            if (_redisConfig == null)
            {
                InitRedisConfig();
            }
            if (_multiRoute == null)
            {
                _multiRoute = ConnectionMultiplexer.Connect(_redisConfig);
            }
        }
        public static int DefaultValidTimeSpan { get; set; }
        internal static IDatabase DB()
        {
            CheckMultiRoute();
            return _multiRoute.GetDatabase(Db);
        }
        internal static string Main { get; set; }
        internal static string Sub { get; set; }
        private Redis() { }
    }
}
