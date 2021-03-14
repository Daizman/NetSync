using System;
using System.Windows.Forms;

namespace CreateFolder
{
    public partial class wndCreateFolder : Form
    {
        private string _selectedPath = "";
        public wndCreateFolder(string title="Создание папки")
        {
            InitializeComponent();
            Text = title;
        }

        public string SelectedPath 
        {
            get
            {
                return _selectedPath;
            }
        }

        private void bCreate_Click(object sender, EventArgs e)
        {
            if (_selectedPath != "")
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Укажите путь");
            }
        }

        private void bChoosePath_Click(object sender, EventArgs e)
        {
            if (fbdPath.ShowDialog() == DialogResult.OK)
            {
                _selectedPath = fbdPath.SelectedPath;
                lFolderPath.Text = _selectedPath;
            }
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            _selectedPath = "";
            lFolderPath.Text = _selectedPath;
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
