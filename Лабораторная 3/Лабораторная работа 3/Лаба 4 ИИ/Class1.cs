using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Лаба_4_ИИ
{
    class City
    {
        public int number;
        public int x;
        public int y;
        public City()
        {
            x = 0;
            y = 0;
            number = 0;
        }
        public City(int xx, int yy, int numb)
        {
            x = xx;
            y = yy;
            number = numb;
        }
    }
    class Chromosome
    {
        public int[] mass;
        public int[] sortedmass;
        public double length;
        public Chromosome(int col, int[] mass2, int[] smass)
        {
            mass = new int[col];
            for (int i = 0; i < col; i++)
            {
                mass[i] = mass2[i];
            }
            sortedmass = new int[col];
            smass.CopyTo(sortedmass, 0);
            int a = 0;
        }
        public Chromosome()
        {
            mass = new int[48];
        }
    }

    class GeneticAlgorithm
    {
        public City[] cities;
        public List<Chromosome> chromosomes;
        public StringBuilder textAll = new StringBuilder();

        private Random rand = new Random();
        public GeneticAlgorithm()
        {

        }
        public void init(int colc, int colh, int[,] mass2)
        {
            chromosomes = new List<Chromosome>();
            List<int> list = new List<int>();
            cities = new City[colc];
            for (int i = 0; i < colc; i++)
            {
                cities[i] = new City();
                cities[i].number = mass2[i, 0];
                cities[i].x = mass2[i, 1];
                cities[i].y = mass2[i, 2];
                list.Add(mass2[i, 0]);
                //    mascity[i] = true;
            }
            chromosomes = new List<Chromosome>();
            int[] cline = new int[colc];
            int tmp, tmp2;
            List<int> tmplist = new List<int>();
            //List<int> tmplist2 = list;
            for (int i = 0; i < colh; i++)
            {
                tmplist.AddRange(list);
                for (int j = 0; j < colc; j++)
                {
                l1: tmp = tmplist[rand.Next(0, colc - j)];
                    if ((tmp == j + 1) && (tmplist.Count > 1)) goto l1;
                    cline[j] = tmp;
                    tmplist.Remove(cline[j]);
                }

                //int[] massprov = new int[colc];
                //tmplist.Clear();

                //massprov[0] = 1;
                //tmplist.AddRange(list);
                //tmplist.Remove(1);
                //for (int j = 0; j < colc - 1; j++)
                //{
                //    tmp = cline[massprov[j] - 1];
                //    if (massprov.Contains(tmp))
                //    {
                //    l2: int randomm = rand.Next(0, colc - j - 1);
                //        tmp = tmplist[randomm];
                //        if (massprov.Contains(tmp))
                //        {
                //            goto l2;
                //        }
                //        else
                //        {
                //            tmp2 = cline[massprov[j] - 1];
                //            cline[massprov[j] - 1] = tmp;
                //            cline[Array.IndexOf(cline, tmp)] = tmp2;
                //        }
                //    }
                //    massprov[j + 1] = tmp;
                //    tmplist.Remove(tmp);
                //}
                chromosomes.Add(road(list, cline));
                chromosomes[i].length = count_length(chromosomes[i]);

                textAll.Append(" Хромосома №" + (i + 1) + " длина: " + chromosomes[i].length + Environment.NewLine);
            }
            //Chromosome best = chromosomes.Find(Chromosome => Chromosome.length == chromosomes.Min(c => c.length));
            textAll.Append(" Лучшая длина: " + chromosomes.Min(c => c.length) + Environment.NewLine);
        }
        public Chromosome road(List<int> list, int[] cline)
        {
            int[] massprov = new int[cities.Length];
            List<int> tmplist = new List<int>();
            int tmp, tmp2;
            massprov[0] = 1;
            tmplist.AddRange(list);
            tmplist.Remove(1);
            for (int j = 0; j < cities.Length - 1; j++)
            {
                tmp = cline[massprov[j] - 1];
                if (massprov.Contains(tmp))
                {
                l2: int randomm = rand.Next(0, cities.Length - j - 1);
                    tmp = tmplist[randomm];
                    if (massprov.Contains(tmp))
                    {
                        goto l2;
                    }
                    else
                    {
                        tmp2 = cline[massprov[j] - 1];
                        cline[massprov[j] - 1] = tmp;
                        cline[Array.IndexOf(cline, tmp)] = tmp2;
                    }
                }
                massprov[j + 1] = tmp;
                tmplist.Remove(tmp);
            }
            return new Chromosome(cline.Length, cline, massprov);
        }
        public List<Chromosome> algorithm(int colsteps, double crosschans, double mutchans)
        {
            List<Chromosome> newgen = new List<Chromosome>();
            for (int i = 0; i < colsteps; i++)
            {
                for (int j = 0; j < chromosomes.Count; j++)
                {
                    if (rand.NextDouble() <= crosschans)
                    {
                        int[] abc = crossingover(j, rand.Next(0, chromosomes.Count - 1));
                        newgen.Add(road(chromosomes[0].mass.ToList(), abc));
                        newgen.Last().length = count_length(newgen.Last());
                        ;
                    }
                    if (rand.NextDouble() <= mutchans)
                    {
                        //mutation(chromosomes[j]);
                    }
                }
                chromosomes.AddRange(newgen);
                Chromosome temp;
                for (int k = 0; k < chromosomes.Count; k++)
                {
                    for (int l = i + 1; l < chromosomes.Count; l++)
                    {
                        if (chromosomes[k].length > chromosomes[l].length)
                        {
                            temp = chromosomes[k];
                            chromosomes[k] = chromosomes[l];
                            chromosomes[l] = temp;
                        }
                    }
                }
                for (int k = chromosomes.Count - newgen.Count; k < chromosomes.Count;)
                {
                    chromosomes.RemoveAt(k);
                }
                newgen.Clear();
                //chromosomes = tour(chromosomes, chromosomes.Count - newgen.Count);

            }
            return chromosomes;
        }

        public int[] crossingover(int num1, int num2)
        {
            Chromosome parent1 = chromosomes[num1];
            Chromosome parent2 = chromosomes[num2];

            int[] masschild1 = new int[parent1.mass.Length];
            int[] masschild2 = new int[parent2.mass.Length];

            List<int> tmpcities1 = new List<int>();
            tmpcities1.AddRange(parent1.mass.ToList());
            List<int> tmpcities2 = new List<int>();
            tmpcities2.AddRange(parent2.mass.ToList());
            //List<int> tmpcities11 = new List<int>();
            //tmpcities1.AddRange(parent1.mass.ToList());
            //List<int> tmpcities22 = new List<int>();
            //tmpcities2.AddRange(parent2.mass.ToList());

            //int tmpchild1;
            //int tmpchild2;
            double tmpchild1;
            double tmpchild2;

            for (int i = 0; i < parent1.mass.Length; i++)
            {
                masschild1[i] = -1;
            }

            //masschild1[0] = parent1.mass[0];
            //masschild2[0] = parent2.mass[0];
            //tmpcities1.Remove(masschild1[0]);
            //tmpcities2.Remove(masschild2[0]);

            masschild1[0] = parent1.mass[0];
            int num = 0;
            tmpcities1.Remove(masschild1[0]);
            tmpcities1.Remove(1);
            //tmpcities2.Remove(masschild1[0]);
            while (tmpcities1.Count > 0)
            {
                if (parent1.mass[masschild1[num] - 1] == 1 || parent2.mass[masschild1[num] - 1] == 1)
                {
                    int abc;
                crosserror: abc = tmpcities1[rand.Next(0, tmpcities1.Count)];
                    if (abc == masschild1[num]) goto crosserror;
                    if (masschild1[abc - 1] != -1) goto crosserror;
                    masschild1[masschild1[num] - 1] = abc;
                    tmpcities1.Remove(masschild1[masschild1[num] - 1]);
                    num = masschild1[num] - 1;
                }
                else if (!masschild1.Contains(parent1.mass[masschild1[num] - 1]) && !masschild1.Contains(parent2.mass[masschild1[num] - 1]))
                {
                    tmpchild1 = Math.Sqrt(Math.Pow((cities[masschild1[num] - 1].x - cities[parent1.mass[masschild1[num] - 1] - 1].x), 2)
                      + Math.Pow((cities[masschild1[num] - 1].y - cities[parent1.mass[masschild1[num] - 1] - 1].y), 2));
                    tmpchild2 = Math.Sqrt(Math.Pow((cities[masschild1[num] - 1].x - cities[parent2.mass[masschild1[num] - 1] - 1].x), 2)
                        + Math.Pow((cities[masschild1[num] - 1].y - cities[parent2.mass[masschild1[num] - 1] - 1].y), 2));
                    if (tmpchild1 <= tmpchild2)
                    {
                        masschild1[masschild1[num] - 1] = parent1.mass[masschild1[num] - 1];
                        tmpcities1.Remove(masschild1[masschild1[num] - 1]);
                        num = masschild1[num] - 1;
                    }
                    if (tmpchild1 > tmpchild2)
                    {
                        masschild1[masschild1[num] - 1] = parent2.mass[masschild1[num] - 1];
                        tmpcities1.Remove(masschild1[masschild1[num] - 1]);
                        num = masschild1[num] - 1;
                    }
                }
                else
                {
                    if (!masschild1.Contains(parent1.mass[masschild1[num] - 1]))
                    {
                        masschild1[masschild1[num] - 1] = parent1.mass[masschild1[num] - 1];
                        tmpcities1.Remove(masschild1[masschild1[num] - 1]);
                        num = masschild1[num] - 1;
                    }
                    else
                    {
                        if (!masschild1.Contains(parent2.mass[masschild1[num] - 1]))
                        {
                            masschild1[masschild1[num] - 1] = parent2.mass[masschild1[num] - 1];
                            tmpcities1.Remove(masschild1[masschild1[num] - 1]);
                            num = masschild1[num] - 1;
                        }
                        else
                        {
                            if (tmpcities1.Count == 1)
                            {
                                masschild1[masschild1[num] - 1] = tmpcities1[0];
                                tmpcities1.Remove(masschild1[masschild1[num] - 1]);
                                num = masschild1[num] - 1;
                            }
                            else
                            {
                                int abc;
                            crosserror: abc = tmpcities1[rand.Next(0, tmpcities1.Count)];
                                if (abc == masschild1[num]) goto crosserror;
                                if (masschild1[abc - 1] != -1) goto crosserror;
                                masschild1[masschild1[num] - 1] = abc;
                                tmpcities1.Remove(masschild1[masschild1[num] - 1]);
                                num = masschild1[num] - 1;
                            }
                        }
                    }
                }
            }
            if (masschild1.Contains(-1))
            {
                masschild1[Array.IndexOf(masschild1, -1)] = 1;
            }
            return masschild1;
            //for (int i = 0; i < parent1.mass.Length; i++)
            //{
            //    if (i % 2 != 0)
            //    {
            //        tmpchild1 = parent1.mass[i];
            //        tmpchild2 = parent2.mass[i];

            //    l31: if (masschild1.Contains(tmpchild1))
            //        {
            //            tmpchild1 = tmpcities1[rnd.Next(0, tmpcities1.Count - 1)];
            //            goto l31;
            //        }
            //        masschild1[i] = tmpchild1;
            //        tmpcities1.Remove(tmpchild1);

            //    l41: if (masschild2.Contains(tmpchild2))
            //        {
            //            tmpchild2 = tmpcities2[rnd.Next(0, tmpcities2.Count - 1)];
            //            goto l41;
            //        }
            //        masschild2[i] = tmpchild2;
            //        tmpcities2.Remove(tmpchild2);

            //        //tmpcities1.Remove(tmpchild2);
            //        //tmpcities2.Remove(tmpchild1);
            //    }
            //    if (i % 2 == 0)
            //    {
            //        tmpchild1 = parent2.mass[i];
            //        tmpchild2 = parent1.mass[i];

            //    l32: if (masschild1.Contains(tmpchild1))
            //        {
            //            tmpchild1 = tmpcities1[rnd.Next(0, tmpcities2.Count - 1)];
            //            goto l32;
            //        }
            //        masschild1[i] = tmpchild1;
            //        tmpcities1.Remove(tmpchild1);

            //    l42: if (masschild2.Contains(tmpchild2))
            //        {
            //            tmpchild2 = tmpcities2[rnd.Next(0, tmpcities1.Count - 1)];
            //            goto l42;
            //        }
            //        masschild2[i] = tmpchild2;
            //        tmpcities2.Remove(tmpchild2);

            //        //tmpcities1.Remove(tmpchild2);
            //        //tmpcities2.Remove(tmpchild1);
            //    }
            //}
            chromosomes[num1] = road(parent1.mass.ToList(), masschild1);
            //chromosomes[num1].length = count_length(chromosomes[num1]);
            //chromosomes[num2] = road(parent2.mass.ToList(), masschild2);
            //chromosomes[num2].length = count_length(chromosomes[num2]);
        }
        public void mutation(Chromosome chromosome)
        {
            int mut = rand.Next(0, chromosome.mass.Length - 1);
            int[] massprov = new int[cities.Length];
            List<int> tmplist = new List<int>();
            int tmp, tmp2;
            massprov[0] = 1;
            tmplist.AddRange(chromosome.mass.ToList());
            for (int i = 0; i < mut; i++)
            {
                tmplist.Remove(chromosome.mass[i]);
            }
            tmplist.Remove(1);
            for (int j = mut; j < cities.Length - 1; j++)
            {
                tmp = chromosome.mass[massprov[j] - 1];
                if (massprov.Contains(tmp))
                {
                l2: int randomm = rand.Next(0, cities.Length - j - 1);
                    tmp = tmplist[randomm];
                    if (massprov.Contains(tmp))
                    {
                        goto l2;
                    }
                    else
                    {
                        tmp2 = chromosome.mass[massprov[j] - 1];
                        chromosome.mass[massprov[j] - 1] = tmp;
                        chromosome.mass[Array.IndexOf(chromosome.mass, tmp)] = tmp2;
                    }
                }
                massprov[j + 1] = tmp;
                tmplist.Remove(tmp);
            }
        }
        public List<Chromosome> tour(List<Chromosome> n, int diff)
        {
            List<Chromosome> newgen = new List<Chromosome>();
            //newgen.AddRange(n);

            int tmpn, bestn = 0;
            double tmpl, bestl = n[0].length;

            for (int i = 0; i < diff; i++)
            {
                bestn = rand.Next(0, diff - 1); ;
                bestl = n[bestn].length;
                for (int j = 0; j < 10; j++)
                {
                    tmpn = rand.Next(0, diff - 1);
                    if (n[tmpn].length < bestl)
                    {
                        bestn = tmpn;
                        bestl = n[tmpn].length;
                    }
                }
                newgen.Add(n[bestn]);
            }
            return newgen;
        }
        public double count_length(Chromosome chr)
        {
            double sum = 0;
            for (int i = 0; i < chr.mass.Length - 1; i++)
            {
                sum += Math.Sqrt(Math.Pow((cities[chr.sortedmass[i + 1] - 1].x - cities[chr.sortedmass[i] - 1].x), 2)
                    + Math.Pow((cities[chr.sortedmass[i + 1] - 1].y - cities[chr.sortedmass[i] - 1].y), 2));
            }
            sum += Math.Sqrt(Math.Pow((cities[chr.sortedmass[chr.mass.Length - 1] - 1].x - cities[chr.sortedmass[0] - 1].x), 2)
                    + Math.Pow((cities[chr.sortedmass[chr.mass.Length - 1] - 1].y - cities[chr.sortedmass[0] - 1].y), 2));
            return sum;
        }
    }

}
