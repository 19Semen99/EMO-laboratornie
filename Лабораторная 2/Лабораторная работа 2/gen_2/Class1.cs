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
        public string xbinVal;
        public string ybinVal;
        public double xdecVal;
        public double ydecVal;
        public double funcVal;

        private ScatterSeries point;
        public Chromosome(string xbin, string ybin)
        {
            xbinVal = xbin;
            ybinVal = ybin;
        }

        public Chromosome(string xbin, double xdec, string ybin, double ydec, double fun)
        {

            xbinVal = xbin;
            xdecVal = xdec;
            ybinVal = ybin;
            ydecVal = ydec;
            funcVal = fun;
        }
        public ScatterSeries Point
        {
            get
            {
                this.point = new ScatterSeries();
                this.point.ColorAxisKey = "ColorAxis";
                this.point.MarkerSize = 8;
                this.point.MarkerType = MarkerType.Plus;

                point.Points.Add(new ScatterPoint(xdecVal, ydecVal, double.NaN, 1));
                return point;
            }
            set => point = value;
        }
        public void ReCreate(string xbin, string ybin)
        {
            xbinVal = xbin;
            ybinVal = ybin;
            xdecVal = 0;
            ydecVal = 0;
            funcVal = 0;
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

        public string TextAll { get { return textAll.ToString(); } }
        public GeneticAlgorithm() { }
        public Chromosome Evolution(int chromosomesCount, int iterationsCount, double crossingoverChance, double mutationChance)
        {

            textAll.Clear();
            double minInterval = FitnessFunc.minInt;
            double maxInterval = FitnessFunc.maxInt;
            int intervalsCount = (int)(maxInterval - minInterval) * 1000;
            int chromosomesSize = getChromosomeSize(intervalsCount);
            int currentCount = 0;

            List<Chromosome> genome = CreateGenome(chromosomesSize, chromosomesSize, chromosomesCount);

            foreach (Chromosome c in genome)
            {
                c.xdecVal = getChromosomeValue(c.xbinVal, minInterval, maxInterval);
                c.ydecVal = getChromosomeValue(c.ybinVal, minInterval, maxInterval);
                c.funcVal = FitnessFunc.Func(c.xdecVal, c.ydecVal);
            }

            textAll.Append("Исходная популяция" + Environment.NewLine);
            printGenome(genome);
            Chromosome best = genome.Find(g => g.funcVal == genome.Min(v => v.funcVal));
            while (best.funcVal > 0 && currentCount < iterationsCount)
            {

                textAll.Append("Итерация " + (currentCount + 1) + Environment.NewLine);
                Reproduction(genome, minInterval, maxInterval, chromosomesCount);
                printGenome(genome);

                foreach (Chromosome chromosome in genome)
                {
                    if (random.NextDouble() <= crossingoverChance)
                    {
                        textAll.Append(genome.IndexOf(chromosome) + " Кроссинговер с шансом " + crossingoverChance + Environment.NewLine);
                        Crossingover(genome, chromosome, minInterval, maxInterval);
                    }

                    string xbinary = chromosome.xbinVal;
                    string ybinary = chromosome.ybinVal;
                    if (random.NextDouble() <= mutationChance)
                    {

                        textAll.Append(" Мутация с шансом " + mutationChance + Environment.NewLine);
                        textAll.Append("X: " + chromosome.xbinVal + " " + chromosome.xdecVal + Environment.NewLine);
                        textAll.Append("Y: " + chromosome.ybinVal + " " + chromosome.ydecVal + Environment.NewLine);
                        Chromosome chr = Mutation(chromosome);
                        chromosome.xbinVal = chr.xbinVal;
                        chromosome.ybinVal = chr.ybinVal;
                        chromosome.xdecVal = getChromosomeValue(chromosome.xbinVal, minInterval, maxInterval);
                        chromosome.ydecVal = getChromosomeValue(chromosome.ybinVal, minInterval, maxInterval);
                        chromosome.funcVal = FitnessFunc.Func(chromosome.xdecVal, chromosome.ydecVal);
                        textAll.Append("newX: " + chromosome.xbinVal + " " + chromosome.xdecVal + Environment.NewLine);
                        textAll.Append("newY: " + chromosome.ybinVal + " " + chromosome.ydecVal + Environment.NewLine);
                    }



                }

                Chromosome cr;
                for (int i = 0; i < genome.Count; i++)
                {
                    for (int j = 0; j < genome.Count; j++)
                    {
                        if (genome[i].funcVal < genome[j].funcVal)
                        {
                            cr = genome[i];
                            genome[i] = genome[j];
                            genome[j] = cr;
                        }
                    }
                }
                if (genome.Count > chromosomesCount)
                {
                    for (int i = genome.Count - 1; i > chromosomesCount - 1; i--)
                    {
                        genome.RemoveAt(i);
                    }
                }
                textAll.Append("сокращённая популяция" + Environment.NewLine);
                printGenome(genome);
                textAll.Append("Среднее значение: " + genome.Average(g => g.funcVal) + Environment.NewLine + "Минимальное значение: " + genome.Min(g => g.funcVal) + Environment.NewLine);
                currentCount++;
                best = genome.Find(g => g.funcVal == genome.Min(v => v.funcVal));
                int abc = 0;
            }

            textAll.Append("Финальный геном" + Environment.NewLine);
            printGenome(genome);
            //Chromosome best = genome.Find(g => g.funcVal == genome.Min(v => v.funcVal));


            textAll.Append("Итераций: " + currentCount + " Лучшая: " + "X: " + best.xdecVal + " | " + "Y: " + best.ydecVal + " | F: " + best.funcVal + Environment.NewLine);
            //textAll.Append("Лучшая: " + best.binVal + " | " + nom1 + ran + " | " + nom2+ran);

            return best;

        }

        private Chromosome Mutation(Chromosome chromosome)
        {

            StringBuilder newChromosomex = new StringBuilder(chromosome.xbinVal);
            StringBuilder newChromosomey = new StringBuilder(chromosome.ybinVal);
            int i = random.Next(chromosome.xbinVal.Count());
            newChromosomex[i] = (newChromosomex[i] == '0') ? '1' : '0';
            i = random.Next(chromosome.ybinVal.Count());
            newChromosomey[i] = (newChromosomey[i] == '0') ? '1' : '0';
            chromosome.xbinVal = newChromosomex.ToString();
            chromosome.ybinVal = newChromosomey.ToString();
            return chromosome;

        }
        private void Reproduction(List<Chromosome> genome, double minInterval, double maxInterval, int chromosomesCount)
        {

            double fValuesSum = 0, a = 0, b = 0;

            double sum;
            List<Chromosome> newGenome = new List<Chromosome>();
            textAll.Append("Репродукция:" + Environment.NewLine);
            Chromosome cr;
            //for (int i = 0; i < genome.Count; i++)
            //{
            //    for (int j = 0; j < genome.Count; j++)
            //    {
            //        if (genome[i].funcVal < genome[j].funcVal)
            //        {
            //            cr = genome[i];
            //            genome[i] = genome[j];
            //            genome[j] = cr;
            //        }
            //    }
            //}

            Random rnd = new Random();
            double[] mass22 = new double[genome.Count];
            int[] mass11 = new int[3];
            int[] rez = new int[genome.Count];
            for (int i = 0; i < genome.Count; i++)
            {
                for (int j = 0; j < mass11.Length; j++)
                {
                    mass11[j] = rnd.Next(0, genome.Count);
                }
                mass22[i] = genome[mass11[0]].funcVal;
                rez[i] = mass11[0];
                for (int j = 1; j < mass11.Length; j++)
                {
                    if (mass22[i] > mass22[mass11[j]])
                    {
                        mass22[j] = mass22[mass11[j]];
                        rez[i] = mass11[j];
                    }
                }
            }

            for (int i = 0; i < genome.Count; i++)
            {
                {
                    newGenome.Add(new Chromosome(genome[rez[i]].xbinVal, genome[rez[i]].xdecVal, genome[rez[i]].ybinVal, genome[rez[i]].ydecVal, genome[rez[i]].funcVal));
                }
            }


            genome.Clear();
            genome.AddRange(newGenome);

        }
        private void Crossingover(List<Chromosome> genome, Chromosome parent1, double minInterval, double maxInterval)
        {

            Chromosome parent2 = getParents(genome, parent1);
            if (parent2 != null)
            {

                textAll.Append("Parent 1 " + "x= " + parent1.xbinVal + " " + parent1.xdecVal + "y= " + parent1.ybinVal + " " + parent1.ydecVal + Environment.NewLine);
                textAll.Append("Parent 2 " + "x= " + parent2.xbinVal + " " + parent2.xdecVal + "y= " + parent2.ybinVal + " " + parent2.ydecVal + Environment.NewLine);
                GetChildBySinglePointCrossingover(parent1, parent2, minInterval, maxInterval);
            }

        }
        private string CreateBinaryModel(int size)
        {
            StringBuilder value = new StringBuilder();
            for (int i = 0; i < size; i++)
                value.Append(random.NextDouble() <= 0.5 ? '1' : '0');

            return value.ToString();
        }
        private List<Chromosome> CreateGenome(int sizex, int sizey, int count)
        {

            List<Chromosome> genome = new List<Chromosome>();
            for (int i = 0; i < count; i++)
                genome.Add(new Chromosome(CreateBinaryModel(sizex), CreateBinaryModel(sizey)));

            return genome;

        }
        private Chromosome getParents(List<Chromosome> genome, Chromosome parent1)
        {

            //bool contains = false;
            //foreach (Chromosome c in genome)
            //    if (c.Equals(parent1))
            //    {
            //        contains = true;
            //        break;
            //    }

            //if (contains)
            {
                Chromosome parent2;
                int[] mass = new int[genome.Count];
                int count = 0;
                double tmp1, tmp2;
                bool b = false;
                tmp1 = Math.Abs(parent1.funcVal - genome[0].funcVal);
                for (int i = 1; i < genome.Count; i++)
                {
                    parent2 = genome[i];
                    tmp2 = Math.Abs(parent1.funcVal - parent2.funcVal);
                    if (tmp2 > 0 && tmp1 < tmp2)
                    {
                        b = true;
                        tmp1 = parent1.funcVal - parent2.funcVal;//parent2.funcVal;
                        count = i;
                    }
                }
                if (b)
                {
                    return genome[count];
                }
                else return genome[0];//null;
                //Chromosome parent2;
                //int[] mass = new int[genome.Count];
                //int count = 100, count2 = -1, tmp;
                //for (int i = 0; i < genome.Count; i++)
                //{
                //    parent2 = genome[i];
                //    tmp = checkbin(parent1.binVal, parent2.binVal);
                //    if (count > tmp && tmp > 0)
                //    {
                //        count = tmp;
                //        count2 = i;
                //    }
                //    //mass[i] = checkbin(parent1.binVal, parent2.binVal);
                //}
                ////do {
                ////    parent2 = genome[random.Next(genome.Count)];

                ////}
                ////while (!checkbin(parent1.binVal, parent2.binVal));
                //if (count2 > 0 && count <= blrod)
                //    return genome[count2];
                //else return null;
            }
            //else
               // return null;

        }
        private int checkbin(string str1, string str2)
        {
            int count = 0;
            for (int i = 0; i < str1.Length; i++)
            {
                if (str1[i] != str2[i])
                    count++;
            }
            return count;
        }
        private void GetChildBySinglePointCrossingover(Chromosome parent1, Chromosome parent2, double minInterval, double maxInterval)
        {

            int k = random.Next(parent1.xbinVal.Length);
            textAll.Append("K = " + k + " ,размер = " + parent1.xbinVal.Count() + Environment.NewLine);
            StringBuilder xchild1 = new StringBuilder();
            StringBuilder xchild2 = new StringBuilder();
            StringBuilder ychild1 = new StringBuilder();
            StringBuilder ychild2 = new StringBuilder();
            xchild1.Append(parent1.xbinVal.Substring(0, k));
            xchild1.Append(parent2.xbinVal.Substring(k, parent2.xbinVal.Length - k));
            xchild2.Append(parent2.xbinVal.Substring(0, k));
            xchild2.Append(parent1.xbinVal.Substring(k, parent1.xbinVal.Length - k));
            ychild1.Append(parent1.ybinVal.Substring(0, k));
            ychild1.Append(parent2.ybinVal.Substring(k, parent2.ybinVal.Length - k));
            ychild2.Append(parent2.ybinVal.Substring(0, k));
            ychild2.Append(parent1.ybinVal.Substring(k, parent1.ybinVal.Length - k));
            //textAll.Append("Child 1 " + parent1.binVal.Substring(0, k) + "_" + parent2.binVal.Substring(k, parent2.binVal.Length - k) + Environment.NewLine);
            //textAll.Append("Child 2 " + parent2.binVal.Substring(0, k) + "_" + parent1.binVal.Substring(k, parent1.binVal.Length - k) + Environment.NewLine);
            parent1.ReCreate(xchild1.ToString(), ychild1.ToString());
            parent2.ReCreate(xchild2.ToString(), ychild2.ToString());
            parent1.xdecVal = getChromosomeValue(xchild1.ToString(), minInterval, maxInterval);
            parent1.ydecVal = getChromosomeValue(ychild1.ToString(), minInterval, maxInterval);
            parent2.xdecVal = getChromosomeValue(xchild2.ToString(), minInterval, maxInterval);
            parent2.ydecVal = getChromosomeValue(ychild2.ToString(), minInterval, maxInterval);
            textAll.Append("Child 1 x: " + parent1.xdecVal + " y: " + parent1.ydecVal + Environment.NewLine);
            textAll.Append("Child 2 x: " + parent2.xdecVal + " y: " + parent2.ydecVal + Environment.NewLine);
            parent1.funcVal = FitnessFunc.Func(parent1.xdecVal, parent1.ydecVal);
            parent2.funcVal = FitnessFunc.Func(parent2.xdecVal, parent2.ydecVal);
        }

        public void printGenome(List<Chromosome> genome)
        {

            int i = 0;
            foreach (Chromosome c in genome)
            {
                textAll.Append("[" + i++ + "] " + "x = " + c.xdecVal + " y = " + c.ydecVal + " F = " + c.funcVal + Environment.NewLine);
            }

        }
        public static int getChromosomeSize(int num)
        {

            int size = 0;
            while (num > 0)
            {
                num >>= 1;
                size++;
            }

            return size;

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
            //sum = sum;
            return minInterval + sum * ((maxInterval - minInterval) / (Math.Pow(2, s.Count()) - 1));
        }


    }
}
