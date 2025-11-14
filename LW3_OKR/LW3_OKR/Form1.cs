using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.DataFormats;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
namespace LW3_OKR
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //mongoService = new MongoService();
            //LoadData();
            button1.Font = new Font("Segoe UI Emoji", 12);
            button2.Font = new Font("Segoe UI Emoji", 12);
            button3.Font = new Font("Segoe UI Emoji", 12);
            button4.Font = new Font("Segoe UI Emoji", 12);
        }
        //void LoadData()
        //{
        //    var client = new MongoClient("mongodb+srv://<ivandmytruk42_db_user>:<xd7NiRFVNU2atx5e>@formia.awxcqul.mongodb.net/?appName=ForMiA");
        //    var database = client.GetDatabase("LW3_OKR");
        //    var collection = database.GetCollection<Orders>("Orders");
        //    var orders = collection.Find(new BsonDocument()).ToList();
        //}

        private void VivePersonal_Click(object sender, EventArgs e)
        {
            FVivePersonal fVivePersonal = new FVivePersonal();
            fVivePersonal.ShowDialog();
            //LoadData();
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                button.BackColor = Color.LightBlue;
            }
            if(button1==button)
            {
                button.Text = "🍣";
            }
            if (button2 == button)
            {
                button.Text = "🍱";
            }
            if (button3 == button)
            {
                button.Text = "🍙";
            }
            if (button4 == button)
            {
                button.Text = "🥤";
            }

        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                button.BackColor =  SystemColors.Control;
            }
            if (button1 == button)
            {
                button.Text = "Суші";
            }
            if (button2 == button)
            {
                button.Text = "Сети";
            }
            if (button3 == button)
            {
                button.Text = "Роли";
            }
            if (button4 == button)
            {
                button.Text = "Напої";
            }
        }
    }
}
