namespace Cen.Wms.Client.Forms.Purchase
{
    partial class PurchaseTaskContentScanForm
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
            this.btnPause = new System.Windows.Forms.Button();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.btnInputBarcodeGood = new System.Windows.Forms.Button();
            this.lblStats = new System.Windows.Forms.Label();
            this.btnInputBarcodePallet = new System.Windows.Forms.Button();
            this.lblPaletteStats = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnPause
            // 
            this.btnPause.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPause.BackColor = System.Drawing.Color.DarkOrange;
            this.btnPause.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular);
            this.btnPause.ForeColor = System.Drawing.Color.White;
            this.btnPause.Location = new System.Drawing.Point(93, 3);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(97, 50);
            this.btnPause.TabIndex = 1;
            this.btnPause.Text = "ПАУЗА";
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // pnlGrid
            // 
            this.pnlGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlGrid.Location = new System.Drawing.Point(0, 59);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new System.Drawing.Size(238, 206);
            // 
            // btnInputBarcodeGood
            // 
            this.btnInputBarcodeGood.BackColor = System.Drawing.Color.DarkBlue;
            this.btnInputBarcodeGood.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnInputBarcodeGood.ForeColor = System.Drawing.Color.White;
            this.btnInputBarcodeGood.Location = new System.Drawing.Point(3, 3);
            this.btnInputBarcodeGood.Name = "btnInputBarcodeGood";
            this.btnInputBarcodeGood.Size = new System.Drawing.Size(39, 50);
            this.btnInputBarcodeGood.TabIndex = 2;
            this.btnInputBarcodeGood.Text = "Ш";
            this.btnInputBarcodeGood.Click += new System.EventHandler(this.btnInputBarcodeGood_Click);
            // 
            // lblStats
            // 
            this.lblStats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStats.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular);
            this.lblStats.Location = new System.Drawing.Point(119, 268);
            this.lblStats.Name = "lblStats";
            this.lblStats.Size = new System.Drawing.Size(116, 24);
            this.lblStats.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblStats.ParentChanged += new System.EventHandler(this.lblStats_ParentChanged);
            // 
            // btnInputBarcodePallet
            // 
            this.btnInputBarcodePallet.BackColor = System.Drawing.Color.Purple;
            this.btnInputBarcodePallet.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnInputBarcodePallet.ForeColor = System.Drawing.Color.White;
            this.btnInputBarcodePallet.Location = new System.Drawing.Point(48, 3);
            this.btnInputBarcodePallet.Name = "btnInputBarcodePallet";
            this.btnInputBarcodePallet.Size = new System.Drawing.Size(39, 50);
            this.btnInputBarcodePallet.TabIndex = 4;
            this.btnInputBarcodePallet.Text = "П";
            this.btnInputBarcodePallet.Click += new System.EventHandler(this.btnInputBarcodePallet_Click);
            // 
            // lblPaletteStats
            // 
            this.lblPaletteStats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPaletteStats.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular);
            this.lblPaletteStats.Location = new System.Drawing.Point(3, 268);
            this.lblPaletteStats.Name = "lblPaletteStats";
            this.lblPaletteStats.Size = new System.Drawing.Size(120, 24);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.BackColor = System.Drawing.Color.DarkGreen;
            this.btnRefresh.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(196, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(39, 50);
            this.btnRefresh.TabIndex = 7;
            this.btnRefresh.Text = "R";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // PurchaseTaskContentScanForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 295);
            this.ControlBox = false;
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lblPaletteStats);
            this.Controls.Add(this.btnInputBarcodePallet);
            this.Controls.Add(this.lblStats);
            this.Controls.Add(this.btnInputBarcodeGood);
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.btnPause);
            this.Name = "PurchaseTaskContentScanForm";
            this.Text = "Позиции в задаче";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.Button btnInputBarcodeGood;
        private System.Windows.Forms.Label lblStats;
        private System.Windows.Forms.Button btnInputBarcodePallet;
        private System.Windows.Forms.Label lblPaletteStats;
        private System.Windows.Forms.Button btnRefresh;

    }
}