using Newtonsoft.Json;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace gen_2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private PlotModel model;

        private int sizeMap = 200;
        public double startPoint = -5.12, endPoint = 5.12;

        private ScatterSeries point = new ScatterSeries();
        GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm();
        public MainWindow()
        {
            InitializeComponent();

            initHeatMap();
            point.ColorAxisKey = "ColorAxis";
            point.MarkerSize = 8;
            point.MarkerType = MarkerType.Plus;

        }
        
        private void initHeatMap()
        {
            model = new PlotModel { Title = "Heatmap" };
            model.Axes.Add(new LinearColorAxis { Palette = OxyPalettes.Viridis(255) });

            var data = new double[sizeMap, sizeMap];
            double xValue = startPoint, yValue = startPoint, max = endPoint;
            double det = (Math.Abs(xValue) + max) / sizeMap;

            for (int y = 0; y < sizeMap; ++y)
            {
                xValue = startPoint;
                for (int x = 0; x < sizeMap; ++x)
                {
                    data[x, y] = getFunction(xValue, yValue);
                    xValue += det;
                }
                yValue += det;
            }

            var heatMapSeries = new HeatMapSeries
            {
                X0 = startPoint,
                X1 = endPoint,
                Y0 = startPoint,
                Y1 = endPoint,
                Interpolate = true,
                RenderMethod = HeatMapRenderMethod.Bitmap,
                Data = data
            };

            var axis1 = new LinearColorAxis();
            axis1.Key = "ColorAxis";
            axis1.IsAxisVisible = true;
            model.Axes.Add(axis1);

            axis1.Palette.Colors.Clear();
            axis1.Palette.Colors.Add(OxyColor.FromArgb((byte)255, 255, 0, 0));

            model.Series.Add(heatMapSeries);
            testPlotView.Model = model;
        }


        public double getFunction(double x, double y)
        {
            return (x * x) + (2 * y * y);
        }


        
        /// <summary>
        /// /////////////////////////////////////////////////////////////////////
        /// </summary>

        private void testButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            int kolChromosome = Convert.ToInt32(textBlock2.Text);
            int kolIteration = Convert.ToInt32(textBlock1.Text);
            double VerMyta = Convert.ToDouble(textBlock4.Text);
            double VerCrosengove = Convert.ToDouble(textBlock3.Text);

            Chromosome components = geneticAlgorithm.Evolution(kolChromosome, kolIteration, VerCrosengove, VerMyta);
            double x = components.xdecVal;
            double y = components.ydecVal;//FitnessFunc.Func(x);//components.funcVal;
            initHeatMap();
            var random = new Random();
            textBox1.Clear();
            textBox1.AppendText(geneticAlgorithm.TextAll + Environment.NewLine);
            //textBox1.AppendText(" | " + x + " | " + y + Environment.NewLine);
            while (model.Series.Count > 1)
            {
                model.Series.Remove(model.Series.Last());
            }
            //point.Points.Clear();
            //point.Points.Add(new ScatterPoint(x, y, double.NaN, 1));
            model.Series.Add(components.Point);
            //testPlotView.Model = model;
            testPlotView.Model.InvalidatePlot(true);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
        }



    }

}
