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

using ImagingTests.Properties;

namespace ImagingTests.Rendering
{
    public partial class RenderingWindow : UserControl
    {
        //NOTE: This is a test for Github, please ignore
        //NOTE: Change line 214 to test multithreading

        ImageSys myimage;
        Renderor ren;

        public RenderingWindow()
        {
            InitializeComponent();
            myimage = new Bitmap(500, 500);
            ren = new Renderor();

            IncrementBarDelegate = (this.IncrementBar);
            DrawMyImageDelegate = (this.DrawMyImage);
            AppendTextDelegate = (this.AppendText);

            ren.RenderEvent += ren_RenderEvent;
            ren.FinishEvent += ren_FinishEvent;
        }

        private Texture GetTestPatern()
        {
            Texture t = new AliasTestPatern();

            ImageSystem bmp;
            ImageColor64 pano;
            Interpolent ipo;


            switch (cboPattern.SelectedIndex)
            {
                case 0:
                    t = new AliasTestPatern();
                    break;
                case 1:
                    t = new ComplexTestPatern();
                    break;
                case 2:
                    t = ColorWheel.Modulated;
                    break;
                case 3:
                    //pano = Resources.Panorama01;
                    bmp = new Bitmap(Resources.TestSuite + "Panorama01.jpg");
                    pano = new ImageColor64(bmp.Width, bmp.Height);
                    pano.FillData(bmp);

                    ipo = new Interpolent(pano, Intpol.BiLiniar);
                    t = new Stereograph(ipo);
                    break;
                case 4:
                    //pano = Resources.Panorama02;
                    bmp = new Bitmap(Resources.TestSuite + "Panorama02.jpg");
                    pano = new ImageColor64(bmp.Width, bmp.Height);
                    pano.FillData(bmp);

                    ipo = new Interpolent(pano, Intpol.BiLiniar);
                    t = new Stereograph(ipo);
                    break;
                case 5:
                    t = new NewtonFractal();
                    break;
                case 6:
                    t = new NewtonFractal2();
                    break;
            }

            return t;
        }

        private void SetRenderor()
        {
            ren.AitiAilising = chkAA.Checked;
            ren.Jitter = chkJit.Checked;

            bool pass = false;
            double s = 1.5;
            int n = 4;

            pass = Int32.TryParse(txtSamp.Text, out n);
            ren.Samples = pass ? n : 4;

            pass = Double.TryParse(txtRad.Text, out s);
            ren.Radius = pass ? s : 1.5;

            switch (cboWin.SelectedIndex)
            {
                case 0: ren.Window = Window.Box; break;
                case 1: ren.Window = Window.Tent; break;
                case 3: ren.Window = Window.Cosine; break;
                case 4: ren.Window = Window.Gausian; break;
                case 5: ren.Window = Window.Sinc; break;
                case 6: ren.Window = Window.Lanczos; break;
            }

        }

        #region Thread Safe Methods

        private delegate void DelegateDrawMyImage();
        private DelegateDrawMyImage DrawMyImageDelegate;

        /**
         *  Draws the contents of the buffer image to the screen.
         *  This allows us to show the image as it's being rendered.
         */
        private void DrawMyImage()
        {
            if (this.InvokeRequired)
            {
                //must invoke the delegate to be thread safe
                this.Invoke(DrawMyImageDelegate);
            }
            else
            {
                lock (myimage.Key)
                {
                    Graphics gfx = pnlCanvas.CreateGraphics();
                    gfx.DrawImage((Bitmap)myimage, 0, 0);
                    gfx.Dispose();
                }
            }
        }

        private delegate void DelegateIncrementBar();
        private DelegateIncrementBar IncrementBarDelegate;

        /**
         *  This allows the process to report on its progress
         *  by updateing the progress bar each time it completes
         *  a row of the rendered image.
         */
        private void IncrementBar()
        {
            if (this.InvokeRequired)
            {
                //must invoke the delegate to be thread safe
                this.Invoke(IncrementBarDelegate);
            }
            else
            {
                //we are safe, increment the bar
                barProgress.Increment(1);
                barProgress.Refresh();
            }
        }

        private delegate void DelegateAppendText(TimeSpan time);
        private DelegateAppendText AppendTextDelegate;

        /**
         *  Allosw the loading process to apend text to the info window,
         *  this is primarly to desplay error messages that occor while
         *  loading the data.
         */
        private void AppendText(TimeSpan time)
        {
            if (this.InvokeRequired)
            {
                //must invoke the delegate to be thread safe
                this.Invoke(AppendTextDelegate, time);
            }
            else
            {
                lblTime.Text = String.Format("Time:  {0}", time);
                lblTime.Refresh();
            }
        }

        #endregion

        #region Event Based Connection

        DateTime time_last;
        DateTime time_start;

        private void RenderImageThreadStart()
        {
            SetRenderor();

            Texture t = GetTestPatern();
            time_last = DateTime.Now;
            time_start = time_last;

            //clears the image of all data
            lock (myimage.Key)
            {
                for (int x = 0; x < 500; x++)
                {
                    for (int y = 0; y < 500; y++)
                    myimage.SetPixel(x, y, new VColor());
                }
            }

            DrawMyImage();

            barProgress.Value = 0;
            barProgress.Refresh();

            //ThreadStart s = () => ren.Render(t, myimage);
            ThreadStart s = () => ren.RenderParallel(t, myimage);
            Thread thread = new Thread(s);
            thread.Start(); 
        }

        private void ren_RenderEvent(object sender, RenderEventArgs e)
        {
            if (e.Count % 500 == 0)
            {
                TimeSpan check = DateTime.Now - time_last;
                IncrementBar();

                if (check.TotalMilliseconds > 250.0)
                {
                    DrawMyImage();
                    time_last = DateTime.Now;
                }
            }
        }

        private void ren_FinishEvent(object sender, EventArgs e)
        {
            TimeSpan total = DateTime.Now - time_start;
            DrawMyImage();
            AppendText(total);
        }

        #endregion 

        
        private void btnLong_Click(object sender, EventArgs e)
        {
            RenderImageThreadStart();       
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.ShowDialog();

            lock (myimage.Key)
            {
                Bitmap bmp = (Bitmap)myimage;
                bmp.Save(sfd.FileName);
            }
        }
    }
}
