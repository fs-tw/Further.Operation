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

        //暫不開放Set功能
        //void SetCurrentId(Guid id);
    }
}
