using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using ESADS.EGraphics;

namespace FEM
{
    /// <summary>
    /// Class which represent node class in FEM.
    /// </summary>
    public class eNode 
    {
        #region Fields
        /// <summary>
        /// Holds the name of the Node.
        /// </summary>
        private string name;
        /// <summary>
        /// Holds the degree of freedom of the Node.
        /// </summary>
        private eDOFs dofs;
        /// <summary>
        /// Holds the coordinate of the node. 
        /// </summary>
        private ePoint coord;
        /// <summary>
        /// Holds a value indicating wether the node is cornner node or not.
        /// </summary>
        public bool isCornerNode;
        /// <summary>
        /// Holds the nodal value of field variable.
        /// </summary>
        private Vector<double> values;
        #endregion

        #region Constructor

        /// <summary>
        /// Creates new Node from the given parameters.
        /// </summary>
        /// <param name="Coord">The point which specifies the coordinate of the node.</param>
        /// <param name="DOFsPerNode">Number of degree of freedom per node.</param>
        public eNode(ePoint Coord, int DOFsPerNode)
        {
            this.name = Name;
            this.coord = Coord;
            this.dofs = new eDOFs(DOFsPerNode);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets wether the node is corner node or not.
        /// </summary>
        public bool IsCornerNode
        {
            get
            {
               return isCornerNode;
            }
            set
            {
                isCornerNode = value;
            }
        }
        /// <summary>
        /// Gets or sets the label of the Node.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        /// <summary>
        /// Gets the coordinate of the node. 
        /// </summary>
        public ePoint Coord
        {
            get
            {
                return coord;
            }
    
        }

        /// <summary>
        /// Gets the degree of freedoms in the node. 
        /// </summary>
        public eDOFs DOFs
        {
            get
            {
                return dofs;
            }
        }

        /// <summary>
        /// Gets or sets the X-Coordinate of the Node.
        /// </summary>
        public double X
        {
            get { return coord.X; }
            set { coord.X = value; }
        }

        /// <summary>
        /// Gets or sets the Y-Coordinate of the Node.
        /// </summary>
        public double Y
        {
            get { return coord.Y; }
            set { coord.Y = value; }
        }

        /// <summary>
        /// Gets or sets the Z-Coordinate of the Node.
        /// </summary>
        public double Z
        {
            get { return coord.Z; }
            set { coord.Z = value; }
        }

        /// <summary>
        /// Gets or sets total number of degree of freedoms in the node. 
        /// </summary>
        public int DOFsPerNode
        {
            get
            {
                return dofs.Count;
            }
        }

        /// <summary>
        /// Gets or sets the nodal values.
        /// </summary>
        public Vector<double> Values
        {
            get
            {
                return values;
            }
            set
            {
                values = value;
            }
        }

        #endregion

        #region Methods

        public void Draw(eLayer layer,float size)
        {
            layer.AddCircle(coord.X, coord.Y, size, eDrawType.Fill);
            layer.AddText(name, coord.X, coord.Y - 3 * size);
        }

        #endregion
    }
}
