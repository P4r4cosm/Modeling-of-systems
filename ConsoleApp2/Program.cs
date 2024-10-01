using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ConsoleApp2
{
    public class Program
    {
        static int n = 0;
        static int[] pow = { 10000, 100000 , 1000000 , 10000000 , 100000000 };
        
        static int m = 0;
        static Random random = new Random();
        static double p;
        static double x, y;
        static double[][] CalcPi(int[] n, int count_seria,double x0, double y0, double r0)
        {
            double xmin = x0 - r0;
            double xmax = x0 + r0;
            double ymin = y0 - r0;
            double ymax = y0 + r0;
            double[][]result= new double[count_seria][];
            for (int i=0; i<result.Length; i++)
            {
                result[i]=new double[n.Length];
                for (int j=0; j<n.Length; j++)
                {
                    m = 0;
                    for (int k = 0; k < n[j]; k++)
                    {
                        p = random.NextDouble();
                        x = (xmax - xmin) * p + xmin;

                        p = random.NextDouble();
                        y = (ymax - ymin) * p + ymin;
                        if (Math.Pow((x - x0), 2) + Math.Pow((y - y0), 2) < Math.Pow(r0, 2))
                        {
                            m++;
                        }
                    }
                    result[i][j] = (double)m / n[j] * 4;
                }
            }
            return result;
        }
        
        static double[][] CalcIntegral(int[] n, double a, double b,int countserias)
        {
            double xmin = a;
            double xmax = b;
            double ymin = 0;
            double ymax = (int)Math.Pow(b, 3) + 1;
            double[][]result= new double[countserias][];

            for (int i = 0; i<countserias;i++)
            {
                result[i] = new double[n.Length];
           
                for (int j = 0; j < result[i].Length;j++)
                {
                    m = 0;
                    for (int k = 0; k < n[j]; k++)
                    {
                        p = random.NextDouble();
                        x = (xmax - xmin) * p + xmin;

                        p = random.NextDouble();
                        y = (ymax - ymin) * p + ymin;

                        if (Math.Pow(x, 3) + 1 > y)
                        {
                            m++;
                        }
                    }
                    result[i][j] = (double)m / n[j] * (b - a) * (Math.Pow(b, 3) + 1);
                }
                
            }
            return result;
        }
        static double[] CalcEps_S_e (double[][] serias, double value)
        {
            double[] S_Eps = new double[serias[0].Length];
            double s = 0;
            for (int i = 0; i < serias[0].Length; i++)
            {
                for (int j=0; j < serias.Length; j++)
                {
                    s += serias[j][i];
                   
                }
                s = s / serias.Length;
                S_Eps[i] = Math.Abs((s-value)/value);
                s = 0;
            }
            return S_Eps;
        }
        static void PrintArray(double[] array, string s)
        {
            n++;
            Console.Write($"{s}{n}: ");
            for (int i = 0;i<array.Length;i++) 
            {
                Console.Write(array[i]+"  ");
            }
            Console.WriteLine();
        }
        static double[][] CalcEps(double[][] array, double value)
        {
            double[][] eps=new double[array.Length][];
            for (int i = 0; i < array.Length; i++)
            {
                eps[i]= new double[array[i].Length];
                for (int j=0; j < array[i].Length; j++)
                {
                    eps[i][j] = Math.Abs((array[i][j] -  value) / value);
                }
            }
            return eps;
        }
        static void Task1_3()
        {
            Console.Write("Введите координаты x0, y0 центра окружности через пробел: ");
            double x0, y0;
            double[] numbers = Console.ReadLine().Split(' ').Select(double.Parse).ToArray();
            x0 = numbers[0];
            y0 = numbers[1];
            Console.WriteLine();
            Console.Write("Введите радиус r0: ");

            double r0=double.Parse(Console.ReadLine());

            double[][] massive = CalcPi(pow, 5,x0,y0,r0);
            for (int i = 0; i < massive.Length; i++)
            {
                PrintArray(massive[i], "Seria");
            }
            Console.WriteLine();
            double[][] Eps = CalcEps(massive, Math.PI);
            n = 0;
            for (int i = 0; i < Eps.Length; i++)
            {
                PrintArray(Eps[i], "Eps");
            }
            n = 0;
            Console.WriteLine();
            double[] arr = CalcEps_S_e(massive, Math.PI);
            for (int i = 0; i < arr.Length; i++)
            {
                Console.WriteLine($"Eps_S_e{4 + i}: {arr[i]}");
            }
        }
        static void Task4()
        {
            double a, b;
            Console.Write("Введите предел интегрирования a,b через пробел: ");
            double[] numbers = Console.ReadLine().Split(' ').Select(double.Parse).ToArray();
            a = numbers[0];
            b = numbers[1];
            double[][] massive = CalcIntegral(pow[..^1], a, b, 3);
            n = 0;
            for (int i = 0; i < massive.Length; i++)
            {
                PrintArray(massive[i], "Seria");
            }
            double[][] Eps = CalcEps(massive, 6);
            n = 0;
            for (int i = 0; i < Eps.Length; i++)
            {
                PrintArray(Eps[i], "Eps");
            }
            n = 0;
            Console.WriteLine();
            double[] arr = CalcEps_S_e(massive, 6);
            for (int i = 0; i < arr.Length; i++)
            {
                Console.WriteLine($"Eps_S_e{4 + i}: {arr[i]}");
            }
        }
        static void Main()
        {
            Task1_3();
            Task4();
        }

    }
}