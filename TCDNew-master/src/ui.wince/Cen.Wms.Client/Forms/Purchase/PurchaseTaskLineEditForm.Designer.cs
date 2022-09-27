namespace Cen.Wms.Client.Forms.Purchase
{
    partial class PurchaseTaskLineEditForm
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
            this.lblName = new System.Windows.Forms.Label();
            this.lblNameS = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnRetry = new System.Windows.Forms.Button();
            this.lblSumS = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblTotalS = new System.Windows.Forms.Label();
            this.lblSum = new System.Windows.Forms.Label();
            this.tcQuantity = new System.Windows.Forms.TabControl();
            this.tpQunatity = new System.Windows.Forms.TabPage();
            this.lblSumQuantity = new System.Windows.Forms.Label();
            this.lblEquals = new System.Windows.Forms.Label();
            this.lblPlus = new System.Windows.Forms.Label();
            this.tbQuantityAdd = new System.Windows.Forms.TextBox();
            this.tbQuantityBefore = new System.Windows.Forms.TextBox();
            this.tpBroken = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSumBroken = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbBrokenAdd = new System.Windows.Forms.TextBox();
            this.tbBrokenBefore = new System.Windows.Forms.TextBox();
            this.cbDateExp = new System.Windows.Forms.CheckBox();
            this.pDateExp = new System.Windows.Forms.DateTimePicker();
            this.tbDaysExp = new System.Windows.Forms.TextBox();
            this.lblDaysExp = new System.Windows.Forms.Label();
            this.lblPlus2 = new System.Windows.Forms.Label();
            this.tcQuantity.SuspendLayout();
            this.tpQunatity.SuspendLayout();
            this.tpBroken.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblName.Location = new System.Drawing.Point(0, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(238, 20);
            this.lblName.Text = "Наименование:";
            // 
            // lblNameS
            // 
            this.lblNameS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNameS.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblNameS.Location = new System.Drawing.Point(3, 20);
            this.lblNameS.Name = "lblNameS";
            this.lblNameS.Size = new System.Drawing.Size(232, 40);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.BackColor = System.Drawing.Color.DarkGreen;
            this.btnOK.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(70, 228);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(165, 64);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "ОК";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnRetry
            // 
            this.btnRetry.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRetry.BackColor = System.Drawing.Color.DarkRed;
            this.btnRetry.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnRetry.ForeColor = System.Drawing.Color.White;
            this.btnRetry.Location = new System.Drawing.Point(3, 228);
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.Size = new System.Drawing.Size(61, 64);
            this.btnRetry.TabIndex = 3;
            this.btnRetry.Text = "ПЕРЕ\r\n";
            this.btnRetry.Click += new System.EventHandler(this.btnRetry_Click);
            // 
            // lblSumS
            // 
            this.lblSumS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSumS.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.lblSumS.Location = new System.Drawing.Point(147, 170);
            this.lblSumS.Name = "lblSumS";
            this.lblSumS.Size = new System.Drawing.Size(86, 29);
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotal.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.lblTotal.Location = new System.Drawing.Point(1, 199);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(126, 29);
            this.lblTotal.Text = "ЗАКУПКА:";
            // 
            // lblTotalS
            // 
            this.lblTotalS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalS.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.lblTotalS.Location = new System.Drawing.Point(147, 199);
            this.lblTotalS.Name = "lblTotalS";
            this.lblTotalS.Size = new System.Drawing.Size(86, 29);
            // 
            // lblSum
            // 
            this.lblSum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSum.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.lblSum.Location = new System.Drawing.Point(1, 170);
            this.lblSum.Name = "lblSum";
            this.lblSum.Size = new System.Drawing.Size(126, 29);
            this.lblSum.Text = "ИТОГО:";
            // 
            // tcQuantity
            // 
            this.tcQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcQuantity.Controls.Add(this.tpQunatity);
            this.tcQuantity.Controls.Add(this.tpBroken);
            this.tcQuantity.Location = new System.Drawing.Point(3, 118);
            this.tcQuantity.Name = "tcQuantity";
            this.tcQuantity.SelectedIndex = 0;
            this.tcQuantity.Size = new System.Drawing.Size(232, 56);
            this.tcQuantity.TabIndex = 32;
            // 
            // tpQunatity
            // 
            this.tpQunatity.Controls.Add(this.lblSumQuantity);
            this.tpQunatity.Controls.Add(this.lblEquals);
            this.tpQunatity.Controls.Add(this.lblPlus);
            this.tpQunatity.Controls.Add(this.tbQuantityAdd);
            this.tpQunatity.Controls.Add(this.tbQuantityBefore);
            this.tpQunatity.Location = new System.Drawing.Point(4, 25);
            this.tpQunatity.Name = "tpQunatity";
            this.tpQunatity.Size = new System.Drawing.Size(224, 27);
            this.tpQunatity.Text = "Количество";
            // 
            // lblSumQuantity
            // 
            this.lblSumQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSumQuantity.BackColor = System.Drawing.SystemColors.Window;
            this.lblSumQuantity.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.lblSumQuantity.Location = new System.Drawing.Point(167, 2);
            this.lblSumQuantity.Name = "lblSumQuantity";
            this.lblSumQuantity.Size = new System.Drawing.Size(54, 23);
            this.lblSumQuantity.Text = "0";
            // 
            // lblEquals
            // 
            this.lblEquals.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.lblEquals.Location = new System.Drawing.Point(142, 2);
            this.lblEquals.Name = "lblEquals";
            this.lblEquals.Size = new System.Drawing.Size(23, 23);
            this.lblEquals.Text = "=";
            // 
            // lblPlus
            // 
            this.lblPlus.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.lblPlus.Location = new System.Drawing.Point(61, 2);
            this.lblPlus.Name = "lblPlus";
            this.lblPlus.Size = new System.Drawing.Size(23, 23);
            this.lblPlus.Text = "+";
            // 
            // tbQuantityAdd
            // 
            this.tbQuantityAdd.Location = new System.Drawing.Point(86, 2);
            this.tbQuantityAdd.Name = "tbQuantityAdd";
            this.tbQuantityAdd.Size = new System.Drawing.Size(54, 23);
            this.tbQuantityAdd.TabIndex = 12;
            this.tbQuantityAdd.Text = "0";
            // 
            // tbQuantityBefore
            // 
            this.tbQuantityBefore.Enabled = false;
            this.tbQuantityBefore.Location = new System.Drawing.Point(5, 2);
            this.tbQuantityBefore.Name = "tbQuantityBefore";
            this.tbQuantityBefore.Size = new System.Drawing.Size(54, 23);
            this.tbQuantityBefore.TabIndex = 16;
            this.tbQuantityBefore.Text = "0";
            // 
            // tpBroken
            // 
            this.tpBroken.Controls.Add(this.label1);
            this.tpBroken.Controls.Add(this.lblSumBroken);
            this.tpBroken.Controls.Add(this.label3);
            this.tpBroken.Controls.Add(this.tbBrokenAdd);
            this.tpBroken.Controls.Add(this.tbBrokenBefore);
            this.tpBroken.Location = new System.Drawing.Point(4, 25);
            this.tpBroken.Name = "tpBroken";
            this.tpBroken.Size = new System.Drawing.Size(224, 27);
            this.tpBroken.Text = "Брак";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(142, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 23);
            this.label1.Text = "=";
            // 
            // lblSumBroken
            // 
            this.lblSumBroken.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSumBroken.BackColor = System.Drawing.Color.Pink;
            this.lblSumBroken.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.lblSumBroken.Location = new System.Drawing.Point(167, 2);
            this.lblSumBroken.Name = "lblSumBroken";
            this.lblSumBroken.Size = new System.Drawing.Size(54, 23);
            this.lblSumBroken.Text = "0";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(61, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 23);
            this.label3.Text = "+";
            // 
            // tbBrokenAdd
            // 
            this.tbBrokenAdd.BackColor = System.Drawing.Color.Pink;
            this.tbBrokenAdd.Location = new System.Drawing.Point(86, 2);
            this.tbBrokenAdd.Name = "tbBrokenAdd";
            this.tbBrokenAdd.Size = new System.Drawing.Size(54, 23);
            this.tbBrokenAdd.TabIndex = 26;
            this.tbBrokenAdd.Text = "0";
            // 
            // tbBrokenBefore
            // 
            this.tbBrokenBefore.BackColor = System.Drawing.Color.Pink;
            this.tbBrokenBefore.Enabled = false;
            this.tbBrokenBefore.Location = new System.Drawing.Point(5, 2);
            this.tbBrokenBefore.Name = "tbBrokenBefore";
            this.tbBrokenBefore.Size = new System.Drawing.Size(54, 23);
            this.tbBrokenBefore.TabIndex = 27;
            this.tbBrokenBefore.Text = "0";
            // 
            // cbDateExp
            // 
            this.cbDateExp.Checked = true;
            this.cbDateExp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDateExp.Location = new System.Drawing.Point(3, 63);
            this.cbDateExp.Name = "cbDateExp";
            this.cbDateExp.Size = new System.Drawing.Size(93, 49);
            this.cbDateExp.TabIndex = 51;
            this.cbDateExp.Text = "Срок\r\nгодности:";
            // 
            // pDateExp
            // 
            this.pDateExp.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.pDateExp.Location = new System.Drawing.Point(102, 63);
            this.pDateExp.Name = "pDateExp";
            this.pDateExp.Size = new System.Drawing.Size(133, 24);
            this.pDateExp.TabIndex = 1;
            this.pDateExp.Value = new System.DateTime(2014, 1, 1, 0, 0, 0, 0);
            // 
            // tbDaysExp
            // 
            this.tbDaysExp.Location = new System.Drawing.Point(129, 93);
            this.tbDaysExp.Name = "tbDaysExp";
            this.tbDaysExp.Size = new System.Drawing.Size(61, 23);
            this.tbDaysExp.TabIndex = 43;
            // 
            // lblDaysExp
            // 
            this.lblDaysExp.Location = new System.Drawing.Point(196, 95);
            this.lblDaysExp.Name = "lblDaysExp";
            this.lblDaysExp.Size = new System.Drawing.Size(37, 20);
            this.lblDaysExp.Text = "дней";
            // 
            // lblPlus2
            // 
            this.lblPlus2.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.lblPlus2.Location = new System.Drawing.Point(102, 92);
            this.lblPlus2.Name = "lblPlus2";
            this.lblPlus2.Size = new System.Drawing.Size(23, 23);
            this.lblPlus2.Text = "+";
            // 
            // PurchaseTaskLineEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 295);
            this.ControlBox = false;
            this.Controls.Add(this.cbDateExp);
            this.Controls.Add(this.lblPlus2);
            this.Controls.Add(this.lblDaysExp);
            this.Controls.Add(this.tbDaysExp);
            this.Controls.Add(this.tcQuantity);
            this.Controls.Add(this.lblSum);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.lblTotalS);
            this.Controls.Add(this.lblSumS);
            this.Controls.Add(this.btnRetry);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.pDateExp);
            this.Controls.Add(this.lblNameS);
            this.Controls.Add(this.lblName);
            this.Name = "PurchaseTaskLineEditForm";
            this.Text = "Отсканированная позиция";
            this.tcQuantity.ResumeLayout(false);
            this.tpQunatity.ResumeLayout(false);
            this.tpBroken.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblNameS;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnRetry;
        private System.Windows.Forms.Label lblSumS;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblTotalS;
        private System.Windows.Forms.Label lblSum;
        private System.Windows.Forms.TabControl tcQuantity;
        private System.Windows.Forms.TabPage tpQunatity;
        private System.Windows.Forms.TabPage tpBroken;
        private System.Windows.Forms.Label lblSumQuantity;
        private System.Windows.Forms.Label lblEquals;
        private System.Windows.Forms.Label lblPlus;
        private System.Windows.Forms.TextBox tbQuantityAdd;
        private System.Windows.Forms.TextBox tbQuantityBefore;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSumBroken;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbBrokenAdd;
        private System.Windows.Forms.TextBox tbBrokenBefore;
        private System.Windows.Forms.CheckBox cbDateExp;
        private System.Windows.Forms.DateTimePicker pDateExp;
        private System.Windows.Forms.TextBox tbDaysExp;
        private System.Windows.Forms.Label lblDaysExp;
        private System.Windows.Forms.Label lblPlus2;


    }
}