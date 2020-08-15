﻿namespace ImagingTests.Pallets
{
    partial class PalletSelector
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.barProgress = new System.Windows.Forms.ProgressBar();
            this.pnlBorder1 = new System.Windows.Forms.Panel();
            this.pnlCanvas = new System.Windows.Forms.Panel();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.cmbPallet = new System.Windows.Forms.ComboBox();
            this.lblPallet = new System.Windows.Forms.Label();
            this.btnOriginal = new System.Windows.Forms.Button();
            this.btnPallet = new System.Windows.Forms.Button();
            this.btnDither = new System.Windows.Forms.Button();
            this.lblTime = new System.Windows.Forms.Label();
            this.pnlBorder1.SuspendLayout();
            this.SuspendLayout();
            // 
            // barProgress
            // 
            this.barProgress.Location = new System.Drawing.Point(5, 522);
            this.barProgress.Maximum = 500;
            this.barProgress.Name = "barProgress";
            this.barProgress.Size = new System.Drawing.Size(758, 23);
            this.barProgress.TabIndex = 25;
            // 
            // pnlBorder1
            // 
            this.pnlBorder1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlBorder1.Controls.Add(this.pnlCanvas);
            this.pnlBorder1.Location = new System.Drawing.Point(283, 3);
            this.pnlBorder1.Name = "pnlBorder1";
            this.pnlBorder1.Size = new System.Drawing.Size(480, 480);
            this.pnlBorder1.TabIndex = 24;
            // 
            // pnlCanvas
            // 
            this.pnlCanvas.BackColor = System.Drawing.Color.White;
            this.pnlCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCanvas.Location = new System.Drawing.Point(0, 0);
            this.pnlCanvas.Name = "pnlCanvas";
            this.pnlCanvas.Size = new System.Drawing.Size(476, 476);
            this.pnlCanvas.TabIndex = 0;
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(5, 5);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.ReadOnly = true;
            this.txtFileName.Size = new System.Drawing.Size(272, 20);
            this.txtFileName.TabIndex = 26;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(5, 31);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(272, 23);
            this.btnLoad.TabIndex = 27;
            this.btnLoad.Text = "Load File";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // cmbPallet
            // 
            this.cmbPallet.FormattingEnabled = true;
            this.cmbPallet.Location = new System.Drawing.Point(5, 87);
            this.cmbPallet.Name = "cmbPallet";
            this.cmbPallet.Size = new System.Drawing.Size(272, 21);
            this.cmbPallet.TabIndex = 28;
            // 
            // lblPallet
            // 
            this.lblPallet.AutoSize = true;
            this.lblPallet.Location = new System.Drawing.Point(5, 68);
            this.lblPallet.Name = "lblPallet";
            this.lblPallet.Size = new System.Drawing.Size(66, 13);
            this.lblPallet.TabIndex = 29;
            this.lblPallet.Text = "Select Pallet";
            // 
            // btnOriginal
            // 
            this.btnOriginal.Location = new System.Drawing.Point(5, 174);
            this.btnOriginal.Name = "btnOriginal";
            this.btnOriginal.Size = new System.Drawing.Size(272, 34);
            this.btnOriginal.TabIndex = 30;
            this.btnOriginal.Text = "Show Original";
            this.btnOriginal.UseVisualStyleBackColor = true;
            // 
            // btnPallet
            // 
            this.btnPallet.Location = new System.Drawing.Point(5, 214);
            this.btnPallet.Name = "btnPallet";
            this.btnPallet.Size = new System.Drawing.Size(272, 34);
            this.btnPallet.TabIndex = 31;
            this.btnPallet.Text = "Apply Pallet";
            this.btnPallet.UseVisualStyleBackColor = true;
            // 
            // btnDither
            // 
            this.btnDither.Location = new System.Drawing.Point(5, 254);
            this.btnDither.Name = "btnDither";
            this.btnDither.Size = new System.Drawing.Size(272, 34);
            this.btnDither.TabIndex = 32;
            this.btnDither.Text = "Apply Dither";
            this.btnDither.UseVisualStyleBackColor = true;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(17, 486);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(54, 13);
            this.lblTime.TabIndex = 33;
            this.lblTime.Text = "Time: ???";
            // 
            // PalletSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.btnDither);
            this.Controls.Add(this.btnPallet);
            this.Controls.Add(this.btnOriginal);
            this.Controls.Add(this.lblPallet);
            this.Controls.Add(this.cmbPallet);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.barProgress);
            this.Controls.Add(this.pnlBorder1);
            this.Name = "PalletSelector";
            this.Size = new System.Drawing.Size(770, 550);
            this.pnlBorder1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar barProgress;
        private System.Windows.Forms.Panel pnlBorder1;
        private System.Windows.Forms.Panel pnlCanvas;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.ComboBox cmbPallet;
        private System.Windows.Forms.Label lblPallet;
        private System.Windows.Forms.Button btnOriginal;
        private System.Windows.Forms.Button btnPallet;
        private System.Windows.Forms.Button btnDither;
        private System.Windows.Forms.Label lblTime;
    }
}
