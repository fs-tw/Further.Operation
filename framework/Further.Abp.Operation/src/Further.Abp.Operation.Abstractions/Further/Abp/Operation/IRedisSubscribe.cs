using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Further.Abp.Operation
{
    public interface IRedisSubscribe
    {
        /// <summary>
        /// Redis事件訂閱
        /// </summary>
        /// <param name="channel">發出事件的key</param>
        /// <param name="value">事件類型</param>
        /// <returns></returns>
        Task SubscribeAsync(string channel, string value);
    }
}
