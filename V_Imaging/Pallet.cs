using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vulpine.Core.Data;
using Vulpine.Core.Data.Lists;

using Vulpine.Core.Calc;
using Vulpine.Core.Calc.Matrices;
using Vulpine.Core.Calc.Data;

namespace Vulpine.Core.Draw
{
    public class Pallet : IEnumerable<Color>
    {
        #region Class Deffinitions...

        //determins the space used to match colors
        private ColorSpace format;

        //stores the collors of the pallet in a list
        private VList<Color> collors; 

        //uses a KD-Tree to find the indicies in the pallet
        private TreeVector<Int32> pallet;

        /// <summary>
        /// Creates an empty pallet, using the RGB color space.
        /// </summary>
        public Pallet()
        {
            this.format = ColorSpace.RGB;
            this.collors = new VListArray<Color>(16);
            this.pallet = new TreeKD<Int32>(3);
        }

        /// <summary>
        /// Creates an empty pallet, using the desired collor space
        /// </summary>
        /// <param name="space">Collor space for the pallet</param>
        public Pallet(ColorSpace space)
        {
            this.format = space;
            this.collors = new VListArray<Color>(16);
            this.pallet = new TreeKD<Int32>(3);
        }

        /// <summary>
        /// Creates a pallet using the given array of collors. The
        /// color space for the pallet defaults to RGB.
        /// </summary>
        /// <param name="colors">The colors of the pallet</param>
        public Pallet(params Color[] colors)
        {
            //determins the number of collors used
            int count = colors.Length;

            //sets up the data structurs
            this.format = ColorSpace.RGB;
            this.collors = new VListArray<Color>(count);
            this.pallet = new TreeKD<Int32>(3);

            //adds each of the collors to the pallet
            for (int i = 0; i < count; i++)
                AddColor(colors[i]);

            //pre-builds the search tree
            pallet.Build();
        }

        /// <summary>
        /// Creates a pallet using the given array of collors and
        /// the given color space.
        /// </summary>
        /// <param name="space">Color space for the pallet</param>
        /// <param name="colors">The colors of the pallet</param>
        public Pallet(ColorSpace space, params Color[] colors)
        {
            //determins the number of collors used
            int count = colors.Length;

            //sets up the data structurs
            this.format = space;
            this.collors = new VListArray<Color>(count);
            this.pallet = new TreeKD<Int32>(3);

            //adds each of the collors to the pallet
            for (int i = 0; i < count; i++)
                AddColor(colors[i]);

            //pre-builds the search tree
            pallet.Build();
        }

        #endregion /////////////////////////////////////////////////////////////////

        #region Class Properties...

        /// <summary>
        /// Determins the number of colors that make up the pallet.
        /// </summary>
        public int NumColors
        {
            get { return collors.Count; }
        }

        /// <summary>
        /// Determins the color space used internaly by the pallet.
        /// </summary>
        public ColorSpace SearchSpace
        {
            get { return format; }
        }

        #endregion /////////////////////////////////////////////////////////////////

        #region Pallet Operations...

        /// <summary>
        /// Adds a color to the existing pallet
        /// </summary>
        /// <param name="c">Color to add</param>
        public void AddColor(Color c)
        {
            //converts the color to the local color space
            Vector rep = c.ToVector(format);
            int index = collors.Count;

            //adds the collor to the pallet
            collors.Insert(index, c);
            pallet.Add(rep, index);
        }

        /// <summary>
        /// Obtains the desired color from the pallet, based on it's index.
        /// Collors outside the range of the pallet are rendered as transparent.
        /// </summary>
        /// <param name="index">Index of the color</param>
        /// <returns>The desired color</returns>
        public Color GetColor(int index)
        {
            //if we are outside the range, return transparent
            if (index < 0 || index > collors.Count)
                return new Color(0.0, 0.0, 0.0, 0.0);

            //simply queerys the list of collors
            return collors.GetItem(index);
        }

        /// <summary>
        /// Obtains the closest matching color in the pallet to the given
        /// target color. That is, how the target color should look when
        /// applied to this pallet.
        /// </summary>
        /// <param name="target">Color to match</param>
        /// <returns>Closest matching color</returns>
        public Color GetClosest(Color target)
        {
            //uses the method below to retrieve the index
            int index = GetIndex(target);
            return collors.GetItem(index);
        }

        /// <summary>
        /// Obtains the index of the closest matching color to the given
        /// target color. The index represents a paticular color in the pallet.
        /// </summary>
        /// <param name="target">Color to match</param>
        /// <returns>Index of the closest matching color</returns>
        public int GetIndex(Color target)
        {
            //rebuilds the pallet, if nessary
            if (pallet.BuildRequired) pallet.Build();

            //obtains the index of the nearest matching color
            Vector probe = target.ToVector(format);
            var output = pallet.GetNearest(probe);
            return output.Value;
        }

        /// <summary>
        /// Lists all of the colors that make up this pallet.
        /// </summary>
        /// <returns>All the colors</returns>
        public IEnumerator<Color> GetEnumerator()
        {
            //simply defers to the list of collors
            return collors.GetEnumerator();
        }

        #endregion /////////////////////////////////////////////////////////////////

        IEnumerator IEnumerable.GetEnumerator()
        { return collors.GetEnumerator(); }

    }
}
