using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulpine.Core.Draw.Images
{
    public class ImagePallet8 : Image
    {
        //remembers the width and height of the image
        private int width;
        private int height;

        //stores the pallet used to encode the image
        private Pallet pallet;

        //stores the image data as a single bite array
        private byte[] data;


        public ImagePallet8(int width, int height, Pallet pallet)
        {
            //sets the width and the height
            this.width = width;
            this.height = height;
        }

        public override int Width
        {
            get { return width; }
        }

        public override int Height
        {
            get { return height; }
        }

        protected override Color GetPixelInit(int col, int row)
        {
            throw new NotImplementedException();
        }

        protected override void SetPixelInit(int col, int row, Color color)
        {
            throw new NotImplementedException();
        }
    }
}
