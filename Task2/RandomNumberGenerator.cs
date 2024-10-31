using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc_PI
{
    class RandomNumberGenerator
    {
        private long A { get; set; }
        private long B { get; set; }
        private long M { get; set; }
        private long Seed { get; set; }
        public RandomNumberGenerator(long a, long b, long m, long seed)
        {
            A = a;
            B = b; 
            M = m;
            Seed = seed;
        }
        public long Next()
        {
            Seed = (A * Seed + B) % M;
            return Seed;
        }
        public double NextDouble()
        {
            return (double)Next() / M;
        }
        public double NextDouble(double min, double max)
        {
            return min + (NextDouble() * (max - min));
        }

        public double[] GenerateArray(int size, long seed, double min, double max)
        {
            double[] array = new double[size];
            Seed = seed;
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = NextDouble(min, max);
            }
            return array;

        }
    }
}
