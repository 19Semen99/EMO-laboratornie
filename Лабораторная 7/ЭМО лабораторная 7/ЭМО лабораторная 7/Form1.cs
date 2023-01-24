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

namespace ЭМО_лабораторная_7
{
    public partial class Form1 : Form
    {
        public static int[,] Graph = new int[5, 5];
        int kol_ant;
        AntAlgorithm alg = new AntAlgorithm();

        public Form1()
        {
            InitializeComponent();
            kol_ant = Convert.ToInt32(this.textBox1.Text);


            using (StreamReader reader = new StreamReader("Graph.txt"))
            {
                string line;
                string[] coll;
                int counter = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    coll = line.Split(' ');
                    this.dataGridView1.Rows.Add();
                    //this.dataGridView1.Rows[counter].HeaderCell.Value = counter;
                    //Debug.Print(line);
                    for (int i = 0; i < coll.Length; i++)
                    {
                        if (int.Parse(coll[i]) > 0)
                            Graph[counter, i] = 1;
                        else
                            Graph[counter, i] = 0;

                        this.dataGridView1[i, counter].Value = Graph[counter, i];
                    }
                    counter++;
                }
                alg.algorithminit(counter);
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            this.button2.Enabled = true;
            this.textBox1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            kol_ant = Convert.ToInt32(this.textBox1.Text);
            this.button2.Enabled = false;
            this.textBox1.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            alg.maincikl(Convert.ToInt32(textBox2.Text));
            int best = 0;
            int tmpbestcol = alg.ants[0].daway.Count;
            for (int i = 1; i < alg.ants.Count; i++)
            {
                if (alg.ants[i].daway.Count < tmpbestcol)
                {
                    best = i;
                    tmpbestcol = alg.ants[i].daway.Count;
                }
            }
            //string str = alg.ants[best].daway.ToString();
            label5.Text = "Лучший путь: " + alg.ants[best].toString();
            for(int i=0; i<Math.Sqrt(alg.feromons.Length); i++)
            {
                this.dataGridView2.Rows.Add();
                for (int j = 0; j < Math.Sqrt(alg.feromons.Length); j++)
                {
                    this.dataGridView2[j, i].Value = alg.feromons[i, j];
                }
            }
            label4.Text = Convert.ToString(Convert.ToInt32(label4.Text) + 1);
        }
    }
}
