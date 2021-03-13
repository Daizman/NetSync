using System;
using System.Windows.Forms;

namespace AddFriend
{
    public partial class wndAddFriend : Form
    {
        private string _friendPublicKey;
        public wndAddFriend()
        {
            InitializeComponent();
        }

        public string FriendPublicKey
        {
            get
            {
                return _friendPublicKey;
            }
        }

        private void bAddFriend_Click(object sender, EventArgs e)
        {
            if (tbPublicKey.Text.Trim() != "")
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Введите значение публичного ключа");
            }
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            _friendPublicKey = "";
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void tbPublicKey_TextChanged(object sender, EventArgs e)
        {
            _friendPublicKey = tbPublicKey.Text.Trim();
        }
    }
}
