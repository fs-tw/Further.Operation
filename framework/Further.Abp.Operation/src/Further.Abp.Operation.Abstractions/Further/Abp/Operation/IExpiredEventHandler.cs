using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Further.Abp.Operation
{
    public interface IExpiredEventHandler
    {
        /// <summary>
        /// Redis過期事件監聽
        /// </summary>
        /// <param name="value">過期事件的key</param>
        /// <returns></returns>
        Task HandlerAsync(string value);
    }
}
