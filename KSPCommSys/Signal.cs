using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KSPCommSys
{
    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class Complex : MonoBehaviour
    {
        public void Awake()
        {
            CommSysLog.Log("Complex Awake() called");
        }

        public void Start()
        {
            CommSysLog.Log("Complex Start() called");
        }

        public float a { get; set; }
        public float b { get; set; }
        public float Magnitude
        {
            get
            {
                return Mathf.Sqrt(a * a + b * b);
            }
        }
        public float Phase
        {
            get
            {
                // If non-zero, compute value, otherwise throw non-real exception
                if (!(Mathf.Abs(a) < 0.001))
                    return Mathf.Atan2(a, b);
                else
                    throw new Exception();
            }
        }

    }

    public class Signal
    {
        private List<float> x;
        private List<float> t;
        private Func<float, float> expression;
        public int Size { get; set; }

        public Signal() : this(x => x, 0f, 99f, 1f) { }
        public Signal(Func<float, float> f_t) : this(f_t, 0f, 99f, 1f) { }
        public Signal(Func<float, float> f_t, float start, float stop) : this(f_t, start, stop, 1f) { }
        public Signal(Func<float, float> f_t, float start, float end, float step)
        {
            expression = f_t;
            t = Enumerable.Range((int)start, (int)end).Select(f => (float)f).ToList<float>();
            x = t.Select(f_t).ToList<float>();
            Size = x.Count;
        }

        // Indexer
        public float this[int i]
        {
            get
            {
                return x[i];
            }
            set
            {
                x[i] = value;
            }
        }

        public void DisplaySignal()
        {
            string strSignal = "{ ";
            int i = 0;
            for (; i < Size - 1; ++i)
            {
                // Temp log resolver
                if (i % 4 == 0)
                {
                    CommSysLog.Log(strSignal);
                    strSignal = String.Empty;
                }
                strSignal += "(" + i.ToString() + ":" + x[i].ToString() + "), ";
            }
            strSignal += "(" + i + ":" + x[i].ToString() + ") }";

            CommSysLog.Log(strSignal);
        }

        private Signal FFT()
        {
            // TODO: implement basic FFT test
            return new Signal();
        }
    }

    public static class Channel
    {
        public static Signal AdditiveNoise(Signal x)
        {
            Signal y = new Signal();

            for (int i = 0; i < x.Size; ++i)
            {
                y[i] = x[i] + UnityEngine.Random.value;
            }
            return y;
        }
    }
}
