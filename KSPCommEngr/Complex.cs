using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KSPCommEngr
{
    public class Complex
    {
        public float Real { get; set; }
        public float Imag { get; set; }
        
        public Complex(float a = 0f, float b = 0f)
        {
            Real = a;
            Imag = b;
        }

        public static Complex operator +(Complex u, Complex v)
        {
            return new Complex(u.Real + v.Real, u.Imag + v.Imag);
        }

        public static Complex operator -(Complex u, Complex v)
        {
            return new Complex(u.Real - v.Real, u.Imag - v.Imag);
        }

        public static Complex operator *(Complex u, Complex v)
        {
            return new Complex(u.Real * v.Real - u.Imag * v.Imag, u.Real * v.Imag + u.Imag * v.Real);
        }

        public static Complex operator *(float a, Complex v)
        {
            return new Complex(a * v.Real, a * v.Imag);
        }

        public static Complex operator /(Complex u, Complex v)
        {
            return (1 / (v.Real * v.Real + v.Imag * v.Imag)) * (u * Conjugate(v));
        }

        public static bool operator ==(Complex u, Complex v)
        {
            if (System.Object.ReferenceEquals(u, v)) return true;
            if (((object)u == null) || ((object)v == null)) return false;
            return u.Real == v.Real && u.Imag == v.Imag;
        }

        public static bool operator !=(Complex u, Complex v)
        {
            return !(u == v);
        }

        public static Complex Conjugate(Complex z)
        {
            return new Complex(z.Real, -z.Imag);
        }

        public static float Magnitude(Complex z)
        {
            return Mathf.Sqrt(z.Real * z.Real + z.Imag * z.Imag);
        }

        public static float Phase(Complex z)
        {
            return Mathf.Atan(z.Imag / z.Real);
        }
    }
}
