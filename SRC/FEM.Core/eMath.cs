using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEM
{
    /// <summary>
    /// Contains static mathematical methods which are used in FEM.
    /// </summary>
    public static class eMath
    {
        /// <summary>
        /// Returns the length of the line connecting the two points. 
        /// </summary>
        /// <param name="p1">The first point.</param>
        /// <param name="p2">The second point.</param>
        /// <returns></returns>
        public static double GetLength(ePoint p1, ePoint p2)
        {
            
            return Math.Sqrt(Math.Pow((p2.X - p1.X), 2) + Math.Pow((p2.Y - p1.Y), 2) + Math.Pow((p2.Z - p1.Z), 2));
        }

        /// <summary>
        /// Returns true if the two number are equal upto 6 decimal place and false otherwise.
        /// </summary>
        /// <param name="n1">The first number to be compared.</param>
        /// <param name="n2">The second number to be compared.</param>
        /// <returns></returns>
        public static bool AreEqual(double n1, double n2)
        {

            return Math.Round(n1, 6) == Math.Round(n2, 6);
        }

        public static bool IsGreate(double n1, double n2)
        {
            return Math.Round(n1, 6) > Math.Round(n2, 6);
        }
        public static bool IsLess(double n1, double n2)
        {
            return Math.Round(n1, 6) <= Math.Round(n2, 6);
        }
    }
}
