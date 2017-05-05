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

        public RenderingWindow()
        {
            InitializeComponent();
            myimage = new Bitmap(500, 500);

            IncrementBarDelegate = (this.IncrementBar);
            DrawMyImageDelegate = (this.DrawMyImage);
            AppendTextDelegate = (this.AppendText);
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

        private Renderor GetRenderor()
        {
            AntiAilis meth = AntiAilis.None;
            int n = 4;

            bool pass = Int32.TryParse(txtN.Text, out n);
            if (!pass) n = 4;

            switch (cboAA.SelectedIndex)
            {
                case 0:
                    meth = AntiAilis.None;
                    break;
                case 1:
                    meth = AntiAilis.Random;
                    break;
                case 2:
                    meth = AntiAilis.Jittred;
                    break;
                case 3:
                    meth = AntiAilis.Poisson;
                    break;
            }

            return new Renderor(meth, n);
        }

        private void RenderImage()
        {
            lock (myimage)
            {
                Texture t = GetTestPatern();
                Renderor r = GetRenderor();

                r.Render(t, myimage);

                Graphics gfx = pnlCanvas.CreateGraphics();
                //gfx.Clear(System.Drawing.Color.White);
                gfx.DrawImage((Bitmap)myimage, 0, 0);

                gfx.Dispose();
            }
        }

        #region Thread Safe Methods

        private delegate void DelegateIncrementBar();
        private DelegateIncrementBar IncrementBarDelegate;

        /**
         *  This allows the process to report on its progress
         *  by updateing the progress bar each time it completes
         *  processing an image.
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

        private void DrawMyImage()
        {
            if (this.InvokeRequired)
            {
                //must invoke the delegate to be thread safe
                this.Invoke(DrawMyImageDelegate);
            }
            else
            {
                lock (myimage)
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

        private void SetPixel(int x, int y, VColor c)
        {
            lock (myimage)
            {
                myimage.SetPixel(x, y, c);
            }
        }

        #endregion

        private void RenderImageThreadStart()
        {
            //clears the image of all data
            lock (myimage)
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
            Texture t = GetTestPatern();
            Renderor r = GetRenderor();
            object[] data = { t, r };


            //kicks off the new threaded process
            Thread thread = new Thread(new ParameterizedThreadStart(RenderImageThread));
            thread.IsBackground = true;
            thread.Start(data);
        }

        private void RenderImageThread(object data)
        {
            object[] paramaters = (object[])data;
            Texture t = (Texture)paramaters[0];
            Renderor r = (Renderor)paramaters[1];

            DateTime last = DateTime.Now;
            DateTime start = last;
            

            int x = 0;
            int y = 0;

            foreach (VColor pixel in r.Render(t, 500, 500))
            {
                SetPixel(x, y, pixel);
                x++;

                if (x >= 500)
                {
                    x = 0; y++;
                    TimeSpan check = DateTime.Now - last;
                    IncrementBar();

                    if (check.TotalMilliseconds > 250.0)
                    {
                        DrawMyImage();
                        last = DateTime.Now;
                    }
                }
            }

            //draws the final image
            TimeSpan total = DateTime.Now - start;
            DrawMyImage();
            AppendText(total);
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            RenderImage();

        }

        private void btnLong_Click(object sender, EventArgs e)
        {
            RenderImageThreadStart();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.ShowDialog();

            lock (myimage)
            {
                Bitmap bmp = (Bitmap)myimage;
                bmp.Save(sfd.FileName);
            }
        }
    }
}
