using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЭМО_лабораторная_7
{
    class Ant
    {
        public List<int> daway;
        public bool colch;
        public bool[] vertexes;

        public Ant()
        {
            daway = new List<int>();
            colch = true;
        }

        public void setvert(int colv)
        {
            vertexes = new bool[colv];

            for (int i = 0; i < colv; i++)
            {
                vertexes[i] = false;
            }
        }
        public string toString()
        {
            string str = "";
            foreach(var v in daway)
            {
                str += Convert.ToString(v);
            }
            return str;
        }
    }

    class AntAlgorithm
    {
        private double isparenie;
        public double[,] feromons;
        public List<Ant> ants;
        public Random rand;

        public AntAlgorithm()
        {
            isparenie = 0.25;
            ants = new List<Ant>();
            rand = new Random();
        }

        public void algorithminit(int colf)
        {
            feromons = new double[colf, colf];
            

            for (int i = 0; i < colf; i++)
            {
                for (int j = 0; j < colf; j++)
                {
                    if (Form1.Graph[i, j] == 1)
                    {
                        feromons[i, j] = 1;
                    }
                }
            }

            for (int i = 0; i < colf; i++)
            {
                ants.Add(new Ant());
            }

        }

        public void maincikl(int coliter)
        {
            bool t1;
            double sum;
            List<double> chance = new List<double>();
            List<int> chancenum = new List<int>();
            int tmpv = 0;
            for (int i = 0; i < coliter; i++)
            {
                foreach (var ant in ants)
                {
                    ant.daway.Clear();
                    t1 = false;
                    ant.daway.Add(0);
                    ant.colch = true;
                    ant.setvert(Convert.ToInt32(Math.Sqrt(feromons.Length)));

                    do
                    {
                        sum = 0;
                        chance.Clear();
                        chancenum.Clear();
                        for (int j = 0; j < Math.Sqrt(feromons.Length); j++)
                        {
                            sum += feromons[ant.daway.Last(), j];//Form1.Graph[ant.daway.Last(), j];
                            if (Form1.Graph[ant.daway.Last(), j] > 0)
                            {
                                chancenum.Add(j);
                            }
                        }
                        if (chancenum.Count > 0)
                        {
                            if (chancenum.Count > 1)
                            {
                                chance.Add(feromons[ant.daway.Last(), chancenum[0]] / sum);
                                for (int j = 1; j < chancenum.Count; j++)
                                {
                                    chance.Add(feromons[ant.daway.Last(), chancenum[j]] / sum);
                                    chance[j] += chance[j - 1];
                                }
                                double tmp = rand.NextDouble();
                                for (int j = 0; j < chance.Count; j++)
                                {
                                    if (tmp <= chance[j])
                                    { ant.daway.Add(chancenum[j]);
                                        break;

                                    }

                                    
                                }
                                tmpv = ant.daway.Last();
                                ant.vertexes[tmpv] = true;
                            }
                            else
                            {
                                ant.daway.Add(chancenum.Last());
                                tmpv = ant.daway.Last();
                                ant.vertexes[tmpv] = true;
                            }
                        }
                        else
                        {
                            ant.colch = false;
                            break;
                        }
                        //tmpv = ant.daway.Last();
                        //ant.vertexes[tmpv] = true;
                        //tmpv = 0;
                    }
                    while (ant.vertexes.Contains(false));
                }
                addferomons();
            }
        }

        private void addferomons()
        {
            for(int i =0; i<Math.Sqrt(feromons.Length); i++)
            {
                for (int j = 0; j < Math.Sqrt(feromons.Length); j++)
                {
                    if (feromons[i, j] > 0) feromons[i, j] *= 0.5;
                }
            }
            double timeferomon;
            foreach (var element in ants)
            {
                double asd = element.daway.Count;
                timeferomon = (1.0/asd);
                for (int i = 1; i < element.daway.Count; i++)
                {
                    feromons[element.daway[i - 1], element.daway[i]] += timeferomon;
                }
            }
        }
    }
}
