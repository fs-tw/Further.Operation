using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Further.Operation
{
    public class OperationOptions
    {
        public TimeSpan DefaultSlidingExpiration { get; set; } = TimeSpan.FromSeconds(5);

        public List<Type> ExpiredProviders { get; } = new List<Type>();
    }
}
