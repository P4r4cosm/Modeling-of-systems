using Calc_PI;
using Task2;
public class Program
{
    static void Foo(double[] array_1, double[] array_2, double[] array_3, double[] array_4, string HistName)
    {
        //Console.WriteLine("array_1      array_2         array_3         array_4");
        //for (int i = 0, j = 0, z = 0, d = 0; i < array_4.Length; i++)
        //{
        //    Console.WriteLine($"{array_1[j]}   {array_2[z]}   {array_3[d]}   {array_4[i]}");
        //    j = j < array_1.Length - 1 ? ++j : j;
        //    z = z < array_2.Length - 1 ? ++z : z;
        //    d = d < array_3.Length - 1 ? ++d : d;
        //}
        Console.WriteLine();
        //Мат. Ожидание: 
        double TheorExpectation = MathExpectation.CalcTheorExpectation(0, 10);
        Console.WriteLine($"Теоретическое мат. ожидание: {TheorExpectation}");
        double array_1Expectation = MathExpectation.CalcExpectationSequnces(array_1);
        double array_2Expectation = MathExpectation.CalcExpectationSequnces(array_2);
        double array_3Expectation = MathExpectation.CalcExpectationSequnces(array_3);
        double array_4Expectation = MathExpectation.CalcExpectationSequnces(array_4);
        Console.WriteLine($"мат. ожидание 1: {array_1Expectation}");
        Console.WriteLine($"мат. ожидание 2: {array_2Expectation}");
        Console.WriteLine($"мат. ожидание 3: {array_3Expectation}");
        Console.WriteLine($"мат. ожидание 4: {array_4Expectation}");
        //Дисперсия: 
        double TheorDispersion = MathDispersion.CalcTheorDispersion(0, 10);
        Console.WriteLine($"Теоретическая дисперсия: {TheorExpectation}");
        double array_1Dispersion = MathDispersion.CalcDispersionSequence(array_1);
        double array_2Dispersion = MathDispersion.CalcDispersionSequence(array_2);
        double array_3Dispersion = MathDispersion.CalcDispersionSequence(array_3);
        double array_4Dispersion = MathDispersion.CalcDispersionSequence(array_4);
        Console.WriteLine($"Дисперсия 1: {array_1Dispersion}");
        Console.WriteLine($"Дисперсия 2: {array_2Dispersion}");
        Console.WriteLine($"Дисперсия 3: {array_3Dispersion}");
        Console.WriteLine($"Дисперсия 4: {array_4Dispersion}");
        Console.WriteLine();
        //Погрешность Ожидания: 
        double faultarray_1Ex = Calculator.Calculate(TheorExpectation, array_1Expectation);
        double faultarray_2Ex = Calculator.Calculate(TheorExpectation, array_2Expectation);
        double faultarray_3Ex = Calculator.Calculate(TheorExpectation, array_3Expectation);
        double faultarray_4Ex = Calculator.Calculate(TheorExpectation, array_4Expectation);
        Console.WriteLine($"Погрешность ожидания 1: {faultarray_1Ex}");
        Console.WriteLine($"Погрешность ожидания 2: {faultarray_2Ex}");
        Console.WriteLine($"Погрешность ожидания 3: {faultarray_3Ex}");
        Console.WriteLine($"Погрешность ожидания 4: {faultarray_4Ex}");
        //Погрешность Дисперсии: 
        double faultarray_1Dis = Calculator.Calculate(TheorDispersion, array_1Dispersion);
        double faultarray_2Dis = Calculator.Calculate(TheorDispersion, array_2Dispersion);
        double faultarray_3Dis = Calculator.Calculate(TheorDispersion, array_3Dispersion);
        double faultarray_4Dis = Calculator.Calculate(TheorDispersion, array_4Dispersion);
        Console.WriteLine($"Погрешность дисперсии 1: {faultarray_1Dis}");
        Console.WriteLine($"Погрешность дисперсии 2: {faultarray_2Dis}");
        Console.WriteLine($"Погрешность дисперсии 3: {faultarray_3Dis}");
        Console.WriteLine($"Погрешность дисперсии 4: {faultarray_4Dis}");
        Console.WriteLine();

        //Период
        Console.WriteLine("array_1: ");
        Calculator.Printer(Calculator.CalcPeriod(array_1));
        Console.WriteLine("array_2: ");
        Calculator.Printer(Calculator.CalcPeriod(array_2));
        Console.WriteLine("array_3: ");
        Calculator.Printer(Calculator.CalcPeriod(array_3));
        Console.WriteLine("array_4: ");
        Calculator.Printer(Calculator.CalcPeriod(array_4));
        Console.WriteLine();
        //Частота:
        var freq1 = Calculator.GetFreqDist(array_1, 0, 10, 10);
        var freq2 = Calculator.GetFreqDist(array_2, 0, 10, 10);
        var freq3 = Calculator.GetFreqDist(array_3, 0, 10, 10);
        var freq4 = Calculator.GetFreqDist(array_4, 0, 10, 10);
        Calculator.Printer(freq1, "относительная частота случайных чисел 1:");
        Calculator.Printer(freq2, "относительная частота случайных чисел 2:");
        Calculator.Printer(freq3, "относительная частота случайных чисел 3:");
        Calculator.Printer(freq4, "относительная частота случайных чисел 4:");
        //Рисуем гистограммы
        HistogrammDrawler.Draw(freq1, $"{HistName}1");
        HistogrammDrawler.Draw(freq2, $"{HistName}2");
        HistogrammDrawler.Draw(freq3, $"{HistName}3");
        HistogrammDrawler.Draw(freq4, $"{HistName}4");
        Console.WriteLine();
        //Пирсон
        Console.WriteLine("Значение критерия пирсона 1: "+Calculator.CalcPirson(freq1));
        Console.WriteLine("Значение критерия пирсона 2: " + Calculator.CalcPirson(freq2));
        Console.WriteLine("Значение критерия пирсона 3: " + Calculator.CalcPirson(freq3));
        Console.WriteLine("Значение критерия пирсона 4: " + Calculator.CalcPirson(freq4));
    }
    static void Main()
    {
        //RandomNumberGenerator
        var rand = new RandomNumberGenerator(22695477, 1, (long)Math.Pow(2, 32), 1);
        int a = 10;
        double[] array_1 = rand.GenerateArray(size: a *= 10, seed: 1, min: 0, max: 10);
        double[] array_2 = rand.GenerateArray(size: a *= 10, seed: 1, min: 0, max: 10);
        double[] array_3 = rand.GenerateArray(size: a *= 10, seed: 1, min: 0, max: 10);
        double[] array_4 = rand.GenerateArray(size: a *= 10, seed: 1, min: 0, max: 10);
        Foo(array_1,array_2, array_3, array_4, "RandomNumberGeneratorHist");
        //Random 
        var random=new Random();
        array_1 = new double[100]; for (int i = 0; i < array_1.Length; i++)  array_1[i] = random.NextDouble() * 10;
        array_2 = new double[1000]; for (int i = 0; i < array_2.Length; i++) array_2[i] = random.NextDouble() * 10;
        array_3 = new double[10000]; for (int i = 0; i < array_3.Length; i++) array_3[i] = random.NextDouble() * 10;
        array_4 = new double[100000]; for (int i = 0; i < array_4.Length; i++) array_4[i] = random.NextDouble() * 10;
        Foo(array_1, array_2, array_3, array_4, "RandomC#Hist");
    }
}