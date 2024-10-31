using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc_PI
{
    public class Calculator
    {
        public static Dictionary<string, double> CalcPeriod(double[] array)
        {
            int n = array.Length;
            Dictionary<string, double> result = new Dictionary<string, double>()
            {
                 { "period", -1},
                 { "first_pos",-1},
                 { "second_pos", -1}
            };
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (array[i] == array[j])
                    {
                        result["period"] = j - i;
                        result["first_pos"] = i;
                        result["second_pos"] = j;
                        return result;
                    }
                }
            }
            return result;
        }
        public static void Printer(Dictionary<string, double> result)
        {
            foreach (var item in result)
            {
                Console.WriteLine($"{item.Key}  value: {item.Value}");
            }
        }

        public static void Printer(double[] result, string name)
        {
            Console.WriteLine(name);
            foreach (var item in result)
            {
                Console.Write($"{item}\t");
            }
            Console.WriteLine();
        }
        public static double[] GetFreqDist(double[] RParamsArr, double A, double B, int IntervalsCount)
        {
            double dY = (B - A) / IntervalsCount;
            double[] Freq = new double[IntervalsCount];

            // Подсчёт количества значений в каждом интервале
            foreach (var Yc in RParamsArr)
            {
                int intervalIndex = (int)Math.Floor(Yc / dY);

                // Проверка, чтобы индекс не вышел за границы массива
                if (intervalIndex >= 0 && intervalIndex < IntervalsCount)
                {
                    Freq[intervalIndex] += 1;
                }
            }

            // Нормализация частот
            int totalCount = RParamsArr.Length;
            for (int i = 0; i < IntervalsCount; i++)
            {
                Freq[i] /= (totalCount * dY);
            }

            return Freq;
        }

        public static double CalcPirson(double[] array)
        {
            int K = array.Length;
            double result = 0.0;
            double averageExpected = 1.0 / K;

            for (int i = 0; i < K; i++)
            {
                double difference = Math.Pow(averageExpected - array[i],2);
                result += difference / array[i];
            }
            return result;
        }
    }
}

