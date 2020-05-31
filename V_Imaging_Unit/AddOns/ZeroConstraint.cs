﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Vulpine_Core_Draw_Tests.AddOns
{
    public class ZeroConstraint : Constraint
    {
        /// <summary>
        /// Constructs a new zero constraint with the given tollerence.
        /// </summary>
        /// <param name="tollerence">The error tollerence</param>
        public ZeroConstraint()
        {
            this.actual = null;
        }

        /// <summary>
        /// Determins if a given object matches the constraint
        /// </summary>
        /// <param name="actual">Object to match</param>
        /// <returns>True if the object matches the constraint</returns>
        public override bool Matches(object actual)
        {
            this.actual = actual;
            double error;

            if (actual is double)
            {
                double a = (double)actual;
                error = Math.Abs(a);
            }
            else if (actual is float)
            {
                double a = (float)actual;
                error = Math.Abs(a);
            }
            else
            {
                dynamic a = actual;
                double dist = a.Norm();
                error = Math.Abs(dist);
            }

            //return error < tollerence;

            //determins if the number is invertable
            double test = 1.0 / error;
            return Double.IsInfinity(test);
        }

        /// <summary>
        /// Writes a discription of the constraint
        /// </summary>
        /// <param name="writer">Used to discribe the constraint</param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.Write("practaly zero");
        }
    }
}
