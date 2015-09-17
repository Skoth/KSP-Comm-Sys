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
    public class Signal : ICollection
    {
        private Complex[] x;
        private float[] x_f;

        public Texture2D plot;

        public int Count
        {
            get
            {
                return x.Length;
            }
        }

        // Real Signal
        public Signal(int Size, Func<float, float> expression)
        {
            float step = 1 / (float)Size,
            N_1 = (float)(Size - 1);
            int i = 0;
            for(float f = 0f; f <= N_1; f += step)
            {
                x[i] = new Complex(expression(f));
                ++i;
            }
            plot = new Texture2D(x.Length, x.Length);
        }

        // Complex Signal
        public Signal(Func<float, float, float> expression)
        {
            plot = new Texture2D(x.Length, x.Length);
        }


        public Signal(Complex[] u = null)
        {
            plot = new Texture2D(x.Length, x.Length);
        }

        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < Count; ++i)
            {
                yield return x[i];
            }
        }

        // Indexer
        public Complex this[int i]
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

        public static Signal operator +(Signal u, Signal v)
        {
            if (u.Count != v.Count) return null;
            Signal w = new Signal();
            for(int i = 0; i < w.x.Length; ++i)
            {
                w[i] = u[i] + v[i];
            }
            return w;
        }

        public static Signal operator -(Signal u, Signal v)
        {
            if (u.Count != v.Count) return null;
            Signal w = new Signal();
            for (int i = 0; i < w.x.Length; ++i)
            {
                w[i] = u[i] - v[i];
            }
            return w;
        }

        public static Signal operator *(Signal u, Signal v)
        {
            if (u.Count != v.Count) return null;
            Signal w = new Signal();
            for (int i = 0; i < u.Count; ++i)
            {
                w[i] = u[i] * v[i];
            }
            return w;
        }
        
        // Convolution operator
        public static Signal operator %(Signal u, Signal v)
        {
            Signal U = FFT(u);
            Signal V = FFT(v);
            Signal W = U * V;
            return IFFT(W);
        }

        private float[] FFT(Signal u)
        {

        }

        public float[] IFFT(Signal x)
        {
            return new Signal();
        }
    }
}
