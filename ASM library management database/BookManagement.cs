using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ASM_library_management_database
{
    public partial class BookManagement : Form
    {
        SqlConnection connection;

        public BookManagement()
        {
            InitializeComponent();
            connection = new SqlConnection("Data Source=DESKTOP-NB4LK8U\\SQLEXPRESS;Initial Catalog=library;Integrated Security=True");
        }
        public BookManagement(string username)
        {
            InitializeComponent();
            connection = new SqlConnection("Data Source=DESKTOP-NB4LK8U\\SQLEXPRESS;Initial Catalog=library;Integrated Security=True");

        }
        private void BookManagement_Load(object sender, EventArgs e)
        {
            connection.Open();
            Filldata();
            GetCategories();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
        public void Filldata()
        {
            string query = "select * from books";
            DataTable tbl = new DataTable();
            SqlDataAdapter ad = new SqlDataAdapter(query, connection);
            ad.Fill(tbl);
            dgvBookManagement.DataSource = tbl;
            connection.Close();
        }
        private void tabPage2_Click(object sender, EventArgs e)
        {

        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm login = new LoginForm();
            login.ShowDialog();
            this.Dispose();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if ((MessageBox.Show(this, "Do you want to exit?", " Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
            {
                Application.Exit();
            }
        }
        public void GetCategories()
        {
            string query = "select category_id, category_name from categories";
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            adapter.Fill(table);
            cbxCategoryId.DataSource = table;
            cbxCategoryId.DisplayMember = "category_name";
            cbxCategoryId.ValueMember = "category_id";
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            int error = 0;
            string BookID = txbBookId.Text;
            if (BookID.Equals(""))
            {
                error = error + 1;
                lblBookIdError.Text = "Book ID can not be blank";
            }
            else
                lblBookIdError.Text = "";

            string BookName = txbBookName.Text;
            if (BookName.Equals(""))
            {
                error = error + 1;
                lblBookNameError.Text = "Book Name can not be blank";
            }
            else
                lblBookNameError.Text = "";

            string Title = txbTitle.Text;
            if (Title.Equals(""))
            {
                error = error + 1;
                lblTitleError.Text = "Title can not be blank";
            }
            else
                lblTitleError.Text = "";

            string AuthorID = txbAuthorId.Text;
            if (AuthorID.Equals(""))
            {
                error = error + 1;
                lblAuthorIdError.Text = "Author can not be blank";
            }
            else
                lblAuthorIdError.Text = "";

            string Quantity = lblQuantityError.Text;
            if (Quantity.Equals(""))
            {
                error = error + 1;
                lblQuantityError.Text = "Quantity can not be blank";
            }
            else
                lblQuantityError.Text = "";
          

            string CategoryID = cbxCategoryId.SelectedValue.ToString();
            if (error == 0)
            {
                string insert = "insert into books values (@BookID,@BookName,@Title,@AuthorID,@Quantity,@CategoryID)";
                connection.Open();
                SqlCommand cmd = new SqlCommand(insert, connection);

                cmd.Parameters.Add("@BookID", SqlDbType.NChar);
                cmd.Parameters["@BookID"].Value = BookID;

                cmd.Parameters.Add("@BookName", SqlDbType.NVarChar);
                cmd.Parameters["@BookName"].Value = BookName;

                cmd.Parameters.Add("@Title", SqlDbType.NVarChar);
                cmd.Parameters["@Title"].Value = Title;

                cmd.Parameters.Add("@AuthorID", SqlDbType.NChar);
                cmd.Parameters["@AuthorID"].Value = AuthorID;

                cmd.Parameters.Add("@Quantity", SqlDbType.Int);
                cmd.Parameters["@Quantity"].Value = Quantity;

                cmd.Parameters.Add("@CategoryID", SqlDbType.NChar);
                cmd.Parameters["@CategoryID"].Value = CategoryID;

                cmd.ExecuteNonQuery();
                Filldata();
                MessageBox.Show(this, "Insert successfully", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txbAuthorId.Text = "";
            txbBookId.Text = "";
            txbBookName.Text = "";
            txbQuantity.Text = "";
            txbTitle.Text = "";
            cbxCategoryId.Text = "";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if ((MessageBox.Show(this, "Do you want to delete?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
                connection.Open();
            string delete = "delete from books where  = @BookID";
            SqlCommand cmd = new SqlCommand(delete, connection);
            cmd.Parameters.Add("@BookID", SqlDbType.Int);
            cmd.Parameters["@BookID"].Value = txbBookId.Text;
            cmd.ExecuteNonQuery();
            Filldata();
            MessageBox.Show(this, "Delete successfully", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if ((MessageBox.Show(this, "Do you want to edit?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
            {
                string update = "update books set book_name = @BookName, title = @Title,  "
                    + " where product_id = @productid";
                connection.Open();
                SqlCommand cmd = new SqlCommand(update, connection);
                cmd.Parameters.Add("@name", SqlDbType.VarChar);
                cmd.Parameters["@name"].Value = txtProductName.Text;

                cmd.Parameters.Add("@quantity", SqlDbType.Int);
                cmd.Parameters["@quantity"].Value = txtQuantity.Text;

                cmd.Parameters.Add("@productid", SqlDbType.Int);
                cmd.Parameters["@productid"].Value = txtProductID.Text;

                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    FillData();
                    MessageBox.Show(this, "Update successfully", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
    }

}