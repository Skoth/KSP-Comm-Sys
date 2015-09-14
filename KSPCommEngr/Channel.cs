using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPCommEngr
{
    public static class Channel
    {
        public static void AWGN(this Signal u)
        {
            for (int i = 0; i < u.Count; ++i)
            {
                u[i].Real = u[i].Real + UnityEngine.Random.value;
            }
        }
    }
}
