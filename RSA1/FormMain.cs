using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RSA1
{
    public partial class FormMain : Form
    {
        private Form active;
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            PanelForm(new Form1());
        }

        private void PanelForm(Form form) //Вывод других форм
        {
            if (active != null)
            {
                active.Close(); 
            }
            active = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            this.panel1.Controls.Add(form);
            this.panel1.Tag = form;
            form.BringToFront();
            form.Show();
        }

        private void button2_Click(object sender, EventArgs e) //Вывод формы со справкой
        {
            PanelForm(new FormInfo());
        }

        private void button1_Click(object sender, EventArgs e) //Вывод формы со шифрованием/расшифрованием
        {
            PanelForm(new Form1());
        }

        private void button3_Click(object sender, EventArgs e) //Вывод формы со описанием алгоритма RSA
        {
            PanelForm(new FormDesc());
        }
    }
}
