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

        private void button1_Click(object sender, EventArgs e)
        {
            GeneticAlgorithm gen = new GeneticAlgorithm();
            int N = Convert.ToInt32(textBox2.Text);
            Chromosome best = gen.algorithm(N, Convert.ToInt32(textBox3.Text), Convert.ToDouble(textBox5.Text), Convert.ToDouble(textBox6.Text));
            textBox1.Text = ("Хромосома: " + string.Join("", best.mass) + " количество циклов: " + best.colcikl);
            
            Graphics g = pictureBox1.CreateGraphics();
            g.Clear(Color.White);
            for (int j = 0; j < N; j ++)
            {
                for (int f = 0; f < N; f ++)
                {
                    g.DrawRectangle(Pens.Black, ((pictureBox1.Width / N) * f), ((pictureBox1.Height / N) * j), pictureBox1.Width / N, pictureBox1.Height / N);
                }

                g.FillRectangle(Brushes.Black, ((pictureBox1.Width / N))*best.mass[j], (pictureBox1.Height / N)*j, pictureBox1.Width / N, pictureBox1.Height / N);
            }
        }

        

    }
}
