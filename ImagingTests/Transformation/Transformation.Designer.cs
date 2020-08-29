namespace ImagingTests.Transformation
{
    partial class Transformation
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
            this.btnLoad = new System.Windows.Forms.Button();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.pnlBorder1 = new System.Windows.Forms.Panel();
            this.pnlCanvas = new System.Windows.Forms.Panel();
            this.barProgress = new System.Windows.Forms.ProgressBar();
            this.lblInterp = new System.Windows.Forms.Label();
            this.cboInterp = new System.Windows.Forms.ComboBox();
            this.lblBackground = new System.Windows.Forms.Label();
            this.cboBackground = new System.Windows.Forms.ComboBox();
            this.barScale = new System.Windows.Forms.HScrollBar();
            this.lblScale = new System.Windows.Forms.Label();
            this.lblRotation = new System.Windows.Forms.Label();
            this.barRoation = new System.Windows.Forms.HScrollBar();
            this.txtX = new System.Windows.Forms.TextBox();
            this.lblX = new System.Windows.Forms.Label();
            this.lblY = new System.Windows.Forms.Label();
            this.txtY = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLong = new System.Windows.Forms.Button();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblScaling = new System.Windows.Forms.Label();
            this.cboScaling = new System.Windows.Forms.ComboBox();
            this.pnlBorder1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(3, 29);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(272, 23);
            this.btnLoad.TabIndex = 29;
            this.btnLoad.Text = "Load File";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(3, 3);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.ReadOnly = true;
            this.txtFileName.Size = new System.Drawing.Size(272, 20);
            this.txtFileName.TabIndex = 28;
            // 
            // pnlBorder1
            // 
            this.pnlBorder1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBorder1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlBorder1.Controls.Add(this.pnlCanvas);
            this.pnlBorder1.Location = new System.Drawing.Point(281, 3);
            this.pnlBorder1.Name = "pnlBorder1";
            this.pnlBorder1.Size = new System.Drawing.Size(528, 481);
            this.pnlBorder1.TabIndex = 30;
            // 
            // pnlCanvas
            // 
            this.pnlCanvas.BackColor = System.Drawing.Color.White;
            this.pnlCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCanvas.Location = new System.Drawing.Point(0, 0);
            this.pnlCanvas.Name = "pnlCanvas";
            this.pnlCanvas.Size = new System.Drawing.Size(524, 477);
            this.pnlCanvas.TabIndex = 0;
            // 
            // barProgress
            // 
            this.barProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.barProgress.Location = new System.Drawing.Point(3, 490);
            this.barProgress.Maximum = 500;
            this.barProgress.Name = "barProgress";
            this.barProgress.Size = new System.Drawing.Size(804, 23);
            this.barProgress.TabIndex = 26;
            // 
            // lblInterp
            // 
            this.lblInterp.AutoSize = true;
            this.lblInterp.Location = new System.Drawing.Point(13, 75);
            this.lblInterp.Name = "lblInterp";
            this.lblInterp.Size = new System.Drawing.Size(107, 13);
            this.lblInterp.TabIndex = 32;
            this.lblInterp.Text = "Interpolation Method:";
            // 
            // cboInterp
            // 
            this.cboInterp.FormattingEnabled = true;
            this.cboInterp.Items.AddRange(new object[] {
            "Nearest",
            "Bilenear",
            "Bicubic",
            "Catrom",
            "Mitchel",
            "Sinc3"});
            this.cboInterp.Location = new System.Drawing.Point(3, 91);
            this.cboInterp.Name = "cboInterp";
            this.cboInterp.Size = new System.Drawing.Size(272, 21);
            this.cboInterp.TabIndex = 31;
            // 
            // lblBackground
            // 
            this.lblBackground.AutoSize = true;
            this.lblBackground.Location = new System.Drawing.Point(13, 115);
            this.lblBackground.Name = "lblBackground";
            this.lblBackground.Size = new System.Drawing.Size(68, 13);
            this.lblBackground.TabIndex = 34;
            this.lblBackground.Text = "Background:";
            // 
            // cboBackground
            // 
            this.cboBackground.FormattingEnabled = true;
            this.cboBackground.Items.AddRange(new object[] {
            "None",
            "Transparent",
            "Black",
            "Magenta",
            "White"});
            this.cboBackground.Location = new System.Drawing.Point(3, 131);
            this.cboBackground.Name = "cboBackground";
            this.cboBackground.Size = new System.Drawing.Size(272, 21);
            this.cboBackground.TabIndex = 33;
            // 
            // barScale
            // 
            this.barScale.Location = new System.Drawing.Point(3, 224);
            this.barScale.Minimum = -100;
            this.barScale.Name = "barScale";
            this.barScale.Size = new System.Drawing.Size(272, 17);
            this.barScale.TabIndex = 37;
            this.barScale.Scroll += new System.Windows.Forms.ScrollEventHandler(this.barScale_Scroll);
            // 
            // lblScale
            // 
            this.lblScale.AutoSize = true;
            this.lblScale.Location = new System.Drawing.Point(13, 208);
            this.lblScale.Margin = new System.Windows.Forms.Padding(3);
            this.lblScale.Name = "lblScale";
            this.lblScale.Size = new System.Drawing.Size(61, 13);
            this.lblScale.TabIndex = 38;
            this.lblScale.Text = "Scale: 1.00";
            // 
            // lblRotation
            // 
            this.lblRotation.AutoSize = true;
            this.lblRotation.Location = new System.Drawing.Point(13, 251);
            this.lblRotation.Margin = new System.Windows.Forms.Padding(3);
            this.lblRotation.Name = "lblRotation";
            this.lblRotation.Size = new System.Drawing.Size(89, 13);
            this.lblRotation.TabIndex = 40;
            this.lblRotation.Text = "Rotation: 0.0 deg";
            // 
            // barRoation
            // 
            this.barRoation.Location = new System.Drawing.Point(3, 267);
            this.barRoation.Minimum = -100;
            this.barRoation.Name = "barRoation";
            this.barRoation.Size = new System.Drawing.Size(272, 17);
            this.barRoation.TabIndex = 39;
            this.barRoation.Scroll += new System.Windows.Forms.ScrollEventHandler(this.barRoation_Scroll);
            // 
            // txtX
            // 
            this.txtX.Location = new System.Drawing.Point(37, 302);
            this.txtX.Name = "txtX";
            this.txtX.Size = new System.Drawing.Size(61, 20);
            this.txtX.TabIndex = 41;
            this.txtX.Text = "0.0";
            this.txtX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblX
            // 
            this.lblX.AutoSize = true;
            this.lblX.Location = new System.Drawing.Point(8, 305);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(23, 13);
            this.lblX.TabIndex = 42;
            this.lblX.Text = "X =";
            // 
            // lblY
            // 
            this.lblY.AutoSize = true;
            this.lblY.Location = new System.Drawing.Point(116, 305);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(23, 13);
            this.lblY.TabIndex = 44;
            this.lblY.Text = "Y =";
            // 
            // txtY
            // 
            this.txtY.Location = new System.Drawing.Point(145, 302);
            this.txtY.Name = "txtY";
            this.txtY.Size = new System.Drawing.Size(61, 20);
            this.txtY.TabIndex = 43;
            this.txtY.Text = "0.0";
            this.txtY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(3, 379);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(272, 23);
            this.btnSave.TabIndex = 46;
            this.btnSave.Text = "Save Output";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLong
            // 
            this.btnLong.Location = new System.Drawing.Point(3, 350);
            this.btnLong.Name = "btnLong";
            this.btnLong.Size = new System.Drawing.Size(272, 23);
            this.btnLong.TabIndex = 45;
            this.btnLong.Text = "Long Render";
            this.btnLong.UseVisualStyleBackColor = true;
            this.btnLong.Click += new System.EventHandler(this.btnLong_Click);
            // 
            // lblTime
            // 
            this.lblTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(8, 469);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(54, 13);
            this.lblTime.TabIndex = 47;
            this.lblTime.Text = "Time: ???";
            // 
            // lblScaling
            // 
            this.lblScaling.AutoSize = true;
            this.lblScaling.Location = new System.Drawing.Point(13, 155);
            this.lblScaling.Name = "lblScaling";
            this.lblScaling.Size = new System.Drawing.Size(45, 13);
            this.lblScaling.TabIndex = 49;
            this.lblScaling.Text = "Scaling:";
            // 
            // cboScaling
            // 
            this.cboScaling.FormattingEnabled = true;
            this.cboScaling.Items.AddRange(new object[] {
            "Scale X",
            "Scale Y",
            "Streach"});
            this.cboScaling.Location = new System.Drawing.Point(3, 171);
            this.cboScaling.Name = "cboScaling";
            this.cboScaling.Size = new System.Drawing.Size(272, 21);
            this.cboScaling.TabIndex = 48;
            // 
            // Transformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblScaling);
            this.Controls.Add(this.cboScaling);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLong);
            this.Controls.Add(this.lblY);
            this.Controls.Add(this.txtY);
            this.Controls.Add(this.lblX);
            this.Controls.Add(this.txtX);
            this.Controls.Add(this.lblRotation);
            this.Controls.Add(this.barRoation);
            this.Controls.Add(this.lblScale);
            this.Controls.Add(this.barScale);
            this.Controls.Add(this.lblBackground);
            this.Controls.Add(this.cboBackground);
            this.Controls.Add(this.lblInterp);
            this.Controls.Add(this.cboInterp);
            this.Controls.Add(this.barProgress);
            this.Controls.Add(this.pnlBorder1);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.txtFileName);
            this.Name = "Transformation";
            this.Size = new System.Drawing.Size(812, 516);
            this.pnlBorder1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Panel pnlBorder1;
        private System.Windows.Forms.Panel pnlCanvas;
        private System.Windows.Forms.ProgressBar barProgress;
        private System.Windows.Forms.Label lblInterp;
        private System.Windows.Forms.ComboBox cboInterp;
        private System.Windows.Forms.Label lblBackground;
        private System.Windows.Forms.ComboBox cboBackground;
        private System.Windows.Forms.HScrollBar barScale;
        private System.Windows.Forms.Label lblScale;
        private System.Windows.Forms.Label lblRotation;
        private System.Windows.Forms.HScrollBar barRoation;
        private System.Windows.Forms.TextBox txtX;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.Label lblY;
        private System.Windows.Forms.TextBox txtY;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLong;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblScaling;
        private System.Windows.Forms.ComboBox cboScaling;
    }
}
