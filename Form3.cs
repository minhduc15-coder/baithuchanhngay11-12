
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form3 : Form
    {
        // Danh sách sản phẩm trong bộ nhớ
        BindingList<Product> products = new BindingList<Product>();
        BindingSource bindingSource = new BindingSource();

        TextBox txtProductId, txtProductName;
        TextBox txtUnitPrice, txtQuantity, txtSearch;

        ComboBox cboCategory;

        DataGridView dgvProducts;

        Button btnAdd, btnEdit, btnDelete;
        Button btnSearch, btnRefresh, btnClear;

        Product selectedProduct = null;

        public Form3()
        {
            InitializeComponent();
            CreateInterface();
        }

        // Lớp đối tượng Product
        public class Product
        {
            public string ProductId { get; set; }
            public string ProductName { get; set; }
            public decimal UnitPrice { get; set; }
            public int Quantity { get; set; }
            public string Category { get; set; }
        }

        // Tạo giao diện
        private void CreateInterface()
        {
            Text = "QUẢN LÝ DANH SÁCH SẢN PHẨM";
            Size = new Size(1050, 700);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;

            Label lblTitle = new Label();
            lblTitle.Text = "QUẢN LÝ SẢN PHẨM";
            lblTitle.Font = new Font("Arial", 20, FontStyle.Bold);
            lblTitle.ForeColor = Color.DarkBlue;
            lblTitle.Location = new Point(330, 20);
            lblTitle.AutoSize = true;
            Controls.Add(lblTitle);

            // GroupBox thông tin sản phẩm
            GroupBox grpInfo = new GroupBox();
            grpInfo.Text = "Thông tin sản phẩm";
            grpInfo.Font = new Font("Arial", 10, FontStyle.Bold);
            grpInfo.Location = new Point(30, 80);
            grpInfo.Size = new Size(960, 180);
            Controls.Add(grpInfo);

            AddLabel(grpInfo, "Mã sản phẩm:", 25, 35);
            txtProductId = AddTextBox(grpInfo, 160, 30);

            AddLabel(grpInfo, "Tên sản phẩm:", 500, 35);
            txtProductName = AddTextBox(grpInfo, 650, 30);

            AddLabel(grpInfo, "Đơn giá:", 25, 85);
            txtUnitPrice = AddTextBox(grpInfo, 160, 80);

            AddLabel(grpInfo, "Số lượng:", 500, 85);
            txtQuantity = AddTextBox(grpInfo, 650, 80);

            AddLabel(grpInfo, "Danh mục:", 25, 130);

            cboCategory = new ComboBox();
            cboCategory.Location = new Point(160, 125);
            cboCategory.Size = new Size(200, 30);
            cboCategory.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cboCategory.Items.AddRange(new string[]
            {
                "Điện thoại",
                "Laptop",
                "Phụ kiện"
            });

            cboCategory.SelectedIndex = 0;
            grpInfo.Controls.Add(cboCategory);

            // GroupBox chức năng
            GroupBox grpFunction = new GroupBox();
            grpFunction.Text = "Chức năng";
            grpFunction.Font = new Font("Arial", 10, FontStyle.Bold);
            grpFunction.Location = new Point(30, 275);
            grpFunction.Size = new Size(960, 90);
            Controls.Add(grpFunction);

            btnAdd = AddButton(grpFunction, "Thêm", 25, 30);
            btnEdit = AddButton(grpFunction, "Sửa", 145, 30);
            btnDelete = AddButton(grpFunction, "Xóa", 265, 30);
            btnClear = AddButton(grpFunction, "Làm mới", 385, 30);

            AddLabel(grpFunction, "Tìm kiếm:", 540, 35);

            txtSearch = new TextBox();
            txtSearch.Location = new Point(625, 30);
            txtSearch.Size = new Size(180, 30);
            grpFunction.Controls.Add(txtSearch);

            btnSearch = AddButton(grpFunction, "Tìm", 820, 28);

            // DataGridView
            Label lblList = new Label();
            lblList.Text = "DANH SÁCH SẢN PHẨM";
            lblList.Font = new Font("Arial", 12, FontStyle.Bold);
            lblList.Location = new Point(30, 380);
            lblList.AutoSize = true;
            Controls.Add(lblList);

            dgvProducts = new DataGridView();
            dgvProducts.Location = new Point(30, 415);
            dgvProducts.Size = new Size(960, 220);
            dgvProducts.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgvProducts.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;
            dgvProducts.ReadOnly = true;
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.BackgroundColor = Color.White;

            Controls.Add(dgvProducts);

            // Kết nối dữ liệu
            bindingSource.DataSource = products;
            dgvProducts.DataSource = bindingSource;

            dgvProducts.CellClick += DgvProducts_CellClick;

            // Sự kiện các nút
            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnSearch.Click += BtnSearch_Click;
            btnClear.Click += BtnClear_Click;
        }

        // Tạo Label
        private void AddLabel(Control parent, string text,
            int x, int y)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Location = new Point(x, y);
            lbl.Size = new Size(130, 25);
            lbl.Font = new Font("Arial", 9);
            parent.Controls.Add(lbl);
        }

        // Tạo TextBox
        private TextBox AddTextBox(Control parent, int x, int y)
        {
            TextBox txt = new TextBox();
            txt.Location = new Point(x, y);
            txt.Size = new Size(220, 25);
            parent.Controls.Add(txt);
            return txt;
        }

        // Tạo Button
        private Button AddButton(Control parent, string text,
            int x, int y)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Location = new Point(x, y);
            btn.Size = new Size(100, 35);
            parent.Controls.Add(btn);
            return btn;
        }

        // Kiểm tra dữ liệu nhập
        private bool ValidateInput(out decimal price,
            out int quantity)
        {
            price = 0;
            quantity = 0;

            if (string.IsNullOrWhiteSpace(txtProductId.Text) ||
                string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("Vui lòng nhập mã và tên sản phẩm!");
                return false;
            }

            if (!decimal.TryParse(txtUnitPrice.Text, out price)
                || price < 0)
            {
                MessageBox.Show("Đơn giá không hợp lệ!");
                return false;
            }

            if (!int.TryParse(txtQuantity.Text, out quantity)
                || quantity < 0)
            {
                MessageBox.Show("Số lượng không hợp lệ!");
                return false;
            }

            return true;
        }

        // Nút thêm sản phẩm
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            decimal price;
            int quantity;

            if (!ValidateInput(out price, out quantity))
                return;

            if (products.Any(p =>
                p.ProductId.Equals(txtProductId.Text.Trim(),
                    StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Mã sản phẩm đã tồn tại!");
                return;
            }

            Product product = new Product()
            {
                ProductId = txtProductId.Text.Trim(),
                ProductName = txtProductName.Text.Trim(),
                UnitPrice = price,
                Quantity = quantity,
                Category = cboCategory.Text
            };

            products.Add(product);

            ShowAllProducts();
            ClearInput();

            MessageBox.Show("Thêm sản phẩm thành công!");
        }

        // Chọn một dòng trong DataGridView
        private void DgvProducts_CellClick(object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            selectedProduct =
                dgvProducts.Rows[e.RowIndex].DataBoundItem as Product;

            if (selectedProduct == null)
                return;

            txtProductId.Text = selectedProduct.ProductId;
            txtProductName.Text = selectedProduct.ProductName;
            txtUnitPrice.Text = selectedProduct.UnitPrice.ToString();
            txtQuantity.Text = selectedProduct.Quantity.ToString();
            cboCategory.Text = selectedProduct.Category;
        }

        // Nút sửa sản phẩm
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (selectedProduct == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa!");
                return;
            }

            decimal price;
            int quantity;

            if (!ValidateInput(out price, out quantity))
                return;

            if (products.Any(p =>
                p != selectedProduct &&
                p.ProductId.Equals(txtProductId.Text.Trim(),
                    StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Mã sản phẩm đã tồn tại!");
                return;
            }

            selectedProduct.ProductId = txtProductId.Text.Trim();
            selectedProduct.ProductName = txtProductName.Text.Trim();
            selectedProduct.UnitPrice = price;
            selectedProduct.Quantity = quantity;
            selectedProduct.Category = cboCategory.Text;

            ShowAllProducts();
            ClearInput();

            MessageBox.Show("Cập nhật sản phẩm thành công!");
        }

        // Nút xóa sản phẩm
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedProduct == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa sản phẩm này?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                products.Remove(selectedProduct);
                ShowAllProducts();
                ClearInput();

                MessageBox.Show("Xóa sản phẩm thành công!");
            }
        }

        // Tìm kiếm theo tên sản phẩm
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                ShowAllProducts();
                return;
            }

            var result = products.Where(p =>
                p.ProductName.IndexOf(keyword,
                    StringComparison.OrdinalIgnoreCase) >= 0
            ).ToList();

            bindingSource.DataSource = new BindingList<Product>(result);

            dgvProducts.DataSource = bindingSource;

            selectedProduct = null;
        }

        // Hiển thị toàn bộ sản phẩm
        private void ShowAllProducts()
        {
            bindingSource.DataSource = products;
            dgvProducts.DataSource = bindingSource;
            dgvProducts.Refresh();
        }

        // Làm mới dữ liệu
        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            ShowAllProducts();
            ClearInput();
        }

        // Xóa nội dung các ô nhập
        private void ClearInput()
        {
            txtProductId.Clear();
            txtProductName.Clear();
            txtUnitPrice.Clear();
            txtQuantity.Clear();

            cboCategory.SelectedIndex = 0;

            selectedProduct = null;
            dgvProducts.ClearSelection();
        }
    }
}
