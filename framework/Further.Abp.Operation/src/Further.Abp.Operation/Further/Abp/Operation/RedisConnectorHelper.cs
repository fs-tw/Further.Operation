using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
            var subscriber = Connection.GetSubscriber();

            Connection.GetServer(Connection.GetEndPoints().Single())
                .ConfigSet("notify-keyspace-events", "KEA");

            subscriber.Subscribe("__keyspace@0__:*", async (channel, value) =>
            {
                foreach (var method in options.Subscribes)
                {
                    try
                    {
                        using (var scope = serviceScopeFactory.CreateScope())
                        {
                            var subscribeMethod = scope.ServiceProvider.GetRequiredService(method) as IRedisSubscribe;

                            if (subscribeMethod == null) continue;

                            await subscribeMethod!.SubscribeAsync(channel.ToString(), value.ToString());
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
