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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityEngine;
using KSPCommEngr;

namespace CommEngrTest
{
    /*
        See: https://msdn.microsoft.com/en-us/library/dd264975.aspx
    */

    [TestClass]
    public class ComplexTests
    {
        private Complex u;
        private Complex v;

        [TestInitialize]
        public void Setup()
        {
            u = new Complex(4f, 3f);
            v = new Complex(6f, 8f);
        }

        [TestMethod]
        public void Addition()
        {
            Complex t = u + v;
            Complex r = new Complex(u.Real + v.Real, u.Imag + v.Imag);
            Assert.AreEqual(t.Real, r.Real);
            Assert.AreEqual(t.Imag, r.Imag);
        }

        [TestMethod]
        public void Subtraction()
        {
            Complex t = u - v;
            Complex r = new Complex(u.Real - v.Real, u.Imag - v.Imag);
            Assert.AreEqual(t.Real, r.Real);
            Assert.AreEqual(t.Imag, r.Imag);
        }

        [TestMethod]
        public void Multiplication()
        {
            // Complex
            Complex t = u * v;
            Complex r = new Complex(u.Real * v.Real - u.Imag * v.Imag, u.Real * v.Imag + u.Imag * v.Real);
            Assert.AreEqual(t.Real, r.Real);
            Assert.AreEqual(t.Imag, r.Imag);

            // Scalar
            t = 10f * u;
            r = new Complex(10f * u.Real, 10f * u.Imag);
            Assert.AreEqual(t.Real, r.Real);
            Assert.AreEqual(t.Imag, r.Imag);
        }

        [TestMethod]
        public void Division()
        {
            Complex t = u / v;
            Complex r = new Complex((u.Real * v.Real + u.Imag * v.Imag) / (v.Real * v.Real + v.Imag * v.Imag),
                (u.Imag * v.Real - u.Real * v.Imag) / (v.Real * v.Real + v.Imag * v.Imag));
            Assert.AreEqual(t.Real, r.Real);
            Assert.AreEqual(t.Imag, r.Imag);
        }
    }
}
