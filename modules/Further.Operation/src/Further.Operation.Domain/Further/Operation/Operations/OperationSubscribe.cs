using Further.Abp.Operation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Further.Operation.Operations
{
    public class OperationSubscribe : IRedisSubscribe, ITransientDependency
    {
        private readonly SaveOperationProvider saveOperationProvider;

        public OperationSubscribe(
            SaveOperationProvider saveOperationProvider)
        {
            this.saveOperationProvider = saveOperationProvider;
        }

        public async Task SubscribeAsync(string channel, string value)
        {
            if (value.ToString() != "expired") return;

            if (!channel.ToString().Contains("OperationInfo")) return;

            var startIndex = channel.ToString().IndexOf("Operation:");

            if (startIndex == -1) return;

            var key = channel.ToString().Substring(startIndex);

            await saveOperationProvider.SaveExpiredCacheOperation(key);
        }
    }
}
