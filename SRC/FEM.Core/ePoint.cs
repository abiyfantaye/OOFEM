using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FEM
{
    /// <summary>
    /// Represents coordinate of a point in space. 
    /// </summary>
    public class ePoint
    {
        /// <summary>
        /// Holds  X-coordinate of the point.
        /// </summary>
        private double x;
        /// <summary>
        /// Holds  y-coordinate of the point.
        /// </summary>
        private double y;
        /// <summary>
        /// Holds  z-coordinate of the point.
        /// </summary>
        private double z;

        /// <summary>
        /// Creates new point given X and y cooordinate of the point.
        /// </summary>
        /// <param name="X">X-coordinate of the point.</param>
        /// <param name="Y">Y-coordinate of the point.</param>
        public ePoint(double X, double Y)
        {
            this.x = X;
            this.y = Y;
        }

        /// <summary>
        /// Creates new point given X, Y and Z coordinates of the point.
        /// </summary>
        /// <param name="X">X-coordinate of the point.</param>
        /// <param name="Y">Y-coordinate of the point.</param>
        /// <param name="Z">Z-coordinate of the point.</param>
        public ePoint(double X, double Y, double Z)
        {
            this.x = X;
            this.y = Y;
            this.z = Z;
        }

        /// <summary>
        /// Creates new point with x = y = z = 0
        /// </summary>
        public ePoint()
        {
        }

        /// <summary>
        /// Creates new point given X coordinate of the point. This constractor is used for one dimensional proplemes.
        /// </summary>
        /// <param name="X">X-coordinate of the point.</param>
        public ePoint(double X)
        {
            this.x = X;
            this.y = 0;
            this.z = 0;
        }
        /// <summary>
        /// Gets or sets  X-coordinate of the point.
        /// </summary>
        public double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        /// <summary>
        /// Gets or sets  y-coordinate of the point.
        /// </summary>
        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        /// <summary>
        /// Gets or sets  z-coordinate of the point.
        /// </summary>
        public double Z
        {
            get
            {
                return z;
            }
            set
            {
                z = value;
            }
        }

        /// <summary>
        /// Returns the distance of the point from the origin.
        /// </summary>
        public void GetDistance()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Compares if two points are equal or not.
        /// </summary>
        /// <param name="p1">First point.</param>
        /// <param name="p2">Second point.</param>
        /// <returns>Returns true if the points have the same x,y and z coordinate.</returns>
        public static bool operator ==(ePoint p1, ePoint p2)
        {
                return eMath.AreEqual(p1.X, p2.X) && eMath.AreEqual(p1.Y, p2.Y) && eMath.AreEqual(p1.Z, p2.Z);
        }

        /// <summary>
        /// Compares if two points are equal or not.
        /// </summary>
        /// <param name="p1">First point.</param>
        /// <param name="p2">Second point.</param>
        /// <returns>Returns true if the points have different x,y and z coordinate.</returns>
        public static bool operator !=(ePoint p1, ePoint p2)
        {
            return !(eMath.AreEqual(p1.X, p2.X) && eMath.AreEqual(p1.Y, p2.Y) && eMath.AreEqual(p1.Z, p2.Z));
        }

        /// <summary>
        /// Compare this object and the given object.
        /// </summary>
        /// <param name="obj">Object to be compared.</param>
        /// <returns>Returns true if they have equal properties.</returns>
        public override bool Equals(object obj)
        {
            return ((ePoint)obj) == this;
        }

        /// <summary>
        /// Serves hash function for ePoint type. 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Implicitly cast a point to double array contianing the x,y and z coordinates. 
        /// </summary>
        /// <param name="p">The point to be casted.</param>
        /// <returns>Returns x,y and z coordinate of the point in on array respectively.</returns>
        public static implicit operator double[](ePoint p)
        {
            return new double[] { p.X, p.Y, p.Z };
        }

        /// <summary>
        /// Convertes double array to ePoint.
        /// </summary>
        /// <param name="Coord">The coordinate array containing x, y and z respectively.</param>
        /// <returns></returns>
        public static implicit operator ePoint(double[] Coord)
        {
            return new ePoint(Coord[0], Coord[1], Coord[2]);
        }

        public static implicit operator PointF(ePoint p)
        {
            return new PointF((float)p.X, -(float)p.Y);
        }

        public static implicit operator ePoint(PointF p)
        {
            return new ePoint(p.X,-p.Y);
        }

    }
}
