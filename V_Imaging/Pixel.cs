using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulpine.Core.Draw
{
    public struct Pixel
    {
        //stores the color of the pixel
        private Color color;

        //refrences the location of the pixel
        private int posx;
        private int posy;

        public Pixel(int x, int y, Color c)
        {
            posx = x > 0 ? x : 0;
            posy = y > 0 ? y : 0;
            color = c;
        }

        public Color Color
        {
            get { return color; }
        }

        public int Col
        {
            get { return posx; }
        }

        public int Row
        {
            get { return posy; }
        }

    }
}
