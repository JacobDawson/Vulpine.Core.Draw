using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Matrices;

namespace Vulpine.Core.Draw.Filters
{
    public class FilterKernal : Filter
    {
        #region Class Definitions...

        //stores the convolution kernal as a matrix
        private Matrix kernal;

        //keeps a seperate refrence to the size
        private int size;


        protected FilterKernal(Matrix kernal)
        {
            this.kernal = kernal;
            this.size = kernal.NumRows;
        }

        protected FilterKernal(int size, params double[] values)
        {
            this.kernal = new Matrix(size, size, values);
            this.size = size;
        }

        #endregion /////////////////////////////////////////////////////////////////////////

        #region Class Properties...

        public override int Size
        {
            get { return size; }
        }

        public Matrix GetKernal()
        {
            //returns a copy of the kernal matrix
            return new Matrix(kernal);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////

        #region Filter Implementation...     

        public override Color Sample(Image source, int x, int y)
        {
            //used in computing the offesets
            int half = size / 2;
            int xoff, yoff;

            Vector pix = new Vector(4);
            Vector total = new Vector(4);

            for (int i = 0; i < size; i++)
            {
                //computes the x offset
                xoff = x - (i - half);

                for (int j = 0; j < size; j++)
                {
                    //computes the y offset
                    yoff = y - (j - half);

                    //convolves the pixel and adds it to the total
                    pix = source.GetPixel(xoff, yoff).ToRGBA();
                    total += kernal[j, i] * pix;
                }
            }

            //returns the weighted total
            return Color.FromRGBA(total);
        }

        #endregion /////////////////////////////////////////////////////////////////////////

        


        #region Factory Methods...

        public static FilterKernal BoxBlur(int size)
        {
            //makes certain that the size is valid
            size = CorrectSize(size);

            //used in creating the kernal
            int total = size * size;
            double norm = 1.0 / (double)total;
            double[] kernal = new double[total];

            //fills every cell with the same value
            for (int i = 0; i < total; i++)
            {
                kernal[i] = norm;
            }

            //returns the constructed filter
            return new FilterKernal(size, kernal); 
        }

        public static FilterKernal GausianBlur(int size)
        {
            //makes certain that the size is valid
            size = CorrectSize(size);

            //calls upon the method below with ideal sd
            double sd = (double)size / 6.0;
            return GausianBlur(size, sd);
        }

        public static FilterKernal GausianBlur(int size, double sd)
        {
            //makes certain that the size is valid
            size = CorrectSize(size);

            //creates a matrix to store our kernal
            Matrix kernal = new Matrix(size, size);
            int cut = size / 2;
            
            //used in computing the gausian funciton
            double ssd = 2.0 * sd * sd;
            double div = Math.PI * sd;
            double total = 0.0;

            for (int i = -cut; i <= cut; i++)
            {
                for (int j = -cut; j <= cut; j++)
                {
                    //computes the gausian function at (i, j)
                    double gause = (i * i) + (j * j);
                    gause = Math.Exp(-gause / ssd);
                    gause = gause / div;

                    //fills in the cell and updates the total
                    kernal[i + cut, j + cut] = gause;
                    total += gause;
                }
            }

            //normalizes the matrix and returns
            kernal = kernal / total;
            return new FilterKernal(kernal);
        }

        #endregion /////////////////////////////////////////////////////////////////////////

        #region Prebuilt Kernals...

 
        public static FilterKernal Sharpen
        {
            get
            {
                return new FilterKernal(3,
                    +0, -1, +0,
                    -1, +5, -1,
                    +0, -1, +0);
            }
        }

        public static FilterKernal Emboss
        {
            get
            {
                return new FilterKernal(3,
                    -2, -1, +0,
                    -1, +1, +1,
                    +0, +1, +2);
            }
        }

        public static FilterKernal LineVert
        {
            get
            {
                return new FilterKernal(3,
                    -1, +2, -1,
                    -1, +2, -1,
                    -1, +2, -1);
            }
        }

        public static FilterKernal LineHorz
        {
            get
            {
                return new FilterKernal(3,
                    -1, -1, -1,
                    +2, +2, +2,
                    -1, -1, -1);
            }
        }

        public static FilterKernal LineDiag1
        {
            get
            {
                return new FilterKernal(3,
                    -1, -1, +2,
                    -1, +2, -1,
                    +2, -1, -1);
            }
        }

        public static FilterKernal LineDiag2
        {
            get
            {
                return new FilterKernal(3,
                    +2, -1, -1,
                    -1, +2, -1,
                    -1, -1, +2);
            }
        }

        public static FilterKernal SobelVert
        {
            get
            {
                return new FilterKernal(3,
                    +1, +0, -1,
                    +2, +0, -2,
                    +1, +0, -1);
            }
        }

        public static FilterKernal SobelHorz
        {
            get
            {
                return new FilterKernal(3,
                    +1, +2, +1,
                    +0, +0, +0,
                    -1, -2, -1);
            }
        }

        public static FilterKernal Laplass
        {
            get
            {
                return new FilterKernal(3,
                    +0, -1, +0,
                    -1, +4, -1,
                    +0, -1, +0);
            }
        }

        public static FilterKernal Outline
        {
            get
            {
                return new FilterKernal(3,
                    -1, -1, -1,
                    -1, +8, -1,
                    -1, -1, -1);
            }
        }

        public static FilterKernal BoxBlur3
        {
            get
            {
                return new FilterKernal(3,
                    1.0 / 9.0, 1.0 / 9.0, 1.0 / 9.0,
                    1.0 / 9.0, 1.0 / 9.0, 1.0 / 9.0,
                    1.0 / 9.0, 1.0 / 9.0, 1.0 / 9.0);
            }
        }

        public static FilterKernal BoxBlur5
        {
            get
            {
                return new FilterKernal(5,
                    0.04, 0.04, 0.04, 0.04, 0.04,
                    0.04, 0.04, 0.04, 0.04, 0.04,
                    0.04, 0.04, 0.04, 0.04, 0.04,
                    0.04, 0.04, 0.04, 0.04, 0.04,
                    0.04, 0.04, 0.04, 0.04, 0.04);
            }
        }

        public static FilterKernal GauseBlur3
        {
            get
            {
                return new FilterKernal(3,
                    0.0625, 0.1250, 0.0625,
                    0.1250, 0.2500, 0.1250,
                    0.0625, 0.1250, 0.0625);
            }
        }

        public static FilterKernal GauseBlur5
        {
            get
            {
                Matrix kernal = new Matrix(5, 5,
                    01.0, 04.0, 06.0, 04.0, 01.0,
                    04.0, 16.0, 24.0, 16.0, 04.0,
                    06.0, 24.0, 36.0, 24.0, 06.0,
                    04.0, 16.0, 24.0, 16.0, 04.0,
                    01.0, 04.0, 06.0, 04.0, 01.0);

                kernal = kernal / 256.0;
                return new FilterKernal(kernal);
            }
        }

        public static FilterKernal Unsharp5
        {
            get
            {
                Matrix kernal = new Matrix(5, 5,
                    01.0, 04.0, 06.0, 04.0, 01.0,
                    04.0, 16.0, 24.0, 16.0, 04.0,
                    06.0, 24.0, -476.0, 24.0, 06.0,
                    04.0, 16.0, 24.0, 16.0, 04.0,
                    01.0, 04.0, 06.0, 04.0, 01.0);

                kernal = kernal / -256.0;
                return new FilterKernal(kernal);
            }
        }

        #endregion /////////////////////////////////////////////////////////////////////////

        #region Helper Methods...

        private static int CorrectSize(int size)
        {
            if (size < 3) return 3;
            else if (size % 2 == 0) return size + 1;
            else return size;
        }

        #endregion /////////////////////////////////////////////////////////////////////////


    }
}
