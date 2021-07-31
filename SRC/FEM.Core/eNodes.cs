using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using ESADS.EGraphics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace FEM
{
    /// <summary>
    /// Collection class which hold all the nodes in the model. 
    /// </summary>
    public class eNodes
    {

        /// <summary>
        /// Holds list of all nodes.
        /// </summary>
        private List<eNode> nodes;

        /// <summary>
        /// Creates new instance of collection class which contain all nodes in the model..
        /// </summary>
        public eNodes()
        {
            this.nodes = new List<eNode>();
        }

        /// <summary>
        /// Accesses the node in the collection by its given index. 
        /// </summary>
        /// <param name="index">Index of the node.</param>
        /// <returns></returns>
        public eNode this[int index]
        {
            get
            {
                return nodes[index];
            }
            set
            {
                nodes[index] = value;
            }
        }

        /// <summary>
        /// Gets the last element in the collection.
        /// </summary>
        public eNode Last
        {
            get
            {
                return nodes[Count - 1];
            }
        }

        /// <summary>
        /// Gets the first element in the collection.
        /// </summary>
        public eNode First
        {
            get
            {
                return nodes[0];
            }
        }

        /// <summary>
        /// Adds new node goven its coordinate. 
        /// </summary>
        /// <param name="Point">The coordinate of the point.</param>
        /// <returns></returns>
        public eNode AddNode(ePoint Point, int DOFsPerNode)
        {
            eNode n;

            if (!Contain(Point, out n))
            {
                n = new eNode(Point, DOFsPerNode);
                n.Name = (Count + 1).ToString();
                nodes.Add(n);
            }

            return n;
        }

        /// <summary>
        /// Gives global system index to all degree of freedoms in all nodes. 
        /// </summary>
        public void SetSysIndexToDOFs()
        {
            int count = 0;
            foreach (eNode n in nodes)
                n.DOFs.SetSysIndex(ref count);
        }

        /// <summary>
        /// Returns the node at ther required point. 
        /// Exception:
        /// Throws exception if the node is not find.
        /// </summary>
        /// <param name="p">The coordinate where the node found.</param>
        /// <returns></returns>
        public eNode FindNode(ePoint p)
        {
            foreach (eNode n in nodes)
            {
                if (n.Coord == p)
                    return n;
            }

            throw new Exception("The node is not found!");
        }

        /// <summary>
        /// Returns the node at ther required point. 
        /// Exception:
        /// Throws exception if the node is not find.
        /// </summary>
        /// <param name="X">The X-coordinate where the node is found.</param>
        /// <returns></returns>
        public eNode FindNode(double X)
        {
            foreach (eNode n in nodes)
            {
                if (n.Coord.X == X)
                    return n;
            }

            throw new Exception("The node is not found!");
        }

        /// <summary>
        /// Returns the node at ther required point. 
        /// Exception:
        /// Throws exception if the node is not find.
        /// </summary>
        /// <param name="X">The X-coordinate where the node is found.</param>
        /// <param name="Y">Y-coordinate of where the node is found.</param>
        /// <returns></returns>
        public eNode FindNode(double X, double Y)
        {
            foreach (eNode n in nodes)
            {
                if (eMath.AreEqual(n.Coord.X, X) && eMath.AreEqual(n.Coord.Y, Y))
                    return n;
            }

            throw new Exception("The node is not found!");
        }

        /// <summary>
        /// Returns the node at ther required point. 
        /// Exception:
        /// Throws exception if the node is not find.
        /// </summary>
        /// <param name="X">The X-coordinate where the node is found.</param>
        /// <param name="Y">Y-coordinate of where the node is found.</param>
        /// <returns></returns>
        public eNode FindNode(double X, double Y,double Z)
        {
            foreach (eNode n in nodes)
            {
                if (eMath.AreEqual(n.Coord.X, X) && eMath.AreEqual(n.Coord.Y, Y) && eMath.AreEqual(n.Coord.Z, Z))
                    return n;
            }

            throw new Exception("The node is not found!");
        }

        /// <summary>
        /// Gets the total number of nodes in the collection.
        /// </summary>
        public int Count
        {
            get
            {
                return nodes.Count;
            }
        }

        /// <summary>
        /// Returns the totla number of degree freedom in the model.
        /// </summary>
        /// <returns></returns>
        public int GetTotalDOFs()
        {
            int totDof = 0;

            foreach (eNode n in nodes)
                totDof += n.DOFs.Count;
            return totDof;
        }

        /// <summary>
        /// Returns the X-Coordinate of all the nodes in array form.
        /// </summary>
        /// <returns>Returns double array of x-coordinates.</returns>
        public double[] GetNodalXValues()
        {
            List<double> coords = new List<double>();
            foreach (eNode n in nodes)
                coords.Add(n.Coord.X);
            return coords.ToArray();
        }

        /// <summary>
        /// Returns the Y-Coordinate of all the nodes in array form.
        /// </summary>
        /// <returns>Returns double array of Y-coordinates.</returns>
        public double[] GetNodalYValues()
        {
            List<double> coords = new List<double>();
            foreach (eNode n in nodes)
                coords.Add(n.Coord.Y);
            return coords.ToArray();
        }



        /// <summary>
        /// Draws all the nodes in the the collection in the given layer.
        /// </summary>
        /// <param name="layer">The layer on which the drawing is done.</param>
        /// <param name="size">The radius of of the circle which is going to be drawn to represent node.</param>
        public void Draw(eLayer layer,float size)
        {
            foreach (eNode n in nodes)
                n.Draw(layer, size);
        }

        /// <summary>
        /// Removes all nodes in the collection. 
        /// </summary>
        public void Reset()
        {
            this.nodes = new List<eNode>();
        }

        /// <summary>
        /// Checks if there is node or no node at the specified point.
        /// </summary>
        /// <param name="p">Point to be checked for existance of node.</param>
        /// <param name="Node">Assing the node if its found.</param>
        /// <returns>Returns true if there is node and alse otherwise.</returns>
        public bool Contain(ePoint p, out eNode Node)
        {
            foreach (eNode n in nodes)
            {
                if (p == n.Coord)
                {
                    Node = n;
                    return true;
                }
            }
            Node = null;
            return false;
        }

        public void FillNodalValues(Vector<double> nodalResult)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Values = new DenseVector(nodes[i].DOFs.Count);

                for (int j = 0; j < nodes[i].Values.Count; j++)
                {
                    nodes[i].Values[j] = nodalResult[nodes[i].DOFs[j]];
                }
            }
        }
    }
}
