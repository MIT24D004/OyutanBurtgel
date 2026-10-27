
#nullable disable
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace OyutanBurtgel
{
    public class Student
    {
        public string Code, Phone, Faculty, Course, Language, Subjects;
        public int Age;
        public DateTime Birth;
        public bool Scholarship;
    }

    public partial class Form1 : Form
    {
        private int niit = 0;

        public Form1()
        {
            InitializeComponent();

            // Бүх event-ийг энд кодоор холбоно (Design дээр давхар дарахгүй!)
            this.Load += Form1_Load;
            this.FormClosing += Form1_FormClosing;
            textBox1.TextChanged += Input_Changed;
            maskedTextBox1.TextChanged += Input_Changed;
            comboBox1.SelectedIndexChanged += Input_Changed;
            checkedListBox1.ItemCheck += checkedListBox1_ItemCheck;
            dateTimePicker1.ValueChanged += dateTimePicker1_ValueChanged;
            maskedTextBox1.MaskInputRejected += maskedTextBox1_MaskInputRejected;
            treeView1.AfterSelect += treeView1_AfterSelect;
            pictureBox1.Click += pictureBox1_Click;
            button1.Click += button1_Click;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
        }

        // ===== Form нээгдэх / хаагдах =====
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Оюутны бүртгэлийн програм";
            button1.Text = "Бүртгэх";
            label1.Text = "Нийт: 0";
            label2.Text = "Бүртгэлийн явц: 0%";
           
            linkLabel1.Text = "Цэвэрлэх";
            checkBox1.Text = "Тэтгэлэгтэй";
            radioButton1.Text = "Өдрийн сургалт";
            radioButton2.Text = "Оройн сургалт";
            radioButton1.Checked = true;

            textBox1.PlaceholderText = "Оюутны код, жишээ нь M.SW24D000";

            maskedTextBox1.Mask = "0000-0000";

            numericUpDown1.Minimum = 16;
            numericUpDown1.Maximum = 60;
            numericUpDown1.Value = 20;

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";

            checkedListBox1.CheckOnClick = true;
            checkedListBox1.Items.Clear();
            checkedListBox1.Items.AddRange(new object[]
                { "Програмчлал", "Өгөгдлийн бүтэц", "Алгоритм", "Өгөгдлийн сан", "Сүлжээ" });

            listBox1.Items.Clear();
            listBox1.Items.AddRange(new object[]
                { "Монгол", "Англи", "Орос", "Хятад", "Япон", "Солонгос" });

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new object[]
                { "-- Тэнхим сонгох --", "Компьютерын ухаан", "Мэдээллийн технологи",
                  "Программ хангамж", "Математик", "Физик" });
            comboBox1.SelectedIndex = 0;

            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.MultiSelect = false;
            listView1.Columns.Clear();
            listView1.Columns.Add("Код", 110);
            listView1.Columns.Add("Утас", 80);
            listView1.Columns.Add("Нас", 50);

            treeView1.Nodes.Clear();
            TreeNode it = new TreeNode("Мэдээллийн технологийн сургууль");
            it.Nodes.Add("Компьютерын ухаан");
            it.Nodes.Add("Мэдээллийн технологи");
            it.Nodes.Add("Программ хангамж");
            TreeNode nat = new TreeNode("Байгалийн ухааны сургууль");
            nat.Nodes.Add("Математик");
            nat.Nodes.Add("Физик");
            treeView1.Nodes.Add(it);
            treeView1.Nodes.Add(nat);
            treeView1.ExpandAll();

            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;

            richTextBox1.ReadOnly = true;
            richTextBox1.BackColor = Color.White;

            progressBar1.Minimum = 0;
            progressBar1.Maximum = 5;

            notifyIcon1.Icon = SystemIcons.Information;
            notifyIcon1.Text = "Оюутны бүртгэл";

            dateTimePicker1.Value = new DateTime(2005, 1, 28);   // нас 21 болно
            UpdateProgress();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            notifyIcon1.Visible = false;
        }

        // ===== Нэг handler хуваалцах =====
        private void Input_Changed(object sender, EventArgs e)
        {
            UpdateProgress();
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            BeginInvoke(new Action(UpdateProgress));
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            int age = DateTime.Today.Year - dateTimePicker1.Value.Year;
            if (dateTimePicker1.Value.Date > DateTime.Today.AddYears(-age)) age--;
            if (age >= numericUpDown1.Minimum && age <= numericUpDown1.Maximum)
                numericUpDown1.Value = age;
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            toolTip1.Show("Зөвхөн тоо оруулна уу", maskedTextBox1, 0, maskedTextBox1.Height, 1500);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int idx = comboBox1.Items.IndexOf(e.Node.Text);
            if (idx > 0) comboBox1.SelectedIndex = idx;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Зураг|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image?.Dispose();
                    using (var tmp = Image.FromFile(ofd.FileName))
                        pictureBox1.Image = new Bitmap(tmp);   // файлыг түгжихгүй
                    UpdateProgress();
                }
            }
        }

        // ===== Бүртгэх =====
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                Warn("Оюутны кодоо оруулна уу.");
                textBox1.Focus();
                return;
            }
            if (!maskedTextBox1.MaskCompleted)
            {
                Warn("Утасны дугаараа бүрэн (8 оронтой) оруулна уу.");
                maskedTextBox1.Focus();
                return;
            }
            if (comboBox1.SelectedIndex <= 0)
            {
                Warn("Тэнхимээ сонгоно уу.");
                comboBox1.Focus();
                return;
            }

            var subjects = new List<string>();
            foreach (var item in checkedListBox1.CheckedItems)
                subjects.Add(item.ToString());

            var s = new Student
            {
                Code = textBox1.Text.Trim(),
                Phone = maskedTextBox1.Text,
                Age = (int)numericUpDown1.Value,
                Birth = dateTimePicker1.Value,
                Course = radioButton1.Checked ? "Өдрийн" : "Оройн",
                Faculty = comboBox1.Text,
                Scholarship = checkBox1.Checked,
                Subjects = string.Join(", ", subjects),
                Language = listBox1.SelectedItem?.ToString() ?? ""
            };

            ShowInRichText(s);

            var row = new ListViewItem(s.Code);
            row.SubItems.Add(s.Phone);
            row.SubItems.Add(s.Age.ToString());
            row.Tag = s;
            listView1.Items.Add(row);

            niit++;
            label1.Text = "Нийт: " + niit;

            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(2000, "Бүртгэл", "Амжилттай бүртгэгдлээ", ToolTipIcon.Info);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1 &&
                listView1.SelectedItems[0].Tag is Student s)
            {
                ShowInRichText(s);
            }
        }

        // ===== Цэвэрлэх =====
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox1.Clear();
            maskedTextBox1.Clear();
            comboBox1.SelectedIndex = 0;
            checkBox1.Checked = false;
            radioButton1.Checked = true;
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
                checkedListBox1.SetItemChecked(i, false);
            listBox1.ClearSelected();
            numericUpDown1.Value = 20;
            dateTimePicker1.Value = new DateTime(2005, 1, 28);
            richTextBox1.Clear();
            pictureBox1.Image?.Dispose();
            pictureBox1.Image = null;
            UpdateProgress();
        }

        // ===== Туслах функцууд =====
        private void Warn(string msg)
        {
            MessageBox.Show(msg, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void UpdateProgress()
        {
            int filled = 0;
            if (!string.IsNullOrWhiteSpace(textBox1.Text)) filled++;
            if (maskedTextBox1.MaskCompleted) filled++;
            if (comboBox1.SelectedIndex > 0) filled++;
            if (checkedListBox1.CheckedItems.Count > 0) filled++;
            if (pictureBox1.Image != null) filled++;

            progressBar1.Value = filled;
            label2.Text = $"Бүртгэлийн явц: {filled * 100 / 5}%";
        }

        private void AddLine(string label, string value)
        {
            richTextBox1.SelectionFont = new Font("Segoe UI", 10, FontStyle.Bold);
            richTextBox1.SelectionColor = Color.Black;
            richTextBox1.AppendText(label + " ");
            richTextBox1.SelectionFont = new Font("Segoe UI", 10, FontStyle.Regular);
            richTextBox1.SelectionColor = Color.Green;
            richTextBox1.AppendText(value + "\n");
        }

        private void ShowInRichText(Student s)
        {
            richTextBox1.Clear();
            richTextBox1.SelectionFont = new Font("Segoe UI", 14, FontStyle.Bold);
            richTextBox1.SelectionColor = Color.DarkBlue;
            richTextBox1.AppendText("ОЮУТНЫ БҮРТГЭЛ\n\n");

            AddLine("Код:", s.Code);
            AddLine("Утас:", s.Phone);
            AddLine("Нас:", s.Age.ToString());
            AddLine("Төрсөн өдөр:", s.Birth.ToString("yyyy-MM-dd"));
            AddLine("Сургалт:", s.Course);
            AddLine("Тэнхим:", s.Faculty);
            AddLine("Тэтгэлэг:", s.Scholarship ? "Тийм" : "Үгүй");
            AddLine("Хичээл:", s.Subjects);
            AddLine("Хэл:", s.Language);
        }
    }
}