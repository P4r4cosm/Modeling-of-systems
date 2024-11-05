using OxyPlot;
using OxyPlot.Series;
using OxyPlot.SkiaSharp;

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



            var Xvalues=new List<int>();
            for (int i = 0;i < array.Length; i++) Xvalues.Add(i);

            plotModel.Series.Add(rectangleBarSeries);
            // Настраиваем ось категорий для значений
            plotModel.Axes.Add(new OxyPlot.Axes.CategoryAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                Key = "CategoryAxis",
                ItemsSource = Xvalues,
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
                var exporter = new PngExporter {Width = 1600, Height = 600};
                exporter.Export(plotModel, stream);
            }

            //Console.WriteLine($"Гистограмма сохранена в файл: {outputPath}");

        }
        public static void Draw(double[] array, string ImageName, double A, double B, int IntervalsCount)
        {

            var plotModel = new PlotModel { Title = ImageName, Background = OxyColors.White };
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


            double dY = (B - A) / IntervalsCount;
            var Xvalues = new List<double>();
            for (int i = 0; i < array.Length; i++) Xvalues.Add(Math.Round(i*dY,2));

            plotModel.Series.Add(rectangleBarSeries);
            // Настраиваем ось категорий для значений
            plotModel.Axes.Add(new OxyPlot.Axes.CategoryAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                Key = "CategoryAxis",
                ItemsSource = Xvalues,
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
                var exporter = new PngExporter { Width = 4000, Height = 1000 };
                exporter.Export(plotModel, stream);
            }

            //Console.WriteLine($"Гистограмма сохранена в файл: {outputPath}");

        }
    }
}
