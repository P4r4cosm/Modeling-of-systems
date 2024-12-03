using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.SkiaSharp;

class Task4
{
    public static double PDF(double x, double mean, double sigma)
    {
        if (x <= 0) return 0;
        double variance = sigma * sigma;
        double denominator = x * Math.Sqrt(2 * Math.PI * variance);
        double exponent = -Math.Pow(Math.Log(x) - mean, 2) / (2 * variance);
        return Math.Exp(exponent) / denominator;
    }

    public static double CDF(double x, double mean, double sigma)
    {
        if (x <= 0) return 0;

        double step = 0.01;
        double sum = 0;

        for (double t = step; t <= x; t += step)
        {
            double y1 = PDF(t - step, mean, sigma);
            double y2 = PDF(t, mean, sigma);
            sum += (y1 + y2) / 2 * step; // Метод трапеций
        }

        return sum;
    }

    private static double[] cdfPrecomputed;
    private static double[] xPrecomputed;

    public static void PrecomputeCDF(double mean, double sigma, double xLimitation, int intervals = 100)
    {
        cdfPrecomputed = new double[intervals + 1];
        xPrecomputed = new double[intervals + 1];

        for (int i = 0; i <= intervals; i++)
        {
            xPrecomputed[i] = xLimitation * i / intervals;
            cdfPrecomputed[i] = CDF(xPrecomputed[i], mean, sigma);
        }
    }

    public static double InverseTransformSamplingOptimized(double u)
    {
        int low = 0, high = cdfPrecomputed.Length - 1;

        // Бинарный поиск
        while (low < high)
        {
            int mid = (low + high) / 2;
            if (cdfPrecomputed[mid] < u)
                low = mid + 1;
            else
                high = mid;
        }

        // Линейная интерполяция
        int i = Math.Max(0, low - 1);
        double x1 = xPrecomputed[i], x2 = xPrecomputed[i + 1];
        double cdf1 = cdfPrecomputed[i], cdf2 = cdfPrecomputed[i + 1];
        return x1 + (u - cdf1) * (x2 - x1) / (cdf2 - cdf1);
    }

    public static List<double> SimulateLogNormalOptimized(int numExperiments, double mean, double sigma, double xLimitation)
    {
        Random random = new Random();
        List<double> results = new List<double>();

        PrecomputeCDF(mean, sigma, xLimitation);

        for (int i = 0; i < numExperiments; i++)
        {
            double u = random.NextDouble();
            results.Add(InverseTransformSamplingOptimized(u));
        }

        return results;
    }

    static void PrintPDF(IEnumerable<(double mean, double sigma)> parameters, string name, double xLimitation = 20)
    {
        var plotModel = new PlotModel
        {
            Title = "Плотность вероятности логнормального распределения",
            Background = OxyColors.White
        };

        foreach (var (mean, sigma) in parameters)
        {
            var series = new LineSeries
            {
                Title = $"M={mean}, sigma={sigma}",
                StrokeThickness = 2
            };

            for (double x = 0; x <= xLimitation; x += 0.01)
            {
                double y = PDF(x, mean, sigma);
                series.Points.Add(new DataPoint(x, y));
            }

            plotModel.Series.Add(series);
        }

        using (var stream = File.Create($"{name}.png"))
        {
            PngExporter.Export(plotModel, stream, 800, 600);
        }

        Console.WriteLine($"График PDF сохранен как '{name}.png'");
    }

    static void PrintCDF(IEnumerable<(double mean, double sigma)> parameters, string name, double xLimitation = 20)
    {
        var plotModel = new PlotModel
        {
            Title = "Функция распределения (CDF) логнормального распределения",
            Background = OxyColors.White
        };

        foreach (var (mean, sigma) in parameters)
        {
            var series = new LineSeries
            {
                Title = $"M={mean}, sigma={sigma}",
                StrokeThickness = 2
            };

            for (double x = 0; x <= xLimitation; x += 0.01)
            {
                double y = CDF(x, mean, sigma);
                series.Points.Add(new DataPoint(x, y));
            }

            plotModel.Series.Add(series);
        }

        using (var stream = File.Create($"{name}.png"))
        {
            PngExporter.Export(plotModel, stream, 800, 600);
        }

        Console.WriteLine($"График CDF сохранен как '{name}.png'");
    }

    static void PrintHistogram(IEnumerable<double> data, string name, int bins = 100)
    {
        var plotModel = new PlotModel
        {
            Title = "Гистограмма относительных частот",
            Background = OxyColors.White
        };

        var histogram = new HistogramSeries
        {
            StrokeThickness = 0
        };

        double min = data.Min();
        double max = data.Max();
        double binWidth = (max - min) / bins;

        for (int i = 0; i < bins; i++)
        {
            double start = min + i * binWidth;
            double end = start + binWidth;
            int count = data.Count(x => x >= start && x < end);
            histogram.Items.Add(new HistogramItem(start, end, count, 0));
        }

        plotModel.Series.Add(histogram);

        using (var stream = File.Create($"{name}.png"))
        {
            PngExporter.Export(plotModel, stream, 800, 600);
        }

        Console.WriteLine($"График гистограммы сохранен как '{name}.png'");
    }

    static double CalculateDeviation(IEnumerable<double> data, double mean, double sigma, int bins = 100)
    {
        double min = data.Min();
        double max = data.Max();
        double binWidth = (max - min) / bins;
        double deviation = 0;

        for (int i = 0; i < bins; i++)
        {
            double start = min + i * binWidth;
            double end = start + binWidth;
            double midpoint = (start + end) / 2;

            double expectedDensity = PDF(midpoint, mean, sigma);
            int observedCount = data.Count(x => x >= start && x < end);
            double observedDensity = observedCount / (data.Count() * binWidth);

            deviation += Math.Pow(expectedDensity - observedDensity, 2);
        }

        return Math.Sqrt(deviation / bins);
    }

    static void Main()
    {
        double xLimitation = 20.0;

        // Задание 1: Графики плотности вероятности
        var pdfParameters = new List<(double mean, double sigma)>
        {
            (0, 1),
            (0, 2),
            (0, 0.5)
        };
        PrintPDF(pdfParameters, "LogNormal_PDF", xLimitation);

        // Задание 2: График CDF
        var cdfParameters = new List<(double mean, double sigma)> { (0, 1) };
        PrintCDF(cdfParameters, "LogNormal_CDF", xLimitation);

        // Задание 3 и 4: Моделирование и гистограммы
        List<int> numExperiments = new List<int> { 1000, 10000, 100000, 1000000 };
        foreach (var (mean, sigma) in cdfParameters)
        {
            foreach(var count in numExperiments)
            {
                Console.WriteLine($"Моделирование {count} экспериментов...");
                var simulatedData = SimulateLogNormalOptimized(count, mean, sigma, xLimitation);

                // Гистограмма
                PrintHistogram(simulatedData, $"LogNormal_Histogram_{count}");

                // СКО
                double deviation = CalculateDeviation(simulatedData, mean, sigma);
                Console.WriteLine($"Среднеквадратическое отклонение: {deviation:F6}");
            }
        }
    }
}
