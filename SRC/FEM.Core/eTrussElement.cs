using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace FEM
{
    public abstract class  eTrussElement : eLineElement
    {
        /// <summary>
        /// Holds the are of the element.
        /// </summary>
        protected double A;
        /// <summary>
        /// Holds the modulus of elasticity of element.
        /// </summary>
        protected double E;
        /// <summary>
        /// Holds the density of the element.
        /// </summary>
        protected double density;
        /// <summary>
        /// Holds the type of the mass matrix( lumped or consistent)
        /// </summary>
        protected eMassMatrixType massMatrixType;

        /// <summary>
        /// Golds the displacement of the node in global coordinate.
        /// </summary>
        protected Vector<double> gDisp;


        public eTrussElement(eNode[] ConnNodes)
            : base(ConnNodes)
        {
            massMatrixType = eMassMatrixType.Consistent;
        }
        /// <summary>
        /// Gets or sets the area of the truss element used. 
        /// </summary>
        public double Area
        {
            get
            {
                return A;
            }

            set
            {
                A = value;
            }
        }

        /// <summary>
        /// Gets or sets the modulus of elasticity of the matiral.
        /// </summary>
        public double ModulusOfElasticity
        {
            get
            {
                return E;
            }
            set
            {
                E = value;
            }
        }
        /// <summary>
        /// Gets or sets the mass desity of the element.
        /// </summary>
        public double Density
        {
            get
            {
                return density;
            }

            set { density = value; }
        }

        /// <summary>
        /// Gets or sets the type of the mass matrix to be used.
        /// </summary>
        public eMassMatrixType MassMatrixType
        {
            get
            {
                return massMatrixType;
            }

            set
            {
                massMatrixType = value;
            }
        }

        /// <summary>
        /// Gets or sets the displacement vector of the element in global coordinate system.
        /// </summary>
        public Vector<double> GDisp
        {
            get
            {
                return gDisp;
            }

            set
            {
                gDisp = value;
            }
        }
    }
}
