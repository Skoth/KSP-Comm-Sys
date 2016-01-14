#region license

/* The MIT License (MIT)

 * Copyright (c) 2016 Skoth

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
using System.Linq.Expressions;
using UnityEngine;

namespace KSPCommEngr
{
    public class Signal : CollectionBase, IEnumerable, IQueryable
    {
        // Discrete time representation
        private Complex[] x
        {
            get { return x; }
            set
            {
                x = value;
                X = FFT(x);
            }
        }

        // Discrete frequency representation
        private Complex[] X
        {
            get { return X; }
            set
            {
                X = value;
                x = IFFT(X);
            }
        }

        public Texture2D plot;

        public int Length
        {
            get
            {
                return x.Length;
            }
        }

        public Expression Expression
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Type ElementType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IQueryProvider Provider
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < x.Length; ++i)
            {
                yield return x[i];
            }
        }

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



        public Signal(Complex[] v)
        {
            x = v;
            plot = new Texture2D(v.Length, v.Length);
        }

        public Signal(Func<float, Complex> expression = null, int Size = 256)
        {
            float step = 1 / (float)Size,
            N_1 = (float)(Size - 1);
            int i = 0;
            for (float f = 0f; f <= N_1; f += step)
            {
                x[i] = expression(f);
                ++i;
            }
            plot = new Texture2D(x.Length, x.Length);
        }

        public static Signal operator +(Signal u, Signal v)
        {
            if (u.Count != v.Count) return null;
            Signal w = new Signal();
            for (int i = 0; i < w.Count; ++i)
            {
                w[i] = u[i] + v[i];
            }
            return w;
        }

        public static Signal operator -(Signal u, Signal v)
        {
            if (u.Count != v.Count) return null;
            Signal w = new Signal();
            for (int i = 0; i < w.Count; ++i)
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
        public static Complex[] operator %(Signal u, Signal v)
        {
            Complex[] U = FFT(u.x);
            Complex[] V = FFT(v.x);
            var multQuery = from i in U
                          from j in V
                          select i * j;
            Complex[] W = multQuery.ToArray();
            return IFFT(W);
        }

        // TODO: Way to define Signal as Complex collection?
        private static Complex[] FFT(Complex[] v)
        {
            int N = v.Length;
            if (N == 1) return v;
            Complex[] E = FFT(v.Where((element, i) => i % 2 == 0).ToArray());
            Complex[] O = FFT(v.Where((element, i) => i % 2 != 0).ToArray());
            Complex[] T = E.Where((element, i) => i == i % N / 2).ToArray();
            Complex[] U = O.Select((element, i) =>
                {
                    float a = Mathf.Cos(-i * 2f * Mathf.PI / N);
                    float b = Mathf.Sin(-i * 2f * Mathf.PI / N);
                    return (i == i % N / 2) ? element * new Complex(a, b) : element;
                }).Where((element, i) => i == i % N / 2).ToArray();
            Complex[] R = new Complex[Math.Min(U.Length, T.Length)];
            for (int k = 0; k < R.Length; ++k)
            {
                R[k] = T[k] + U[k];
            }
            return R;
        }

        private static Complex[] IFFT(Complex[] V)
        {
            return new Complex[] { };
        }

        public static Signal Transform(Func<Complex, Complex> expression)
        {
            return new Signal(new Complex[]{ });
        }
    }
}
