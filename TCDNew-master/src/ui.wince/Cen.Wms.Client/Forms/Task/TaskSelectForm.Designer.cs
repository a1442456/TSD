namespace Cen.Wms.Client.Forms.Task
{
    partial class TaskSelectForm
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
            this.btnPurchaseByTask = new System.Windows.Forms.Button();
            this.lblDescription = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnPurchaseByPapers = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnPurchaseByTask
            // 
            this.btnPurchaseByTask.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPurchaseByTask.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnPurchaseByTask.Location = new System.Drawing.Point(0, 96);
            this.btnPurchaseByTask.Name = "btnPurchaseByTask";
            this.btnPurchaseByTask.Size = new System.Drawing.Size(238, 50);
            this.btnPurchaseByTask.TabIndex = 0;
            this.btnPurchaseByTask.Text = "Приемка по заданию";
            this.btnPurchaseByTask.Click += new System.EventHandler(this.btnPurchaseByTask_Click);
            // 
            // lblDescription
            // 
            this.lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescription.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Italic);
            this.lblDescription.Location = new System.Drawing.Point(0, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(238, 50);
            this.lblDescription.Text = "Выберите вид работ:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnExit.Location = new System.Drawing.Point(0, 245);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(238, 50);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "ВЫХОД";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnPurchaseByPapers
            // 
            this.btnPurchaseByPapers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPurchaseByPapers.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnPurchaseByPapers.Location = new System.Drawing.Point(0, 152);
            this.btnPurchaseByPapers.Name = "btnPurchaseByPapers";
            this.btnPurchaseByPapers.Size = new System.Drawing.Size(238, 50);
            this.btnPurchaseByPapers.TabIndex = 5;
            this.btnPurchaseByPapers.Text = "Приемка по КЛП";
            this.btnPurchaseByPapers.Click += new System.EventHandler(this.btnPurchaseByPapers_Click);
            // 
            // TaskSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 295);
            this.ControlBox = false;
            this.Controls.Add(this.btnPurchaseByPapers);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.btnPurchaseByTask);
            this.Name = "TaskSelectForm";
            this.Text = "Вид работ:";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPurchaseByTask;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnPurchaseByPapers;
    }
}