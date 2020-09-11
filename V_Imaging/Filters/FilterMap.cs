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
        
    }
}
