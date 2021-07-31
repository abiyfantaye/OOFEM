using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using ESADS.EGraphics;
using FEM.Mathematics;

namespace FEM
{
    /// <summary>
    /// Defines a base class for all Element types in FEM.
    /// </summary>
    public abstract class eElement 
    {
        #region Fields 

       
        /// <summary>
        /// Holds displacement vector for the Element.
        /// </summary>
        protected Vector<double> fe;
        /// <summary>
        /// Holds the lable of the Element.
        /// </summary>
        protected string name;
        /// <summary>
        /// Cantailns all Nodes connected to the Element.
        /// </summary>
        protected eNode [] nodes;
        /// <summary>
        /// Contains the matrix of the Element.
        /// </summary>
        protected Matrix<double> ke;
        /// <summary>
        /// Holds the nodal value of field parameter from all nodes connected to this elelement. 
        /// </summary>
        protected Vector<double> nodalResults;
        /// <summary>
        /// Holds shape function for the element.
        /// </summary>
        protected ePolynomial[] shapeFuncs;
        /// <summary>
        /// Holds the coordinates of the nodes in the element.
        /// </summary>
        protected ePoint[] points;
        /// <summary>
        /// Holds the mass matrix for transient problems.
        /// </summary>
        protected Matrix<double> me;
        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of Element class from given parameters.
        /// </summary>
        /// <param name="connNodes">Nodes connecting to the element.</param>
        public eElement( eNode [] connNodes)
        {
            this.name = "";
            this.nodes = connNodes;
            this.points = GetNodalPoints();
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets the mass matrix for the element.
        /// </summary>
        public Matrix<double> MassMatrix
        {
            get
            {
                return me;
            }
        }

        /// <summary>
        /// Gets the total number of degree of freedom in the element.
        /// </summary>
        public int NoDOFs
        {
            get
            {
               return GetDofPerElement();
            }
        }

        /// <summary>
        /// Gets or sets the name or the label of the Element.
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
        /// Gets or sests all Nodes connected to the Element.
        /// </summary>
        public eNode [] Nodes
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
        /// Gets or sets the displacement vector for the Element.
        /// </summary>
        public Vector<double> ElVctr
        {
            get
            {
                return fe;
            }
        }

        /// <summary>
        /// Gets the Element matrix.
        /// </summary>
        public Matrix<double> ElMtx
        {
            get
            {
                return ke;
            }
        }

        /// <summary>
        /// Gets the coordinates of all nodes connected to the element.
        /// </summary>
        public ePoint[] Points
        {
            get
            {
                return points;
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Generates the Element matrix.
        /// </summary>
        public abstract void FormElementMatrix();

        /// <summary>
        /// Generates element Vector.
        /// </summary>
        public abstract void FormElementVector();

        public abstract void FormMassMatrix();

        /// <summary>
        /// Returns system(global) matrix index for the element matrix intries.
        /// </summary>
        /// <returns></returns>
        public int[] GetSysIndex()
        {

            List<int> res = new List<int>();
            foreach (eNode n in nodes)
                res.AddRange(n.DOFs.Indexes);

            return (int[])res.ToArray();
        }

        /// <summary>
        /// Returns the number of DOF per node.
        /// </summary>
        /// <returns></returns>
        protected int GetDofPerElement()
        {
            return nodes[0].DOFsPerNode * nodes.Length;
        }

        /// <summary>
        /// Returns the Coordinate array of nodal points.
        /// </summary>
        /// <returns></returns>
        public ePoint[] GetNodalPoints()
        {
            ePoint[] pts = new ePoint[nodes.Length];
            for (int i = 0; i < pts.Length; i++)
            {
                pts[i] = nodes[i].Coord;
            }

            return pts;
        }

        /// <summary>
        /// Assembels the Element matrix and vector to the global main matrix and vector.
        /// </summary>
        /// <param name="K">The global main matrix.</param>
        /// <param name="F">The global main vector.</param>
        /// <param name="M">The global mass matrix.</param>
        public void Assembele(Matrix<double> K, Vector<double> F, Matrix<double> M = null)
        {
            int[] indexes = GetSysIndex();
        
            if (M == null)
            {
                for (int i = 0; i < ke.RowCount; i++)
                {
                    F[indexes[i]] += fe[i];
                    for (int j = 0; j < ke.ColumnCount; j++)
                    {
                        K[indexes[i], indexes[j]] += ke[i, j];
                    }
                }
            }
            else
            {
                for (int i = 0; i < ke.RowCount; i++)
                {
                    F[indexes[i]] += fe[i];
                    for (int j = 0; j < ke.ColumnCount; j++)
                    {
                        K[indexes[i], indexes[j]] += ke[i, j];
                        M[indexes[i], indexes[j]] += me[i, j];
                    }
                }
            }
        }

        public abstract void Draw(eLayer layer);
        #endregion      
    }
}
