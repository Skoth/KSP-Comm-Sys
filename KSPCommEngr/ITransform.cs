using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPCommEngr
{
    interface ITransform<T>
    {
        T TransferFunction(params T[] input);
    }
}
