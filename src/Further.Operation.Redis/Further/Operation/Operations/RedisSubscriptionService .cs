using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace Further.Operation.Operations
{
    public class RedisSubscriptionService:ISingletonDependency
    {
        private readonly IConfiguration configuration;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public RedisSubscriptionService(
            IConfiguration configuration,
            IServiceScopeFactory serviceScopeFactory)
        {
            this.configuration = configuration;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public void Initialize()
        {
            var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"] + ",allowAdmin=true");
            var subscriber = redis.GetSubscriber();

            redis.GetServer(redis.GetEndPoints().Single())
                .ConfigSet("notify-keyspace-events", "KEA");

            subscriber.Subscribe("__keyspace@0__:*", async (channel, value) =>
            {
                if (value.ToString() != "expired") return;

                if (!channel.ToString().Contains("CacheOperationInfo")) return;

                // 使用獨立的作用域處理每個事件
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var key = (channel.ToString().Split(',')[1]).Split(new[] { ':' }, 2)[1];

                    var saveOperationProvider = scope.ServiceProvider.GetRequiredService<SaveOperationProvider>();
                    await saveOperationProvider.SaveExpiredCacheOperation(key);
                }
            });
        }
    }
}
