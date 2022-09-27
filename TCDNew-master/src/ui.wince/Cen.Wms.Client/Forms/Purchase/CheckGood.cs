using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Datalogic.API;

namespace Cen.Wms.Client.Forms.Purchase
{
    public partial class CheckGood : Common.ScanBaseForm
    {
        private bool _isSuccess;
        public CheckGood(string barcode)
        {
            label1.BackColor = Color.Green;
            label1.Text = barcode;
            Random rnd = new Random();
            _isSuccess = rnd.Next(0, 2) == 0 ? false : true;
            if (!_isSuccess)
                label1.BackColor = Color.Red;
            
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {   
            this.Close();
        }

        private void CheckGood_Load(object sender, EventArgs e)
        {

        }
        protected override void ProcessBarcode(string barcode, CodeId codeId)
        {
            
        }
    }
}