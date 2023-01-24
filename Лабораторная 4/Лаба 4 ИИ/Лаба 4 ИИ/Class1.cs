using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Лаба_4_ИИ
{

    class Chromosome
    {
        public int[] mass;
        public int hits;
        public int colcikl;
        public Chromosome(int[] mass2)
        {
            mass = new int[mass2.Length];
            mass2.CopyTo(mass, 0);
            hits = mass2.Length;
        }
        public Chromosome(int n)
        {
            mass = new int[n];
            mass.Initialize();
            hits = n;
        }
        public Chromosome()
        {

        }
    }

    class GeneticAlgorithm
    {
        int colN, colH;
        public List<Chromosome> chromosomes;
        public StringBuilder textAll;
        private Random random;
        public GeneticAlgorithm()
        {
            chromosomes = new List<Chromosome>();
            textAll = new StringBuilder();
            random = new Random();
        }
        public void init(int n, int m)
        {
            colN = n;
            colH = m;
            for (int i = 0; i < colH; i++)
            {
                chromosomes.Add(new Chromosome(chrString(n)));
            }
        }
        public int[] chrString(int n)
        {
            List<int> list = new List<int>();
            int[] mass = new int[n];
            for (int i = 0; i < n; i++)
            {
                list.Add(i);
            }
            for (int i = 0; i < n; i++)
            {
                int tmp = list[random.Next(0, list.Count - 1)];
                mass[i] = tmp;
                list.Remove(tmp);
            }
            return mass;
        }
        public Chromosome algorithm(int colN, int colChr, double crosschans, double mutchans)
        {
            init(colN, colChr);
            List<Chromosome> newgen = new List<Chromosome>();
            Chromosome best = new Chromosome();
            
            bool b = true;
            int count = 0;
            while (b)
            {
                for (int j = 0; j < chromosomes.Count; j++)
                {
                    if (random.NextDouble() <= crosschans)
                    {
                        newgen.Add(new Chromosome(crossingover(j)));

                    }
                    if (random.NextDouble() <= mutchans)
                    {
                        mutation(chromosomes[j]);
                    }
                    
                }
                chromosomes.AddRange(newgen);
                newgen.Clear();
                for (int j = 0; j < chromosomes.Count; j++)
                {
                    chromosomes[j].hits = crossCheck(chromosomes[j].mass);
                    if (chromosomes[j].hits == 0)
                    {
                        b = false;
                        best = chromosomes[j];
                    }
                }

                Chromosome tmp;
                int tmpi;
                for (int k=0; k<chromosomes.Count; k++)
                {
                    tmp = chromosomes[k];
                    tmpi = k;
                    for (int l = 0; l < chromosomes.Count; l++)
                    {
                        if(tmp.hits > chromosomes[l].hits)
                        {
                            chromosomes[tmpi] = chromosomes[l];
                            chromosomes[l] = tmp;
                            tmpi = l;
                        }
                    }
                }
                for (int i = colChr; i < chromosomes.Count;)
                {
                    chromosomes.RemoveAt(i);
                }
                count++;
            }
            best.colcikl = count;
            return best;
        }

        public int[] crossingover(int p1num)
        {
            int p2num;
        crossingerr: p2num = random.Next(0, chromosomes.Count - 1);
            if (p2num == p1num)
            {
                goto crossingerr;
            }
            int[] child = new int[chromosomes[p1num].mass.Length];
            List<int> tmplist = new List<int>();
            tmplist.AddRange(chromosomes[p1num].mass.ToList());

            for (int i = 0; i < chromosomes[p1num].mass.Length; i++)
            {
                if (chromosomes[p1num].mass[i] == chromosomes[p2num].mass[i])
                {
                    child[i] = chromosomes[p1num].mass[i];
                    tmplist.Remove(child[i]);
                }
                else child[i] = -1;
            }
            for (int i = 0; i < chromosomes[p1num].mass.Length; i++)
            {
                if (child[i]==-1)
                {
                    int tmp = tmplist[random.Next(0, tmplist.Count - 1)];
                    child[i] = tmp;
                    tmplist.Remove(tmp);
                }
            }
            
            return child;
        }
        public void mutation(Chromosome chromosome)
        {
            int i, j;
            int tmp;
            i = random.Next(0, chromosome.mass.Length - 1);
        muterror: j = random.Next(0, chromosome.mass.Length - 1);
            if (j == i) goto muterror;
            tmp = chromosome.mass[i];
            chromosome.mass[i] = chromosome.mass[j];
            chromosome.mass[j] = tmp;
        }
        
        public int crossCheck(int[] mass)
        {
            int sum = 0;

            for(int i=0; i< mass.Length-1; i++)
            {
                for (int j = i+1; j < mass.Length; j++)
                {
                    if (j - i == Math.Abs(mass[j] - mass[i])) sum++;
                }
            }

            return sum;
        }
    }

}
