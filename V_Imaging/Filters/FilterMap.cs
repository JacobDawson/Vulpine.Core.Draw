using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Matrices;

namespace Vulpine.Core.Draw.Filters
{
    public class FilterMap : Filter
    {
        private FColor map;

        public FilterMap(FColor map)
        {
            this.map = map;
        }

        public override int Size
        {
            get { return 1; }
        }

        public override Color Sample(Image source, int x, int y)
        {
            Color c = source.GetPixel(x, y);
            return map.Invoke(c);
        }

        public static implicit operator FilterMap(FColor map)
        {
            return new FilterMap(map);
        }

        public static FilterMap Threshold(double cut)
        {
            FColor map = x => 
                (x.Luminance > cut) ? Color.White : Color.Black;
            return new FilterMap(map);               
        }

        public static FilterMap Grayscale(Desaturate method)
        {
            FColor map = x => x.GetGrayscale(method);
            return new FilterMap(map);
        }

        public static FilterMap RedChanel
        {
            get
            {
                FColor map = x =>
                    Color.FromRGB(x.Red, 0.0, 0.0);
                return new FilterMap(map);
            }
        }

        public static FilterMap GreenChanel
        {
            get
            {
                FColor map = x =>
                    Color.FromRGB(0.0, x.Green, 0.0);
                return new FilterMap(map);
            }
        }

        public static FilterMap BlueChanel
        {
            get
            {
                FColor map = x =>
                    Color.FromRGB(0.0, 0.0, x.Blue);
                return new FilterMap(map);
            }
        }

        //this one is the best
        public static FilterMap HueBright
        {
            get
            {
                return (FColor) delegate(Color c)
                {
                    Vector hsl = c.ToHSL();
                    hsl[1] = 1.0;
                    return Color.FromHSL(hsl);
                };
            }
        }

        public static FilterMap HueOnly
        {
            get
            {
                return (FColor) delegate(Color c)
                {
                    double hue = c.Hue;
                    return Color.FromHSV(hue, 1.0, 1.0);
                };
            }
        }

        public static FilterMap MaxValue
        {
            get
            {
                return (FColor) delegate(Color c)
                {
                    double hue = c.Hue;
                    double sat = c.Saturation;
                    return Color.FromHSV(hue, sat, 1.0);
                };
            }
        }

        public static FilterMap SwapSatValue
        {
            get
            {
                return (FColor)delegate(Color c)
                {
                    Vector hsv = c.ToHSV();
                    return Color.FromHSV(hsv[0], hsv[2], hsv[1]);
                };
            }
        }

        public static FilterMap MidBrightness
        {
            get
            {
                return (FColor)delegate(Color c)
                {
                    Vector hsl = c.ToHSL();
                    return Color.FromHSL(hsl[0], hsl[1], 0.5);
                };
            }
        }

        public static FilterMap HueValue
        {
            get
            {
                return (FColor)delegate(Color c)
                {
                    double hue = c.Hue;
                    double value = c.Value;
                    return Color.FromHSV(hue, 1.0, value);
                };
            }
        }

        public static FilterMap CyanRed
        {
            get
            {
                return (FColor)delegate(Color c)
                {
                    double cyan = c.Green * 0.83738 + c.Blue * 0.16262;
                    return Color.FromRGB(c.Red, cyan, cyan);
                };
            }
        }

        public static FilterMap YellowBlue
        {
            get
            {
                return (FColor)delegate(Color c)
                {
                    double yellow = c.Red * 0.33747 + c.Green * 0.66253;
                    return Color.FromRGB(yellow, yellow, c.Blue);
                };
            }
        }

        public static FilterMap MagentaGreen
        {
            get
            {
                return (FColor)delegate(Color c)
                {
                    double magenta = c.Red * 0.72397 + c.Blue * 0.27603;
                    return Color.FromRGB(magenta, c.Green, magenta);
                };
            }
        }
        
    }
}
