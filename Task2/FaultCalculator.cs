using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc_PI
{
    public class FaultCalculator
    {
        public static double Calculate(double theorValue, double value)
        {
            return Math.Abs((theorValue - value)/theorValue)*100;
        }
    }
}
