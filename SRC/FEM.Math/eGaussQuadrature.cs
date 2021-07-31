using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEM.Mathematics
{
    /// <summary>
    /// Contains methods and properties that are used to handle GaussQuadrature intergration.
    /// </summary>
    public static class eGaussQuadrature
    {
       

        /// <summary>
        /// Integrates using GaussQuadrature rule in one dimension. 
        /// </summary>
        /// <param name="N">Number of poitns.</param>
        /// <param name="Func">The function to be integrated.</param>
        /// <returns></returns>
        public static double Integrate(int N, double [] Func)
        {
            double value = 0;
            double[] W;

            W = GetIntWeights(N);

            for (int i = 0; i < N; i++)
            {
                    value += W[i] * Func[i];
            }

            return value;
        }

        /// <summary>
        /// Integrates using GaussQuadrature rule in one dimension. 
        /// </summary>
        /// <param name="Nx">Number of poitns in x-direction.</param>
        /// <param name="Ny">Number of points in y-direction.</param>
        /// <param name="Func">The function to be integrated.</param>
        /// <returns></returns>
        public static double Integrate(int Nx, int Ny, double[,] Func)
        {
            double value = 0;
            double[] Wx, Wy;

            Wx = GetIntWeights(Nx);
            Wy = GetIntWeights(Ny);

            for (int i = 0; i < Nx; i++)
            {
                for (int j = 0; j < Ny; j++)
                {
                    value += Wx[i] * Wy[j] * Func[i, j];
                }
            }

            return value;
        }

        /// <summary>
        /// Integrates using GaussQuadrature rule in one dimension. 
        /// </summary>
        /// <param name="Nx">Number of poitns in x-direction.</param>
        /// <param name="Ny">Number of points in y-direction.</param>
        /// <param name="Nz">Number of pionts in z-direction.</param>
        /// <returns></returns>
        public static double Integrate(int Nx, int Ny, int Nz, double[,,] Func)
        {
            double value = 0;
            double[] Wx, Wy, Wz;

            Wx = GetIntWeights(Nx);
            Wy = GetIntWeights(Ny);
            Wz = GetIntWeights(Nz);

            for (int i = 0; i < Nx; i++)
            {
                for (int j = 0; j < Ny; j++)
                {
                    for (int k = 0; k < Nz; k++)
                    {
                        value += Wx[i] * Wy[j] * Wz[k] * Func[i, j, k];
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Retures one dimensional array of integration points.
        /// <param name="N">Number of point for integration.</param>
        /// </summary>
        /// <returns>Returs the Gauss integration points</returns>
        public static double[] GetIntPoits(int N)
        {
            double[] result = new double[N];

            if (N == 1)
            {
                result[0] = 0;
            }
            else if (N == 2)
            {
                result[0] = -0.577350269189626;
                result[1] = 0.577350269189626;
            }
            else if (N == 3)
            {
                result[0] = -0.774596669241483;
                result[1] = 0;
                result[2] = 0.774596669241483;
            }
            else if (N == 4)
            {
                result[0] = -0.861136311594053;
                result[1] = -0.339981043584856;
                result[2] = 0.339981043584856;
                result[3] = 0.861136311594053;
            }
            else if (N == 5)
            {
                result[0] = -0.906179845938664;
                result[1] = -0.538469310105683;
                result[2] = 0;
                result[3] = 0.538469310105683;
                result[4] = 0.906179845938664;
            }
            else
            {
                throw new Exception("This case is not included");
            }

            return result;
        }

        /// <summary>
        /// Retures one dimensional array of integration points.
        /// <param name="N">Number of point for integration.</param>
        /// </summary>
        /// <returns>Returs the Gauss integration points</returns>
        public static double[] GetIntWeights(int N)
        {
            double[] result = new double[N];

            if (N == 1)
            {
                result[0] = 2.0;
            }
            else if (N == 2)
            {
                result[0] = 1.0;
                result[1] = 1.0;
            }
            else if (N == 3)
            {
                result[0] = 0.555555555555556;
                result[1] = 0.888888888888889;
                result[2] = 0.555555555555556;
            }
            else if (N == 4)
            {
                result[0] = 0.347854845137454;
                result[1] = 0.652145154862546;
                result[2] = 0.652145154862546;
                result[3] = 0.347854845137454;
            }
            else if (N == 5)
            {
                result[0] = 0.236926885056189;
                result[1] = 0.478628670499366;
                result[2] = 0.568888888888889;
                result[3] = 0.478628670499366;
                result[4] = 0.236926885056189;
            }
            else
            {
                throw new Exception("This case is not included");
            }

            return result;
        }
    }
}
