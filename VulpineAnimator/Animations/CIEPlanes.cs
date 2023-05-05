using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Draw;
using Vulpine.Core.Draw.Textures;
using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Numbers;
using Vulpine.Core.Calc.Algorithms;

namespace VulpineAnimator.Animations
{


    public class CIEPlanes : Animation
    {
        //weights that can be used for caluclating luma
        private const double WR = 0.299;
        private const double WG = 0.587;
        private const double WB = 0.114;

        //weights used in converting from YUV
        private const double IWR = 1.402;
        private const double IWB = 1.772;

        //6s animation (180 frames @ 30 fps)

        public Color Sample(double u, double v, int frame)
        {
            //double y = frame / 180.0;
            //double u1 = (u * 0.5) + 0.5;
            //double v1 = (v * 0.5) + 0.5;

            ////return FromXYZ(u1, y, v1);
            //return FromXYY(u1, v1, y);

            double y = (frame / 180.0) * 100.0;
            double u1 = (u * 200.0);
            double v1 = (v * 200.0);

            return FromLAB(y, u1, v1);
            //return FromLUV(y, u1, v1);
        }

        public Texture GetFrame(int frame)
        {
            return new Frame(this, frame);

            //throw new NotImplementedException();
        }

        public static Color Rectify(double r, double g, double b)
        {
            //double min = Math.Min(r, g);
            //min = Math.Min(min, b);

            //if (min < 0.0)
            //{
            //    r = r - min;
            //    g = g - min;
            //    b = b - min;
            //}

            r = Math.Max(r, 0.0);
            g = Math.Max(g, 0.0);
            b = Math.Max(b, 0.0);

            double max = Math.Max(r, g);
            max = Math.Max(max, b);

            if (max > 1.0)
            {
                r = r / max;
                g = g / max;
                b = b / max;
            }

            return Color.FromRGB(r, g, b);

        }


        public static Color FromXYZ(double x, double y, double z)
        {
            //converts from XYZ to liniar RGB
            double r = MX[0] * x + MX[1] * y + MX[2] * z;
            double g = MX[3] * x + MX[4] * y + MX[5] * z;
            double b = MX[6] * x + MX[7] * y + MX[8] * z;

            //applies a gamma curve to the liniar RGB
            r = InvGamma(r);
            g = InvGamma(g);
            b = InvGamma(b);

            //bool fail = false;

            //fail |= (r < 0.0 || r > 1.0);
            //fail |= (g < 0.0 || g > 1.0);
            //fail |= (b < 0.0 || b > 1.0);

            //if (fail) return Color.FromRGB(0.5, 0.5, 0.5);

            ////returnes the scaled RGB values
            //return Color.FromRGB(r, g, b);

            return Rectify(r, g, b);
        }

        public static Color FromXYY(double x0, double y0, double y1)
        {
            double yn = y1 / y0;
            double x1 = yn * x0;
            double z1 = yn * (1.0 - x0 - y0);

            return FromXYZ(x1, y1, z1);
        }

        public static Color FromLAB(double lum, double a, double b)
        {
            //used in further calculations
            double t = (lum + 16.0) / 116.0;

            //computes the CIE XYZ cordinates
            double x = 0.95047 * LabInvF(t + (a / 500.0));
            double y = 1.0 * LabInvF(t);
            double z = 1.08883 * LabInvF(t - (b / 200.0));

            //continues the conversion to RGB
            return FromXYZ(x, y, z);
        }

        public static Color FromLUV(double lum, double u, double v)
        {
            double up = (u / (13.0 * lum)) + 0.1978398248;
            double vp = (v / (13.0 * lum)) + 0.4683363029;

            double y = (lum > 8.0) ? (lum + 16.0) / 116.0 : lum;
            y = (lum > 8.0) ? y * y * y : y * 0.001107056460;

            double x = y * ((9.0 * up) / (4.0 * vp));
            double z = y * ((12.0 - (3.0 * up) - (20.0 * vp)) / (4.0 * vp));

            return FromXYZ(x, y, z);
        }

        private static double Gamma(double u)
        {
            if (u < 0.04045)
            {
                //uses a liniar function near zero
                return u / 12.92;
            }
            else
            {
                //uses the augmented gamma adjustment
                double temp = (u + 0.055) / 1.055;
                return Math.Pow(temp, 2.4);
            }
        }

        private static double InvGamma(double u)
        {
            if (u < 0.003130807283) return 12.92 * u;
            else return 1.055 * Math.Pow(u, 1.0 / 2.4) - 0.055;
        }


        private static double LabF(double t)
        {
            if (t > 0.008856451679) return Math.Pow(t, 1.0 / 3.0);
            else return t * 0.1284185493 + 0.1379310345;
        }

        private static double LabInvF(double t)
        {
            if (t > 0.2068965517) return t * t * t;
            else return 0.1284185493 * (t - 0.1379310344);
        }


        //stores the matrix values for conversion from XYZ space
        private static readonly double[] MX =
        {
             3.2406,
            -1.5372,
            -0.4986,
            -0.9689,
             1.8758,
             0.0415,
             0.0557,
            -0.2040,
             1.0570,
        };

        //stores the matrix values for conversion to XYZ space
        private static readonly double[] IMX =
        {
            0.4123955890,
            0.3575834308,
            0.1804926474,
            0.2125862308,
            0.7151703037,
            0.0722004986,
            0.0192972155,
            0.1191838646,
            0.9504971251,
        };
    }
}
