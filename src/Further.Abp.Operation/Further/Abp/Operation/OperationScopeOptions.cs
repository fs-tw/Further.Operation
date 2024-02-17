using System;
using System.Collections.Generic;
using System.Text;

namespace Further.Abp.Operation
{
    public class OperationScopeOptions
    {
        public bool SaveToCache { get; set; } = false;

        public bool EnabledLogger { get; set; } = false;

        /// <summary>
        /// 最大保存時間，單位是分
        /// </summary>
        public int MaxSurvivalTime { get; set; } = 5;
    }
}
