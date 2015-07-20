using System;
using System.Collections.Generic;
using System.Linq;

namespace KSPCommSys
{
    public class Complex
    {
        public double a { get; set; }
        public double b { get; set; }
        public double Magnitude {
            get {
                return Math.Sqrt(a*a + b*b);
            }
        }
        public double Phase
        {
            get
            {
                // If non-zero, compute value, otherwise throw non-real exception
                if (!(Math.Abs(b) < 0.001))
                    return Math.Atan2(a, b);
                else
                    throw new Exception();
            }
        }

    }

    public class Signal
    {
        private List<double> x;
        private List<double> t;
        public Signal() : this(x => x, 0d, 99d, 1d) { }
        public Signal(Func<double, double> f_t, double start, double end, double step)
        {
            t = Enumerable.Range((int)start, (int)end).Select(f => (double)f).ToList<double>();
            x = t.Select(f_t).ToList<double>();
        }

        private Signal FFT() {


            return new Signal();
        }
    }
}
