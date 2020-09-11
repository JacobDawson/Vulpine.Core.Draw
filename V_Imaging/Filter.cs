using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulpine.Core.Draw
{
    public abstract class Filter
    {

        public abstract int Size { get; }

        public abstract Color Sample(Image source, int x, int y);


        public void Apply(Image source, Image target)
        {
            //makes certain that source is not the same as target
            bool same = Object.ReferenceEquals(source, target);
            if (same) throw new ArgumentException(
                "Source and target reffer to the same object!");

            //computes the intersection of both images
            int w = Math.Min(source.Width, target.Width);
            int h = Math.Min(source.Height, target.Height);

            //fills the image with new data
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    Color c = this.Sample(source, j, i);
                    target.SetPixel(j, i, c);
                }
            }
        }
    }
}
