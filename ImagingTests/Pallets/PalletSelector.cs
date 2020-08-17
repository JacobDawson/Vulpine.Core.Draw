using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Vulpine.Core.Draw;
using Vulpine.Core.Draw.Textures;
using Vulpine.Core.Draw.Images;

using VImage = Vulpine.Core.Draw.Image;
using VColor = Vulpine.Core.Draw.Color;

using ImagingTests.Properties;

namespace ImagingTests.Pallets
{
    public partial class PalletSelector : UserControl
    {
        private ImageSystem myimage;
        private ImageSystem source;

        private ImagePallet pimage;

        private Renderor ren;

        ////stores the avilable pallets
        //private List<Pallet> pallets;

        public PalletSelector()
        {
            InitializeComponent();
            myimage = new Bitmap(480, 480);
            source = new Bitmap(480, 480);

            ren = new Renderor();
            ren.Scaling = Scaling.Streach;      

            IncrementBarDelegate = (this.IncrementBar);
            DrawMyImageDelegate = (this.DrawMyImage);
            AppendTextDelegate = (this.AppendText);
        }

        private void LoadFile(string file)
        {
            if (source != null) source.Dispose();
            if (myimage != null) myimage.Dispose();

            source = new ImageSystem(file);
            myimage = new ImageSystem(source.Width, source.Height);

            RenderSource();
        }


        private void RenderSource()
        {
            Interpolent intpol = new Interpolent(source, Intpol.Nearest);
            ren.Render(intpol, myimage);
            DrawMyImage();
        }

        private void RenderPallet()
        {
            Pallet pallet = GetPallet();
            pimage = new ImagePallet(source.Width, source.Height, pallet);
            pimage.FillData(source);

            Interpolent intpol = new Interpolent(pimage, Intpol.Nearest);
            ren.Render(intpol, myimage);
            //myimage.FillData(pimage);
            DrawMyImage();
        }

        private void RenderDither()
        {
            Pallet pallet = GetPallet();
            double amount = GetAmount();

            pimage = new ImagePallet(source.Width, source.Height, pallet);
            pimage.FillDither(source, amount);

            Interpolent intpol = new Interpolent(pimage, Intpol.Nearest);
            ren.Render(intpol, myimage);
            //myimage.FillData(pimage);
            DrawMyImage();
        }

        private void RenderFS()
        {
            Pallet pallet = GetPallet();
            pimage = new ImagePallet(source.Width, source.Height, pallet);
            pimage.FillDitherFS(source);

            Interpolent intpol = new Interpolent(pimage, Intpol.Nearest);
            ren.Render(intpol, myimage);
            //myimage.FillData(pimage);
            DrawMyImage();
        }

        private double GetAmount()
        {
            switch (cmbAmount.SelectedIndex)
            {
                case 0: return 1.0;
                case 1: return 1.0 / 2.0;
                case 2: return 1.0 / 4.0;
                case 3: return 1.0 / 8.0;
                case 4: return 1.0 / 16.0;
                case 5: return 1.0 / 32.0;
                case 6: return 1.0 / 64.0;
                case 7: return 1.0 / 128.0;
                case 8: return 1.0 / 256.0;

                default: return 1.0 / 4.0;
            }
        }

        private Pallet GetPallet()
        {
            switch (cmbPallet.SelectedIndex)
            {
                case 0: return new Pallet( //Black and White
                    VColor.FromRGB24(0, 0, 0),
                    VColor.FromRGB24(255, 255, 255));
                case 1: return new Pallet( //Normal
                    VColor.FromRGB24(230, 38, 38),
                    VColor.FromRGB24(230, 120, 38),
                    VColor.FromRGB24(230, 230, 38),
                    VColor.FromRGB24(120, 230, 38),
                    VColor.FromRGB24(12, 140, 12),
                    VColor.FromRGB24(52, 184, 215),
                    VColor.FromRGB24(38, 148, 216),
                    VColor.FromRGB24(58, 58, 230),
                    VColor.FromRGB24(138, 74, 230),
                    VColor.FromRGB24(230, 90, 202),
                    VColor.FromRGB24(230, 170, 154),
                    VColor.FromRGB24(140, 78, 60),
                    VColor.FromRGB24(230, 230, 230),
                    VColor.FromRGB24(110, 110, 110),
                    VColor.FromRGB24(0, 0, 0));
                case 2: return new Pallet( //Pastel
                    VColor.FromRGB24(230, 134, 106),
                    VColor.FromRGB24(230, 168, 122),
                    VColor.FromRGB24(230, 194, 170),
                    VColor.FromRGB24(230, 140, 138),
                    VColor.FromRGB24(230, 106, 106),
                    VColor.FromRGB24(174, 216, 190),
                    VColor.FromRGB24(150, 200, 148),
                    VColor.FromRGB24(70, 156, 122),
                    VColor.FromRGB24(158, 200, 90),
                    VColor.FromRGB24(230, 210, 122),
                    VColor.FromRGB24(230, 196, 58),
                    VColor.FromRGB24(200, 154, 78),
                    VColor.FromRGB24(230, 216, 216),
                    VColor.FromRGB24(170, 125, 135),
                    VColor.FromRGB24(12, 64, 82));
                case 3: return new Pallet( //Vibrant
                    VColor.FromRGB24(70, 70, 216),
                    VColor.FromRGB24(106, 132, 230),
                    VColor.FromRGB24(112, 216, 175),
                    VColor.FromRGB24(58, 186, 135),
                    VColor.FromRGB24(32, 126, 108),
                    VColor.FromRGB24(230, 184, 220),
                    VColor.FromRGB24(230, 140, 192),
                    VColor.FromRGB24(170, 52, 150),           
                    VColor.FromRGB24(216, 70, 98),
                    VColor.FromRGB24(230, 162, 58),
                    VColor.FromRGB24(230, 230, 75),
                    VColor.FromRGB24(172, 172, 36),
                    VColor.FromRGB24(216, 224, 228),
                    VColor.FromRGB24(114, 148, 156),
                    VColor.FromRGB24(0, 60, 96));
                case 4: return new Pallet( //Earth Tones
                    VColor.FromRGB24(200, 90, 64),
                    VColor.FromRGB24(216, 148, 38),
                    VColor.FromRGB24(230, 182, 108),
                    VColor.FromRGB24(170, 116, 76),
                    VColor.FromRGB24(110, 60, 50),
                    VColor.FromRGB24(164, 200, 108),
                    VColor.FromRGB24(106, 156, 90),
                    VColor.FromRGB24(65, 110, 55),
                    VColor.FromRGB24(90, 126, 110),
                    VColor.FromRGB24(150, 186, 178),
                    VColor.FromRGB24(136, 136, 186),
                    VColor.FromRGB24(90, 90, 172),
                    VColor.FromRGB24(230, 222, 184),
                    VColor.FromRGB24(140, 110, 102),
                    VColor.FromRGB24(76, 52, 38));
                case 5: return new Pallet( //Orange-Blue-Pink
                    VColor.FromRGB24(230, 230, 58),
                    VColor.FromRGB24(232, 218, 172),
                    VColor.FromRGB24(230, 188, 0),
                    VColor.FromRGB24(230, 170, 74),
                    VColor.FromRGB24(230, 145, 0),
                    VColor.FromRGB24(90, 120, 234),
                    VColor.FromRGB24(58, 58, 230),
                    VColor.FromRGB24(12, 12, 140),
                    VColor.FromRGB24(12, 84, 124),
                    VColor.FromRGB24(230, 154, 216),
                    VColor.FromRGB24(230, 106, 156),
                    VColor.FromRGB24(230, 74, 76),
                    VColor.FromRGB24(230, 230, 230),
                    VColor.FromRGB24(124, 124, 170),
                    VColor.FromRGB24(12, 12, 78));
                case 6: return new Pallet( //Purple-Orange-Green
                    VColor.FromRGB24(140, 0, 90),
                    VColor.FromRGB24(214, 70, 214),
                    VColor.FromRGB24(175, 115, 214),
                    VColor.FromRGB24(130, 70, 216),
                    VColor.FromRGB24(70, 34, 198),
                    VColor.FromRGB24(230, 230, 0),
                    VColor.FromRGB24(230, 188, 0),
                    VColor.FromRGB24(230, 150, 22),
                    VColor.FromRGB24(230, 120, 36),
                    VColor.FromRGB24(196, 230, 36),
                    VColor.FromRGB24(120, 200, 64),
                    VColor.FromRGB24(30, 158, 84),
                    VColor.FromRGB24(230, 230, 216),
                    VColor.FromRGB24(75, 128, 64),
                    VColor.FromRGB24(0, 60, 0));
                case 7: return new Pallet( //Warm Colors
                    VColor.FromRGB24(230, 230, 116),
                    VColor.FromRGB24(230, 230, 0),
                    VColor.FromRGB24(230, 188, 0),
                    VColor.FromRGB24(230, 145, 0),
                    VColor.FromRGB24(230, 100, 0),
                    VColor.FromRGB24(216, 86, 86),
                    VColor.FromRGB24(186, 28, 28),
                    VColor.FromRGB24(126, 0, 0),
                    VColor.FromRGB24(155, 0, 96),
                    VColor.FromRGB24(216, 38, 112),
                    VColor.FromRGB24(214, 124, 146),
                    VColor.FromRGB24(230, 170, 170),
                    VColor.FromRGB24(230, 216, 216),
                    VColor.FromRGB24(156, 90, 90),
                    VColor.FromRGB24(60, 12, 12));
                case 8: return new Pallet( //Cool Colors
                    VColor.FromRGB24(152, 202, 186),
                    VColor.FromRGB24(52, 216, 152),
                    VColor.FromRGB24(26, 172, 144),
                    VColor.FromRGB24(60, 198, 230),
                    VColor.FromRGB24(20, 150, 230),
                    VColor.FromRGB24(20, 102, 216),
                    VColor.FromRGB24(12, 60, 202),
                    VColor.FromRGB24(12, 12, 158),
                    VColor.FromRGB24(72, 40, 186),
                    VColor.FromRGB24(120, 52, 216),
                    VColor.FromRGB24(78, 0, 126),
                    VColor.FromRGB24(46, 0, 78),
                    VColor.FromRGB24(216, 228, 228),
                    VColor.FromRGB24(80, 96, 156),
                    VColor.FromRGB24(14, 14, 78));
                case 9: return new Pallet( //Skintones
                    VColor.FromRGB24(230, 220, 214),
                    VColor.FromRGB24(230, 194, 170),
                    VColor.FromRGB24(230, 174, 138),
                    VColor.FromRGB24(230, 156, 106),
                    VColor.FromRGB24(200, 130, 76),
                    VColor.FromRGB24(230, 140, 138),
                    VColor.FromRGB24(230, 106, 106),
                    VColor.FromRGB24(200, 90, 90),
                    VColor.FromRGB24(186, 94, 70),
                    VColor.FromRGB24(140, 78, 60),
                    VColor.FromRGB24(140, 94, 60),
                    VColor.FromRGB24(110, 74, 46),
                    VColor.FromRGB24(76, 50, 28),
                    VColor.FromRGB24(46, 20, 0),
                    VColor.FromRGB24(0, 0, 0));
                case 10: return new Pallet( //Hair Color
                    VColor.FromRGB24(230, 220, 214),
                    VColor.FromRGB24(230, 206, 108),
                    VColor.FromRGB24(230, 188, 0),
                    VColor.FromRGB24(230, 170, 72),
                    VColor.FromRGB24(230, 146, 0),
                    VColor.FromRGB24(230, 156, 138),
                    VColor.FromRGB24(230, 120, 90),
                    VColor.FromRGB24(214, 86, 52),
                    VColor.FromRGB24(186, 52, 12),
                    VColor.FromRGB24(140, 38, 0),
                    VColor.FromRGB24(170, 95, 38),
                    VColor.FromRGB24(156, 70, 12),
                    VColor.FromRGB24(110, 46, 0),
                    VColor.FromRGB24(78, 34, 0),
                    VColor.FromRGB24(45, 12, 0));
            }

            throw new InvalidOperationException();
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
                //creates the graphics object for the canvis
                Graphics gfx = pnlCanvas.CreateGraphics();
                gfx.InterpolationMode = InterpolationMode.NearestNeighbor;

                //clears the background
                gfx.Clear(System.Drawing.Color.White);

                //computes the ideal width and height for the image
                int w0 = myimage.Width;
                int h0 = myimage.Height;
                int w1 = pnlCanvas.Width;
                int h1 = pnlCanvas.Height;

                int wp = w1;
                int hp = wp * h0 / w0;

                if (hp > h1)
                {
                    hp = h1;
                    wp = hp * w0 / h0;
                }

                //locks the image while we draw it
                lock (myimage.Key)
                {
                    gfx.DrawImage((Bitmap)myimage, 0, 0, wp, hp);
                }

                //releases the graphics object
                gfx.Dispose();
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


        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"H:\Programing\TestImage";
            ofd.ShowDialog(this);
            LoadFile(ofd.FileName);
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = @"H:\Programing\TestImage\Output";
            sfd.ShowDialog(this);

            lock (myimage.Key)
            {
                Bitmap bmp = (Bitmap)myimage;
                bmp.Save(sfd.FileName);
            }
        }

        private void btnOriginal_Click(object sender, EventArgs e)
        {
            RenderSource();
        }

        private void btnPallet_Click(object sender, EventArgs e)
        {
            RenderPallet();
        }

        private void btnDither_Click(object sender, EventArgs e)
        {
            RenderDither();
        }

        private void btnFloyd_Click(object sender, EventArgs e)
        {
            RenderFS();
        }
    }
}
