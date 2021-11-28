using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab7_3
{
    class ValuePair<TFirst, TSecond>
    {
        public TFirst First { set; get; }
        public TSecond Second { set; get; }
        public ValuePair(TFirst first, TSecond second)
        {
            First = first;
            Second = second;
        }
        public ValuePair()
        {
            First = default;
            Second = default;
        }
    }
}
