using System;
using System.Collections.Generic;
using System.Text;

namespace Further.Abp.Operation
{
    public class OperationOptions
    {
        public string Configuration { get; set; } = "127.0.0.1";
        public TimeSpan DefaultExpiry { get; set; } = TimeSpan.FromSeconds(5);

        public TimeSpan DefaultWait { get; set; } = TimeSpan.FromSeconds(2);

        public TimeSpan DefaultRetry { get; set; } = TimeSpan.FromMilliseconds(200);

        public TimeSpan MaxSurvivalTime { get; set; } = TimeSpan.FromSeconds(5);

        public List<Type> Subscribes { get; } = new();
    }
}
