using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        //GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm();
        private void button1_Click(object sender, EventArgs e)
        {
            int kolChromosome = Convert.ToInt32(textBox3.Text);
            int kolIteration = Convert.ToInt32(textBox2.Text);
            double VerMyta = Convert.ToDouble(textBox6.Text);
            double VerCrosengove = Convert.ToDouble(textBox5.Text);
            int[,] mass = new int[48, 3];
            var infile = new StreamReader("1.txt");
            int i = 0;
            while (!infile.EndOfStream)
            {
                var line = infile.ReadLine();
                var ints = line.Split(' ');

                for (int j = 0; j < 3; j++)
                {
                    mass[i, j] = Convert.ToInt32(ints[j]);
                }
                i++;
            }

            GeneticAlgorithm algorithm = new GeneticAlgorithm();
            algorithm.init(48, kolChromosome, mass);
            //best length = 1617.1647428318208 1143.5081559521798 33523.708507435593
            List<Chromosome> chromosomes = algorithm.algorithm(kolIteration, VerCrosengove, VerMyta);
            Chromosome c = chromosomes.Find(chr => chr.length == chromosomes.Min(Chromosome => Chromosome.length));
            int[] mass1 =
            {
                1,
8,
38,
31,
44,
18,
7,
28,
6,
37,
19,
27,
17,
43,
30,
36,
46,
33,
20,
47,
21,
32,
39,
48,
5,
42,
24,
10,
45,
35,
4,
26,
2,
29,
34,
41,
16,
22,
3,
23,
14,
25,
13,
11,
12,
15,
40,
9,
1
            };
            Chromosome prob = new Chromosome();
            prob.mass = mass1;
            prob.sortedmass = mass1;
            double best2 = algorithm.count_length(prob);
            double best = chromosomes.Min(Chromosome => Chromosome.length);
            //.Select(Convert.ToInt32).ToList();
            //var random = new Random();
            textBox1.Clear();
            textBox1.AppendText(algorithm.textAll + Environment.NewLine);
            textBox1.AppendText(Environment.NewLine + "Финальный геном:" + Environment.NewLine);
            for (int j = 0; j < chromosomes.Count; j++)
            {
                textBox1.AppendText("Хромосома №" + (j + 1) + " длина: " + chromosomes[j].length + Environment.NewLine);
            }
            chart1.Series.Clear();
            chart1.Series.Add("Line");
            for (int j = 0; j < algorithm.cities.Length - 1; j++)
            {

                //chart1.Series["Line"].Points.Add(new DataPoint(algorithm.cities[mass1[j] - 1].x, algorithm.cities[mass1[j] - 1].y));
                //DataPoint dp = new DataPoint(algorithm.cities[mass1[j] - 1].x, algorithm.cities[mass1[j] - 1].y);
                //chart1.Series["Line"].Points.Add(dp);
                //chart1.Series["Line"].ChartType = SeriesChartType.Line;
                chart1.Series["Line"].Points.Add(new DataPoint(algorithm.cities[c.sortedmass[j] - 1].x, algorithm.cities[c.sortedmass[j] - 1].y));
                DataPoint dp = new DataPoint(algorithm.cities[c.sortedmass[j + 1] - 1].x, algorithm.cities[c.sortedmass[j + 1] - 1].y);
                chart1.Series["Line"].Points.Add(dp);
                chart1.Series["Line"].ChartType = SeriesChartType.Line;
            }
            //chart1.Series.Add("nLine");
            //chart1.Series["nLine"].Points.Add(chart1.Series["Line"].Points[chart1.Series["Line"].Points.Count - 1]);
            //chart1.Series["nLine"].Points.Add(new DataPoint(algorithm.cities[mass1[0] - 1].x, algorithm.cities[mass1[0] - 1].y));
            //chart1.Series["nLine"].Color = Color.Red;
            //chart1.Series["nLine"].ChartType = SeriesChartType.Line;
            chart1.Series.Add("nLine");

            chart1.Series["nLine"].Points.Add(new DataPoint(algorithm.cities[c.sortedmass[0] - 1].x, algorithm.cities[c.sortedmass[0] - 1].y));
            chart1.Series["nLine"].Points.Add(new DataPoint(algorithm.cities[c.sortedmass.Last() - 1].x, algorithm.cities[c.sortedmass.Last() - 1].y));
            chart1.Series["nLine"].Color = Color.Red;
            chart1.Series["nLine"].ChartType = SeriesChartType.Line;
            textBox1.AppendText(Environment.NewLine + "Лучшая длина: " + best.ToString() + Environment.NewLine);
        }

        private void button2_Click(object sender, EventArgs e)
        {

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

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
