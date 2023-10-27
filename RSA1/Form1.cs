using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RSA1
{
    public partial class Form1 : Form
    {
        OpenFileDialog openfile = new OpenFileDialog();
        OpenFileDialog openfile1 = new OpenFileDialog();
        SaveFileDialog savefile = new SaveFileDialog();
        SaveFileDialog savefile1 = new SaveFileDialog();
        bool flag1 = false;
        bool flag2 = false;
        bool flag3 = false;

        private long Obratnoed(long e_, long FE) //Закрытый ключ(Обратный закрытый ключ к открытому по модулю Функции Эйлера)
        {
            long a = e_, b = FE, bx = b, x0 = 1, x1 = 0, y1 = 1, y0 = 0, r, q, x = 0, y = 0;
            while (b != 0) //Расширенный Евклид
            {
                r = a % b;
                q = a / b;
                a = b;
                b = r;
                x = x0 - (x1 * q);
                y = y0 - (y1 * q);
                x0 = x1;
                y0 = y1;
                x1 = x;
                y1 = y;
            }
            if (x0 < 0)
            {
                x0 += bx;
            }
            return x0;
        }

        private string Rasshifrovka(string input, long d, long n)// Расшифровка методом RSA
        {
            string result = "";

            BigInteger x;
            string itog = "";
            bool f = true;

            for (int i = 0; i < richTextBox2.Text.Length; i++)
            {
                while (richTextBox2.Text[i] != ' ')
                {
                    itog += richTextBox2.Text[i];
                    i++;
                    if (i == richTextBox2.Text.Length)
                        break;
                }
                if (i == richTextBox2.Text.Length)
                    break;
                x = BigInteger.Parse(itog);
                itog = "";
                x = BigInteger.ModPow(x, d, n);

                Int64 b = (Int64)x;
                result = result + Convert.ToChar(b).ToString();
            }
            return result;
        }
        private string Zashifrovka(string s, long e_, long n)// Шифровка методом RSA
        {
            string result = "";

            BigInteger x;

            for (int i = 0; i < s.Length; i++)
            {
                x = new BigInteger(s[i]);
                x = BigInteger.ModPow(x, (int)e_, n);
                result += x.ToString() + " ";
            }
            return result;
        }
        private bool IsTheNumberSimple(BigInteger n) //Проверка на простое число
        {
            int k = 100;

            if (n == 2 || n == 3)
                return true;


            if (n < 2 || n % 2 == 0)
                return false;

            BigInteger t = n - 1;

            int s = 0;

            while (t % 2 == 0)
            {
                t /= 2;
                s += 1;
            }

            for (int i = 0; i < k; i++)
            { 
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                byte[] _a = new byte[n.ToByteArray().LongLength];

                BigInteger a;

                do
                {
                    rng.GetBytes(_a);
                    a = new BigInteger(_a);
                }
                while (a < 2 || a >= n - 2);

                BigInteger x = BigInteger.ModPow(a, t, n);

                if (x == 1 || x == n - 1)
                    continue;

                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, n);

                    if (x == 1)
                        return false;

                    if (x == n - 1)
                        break;
                }
                if (x != n - 1)
                    return false;
            }
            return true;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e) //Зашифровка
        {
            savefile.DefaultExt = ".txt";
            savefile.FileName = "out1";
            savefile.Filter = "|.txt";
            DialogResult res = MessageBox.Show("Сохранить файл?", "Сообщение", MessageBoxButtons.YesNo);
            if ((textBox1.Text.Length > 0) && (textBox2.Text.Length > 0))
            {
                long p = Convert.ToInt64(textBox1.Text);
                long q = Convert.ToInt64(textBox2.Text);

                if (IsTheNumberSimple(p) && IsTheNumberSimple(q))
                {
                    string s = " ";
                    if (flag1 == true)
                    {
                        StreamReader sr = new StreamReader(openfile.FileName);

                        while (!sr.EndOfStream)
                        {
                            s += sr.ReadLine();
                        }

                        sr.Close();
                        richTextBox1.Text = s;
                        flag1 = false;
                    }
                    s = richTextBox1.Text;
                    richTextBox1.Text = s;

                    long n = p * q; //Произведение простых чисел
                    long FE = (p - 1) * (q - 1); //Функция эйлера
                    long e_ = FE - 1; //Открытый ключ(Взаимно простое число с функцией Эйлера)
                    long d_ = Obratnoed(e_, FE); //Закрытый ключ(Обратный закрытый ключ к открытому по модулю Функции Эйлера)

                    string result = Zashifrovka(s, e_, n);
                    string itog = "";

                    if (res == DialogResult.Yes)
                    {
                        if (savefile.ShowDialog() == System.Windows.Forms.DialogResult.OK && savefile.FileName.Length > 0)
                        {
                            StreamWriter out1 = new StreamWriter(savefile.FileName, false);
                            for (int i = 0; i < result.Length; i++)
                            {

                                itog = itog + result[i];
                            }
                            out1.WriteLine(itog);
                            out1.Close();
                            flag3 = true;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < result.Length; i++)
                        {

                            itog = itog + result[i];
                        }
                    }
                    richTextBox2.Text = itog;
                    textBox4.Text = d_.ToString();
                    textBox3.Text = n.ToString();
                    textBox5.Text = e_.ToString();
                }
                else
                    MessageBox.Show("p или q - не простые числа!");
            }
            else
                MessageBox.Show("Введите p и q!");
        }
        private void button2_Click(object sender, EventArgs e)// Расшифровка
        {
            savefile1.DefaultExt = ".txt";
            savefile1.FileName = "out2";
            savefile1.Filter = "|.txt";
            DialogResult res = MessageBox.Show("Сохранить файл?", "Сообщение", MessageBoxButtons.YesNo);
            if ((textBox4.Text.Length > 0) && (textBox3.Text.Length > 0))
            {
                long d = Convert.ToInt64(textBox4.Text);
                long n = Convert.ToInt64(textBox3.Text);

                string input = "";

                if (flag3 == true)
                {
                    StreamReader out1 = new StreamReader(savefile.FileName);

                    while (!out1.EndOfStream)
                    {
                        input += out1.ReadLine();
                    }

                    out1.Close();
                    flag3 = false;
                }
                if (flag2 == true)
                {
                    StreamReader out1 = new StreamReader(openfile1.FileName);
                    while (!out1.EndOfStream)
                    {
                        input += out1.ReadLine();
                    }

                    out1.Close();
                    flag2 = false;
                }
                else
                {
                    foreach (var item in richTextBox2.Text)
                    {
                        input += item;
                    }
                }
                
                string result = Rasshifrovka(input, d, n);
                if (res == DialogResult.Yes)
                {
                    if (savefile1.ShowDialog() == System.Windows.Forms.DialogResult.OK && savefile1.FileName.Length > 0)
                    {
                        StreamWriter out2 = new StreamWriter(savefile1.FileName, false);
                        out2.WriteLine(result);
                        out2.Close();
                    }
                }
                richTextBox3.Text = result;

            }
            else
                MessageBox.Show("Введите секретный ключ!");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e) //Выбор файла с текстом для шифрования
        {
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Text = File.ReadAllText(openfile.FileName);
                flag1 = true;
            }
        }

        private void button4_Click(object sender, EventArgs e) //Выбор файла с последовательностью для расшифрования
        {

            if (openfile1.ShowDialog() == DialogResult.OK)
            {
                richTextBox2.Text = File.ReadAllText(openfile1.FileName);
                flag2 = true;
            }
        }
    }
}
