using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        TextBox txtUsername, txtPassword, txtConfirmPassword;
        DateTimePicker dtpBirth;
        RadioButton rdoMale, rdoFemale;
        CheckBox chkTerms;
        Button btnRegister, btnReset;
        ErrorProvider epCheck;

        public Form1()
        {
            // Không cần gọi InitializeComponent()
            CreateInterface();
        }

        private void CreateInterface()
        {
            this.Text = "Đăng ký tài khoản";
            this.Size = new Size(600, 570);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            epCheck = new ErrorProvider();
            epCheck.ContainerControl = this;

            Label lblTitle = new Label();
            lblTitle.Text = "ĐĂNG KÝ TÀI KHOẢN";
            lblTitle.Font = new Font("Arial", 18, FontStyle.Bold);
            lblTitle.ForeColor = Color.DarkBlue;
            lblTitle.Location = new Point(140, 20);
            lblTitle.AutoSize = true;
            Controls.Add(lblTitle);

            AddLabel("Tên đăng nhập:", 50, 90);
            txtUsername = AddTextBox(220, 85);

            AddLabel("Mật khẩu:", 50, 140);
            txtPassword = AddTextBox(220, 135);
            txtPassword.UseSystemPasswordChar = true;

            AddLabel("Xác nhận mật khẩu:", 50, 190);
            txtConfirmPassword = AddTextBox(220, 185);
            txtConfirmPassword.UseSystemPasswordChar = true;

            AddLabel("Ngày sinh:", 50, 240);

            dtpBirth = new DateTimePicker();
            dtpBirth.Location = new Point(220, 235);
            dtpBirth.Size = new Size(230, 30);
            dtpBirth.Format = DateTimePickerFormat.Short;
            dtpBirth.MaxDate = DateTime.Today;
            dtpBirth.Value = DateTime.Today.AddYears(-18);
            Controls.Add(dtpBirth);

            AddLabel("Giới tính:", 50, 290);

            rdoMale = new RadioButton();
            rdoMale.Text = "Nam";
            rdoMale.Location = new Point(220, 285);
            rdoMale.AutoSize = true;
            rdoMale.Checked = true;
            Controls.Add(rdoMale);

            rdoFemale = new RadioButton();
            rdoFemale.Text = "Nữ";
            rdoFemale.Location = new Point(300, 285);
            rdoFemale.AutoSize = true;
            Controls.Add(rdoFemale);

            chkTerms = new CheckBox();
            chkTerms.Text = "Tôi đồng ý với điều khoản dịch vụ";
            chkTerms.Location = new Point(220, 335);
            chkTerms.AutoSize = true;
            Controls.Add(chkTerms);

            btnRegister = new Button();
            btnRegister.Text = "Đăng ký";
            btnRegister.Location = new Point(150, 400);
            btnRegister.Size = new Size(110, 40);
            btnRegister.BackColor = Color.LightBlue;
            btnRegister.Click += BtnRegister_Click;
            Controls.Add(btnRegister);

            btnReset = new Button();
            btnReset.Text = "Làm mới";
            btnReset.Location = new Point(300, 400);
            btnReset.Size = new Size(110, 40);
            btnReset.Click += BtnReset_Click;
            Controls.Add(btnReset);
        }

        private void AddLabel(string text, int x, int y)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Location = new Point(x, y);
            lbl.Size = new Size(160, 25);
            lbl.Font = new Font("Arial", 10);
            Controls.Add(lbl);
        }

        private TextBox AddTextBox(int x, int y)
        {
            TextBox txt = new TextBox();
            txt.Location = new Point(x, y);
            txt.Size = new Size(230, 25);
            Controls.Add(txt);
            return txt;
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            epCheck.Clear();

            bool isValid = true;

            // Kiểm tra tên đăng nhập
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                epCheck.SetError(txtUsername,
                    "Tên đăng nhập không được để trống!");
                isValid = false;
            }

            // Kiểm tra mật khẩu
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                epCheck.SetError(txtPassword,
                    "Mật khẩu không được để trống!");
                isValid = false;
            }

            // Kiểm tra xác nhận mật khẩu
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                epCheck.SetError(txtConfirmPassword,
                    "Mật khẩu xác nhận không khớp!");
                isValid = false;
            }

            // Tính tuổi
            int age = DateTime.Today.Year - dtpBirth.Value.Year;

            if (dtpBirth.Value.Date > DateTime.Today.AddYears(-age))
                age--;

            if (age < 18)
            {
                epCheck.SetError(dtpBirth,
                    "Bạn phải đủ 18 tuổi trở lên!");
                isValid = false;
            }

            // Kiểm tra điều khoản
            if (!chkTerms.Checked)
            {
                epCheck.SetError(chkTerms,
                    "Bạn phải đồng ý với điều khoản dịch vụ!");
                isValid = false;
            }

            // Thông báo kết quả
            if (isValid)
            {
                string gender = rdoMale.Checked ? "Nam" : "Nữ";

                MessageBox.Show(
                    "Đăng ký tài khoản thành công!\n" +
                    "Tên đăng nhập: " + txtUsername.Text +
                    "\nGiới tính: " + gender +
                    "\nTuổi: " + age,
                    "Thông báo",
MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else
            {
                MessageBox.Show(
                    "Vui lòng kiểm tra lại thông tin!",
                    "Có lỗi xảy ra",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtConfirmPassword.Clear();

            dtpBirth.Value = DateTime.Today.AddYears(-18);

            rdoMale.Checked = true;
            chkTerms.Checked = false;

            epCheck.Clear();
            txtUsername.Focus();
        }
    }
}