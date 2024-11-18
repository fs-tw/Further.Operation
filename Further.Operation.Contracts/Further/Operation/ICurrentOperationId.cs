using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Further.Operation
{
    public interface ICurrentOperationId
    {
        Guid? GetCurrentId();

        void SetCurrentId(Guid id);
    }
}
