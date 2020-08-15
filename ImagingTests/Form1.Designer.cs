namespace ImagingTests
{
    partial class Form1
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.samplingWindow1 = new ImagingTests.SuperSmpl.SamplingWindow();
            this.renderingWindow1 = new ImagingTests.Rendering.RenderingWindow();
            this.resampeling1 = new ImagingTests.Resampeling.Resampeling();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.palletSelector1 = new ImagingTests.Pallets.PalletSelector();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(784, 562);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.samplingWindow1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(776, 536);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Sampeling";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.renderingWindow1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(776, 536);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Anti-Ailising";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.resampeling1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(776, 536);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Resampeling";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // samplingWindow1
            // 
            this.samplingWindow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.samplingWindow1.Location = new System.Drawing.Point(3, 3);
            this.samplingWindow1.Name = "samplingWindow1";
            this.samplingWindow1.Size = new System.Drawing.Size(770, 530);
            this.samplingWindow1.TabIndex = 0;
            // 
            // renderingWindow1
            // 
            this.renderingWindow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renderingWindow1.Location = new System.Drawing.Point(3, 3);
            this.renderingWindow1.Name = "renderingWindow1";
            this.renderingWindow1.Size = new System.Drawing.Size(770, 530);
            this.renderingWindow1.TabIndex = 0;
            this.renderingWindow1.Load += new System.EventHandler(this.renderingWindow1_Load);
            // 
            // resampeling1
            // 
            this.resampeling1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resampeling1.Location = new System.Drawing.Point(0, 0);
            this.resampeling1.Name = "resampeling1";
            this.resampeling1.Size = new System.Drawing.Size(776, 536);
            this.resampeling1.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.palletSelector1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(776, 536);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Pallets";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // palletSelector1
            // 
            this.palletSelector1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.palletSelector1.Location = new System.Drawing.Point(0, 0);
            this.palletSelector1.Name = "palletSelector1";
            this.palletSelector1.Size = new System.Drawing.Size(776, 536);
            this.palletSelector1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private SuperSmpl.SamplingWindow samplingWindow1;
        private Rendering.RenderingWindow renderingWindow1;
        private System.Windows.Forms.TabPage tabPage3;
        private Resampeling.Resampeling resampeling1;
        private System.Windows.Forms.TabPage tabPage4;
        private Pallets.PalletSelector palletSelector1;
    }
}

