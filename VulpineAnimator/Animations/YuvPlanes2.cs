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


    public class YuvPlanes2 : Animation
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
            //double u1 = u * 0.5;
            //double v1 = v * 0.5;

            double u1 = (frame / 180.0) - 0.5;
            double v1 = u * 0.5;
            double y = (v / 2.0) + 0.5;

            return FromYUV(y, u1, v1);
        }

        public Texture GetFrame(int frame)
        {
            return new Frame(this, frame);

            //throw new NotImplementedException();
        }


        public static Color FromYUV(double luma, double u, double v)
        {
            //clamps the values to be within there expected ranges
            luma = VMath.Clamp(luma);
            u = VMath.Clamp(u, -0.5, 0.5);
            v = VMath.Clamp(v, -0.5, 0.5);

            //first computes the red and blue channels
            double red = luma + v * IWR;
            double blue = luma + u * IWB;

            //the computes the green channel
            double green = luma - v * ((WR * IWR) / WG);
            green = green - u * ((WB * IWB) / WG);

            bool fail = false;

            fail |= (red < 0.0 || red > 1.0);
            fail |= (green < 0.0 || green > 1.0);
            fail |= (blue < 0.0 || blue > 1.0);

            if (fail) return Color.FromRGB(0.5, 0.5, 0.5);
                
            return Color.FromRGB(red, green, blue);
        }
    }
}
