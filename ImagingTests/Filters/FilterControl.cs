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
using Vulpine.Core.Draw.Filters;
using Vulpine.Core.Draw.Images;

namespace ImagingTests.Filters
{
    public partial class FilterControl : UserControl
    {
        private ImageSystem myimage;
        private ImageSystem source;
        private ImageBasic working;
        private ImageBasic undo;

        public FilterControl()
        {
            InitializeComponent();
            myimage = new Bitmap(480, 480);
            source = new Bitmap(480, 480);

            DrawMyImageDelegate = (this.DrawMyImage);
        }

        private void LoadFile(string file)
        {
            if (source != null) source.Dispose();
            if (myimage != null) myimage.Dispose();

            source = new ImageSystem(file);
            myimage = new ImageSystem(source);
            working = new ImageBasic(source.Width, source.Height, PixelFormat.Rgb24);
            undo = new ImageBasic(source, PixelFormat.Rgb24);

            //RenderSource();
            DrawMyImage();
        }

        public void ApplyFilter()
        {
            undo.FillData(myimage);

            Filter f = GetFilter();
            f.Apply(undo, working);

            myimage.FillData(working);
            DrawMyImage();
        }

        public void Undo()
        {
            myimage.FillData(undo);
            DrawMyImage();
        }

        public void Reset()
        {
            myimage.FillData(source);
            DrawMyImage();
        }


        public Filter GetFilter()
        {
            switch (listFitlers.SelectedIndex)
            {
                case 0: return FilterMap.Threshold(0.25);
                case 1: return FilterMap.Threshold(0.50);
                case 2: return FilterMap.Threshold(0.75);
                case 3: return FilterMap.Grayscale(Desaturate.Natural);
                case 4: return FilterMap.RedChanel;
                case 5: return FilterMap.GreenChanel;
                case 6: return FilterMap.BlueChanel;
                case 7: return FilterMap.HueBright;
                case 8: return FilterMap.HueOnly;
                case 9: return FilterKernal.Sharpen;
                case 10: return FilterKernal.Emboss;
                case 11: return FilterKernal.SobelVert;
                case 12: return FilterKernal.SobelHorz;
                case 13: return FilterKernal.Laplass;
                case 14: return FilterKernal.Outline;
                case 15: return FilterKernal.BoxBlur3;
                case 16: return FilterKernal.BoxBlur5;
                case 17: return FilterKernal.GauseBlur3;
                case 18: return FilterKernal.GauseBlur5;
                case 19: return FilterKernal.Unsharp5;
                case 20: return new FilterStats(3, Stat.Mean);
                case 21: return new FilterStats(5, Stat.Mean);
                case 22: return new FilterStats(7, Stat.Mean);
                case 23: return new FilterStats(3, Stat.Var);
                case 24: return new FilterStats(5, Stat.Var);
                case 25: return new FilterStats(7, Stat.Var);
                case 26: return new FilterStats(3, Stat.Skew);
                case 27: return new FilterStats(5, Stat.Skew);
                case 28: return new FilterStats(7, Stat.Skew);
                case 29: return new FilterStats(3, Stat.Kurt);
                case 30: return new FilterStats(5, Stat.Kurt);
                case 31: return new FilterStats(7, Stat.Kurt);
                case 32: return new FilterSobel2();
                case 33: return FilterKernal.LineVert;
                case 34: return FilterKernal.LineHorz;
                case 35: return FilterKernal.LineDiag1;
                case 36: return FilterKernal.LineDiag2;
                case 37: return FilterSobel.Horizontal;
                case 38: return FilterSobel.Vertical;
                case 39: return FilterSobel.Magnitude;
                case 40: return FilterSobel.Gradient;
                case 41: return FilterMap.MaxValue;
                case 42: return FilterMap.SwapSatValue;
                case 43: return FilterMap.MidBrightness;
                case 44: return FilterMap.HueValue;
                case 45: return FilterMap.CyanRed;
                case 46: return FilterMap.YellowBlue;
                case 47: return FilterMap.MagentaGreen;
            }

            throw new NotImplementedException();
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

        private void btnApply_Click(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }
    }
}
