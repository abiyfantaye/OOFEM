using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEM.Mathematics
{
    public class ePolynomial : eFunction
    {
        private int[] powers;
        private double[] coefs;

        public ePolynomial(eSpaceType Dim, int[] Powers)
        {
            this.powers = Powers;
            this.dim = Dim;
        }

        public override double GetValueAt(double[] coord)
        {
            double Value = 0;
            double temp = 1;

            if (coord.Length < (int)dim)
                throw new Exception("More coordinates are needed");

            for (int i = 0; i < coefs.Length; i++)
            {
                for (int j = 0; j < (int)dim; j++)
                    temp *= System.Math.Pow(coord[j], powers[i * (int)dim + j]);

                Value += coefs[i] * temp;
                temp = 1;
            }

            return Value;
        }

        public override double GetDerivative(params double[] coord)
        {
            int[] dprs;
            double []dcofs;
            Derivate(out dprs, out dcofs);

            double Value = 0;
            double temp = 1;

            for (int i = 0; i < coefs.Length; i++)
            {
                for (int j = 0; j < (int)dim; j++)
                    temp *= System.Math.Pow(coord[j], dprs[i * (int)dim + j]);

                Value += dcofs[i] * temp;
                temp = 1;
            }

            return Value;
        }

        private void Derivate(out int[] dprs, out double[] dcofs)
        {
            dprs = new int[powers.Length];
            dcofs = new double[coefs.Length];

            for (int i = 0; i < coefs.Length; i++)
            {
                for (int j = 0; j < (int)dim; j++)
                {
                    if (powers[i * (int)dim + j] > 0)
                    {
                        dprs[i * (int)dim + j] = powers[i * (int)dim + j] - 1;
                        dcofs[i] = coefs[i] * powers[i * (int)dim + j];
                    }
                }
            }
        }


        public int[] Powers
        {
            get
            {
                return powers;
            }

        }

        public double[] Coefs
        {
            get
            {
                return coefs;
            }
            set
            {
                coefs = value;
            }
        }

        public static eFunction operator+(ePolynomial f1, ePolynomial f2)
        {
            double[] rCofs = new double[f1.coefs.Length];

            for (int i = 0; i < rCofs.Length; i++)
            {
                rCofs[i] = f1.coefs[i] + f2.coefs[i];
            }
            ePolynomial p = new ePolynomial(f1.DimType, f1.powers);
            return p;
        }

        public static eFunction operator -(ePolynomial f1, ePolynomial f2)
        {
            double[] rCofs = new double[f1.coefs.Length];

            for (int i = 0; i < rCofs.Length; i++)
            {
                rCofs[i] = f1.coefs[i] - f2.coefs[i];
            }
            ePolynomial p = new ePolynomial(f1.DimType, f1.powers);
            return p;
        }
    }
}
