using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using Vulpine.Core.Draw;
using Vulpine.Core.Draw.Textures;
using Vulpine.Core.Draw.Images;
using VImage = Vulpine.Core.Draw.Image;
using VColor = Vulpine.Core.Draw.Color;

using VulpineAnimator.Animations;

namespace VulpineAnimator
{
    public partial class MainForm : Form
    {
        public const int STEP = 4096;

        ImageSys myimage;
        Renderor ren;

        VImage back_buffer;
        Renderor quick;

        int frame_no;
        bool halt = true;
        bool twice = true;

        TimeSpan eta;
        TimeSpan elap;

        public MainForm()
        {
            InitializeComponent();
            
            myimage = new Bitmap(480, 480);
            ren = new Renderor();

            //back_buffer = new ImageVector(960, 960);
            back_buffer = new ImageBasic(960, 960, PixelFormat.Rgb48);
            quick = new Renderor();
            quick.Scaling = Scaling.Streach;

            IncrementFrameDelegate = (this.IncrementFrame);
            IncrementTotalDelegate = (this.IncrementTotal);
            DrawBufferDelegate = (this.DrawBuffer);
            ButtonResetDelegate = (this.ButtonReset);
            UpdateLableDelegate = (this.UpdateLable);

            ren.RenderEvent += ren_RenderEvent;
            ren.FinishEvent += ren_FinishEvent;

            //quick.RenderEvent += ren_RenderEvent;
        }

        private Animation GetAnimation()
        {
            //return new BranchCuts();
            //return new SecantStar();
            //return new FountainSpiral();
            //return new WeirdPowers();
            //return new SecantRotation();
            //return new OrbitingRoots();
            //return new LaplassResonanceDX();
            //return new LaplassResonanceFractal();

            //return new ExponentialChange();
            //return new ContDerivation();
            //return new YuvPlanes();
            //return new CIEPlanes();


            return new ElepticIntergrals();

            //throw new NotImplementedException();
        }


        private void SetRenderor()
        {
            ren.AitiAilising = chkAA.Checked;
            ren.Jitter = false;

            bool pass = false;
            double s = 1.5;
            int n = 4;

            pass = Int32.TryParse(txtSamp.Text, out n);
            ren.Samples = pass ? n : 4;

            pass = Double.TryParse(txtRadius.Text, out s);
            ren.Radius = pass ? s : 1.5;

            switch (cboWeight.SelectedIndex)
            {
                case 0: ren.Window = Window.Box; break;
                case 1: ren.Window = Window.Tent; break;
                case 3: ren.Window = Window.Cosine; break;
                case 4: ren.Window = Window.Gausian; break;
                case 5: ren.Window = Window.Sinc; break;
                case 6: ren.Window = Window.Lanczos; break;
            }

        }

        private void SetBuffer()
        {
            int width, height;
            bool pass = false;

            pass = Int32.TryParse(txtWidth.Text, out width);
            width = pass ? width : 960;

            pass = Int32.TryParse(txtHeight.Text, out height);
            height = pass ? height : 720;

            myimage.Dispose();
            myimage = new Bitmap(width, height);

            if (chkSuper.Checked)
            {
                //back_buffer = new ImageVector(width * 2, height * 2);
                back_buffer = new ImageBasic(width * 2, height * 2, PixelFormat.Rgb48);
            }
        }

        #region Thread Safe Methods

        private delegate void DelegateIncrementFrame();
        private DelegateIncrementFrame IncrementFrameDelegate;

        /**
         *  This allows the process to report on its progress
         *  by updateing the progress bar each time it completes
         *  a row of the rendered image.
         */
        private void IncrementFrame()
        {
            if (this.InvokeRequired)
            {
                //must invoke the delegate to be thread safe
                this.Invoke(IncrementFrameDelegate);
            }
            else
            {
                //we are safe, increment the bar
                barFrame.Increment(1);
                barFrame.Refresh();

                double per = barFrame.Value / (double)barFrame.Maximum * 100.0;
                lblFramePer.Text = String.Format("{0:00.00} %", per);
                lblFramePer.Refresh();
            }
        }

        private delegate void DelegateIncrementTotal();
        private DelegateIncrementTotal IncrementTotalDelegate;

        /**
         *  This allows the process to report on its progress
         *  by updateing the progress bar each time it completes
         *  a row of the rendered image.
         */
        private void IncrementTotal()
        {
            if (this.InvokeRequired)
            {
                //must invoke the delegate to be thread safe
                this.Invoke(IncrementTotalDelegate);
            }
            else
            {
                //we are safe, increment the bar
                barTotal.Increment(1);
                barTotal.Refresh();
                frame_no += 1;

                barFrame.Value = 0;
                barFrame.Refresh();

                lblFramePer.Text = "00.00 %";
                lblFramePer.Refresh();

                txtStart.Text = frame_no.ToString();
                txtStart.Refresh();

                //NOTE: Consider Drawing the frame here as well??

                double per = barTotal.Value / (double)barTotal.Maximum * 100.0;
                lblTotalPer.Text = String.Format("{0:00.00} %", per);
                lblTotalPer.Refresh();
            }
        }

        private delegate void DelegateDrawBuffer();
        private DelegateDrawBuffer DrawBufferDelegate;

        /**
         *  Draws a preview of what's curently in the image buffer
         */
        private void DrawBuffer()
        {
            if (this.InvokeRequired)
            {
                //must invoke the delegate to be thread safe
                this.Invoke(DrawBufferDelegate);
            }
            else
            {
                lock (myimage.Key)
                {
                    Graphics gfx = pnlCanvas.CreateGraphics();
                    gfx.DrawImage((Bitmap)myimage, 0, 0, 
                        pnlCanvas.Width, pnlCanvas.Height);
                    gfx.Dispose();
                }
            }
        }

        private delegate void DelegateButtonReset();
        private DelegateButtonReset ButtonResetDelegate;

        private void ButtonReset()
        {
            if (this.InvokeRequired)
            {
                //must invoke the delegate to be thread safe
                this.Invoke(ButtonResetDelegate);
            }
            else
            {
                halt = true;
                btnStartStop.Text = "START !";
                btnStartStop.Refresh();
            }
        }

        private delegate void DelegateUpdateLable();
        private DelegateUpdateLable UpdateLableDelegate;

        private void UpdateLable()
        {
            if (this.InvokeRequired)
            {
                //must invoke the delegate to be thread safe
                this.Invoke(UpdateLableDelegate);
            }
            else
            {
                string seta = eta.ToString(@"dd\:hh\:mm\:ss");
                this.Text = String.Format("Vulpine Animtor [ETA: {0}]", seta);

                string sela = elap.ToString(@"dd\:hh\:mm\:ss");
                lblInfo.Text = String.Format("Elapsed: {0}", sela);

            }
        }

        #endregion

        

        private void RenderStart()
        {
            Animation ani = GetAnimation();
            SetRenderor();
            SetBuffer();

            int start, stop;
            bool pass = false;

            pass = Int32.TryParse(txtStart.Text, out start);
            start = pass ? start : 1;

            pass = Int32.TryParse(txtFinish.Text, out stop);
            stop = pass ? stop : 1800;

            DrawBuffer();
            frame_no = start;

            twice = chkSuper.Checked;

            if (twice)
            {
                barFrame.Maximum = back_buffer.Size / STEP;
                barFrame.Value = 0;
                barFrame.Refresh();
            }
            else
            {
                barFrame.Maximum = myimage.Size / STEP;
                barFrame.Value = 0;
                barFrame.Refresh();
            }

            barTotal.Maximum = stop;
            barTotal.Value = start;
            barTotal.Refresh();

            halt = false;
            btnStartStop.Text = "HALT !";
            btnStartStop.Refresh();

            ThreadStart s = () => RenderMainLoop(ani, start, stop);
            Thread thread = new Thread(s);
            thread.Start(); 
        }

        private void RenderMainLoop(Animation ani, int start, int stop)
        {
            DateTime epoc = DateTime.Now;

            for (int i = start; i <= stop; i++)
            {
                string file = String.Format(@"S:\Animation\frame_{0:0000}.png", i);
                Texture frame = ani.GetFrame(i);

                if (twice)
                {
                    ren.Render(frame, back_buffer);
                    Texture temp = new Interpolent(back_buffer, Intpol.BiCubic);

                    //barFrame.Value = 0;
                    quick.Render(temp, myimage);
                }
                else
                {
                    //we render the image straight
                    ren.Render(frame, myimage);
                }

                lock (myimage.Key)
                {
                    Bitmap bmp = (Bitmap)myimage;
                    bmp.Save(file);
                }

                TimeSpan span = DateTime.Now - epoc;
                double per = span.TotalSeconds / (i - start + 1);
                double rem = per * (stop - i);

                eta = TimeSpan.FromSeconds(rem);
                elap = span;
                UpdateLable();

                IncrementTotal();
                DrawBuffer();

                if (halt)
                {
                    ButtonReset();
                    break;
                }
            }
        }

        private void ren_RenderEvent(object sender, RenderEventArgs e)
        {
            if (e.Count % STEP == 0)
            {
                IncrementFrame();
            }
        }

        private void ren_FinishEvent(object sender, EventArgs e)
        {
            //IncrementTotal();
            //DrawBuffer();
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (halt)
            {
                RenderStart();
            }
            else
            {
                halt = true;
            }
        }

        
    }
}
