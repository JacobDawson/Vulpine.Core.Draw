namespace ImagingTests.Pallets
{
    partial class Quantization
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
            this.pnlBorder2 = new System.Windows.Forms.Panel();
            this.pnlPallet = new System.Windows.Forms.Panel();
            this.barProgress = new System.Windows.Forms.ProgressBar();
            this.pnlBorder1 = new System.Windows.Forms.Panel();
            this.pnlCanvas = new System.Windows.Forms.Panel();
            this.btnLoad = new System.Windows.Forms.Button();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.lblcolors = new System.Windows.Forms.Label();
            this.numColors = new System.Windows.Forms.NumericUpDown();
            this.btnFloyd = new System.Windows.Forms.Button();
            this.btnDither = new System.Windows.Forms.Button();
            this.btnPallet = new System.Windows.Forms.Button();
            this.btnOriginal = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblDownsample = new System.Windows.Forms.Label();
            this.cboDownsample = new System.Windows.Forms.ComboBox();
            this.lblError = new System.Windows.Forms.Label();
            this.lblIttr = new System.Windows.Forms.Label();
            this.pnlStats = new System.Windows.Forms.Panel();
            this.pnlBorder2.SuspendLayout();
            this.pnlBorder1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numColors)).BeginInit();
            this.pnlStats.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBorder2
            // 
            this.pnlBorder2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBorder2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlBorder2.Controls.Add(this.pnlPallet);
            this.pnlBorder2.Location = new System.Drawing.Point(6, 488);
            this.pnlBorder2.Name = "pnlBorder2";
            this.pnlBorder2.Size = new System.Drawing.Size(758, 29);
            this.pnlBorder2.TabIndex = 42;
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
            // barProgress
            // 
            this.barProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.barProgress.Location = new System.Drawing.Point(6, 523);
            this.barProgress.Maximum = 500;
            this.barProgress.Name = "barProgress";
            this.barProgress.Size = new System.Drawing.Size(758, 23);
            this.barProgress.TabIndex = 41;
            // 
            // pnlBorder1
            // 
            this.pnlBorder1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBorder1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlBorder1.Controls.Add(this.pnlCanvas);
            this.pnlBorder1.Location = new System.Drawing.Point(284, 4);
            this.pnlBorder1.Name = "pnlBorder1";
            this.pnlBorder1.Size = new System.Drawing.Size(480, 480);
            this.pnlBorder1.TabIndex = 40;
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
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(6, 32);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(272, 23);
            this.btnLoad.TabIndex = 44;
            this.btnLoad.Text = "Load File";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(6, 6);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.ReadOnly = true;
            this.txtFileName.Size = new System.Drawing.Size(272, 20);
            this.txtFileName.TabIndex = 43;
            // 
            // lblcolors
            // 
            this.lblcolors.AutoSize = true;
            this.lblcolors.Location = new System.Drawing.Point(5, 70);
            this.lblcolors.Margin = new System.Windows.Forms.Padding(3);
            this.lblcolors.Name = "lblcolors";
            this.lblcolors.Size = new System.Drawing.Size(91, 13);
            this.lblcolors.TabIndex = 45;
            this.lblcolors.Text = "Number of Colors:";
            // 
            // numColors
            // 
            this.numColors.Location = new System.Drawing.Point(8, 89);
            this.numColors.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numColors.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numColors.Name = "numColors";
            this.numColors.Size = new System.Drawing.Size(267, 20);
            this.numColors.TabIndex = 46;
            this.numColors.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // btnFloyd
            // 
            this.btnFloyd.Location = new System.Drawing.Point(6, 350);
            this.btnFloyd.Name = "btnFloyd";
            this.btnFloyd.Size = new System.Drawing.Size(272, 34);
            this.btnFloyd.TabIndex = 52;
            this.btnFloyd.Text = "Floyd Steinberg";
            this.btnFloyd.UseVisualStyleBackColor = true;
            this.btnFloyd.Click += new System.EventHandler(this.btnFloyd_Click);
            // 
            // btnDither
            // 
            this.btnDither.Location = new System.Drawing.Point(6, 310);
            this.btnDither.Name = "btnDither";
            this.btnDither.Size = new System.Drawing.Size(272, 34);
            this.btnDither.TabIndex = 51;
            this.btnDither.Text = "Apply Dither";
            this.btnDither.UseVisualStyleBackColor = true;
            this.btnDither.Click += new System.EventHandler(this.btnDither_Click);
            // 
            // btnPallet
            // 
            this.btnPallet.Location = new System.Drawing.Point(6, 270);
            this.btnPallet.Name = "btnPallet";
            this.btnPallet.Size = new System.Drawing.Size(272, 34);
            this.btnPallet.TabIndex = 50;
            this.btnPallet.Text = "Apply Pallet";
            this.btnPallet.UseVisualStyleBackColor = true;
            this.btnPallet.Click += new System.EventHandler(this.btnPallet_Click);
            // 
            // btnOriginal
            // 
            this.btnOriginal.Location = new System.Drawing.Point(6, 230);
            this.btnOriginal.Name = "btnOriginal";
            this.btnOriginal.Size = new System.Drawing.Size(272, 34);
            this.btnOriginal.TabIndex = 49;
            this.btnOriginal.Text = "Show Original";
            this.btnOriginal.UseVisualStyleBackColor = true;
            this.btnOriginal.Click += new System.EventHandler(this.btnOriginal_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(6, 161);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(272, 34);
            this.btnGenerate.TabIndex = 53;
            this.btnGenerate.Text = "Generate Pallet";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // lblTime
            // 
            this.lblTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(3, 69);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(54, 13);
            this.lblTime.TabIndex = 54;
            this.lblTime.Text = "Time: ???";
            // 
            // lblDownsample
            // 
            this.lblDownsample.AutoSize = true;
            this.lblDownsample.Location = new System.Drawing.Point(5, 115);
            this.lblDownsample.Margin = new System.Windows.Forms.Padding(3);
            this.lblDownsample.Name = "lblDownsample";
            this.lblDownsample.Size = new System.Drawing.Size(85, 13);
            this.lblDownsample.TabIndex = 55;
            this.lblDownsample.Text = "Downsampeling:";
            // 
            // cboDownsample
            // 
            this.cboDownsample.FormattingEnabled = true;
            this.cboDownsample.Items.AddRange(new object[] {
            "32 x 32",
            "64 x 64",
            "128 x 128",
            "256 x 256",
            "32 x 32 (Unique)",
            "64 x 64 (Unique)",
            "128 x 128 (Unique)",
            "256 x 256 (Unique)"});
            this.cboDownsample.Location = new System.Drawing.Point(8, 134);
            this.cboDownsample.Name = "cboDownsample";
            this.cboDownsample.Size = new System.Drawing.Size(267, 21);
            this.cboDownsample.TabIndex = 56;
            // 
            // lblError
            // 
            this.lblError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblError.AutoSize = true;
            this.lblError.Location = new System.Drawing.Point(4, 45);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(53, 13);
            this.lblError.TabIndex = 57;
            this.lblError.Text = "Error: ???";
            // 
            // lblIttr
            // 
            this.lblIttr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblIttr.AutoSize = true;
            this.lblIttr.Location = new System.Drawing.Point(3, 20);
            this.lblIttr.Name = "lblIttr";
            this.lblIttr.Size = new System.Drawing.Size(65, 13);
            this.lblIttr.TabIndex = 58;
            this.lblIttr.Text = "Itterations: 0";
            // 
            // pnlStats
            // 
            this.pnlStats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlStats.Controls.Add(this.lblIttr);
            this.pnlStats.Controls.Add(this.lblError);
            this.pnlStats.Controls.Add(this.lblTime);
            this.pnlStats.Location = new System.Drawing.Point(6, 390);
            this.pnlStats.Name = "pnlStats";
            this.pnlStats.Size = new System.Drawing.Size(269, 94);
            this.pnlStats.TabIndex = 59;
            // 
            // Quantization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlStats);
            this.Controls.Add(this.cboDownsample);
            this.Controls.Add(this.lblDownsample);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnFloyd);
            this.Controls.Add(this.btnDither);
            this.Controls.Add(this.btnPallet);
            this.Controls.Add(this.btnOriginal);
            this.Controls.Add(this.numColors);
            this.Controls.Add(this.lblcolors);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.pnlBorder2);
            this.Controls.Add(this.barProgress);
            this.Controls.Add(this.pnlBorder1);
            this.Name = "Quantization";
            this.Size = new System.Drawing.Size(770, 550);
            this.pnlBorder2.ResumeLayout(false);
            this.pnlBorder1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numColors)).EndInit();
            this.pnlStats.ResumeLayout(false);
            this.pnlStats.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlBorder2;
        private System.Windows.Forms.Panel pnlPallet;
        private System.Windows.Forms.ProgressBar barProgress;
        private System.Windows.Forms.Panel pnlBorder1;
        private System.Windows.Forms.Panel pnlCanvas;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label lblcolors;
        private System.Windows.Forms.NumericUpDown numColors;
        private System.Windows.Forms.Button btnFloyd;
        private System.Windows.Forms.Button btnDither;
        private System.Windows.Forms.Button btnPallet;
        private System.Windows.Forms.Button btnOriginal;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblDownsample;
        private System.Windows.Forms.ComboBox cboDownsample;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Label lblIttr;
        private System.Windows.Forms.Panel pnlStats;
    }
}
