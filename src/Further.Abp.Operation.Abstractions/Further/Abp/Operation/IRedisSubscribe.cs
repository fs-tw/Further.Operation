using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Further.Abp.Operation
{
    public interface IRedisSubscribe
    {
        Task SubscribeAsync(string channel, string value);
    }
}
