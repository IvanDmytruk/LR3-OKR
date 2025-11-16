using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic; // для InputBox
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.DataFormats;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace LW3_OKR
{
    public partial class Form1 : Form
    {
        // Поточне замовлення (кошик)
        private Order currentOrder;

        public Form1()
        {
            InitializeComponent();
            button1.Font = new Font("Segoe UI Emoji", 12);
            button2.Font = new Font("Segoe UI Emoji", 12);
            button3.Font = new Font("Segoe UI Emoji", 12);
            button4.Font = new Font("Segoe UI Emoji", 12);
        }

        private void VivePersonal_Click(object sender, EventArgs e)
        {
            FVivePersonal fVivePersonal = new FVivePersonal();
            fVivePersonal.ShowDialog();
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                button.BackColor = Color.LightBlue;
            }
            if (button1 == button)
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
                button.BackColor = SystemColors.Control;
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

        // ====== БІЗНЕС-КЛАСИ ======

        public class Client
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }

            public Client(int id, string name, string phone, string email)
            {
                Id = id;
                Name = name;
                Phone = phone;
                Email = email;
            }

            public override string ToString()
            {
                return $"{Name} ({Phone})";
            }
        }

        public class Income
        {
            public DateTime Date { get; set; }
            public string Description { get; set; }
            public decimal Amount { get; set; }

            public Income(DateTime date, string description, decimal amount)
            {
                Date = date;
                Description = description;
                Amount = amount;
            }

            public override string ToString()
            {
                return $"{Date.ToShortDateString()} — {Description}: +{Amount} грн";
            }
        }

        public class Expense
        {
            public DateTime Date { get; set; }
            public string Category { get; set; }
            public decimal Amount { get; set; }

            public Expense(DateTime date, string category, decimal amount)
            {
                Date = date;
                Category = category;
                Amount = amount;
            }

            public override string ToString()
            {
                return $"{Date.ToShortDateString()} — {Category}: -{Amount} грн";
            }
        }

        public class Restaurant
        {
            public string Name { get; set; }
            public string Address { get; set; }
            private List<Personal> Employees { get; set; }
            private List<Income> Incomes { get; set; }
            private List<Expense> Expenses { get; set; }

            public Restaurant(string name, string address)
            {
                Name = name;
                Address = address;
                Employees = new List<Personal>();
                Incomes = new List<Income>();
                Expenses = new List<Expense>();
            }

            public void AddEmployee(Personal e) => Employees.Add(e);
            public void AddIncome(Income i) => Incomes.Add(i);
            public void AddExpense(Expense e) => Expenses.Add(e);

            public decimal GetTotalIncome() => Incomes.Sum(i => i.Amount);
            public decimal GetTotalExpense() => Expenses.Sum(e => e.Amount);
            public decimal GetProfit() => GetTotalIncome() - GetTotalExpense();

            public override string ToString()
            {
                return $"Ресторан: {Name}, адреса: {Address}, " +
                       $"працівників: {Employees.Count}, " +
                       $"прибуток: {GetProfit()} грн";
            }
        }

        public class RestaurantNetwork
        {
            public string NetworkName { get; set; }
            public List<Restaurant> Restaurants { get; set; }

            public RestaurantNetwork(string networkName)
            {
                NetworkName = networkName;
                Restaurants = new List<Restaurant>();
            }

            public void AddRestaurant(Restaurant r) => Restaurants.Add(r);

            public decimal GetTotalProfit()
            {
                return Restaurants.Sum(r => r.GetProfit());
            }

            public override string ToString()
            {
                return $"Мережа ресторанів \"{NetworkName}\", філій: {Restaurants.Count}, " +
                       $"загальний прибуток: {GetTotalProfit()} грн";
            }
        }

        // НОВЕ: клас поточного замовлення
        public class Order
        {
            public Client Client { get; set; }
            public List<Goods> Items { get; set; }
            public decimal Tips { get; set; }
            public DateTime Date { get; set; }

            public Order(Client client)
            {
                Client = client;
                Items = new List<Goods>();
                Date = DateTime.Now;
                Tips = 0;
            }

            // Додати товар в замовлення
            public void AddItem(Goods g) => Items.Add(g);

            // Сума страв (припускаю, що ціна = Quantity)
            public decimal GetItemsSum()
            {
                return Items.Sum(x => (decimal)x.Quantity);
            }

            // Повна сума = страви + чайові
            public decimal GetTotal()
            {
                return GetItemsSum() + Tips;
            }
        }

        // ====== Обробники кнопок меню ======

        private void button1_Click(object sender, EventArgs e)
        {
            LoadGoods("Sushi");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadGoods("Rols");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadSets();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadGoods("Drinks");
        }

        // ====== Завантаження товарів ======

        private void LoadGoods(string type)
        {
            flowGoods.Controls.Clear(); // очищення старих кнопок

            var client = new MongoClient("mongodb+srv://ivandmytruk42_db_user:lwokr123@db.rdcvntl.mongodb.net/?appName=DB");
            var db = client.GetDatabase("LW3_OKR_DB");
            var collection = db.GetCollection<Goods>("Goods");

            var filter = Builders<Goods>.Filter.Eq(g => g.Type, type);
            var goods = collection.Find(filter).ToList();

            foreach (var g in goods)
            {
                Button btn = new Button();
                btn.Width = 150;
                btn.Height = 60;

                // 🔹 Показуємо назву + ціну на кнопці
                btn.Text = $"{g.Name}\n{g.Quantity} грн";
                btn.Font = new Font("Segoe UI", 10);
                btn.BackColor = Color.WhiteSmoke;
                btn.FlatStyle = FlatStyle.Flat;

                // При натисканні:
                btn.Click += (s, e) =>
                {
                    // 1) Якщо замовлення ще не створене — створюємо для гостя
                    if (currentOrder == null)
                    {
                        var guest = new Client(1, "Гість", "000", "-");
                        currentOrder = new Order(guest);
                    }

                    // 2) Додаємо товар у поточне замовлення
                    currentOrder.AddItem(g);

                    // 3) Показуємо маленьке вікно з інформацією (як було раніше, але + фраза)
                    MessageBox.Show(
                        $"Товар: {g.Name}\nВартість: {g.Quantity} грн\n\nДодано до поточного замовлення.",
                        "Товар додано",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                };

                flowGoods.Controls.Add(btn);
            }
        }

        private void LoadSets()
        {
            flowGoods.Controls.Clear();

            var client = new MongoClient("mongodb+srv://ivandmytruk42_db_user:lwokr123@db.rdcvntl.mongodb.net/?appName=DB");
            var db = client.GetDatabase("LW3_OKR_DB");

            var setsCollection = db.GetCollection<Sets>("Sets");
            var goodsCollection = db.GetCollection<Goods>("Goods");

            var sets = setsCollection.Find(new BsonDocument()).ToList();

            foreach (var set in sets)
            {
                // Завантажуємо товари, які входять у сет
                var filter = Builders<Goods>.Filter.In(g => g.Id, set.GoodsIds);
                var goodsInSet = goodsCollection.Find(filter).ToList();

                // Формуємо текст для кнопки (імена товарів)
                string goodsList = string.Join(", ", goodsInSet.Select(g => g.Name));

                Button btn = new Button();
                btn.Width = 200;
                btn.Height = 80;
                btn.Font = new Font("Segoe UI", 10);
                btn.BackColor = Color.LightGoldenrodYellow;
                btn.FlatStyle = FlatStyle.Flat;

                btn.Text = $"{set.SetName}\n[{goodsList}]";

                btn.Click += (s, e) =>
                {
                    // Якщо замовлення ще немає тоді створюємо
                    if (currentOrder == null)
                    {
                        var guest = new Client(1, "Гість", "000", "-");
                        currentOrder = new Order(guest);
                    }

                    // Додаємо всі товари сета до замовлення
                    foreach (var g in goodsInSet)
                    {
                        currentOrder.AddItem(g);
                    }

                    // Рахуємо суму сета
                    decimal setSum = goodsInSet.Sum(x => (decimal)x.Quantity);

                    MessageBox.Show(
                        $"Сет: {set.SetName}\n\n" +
                        $"До складу входять:\n{string.Join("\n", goodsInSet.Select(g => "- " + g.Name))}\n\n" +
                        $"Сума сета: {setSum} грн\n\nДодано до поточного замовлення.",
                        "Сет додано",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                };

                flowGoods.Controls.Add(btn);
            }
        }

        // ====== ВІКНО ПОТОЧНОГО ЗАМОВЛЕННЯ + СКАСУВАННЯ ======

        private void ShowCurrentOrder()
        {
            if (currentOrder == null || currentOrder.Items.Count == 0)
            {
                MessageBox.Show("Поточне замовлення порожнє.", "Інформація",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Запитуємо/оновлюємо чайові
            string tipsStr = Interaction.InputBox(
                "Введіть чайові (грн, можна 0):",
                "Чайові",
                currentOrder.Tips.ToString()
            );

            if (decimal.TryParse(tipsStr, out decimal tips))
            {
                currentOrder.Tips = tips;
            }

            // Формуємо текст замовлення
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Поточне замовлення:");
            sb.AppendLine(new string('-', 30));

            foreach (var item in currentOrder.Items)
            {
                sb.AppendLine($"{item.Name} — {item.Quantity} грн");
            }

            sb.AppendLine(new string('-', 30));
            sb.AppendLine($"Сума страв: {currentOrder.GetItemsSum()} грн");
            sb.AppendLine($"Чайові: {currentOrder.Tips} грн");
            sb.AppendLine($"Разом: {currentOrder.GetTotal()} грн");
            sb.AppendLine();
            sb.AppendLine("Натисніть \"Так\", щоб скасувати поточне замовлення.");
            sb.AppendLine("Натисніть \"Ні\", щоб залишити замовлення.");

            var result = MessageBox.Show(
                sb.ToString(),
                "Поточне замовлення",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information
            );

            if (result == DialogResult.Yes)
            {
                currentOrder = null;
                MessageBox.Show("Поточне замовлення скасовано.", "Скасовано",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ПРИКЛАД обробника для кнопки "Поточне замовлення"
        // Створи на формі кнопку, назви її, наприклад, buttonCurrentOrder
        // і прив'яжи цей метод до події Click.
        private void buttonCurrentOrder_Click(object sender, EventArgs e)
        {
            ShowCurrentOrder();
        }
    }
}
