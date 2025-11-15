using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Driver;
using System.IO;
namespace LW3_OKR
{
    public partial class FVivePersonal : Form
    {
        private MongoDBPersonal db;
        private List<Personal> persons;
        private int q;
        private int position = 0;

        public FVivePersonal()
        {
            InitializeComponent();
            db = new MongoDBPersonal();
            persons = db.GetAllPersonals();
            q = persons.Count;

            if (q == 0)
            {
                MessageBox.Show("Немає персоналу в базі!");
                return;
            }

            ShowPerson(0);
        }
        public void ShowPerson(int i)
        {
            Personal p = persons[i];

            label2.Text = "Ім'я: " + p.Name;
            label3.Text = "Прізвище: " + p.SurName;
            label4.Text = "Посада: " + p.Position;
            label5.Text = "Стать: " + p.Stat;

            string fullPath = Path.Combine(Application.StartupPath, "Images", p.Image);

            if (File.Exists(fullPath))
                pictureBox1.Image = Image.FromFile(fullPath);
            else
            {
                MessageBox.Show("NO FILE FOUND at: " + fullPath);
                pictureBox1.Image = null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            position--;
            if (position < 0)
                position = q - 1;

            ShowPerson(position);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            position++;
            if (position >= q)
                position = 0;

            ShowPerson(position);
        }
    }
}
