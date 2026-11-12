
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form4 : Form
    {
        private TreeView tvDepartments;
        private ListView lvEmployees;
        private ComboBox cboView;

        private ImageList treeImages;
        private ImageList employeeImages;

        private List<Employee> employees = new List<Employee>();

        public Form4()
        {
            InitializeComponent();

            CreateEmployees();
            CreateInterface();
            CreateTree();

            LoadEmployees(employees);
        }

        // Lớp thông tin nhân viên
        public class Employee
        {
            public string EmployeeId { get; set; }
            public string FullName { get; set; }
            public string Position { get; set; }
            public DateTime StartDate { get; set; }
            public string Department { get; set; }
            public string Group { get; set; }
        }

        // Dữ liệu nhân viên mẫu
        private void CreateEmployees()
        {
            employees.AddRange(new Employee[]
            {
                new Employee
                {
                    EmployeeId = "NV01",
                    FullName = "Nguyễn Văn An",
                    Position = "Trưởng phòng",
                    StartDate = new DateTime(2020, 3, 15),
                    Department = "Kỹ thuật",
                    Group = "Phát triển phần mềm"
                },
                new Employee
                {
                    EmployeeId = "NV02",
                    FullName = "Trần Minh Tuấn",
                    Position = "Lập trình viên",
                    StartDate = new DateTime(2021, 5, 10),
                    Department = "Kỹ thuật",
                    Group = "Phát triển phần mềm"
                },
                new Employee
                {
                    EmployeeId = "NV03",
                    FullName = "Lê Hoàng Nam",
                    Position = "Lập trình viên",
                    StartDate = new DateTime(2022, 7, 20),
                    Department = "Kỹ thuật",
                    Group = "Phát triển phần mềm"
                },
                new Employee
                {
                    EmployeeId = "NV04",
                    FullName = "Phạm Thu Hà",
                    Position = "Kiểm thử viên",
                    StartDate = new DateTime(2021, 2, 12),
                    Department = "Kỹ thuật",
                    Group = "Kiểm thử"
                },
                new Employee
                {
                    EmployeeId = "NV05",
                    FullName = "Đỗ Quang Huy",
                    Position = "Trưởng nhóm",
                    StartDate = new DateTime(2020, 8, 1),
                    Department = "Kỹ thuật",
                    Group = "Kiểm thử"
                },
                new Employee
                {
                    EmployeeId = "NV06",
                    FullName = "Nguyễn Thị Lan",
                    Position = "Trưởng phòng",
                    StartDate = new DateTime(2019, 4, 5),
                    Department = "Kinh doanh",
                    Group = "Bán hàng"
                },
                new Employee
                {
                    EmployeeId = "NV07",
                    FullName = "Hoàng Văn Đức",
                    Position = "Nhân viên bán hàng",
                    StartDate = new DateTime(2022, 6, 15),
                    Department = "Kinh doanh",
                    Group = "Bán hàng"
                },
                new Employee
                {
                    EmployeeId = "NV08",
                    FullName = "Vũ Thị Mai",
                    Position = "Nhân viên Marketing",
                    StartDate = new DateTime(2023, 1, 10),
                    Department = "Kinh doanh",
                    Group = "Marketing"
                },
                new Employee
                {
                    EmployeeId = "NV09",
                    FullName = "Bùi Minh Anh",
                    Position = "Trưởng phòng",
                    StartDate = new DateTime(2018, 9, 20),
                    Department = "Nhân sự",
                    Group = "Tuyển dụng"
                },
                new Employee
                {
                    EmployeeId = "NV10",
                    FullName = "Phan Thu Trang",
                    Position = "Nhân viên tuyển dụng",
                    StartDate = new DateTime(2022, 11, 5),
                    Department = "Nhân sự",
                    Group = "Tuyển dụng"
                },
                new Employee
                {
                    EmployeeId = "NV11",
                    FullName = "Ngô Văn Bình",
                    Position = "Nhân viên hành chính",
                    StartDate = new DateTime(2023, 2, 18),
                    Department = "Nhân sự",
                    Group = "Hành chính"
                }
            });
        }

        // Tạo giao diện
        private void CreateInterface()
        {
            Text = "BÀI 5.4 - QUẢN LÝ NHÂN VIÊN";
            Size = new Size(1100, 650);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;

            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ DANH SÁCH NHÂN VIÊN",
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 17, FontStyle.Bold),
                ForeColor = Color.DarkBlue
            };

            Panel topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50
            };

            Label lblView = new Label
            {
                Text = "Chế độ hiển thị:",
                Location = new Point(15, 15),
                AutoSize = true,
                Font = new Font("Arial", 10)
            };

            cboView = new ComboBox
            {
                Location = new Point(140, 10),
                Width = 180,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cboView.Items.AddRange(new object[]
            {
                "Details",
                "SmallIcon",
                "LargeIcon",
                "Tile"
            });

            cboView.SelectedIndex = 0;
            cboView.SelectedIndexChanged += CboView_SelectedIndexChanged;

            topPanel.Controls.Add(lblView);
            topPanel.Controls.Add(cboView);

            // Tạo vùng chia đôi
            SplitContainer split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle
            };

            split.SplitterDistance = 300;

            // TreeView
            tvDepartments = new TreeView
            {
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 11),
                HideSelection = false
            };

            // ListView
            lvEmployees = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                MultiSelect = false,
                Font = new Font("Arial", 10)
            };

            lvEmployees.Columns.Add("Mã NV", 90);
            lvEmployees.Columns.Add("Họ Tên", 190);
            lvEmployees.Columns.Add("Chức vụ", 160);
            lvEmployees.Columns.Add("Ngày vào làm", 130);

            // ImageList cho TreeView
            treeImages = new ImageList();
            treeImages.ImageSize = new Size(16, 16);
            treeImages.Images.Add("company", SystemIcons.Application);
            treeImages.Images.Add("department", SystemIcons.Information);
            treeImages.Images.Add("group", SystemIcons.Asterisk);

            tvDepartments.ImageList = treeImages;

            // ImageList cho ListView
            employeeImages = new ImageList();
            employeeImages.ImageSize = new Size(32, 32);
            employeeImages.Images.Add("employee", SystemIcons.Application);

            lvEmployees.LargeImageList = employeeImages;
            lvEmployees.SmallImageList = employeeImages;

            // Sự kiện
            tvDepartments.AfterSelect += TvDepartments_AfterSelect;

            split.Panel1.Controls.Add(tvDepartments);
            split.Panel2.Controls.Add(lvEmployees);

            Controls.Add(split);
            Controls.Add(topPanel);
            Controls.Add(lblTitle);
        }

        // Tạo cây phân cấp
        private void CreateTree()
        {
            TreeNode company = new TreeNode("Công ty ABC")
            {
                ImageKey = "company",
                SelectedImageKey = "company",
                Tag = new NodeInfo("", "")
            };

            AddDepartment(company, "Kỹ thuật",
                "Phát triển phần mềm", "Kiểm thử");

            AddDepartment(company, "Kinh doanh",
                "Bán hàng", "Marketing");

            AddDepartment(company, "Nhân sự",
                "Tuyển dụng", "Hành chính");

            tvDepartments.Nodes.Add(company);
            tvDepartments.ExpandAll();
            tvDepartments.SelectedNode = company;
        }

        // Thêm phòng ban và nhóm
        private void AddDepartment(
            TreeNode parent,
            string department,
            params string[] groups)
        {
            TreeNode deptNode = new TreeNode(department)
            {
                ImageKey = "department",
                SelectedImageKey = "department",
                Tag = new NodeInfo(department, "")
            };

            foreach (string group in groups)
            {
                TreeNode groupNode = new TreeNode(group)
                {
                    ImageKey = "group",
                    SelectedImageKey = "group",
                    Tag = new NodeInfo(department, group)
                };

                deptNode.Nodes.Add(groupNode);
            }

            parent.Nodes.Add(deptNode);
        }

        // Thông tin Node
        private class NodeInfo
        {
            public string Department { get; set; }
            public string Group { get; set; }

            public NodeInfo(string department, string group)
            {
                Department = department;
                Group = group;
            }
        }

        // Lọc nhân viên theo Node
        private void TvDepartments_AfterSelect(
            object sender, TreeViewEventArgs e)
        {
            NodeInfo info = e.Node.Tag as NodeInfo;

            if (info == null)
                return;

            IEnumerable<Employee> result = employees;

            if (info.Department != "")
            {
                result = result.Where(x =>
                    x.Department == info.Department);
            }

            if (info.Group != "")
            {
                result = result.Where(x =>
                    x.Group == info.Group);
            }

            LoadEmployees(result.ToList());
        }

        // Hiển thị danh sách nhân viên
        private void LoadEmployees(List<Employee> data)
        {
            lvEmployees.Items.Clear();

            foreach (Employee emp in data)
            {
                ListViewItem item = new ListViewItem(emp.EmployeeId);

                item.SubItems.Add(emp.FullName);
                item.SubItems.Add(emp.Position);
                item.SubItems.Add(
                    emp.StartDate.ToString("dd/MM/yyyy"));

                item.ImageKey = "employee";

                lvEmployees.Items.Add(item);
            }
        }

        // Thay đổi chế độ hiển thị
        private void CboView_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            switch (cboView.SelectedIndex)
            {
                case 0:
                    lvEmployees.View = View.Details;
                    break;

                case 1:
                    lvEmployees.View = View.SmallIcon;
                    break;

                case 2:
                    lvEmployees.View = View.LargeIcon;
                    break;

                case 3:
                    lvEmployees.View = View.Tile;
                    lvEmployees.TileSize = new Size(280, 60);
                    break;
            }
        }
    }
}
