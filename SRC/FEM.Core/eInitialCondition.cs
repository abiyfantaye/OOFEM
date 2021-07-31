using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FEM.Mathematics;

namespace FEM
{
    /// <summary>
    /// Holds initial variables for transient analysis.
    /// </summary>
    public class eInitialCondition
    {
        private int dofIndex;
        private double _value;
        private double startTime;
      
        /// <summary>
        /// Creates new instance of Initial Condition for transient types of models.
        /// </summary>
        /// <param name="DOFIndex">The index of degree of freedom to apply the initial condition.</param>
        /// <param name="Value">The value of the initial condition at starting time t = 0.</param>
        public eInitialCondition(int DOFIndex, double Value)           
        {
            this.dofIndex = DOFIndex;
            this._value = Value;
            this.startTime = 0;
        }

        /// <summary>
        /// The starting time where the initial value is available.
        /// </summary>
        public double StartTime
        {
            get
            {
               return startTime;
            }

            set
            {
                startTime = value;
            }
            
        }

        /// <summary>
        /// The value of the initial codition at the starting time.
        /// </summary>
        public double Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }

        /// <summary>
        /// Gets or sets the DOF index on which the intial condition is applied.
        /// </summary>
        public int DOFIndex
        {
            get
            {
                return dofIndex;
            }
            set
            {
                dofIndex = value;
            }
        }
    }
}
