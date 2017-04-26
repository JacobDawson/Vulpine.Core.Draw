namespace ImagingTests.Resampeling
{
    partial class Resampeling
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
            this.pnlBorder1 = new System.Windows.Forms.Panel();
            this.pnlCanvas = new System.Windows.Forms.Panel();
            this.pnlBorder2 = new System.Windows.Forms.Panel();
            this.pnlOriginal = new System.Windows.Forms.Panel();
            this.lblOriginal = new System.Windows.Forms.Label();
            this.cboImage = new System.Windows.Forms.ComboBox();
            this.lblInterp = new System.Windows.Forms.Label();
            this.cboInterp = new System.Windows.Forms.ComboBox();
            this.btnLong = new System.Windows.Forms.Button();
            this.barProgress = new System.Windows.Forms.ProgressBar();
            this.lblTime = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.cboDouble = new System.Windows.Forms.CheckBox();
            this.cboTileable = new System.Windows.Forms.CheckBox();
            this.pnlBorder1.SuspendLayout();
            this.pnlBorder2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBorder1
            // 
            this.pnlBorder1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlBorder1.Controls.Add(this.pnlCanvas);
            this.pnlBorder1.Location = new System.Drawing.Point(255, 3);
            this.pnlBorder1.Name = "pnlBorder1";
            this.pnlBorder1.Size = new System.Drawing.Size(512, 512);
            this.pnlBorder1.TabIndex = 0;
            // 
            // pnlCanvas
            // 
            this.pnlCanvas.BackColor = System.Drawing.Color.White;
            this.pnlCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCanvas.Location = new System.Drawing.Point(0, 0);
            this.pnlCanvas.Name = "pnlCanvas";
            this.pnlCanvas.Size = new System.Drawing.Size(508, 508);
            this.pnlCanvas.TabIndex = 0;
            // 
            // pnlBorder2
            // 
            this.pnlBorder2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlBorder2.Controls.Add(this.pnlOriginal);
            this.pnlBorder2.Location = new System.Drawing.Point(122, 3);
            this.pnlBorder2.Name = "pnlBorder2";
            this.pnlBorder2.Size = new System.Drawing.Size(127, 127);
            this.pnlBorder2.TabIndex = 1;
            // 
            // pnlOriginal
            // 
            this.pnlOriginal.BackColor = System.Drawing.Color.White;
            this.pnlOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOriginal.Location = new System.Drawing.Point(0, 0);
            this.pnlOriginal.Name = "pnlOriginal";
            this.pnlOriginal.Size = new System.Drawing.Size(123, 123);
            this.pnlOriginal.TabIndex = 0;
            // 
            // lblOriginal
            // 
            this.lblOriginal.AutoSize = true;
            this.lblOriginal.Location = new System.Drawing.Point(55, 56);
            this.lblOriginal.Name = "lblOriginal";
            this.lblOriginal.Size = new System.Drawing.Size(61, 13);
            this.lblOriginal.TabIndex = 2;
            this.lblOriginal.Text = "ORIGINAL:";
            // 
            // cboImage
            // 
            this.cboImage.FormattingEnabled = true;
            this.cboImage.Items.AddRange(new object[] {
            "Animetastic",
            "Checkerboard",
            "Eevee2",
            "FoxCubs",
            "Handwriting",
            "Leaf",
            "Picknick",
            "Rainbow",
            "RPG",
            "MarioWorld",
            "Text",
            "Winter"});
            this.cboImage.Location = new System.Drawing.Point(7, 136);
            this.cboImage.Name = "cboImage";
            this.cboImage.Size = new System.Drawing.Size(240, 21);
            this.cboImage.TabIndex = 14;
            this.cboImage.SelectionChangeCommitted += new System.EventHandler(this.cboImage_SelectionChangeCommitted);
            // 
            // lblInterp
            // 
            this.lblInterp.AutoSize = true;
            this.lblInterp.Location = new System.Drawing.Point(17, 174);
            this.lblInterp.Name = "lblInterp";
            this.lblInterp.Size = new System.Drawing.Size(107, 13);
            this.lblInterp.TabIndex = 21;
            this.lblInterp.Text = "Interpolation Method:";
            // 
            // cboInterp
            // 
            this.cboInterp.FormattingEnabled = true;
            this.cboInterp.Items.AddRange(new object[] {
            "Nearest-Neighbor",
            "Bilenear",
            "Bicubic",
            "Sinc3"});
            this.cboInterp.Location = new System.Drawing.Point(7, 190);
            this.cboInterp.Name = "cboInterp";
            this.cboInterp.Size = new System.Drawing.Size(240, 21);
            this.cboInterp.TabIndex = 20;
            // 
            // btnLong
            // 
            this.btnLong.Location = new System.Drawing.Point(7, 217);
            this.btnLong.Name = "btnLong";
            this.btnLong.Size = new System.Drawing.Size(240, 23);
            this.btnLong.TabIndex = 22;
            this.btnLong.Text = "Long Render";
            this.btnLong.UseVisualStyleBackColor = true;
            this.btnLong.Click += new System.EventHandler(this.btnLong_Click);
            // 
            // barProgress
            // 
            this.barProgress.Location = new System.Drawing.Point(7, 519);
            this.barProgress.Maximum = 500;
            this.barProgress.Name = "barProgress";
            this.barProgress.Size = new System.Drawing.Size(758, 23);
            this.barProgress.TabIndex = 23;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(17, 489);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(54, 13);
            this.lblTime.TabIndex = 24;
            this.lblTime.Text = "Time: ???";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(7, 246);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(240, 23);
            this.btnSave.TabIndex = 25;
            this.btnSave.Text = "Save Output";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cboDouble
            // 
            this.cboDouble.AutoSize = true;
            this.cboDouble.Location = new System.Drawing.Point(20, 293);
            this.cboDouble.Name = "cboDouble";
            this.cboDouble.Size = new System.Drawing.Size(92, 17);
            this.cboDouble.TabIndex = 26;
            this.cboDouble.Text = "Double Image";
            this.cboDouble.UseVisualStyleBackColor = true;
            // 
            // cboTileable
            // 
            this.cboTileable.AutoSize = true;
            this.cboTileable.Location = new System.Drawing.Point(20, 316);
            this.cboTileable.Name = "cboTileable";
            this.cboTileable.Size = new System.Drawing.Size(63, 17);
            this.cboTileable.TabIndex = 27;
            this.cboTileable.Text = "Tileable";
            this.cboTileable.UseVisualStyleBackColor = true;
            // 
            // Resampeling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cboTileable);
            this.Controls.Add(this.cboDouble);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.barProgress);
            this.Controls.Add(this.btnLong);
            this.Controls.Add(this.lblInterp);
            this.Controls.Add(this.cboInterp);
            this.Controls.Add(this.cboImage);
            this.Controls.Add(this.lblOriginal);
            this.Controls.Add(this.pnlBorder2);
            this.Controls.Add(this.pnlBorder1);
            this.Name = "Resampeling";
            this.Size = new System.Drawing.Size(770, 550);
            this.pnlBorder1.ResumeLayout(false);
            this.pnlBorder2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlBorder1;
        private System.Windows.Forms.Panel pnlCanvas;
        private System.Windows.Forms.Panel pnlBorder2;
        private System.Windows.Forms.Panel pnlOriginal;
        private System.Windows.Forms.Label lblOriginal;
        private System.Windows.Forms.ComboBox cboImage;
        private System.Windows.Forms.Label lblInterp;
        private System.Windows.Forms.ComboBox cboInterp;
        private System.Windows.Forms.Button btnLong;
        private System.Windows.Forms.ProgressBar barProgress;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox cboDouble;
        private System.Windows.Forms.CheckBox cboTileable;
    }
}
