namespace ImagingTests.Pallets
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnFloyd = new System.Windows.Forms.Button();
            this.lblAmount = new System.Windows.Forms.Label();
            this.cmbAmount = new System.Windows.Forms.ComboBox();
            this.pnlBorder2 = new System.Windows.Forms.Panel();
            this.pnlPallet = new System.Windows.Forms.Panel();
            this.pnlBorder1.SuspendLayout();
            this.pnlBorder2.SuspendLayout();
            this.SuspendLayout();
            // 
            // barProgress
            // 
            this.barProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.barProgress.Location = new System.Drawing.Point(5, 522);
            this.barProgress.Maximum = 500;
            this.barProgress.Name = "barProgress";
            this.barProgress.Size = new System.Drawing.Size(758, 23);
            this.barProgress.TabIndex = 25;
            // 
            // pnlBorder1
            // 
            this.pnlBorder1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
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
            this.cmbPallet.Items.AddRange(new object[] {
            "Black And White",
            "Normal",
            "Pastel",
            "Vibrant",
            "Earth Tones",
            "Orange-Blue-Pink",
            "Purple-Orange-Green",
            "Warm Colors",
            "Cool Colors",
            "Skintones",
            "Hair Color",
            "CGA-1",
            "CGA-2",
            "EGA",
            "NTSC Artifact",
            "NTSC Artifact G-R-Y",
            "NTSC Artifact C-M-W",
            "CGA-3",
            "CGA-4",
            "Apple II",
            "Macintosh",
            "Gameboy",
            "NES",
            "Mario Paint",
            "MC Wool",
            "MC Clay",
            "MC Stone",
            "MC Wood",
            "MC Wood Plus",
            "Pico-8",
            "DawnBringer16",
            "Kensler16",
            "DawnBringer32"});
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
            this.lblPallet.Size = new System.Drawing.Size(69, 13);
            this.lblPallet.TabIndex = 29;
            this.lblPallet.Text = "Select Pallet:";
            // 
            // btnOriginal
            // 
            this.btnOriginal.Location = new System.Drawing.Point(5, 174);
            this.btnOriginal.Name = "btnOriginal";
            this.btnOriginal.Size = new System.Drawing.Size(272, 34);
            this.btnOriginal.TabIndex = 30;
            this.btnOriginal.Text = "Show Original";
            this.btnOriginal.UseVisualStyleBackColor = true;
            this.btnOriginal.Click += new System.EventHandler(this.btnOriginal_Click);
            // 
            // btnPallet
            // 
            this.btnPallet.Location = new System.Drawing.Point(5, 214);
            this.btnPallet.Name = "btnPallet";
            this.btnPallet.Size = new System.Drawing.Size(272, 34);
            this.btnPallet.TabIndex = 31;
            this.btnPallet.Text = "Apply Pallet";
            this.btnPallet.UseVisualStyleBackColor = true;
            this.btnPallet.Click += new System.EventHandler(this.btnPallet_Click);
            // 
            // btnDither
            // 
            this.btnDither.Location = new System.Drawing.Point(5, 254);
            this.btnDither.Name = "btnDither";
            this.btnDither.Size = new System.Drawing.Size(272, 34);
            this.btnDither.TabIndex = 32;
            this.btnDither.Text = "Apply Dither";
            this.btnDither.UseVisualStyleBackColor = true;
            this.btnDither.Click += new System.EventHandler(this.btnDither_Click);
            // 
            // lblTime
            // 
            this.lblTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(17, 441);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(54, 13);
            this.lblTime.TabIndex = 33;
            this.lblTime.Text = "Time: ???";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(5, 390);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(272, 34);
            this.btnSave.TabIndex = 34;
            this.btnSave.Text = "Save Output";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnFloyd
            // 
            this.btnFloyd.Location = new System.Drawing.Point(5, 294);
            this.btnFloyd.Name = "btnFloyd";
            this.btnFloyd.Size = new System.Drawing.Size(272, 34);
            this.btnFloyd.TabIndex = 35;
            this.btnFloyd.Text = "Floyd Steinberg";
            this.btnFloyd.UseVisualStyleBackColor = true;
            this.btnFloyd.Click += new System.EventHandler(this.btnFloyd_Click);
            // 
            // lblAmount
            // 
            this.lblAmount.AutoSize = true;
            this.lblAmount.Location = new System.Drawing.Point(5, 117);
            this.lblAmount.Margin = new System.Windows.Forms.Padding(3);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(77, 13);
            this.lblAmount.TabIndex = 37;
            this.lblAmount.Text = "Dither Amount:";
            // 
            // cmbAmount
            // 
            this.cmbAmount.FormattingEnabled = true;
            this.cmbAmount.Items.AddRange(new object[] {
            "1/1",
            "1/2",
            "1/4",
            "1/8",
            "1/16",
            "1/32",
            "1/64",
            "1/128",
            "1/256"});
            this.cmbAmount.Location = new System.Drawing.Point(5, 136);
            this.cmbAmount.Name = "cmbAmount";
            this.cmbAmount.Size = new System.Drawing.Size(272, 21);
            this.cmbAmount.TabIndex = 38;
            // 
            // pnlBorder2
            // 
            this.pnlBorder2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBorder2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlBorder2.Controls.Add(this.pnlPallet);
            this.pnlBorder2.Location = new System.Drawing.Point(5, 487);
            this.pnlBorder2.Name = "pnlBorder2";
            this.pnlBorder2.Size = new System.Drawing.Size(758, 29);
            this.pnlBorder2.TabIndex = 39;
            // 
            // pnlPallet
            // 
            this.pnlPallet.BackColor = System.Drawing.Color.White;
            this.pnlPallet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPallet.Location = new System.Drawing.Point(0, 0);
            this.pnlPallet.Name = "pnlPallet";
            this.pnlPallet.Size = new System.Drawing.Size(754, 25);
            this.pnlPallet.TabIndex = 0;
            // 
            // PalletSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlBorder2);
            this.Controls.Add(this.cmbAmount);
            this.Controls.Add(this.lblAmount);
            this.Controls.Add(this.btnFloyd);
            this.Controls.Add(this.btnSave);
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
            this.pnlBorder2.ResumeLayout(false);
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
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnFloyd;
        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.ComboBox cmbAmount;
        private System.Windows.Forms.Panel pnlBorder2;
        private System.Windows.Forms.Panel pnlPallet;
    }
}
