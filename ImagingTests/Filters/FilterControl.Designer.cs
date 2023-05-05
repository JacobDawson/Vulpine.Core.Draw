namespace ImagingTests.Filters
{
    partial class FilterControl
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
            this.lblTime = new System.Windows.Forms.Label();
            this.listFitlers = new System.Windows.Forms.ListBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnUndo = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlBorder1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(3, 29);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(272, 23);
            this.btnLoad.TabIndex = 31;
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
            this.txtFileName.TabIndex = 30;
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
            this.pnlBorder1.TabIndex = 32;
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
            this.barProgress.Location = new System.Drawing.Point(3, 488);
            this.barProgress.Maximum = 500;
            this.barProgress.Name = "barProgress";
            this.barProgress.Size = new System.Drawing.Size(804, 23);
            this.barProgress.TabIndex = 27;
            // 
            // lblTime
            // 
            this.lblTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(3, 469);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(54, 13);
            this.lblTime.TabIndex = 48;
            this.lblTime.Text = "Time: ???";
            // 
            // listFitlers
            // 
            this.listFitlers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.listFitlers.FormattingEnabled = true;
            this.listFitlers.IntegralHeight = false;
            this.listFitlers.Items.AddRange(new object[] {
            "Threshold 0.25",
            "Threshold 0.50",
            "Threshold 0.75",
            "Grayscale",
            "RedChanel",
            "GreenChanel",
            "BlueChanel",
            "Hue-Brightness",
            "Hue-Only",
            "Sharpen",
            "Emboss",
            "Sobel Vertival",
            "Sobel Horizontal",
            "Laplass",
            "Outline",
            "Box Blur 3",
            "Box Blur 5",
            "Gausian Blur 3",
            "Gausian Blur 5",
            "Unsharp Mask",
            "Mean3",
            "Mean5",
            "Mean7",
            "Var3",
            "Var5",
            "Var7",
            "Skew3",
            "Skew5",
            "Skew7",
            "Kurt3",
            "Kurt5",
            "Kurt7",
            "True Sobel",
            "Line Vertical",
            "Line Horizontal",
            "Line Diagonal",
            "Line Antidiagonal",
            "Sobel Horizontal 2",
            "Sobel Vertical 2",
            "Sobel Magnitued",
            "Sobel Gradient",
            "Max Value",
            "Swap Sat-Value",
            "Mid Brightness",
            "Hue-Value",
            "Cyan-Red",
            "Yellow-Blue",
            "Mangenta-Green"});
            this.listFitlers.Location = new System.Drawing.Point(6, 58);
            this.listFitlers.Name = "listFitlers";
            this.listFitlers.Size = new System.Drawing.Size(269, 281);
            this.listFitlers.TabIndex = 49;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnApply.Location = new System.Drawing.Point(6, 345);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(269, 23);
            this.btnApply.TabIndex = 50;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnUndo
            // 
            this.btnUndo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUndo.Location = new System.Drawing.Point(6, 374);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(269, 23);
            this.btnUndo.TabIndex = 51;
            this.btnUndo.Text = "Undo";
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReset.Location = new System.Drawing.Point(6, 403);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(269, 23);
            this.btnReset.TabIndex = 52;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(6, 432);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(269, 23);
            this.btnSave.TabIndex = 53;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // FilterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnUndo);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.listFitlers);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.barProgress);
            this.Controls.Add(this.pnlBorder1);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.txtFileName);
            this.Name = "FilterControl";
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
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.ListBox listFitlers;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnSave;
    }
}
