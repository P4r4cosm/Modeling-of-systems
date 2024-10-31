using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.SkiaSharp;
using OxyPlot.Axes;

namespace Task2
{
    public class HistogrammDrawler
    {
        public static void Draw(double[] array, string ImageName)
        {
            var plotModel = new PlotModel { Title = ImageName, Background=OxyColors.White};
            // Добавляем серию для гистограммы
            var rectangleBarSeries = new RectangleBarSeries
            {
                FillColor = OxyColors.SkyBlue,
                StrokeColor = OxyColors.Black,
                StrokeThickness = 1
            };
            // Добавляем каждый столбец в RectangleBarSeries
            for (int i = 0; i < array.Length; i++)
            {
                // Определяем границы столбца
                var rectangleBar = new RectangleBarItem(i - 0.5, 0, i + 0.5, array[i]);
                rectangleBarSeries.Items.Add(rectangleBar);
            }
            plotModel.Series.Add(rectangleBarSeries);
            // Настраиваем ось категорий для значений
            plotModel.Axes.Add(new OxyPlot.Axes.CategoryAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                Key = "CategoryAxis",
                ItemsSource = new[] { "0","1", "2", "3", "4", "5", "6", "7", "8", "9" },
                Title = "resXk"
            });
            // Настраиваем ось значений для частот
            plotModel.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Left,
                Minimum = 0,
                Title = "resYk"
            });
            // Путь для сохранения графика
            var outputPath = Path.Combine(Directory.GetCurrentDirectory(), $"{ImageName}.png");

            // Сохраняем гистограмму как изображение
            using (var stream = File.Create(outputPath))
            {
                var exporter = new PngExporter {Width = 1000, Height = 600};
                exporter.Export(plotModel, stream);
            }

            //Console.WriteLine($"Гистограмма сохранена в файл: {outputPath}");

        }
    }
}
