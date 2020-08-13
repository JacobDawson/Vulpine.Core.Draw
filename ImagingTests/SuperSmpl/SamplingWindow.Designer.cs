namespace ImagingTests.SuperSmpl
{
    partial class SamplingWindow
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
            this.pnlCanvas = new System.Windows.Forms.Panel();
            this.pnlBorder = new System.Windows.Forms.Panel();
            this.cboMethod = new System.Windows.Forms.ComboBox();
            this.lblMethod = new System.Windows.Forms.Label();
            this.btnStep = new System.Windows.Forms.Button();
            this.btnStep10 = new System.Windows.Forms.Button();
            this.btnFill = new System.Windows.Forms.Button();
            this.lblN = new System.Windows.Forms.Label();
            this.txtN = new System.Windows.Forms.TextBox();
            this.txtK = new System.Windows.Forms.TextBox();
            this.lblK = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.btnA = new System.Windows.Forms.Button();
            this.btnB = new System.Windows.Forms.Button();
            this.lblSample = new System.Windows.Forms.Label();
            this.txtDelta = new System.Windows.Forms.TextBox();
            this.lblDelta = new System.Windows.Forms.Label();
            this.pnlBorder.SuspendLayout();
            this.SuspendLayout();
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
            // pnlBorder
            // 
            this.pnlBorder.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlBorder.Controls.Add(this.pnlCanvas);
            this.pnlBorder.Location = new System.Drawing.Point(249, 3);
            this.pnlBorder.Name = "pnlBorder";
            this.pnlBorder.Size = new System.Drawing.Size(500, 500);
            this.pnlBorder.TabIndex = 1;
            // 
            // cboMethod
            // 
            this.cboMethod.FormattingEnabled = true;
            this.cboMethod.Items.AddRange(new object[] {
            "Random",
            "Jittred",
            "PoissonA",
            "PoissonB",
            "PoissonKD",
            "PoissonDisk*",
            "HexGrid"});
            this.cboMethod.Location = new System.Drawing.Point(13, 33);
            this.cboMethod.Name = "cboMethod";
            this.cboMethod.Size = new System.Drawing.Size(230, 21);
            this.cboMethod.TabIndex = 2;
            this.cboMethod.SelectionChangeCommitted += new System.EventHandler(this.cboMethod_SelectionChangeCommitted);
            // 
            // lblMethod
            // 
            this.lblMethod.AutoSize = true;
            this.lblMethod.Location = new System.Drawing.Point(16, 17);
            this.lblMethod.Name = "lblMethod";
            this.lblMethod.Size = new System.Drawing.Size(43, 13);
            this.lblMethod.TabIndex = 3;
            this.lblMethod.Text = "Method";
            // 
            // btnStep
            // 
            this.btnStep.Location = new System.Drawing.Point(13, 193);
            this.btnStep.Name = "btnStep";
            this.btnStep.Size = new System.Drawing.Size(230, 23);
            this.btnStep.TabIndex = 4;
            this.btnStep.Text = "Step";
            this.btnStep.UseVisualStyleBackColor = true;
            this.btnStep.Click += new System.EventHandler(this.btnStep_Click);
            // 
            // btnStep10
            // 
            this.btnStep10.Location = new System.Drawing.Point(13, 222);
            this.btnStep10.Name = "btnStep10";
            this.btnStep10.Size = new System.Drawing.Size(230, 23);
            this.btnStep10.TabIndex = 5;
            this.btnStep10.Text = "Step 10";
            this.btnStep10.UseVisualStyleBackColor = true;
            this.btnStep10.Click += new System.EventHandler(this.btnStep10_Click);
            // 
            // btnFill
            // 
            this.btnFill.Location = new System.Drawing.Point(13, 251);
            this.btnFill.Name = "btnFill";
            this.btnFill.Size = new System.Drawing.Size(230, 23);
            this.btnFill.TabIndex = 6;
            this.btnFill.Text = "Fill";
            this.btnFill.UseVisualStyleBackColor = true;
            this.btnFill.Click += new System.EventHandler(this.btnFill_Click);
            // 
            // lblN
            // 
            this.lblN.AutoSize = true;
            this.lblN.Location = new System.Drawing.Point(16, 82);
            this.lblN.Name = "lblN";
            this.lblN.Size = new System.Drawing.Size(27, 13);
            this.lblN.TabIndex = 7;
            this.lblN.Text = "N = ";
            // 
            // txtN
            // 
            this.txtN.Location = new System.Drawing.Point(67, 79);
            this.txtN.Name = "txtN";
            this.txtN.Size = new System.Drawing.Size(92, 20);
            this.txtN.TabIndex = 8;
            this.txtN.Text = "10";
            // 
            // txtK
            // 
            this.txtK.Location = new System.Drawing.Point(67, 105);
            this.txtK.Name = "txtK";
            this.txtK.Size = new System.Drawing.Size(92, 20);
            this.txtK.TabIndex = 10;
            this.txtK.Text = "6";
            // 
            // lblK
            // 
            this.lblK.AutoSize = true;
            this.lblK.Location = new System.Drawing.Point(16, 108);
            this.lblK.Name = "lblK";
            this.lblK.Size = new System.Drawing.Size(26, 13);
            this.lblK.TabIndex = 9;
            this.lblK.Text = "K = ";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(16, 470);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(54, 13);
            this.lblTime.TabIndex = 11;
            this.lblTime.Text = "Time: ???";
            // 
            // btnA
            // 
            this.btnA.Location = new System.Drawing.Point(13, 336);
            this.btnA.Name = "btnA";
            this.btnA.Size = new System.Drawing.Size(75, 23);
            this.btnA.TabIndex = 12;
            this.btnA.Text = "A Test";
            this.btnA.UseVisualStyleBackColor = true;
            this.btnA.Click += new System.EventHandler(this.btnA_Click);
            // 
            // btnB
            // 
            this.btnB.Location = new System.Drawing.Point(94, 336);
            this.btnB.Name = "btnB";
            this.btnB.Size = new System.Drawing.Size(75, 23);
            this.btnB.TabIndex = 13;
            this.btnB.Text = "B Test";
            this.btnB.UseVisualStyleBackColor = true;
            this.btnB.Click += new System.EventHandler(this.btnB_Click);
            // 
            // lblSample
            // 
            this.lblSample.AutoSize = true;
            this.lblSample.Location = new System.Drawing.Point(16, 444);
            this.lblSample.Name = "lblSample";
            this.lblSample.Size = new System.Drawing.Size(115, 13);
            this.lblSample.TabIndex = 14;
            this.lblSample.Text = "Samples Generated:  0";
            // 
            // txtDelta
            // 
            this.txtDelta.Location = new System.Drawing.Point(67, 131);
            this.txtDelta.Name = "txtDelta";
            this.txtDelta.Size = new System.Drawing.Size(92, 20);
            this.txtDelta.TabIndex = 16;
            this.txtDelta.Text = "4";
            // 
            // lblDelta
            // 
            this.lblDelta.AutoSize = true;
            this.lblDelta.Location = new System.Drawing.Point(16, 134);
            this.lblDelta.Name = "lblDelta";
            this.lblDelta.Size = new System.Drawing.Size(27, 13);
            this.lblDelta.TabIndex = 15;
            this.lblDelta.Text = "D = ";
            // 
            // SamplingWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtDelta);
            this.Controls.Add(this.lblDelta);
            this.Controls.Add(this.lblSample);
            this.Controls.Add(this.btnB);
            this.Controls.Add(this.btnA);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.txtK);
            this.Controls.Add(this.lblK);
            this.Controls.Add(this.txtN);
            this.Controls.Add(this.lblN);
            this.Controls.Add(this.btnFill);
            this.Controls.Add(this.btnStep10);
            this.Controls.Add(this.btnStep);
            this.Controls.Add(this.lblMethod);
            this.Controls.Add(this.cboMethod);
            this.Controls.Add(this.pnlBorder);
            this.Name = "SamplingWindow";
            this.Size = new System.Drawing.Size(752, 512);
            this.pnlBorder.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlCanvas;
        private System.Windows.Forms.Panel pnlBorder;
        private System.Windows.Forms.ComboBox cboMethod;
        private System.Windows.Forms.Label lblMethod;
        private System.Windows.Forms.Button btnStep;
        private System.Windows.Forms.Button btnStep10;
        private System.Windows.Forms.Button btnFill;
        private System.Windows.Forms.Label lblN;
        private System.Windows.Forms.TextBox txtN;
        private System.Windows.Forms.TextBox txtK;
        private System.Windows.Forms.Label lblK;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Button btnA;
        private System.Windows.Forms.Button btnB;
        private System.Windows.Forms.Label lblSample;
        private System.Windows.Forms.TextBox txtDelta;
        private System.Windows.Forms.Label lblDelta;
    }
}
