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
            this.cboAA = new System.Windows.Forms.ComboBox();
            this.lblAA = new System.Windows.Forms.Label();
            this.lblN = new System.Windows.Forms.Label();
            this.txtN = new System.Windows.Forms.TextBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.barProgress = new System.Windows.Forms.ProgressBar();
            this.btnLong = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblPattern = new System.Windows.Forms.Label();
            this.cboPattern = new System.Windows.Forms.ComboBox();
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
            // cboAA
            // 
            this.cboAA.FormattingEnabled = true;
            this.cboAA.Items.AddRange(new object[] {
            "None",
            "Random",
            "Jittred",
            "Poisson"});
            this.cboAA.Location = new System.Drawing.Point(3, 86);
            this.cboAA.Name = "cboAA";
            this.cboAA.Size = new System.Drawing.Size(240, 21);
            this.cboAA.TabIndex = 1;
            // 
            // lblAA
            // 
            this.lblAA.AutoSize = true;
            this.lblAA.Location = new System.Drawing.Point(13, 70);
            this.lblAA.Name = "lblAA";
            this.lblAA.Size = new System.Drawing.Size(102, 13);
            this.lblAA.TabIndex = 2;
            this.lblAA.Text = "Anti-Ailising Method:";
            // 
            // lblN
            // 
            this.lblN.AutoSize = true;
            this.lblN.Location = new System.Drawing.Point(13, 137);
            this.lblN.Name = "lblN";
            this.lblN.Size = new System.Drawing.Size(30, 13);
            this.lblN.TabIndex = 3;
            this.lblN.Text = "N =  ";
            // 
            // txtN
            // 
            this.txtN.Location = new System.Drawing.Point(49, 134);
            this.txtN.Name = "txtN";
            this.txtN.Size = new System.Drawing.Size(66, 20);
            this.txtN.TabIndex = 4;
            this.txtN.Text = "4";
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(148, 127);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 33);
            this.btnGo.TabIndex = 5;
            this.btnGo.Text = "GO!";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
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
            this.btnLong.Location = new System.Drawing.Point(5, 195);
            this.btnLong.Name = "btnLong";
            this.btnLong.Size = new System.Drawing.Size(240, 23);
            this.btnLong.TabIndex = 8;
            this.btnLong.Text = "Long Render";
            this.btnLong.UseVisualStyleBackColor = true;
            this.btnLong.Click += new System.EventHandler(this.btnLong_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(5, 224);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(240, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save Output";
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
            "Panorama B"});
            this.cboPattern.Location = new System.Drawing.Point(5, 33);
            this.cboPattern.Name = "cboPattern";
            this.cboPattern.Size = new System.Drawing.Size(240, 21);
            this.cboPattern.TabIndex = 13;
            // 
            // RenderingWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblPattern);
            this.Controls.Add(this.cboPattern);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLong);
            this.Controls.Add(this.barProgress);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.txtN);
            this.Controls.Add(this.lblN);
            this.Controls.Add(this.lblAA);
            this.Controls.Add(this.cboAA);
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
        private System.Windows.Forms.ComboBox cboAA;
        private System.Windows.Forms.Label lblAA;
        private System.Windows.Forms.Label lblN;
        private System.Windows.Forms.TextBox txtN;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.ProgressBar barProgress;
        private System.Windows.Forms.Button btnLong;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblPattern;
        private System.Windows.Forms.ComboBox cboPattern;
    }
}
