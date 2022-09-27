namespace Cen.Wms.Client.Forms.Purchase
{
    partial class PurchaseTaskWaitForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lbPurchaseTasks = new System.Windows.Forms.ListBox();
            this.checkPurchaseTasksTimer = new System.Windows.Forms.Timer();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Location = new System.Drawing.Point(3, 242);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(232, 50);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "ОТМЕНА";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnOK.Location = new System.Drawing.Point(3, 186);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(232, 50);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "НАЧАТЬ ПРИЕМКУ";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lbPurchaseTasks
            // 
            this.lbPurchaseTasks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPurchaseTasks.Location = new System.Drawing.Point(3, 3);
            this.lbPurchaseTasks.Name = "lbPurchaseTasks";
            this.lbPurchaseTasks.Size = new System.Drawing.Size(232, 178);
            this.lbPurchaseTasks.TabIndex = 7;
            this.lbPurchaseTasks.SelectedValueChanged += new System.EventHandler(this.lbPurchaseTasks_SelectedValueChanged);
            // 
            // checkPurchaseTasksTimer
            // 
            this.checkPurchaseTasksTimer.Interval = 5000;
            this.checkPurchaseTasksTimer.Tick += new System.EventHandler(this.OnTimer);
            // 
            // PurchaseTaskWaitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 295);
            this.ControlBox = false;
            this.Controls.Add(this.lbPurchaseTasks);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Name = "PurchaseTaskWaitForm";
            this.Text = "Ожидание задания...";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ListBox lbPurchaseTasks;
        private System.Windows.Forms.Timer checkPurchaseTasksTimer;
    }
}
