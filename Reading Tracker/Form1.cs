using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reading_Tracker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void RefreshList()
        {
            BookData bookData = new BookData();

            List<Book> books = bookData.LoadList();

            var bindingList = new BindingList<Book>(books);
            var source = new BindingSource(bindingList, null);
            dgvBooks.DataSource = bindingList;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BookData bookData = new BookData();
            bookData.VerifyDatabase();
            RefreshList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string bName = txtName.Text;
            string bLocation = txtLocation.Text;
            string bDescription = txtDescription.Text;

            BookData bookData = new BookData();
            bookData.AddItem(bName, bLocation, bDescription);

            RefreshList();
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvBooks.SelectedRows)
            {
                string name = row.Cells["nameDataGridViewTextBoxColumn"].Value.ToString();
                string location = row.Cells["locationDataGridViewTextBoxColumn"].Value.ToString();
                string description = row.Cells["descriptionDataGridViewTextBoxColumn"].Value.ToString();

                BookData bookData = new BookData();
                bookData.RemoveItem(name, location, description);

                dgvBooks.Rows.Remove(row);
            }
        }
    }
}
