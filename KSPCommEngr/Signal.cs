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
    // TODO: size mapping from Texture2D to normalized numeric scale (necessary?)
    public class Signal
    {
        // Complex-valued 1D array [{(Re), (Im)}, {(Re), (Im)}, ... ]
        public float[] x;

        // Parameter that x is a function of
        private float[] t;
        
        // Expression that defines x (if functionally definable)
        private Func<float, float> x_t;

        public Texture2D plot;
        public int Size = 128;

        public Signal(Func<float, float> expression = null, bool real = true)
        {

            t = Enumerable.Range(0, Size).ToArray().Select(x => (float)x).ToArray();
            x = Enumerable.Repeat(0f, Size).ToArray();
            plot = new Texture2D(Size, Size);
            if(expression == null)
            {
                x_t = (x) => 1;
            }
            else if(real)
            {
                for(int i = 0; i < t.Length; ++i)
                {
                    if (i % 2 == 0)
                        x[i] = expression(t[i]);
                    else
                        x[i] = 0;
                }
            }
            else
            {
                for (int i = 0; i < t.Length; ++i)
                {
                    x[i] = expression(t[i]);
                }
            }
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

        // TODO fix operator overloading implementation
        public static Signal operator +(Signal s1, Signal s2)
        {
            return new Signal();
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
                    CommEngrUtils.Log(strSignal);
                    strSignal = String.Empty;
                }
                strSignal += "(" + i.ToString() + ":" + x[i].ToString() + "), ";
            }
            strSignal += "(" + i + ":" + x[i].ToString() + ") }";

            CommEngrUtils.Log(strSignal);
        }

        private Signal FFT()
        {
            // TODO: implement basic FFT test
            return new Signal();
        }
    }
}
