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
        public Complex[] x;
        public Texture2D plot;

        public Signal(Complex[] u = null, Func<float, float> expression = null, bool real = true)
        {
            if(u != null)
            {
                x = u;
            }
            else
            {
                if (expression == null)
                {
                    expression = i => 0f;
                }
                else if (real)
                {
                    for (int i = 0; i < x.Length; ++i)
                    {
                        if (i % 2 == 0)
                            x[i].Real = expression(i);
                        else
                            x[i].Imag = 0f;
                    }
                }
                else
                {
                    for (int i = 0; i < x.Length; ++i)
                    {
                        x[i].Real = expression(i);
                        x[i].Imag = expression(i);
                    }
                }
                plot = new Texture2D(x.Length, x.Length);
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
            Signal w = new Signal();
            for(int i = 0; i < w.x.Length; ++i)
            {
                w[i] = u[i] + v[i];
            }
            return w;
        }

        public static Signal operator -(Signal u, Signal v)
        {
            Signal w = new Signal();
            for (int i = 0; i < w.x.Length; ++i)
            {
                w[i] = u[i] - v[i];
            }
            return w;
        }

        public static Signal operator *(Signal u, Signal v)
        {
            Signal w = new Signal();
            for (int i = 0; i < w.x.Length; ++i)
            {
                w[i] = u[i] * v[i];
            }
            return w;
        }

        // TODO: Fix FFT float[] <---> Complex[] conversions
        // Convolution operator
        //public static Signal operator %(Signal u, Signal v)
        //{
        //    Signal U = FFT(u);
        //    Signal V = FFT(v);
        //    Signal W = U * V;
        //    return IFFT(W);
        //}

        //private static Signal FFT(Signal x)
        //{
        //    float[] real = new float[x.x.Length / 2], 
        //        imag = new float[x.x.Length / 2];

        //    for(int i = 0; i < x.x.Length; ++i)
        //    {
        //        if (i % 2 == 0)
        //            real[i / 2] = x[i];
        //        else
        //            imag[i / 2] = x[i];
        //    }

        //    Signal realFFT = FFT(new Signal(real));
        //    Signal imagFFT = FFT(new Signal(imag));

        //    float Wn = 2 * Mathf.PI / x.Size;
        //    Signal X = new Signal();
        //    for (int i = 0; i < x.Size / 2; ++i)
        //    {

        //    }

        //    return new Signal();
        //}

        //private static Signal IFFT(Signal x)
        //{
        //    return new Signal();
        //}
    }
}
