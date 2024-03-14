using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Further.Abp.Operation
{
    public class OperationOptions
    {
        public TimeSpan DefaultSlidingExpiration { get; set; } = TimeSpan.FromSeconds(5);
    }
}
