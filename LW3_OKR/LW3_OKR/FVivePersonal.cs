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
namespace LW3_OKR
{
    public partial class FVivePersonal : Form
    {
        private MongoDBPersonal db;
        private List<Personal> persons;
        int q = 4;//Це типу кількість "сушистів"
        int position = 0;//це позиція де ми зараз
        public FVivePersonal()
        {
            InitializeComponent();
            db = new MongoDBPersonal();
            persons = db.GetAllPersonals();
            
            Personal p = persons[0];

            label2.Text = "Ім'я: " + p.Name;
            label3.Text = "Прізвище: " + p.Name;
            label3.Text = "Прізвище: " + p.Position;
            label3.Text = "Прізвище: " + p.Stat;

            pictureBox1.Image = Image.FromFile(p.Image);
        }
        public void Refresh(int i)
        {
            Personal p = persons[i];

            label2.Text = "Ім'я: " + p.Name;
            label3.Text = "Прізвище: " + p.Name;
            label3.Text = "Прізвище: " + p.Position;
            label3.Text = "Прізвище: " + p.Stat;

            pictureBox1.Image = Image.FromFile(p.Image);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            position--;
            if(position==-1)
            {
                position = 3;
            }
            Refresh(position);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (position == q)
            {
                position = 0;
            }
            position++;
            Refresh(position);
        }
    }
}
