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

namespace ImagingTests.Resampeling
{
    public partial class Resampeling : UserControl
    {
        //public const PixelFormat Format = PixelFormat.Rc32;

        public readonly PixelFormat Format = PixelFormat.Rgba64;

        ImageSystem myimage;

        public Resampeling()
        {
            InitializeComponent();
            myimage = new Bitmap(512, 512);
            //myimage = new ImageBasic(512, 512, Format);

            IncrementBarDelegate = (this.IncrementBar);
            DrawMyImageDelegate = (this.DrawMyImage);
            AppendTextDelegate = (this.AppendText);
        }

        private ImageSystem GetBitmap()
        {
            string file = "Checkerboard.png";
            //Bitmap img = Resources.Checkerboard;

            switch (cboImage.SelectedIndex)
            {
                case 0:
                    file = "Animetastic.png"; break;
                case 1:
                    file = "Checkerboard.png"; break;
                case 2:
                    file = "Eevee2.png";  break;
                case 3:
                    file = "FoxCubs.png"; break;
                case 4:
                    file = "Handwriting.png"; break;
                case 5:
                    file = "Leaf.png"; break;
                case 6:
                    file = "Picknick.png"; break;
                case 7:
                    file = "Rainbow.png"; break;
                case 8:
                    file = "RPG.png"; break;
                case 9:
                    file = "MarioWorld.png"; break;
                case 10:
                    file = "Text.png"; break;
                case 11:
                    file = "Winter.png"; break;
            }

            //return (Bitmap)Bitmap.FromFile(Resources.TestSuite + file);

            //return new Bitmap(Resources.TestSuite + file);

            bool tile = cboTileable.Checked;
            ImageExt ext = tile ? ImageExt.TileXY : ImageExt.MirrorXY;
            return new ImageSystem(Resources.TestSuite + file, ext);
        }

        private Texture GetInterpolent(ImageSystem bmp)
        {
            bool tile = cboTileable.Checked;
            ImageExt ext = tile ? ImageExt.TileXY : ImageExt.MirrorXY;
            //ImageSys img = new ImageSys(bmp);

            ImageBasic img = new ImageBasic(bmp.Width, bmp.Height, Format, ext);
            img.FillData((ImageSystem)bmp);

            //ImageSystem img = new ImageSystem(bmp.Width, bmp.Height);
            //img.FillData((ImageSystem)bmp);

            Interpolent ipo = new Interpolent(bmp, Intpol.Default);
            

            switch (cboInterp.SelectedIndex)
            {
                case 0:
                    ipo = new Interpolent(bmp, Intpol.Nearest);
                    break;
                case 1:
                    ipo = new Interpolent(bmp, Intpol.BiLiniar);
                    break;
                case 2:
                    ipo = new Interpolent(bmp, Intpol.BiCubic);
                    break;
                case 3:
                    ipo = new Interpolent(bmp, Intpol.Catrom);
                    break;
                case 4:
                    ipo = new Interpolent(bmp, Intpol.Mitchel);
                    break;
                case 5:
                    ipo = new Interpolent(bmp, Intpol.Sinc3);
                    break;
            }

            if (cboDouble.Checked)
            {
                return new TextureDouble(ipo);
            }
            else
            {
                return ipo;
            }
        }

        private Renderor GetRenderor()
        {
            return new Renderor();
        }

        private void RenderThumbnail()
        {
            Bitmap thumb = (Bitmap)GetBitmap();
            Graphics gfx = pnlOriginal.CreateGraphics();
            gfx.DrawImage(thumb, 0, 0, thumb.Width, thumb.Height);
            gfx.Dispose();
        }

        private void RenderImage()
        {
            ImageSystem bmp = GetBitmap();
            Texture t = GetInterpolent(bmp);
            Renderor r = GetRenderor();

            lock (myimage)
            {
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
                for (int x = 0; x < 512; x++)
                {
                    for (int y = 0; y < 512; y++)
                        myimage.SetPixel(x, y, new VColor());
                }
            }

            DrawMyImage();

            barProgress.Value = 0;
            barProgress.Refresh();
            ImageSystem bmp = GetBitmap();
            Texture t = GetInterpolent(bmp);
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

            foreach (Pixel pix in r.RenderStream(t, 512, 512))
            {
                VColor c = pix.Color;
                int x = pix.Col;
                int y = pix.Row;
                
                lock (myimage)
                {
                    myimage.SetPixel(x, y, c);
                }

                if (pix.Col == 0)
                {
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

        private void cboImage_SelectionChangeCommitted(object sender, EventArgs e)
        {
            RenderThumbnail();
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
