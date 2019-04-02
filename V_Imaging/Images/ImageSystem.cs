using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using VColor = Vulpine.Core.Draw.Color;
using SColor = System.Drawing.Color;

namespace Vulpine.Core.Draw.Images
{
    /// <summary>
    /// This class provides a wrapper for the System.Drawing.Bitmap class, alowing system 
    /// bitmaps to be used as Images within the Vulpine Core Lirbary. This is esential in 
    /// order to save or load image data from disk, as the Vulpine Core Library dose not
    /// directly suport external image formats. For multi-threaded enviroments, it is
    /// recomended to use a diffrent image type.
    /// <remarks>Last Update: 2019-03-26</remarks>
    /// </summary>
    public class ImageSystem : Vulpine.Core.Draw.Image, IDisposable
    {
        //stores the internal bitmap
        private Bitmap bmp;

        //an object used for locking the bitmap
        private object key;

        //stores the dimentions of the image
        private int width;
        private int height;

        /// <summary>
        /// Creates a new wrapper for a curently existing bitmap resorce.
        /// </summary>
        /// <param name="bmp">Curent bitmap</param>
        public ImageSystem(Bitmap bmp)
        {
            this.bmp = bmp;

            key = new object();
            width = bmp.Width;
            height = bmp.Height;
        }

        /// <summary>
        /// Creates a new instance of a system bitmap, with the given widht and
        /// height, to be used inside this wrapper class.
        /// </summary>
        /// <param name="width">Width of the bitmap in pixels</param>
        /// <param name="height">Height of the bitmap in pixels</param>
        public ImageSystem(int width, int height)
        {
            bmp = new Bitmap(width, height);
            key = new object();

            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Loads a given image file from disk, creates a system bitmap for
        /// that image file, and wraps it inside this wrapper class.
        /// </summary>
        /// <param name="file">Location to load</param>
        public ImageSystem(string file)
        {
            bmp = new Bitmap(file);
            key = new object();

            width = bmp.Width;
            height = bmp.Height;
        }

        #region Class Properties...

        /// <summary>
        /// The width of the current image in pixels.
        /// </summary>
        public override int Width
        {
            get { return width; }
        }

        /// <summary>
        /// The height of the current image in pixels.
        /// </summary>
        public override int Height
        {
            get { return height; }
        }

        /// <summary>
        /// An object used to lock the internal bitmap resource. Use this
        /// lock if you intend to use the bitmap outside this wraper.
        /// </summary>
        public object Key
        {
            get { return key; }
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Image Implementaiton...

        /// <summary>
        /// Provides access to the internal pixel data. This method should
        /// only ever be called with bounded indicies.
        /// </summary>
        /// <param name="col">Column from the left</param>
        /// <param name="row">Row from the top</param>
        /// <returns>The color of the desired pixel</returns>
        protected override VColor GetPixelInit(int col, int row)
        {
            SColor sc;

            lock (key)
            {
                sc = bmp.GetPixel(col, row);
            }

            double r = sc.R / 255.0;
            double g = sc.G / 255.0;
            double b = sc.B / 255.0;
            double a = sc.A / 255.0;

            return VColor.FromRGB(r, g, b, a);
        }

        /// <summary>
        /// Provides access to the internal pixel data. This method should
        /// only ever be called with bounded indicies.
        /// </summary>
        /// <param name="col">Column from the left</param>
        /// <param name="row">Row from the top</param>
        /// <param name="color">New color of the pixel</param>
        protected override void SetPixelInit(int col, int row, VColor vc)
        {
            byte r = (byte)Math.Floor((vc.Red * 255.0) + 0.5);
            byte g = (byte)Math.Floor((vc.Green * 255.0) + 0.5);
            byte b = (byte)Math.Floor((vc.Blue * 255.0) + 0.5);
            byte a = (byte)Math.Floor((vc.Alpha * 255.0) + 0.5);

            SColor sc = SColor.FromArgb(a, r, g, b);

            lock (key)
            {
                bmp.SetPixel(col, row, sc);
            }
        }

        /// <summary>
        /// Returns access to the internal bitmap resource. This must be
        /// cast to a system bitmap before it can be used.
        /// </summary>
        /// <returns>An object refrence to the internal bitmap</returns>
        public override object GetInternalData()
        {
            return bmp;
        }

        /// <summary>
        /// Saves the image data to an external file, whos location is 
        /// spesified by the file paramater.
        /// </summary>
        /// <param name="file">Location to save</param>
        public void Save(string file)
        {
            bmp.Save(file);
        }

        /// <summary>
        /// We provide a dispose method to clear up any system resorces that
        /// the internal bitmap structor may be holding.
        /// </summary>
        public void Dispose()
        {
            //disposes of the internal object
            bmp.Dispose();
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Class Conversions...

        /// <summary>
        /// Allows for implicit boxing of system bitmap.
        /// </summary>
        public static implicit operator ImageSystem(Bitmap bmp)
        { return new ImageSystem(bmp); }

        /// <summary>
        /// Allows for explicit unboxing of the internal bitmap.
        /// </summary>
        public static explicit operator Bitmap(ImageSystem img)
        { return img.bmp; }

        #endregion //////////////////////////////////////////////////////////////

    }
}
