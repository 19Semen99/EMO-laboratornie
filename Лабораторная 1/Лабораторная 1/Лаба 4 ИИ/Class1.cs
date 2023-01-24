using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Лаба_4_ИИ
{

    class Chromosome
    {
        public string binVal;
        public double decVal;
        public double funcVal;
        public double p;
        public Chromosome(string bin)
        {
            binVal = bin;
        }

        public Chromosome(string bin, double dec, double fun)
        {

            binVal = bin;
            decVal = dec;
            funcVal = fun;
        }

        public void ReCreate(string bin)
        {
            binVal = bin;
            decVal = 0;
            funcVal = 0;
        }
    }



    class FitnessFunc
    {

        public static readonly double minInt = 2;
        public static readonly double maxInt = 4;
        public static double Func(double x)
        {
            return (Math.Cos(Math.Exp(x)) / Math.Sin(Math.Log10(x)));
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

            List<Chromosome> genome = CreateGenome(chromosomesSize, chromosomesCount);

            foreach (Chromosome c in genome)
            {
                c.decVal = getChromosomeValue(c.binVal, minInterval, maxInterval);
            }

            textAll.Append("Исходная популяция" + Environment.NewLine);
            printGenome(genome);
            Chromosome best = genome.Find(g => g.funcVal == genome.Min(v => v.funcVal));
            while (best.funcVal > -2.9 && currentCount < iterationsCount)
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

                    string binary = chromosome.binVal;
                    if (random.NextDouble() <= mutationChance)
                    {

                        textAll.Append(" Мутация с шансом " + mutationChance + Environment.NewLine);
                        textAll.Append(chromosome.binVal + " " + chromosome.decVal + Environment.NewLine);
                        chromosome.binVal = Mutation(binary);
                        chromosome.decVal = getChromosomeValue(chromosome.binVal, minInterval, maxInterval);
                        chromosome.funcVal = FitnessFunc.Func(chromosome.decVal);
                        textAll.Append(chromosome.binVal + " " + chromosome.decVal + Environment.NewLine + " ");
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
            }

            textAll.Append("Финальный геном" + Environment.NewLine);
            printGenome(genome);
            //Chromosome best = genome.Find(g => g.funcVal == genome.Min(v => v.funcVal));

            
            textAll.Append("Итераций" + currentCount + "Лучшая: " + best.binVal + " | " + Environment.NewLine + best.decVal + " | " + best.funcVal + Environment.NewLine);
            //textAll.Append("Лучшая: " + best.binVal + " | " + nom1 + ran + " | " + nom2+ran);

            return best;

        }

        private string Mutation(string chromosome)
        {

            StringBuilder newChromosome = new StringBuilder(chromosome);
            int i = random.Next(chromosome.Count());
            newChromosome[i] = (newChromosome[i] == '0') ? '1' : '0';
            return newChromosome.ToString();

        }
        private void Reproduction(List<Chromosome> genome, double minInterval, double maxInterval, int chromosomesCount)
        {

            double fValuesSum = 0, a = 0, b = 0;
            foreach (Chromosome c in genome)
            {

                double funcVal = FitnessFunc.Func(getChromosomeValue(c.binVal, minInterval, maxInterval));
                c.funcVal = funcVal;
                fValuesSum += funcVal;

            }

            double sum;
            List<Chromosome> newGenome = new List<Chromosome>();
            textAll.Append("Репродукция:" + Environment.NewLine);
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
                    ;
                    newGenome.Add(new Chromosome(genome[rez[i]].binVal, genome[rez[i]].decVal, genome[rez[i]].funcVal));
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

                textAll.Append("Parent 1 " + parent1.binVal + " " + parent1.decVal + Environment.NewLine);
                textAll.Append("Parent 2 " + parent2.binVal + " " + parent2.decVal + Environment.NewLine);
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
        private List<Chromosome> CreateGenome(int size, int count)
        {

            List<Chromosome> genome = new List<Chromosome>();
            for (int i = 0; i < count; i++)
                genome.Add(new Chromosome(CreateBinaryModel(size)));

            return genome;

        }
        private Chromosome getParents(List<Chromosome> genome, Chromosome parent1)
        {

            bool contains = false;
            foreach (Chromosome c in genome)
                if (c.Equals(parent1))
                {
                    contains = true;
                    break;
                }

            if (contains)
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
                        tmp1 = parent2.funcVal;
                        count = i;
                    }
                }
                if (b)
                {
                    return genome[count];
                }
                else return null;
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
            else
                return null;

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

            int k = random.Next(parent1.binVal.Length);
            textAll.Append("K = " + k + " ,размер = " + parent1.binVal.Count() + Environment.NewLine);
            StringBuilder child1 = new StringBuilder();
            StringBuilder child2 = new StringBuilder();
            child1.Append(parent1.binVal.Substring(0, k));
            child1.Append(parent2.binVal.Substring(k, parent2.binVal.Length - k));
            child2.Append(parent2.binVal.Substring(0, k));
            child2.Append(parent1.binVal.Substring(k, parent1.binVal.Length - k));
            textAll.Append("Child 1 " + parent1.binVal.Substring(0, k) + "_" + parent2.binVal.Substring(k, parent2.binVal.Length - k) + Environment.NewLine);
            textAll.Append("Child 2 " + parent2.binVal.Substring(0, k) + "_" + parent1.binVal.Substring(k, parent1.binVal.Length - k) + Environment.NewLine);
            parent1.ReCreate(child1.ToString());
            parent2.ReCreate(child2.ToString());
            parent1.decVal = getChromosomeValue(child1.ToString(), minInterval, maxInterval);
            parent2.decVal = getChromosomeValue(child2.ToString(), minInterval, maxInterval);
            textAll.Append("Child 1 " + parent1.decVal + Environment.NewLine);
            textAll.Append("Child 2 " + parent2.decVal + Environment.NewLine);
        }

        public void printGenome(List<Chromosome> genome)
        {

            int i = 0;
            foreach (Chromosome c in genome)
            {
                textAll.Append("[" + i++ + "] " + c.binVal + " " + c.decVal + Environment.NewLine);
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
