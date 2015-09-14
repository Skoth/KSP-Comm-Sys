using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPCommEngr
{
    public static class Channel
    {
        public static void AWGN(this Signal x)
        {
            for (int i = 0; i < x.x.Length; ++i)
            {
                x[i].Real = x[i].Real + UnityEngine.Random.value;
            }
        }
    }
}
