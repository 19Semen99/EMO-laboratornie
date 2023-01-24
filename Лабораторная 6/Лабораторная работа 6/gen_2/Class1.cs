using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gen_2
{
    class BestPoint
    {
        public double bestx;
        public double besty;
        public double bestfunc;
        public BestPoint() { }
    }
    class Chromosome
    {
        public double xVal;
        public double yVal;
        public double funcVal;
        public double vtx, vty;
        public BestPoint mybest;
        public BestPoint localbest;

        private ScatterSeries point;

        public Chromosome()
        {
            mybest = new BestPoint();
            localbest = new BestPoint();
        }
        public Chromosome(double xval, double yval, double fun, double nvtx, double nvty)
        {

            xVal = xval;
            yVal = yval;
            funcVal = fun;
            vtx = nvtx;
            vty = nvty;
            mybest = new BestPoint();
            localbest = new BestPoint();
        }
        public Chromosome(Chromosome c)
        {
            xVal = c.xVal;
            yVal = c.yVal;
            funcVal = c.funcVal;
            vtx = c.vtx;
            vty = c.vty;
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
            //return 10 * 2 + Math.Pow(x, 2) - 10 * Math.Cos(2 * 3.14 * x) + Math.Pow(y, 2) - 10 * Math.Cos(2 * 3.14 * y);
            return (x * x) + (2 * y * y);
        }
    }

    class GeneticAlgorithm
    {
        public double neihgbourradius;
        private Random random = new Random();
        private StringBuilder textAll = new StringBuilder();
        List<Chromosome> genome;
        Chromosome best;

        public string TextAll { get { return textAll.ToString(); } }
        public GeneticAlgorithm() { }
        private void poisk(double nrad)
        {
            for (int i = 0; i < genome.Count; i++)
            {
                double nrad2 = nrad;
                int colp = 0;
            change_nrad:
                for (int j = 0; j < genome.Count; j++)
                {
                    
                    double v = Math.Sqrt(Math.Pow(genome[i].xVal - genome[j].xVal, 2) + Math.Pow(genome[i].yVal - genome[j].yVal, 2));
                    if (v < nrad2)
                    {
                        colp++;
                        if (genome[j].localbest.bestfunc < genome[i].localbest.bestfunc)
                        {
                            genome[i].localbest.bestfunc = genome[j].localbest.bestfunc;
                            genome[i].localbest.bestx = genome[j].localbest.bestx;
                            genome[i].localbest.besty = genome[j].localbest.besty;
                        }
                    }
                    if (colp == 0)
                    {
                        nrad2 += nrad;
                        goto change_nrad;
                    }
                }
            }
        }
        public List<Chromosome> Evolution(int iterationsCount, double nrad)
        {
            double currentnrad = nrad;
            textAll.Clear();
            double minInterval = FitnessFunc.minInt;
            double maxInterval = FitnessFunc.maxInt;
            int currentCount = 0;

            textAll.Append("Исходная популяция" + Environment.NewLine);
            printGenome(genome);
            Chromosome best = genome.Find(g => g.mybest.bestfunc == genome.Min(v => v.mybest.bestfunc));
            double tmpvtx, tmpvty;
            
            for (int i = 0; i < iterationsCount; i++)
            {
                //if (i % 5 == 0) currentnrad += nrad;
                poisk(currentnrad);
                for (int j = 0; j < genome.Count; j++)
                {
                    tmpvtx = (0.2 * genome[j].vtx) + (0.3 * random.NextDouble() * (genome[j].mybest.bestx - genome[j].xVal)) + (0.5 * random.NextDouble() * (genome[j].localbest.bestx - genome[j].xVal));
                    tmpvty = (0.2 * genome[j].vty) + (0.3 * random.NextDouble() * (genome[j].mybest.besty - genome[j].yVal)) + (0.5 * random.NextDouble() * (genome[j].localbest.besty - genome[j].yVal));
                    genome[j].xVal += tmpvtx;
                    genome[j].yVal += tmpvty;
                    genome[j].funcVal = FitnessFunc.Func(genome[j].xVal, genome[j].yVal);
                    if(genome[j].funcVal< genome[j].mybest.bestfunc)
                    {
                        genome[j].mybest.bestx = genome[j].xVal;
                        genome[j].mybest.besty = genome[j].yVal;
                        genome[j].mybest.bestfunc = genome[j].funcVal;
                    }
                    if(genome[j].mybest.bestfunc < genome[j].localbest.bestfunc )
                    {
                        genome[j].localbest.bestx = genome[j].mybest.bestx;
                        genome[j].localbest.besty = genome[j].mybest.besty;
                        genome[j].localbest.bestfunc = genome[j].mybest.bestfunc;
                    }
                }
                currentCount++;
            }

            best = genome.Find(g => g.mybest.bestfunc == genome.Min(v => v.mybest.bestfunc));
            textAll.Append("Финальный геном" + Environment.NewLine);
            printGenome(genome);

            textAll.Append("Итераций: " + currentCount + " Лучшая: " + "X: " + best.xVal + " | " + "Y: " + best.yVal + " | F: " + best.funcVal + Environment.NewLine);

            return genome;

        }

        public void CreateGenome(int count, double xMin, double xMax, double yMin, double yMax)
        {
            genome = new List<Chromosome>();
            for (int i = 0; i < count; i++)
            {
                genome.Add(new Chromosome((random.NextDouble() * (xMax - xMin) + xMin), (random.NextDouble() * (yMax - yMin) + yMin), 0, 0.5, 0.5));
                genome[i].funcVal = FitnessFunc.Func(genome[i].xVal, genome[i].yVal);
                genome[i].mybest.bestx = genome[i].xVal;
                genome[i].mybest.besty = genome[i].yVal;
                genome[i].mybest.bestfunc = genome[i].funcVal;
                genome[i].localbest.bestx = genome[i].xVal;
                genome[i].localbest.besty = genome[i].yVal;
                genome[i].localbest.bestfunc = genome[i].funcVal;
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
