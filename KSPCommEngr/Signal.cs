#region license

/* The MIT License (MIT)

 * Copyright (c) 2015 Skoth

 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KSPCommEngr
{
    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class Complex : MonoBehaviour
    {
        public void Awake()
        {
            CommEngrLog.Log("Complex Awake() called");
        }

        public void Start()
        {
            CommEngrLog.Log("Complex Start() called");
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
        public float[] x { get; set; }
        private float[] t;
        private Func<float, float> x_t;
        public int Size { get; set; }

        public Signal() : this(x => x, 0f, 127f, 1f) { }
        public Signal(Func<float, float> expression) : this(expression, 0f, 127f, 1f) { }
        public Signal(Func<float, float> expression, float start, float stop) : this(expression, start, stop, 1f) { }
        public Signal(Func<float, float> expression, float start, float end, float step)
        {
            x_t = expression;
            Size = (int)Mathf.Floor((end - start) / step + 1f);
            t = Enumerable.Range(1, Size).Select(i => start + (i - 1)*step).ToArray<float>();
            x = t.Select(x_t).ToArray<float>();
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
                    CommEngrLog.Log(strSignal);
                    strSignal = String.Empty;
                }
                strSignal += "(" + i.ToString() + ":" + x[i].ToString() + "), ";
            }
            strSignal += "(" + i + ":" + x[i].ToString() + ") }";

            CommEngrLog.Log(strSignal);
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
