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


namespace ImagingTests.Transformation
{

    public partial class Transformation : UserControl
    {
        private ImageSystem myimage;
        private ImageSystem source;
        //private ImageSystem preview;

        private Renderor ren;

        private double scale = 1.0;
        private double rot = 0.0;

        private DateTime start;
        private DateTime last;

        private int pixcount;

        public Transformation()
        {
            InitializeComponent();
            myimage = new Bitmap(480, 480);
            source = new Bitmap(480, 480);
            //preview = new Bitmap(1080, 64);


            ren = new Renderor();
            ren.Scaling = Scaling.Vertical;

            IncrementBarDelegate = (this.IncrementBar);
            DrawMyImageDelegate = (this.DrawMyImage);
            AppendTextDelegate = (this.AppendText);
            SetFileNameDelegate = (this.SetFileName);
            SetScrollLablesDelegate = (this.SetScrollLables);

            ren.StartEvent += new EventHandler(ren_StartEvent);
            ren.RenderEvent += new EventHandler<RenderEventArgs>(ren_RenderEvent);
            ren.FinishEvent += new EventHandler(ren_FinishEvent);
        }

        void ren_FinishEvent(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            AppendText(now - start);
            DrawMyImage();

            barProgress.Value = 0;
            barProgress.Refresh();
        }

        void ren_RenderEvent(object sender, RenderEventArgs e)
        {
            //IncrementBar();

            DateTime now = DateTime.Now;
            TimeSpan pass = now - last;

            pixcount++;

            if (pass.TotalSeconds > 1.0)
            {               
                AppendText(now - start);                
                DrawMyImage();
                IncrementBar(pixcount);

                last = now;
                pixcount = 0;
            }

        }

        void ren_StartEvent(object sender, EventArgs e)
        {
            start = last = DateTime.Now;
            pixcount = 0;
        }

        private void LoadFile(string file)
        {
            if (source != null) source.Dispose();
            if (myimage != null) myimage.Dispose();

            //ImageExt tile = GetTileing();
            //source = new ImageSystem(file, tile);
            source = new ImageSystem(file);
            myimage = new ImageSystem(source.Width, source.Height);

            string name = file.Split('\\').LastOrDefault();
            name = name ?? "error reading filename";

            SetFileName(name);
            //RenderSource();
        }


        private void RenderSource()
        {
            Interpolent intpol = new Interpolent(source, Intpol.Nearest);
            ren.Render(intpol, myimage);
            DrawMyImage();
        }

        private void RenderTransform()
        {
            int w = pnlCanvas.Width;
            int h = pnlCanvas.Height;

            if (myimage != null) myimage.Dispose();
            myimage = new ImageSystem(w, h);

            barProgress.Maximum = w * h;
            barProgress.Value = 0;

            Texture t = BuildTransform();
            ren.Render(t, myimage);

            DrawMyImage();
        }

        


        private Texture BuildTransform()
        {
            Intpol i = GetInterpolation();
            TexBorder b = GetBorder();
            VColor bg = GetBackground();
            Scaling s = GetScaling();
            Boolean cf = UseColor();


            Interpolent t = null;
            if (cf) t = new Interpolent(source, i, s, bg);
            else t = new Interpolent(source, i, s, b);

            TextureTransform tt = new TextureTransform(t,
                px: Double.Parse(txtX.Text),
                py: Double.Parse(txtY.Text),
                rot: rot,
                sx: scale,
                sy: scale
            );

            return tt;
        }


        private Intpol GetInterpolation()
        {
            switch (cboInterp.SelectedIndex)
            {
                case 0: return Intpol.Nearest;
                case 1: return Intpol.BiLiniar;
                case 2: return Intpol.BiCubic;
                case 3: return Intpol.Catrom;
                case 4: return Intpol.Mitchel;
                case 5: return Intpol.Sinc3;

                default: return Intpol.BiLiniar;
            }
        }

        private TexBorder GetBorder()
        {
            switch (cboBackground.SelectedIndex)
            {
                case 0: return TexBorder.None;
                case 1: return TexBorder.Transparent;

                default: return TexBorder.Matte;               
            }
        }

        private bool UseColor()
        {
            switch (cboBackground.SelectedIndex)
            {
                case 0: return false;
                case 1: return false;

                default: return true;
            }
        }

        private VColor GetBackground()
        {
            switch (cboBackground.SelectedIndex)
            {
                case 2: return VColor.FromRGB(0.0, 0.0, 0.0);
                case 3: return VColor.FromRGB(1.0, 0.0, 1.0);
                case 4: return VColor.FromRGB(1.0, 1.0, 1.0);

                default: return VColor.FromRGB(1.0, 1.0, 1.0);
            }
        }

        private Scaling GetScaling()
        {
            switch (cboScaling.SelectedIndex)
            {
                case 0: return Scaling.Horizontal;
                case 1: return Scaling.Vertical;
                case 3: return Scaling.Streach;

                default: return Scaling.Vertical;
            }
        }

        //private ImageExt GetTileing()
        //{
        //    bool tx = chkTileX.Checked;
        //    bool ty = chkTileY.Checked;

        //    if (tx)
        //    {
        //        if (ty) return ImageExt.TileXY;
        //        else return ImageExt.TileX_MirrorY;
        //    }
        //    else
        //    {
        //        if (ty) return ImageExt.MirrorX_TileY;
        //        else return ImageExt.MirrorXY;
        //    }
        //}

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
                lock (myimage)
                {
                    Graphics gfx = pnlCanvas.CreateGraphics();
                    gfx.DrawImage((Bitmap)myimage, 0, 0);
                    gfx.Dispose();
                }
            }
        }

        private delegate void DelegateIncrementBar(int amount);
        private DelegateIncrementBar IncrementBarDelegate;

        /**
         *  This allows the process to report on its progress
         *  by updateing the progress bar each time it completes
         *  a row of the rendered image.
         */
        private void IncrementBar(int amount)
        {
            if (this.InvokeRequired)
            {
                //must invoke the delegate to be thread safe
                this.Invoke(IncrementBarDelegate, amount);
            }
            else
            {
                //we are safe, increment the bar
                barProgress.Increment(amount);
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

        private delegate void DelegateSetFileName(string name);
        private DelegateSetFileName SetFileNameDelegate;

        private void SetFileName(string name)
        {
            if (this.InvokeRequired)
            {
                //must invoke the delegate to be thread safe
                this.Invoke(SetFileNameDelegate, name);
            }
            else
            {
                txtFileName.Text = name;
                txtFileName.Refresh();
            }
        }

        private delegate void DelegateSetScrollLables();
        private DelegateSetScrollLables SetScrollLablesDelegate;

        private void SetScrollLables()
        {
            if (this.InvokeRequired)
            {
                //must invoke the delegate to be thread safe
                this.Invoke(SetScrollLablesDelegate);
            }
            else
            {
                string ss = scale.ToString("0.00");
                ss = String.Format("Scale: {0}", ss);

                string rs = rot.ToString("0.0");
                rs = String.Format("Rotation: {0} deg", rs);

                lblRotation.Text = rs;
                lblScale.Text = ss;

                lblRotation.Refresh();
                lblScale.Refresh();
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

        private void barScale_Scroll(object sender, ScrollEventArgs e)
        {
            double s = barScale.Value;
            s = (s / 100.0) + 1.0;
            scale = s;

            SetScrollLables();
        }

        private void barRoation_Scroll(object sender, ScrollEventArgs e)
        {
            double r = barRoation.Value;
            r = (r / 100.0) * 180.0;
            rot = r;

            SetScrollLables();
        }

        private void btnLong_Click(object sender, EventArgs e)
        {
            RenderTransform();
        }
    }
}
