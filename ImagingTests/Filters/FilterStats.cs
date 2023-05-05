using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Draw;
using Vulpine.Core.Draw.Filters;
using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Matrices;

namespace ImagingTests.Filters
{
    public class FilterStats : Filter
    {

        private int size;
        private Stat mode;

        public FilterStats(int size, Stat mode)
        {
            this.size = size;
            this.mode = mode;
        }

        public override int Size
        {
            get { return size; }
        }

        public override Color Sample(Image source, int x, int y)
        {
            int half = size / 2;
            StatRunner sr = new StatRunner(3);

            for (int i = -half; i <= half; i++)
            {
                for (int j = -half; j <= half; j++)
                {
                    int xp = x + i;
                    int yp = y + j;
                    Vector vec = source.GetPixel(xp, yp).ToRGB();
                    sr.Add(vec);
                }
            }

            switch (mode)
            {
                case Stat.Mean:
                    Vector mean = sr.Mean;
                    return Color.FromRGBA(mean);
                case Stat.Var:
                    double var = sr.Var;
                    return Color.FromRGB(var, var, var);
                case Stat.Skew:
                    Vector skew = sr.Skew;
                    //skew = skew + new Vector(2.0, 2.0, 2.0);
                    //skew = skew / 4.0;

                    ////if (skew[0] < 0.0) return Color.Black;
                    ////if (skew[1] < 0.0) return Color.Black;
                    ////if (skew[2] < 0.0) return Color.Black;
                    ////if (skew[0] > 1.0) return Color.FromRGB(1.0, 0.0, 0.0);
                    ////if (skew[1] > 1.0) return Color.FromRGB(1.0, 0.0, 0.0);
                    ////if (skew[2] > 1.0) return Color.FromRGB(1.0, 0.0, 0.0);
                    ////return Color.White;


                    double xs = skew[0];
                    double ys = skew[1];
                    double zs = skew[2];

                    double rad = (xs * xs) + (ys * ys) + (zs * zs);
                    rad =  Math.Sqrt(rad);

                    double lat = Math.Acos(zs / rad);
                    //lat = lat - (Math.PI / 2);
                    lat = lat / Math.PI;

                    double lon = Math.Atan2(ys, xs);
                    lon = VMath.ToDeg(lon);


                    return Color.FromHSL(lon, rad, lat);



                    //return Color.FromRGBA(skew);



                case Stat.Kurt:
                    double kurt = sr.Kurt;
                    return Color.FromRGB(kurt, kurt, kurt);
            }

            return Color.Black;
        }

    }

    public enum Stat
    {
        Mean,
        Var,
        Skew,
        Kurt,
    }
}
