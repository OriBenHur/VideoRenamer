using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace VideoRenamer
{
    public partial class MsgBox : Form
    {
        public MsgBox()
        {
            InitializeComponent();
        }

        private string _id;

        public string MyId
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _title;

        public string MyTitle
        {
            get { return _title; }
            set { _title = value; }
        }
        public void AddLabeles(string link, string lable, int x, int y)
        {
            var tmpLable = new LinkLabel();
            var checkBox = new RadioButton();
            var location = new Point(x, y);
            var checkBoxLocation = new Point(x - 15, y - 5);
            checkBox.Location = checkBoxLocation;
            tmpLable.Text = lable;
            tmpLable.Links.Add(0, link.Length, link);
            tmpLable.AutoSize = true;
            tmpLable.Location = location;
            var id = link.Split('/');
            checkBox.Name = id[id.Length - 1];
            panel1.Controls.Add(tmpLable);
            panel1.Controls.Add(checkBox);
            tmpLable.Click += link_Click;


        }

        private void link_Click(object sender, EventArgs e)
        {
            var link = (LinkLabel)sender;
            Process.Start(link.Links[0].LinkData.ToString());
        }

        private static bool _isOk;
        private void OK_Click(object sender, EventArgs e)
        {
            var item = GetCheckedRadio(panel1);
            if (item != null)
            {
                if (!item.Checked) return;
                MyId = item.Name;
                _isOk = true;
                Dispose();
            }
            else
                MessageBox.Show(@"You Must Pick One Item");

        }


        private void MsgBox_Load(object sender, EventArgs e)
        {
            Text = $@"{_title} Conflicts";
            label1.Text = $@"Witch {_title} is the correct one?";

        }

        private RadioButton GetCheckedRadio(Control container)
        {
            foreach (var control in container.Controls)
            {
                RadioButton radio = control as RadioButton;

                if (radio != null && radio.Checked)
                {
                    return radio;
                }
            }

            return null;
        }

        private void MsgBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CloseCancel() == false)
            {
                e.Cancel = true;
            }
        }

        private static bool CloseCancel()
        {
            if (!_isOk)
            {
                const string message = "Are you sure that you would like to exit?";
                const string caption = "Exit";
                var result = MessageBox.Show(message, caption,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                return result == DialogResult.Yes;
            }
            _isOk = false;
            return true;

        }
    }
}
