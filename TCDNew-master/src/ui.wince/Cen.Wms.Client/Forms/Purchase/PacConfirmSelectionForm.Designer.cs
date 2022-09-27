namespace Cen.Wms.Client.Forms.Purchase
{
    partial class PacConfirmSelectionForm
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
            this.btnStart = new System.Windows.Forms.Button();
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblId = new System.Windows.Forms.Label();
            this.lblSupplier = new System.Windows.Forms.Label();
            this.lblGate = new System.Windows.Forms.Label();
            this.lblIdS = new System.Windows.Forms.Label();
            this.lblSupplierS = new System.Windows.Forms.Label();
            this.lblGateS = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.BackColor = System.Drawing.Color.DarkGreen;
            this.btnStart.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnStart.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(0, 242);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(238, 50);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "ДОБАВИТЬ В СПИСОК";
            // 
            // lblHeader
            // 
            this.lblHeader.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.lblHeader.Location = new System.Drawing.Point(0, 3);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(238, 32);
            this.lblHeader.Text = "Закупка";
            // 
            // lblId
            // 
            this.lblId.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.lblId.Location = new System.Drawing.Point(7, 32);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(228, 20);
            this.lblId.Text = "Код:";
            // 
            // lblSupplier
            // 
            this.lblSupplier.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.lblSupplier.Location = new System.Drawing.Point(7, 72);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(228, 20);
            this.lblSupplier.Text = "Поставщик:";
            // 
            // lblGate
            // 
            this.lblGate.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.lblGate.Location = new System.Drawing.Point(10, 142);
            this.lblGate.Name = "lblGate";
            this.lblGate.Size = new System.Drawing.Size(228, 20);
            this.lblGate.Text = "Рампа:";
            // 
            // lblIdS
            // 
            this.lblIdS.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.lblIdS.Location = new System.Drawing.Point(23, 52);
            this.lblIdS.Name = "lblIdS";
            this.lblIdS.Size = new System.Drawing.Size(212, 20);
            // 
            // lblSupplierS
            // 
            this.lblSupplierS.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.lblSupplierS.Location = new System.Drawing.Point(23, 92);
            this.lblSupplierS.Name = "lblSupplierS";
            this.lblSupplierS.Size = new System.Drawing.Size(212, 50);
            // 
            // lblGateS
            // 
            this.lblGateS.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.lblGateS.Location = new System.Drawing.Point(26, 162);
            this.lblGateS.Name = "lblGateS";
            this.lblGateS.Size = new System.Drawing.Size(212, 20);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.DarkRed;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(185, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(50, 50);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "X";
            // 
            // FormInfoPurchase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 295);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblGateS);
            this.Controls.Add(this.lblSupplierS);
            this.Controls.Add(this.lblIdS);
            this.Controls.Add(this.lblGate);
            this.Controls.Add(this.lblSupplier);
            this.Controls.Add(this.lblId);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.btnStart);
            this.Name = "FormInfoPurchase";
            this.Text = "Информация о закупке";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.Label lblSupplier;
        private System.Windows.Forms.Label lblGate;
        private System.Windows.Forms.Label lblIdS;
        private System.Windows.Forms.Label lblSupplierS;
        private System.Windows.Forms.Label lblGateS;
        private System.Windows.Forms.Button btnCancel;
    }
}