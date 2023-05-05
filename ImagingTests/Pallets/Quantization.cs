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
using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Matrices;
using Vulpine.Core.Calc.Algorithms;

using VImage = Vulpine.Core.Draw.Image;
using VColor = Vulpine.Core.Draw.Color;
using VMatrix = Vulpine.Core.Calc.Matrices.Matrix;

using ImagingTests.Properties;

namespace ImagingTests.Pallets
{
    public partial class Quantization : UserControl
    {
        private ImageSystem myimage;
        private ImageSystem source;
        private ImageSystem preview;

        private ImagePallet pimage;

        private Renderor ren;

        private Pallet mypallet;

        private Quantizer quan;
        private ImageBasic downsize;
        private DateTime last_Update;
        private DateTime run_started;

        ////stores the avilable pallets
        //private List<Pallet> pallets;

        public Quantization()
        {
            InitializeComponent();
            myimage = new Bitmap(480, 480);
            source = new Bitmap(480, 480);
            preview = new Bitmap(1080, 64);


            ren = new Renderor();
            ren.Scaling = Scaling.Streach;

            mypallet = null;
            quan = new Quantizer(1000, 1.0 / 1024.0);
            downsize = new ImageBasic(128, 128);

            //quan.StartEvent += new EventHandler(quan_StartEvent);
            //quan.StepEvent += new EventHandler<StepEventArgs>(quan_StepEvent);       
            //quan.FinishEvent += new EventHandler<StepEventArgs>(quan_FinishEvent);
            last_Update = run_started = DateTime.Now;


            IncrementBarDelegate = (this.IncrementBar);
            DrawMyImageDelegate = (this.DrawMyImage);
            //AppendTextDelegate = (this.AppendText);
            DrawMyPalletDelegate = (this.DrawMyPallet);
        }

        

        //private void quan_StartEvent(object sender, EventArgs e)
        //{
        //    run_started = DateTime.Now;
        //}

        //private void quan_StepEvent(object sender, StepEventArgs e)
        //{
        //    TimeSpan ts = DateTime.Now - last_Update;

        //    if (ts.TotalSeconds > 0.5)
        //    {
        //        TimeSpan total = DateTime.Now - run_started;
        //        AppendText(total, e);
        //        last_Update = DateTime.Now;
        //    }
        //}

        //private void quan_FinishEvent(object sender, StepEventArgs e)
        //{
        //    TimeSpan total = DateTime.Now - run_started;
        //    AppendText(total, e);
        //}

        private void LoadFile(string file)
        {
            if (source != null) source.Dispose();
            if (myimage != null) myimage.Dispose();

            source = new ImageSystem(file);
            myimage = new ImageSystem(source.Width, source.Height);

            RenderSource();
        }


        private void GeneratePallet()
        {
            int size = GetSampleSize();
            int pixels = size * size;
            int colors = (int)numColors.Value;
            //int ittr = (int)numPasses.Value;


            ImageBasic ds = new ImageBasic(size, size);
            VMatrix data = new VMatrix(pixels, 3);

            Interpolent intpol = new Interpolent(source, Intpol.Nearest);
            ren.Render(intpol, ds);

            int i = 0;

            foreach (var pixel in ds)
            {
                VColor c = pixel.Color;
                Vector v = c.ToRGB();

                if (i < pixels) data.SetRow(i, v); 
                
                i++;
            }

            var results = quan.KMeans(data, colors);
            var pallet = results.Select(x => VColor.FromRGBA(x));
            var sorted = pallet.OrderBy(x => x.Hue);
            mypallet = new Pallet(ColorSpace.RGB, sorted);

            DisplayPallet(mypallet);
        }


        private void GeneratePalletUnique()
        {
            int size = GetSampleSize();
            int pixels = size * size;
            int colors = (int)numColors.Value;

            ImageBasic ds = new ImageBasic(size, size);
            Queue<Vector> data = new Queue<Vector>(pixels);

            Interpolent intpol = new Interpolent(source, Intpol.Nearest);
            ren.Render(intpol, ds);

            foreach (var pixel in ds)
            {
                VColor c = pixel.Color;
                Vector v = c.ToRGB();
                bool duplicate = false;

                foreach (Vector vi in data)
                {
                    //duplicate |= vi.Equals(v);
                    duplicate |= vi.Dist(v) < (1.0 / 64.0);
                    if (duplicate) break;
                }

                if (!duplicate) data.Enqueue(v);
            }

            VMatrix m = new VMatrix(data.Count, 3);
            int i = 0;

            foreach (Vector v in data)
            {
                m.SetRow(i, v); i++;
            }

            var results = quan.KMeans(m, colors);
            var pallet = results.Select(x => VColor.FromRGBA(x));
            var sorted = pallet.OrderBy(x => x.Hue);
            mypallet = new Pallet(ColorSpace.RGB, sorted);

            DisplayPallet(mypallet);
        }



        private void RenderSource()
        {
            Interpolent intpol = new Interpolent(source, Intpol.Nearest);
            ren.Render(intpol, myimage);
            DrawMyImage();
        }

        private void RenderPallet()
        {
            pimage = new ImagePallet(source.Width, source.Height, mypallet);
            pimage.FillData(source);

            //Interpolent intpol = new Interpolent(pimage, Intpol.Nearest);
            //ren.Render(intpol, myimage);
            myimage.FillData(pimage);
            DrawMyImage();

            DisplayPallet(mypallet);
        }

        private void RenderDither()
        {
            pimage = new ImagePallet(source.Width, source.Height, mypallet);
            pimage.FillDither(source, 0.25);

            //Interpolent intpol = new Interpolent(pimage, Intpol.Nearest);
            //ren.Render(intpol, myimage);
            myimage.FillData(pimage);
            DrawMyImage();

            DisplayPallet(mypallet);
        }

        private void RenderFS()
        {
            pimage = new ImagePallet(source.Width, source.Height, mypallet);
            pimage.FillDitherFS(source);

            //Interpolent intpol = new Interpolent(pimage, Intpol.Nearest);
            //ren.Render(intpol, myimage);
            myimage.FillData(pimage);
            DrawMyImage();

            DisplayPallet(mypallet);
        }

        private void DisplayPallet(Texture pallet)
        {
            ren.Render(pallet, preview);
            DrawMyPallet();            
        }

        private int GetSampleSize()
        {
            switch (cboDownsample.SelectedIndex)
            {
                case 0: case 4: return 32;
                case 1: case 5: return 64;
                case 2: case 6: return 128;
                case 3: case 7: return 256;

                default: return 64;
            }
        }

        private bool GetUniqueFlag()
        {
            int index = cboDownsample.SelectedIndex;

            if (index < 4) return false;
            else return true;
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

        private delegate void DelegateDrawMyPallet();
        private DelegateDrawMyPallet DrawMyPalletDelegate;

        /**
         *  Draws the contents of the buffer image to the screen.
         *  This allows us to show the image as it's being rendered.
         */
        private void DrawMyPallet()
        {
            if (this.InvokeRequired)
            {
                //must invoke the delegate to be thread safe
                this.Invoke(DrawMyPalletDelegate);
            }
            else
            {
                //creates the graphics object for the canvis
                Graphics gfx = pnlPallet.CreateGraphics();
                gfx.InterpolationMode = InterpolationMode.NearestNeighbor;

                //clears the background
                gfx.Clear(System.Drawing.Color.White);

                //obtains the widht and height of the preview pain
                int w = pnlPallet.Width;
                int h = pnlPallet.Height;

                //locks the image while we draw it
                lock (myimage.Key)
                {
                    gfx.DrawImage((Bitmap)preview, 0, 0, w, h);
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

        //private delegate void DelegateAppendText(TimeSpan time, StepEventArgs step);
        //private DelegateAppendText AppendTextDelegate;

        ///**
        // *  Allosw the loading process to apend text to the info window,
        // *  this is primarly to desplay error messages that occor while
        // *  loading the data.
        // */
        //private void AppendText(TimeSpan time, StepEventArgs step)
        //{
        //    if (this.InvokeRequired)
        //    {
        //        //must invoke the delegate to be thread safe
        //        this.Invoke(AppendTextDelegate, time, step);
        //    }
        //    else
        //    {
        //        lblError.Text = String.Format("Error: {0:0.0000}", step.Error);
        //        lblIttr.Text = String.Format("Iterations: {0}", step.Step);
        //        lblTime.Text = String.Format("Time: {0}", time);

        //        pnlStats.Refresh();
        //    }
        //}

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

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            bool unique = GetUniqueFlag();
            if (unique) GeneratePalletUnique();
            else GeneratePallet();
        }
    }
}
