using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEM
{
    public class eMesh
    {
        #region Fields
        /// <summary>
        /// Holds the total number of elements in the mesh.
        /// </summary>
        private int noElements;
        /// <summary>
        /// Holds the total number of nodes in the mesh.
        /// </summary>
        private int noNodes;
        /// <summary>
        /// Holds the value indicating completion of the mesh.
        /// </summary>
        private bool meshCompleted;
        /// <summary>
        /// The type of element used for meshing.
        /// </summary>
        private eMeshElementType elementType;
        /// <summary>
        /// Holds the maximum mesh size used.
        /// </summary>
        private double meshSize;
        /// <summary>
        /// Holds the model on which the meshing is done.
        /// </summary>
        private eModel model;
        /// <summary>
        /// Provides the meshing method used.
        /// </summary>
        private eMeshingMethod meshingMethod;
        /// <summary>
        /// Holds a value for degree of freedom per node.
        /// </summary>
        private int dofPerNode;

        private double xSize;
        private double ySize;

        public int nx;
        public int ny;
        #endregion

        #region Constructors 
        /// <summary>
        ///Creates new instance of mesh class.
        /// </summary>
        public eMesh(eModel Model)
        {
            this.model = Model;
            this.noElements = 0;
            this.noNodes = 0;
            this.meshCompleted = false;
            this.meshSize = 0;
            this.dofPerNode = 1;
            this.meshingMethod = eMeshingMethod.DivideEqually;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the totla number of elements used for meshing.
        /// </summary>
        public int NoElements
        {
            get
            {
                return noElements;
            }
            set
            {
                noElements = value;
            }

        }

        /// <summary>
        /// Gets the total number of nodes in the mesh.
        /// </summary>
        public int NoNodes
        {
            get
            {
                return noNodes;
            }
        }

        /// <summary>
        /// Gets wether meshing is completed or not.
        /// </summary>
        public bool MeshCompleted
        {
            get
            {
                return meshCompleted;
            }
        }

        /// <summary>
        /// Gets or sets the element type used to mesh.
        /// </summary>
        public eMeshElementType ElementType
        {
            get
            {
                return elementType;
            }
            set
            {
                elementType = value;
            }
        }

        /// <summary>
        /// Gets or sets the mesh size used to form elements. 
        /// </summary>
        public double MeshSize
        {
            get
            {
                return meshSize;
            }
            set
            {
                meshSize = value;
            }

        }

        /// <summary>
        /// Gets or sets number of degree of freedom per node.
        /// </summary>
        public int DOFsPerNode
        {
            get
            {
                return dofPerNode;
            }
            set
            {
                dofPerNode = value;
            }
        }

        #endregion

        #region Methods

        #region General

        /// <summary>
        /// Meshs the model to which this meshing object is attached.
        /// </summary>
        public void Mesh()
        {
            if (model.GetType() == typeof(e1DModel))
                Mesh1D();
            else if (model.GetType() == typeof(e2DModel))
                Mesh2D();
            else if (model.GetType() == typeof(e3DModel))
                Mesh3D();

            this.meshCompleted = true;
            this.noElements = model.Elements.Count;
            this.noNodes = model.Nodes.Count;
        }

        #endregion

        #region 1DMesh

        /// <summary>
        /// Meshs one dimensional medels, with equal element size.
        /// </summary>
        /// <param name="Model">Model to be beshed.</param>
        public void Mesh1D()
        {
            if (elementType == eMeshElementType.LinearLine)
                MeshByLinearLineElement();
            else if (elementType == eMeshElementType.QuadraticLine)
                MeshByQuadraticLineElement();
            else if (elementType == eMeshElementType.CubicLine)
                MeshByCubicLineElement();
        }  
        
        /// <summary>
        /// Creates Mesh using Linear Line Element.
        /// </summary>
        private void MeshByLinearLineElement()
        {
            e1DModel m = (e1DModel)model;

            double X = m.Domain[0].X;
            double r = 0;//Represents the remaining width after devideing equally.

            if (meshingMethod == eMeshingMethod.DivideEqually)
                meshSize = (m.Domain[1].X - m.Domain[0].X) / noElements;

            else if (meshingMethod == eMeshingMethod.UseGivenSize)
            {
                r = (m.Domain[1].X - m.Domain[0].X) / meshSize;
                noElements = (int)((m.Domain[1].X - m.Domain[0].X) / meshSize);
                r = (r - noElements) * meshSize;
            }

            if (meshSize == 0)
                throw new Exception("Element Size cannot be zero");

            m.AddLinearLineElement(X, X + meshSize);
            X += meshSize;

            while (m.Elements.Count != noElements)
            {
                X += meshSize;
                m.AddLinearLineElement(X);
            }

            if (Math.Round(r, 12) != 0)
            {
                m.AddLinearLineElement(X + r);
                noElements++;
            }
        }

        /// <summary>
        /// Creates mesh using Quadratic Line Element.
        /// </summary>
        private void MeshByQuadraticLineElement()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates 1D mesh using Cubic Line Element.
        /// </summary>
        private void MeshByCubicLineElement()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 2DMesh

        public void Mesh2D()
        {
            if (elementType == eMeshElementType.LinearTirangular)
                MeshByLinearTriangularElement(4, 4);
        }

        /// <summary>
        /// Creates two dimensional triagular equal size mesh for rectangular models
        /// </summary>
        /// <param name="nx">Number of elements in x direction.</param>
        /// <param name="ny">Number of elements in y direction.</param>
        public void MeshByLinearTriangularElement(int nx, int ny)
        {
            e2DModel model = (e2DModel)this.model;
            double b, h, x1, x2, y1, y2;

            b = (model.Domain[2].X - model.Domain[0].X) / nx;
            h = (model.Domain[2].Y - model.Domain[0].Y) / ny;
            x2 = model.Domain[0].X;
            y2 = model.Domain[0].Y;

            for (int j = 0; j < ny; j++)
            {
                y1 = y2;
                y2 += h;

                for (int i = 0; i < nx; i++)
                {
                    x1 = x2;
                    x2 += b;
                    model.AddLinearTriangularElement(new ePoint(x1, y1), new ePoint(x2, y1), new ePoint(x2, y2));
                }
                x2 = model.Domain[0].X;
                for (int i = 0; i < nx; i++)
                {
                    x1 = x2;
                    x2 += b;
                    model.AddLinearTriangularElement(new ePoint(x1, y1), new ePoint(x2, y2), new ePoint(x1, y2));
                }
                x2 = model.Domain[0].X;
            }

            noNodes = model.Nodes.Count;
            noElements = model.Elements.Count;
            meshCompleted = true;
        }

        /// <summary>
        /// Adds all the nodes to the model.
        /// </summary>
        /// <param name="nx">Number of nodes in X-direction.</param>
        /// <param name="ny">Number of nodes in Y-direction.</param>
        public void AddNodes(int nx, int ny)
        {

            double X, Y, b, h;
            X = model.Domain[0].X;
            Y = model.Domain[0].Y;

            b = (model.Domain[2].X - model.Domain[0].X) / nx;
            h = (model.Domain[2].Y - model.Domain[0].Y) / ny;


            for (int j = 0; j <= ny; j++)
            {
                for (int i = 0; i <= nx; i++)
                {
                    model.Nodes.AddNode(new ePoint(X, Y), 1);
                    X += b;
                }
                Y += h;
                X = model.Domain[0].X;
            }

          
        }

        public void AddElements(int nx, int ny)
        {
            nx++;
            ny++;
            for (int j = 0; j < ny - 1; j++)
            {
                for (int i = 0; i < nx - 1; i++)
                {
                    model.Elements.AddElement(new eLinearTriangular(new eNode[3] { model.Nodes[nx * j + i], model.Nodes[nx * j + i + 1], model.Nodes[nx * (j + 1) + i + 1] }));
                }

                for (int i = 0; i < nx - 1; i++)
                {
                    model.Elements.AddElement(new eLinearTriangular(new eNode[3] { model.Nodes[nx * j + i], model.Nodes[nx * (j + 1) + i + 1], model.Nodes[nx * (j + 1) + i] }));
                }
            }

        }

        public void AddElementsRect(int nx, int ny)
        {
            nx++;
            ny++;
            for (int j = 0; j < ny - 1; j++)
            {
                for (int i = 0; i < nx - 1; i++)
                {
                    model.Elements.AddElement(new eBiLinearRectangular(new eNode[4] { model.Nodes[nx * j + i], model.Nodes[nx * j + i + 1],
                        model.Nodes[nx * (j + 1) + i + 1], model.Nodes[nx * (j + 1) + i] }));

                }

            }

        }

        public void MeshRectArea2(int nx, int ny)
        {
            this.nx = nx;
            this.ny = ny;
            AddNodes(nx, ny);
            if (elementType == eMeshElementType.LinearTirangular)
                AddElements(nx, ny);
            if (elementType == eMeshElementType.BilinearRectagular)
                AddElementsRect(nx, ny);
        }

        public ePoint[] GetBoundaryPoints()
        {
            List<ePoint> pts = new List<ePoint>();
            List<ePoint> dps = new List<ePoint>(model.Domain);
            dps.Add(dps[0]);

            for (int i = 0; i < dps.Count - 1; i++)
            {
                if (dps[i].Y == dps[i + i].Y)
                {
                    while (pts.Last() != dps[i + 1])
                    {
                        pts.Add(new ePoint(pts.Last().X + xSize, pts.Last().Y));
                    }
                }
                else
                {
                    while (pts.Last() != dps[i + 1])
                    {
                        pts.Add(new ePoint(pts.Last().X, pts.Last().Y + ySize));
                    }
                }
            }
            return pts.ToArray();
        }

        public void MeshByBilinearRectangular(int nx, int ny)
        {

            xSize = (model.Domain[1].X - model.Domain[0].X) / nx;
            ySize = (model.Domain.Last().Y - model.Domain[0].Y) / ny;

            double Xs = model.Domain[3].X, Ys = model.Domain[3].Y;
            double X = model.Domain[0].X, Y = model.Domain[0].Y;

            if (model.Domain.Length == 4)
            {
                Xs = model.Domain[2].X;
                Ys = model.Domain[2].Y;
            }

            for (int j = 0; j < ny; j++)
            {
                for (int i = 0; i < nx; i++)
                {
                    if (eMath.IsLess(X + xSize, Xs) || eMath.IsLess(Y + ySize, Ys))
                        ((e2DModel)model).AddBilinearRectangularElement(new ePoint(X, Y), new ePoint(X + xSize, Y), new ePoint(X + xSize, Y + ySize), new ePoint(X, Y + ySize));
                    X += xSize;
                }
                X = model.Domain[0].X;
                Y += ySize;
            }
        }

        public bool Horizontal(ePoint p1, ePoint p2)
        {
            return p1.Y == p2.Y;
        }

        #endregion

        #region 3DMesh

        private void Mesh3D()
        {
        }

        #endregion

        #endregion
    }
}
