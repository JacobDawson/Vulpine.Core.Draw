using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Draw;

namespace ImagingTests.Resampeling
{
    public class TextureDouble : Texture
    {
        private Texture inner;

        public TextureDouble(Texture inner)
        {
            this.inner = inner;
        }



        public Color Sample(double u, double v)
        {
            double un = u * 2.0;
            double vn = v * 2.0;

            return inner.Sample(un, vn);
        }
    }
}
