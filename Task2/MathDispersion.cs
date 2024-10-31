using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc_PI
{
    public class MathDispersion
    {
        public static double CalcTheorDispersion(double a, double b)
        {
            return Math.Pow((b-a),2)/ 12;
        }
        public static double CalcDispersionSequence(double[] seq)
        {
            return (MathExpectation.CalcExpectationSequncesSqr(seq) -
                Math.Pow(MathExpectation.CalcExpectationSequnces(seq), 2))
                * seq.Length / (seq.Length-1) ;
        }
    }
}
