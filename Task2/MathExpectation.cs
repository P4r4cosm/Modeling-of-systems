using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc_PI
{
    public class MathExpectation
    {
        public static double CalcTheorExpectation(double a, double b)
        {
            return (a + b) / 2;
        }

        public static double CalcExpectationSequnces(double[] seq)
        {
            return seq.Sum() / seq.Length;

        }
        public static double CalcExpectationSequncesSqr(double[] seq)
        {
            return seq.Sum(x=>x*x)/seq.Length;
        }
    }
}
