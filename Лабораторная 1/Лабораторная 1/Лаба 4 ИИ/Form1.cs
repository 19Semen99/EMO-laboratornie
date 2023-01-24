using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Лаба_4_ИИ
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm();
        private void button1_Click(object sender, EventArgs e)
        {
            int kolChromosome = Convert.ToInt32(textBox3.Text);
            int kolIteration = Convert.ToInt32(textBox2.Text);
            double VerMyta = Convert.ToDouble(textBox6.Text);
            double VerCrosengove = Convert.ToDouble(textBox5.Text);

            Chromosome components = geneticAlgorithm.Evolution(kolChromosome, kolIteration, VerCrosengove, VerMyta);
            double x = components.decVal;
            double y = FitnessFunc.Func(x);//components.funcVal;

            var random = new Random();
            textBox1.Clear();
            textBox1.AppendText(geneticAlgorithm.TextAll + Environment.NewLine);
            textBox1.AppendText(" | " + x + " | " + y + Environment.NewLine);
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            ChartArea chart = new ChartArea("Math functions");
            chart.AxisX.Minimum = 2;
            chart.AxisX.Maximum = 4;
            chart.AxisY.Minimum = -4;
            chart.AxisY.Maximum = 4;

            chart.AxisY.Interval = 1;
            chart.AxisX.Interval = 0.1;

            chart1.ChartAreas.Add(chart);



            Series mySeriesOfPoint = new Series("функция");
            mySeriesOfPoint.ChartType = SeriesChartType.Line;
            mySeriesOfPoint.ChartArea = "Math functions";

            Series Finn = new Series("Лучшаяя");
            Finn.ChartType = SeriesChartType.Point;
            Finn.Color = Color.Red;
            Finn.ChartArea = "Math functions";


            for (double xx = FitnessFunc.minInt; xx <= FitnessFunc.maxInt; xx += 0.001)
            {
                mySeriesOfPoint.Points.AddXY(xx, FitnessFunc.Func(xx));
            }

            Finn.Points.AddXY(x, y);
            //Добавляем созданный набор точек в Chart
            chart1.Series.Add(mySeriesOfPoint);
            chart1.Series.Add(Finn);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double i = getChromosomeValue(textBox7.Text, 0, 20);
            textBox8.Text = Convert.ToString(i);
        }
        public static double getChromosomeValue(string s, double minInterval, double maxInterval)
        {

            int sum = 0;
            double c, znach = 0;

            for (int i = 0; i < s.Count(); i++)
            {
                c = char.GetNumericValue(s[i]);
                znach = Math.Pow(2, s.Count() - i - 1);
                sum += (int)(c * znach);
            }
            return minInterval + sum * ((maxInterval - minInterval) / (Math.Pow(2, s.Count()) - 1));
        }
    }
}
