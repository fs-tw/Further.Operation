using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace Further.Abp.Operation
{
    public class RedisConnectorHelper : ISingletonDependency
    {
        private readonly OperationOptions options;
        private readonly ILogger<RedisConnectorHelper> logger;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IConfiguration configuration;

        public static ConnectionMultiplexer Connection { get; private set; }

        public RedisConnectorHelper(
            ILogger<RedisConnectorHelper> logger,
            IServiceScopeFactory serviceScopeFactory,
            IOptions<OperationOptions> options,
            IConfiguration configuration)
        {
            this.options = options.Value;
            this.logger = logger;
            this.serviceScopeFactory = serviceScopeFactory;
            this.configuration = configuration;
        }

        public void Initialize()
        {
            var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"] + ",allowAdmin=true");
            Connection = redis;
        }

        public void Subscribe()
        {
            if (!options.IsEnableSubscribe) return;

            var subscriber = Connection.GetSubscriber();

            Connection.GetServer(Connection.GetEndPoints().Single())
                .ConfigSet("notify-keyspace-events", "KEA");

            subscriber.Subscribe("__keyevent@0__:expired", async (channel, value) =>
            {
                foreach (var method in options.ExpiredEventHandlers.GetHandlers())
                {
                    if (!value.ToString().Contains($"c:{method.CacheItem},k")) continue;

                    try
                    {
                        using (var scope = serviceScopeFactory.CreateScope())
                        {
                            var subscribeMethod = scope.ServiceProvider.GetRequiredService(method.ExpiredEventHandler) as IExpiredEventHandler;

                            if (subscribeMethod == null) continue;

                            await subscribeMethod!.HandlerAsync(value.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Subscribe Error");
                    }
                }
            });
        }

        public ConnectionMultiplexer GetConntction()
        {
            return Connection;
        }

        public IDatabase GetDatabase()
        {
            return Connection.GetDatabase();
        }
    }
}
