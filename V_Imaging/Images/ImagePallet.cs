﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulpine.Core.Draw.Images
{
    /// <summary>
    /// A palleted image uses a color pallet to encode it's data. Instead of
    /// storing the color data for each pixel, it simply stores and index into
    /// the pallet. This allows for detailed images to be stored very compactly.
    /// This class of palated image allows for up to 255 colors to be stored in
    /// it's pallet. An aditonal transparent color may also be stored.
    /// </summary>
    public class ImagePallet : Image
    {
        #region Class Definitions...

        //remembers the width and height of the image
        private int width;
        private int height;

        //stores the pallet used to encode the image
        private Pallet pallet;

        //stores the image data as a single array
        private byte[] data;

        //determins if transparent colors are stored
        private bool trans;

        /// <summary>
        /// Constructs a new palleted image, with the given witdth, height,
        /// and color pallet. Transparent colors are not included.
        /// </summary>
        /// <param name="width">Width of the image</param>
        /// <param name="height">Height of the image</param>
        /// <param name="pallet">Color pallet for the image</param>
        public ImagePallet(int width, int height, Pallet pallet)
        {
            //checks that we can actualy represent all the colors
            if (pallet.NumColors > 255) 
                throw new InvalidOperationException();

            //sets the width and the height
            this.width = width;
            this.height = height;
            this.trans = false;

            //creates the array to store the data
            data = new byte[width * height];

            //makes an internal copy of the pallet
            this.pallet = new Pallet(pallet);

            //pre-builds the pallet
            this.pallet.BuildPalet();
        }

        /// <summary>
        /// Constructs a new palleted image, with the given witdth, height,
        /// and color pallet. Transparent pixels may optionaly be stored
        /// within the image.
        /// </summary>
        /// <param name="width">Width of the image</param>
        /// <param name="height">Height of the image</param>
        /// <param name="pallet">Color pallet for the image</param>
        /// <param name="trans">True to store transparent pixels</param>
        public ImagePallet(int width, int height, Pallet pallet, bool trans)
        {
            //checks that we can actualy represent all the colors
            if (pallet.NumColors > 255)
                throw new InvalidOperationException();

            //sets the width and the height
            this.width = width;
            this.height = height;
            this.trans = trans;

            //creates the array to store the data
            data = new byte[width * height];

            //makes an internal copy of the pallet
            this.pallet = new Pallet(pallet);

            //pre-builds the pallet
            this.pallet.BuildPalet();
        }

        #endregion //////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// The width of the current image.
        /// </summary>
        public override int Width
        {
            get { return width; }
        }

        /// <summary>
        /// The height of the current image.
        /// </summary>
        public override int Height
        {
            get { return height; }
        }

        /// <summary>
        /// Determins if transparent pixels are included.
        /// </summary>
        public bool Transparent
        {
            get { return trans; }
        }

        /// <summary>
        /// Determins the number of colors within the image's pallet.
        /// </summary>
        public int NumColors
        {
            get
            {
                int count = pallet.NumColors;
                if (trans) count += 1;
                return count;
            }
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
        protected override Color GetPixelInit(int col, int row)
        {
            //locates the desiered pixel
            int ix = (col * height) + row;
            int px = data[ix];

            //returns the color from the pallet
            return pallet.GetColor(px);
        }

        /// <summary>
        /// Provides access to the internal pixel data. This method should
        /// only ever be called with bounded indicies.
        /// </summary>
        /// <param name="col">Column from the left</param>
        /// <param name="row">Row from the top</param>
        /// <param name="color">New color of the pixel</param>
        protected override void SetPixelInit(int col, int row, Color color)
        {
            //locates the desiered pixel
            int ix = (col * height) + row;;

            if (trans && color.Alpha < 0.5)
            {
                //stores the index outside the range
                data[ix] = (byte)pallet.NumColors;
            }
            else
            {
                //stores the index of the color
                int px = pallet.GetIndex(color);
                data[ix] = (byte)px;
            }
        }

        /// <summary>
        /// Obtains a copy of the pallet used to encode the image.
        /// </summary>
        /// <returns>A copy of the pallet used to encode the image</returns>
        public Pallet GetPallet()
        {
            return new Pallet(pallet);
        }

        #endregion //////////////////////////////////////////////////////////////
    }
}