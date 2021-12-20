using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shoting
{
    public partial class Form1 : Form
    {
        Point[] p = new Point[1000]; // массив точек - не более 1000
        int n_fakt; // фактическое число пробоин
        int Rmin = 0; // Результат - минимальный радиус
        Graphics graph1; // Графический контент - холст

        // конструктор
        public Form1()
        {
            InitializeComponent();
            graph1 = this.pictureBox1.CreateGraphics();
        }

        // стрельба
        public void shots(Point[] p, int N, int D, int W, int R, int K)
        {
            Random ran = new Random();
            for (int i = 0; i < N; i++)
            {
                double r = R * (11 - K) / 10 * ran.NextDouble();
                double fi = 2 * Math.PI * ran.NextDouble();
                p[i].X = Convert.ToInt32(W * Math.Cos(D * Math.PI / 180) + r * Math.Cos(fi));
                p[i].Y = Convert.ToInt32(W * Math.Sin(R * Math.PI / 180) + r * Math.Sin(fi));
            }
        }
        // Нахождение минимального радиуса и "номера" точки
        private int MinRad(Point[] p, int n, out int t)
        {
            double R = 0.0; // минимальный радиус
            double R1; // радиус точки (x,y)
            t = -1; // номер точки
                    // Цикл по всем точкам, нахождение радиуса R
            for (int i = 0; i < n; i++)
            {
                R1 = Math.Sqrt(p[i].X * p[i].X + p[i].Y * p[i].Y);
                if (R1 > R)
                {
                    R = R1;
                    t = i;
                }
            }
            return Convert.ToInt32(R); // Минимальный радиус
        }

        // Запуск стрельбы по мишени и расчет минимального радиуса круга, содержащего все попадания 
        private void button1_Click(object sender, EventArgs e)
        {
            // Задание параметров:
            RIN n = new RIN(textBox1.Text, 1, 1000, 20, "число пуль");
            n_fakt = n.N;
            textBox1.Text = n.N.ToString();
            if (!n.n_bool)
                MessageBox.Show(n.info);
            RIN d = new RIN(textBox2.Text, 0, 360, 180, "направление ветра");
            if (!d.n_bool)
            {
                MessageBox.Show(d.info);
                textBox2.Text = d.N.ToString();
            }
            RIN w = new RIN(textBox3.Text, 0, 20, 20, "скорость ветра");
            if (!w.n_bool)
            {
                MessageBox.Show(w.info);
                textBox3.Text = w.N.ToString();
            }
            RIN r = new RIN(textBox4.Text, 100, 200, 200, "радиус мишени");
            if (!r.n_bool)
            {
                MessageBox.Show(r.info);
                textBox4.Text = r.N.ToString();
            }
            RIN k = new RIN(textBox5.Text, 1, 10, 3, "кучность стрельбы");
            if (!k.n_bool)
            {
                MessageBox.Show(k.info);
                textBox5.Text = k.N.ToString();
            }
            // Заполнение мишени случайными пробоинами
            shots(p, n.N, d.N, w.N, r.N, k.N);
            // Вывод массива точек (x,y) в ListBox1
            listBox1.Items.Clear();
            for (int i = 0; i < n.N; i++)
                listBox1.Items.Add(p[i].X.ToString() + " " + p[i].Y.ToString());
            // Нахождение минимального радиуса и "номера" точки k
            int t = -1;
            Rmin = MinRad(p, n.N, out t);
            // Вывод результата в ListBox2
            listBox2.Items.Clear();
            listBox2.Items.Add("Результат :");
            listBox2.Items.Add("Радиус максимального круга = " + Rmin.ToString());
            //  Для контроля:
            listBox2.Items.Add("Cамая удаленная точка № " + (t + 1).ToString() + " x = " + p[t].X.ToString() + " y = " + p[t].Y.ToString());
            int grad = d.N;
            if (grad < 180)
                grad += 180;
            else
                grad -= 180;
            listBox2.Items.Add("Нужна поправка " + w.N + " на " + grad.ToString() + " градусов");
        }

        // Графическая иллюстрация
        private void button2_Click(object sender, EventArgs e)
        {
            // инструменты
            Pen Pen1 = new Pen(Color.Green, 2); // Линии
            Pen Pen2 = new Pen(Color.Red, 1); // Окружность
            Pen Pen3 = new Pen(Color.Blue, 1); // Точки
            Pen Pen4 = new Pen(Color.Black, 1); // Мишень
            SolidBrush brush1 = new SolidBrush(Color.Black); // Текст меток на осях
            Font font1 = new Font("Arial", 10); // Шрифт и размер меток
                                                // Связывание холста с pictureBox
            graph1 = this.pictureBox1.CreateGraphics();
            // Отметки на координатных осях
            Single X, Y;
            for (X = -200; X <= 200; X += 50)
                graph1.DrawString(X.ToString(), font1, brush1, X + 200, 200);
            for (Y = -200; Y <= 200; Y += 50)
                graph1.DrawString(Y.ToString(), font1, brush1, 200, 200 - Y);
            // Преобразование компьютерной системы координат в математическую
            // Поворот оси Y
            graph1.ScaleTransform(1, -1);
            // Сдвиг по осям X и Y
            graph1.TranslateTransform(200, -200);
            // Рисование осей математической системы координат
            // Ось X
            graph1.DrawLine(Pen1, -200, 0, 200, 0);
            // Ось Y
            graph1.DrawLine(Pen1, 0, -200, 0, 200);
            // Делаем засечки по осям координат
            for (X = -200; X <= 200; X += 50)
                graph1.DrawLine(Pen1, X, -5, X, 5);
            for (Y = -200; Y <= 200; Y += 50)
                graph1.DrawLine(Pen1, -5, Y, 5, Y);
            // концентрические круги: 10, 9, 8, ... , 1 (подсчет очков)
            int rm = Convert.ToInt32(textBox4.Text);
            double dr = rm / 10;
            double rz = 0;
            for (int i = 0; i < 10; i++)
            {
                rz += dr;
                int riz = (int)rz;
                graph1.DrawEllipse(Pen4, -riz, -riz, 2 * riz, 2 * riz);
            }
            // Рисуем точки и окружность минимального радиуса
            int n = n_fakt;
            for (int i = 0; i < n; i++)
                graph1.FillEllipse(new SolidBrush(Color.BlueViolet), p[i].X - 3, p[i].Y - 3, 6, 6);
            graph1.DrawEllipse(Pen2, -Rmin, -Rmin, 2 * Rmin, 2 * Rmin);
        }
        // Очистка холста
        private void button3_Click(object sender, EventArgs e)
        {
            graph1.Clear(Color.White);
        }
    }  // end class Form1
       // Здесь должно быть описание класса RIN
}
