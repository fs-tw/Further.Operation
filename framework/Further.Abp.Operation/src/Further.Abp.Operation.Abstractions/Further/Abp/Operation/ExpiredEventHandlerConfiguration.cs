using System;
using System.Collections.Generic;
using System.Text;

namespace Further.Abp.Operation
{
    public class ExpiredEventHandlerConfiguration
    {
        public string CacheItem { get; set; } = null!;

        public Type ExpiredEventHandler { get; set; } = null!;
    }
}
