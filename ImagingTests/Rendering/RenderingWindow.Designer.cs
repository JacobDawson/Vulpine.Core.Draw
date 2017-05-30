namespace ImagingTests.Rendering
{
    partial class RenderingWindow
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
            this.pnlBorder = new System.Windows.Forms.Panel();
            this.pnlCanvas = new System.Windows.Forms.Panel();
            this.lblSamp = new System.Windows.Forms.Label();
            this.txtSamp = new System.Windows.Forms.TextBox();
            this.barProgress = new System.Windows.Forms.ProgressBar();
            this.btnLong = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblPattern = new System.Windows.Forms.Label();
            this.cboPattern = new System.Windows.Forms.ComboBox();
            this.chkAA = new System.Windows.Forms.CheckBox();
            this.txtRad = new System.Windows.Forms.TextBox();
            this.lblRad = new System.Windows.Forms.Label();
            this.chkJit = new System.Windows.Forms.CheckBox();
            this.lblWin = new System.Windows.Forms.Label();
            this.cboWin = new System.Windows.Forms.ComboBox();
            this.pnlBorder.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBorder
            // 
            this.pnlBorder.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlBorder.Controls.Add(this.pnlCanvas);
            this.pnlBorder.Location = new System.Drawing.Point(249, 3);
            this.pnlBorder.Name = "pnlBorder";
            this.pnlBorder.Size = new System.Drawing.Size(500, 500);
            this.pnlBorder.TabIndex = 0;
            // 
            // pnlCanvas
            // 
            this.pnlCanvas.BackColor = System.Drawing.Color.White;
            this.pnlCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCanvas.Location = new System.Drawing.Point(0, 0);
            this.pnlCanvas.Name = "pnlCanvas";
            this.pnlCanvas.Size = new System.Drawing.Size(496, 496);
            this.pnlCanvas.TabIndex = 0;
            // 
            // lblSamp
            // 
            this.lblSamp.AutoSize = true;
            this.lblSamp.Location = new System.Drawing.Point(15, 172);
            this.lblSamp.Name = "lblSamp";
            this.lblSamp.Size = new System.Drawing.Size(62, 13);
            this.lblSamp.TabIndex = 3;
            this.lblSamp.Text = "Samples =  ";
            // 
            // txtSamp
            // 
            this.txtSamp.Location = new System.Drawing.Point(92, 169);
            this.txtSamp.Name = "txtSamp";
            this.txtSamp.Size = new System.Drawing.Size(151, 20);
            this.txtSamp.TabIndex = 4;
            this.txtSamp.Text = "16";
            // 
            // barProgress
            // 
            this.barProgress.Location = new System.Drawing.Point(3, 509);
            this.barProgress.Maximum = 500;
            this.barProgress.Name = "barProgress";
            this.barProgress.Size = new System.Drawing.Size(746, 23);
            this.barProgress.TabIndex = 7;
            // 
            // btnLong
            // 
            this.btnLong.Location = new System.Drawing.Point(5, 237);
            this.btnLong.Name = "btnLong";
            this.btnLong.Size = new System.Drawing.Size(112, 23);
            this.btnLong.TabIndex = 8;
            this.btnLong.Text = "Render";
            this.btnLong.UseVisualStyleBackColor = true;
            this.btnLong.Click += new System.EventHandler(this.btnLong_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(123, 237);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(112, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(13, 476);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(54, 13);
            this.lblTime.TabIndex = 12;
            this.lblTime.Text = "Time: ???";
            // 
            // lblPattern
            // 
            this.lblPattern.AutoSize = true;
            this.lblPattern.Location = new System.Drawing.Point(15, 17);
            this.lblPattern.Name = "lblPattern";
            this.lblPattern.Size = new System.Drawing.Size(68, 13);
            this.lblPattern.TabIndex = 14;
            this.lblPattern.Text = "Test Pattern:";
            // 
            // cboPattern
            // 
            this.cboPattern.FormattingEnabled = true;
            this.cboPattern.Items.AddRange(new object[] {
            "Checkerboard",
            "Inverse Sine",
            "Color Wheel",
            "Panorama A",
            "Panorama B",
            "Newton Fractal"});
            this.cboPattern.Location = new System.Drawing.Point(5, 33);
            this.cboPattern.Name = "cboPattern";
            this.cboPattern.Size = new System.Drawing.Size(238, 21);
            this.cboPattern.TabIndex = 13;
            // 
            // chkAA
            // 
            this.chkAA.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkAA.Checked = true;
            this.chkAA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAA.Location = new System.Drawing.Point(16, 102);
            this.chkAA.Name = "chkAA";
            this.chkAA.Size = new System.Drawing.Size(215, 24);
            this.chkAA.TabIndex = 15;
            this.chkAA.Text = "Anti-Ailising:";
            this.chkAA.UseVisualStyleBackColor = true;
            // 
            // txtRad
            // 
            this.txtRad.Location = new System.Drawing.Point(92, 195);
            this.txtRad.Name = "txtRad";
            this.txtRad.Size = new System.Drawing.Size(151, 20);
            this.txtRad.TabIndex = 16;
            this.txtRad.Text = "1.5";
            // 
            // lblRad
            // 
            this.lblRad.AutoSize = true;
            this.lblRad.Location = new System.Drawing.Point(15, 198);
            this.lblRad.Name = "lblRad";
            this.lblRad.Size = new System.Drawing.Size(55, 13);
            this.lblRad.TabIndex = 17;
            this.lblRad.Text = "Radius =  ";
            // 
            // chkJit
            // 
            this.chkJit.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkJit.Checked = true;
            this.chkJit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkJit.Location = new System.Drawing.Point(16, 132);
            this.chkJit.Name = "chkJit";
            this.chkJit.Size = new System.Drawing.Size(215, 24);
            this.chkJit.TabIndex = 18;
            this.chkJit.Text = "Jitter Samples:";
            this.chkJit.UseVisualStyleBackColor = true;
            // 
            // lblWin
            // 
            this.lblWin.AutoSize = true;
            this.lblWin.Location = new System.Drawing.Point(15, 59);
            this.lblWin.Name = "lblWin";
            this.lblWin.Size = new System.Drawing.Size(102, 13);
            this.lblWin.TabIndex = 20;
            this.lblWin.Text = "Weighting Function:";
            // 
            // cboWin
            // 
            this.cboWin.FormattingEnabled = true;
            this.cboWin.Items.AddRange(new object[] {
            "Box",
            "Tent",
            "Cosine",
            "Gausian",
            "Sinc",
            "Lanczos"});
            this.cboWin.Location = new System.Drawing.Point(5, 75);
            this.cboWin.Name = "cboWin";
            this.cboWin.Size = new System.Drawing.Size(238, 21);
            this.cboWin.TabIndex = 19;
            // 
            // RenderingWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblWin);
            this.Controls.Add(this.cboWin);
            this.Controls.Add(this.chkJit);
            this.Controls.Add(this.lblRad);
            this.Controls.Add(this.txtRad);
            this.Controls.Add(this.chkAA);
            this.Controls.Add(this.lblPattern);
            this.Controls.Add(this.cboPattern);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLong);
            this.Controls.Add(this.barProgress);
            this.Controls.Add(this.txtSamp);
            this.Controls.Add(this.lblSamp);
            this.Controls.Add(this.pnlBorder);
            this.Name = "RenderingWindow";
            this.Size = new System.Drawing.Size(770, 550);
            this.pnlBorder.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlBorder;
        private System.Windows.Forms.Panel pnlCanvas;
        private System.Windows.Forms.Label lblSamp;
        private System.Windows.Forms.TextBox txtSamp;
        private System.Windows.Forms.ProgressBar barProgress;
        private System.Windows.Forms.Button btnLong;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblPattern;
        private System.Windows.Forms.ComboBox cboPattern;
        private System.Windows.Forms.CheckBox chkAA;
        private System.Windows.Forms.TextBox txtRad;
        private System.Windows.Forms.Label lblRad;
        private System.Windows.Forms.CheckBox chkJit;
        private System.Windows.Forms.Label lblWin;
        private System.Windows.Forms.ComboBox cboWin;
    }
}
