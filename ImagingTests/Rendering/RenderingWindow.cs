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
using VImage = Vulpine.Core.Draw.Image;
using VColor = Vulpine.Core.Draw.Color;

using ImagingTests.Properties;

namespace ImagingTests.Rendering
{
    public partial class RenderingWindow : UserControl
    {
        ImageSys myimage;
        Renderor ren;

        private EventHandler<RenderEventArgs> render_event;
        //private EventHandler render_start;
        private EventHandler render_finish;

        public RenderingWindow()
        {
            InitializeComponent();
            myimage = new Bitmap(500, 500);
            ren = new Renderor();

            IncrementBarDelegate = (this.IncrementBar);
            DrawMyImageDelegate = (this.DrawMyImage);
            AppendTextDelegate = (this.AppendText);

            render_event = ren_RenderEvent;
            ren.RenderEvent += render_event;

            //render_start = ren_StartEvent;
            //ren.StartEvent += render_start;

            render_finish = ren_FinishEvent;
            ren.FinishEvent += render_finish;
        }

        private Texture GetTestPatern()
        {
            Texture t = new AliasTestPatern();

            ImageSys pano;
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
                    pano = new Bitmap(Resources.TestSuite + "Panorama01.jpg");
                    ipo = new Interpolent(pano, Intpol.BiLiniar);
                    t = new Stereograph(ipo);
                    break;
                case 4:
                    //pano = Resources.Panorama02;
                    pano = new Bitmap(Resources.TestSuite + "Panorama02.jpg");
                    ipo = new Interpolent(pano, Intpol.BiLiniar);
                    t = new Stereograph(ipo);
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

            pass = Double.TryParse(txtSup.Text, out s);
            ren.Support = pass ? s : 1.5;

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

        #region Itererator Based Connection 

        private void RenderImageThreadStart()
        {
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
            SetRenderor();
            barProgress.Value = 0;
            barProgress.Refresh();

            Texture t = GetTestPatern();
            ThreadStart ts = () => RenderImageThread(t);
            Thread thread = new Thread(ts);

            thread.Start();
        }

        private void RenderImageThread(Texture t)
        {
            DateTime last = DateTime.Now;
            DateTime start = last;

            int count = 0;

            foreach (Pixel pix in ren.Render(t, 500, 500))
            {
                VColor c = pix.Color;
                int x = pix.X;
                int y = pix.Y;

                myimage.SetPixel(x, y, c);

                if (count % 500 == 0)
                {
                    TimeSpan check = DateTime.Now - last;
                    IncrementBar();

                    if (check.TotalMilliseconds > 250.0)
                    {
                        DrawMyImage();
                        last = DateTime.Now;
                    }
                }

                count++;
            }

            //draws the final image
            TimeSpan total = DateTime.Now - start;
            DrawMyImage();
            AppendText(total);
        }

        #endregion

        #region Event Based Connection

        DateTime time_last;
        DateTime time_start;

        private void RenderImageThreadStart2()
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

            ThreadStart s = () => ren.Render(t, myimage);
            Thread thread = new Thread(s);
            thread.Start(); 
        }

        //void ren_StartEvent(object sender, EventArgs e)
        //{
        //    if (this.InvokeRequired)
        //    {
        //        //must invoke the delegate to be thread safe
        //        this.Invoke(render_start, sender, e);
        //    }
        //    else
        //    {
        //        //clears the image of all data
        //        lock (myimage)
        //        {
        //            for (int x = 0; x < 500; x++)
        //            {
        //                for (int y = 0; y < 500; y++)
        //                    myimage.SetPixel(x, y, new VColor());
        //            }
        //        }

        //        DrawMyImage();

        //        barProgress.Value = 0;
        //        barProgress.Refresh();
        //    }
        //}

        private void ren_RenderEvent(object sender, RenderEventArgs e)
        {
            if (e.Count % 500 == 0)
            {
                if (this.InvokeRequired)
                {
                    //must invoke the delegate to be thread safe
                    this.Invoke(render_event, sender, e);
                }
                else
                {
                    barProgress.Increment(1);
                    barProgress.Refresh();

                    TimeSpan check = DateTime.Now - time_last;

                    if (check.TotalMilliseconds > 250.0)
                    {
                        DrawMyImage();
                        time_last = DateTime.Now;
                    }
                }
            }
        }

        private void ren_FinishEvent(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                //must invoke the delegate to be thread safe
                this.Invoke(render_finish, sender, e);
            }
            else
            {
                TimeSpan total = DateTime.Now - time_start;
                lblTime.Text = String.Format("Time:  {0}", total);
                lblTime.Refresh();

                DrawMyImage();
            }
        }

        #endregion 

        
        private void btnLong_Click(object sender, EventArgs e)
        {
            //select thread start (2 or 1)
            RenderImageThreadStart2();       
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
