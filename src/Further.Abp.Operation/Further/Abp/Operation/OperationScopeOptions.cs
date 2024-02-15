using System;
using System.Collections.Generic;
using System.Text;

namespace Further.Abp.Operation
{
    public class OperationScopeOptions
    {
        public bool SaveToCache { get; set; } = false;

        public bool EnableLogger { get; set; } = false;
    }
}
