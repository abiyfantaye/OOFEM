using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using FEM;
using FEM.Mathematics;

namespace FEM
{
    /// <summary>
    /// Class which contains boundary condition data. 
    /// </summary>
    public abstract class eBC
    {
        /// <summary>
        /// Holds the value indicating on which DOF type the BC is applied.
        /// </summary>
        protected int dofIndex;
        /// <summary>
        /// Holds the coordinate of the boundary condition.
        /// </summary>
        protected ePoint[] points;
        /// <summary>
        /// Holds the nodes on which the boundary condition is applied.
        /// </summary>
        protected List<eNode> nodes;
        /// <summary>
        /// Holds the value of the boundary condition to be applied at each DOF.
        /// </summary>
        protected double [] nodalVals;
        /// <summary>
        /// Holds the value for the Type of boundary condition.
        /// </summary>
        protected eBCType bcType;
        /// <summary>
        /// Holds the name for the boundary condition.
        /// </summary>
        private string name;
        /// <summary>
        /// Holds the function in the boundary of the domain.
        /// </summary>
        protected eFunction func;

        /// <summary>
        /// Creates new Boundary Condition using the given parameters.
        /// </summary>
        /// <param name="DOFIndex">Index of degree of freedom in the node.</param>
        /// <param name="Points">Points specifying the domain of BC.</param>
        /// <param name="BCType">Type of boundary condition.</param>
        public eBC( ePoint[] Points, int DOFIndex, eBCType BCType, eFunction Func)
        {
            this.dofIndex = DOFIndex;
            this.points = Points;
            this.bcType = BCType;
            this.name = "";
            this.func = Func;
            this.nodes = new List<eNode>();
        }

        /// <summary>
        /// Gets or sets the name of the boundary condition.
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
        /// Gets or sets the nodes on which this boundary condition is applied.
        /// </summary>
        public List<eNode> Nodes
        {
            get
            {
                return nodes;
            }

            set
            {
                nodes = value;
            }
        }

        /// <summary>
        /// Gets or set the index of degree of freedom in the node.
        /// </summary>
        public int DOFIndex
        {
            get
            {
                return dofIndex;
            }
        }

        /// <summary>
        /// Gets the points which the BC path pass.
        /// </summary>
        public ePoint[] Points
        {
            get
            {
                return points;
            }
        }

        /// <summary>
        /// Gets the boundary value vector to be applied at each DOF in the domain of BC.
        /// </summary>
        public double [] NodalValues
        {
            get
            {
                return nodalVals;
            }
        }

        /// <summary>
        /// Gets or sets the type boundary condition.
        /// </summary>
        public eBCType BCType
        {
            get
            {
                return bcType;
            }

            set
            {
                bcType = value;
            }
        }

        /// <summary>
        /// Gets or sets the function which returns the value of the boundary condition in the domain.
        /// </summary>
        public eFunction Function
        {
            get
            {
                return func;
            }
            set
            {
                func = value;
            }
        }

        /// <summary>
        /// Checks if the boundary condition contain the given point.
        /// </summary>
        /// <param name="p">The point to be checked.</param>
        /// <returns>Returns true if the point is found on the boundary condition geometry and false if not.</returns>
        public abstract bool Contain(ePoint p);

        /// <summary>
        /// Calculates the value of each boundary condition to be applied in DOFs at the nodal points.
        /// </summary>
        public abstract void CalculateNodalValues();

        /// <summary>
        /// Returns the degree of freedom on which this boundary condition is applied.
        /// </summary>
        /// <param name="index">Index of the node contained by the BC.</param>
        /// <returns>Returns the DOFs index.</returns>
        public int GetDOF(int index)
        {
            return nodes[index].DOFs[dofIndex];
        }

    }
}
