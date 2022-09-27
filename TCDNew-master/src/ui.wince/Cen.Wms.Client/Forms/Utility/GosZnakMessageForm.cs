using System.Windows.Forms;

namespace Cen.Wms.Client.Forms.Utility
{
    public partial class GosZnakMessageForm : Form
    {
        public GosZnakMessageForm()
        {   
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

    }
}