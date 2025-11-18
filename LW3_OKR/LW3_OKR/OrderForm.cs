using System;
using System.Linq;
using System.Windows.Forms;

namespace LW3_OKR
{
    public partial class OrderForm : Form
    {
        private Form1.Order order;

        public bool OrderCancelled { get; private set; } = false;

        public OrderForm(Form1.Order order)
        {
            InitializeComponent();
            this.order = order;
            LoadOrder();
        }

        private void LoadOrder()
        {
            listBoxItems.Items.Clear();

            foreach (var item in order.Items)
            {
                listBoxItems.Items.Add($"{item.Name} — {item.Quantity} грн");
            }

            lblItemsSum.Text = $"Сума страв: {order.GetItemsSum()} грн";
            lblTips.Text = $"Чайові: {order.Tips} грн";
            lblTotal.Text = $"Разом: {order.GetTotal()} грн";
        }

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Скасувати це замовлення?", "Підтвердження",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                OrderCancelled = true;
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
