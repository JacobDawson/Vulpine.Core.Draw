using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Draw;

namespace ImagingTests.Rendering
{
    public class AliasTestPatern : Texture
    {
        public Color Sample(double u, double v)
        {
            Color black = Color.FromRGB(0.0, 0.0, 0.0);
            Color white = Color.FromRGB(1.0, 1.0, 1.0);

            //double x = u * 128.0;
            //double y = v * 128.0;

            double x = (1.0 + u) * 64.0;
            double y = (1.0 - v) * 64.0;

            return (GeneratePattern(x, y) ? white : black);
        }

        public bool GeneratePattern(double x, double y)
        {
            double t, z;
            int i, j, k;

            x = x / 128.0 - 0.5;
            y = y / 2048.0;

            t = 1.0 / (y + 0.001);
            z = t * x;

            i = (int)Math.Floor(t);
            j = (int)Math.Floor(z);
            k = i + j;

            return ((k % 2) != 0);
        }
    }
}
