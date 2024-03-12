using System;
using System.Collections.Generic;
using System.Text;

namespace Further.Abp.Operation
{
    public class OperationOptions
    {
        public bool IsEnableSubscribe { get; set; } = true;
        public TimeSpan DefaultExpiry { get; set; } = TimeSpan.FromSeconds(5);

        public TimeSpan DefaultWait { get; set; } = TimeSpan.FromSeconds(2);

        public TimeSpan DefaultRetry { get; set; } = TimeSpan.FromMilliseconds(200);

        public TimeSpan MaxSurvivalTime { get; set; } = TimeSpan.FromSeconds(5);

        public ExpiredEventHandlerConfigurations ExpiredEventHandlers { get; } = new();
    }
}
