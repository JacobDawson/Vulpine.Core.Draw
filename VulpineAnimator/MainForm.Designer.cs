namespace VulpineAnimator
{
    partial class MainForm
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
            this.lblWeight = new System.Windows.Forms.Label();
            this.cboWeight = new System.Windows.Forms.ComboBox();
            this.chkAA = new System.Windows.Forms.CheckBox();
            this.lblSamp = new System.Windows.Forms.Label();
            this.txtSamp = new System.Windows.Forms.TextBox();
            this.txtRadius = new System.Windows.Forms.TextBox();
            this.lblRadius = new System.Windows.Forms.Label();
            this.txtStart = new System.Windows.Forms.TextBox();
            this.lblStart = new System.Windows.Forms.Label();
            this.txtFinish = new System.Windows.Forms.TextBox();
            this.lblFinish = new System.Windows.Forms.Label();
            this.gboFrames = new System.Windows.Forms.GroupBox();
            this.gboRender = new System.Windows.Forms.GroupBox();
            this.chkSuper = new System.Windows.Forms.CheckBox();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.barTotal = new System.Windows.Forms.ProgressBar();
            this.barFrame = new System.Windows.Forms.ProgressBar();
            this.lblFrame = new System.Windows.Forms.Label();
            this.pnlBorder = new System.Windows.Forms.Panel();
            this.pnlCanvas = new System.Windows.Forms.Panel();
            this.gboOutput = new System.Windows.Forms.GroupBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.gboSize = new System.Windows.Forms.GroupBox();
            this.lblWidth = new System.Windows.Forms.Label();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.lblHight = new System.Windows.Forms.Label();
            this.txtWidth = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTotalPer = new System.Windows.Forms.Label();
            this.lblFramePer = new System.Windows.Forms.Label();
            this.gboFrames.SuspendLayout();
            this.gboRender.SuspendLayout();
            this.pnlBorder.SuspendLayout();
            this.gboOutput.SuspendLayout();
            this.gboSize.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblWeight
            // 
            this.lblWeight.AutoSize = true;
            this.lblWeight.Location = new System.Drawing.Point(13, 92);
            this.lblWeight.Name = "lblWeight";
            this.lblWeight.Size = new System.Drawing.Size(102, 13);
            this.lblWeight.TabIndex = 0;
            this.lblWeight.Text = "Weighting Funciton:";
            // 
            // cboWeight
            // 
            this.cboWeight.FormattingEnabled = true;
            this.cboWeight.Items.AddRange(new object[] {
            "Box",
            "Tent",
            "Cosine",
            "Gausian",
            "Sinc",
            "Lanczos"});
            this.cboWeight.Location = new System.Drawing.Point(16, 117);
            this.cboWeight.Name = "cboWeight";
            this.cboWeight.Size = new System.Drawing.Size(164, 21);
            this.cboWeight.TabIndex = 1;
            // 
            // chkAA
            // 
            this.chkAA.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkAA.Location = new System.Drawing.Point(16, 49);
            this.chkAA.Name = "chkAA";
            this.chkAA.Size = new System.Drawing.Size(164, 24);
            this.chkAA.TabIndex = 2;
            this.chkAA.Text = "Anti-Ailising:";
            this.chkAA.UseVisualStyleBackColor = true;
            // 
            // lblSamp
            // 
            this.lblSamp.AutoSize = true;
            this.lblSamp.Location = new System.Drawing.Point(13, 150);
            this.lblSamp.Name = "lblSamp";
            this.lblSamp.Size = new System.Drawing.Size(56, 13);
            this.lblSamp.TabIndex = 3;
            this.lblSamp.Text = "Samples =";
            // 
            // txtSamp
            // 
            this.txtSamp.Location = new System.Drawing.Point(85, 147);
            this.txtSamp.Name = "txtSamp";
            this.txtSamp.Size = new System.Drawing.Size(95, 20);
            this.txtSamp.TabIndex = 4;
            this.txtSamp.Text = "16";
            // 
            // txtRadius
            // 
            this.txtRadius.Location = new System.Drawing.Point(85, 173);
            this.txtRadius.Name = "txtRadius";
            this.txtRadius.Size = new System.Drawing.Size(95, 20);
            this.txtRadius.TabIndex = 6;
            this.txtRadius.Text = "1.0";
            // 
            // lblRadius
            // 
            this.lblRadius.AutoSize = true;
            this.lblRadius.Location = new System.Drawing.Point(13, 176);
            this.lblRadius.Name = "lblRadius";
            this.lblRadius.Size = new System.Drawing.Size(49, 13);
            this.lblRadius.TabIndex = 5;
            this.lblRadius.Text = "Radius =";
            // 
            // txtStart
            // 
            this.txtStart.Location = new System.Drawing.Point(85, 25);
            this.txtStart.Name = "txtStart";
            this.txtStart.Size = new System.Drawing.Size(95, 20);
            this.txtStart.TabIndex = 8;
            this.txtStart.Text = "1";
            // 
            // lblStart
            // 
            this.lblStart.AutoSize = true;
            this.lblStart.Location = new System.Drawing.Point(13, 28);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(38, 13);
            this.lblStart.TabIndex = 7;
            this.lblStart.Text = "Start =";
            // 
            // txtFinish
            // 
            this.txtFinish.Location = new System.Drawing.Point(85, 51);
            this.txtFinish.Name = "txtFinish";
            this.txtFinish.Size = new System.Drawing.Size(95, 20);
            this.txtFinish.TabIndex = 10;
            this.txtFinish.Text = "1800";
            // 
            // lblFinish
            // 
            this.lblFinish.AutoSize = true;
            this.lblFinish.Location = new System.Drawing.Point(13, 54);
            this.lblFinish.Name = "lblFinish";
            this.lblFinish.Size = new System.Drawing.Size(43, 13);
            this.lblFinish.TabIndex = 9;
            this.lblFinish.Text = "Finish =";
            // 
            // gboFrames
            // 
            this.gboFrames.Controls.Add(this.lblStart);
            this.gboFrames.Controls.Add(this.txtFinish);
            this.gboFrames.Controls.Add(this.txtStart);
            this.gboFrames.Controls.Add(this.lblFinish);
            this.gboFrames.Location = new System.Drawing.Point(15, 194);
            this.gboFrames.Name = "gboFrames";
            this.gboFrames.Size = new System.Drawing.Size(198, 84);
            this.gboFrames.TabIndex = 11;
            this.gboFrames.TabStop = false;
            this.gboFrames.Text = "Frames:";
            // 
            // gboRender
            // 
            this.gboRender.Controls.Add(this.chkSuper);
            this.gboRender.Controls.Add(this.lblWeight);
            this.gboRender.Controls.Add(this.cboWeight);
            this.gboRender.Controls.Add(this.txtRadius);
            this.gboRender.Controls.Add(this.chkAA);
            this.gboRender.Controls.Add(this.lblRadius);
            this.gboRender.Controls.Add(this.lblSamp);
            this.gboRender.Controls.Add(this.txtSamp);
            this.gboRender.Location = new System.Drawing.Point(221, 12);
            this.gboRender.Name = "gboRender";
            this.gboRender.Size = new System.Drawing.Size(198, 217);
            this.gboRender.TabIndex = 12;
            this.gboRender.TabStop = false;
            this.gboRender.Text = "Render Setings:";
            // 
            // chkSuper
            // 
            this.chkSuper.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSuper.Checked = true;
            this.chkSuper.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSuper.Location = new System.Drawing.Point(16, 19);
            this.chkSuper.Name = "chkSuper";
            this.chkSuper.Size = new System.Drawing.Size(164, 24);
            this.chkSuper.TabIndex = 7;
            this.chkSuper.Text = "2x Super Scaling:";
            this.chkSuper.UseVisualStyleBackColor = true;
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(221, 235);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(198, 39);
            this.btnStartStop.TabIndex = 13;
            this.btnStartStop.Text = "START !";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTotal.Location = new System.Drawing.Point(12, 284);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(62, 23);
            this.lblTotal.TabIndex = 14;
            this.lblTotal.Text = "Total:";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // barTotal
            // 
            this.barTotal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.barTotal.Location = new System.Drawing.Point(80, 284);
            this.barTotal.Name = "barTotal";
            this.barTotal.Size = new System.Drawing.Size(547, 23);
            this.barTotal.TabIndex = 15;
            // 
            // barFrame
            // 
            this.barFrame.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.barFrame.Location = new System.Drawing.Point(80, 313);
            this.barFrame.Name = "barFrame";
            this.barFrame.Size = new System.Drawing.Size(547, 23);
            this.barFrame.TabIndex = 17;
            // 
            // lblFrame
            // 
            this.lblFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFrame.Location = new System.Drawing.Point(12, 313);
            this.lblFrame.Name = "lblFrame";
            this.lblFrame.Size = new System.Drawing.Size(62, 23);
            this.lblFrame.TabIndex = 16;
            this.lblFrame.Text = "Frame:";
            this.lblFrame.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBorder
            // 
            this.pnlBorder.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlBorder.Controls.Add(this.pnlCanvas);
            this.pnlBorder.Location = new System.Drawing.Point(436, 12);
            this.pnlBorder.Name = "pnlBorder";
            this.pnlBorder.Size = new System.Drawing.Size(256, 256);
            this.pnlBorder.TabIndex = 18;
            // 
            // pnlCanvas
            // 
            this.pnlCanvas.BackColor = System.Drawing.Color.White;
            this.pnlCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCanvas.Location = new System.Drawing.Point(0, 0);
            this.pnlCanvas.Name = "pnlCanvas";
            this.pnlCanvas.Size = new System.Drawing.Size(252, 252);
            this.pnlCanvas.TabIndex = 0;
            // 
            // gboOutput
            // 
            this.gboOutput.Controls.Add(this.btnBrowse);
            this.gboOutput.Controls.Add(this.txtFolder);
            this.gboOutput.Location = new System.Drawing.Point(15, 12);
            this.gboOutput.Name = "gboOutput";
            this.gboOutput.Size = new System.Drawing.Size(200, 87);
            this.gboOutput.TabIndex = 19;
            this.gboOutput.TabStop = false;
            this.gboOutput.Text = "Output Folder";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(18, 48);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(166, 23);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            // 
            // txtFolder
            // 
            this.txtFolder.Location = new System.Drawing.Point(18, 21);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(166, 20);
            this.txtFolder.TabIndex = 0;
            // 
            // gboSize
            // 
            this.gboSize.Controls.Add(this.lblWidth);
            this.gboSize.Controls.Add(this.txtHeight);
            this.gboSize.Controls.Add(this.lblHight);
            this.gboSize.Controls.Add(this.txtWidth);
            this.gboSize.Location = new System.Drawing.Point(15, 105);
            this.gboSize.Name = "gboSize";
            this.gboSize.Size = new System.Drawing.Size(200, 83);
            this.gboSize.TabIndex = 20;
            this.gboSize.TabStop = false;
            this.gboSize.Text = "Resolution";
            // 
            // lblWidth
            // 
            this.lblWidth.AutoSize = true;
            this.lblWidth.Location = new System.Drawing.Point(15, 28);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(44, 13);
            this.lblWidth.TabIndex = 11;
            this.lblWidth.Text = "Width =";
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(87, 51);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(95, 20);
            this.txtHeight.TabIndex = 14;
            this.txtHeight.Text = "720";
            // 
            // lblHight
            // 
            this.lblHight.AutoSize = true;
            this.lblHight.Location = new System.Drawing.Point(15, 54);
            this.lblHight.Name = "lblHight";
            this.lblHight.Size = new System.Drawing.Size(47, 13);
            this.lblHight.TabIndex = 13;
            this.lblHight.Text = "Height =";
            // 
            // txtWidth
            // 
            this.txtWidth.Location = new System.Drawing.Point(87, 25);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(95, 20);
            this.txtWidth.TabIndex = 12;
            this.txtWidth.Text = "960";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblInfo});
            this.statusStrip1.Location = new System.Drawing.Point(0, 350);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(704, 22);
            this.statusStrip1.TabIndex = 21;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblInfo
            // 
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(91, 17);
            this.lblInfo.Text = "  Info Goes Here";
            // 
            // lblTotalPer
            // 
            this.lblTotalPer.Location = new System.Drawing.Point(633, 284);
            this.lblTotalPer.Name = "lblTotalPer";
            this.lblTotalPer.Size = new System.Drawing.Size(59, 23);
            this.lblTotalPer.TabIndex = 22;
            this.lblTotalPer.Text = "00.00 %";
            this.lblTotalPer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFramePer
            // 
            this.lblFramePer.Location = new System.Drawing.Point(636, 313);
            this.lblFramePer.Name = "lblFramePer";
            this.lblFramePer.Size = new System.Drawing.Size(56, 23);
            this.lblFramePer.TabIndex = 23;
            this.lblFramePer.Text = "00.00 %";
            this.lblFramePer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 372);
            this.Controls.Add(this.lblFramePer);
            this.Controls.Add(this.lblTotalPer);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.gboSize);
            this.Controls.Add(this.gboOutput);
            this.Controls.Add(this.pnlBorder);
            this.Controls.Add(this.barFrame);
            this.Controls.Add(this.lblFrame);
            this.Controls.Add(this.barTotal);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.gboRender);
            this.Controls.Add(this.gboFrames);
            this.Name = "MainForm";
            this.Text = "Vulpine Animator";
            this.gboFrames.ResumeLayout(false);
            this.gboFrames.PerformLayout();
            this.gboRender.ResumeLayout(false);
            this.gboRender.PerformLayout();
            this.pnlBorder.ResumeLayout(false);
            this.gboOutput.ResumeLayout(false);
            this.gboOutput.PerformLayout();
            this.gboSize.ResumeLayout(false);
            this.gboSize.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblWeight;
        private System.Windows.Forms.ComboBox cboWeight;
        private System.Windows.Forms.CheckBox chkAA;
        private System.Windows.Forms.Label lblSamp;
        private System.Windows.Forms.TextBox txtSamp;
        private System.Windows.Forms.TextBox txtRadius;
        private System.Windows.Forms.Label lblRadius;
        private System.Windows.Forms.TextBox txtStart;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.TextBox txtFinish;
        private System.Windows.Forms.Label lblFinish;
        private System.Windows.Forms.GroupBox gboFrames;
        private System.Windows.Forms.GroupBox gboRender;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.ProgressBar barTotal;
        private System.Windows.Forms.ProgressBar barFrame;
        private System.Windows.Forms.Label lblFrame;
        private System.Windows.Forms.Panel pnlBorder;
        private System.Windows.Forms.Panel pnlCanvas;
        private System.Windows.Forms.GroupBox gboOutput;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.GroupBox gboSize;
        private System.Windows.Forms.Label lblWidth;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.Label lblHight;
        private System.Windows.Forms.TextBox txtWidth;
        private System.Windows.Forms.CheckBox chkSuper;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblInfo;
        private System.Windows.Forms.Label lblTotalPer;
        private System.Windows.Forms.Label lblFramePer;
    }
}