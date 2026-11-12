
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form2 : Form
    {
        ComboBox cboCategory;
        ListBox lstAvailableServices;
        ListBox lstSelectedServices;
        Button btnSelect, btnRemove, btnClearAll;
        Label lblTotal, lblDiscount, lblPayment;
        NumericUpDown nudDiscount;

        Dictionary<string, List<Service>> serviceData =
            new Dictionary<string, List<Service>>();

        public Form2()
        {
            InitializeComponent();
            CreateInterface();
            LoadServiceData();
        }

        // Lớp lưu thông tin dịch vụ
        class Service
        {
            public string Name { get; set; }
            public decimal Price { get; set; }

            public Service(string name, decimal price)
            {
                Name = name;
                Price = price;
            }

            public override string ToString()
            {
                return Name + " - " + Price.ToString("N0") + " đ";
            }
        }

        private void CreateInterface()
        {
            Text = "BẢNG TÍNH TIỀN DỊCH VỤ";
            Size = new Size(850, 620);
            StartPosition = FormStartPosition.CenterScreen;

            Label title = new Label();
            title.Text = "QUẢN LÝ DỊCH VỤ VÀ THANH TOÁN";
            title.Font = new Font("Arial", 17, FontStyle.Bold);
            title.ForeColor = Color.DarkBlue;
            title.Location = new Point(170, 20);
            title.AutoSize = true;
            Controls.Add(title);

            Label lblCategory = new Label();
            lblCategory.Text = "Loại dịch vụ:";
            lblCategory.Location = new Point(40, 85);
            lblCategory.AutoSize = true;
            Controls.Add(lblCategory);

            cboCategory = new ComboBox();
            cboCategory.Location = new Point(150, 80);
            cboCategory.Size = new Size(250, 30);
            cboCategory.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cboCategory.Items.AddRange(new string[]
            {
                "Khám bệnh",
                "Xét nghiệm",
                "Chụp X-Quang",
                "Vắc-xin"
            });

            cboCategory.SelectedIndexChanged +=
                CboCategory_SelectedIndexChanged;

            Controls.Add(cboCategory);

            Label lblAvailable = new Label();
            lblAvailable.Text = "Danh sách dịch vụ:";
            lblAvailable.Location = new Point(40, 130);
            lblAvailable.AutoSize = true;
            Controls.Add(lblAvailable);

            lstAvailableServices = new ListBox();
            lstAvailableServices.Location = new Point(40, 160);
            lstAvailableServices.Size = new Size(300, 230);
            lstAvailableServices.DoubleClick +=
                (s, e) => SelectService();
            Controls.Add(lstAvailableServices);

            Label lblSelected = new Label();
            lblSelected.Text = "Dịch vụ đã chọn:";
            lblSelected.Location = new Point(490, 130);
            lblSelected.AutoSize = true;
            Controls.Add(lblSelected);

            lstSelectedServices = new ListBox();
            lstSelectedServices.Location = new Point(490, 160);
            lstSelectedServices.Size = new Size(300, 230);
            Controls.Add(lstSelectedServices);

            btnSelect = new Button();
            btnSelect.Text = "Chọn  >";
            btnSelect.Location = new Point(365, 200);
            btnSelect.Size = new Size(100, 40);
            btnSelect.Click += (s, e) => SelectService();
            Controls.Add(btnSelect);

            btnRemove = new Button();
            btnRemove.Text = "<  Xóa";
            btnRemove.Location = new Point(365, 260);
            btnRemove.Size = new Size(100, 40);
            btnRemove.Click += (s, e) => RemoveService();
            Controls.Add(btnRemove);

            btnClearAll = new Button();
            btnClearAll.Text = "<< Xóa tất cả";
            btnClearAll.Location = new Point(365, 320);
            btnClearAll.Size = new Size(100, 40);
            btnClearAll.Click += (s, e) => ClearAll();
            Controls.Add(btnClearAll);

            Label lblDiscountTitle = new Label();
            lblDiscountTitle.Text = "Tỷ lệ chiết khấu (%):";
            lblDiscountTitle.Location = new Point(40, 425);
            lblDiscountTitle.AutoSize = true;
            Controls.Add(lblDiscountTitle);

            nudDiscount = new NumericUpDown();
            nudDiscount.Location = new Point(200, 420);
            nudDiscount.Size = new Size(100, 30);
            nudDiscount.Minimum = 0;
            nudDiscount.Maximum = 100;
            nudDiscount.ValueChanged +=
                (s, e) => CalculateTotal();
            Controls.Add(nudDiscount);

            lblTotal = new Label();
            lblTotal.Location = new Point(40, 470);
            lblTotal.Size = new Size(350, 30);
            lblTotal.Font = new Font("Arial", 11);
            Controls.Add(lblTotal);

            lblDiscount = new Label();
            lblDiscount.Location = new Point(40, 505);
            lblDiscount.Size = new Size(350, 30);
            lblDiscount.Font = new Font("Arial", 11);
            Controls.Add(lblDiscount);

            lblPayment = new Label();
            lblPayment.Location = new Point(420, 470);
            lblPayment.Size = new Size(380, 35);
            lblPayment.Font = new Font("Arial", 12,
                FontStyle.Bold);
            lblPayment.ForeColor = Color.DarkRed;
            Controls.Add(lblPayment);
        }

        // Khởi tạo dữ liệu dịch vụ
        private void LoadServiceData()
        {
            serviceData["Khám bệnh"] = new List<Service>
            {
                new Service("Khám tổng quát", 200000),
                new Service("Khám chuyên khoa", 300000),
                new Service("Khám định kỳ", 150000)
            };

            serviceData["Xét nghiệm"] = new List<Service>
            {
                new Service("Xét nghiệm máu", 150000),
                new Service("Xét nghiệm nước tiểu", 100000),
                new Service("Xét nghiệm đường huyết", 120000)
            };

            serviceData["Chụp X-Quang"] = new List<Service>
            {
                new Service("Chụp X-Quang phổi", 250000),
                new Service("Chụp X-Quang xương", 300000),
                new Service("Chụp X-Quang răng", 200000)
            };

            serviceData["Vắc-xin"] = new List<Service>
            {
                new Service("Vắc-xin cúm", 350000),
                new Service("Vắc-xin viêm gan B", 400000),
                new Service("Vắc-xin uốn ván", 250000)
            };

            cboCategory.SelectedIndex = 0;
            CalculateTotal();
        }

        // Hiển thị dịch vụ theo loại
        private void CboCategory_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            lstAvailableServices.Items.Clear();

            string category = cboCategory.SelectedItem.ToString();

            foreach (Service service in serviceData[category])
            {
                lstAvailableServices.Items.Add(service);
            }
        }

        // Chuyển dịch vụ sang danh sách đã chọn
        private void SelectService()
        {
            if (lstAvailableServices.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn dịch vụ!");
                return;
            }

            Service service =
                (Service)lstAvailableServices.SelectedItem;

            foreach (Service item in lstSelectedServices.Items)
            {
                if (item.Name == service.Name)
                {
                    MessageBox.Show("Dịch vụ đã được chọn!");
                    return;
                }
            }

            lstSelectedServices.Items.Add(service);
            CalculateTotal();
        }

        // Xóa một dịch vụ
        private void RemoveService()
        {
            if (lstSelectedServices.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn dịch vụ cần xóa!");
                return;
            }

            lstSelectedServices.Items.Remove(
                lstSelectedServices.SelectedItem);

            CalculateTotal();
        }

        // Xóa toàn bộ dịch vụ
        private void ClearAll()
        {
            lstSelectedServices.Items.Clear();
            CalculateTotal();
        }

        // Tính tổng tiền và chiết khấu
        private void CalculateTotal()
        {
            decimal total = 0;

            foreach (Service service in lstSelectedServices.Items)
            {
                total += service.Price;
            }

            decimal discountRate = nudDiscount == null
                ? 0 : nudDiscount.Value;

            decimal discountAmount =
                total * discountRate / 100;

            decimal payment = total - discountAmount;

            if (lblTotal != null)
                lblTotal.Text = "Tổng tiền: " +
                    total.ToString("N0") + " đ";

            if (lblDiscount != null)
                lblDiscount.Text = "Tiền chiết khấu: " +
                    discountAmount.ToString("N0") + " đ";

            if (lblPayment != null)
                lblPayment.Text = "THÀNH TIỀN: " +
                    payment.ToString("N0") + " đ";
        }
    }
}
