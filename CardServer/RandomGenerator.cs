using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardServer
{
    class RandomGenerator
    {
        public static bool IsHit(double percent)
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            return rnd.NextDouble() < percent;
        }
    }
}
