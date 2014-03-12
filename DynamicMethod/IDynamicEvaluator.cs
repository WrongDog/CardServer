using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMethod
{
    public interface IDynamicEvaluator
    {
        object Evaluate(dynamic obj);
    }
}
