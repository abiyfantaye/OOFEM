using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Solvers;
using ESADS.EGraphics;
using System.Drawing;
using System.Windows.Forms;
using FEM.Output;

namespace FEM
{
    /// <summary>
    /// Base calss for all types of model and defines common 
    /// properteis and methods for all classes FEM model calses.
    /// </summary>
    public abstract class eModel 
    {
        #region Fields
        /// <summary>
        /// Holds the name of the ModelType.
        /// </summary>
        private string name;
        /// <summary>
        /// List contain all the nodes in the model.
        /// </summary>
        protected eNodes nodes;
        /// <summary>
        /// Contains all the elements in the model.
        /// </summary>
        protected eElements elements;
        /// <summary>
        /// Contains boundary value data. 
        /// </summary>
        protected eBCs bcs;
        /// <summary>
        /// Holds the mesh data of the Model.
        /// </summary>
        protected eMesh mesh;
        /// <summary>
        /// Holds the domain of one dimensional problem.
        /// </summary>
        protected ePoint[] domain;
        /// <summary>
        /// A collection which holds all the layers on which the drawing of the model is done.
        /// </summary>
        protected eLayers layers;
        /// <summary>
        /// Holds the type of analysis being performed.
        /// </summary>
        protected eAnalysisType analysisType;
        /// <summary>
        /// Holds the nalysis object for the model.
        /// </summary>
        protected eAnalysis analysis;
         

        #endregion
      
        #region Constructor
        /// <summary>
        /// Creates new model from given parameters.
        /// </summary>
        /// <param name="name">Name of the model.</param>
        /// <param name="AnalysisType">The type of analysis to be done. </param>
        public eModel(string name, eAnalysisType AnalysisType)
        {
            this.name = name;
            this.elements = new eElements();
            this.nodes = new eNodes();
            this.bcs = new eBCs();
            this.mesh = new eMesh(this);
            this.layers = new eLayers();
            this.layers.Add("Elements", Color.Black, eLineTypes.Continuous, 2.0f);
            this.layers.Add("Nodes", Color.Red);
            this.analysisType = AnalysisType;

            switch (AnalysisType)
            {
                case eAnalysisType.SteadyState:
                    analysis = new eSteadyStateAnalysis(this);
                    break;
                case eAnalysisType.Transient:
                    analysis = new eTransientAnalysis(this);
                    break;
                case eAnalysisType.EigneValue:
                    analysis = new eEigenValueAnalysis(this);
                    break;
                case eAnalysisType.Modal:
                    analysis = new eModalAnalysis(this);
                    break;
            }

        }

        #endregion

        #region Properties 

        public eMesh Mesher
        {
            get
            {
                return mesh;
            }
        }

        /// <summary>
        /// Gets or sets the boundary coodition for the model.
        /// </summary>
        public eBCs BCs
        {
            get
            {
                return bcs;
            }
            set
            {
                bcs = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the ModelType.
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
        /// Gets or sets all the Nodes in the ModelType.
        /// </summary>
        public eNodes Nodes
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
        /// Gets or sets all the Elements in the ModelType.
        /// </summary>
        public eElements Elements
        {
            get
            {
                return elements;
            }
            set
            {
                elements = value;
            }
        }

        /// <summary>
        /// Gets or sets the doamain of the model.
        /// </summary>
        public ePoint[] Domain
        {
            get
            {
                return domain;
            }

            set
            {
                domain = value;
            }
        }

        /// <summary>
        /// Gets or sets the layers on which the model is drawn.
        /// </summary>
        public eLayers Layers
        {
            get
            {
                return layers;
            }
            set
            {
                layers = value;
            }
        }

        /// <summary>
        /// Gets or sets the analysis used for the model.
        /// </summary>
        public eAnalysis Analysis
        {
            get
            {
                return analysis;
            }
            set
            {
                analysis = value;
            }
        }
        #endregion 

        #region Methods

        /// <summary>
        ///Meshs the model. 
        /// </summary>
        public void Mesh()
        {
            //Generates mesh for the medel.
            mesh.Mesh();
        }

        /// <summary>
        /// Draws this model.
        /// </summary>
        public void Draw()
        {
            eForm dwgForm = new eForm();          
            layers.SetDrawingForm(dwgForm);
            dwgForm.Invalidate();
            elements.Draw(layers[0]);
            layers.Zoom(new PointF((float)(elements[0].Nodes[0].X),(float)(elements[0].Nodes[0].X)), 100f);
            Application.Run(dwgForm);
        }

        /// <summary>
        /// Removes all elements and nodes in the model.
        /// This method is usually called for remeshing.
        /// </summary>
        public void Refresh()
        {
            this.elements.Reset();
            this.nodes.Reset();
        }

        public abstract void PrintNodalValues();

        public abstract void PlotResult();
        #endregion   
    
      
    }
}

