using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gen_2
{
    class Chromosome
    {
        public double xVal;
        public double yVal;
        public double funcVal;

        private ScatterSeries point;
        public Chromosome()
        {
        }
        public Chromosome(double xval, double yval, double fun)
        {

            xVal = xval;
            yVal = yval;
            funcVal = fun;
        }
        public Chromosome(Chromosome c)
        {

            xVal = c.xVal;
            yVal = c.yVal;
            funcVal = c.funcVal;
        }
        public ScatterSeries Point
        {
            get
            {
                this.point = new ScatterSeries();
                this.point.ColorAxisKey = "ColorAxis";
                this.point.MarkerSize = 8;
                this.point.MarkerType = MarkerType.Plus;

                point.Points.Add(new ScatterPoint(xVal, yVal, double.NaN, 1));
                return point;
            }
            set => point = value;
        }
        public void ReCreateFull(double xval, double yval, double fun)
        {
            xVal = xval;
            yVal = yval;
            funcVal = fun;
        }
    }



    class FitnessFunc
    {

        public static readonly double minInt = -5.12;
        public static readonly double maxInt = 5.12;
        public static double Func(double x, double y)
        {
            return (x * x) + (2 * y * y);
        }
    }

    class GeneticAlgorithm
    {

        private Random random = new Random();
        private StringBuilder textAll = new StringBuilder();
        List<Chromosome> genome;

        public string TextAll { get { return textAll.ToString(); } }
        public GeneticAlgorithm() { }
        public Chromosome Evolution(int iterationsCount, double xMut, double yMut)
        {

            textAll.Clear();
            double minInterval = FitnessFunc.minInt;
            double maxInterval = FitnessFunc.maxInt;
            int currentCount = 0;



            textAll.Append("Исходная популяция" + Environment.NewLine);
            printGenome(genome);
            Chromosome best = genome.Find(g => g.funcVal == genome.Min(v => v.funcVal));
            for (int i = 0; i < iterationsCount; i++)
            {
                foreach (Chromosome chr in genome)
                {
                    Chromosome c = new Chromosome();
                    if (random.Next(0, 2) == 0)
                        c.xVal = chr.xVal + random.NextDouble() * xMut;
                    else
                        c.xVal = chr.xVal - random.NextDouble() * xMut;
                    if (random.Next(0, 2) == 0)
                        c.yVal = chr.yVal + random.NextDouble() * yMut;
                    else
                        c.yVal = chr.yVal - random.NextDouble() * yMut;
                    c.funcVal = FitnessFunc.Func(c.xVal, c.yVal);
                    if (c.funcVal < chr.funcVal)
                    {
                        chr.xVal = c.xVal;
                        chr.yVal = c.yVal;
                        chr.funcVal = c.funcVal;
                    }
                }
                //textAll.Append("сокращённая популяция" + Environment.NewLine);
                //printGenome(genome);
                textAll.Append("Среднее значение: " + genome.Average(g => g.funcVal) + Environment.NewLine + "Минимальное значение: " + genome.Min(g => g.funcVal) + Environment.NewLine);
                currentCount++;
                best = genome.Find(g => g.funcVal == genome.Min(v => v.funcVal));
            }

            textAll.Append("Финальный геном" + Environment.NewLine);
            printGenome(genome);

            textAll.Append("Итераций: " + currentCount + " Лучшая: " + "X: " + best.xVal + " | " + "Y: " + best.yVal + " | F: " + best.funcVal + Environment.NewLine);

            return best;

        }

        //private void Reproduction()
        //{
        //    Chromosome cr;
        //    for (int i = 0; i < genome.Count; i++)
        //    {
        //        for (int j = 0; j < genome.Count; j++)
        //        {
        //            if (genome[i].funcVal < genome[j].funcVal)
        //            {
        //                cr = genome[i];
        //                genome[i] = genome[j];
        //                genome[j] = cr;
        //            }
        //        }
        //    }
        //}

        public void CreateGenome(int count, double xMin, double xMax, double yMin, double yMax)
        {
            genome = new List<Chromosome>();
            for (int i = 0; i < count; i++)
            {
                genome.Add(new Chromosome((random.NextDouble() * (xMax - xMin) + xMin), (random.NextDouble() * (yMax - yMin) + yMin), 0));
                genome[i].funcVal = FitnessFunc.Func(genome[i].xVal, genome[i].yVal);
            }

        }

        public void printGenome(List<Chromosome> genome)
        {

            int i = 0;
            foreach (Chromosome c in genome)
            {
                textAll.Append("[" + i++ + "] " + "x = " + c.xVal + " y = " + c.yVal + " F = " + c.funcVal + Environment.NewLine);
            }

        }

    }
}
