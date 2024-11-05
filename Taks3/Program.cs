using System.Collections.Generic;
using Calc_PI;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.SkiaSharp;
using Task2;


class Task3
{
    static double NormalProbabilityDensity(double x, double mean, double sigma)
    {
        return (1 / (sigma * Math.Sqrt(2 * Math.PI))) * Math.Exp(-((x - mean) * (x - mean)) / (2 * sigma * sigma));
    }
    static void PrintGraphic(List<(double mean, double sigma)> parameters, string name)
    {
        // Создаем модель графика
        var plotModel = new PlotModel
        {
            Title = "Плотность вероятности нормального распределения",
            Background = OxyColors.White
        };

        foreach (var (mean, sigma) in parameters)
        {
            var series = new LineSeries
            {
                Title = $"M={mean}, sigma={sigma}",
                StrokeThickness = 2
            };

            // Заполняем значения для построения графика
            for (double x = 0; x <= 20; x += 0.01)
            {
                double y = NormalProbabilityDensity(x, mean, sigma);
                series.Points.Add(new DataPoint(x, y));
            }

            plotModel.Series.Add(series);
        }

        // Сохраняем график как изображение PNG
        using (var stream = File.Create($"{name}.png"))
        {
            PngExporter.Export(plotModel, stream, 800, 600);
        }

        Console.WriteLine($"График сохранен как '{name}.png'");
    }
    // Функция распределения нормального закона
    static double CumulativeDistributionFunction(double x, double mean, double sigma)
    {
        // Вычисляем функцию распределения через численное интегрирование
        double step = 0.01;
        double sum = 0;
        for (double i = mean - 10 * sigma; i < x; i += step)
        {
            sum += NormalProbabilityDensity(i, mean, sigma) * step;
        }
        return sum;
    }
    static double CalculateMSE(double[] experimentalData, double mean = 10, double sigma = 2,
        double minX = 0, double maxX = 20, int numSegments = 100)
    {
        double segmentWidth = (maxX - minX) / numSegments;
        var experimentalFrequencies = new double[numSegments];

        foreach (var value in experimentalData)
        {
            int segmentIndex = Math.Min((int)((value - minX) / segmentWidth), numSegments - 1);
            experimentalFrequencies[segmentIndex]++;
        }

        for (int i = 0; i < numSegments; i++)
        {
            experimentalFrequencies[i] /= experimentalData.Length;
        }

        var theoreticalProbabilities = new double[numSegments];
        for (int i = 0; i < numSegments; i++)
        {
            double x1 = minX + i * segmentWidth;
            double x2 = x1 + segmentWidth;
            theoreticalProbabilities[i] = CumulativeDistributionFunction(x2, mean, sigma) - CumulativeDistributionFunction(x1, mean, sigma);
        }

        double mse = 0;
        for (int i = 0; i < numSegments; i++)
        {
            mse += Math.Pow(experimentalFrequencies[i] - theoreticalProbabilities[i], 2);
        }
        mse /= numSegments;

        return mse;
    }

    // Кусочно-линейная аппроксимация обратной функции распределения
    static double InverseCdfApproximation(double u, List<(double x, double cdf)> distributionPoints)
    {
        for (int i = 1; i < distributionPoints.Count; i++)
        {
            var (x1, cdf1) = distributionPoints[i - 1];
            var (x2, cdf2) = distributionPoints[i];

            if (u >= cdf1 && u <= cdf2)
            {
                // Линейная интерполяция между точками (x1, cdf1) и (x2, cdf2)
                return x1 + (u - cdf1) * (x2 - x1) / (cdf2 - cdf1);
            }
        }

        // Возвращаем максимальное значение, если u больше cdf для всех точек (на всякий случай)
        return distributionPoints[^1].x;
    }

    static double[] ModelNormalDist(int numberOfExperiments, double mean=10, double sigma=2, 
        double minX=0, double maxX=20, int numSegments=100)
    {

        double segmentWidth = (maxX - minX) / numSegments;


        // Вычисляем точки для кусочно-линейной аппроксимации функции распределения
        var distributionPoints = new List<(double x, double cdf)>();
        for (int i = 0; i <= numSegments; i++)
        {
            double x = minX + i * segmentWidth;
            double cdf = CumulativeDistributionFunction(x, mean, sigma);
            distributionPoints.Add((x, cdf));
        }


        // Моделируем нормальное распределение методом обратной функции
        var random = new Random();
        var simulatedValues = new double[numberOfExperiments];

        for (int i = 0; i < numberOfExperiments; i++)
        {
            double u = random.NextDouble();  // Случайное число от 0 до 1
            double xValue = InverseCdfApproximation(u, distributionPoints);
            simulatedValues[i]=xValue;
        }
        return simulatedValues;
    }
    // Функция для расчета средней квадратичной ошибки
    static void PlotMSEvsExperiments(List<int> experimentCounts, List<double> mseValues)
    {
        var plotModel = new PlotModel { Title = "Зависимость MSE от числа экспериментов", Background=OxyColors.White  };

        // Логарифмическая шкала для оси X (число экспериментов)
        plotModel.Axes.Add(new LogarithmicAxis
        {
            Position = AxisPosition.Bottom,
            Title = "Число экспериментов",
            Base = 10, // основание логарифма (10)
            MinorGridlineStyle = LineStyle.Dot,
            MajorGridlineStyle = LineStyle.Solid
        });

        // Настройка оси Y (MSE)
        plotModel.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "MSE",
            Minimum = 0, // минимальное значение для оси Y
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot
        });

        // Добавляем серию точек
        var series = new LineSeries
        {
            Color = OxyColors.Green,
            MarkerType = MarkerType.Circle,
            MarkerSize = 3,
            MarkerStroke = OxyColors.Black
        };

        for (int i = 0; i < experimentCounts.Count; i++)
        {
            series.Points.Add(new DataPoint(experimentCounts[i], mseValues[i]));
        }

        plotModel.Series.Add(series);

        // Сохраняем график как изображение PNG
        using (var stream = File.Create("MSE_vs_Experiments_LogX.png"))
        {
            PngExporter.Export(plotModel, stream, 800, 600);
        }

        Console.WriteLine("График сохранен как 'MSE_vs_Experiments_LogX.png'");
    }


    static void Main()
    {
        var parameters = new List<(double mean, double sigma)>
            {
                (10, 2),
                (10, 1),
                (10, 0.5),
                (12, 1)
            };
        PrintGraphic(parameters, "task1");
       
        PrintGraphic(new List<(double mean, double sigma)> { (10, 2) }, "task2");
        List<int> countExperiments= new List<int>() { 1000,10000, 100000, 1000000};


        var mseValues = new List<double>();

        foreach (int count in countExperiments)
        {
            var experimentalData = ModelNormalDist(count);
            double mse = CalculateMSE(experimentalData);
            mseValues.Add(mse);

            HistogrammDrawler.Draw(Calculator.GetFreqDist(experimentalData, 0, 20, 100), $"10^{Math.Log10(count):0}", 0, 20, 100);
        }

        PlotMSEvsExperiments(countExperiments, mseValues);
    }
}
