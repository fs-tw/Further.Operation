using Further.Abp.Operation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Further.Operation.Operations
{
    public class OperationSaveHandler : IExpiredEventHandler, ITransientDependency
    {
        private readonly SaveOperationProvider saveOperationProvider;

        public OperationSaveHandler(
            SaveOperationProvider saveOperationProvider)
        {
            this.saveOperationProvider = saveOperationProvider;
        }

        public async Task HandlerAsync(string value)
        {
            var startIndex = value.ToString().IndexOf("Operation:");

            if (startIndex == -1) return;

            var key = value.ToString().Substring(startIndex);

            await saveOperationProvider.SaveExpiredCacheOperation(key);
        }
    }
}
