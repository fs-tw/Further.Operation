using System;
using System.Collections.Generic;
using System.Text;

namespace Further.Abp.Operation
{
    public class ExpiredEventHandlerConfigurations
    {
        private readonly List<ExpiredEventHandlerConfiguration> handlers = new();

        public ExpiredEventHandlerConfigurations Configure<TCacheItem, TExpiredEventHandler>()
            where TExpiredEventHandler : IExpiredEventHandler
        {
            handlers.Add(new ExpiredEventHandlerConfiguration
            {
                CacheItem = typeof(TCacheItem).FullName,
                ExpiredEventHandler = typeof(TExpiredEventHandler)
            });

            return this;
        }

        public List<ExpiredEventHandlerConfiguration> GetHandlers()
        {
            return handlers;
        }
    }
}
