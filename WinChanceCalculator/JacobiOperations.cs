using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WinChanceCalculator
{
    static class JacobiOperations
    {
        public static void CountXVector(double[] xVector, int numberOfIterations, double[,] matrix, double[] bVector)
        {
            int numberOfRows = matrix.GetLength(0);

            for (int i = 0; i < numberOfIterations; i++)
            {
                double[] lastIterationXVector = (double[])xVector.Clone();

                for (int j = 0; j < numberOfRows; j++)
                {
                    xVector[j] = IterationMethodsOperations.ValueFromIterationForRow(lastIterationXVector, matrix, j, bVector);
                }
            }
            
        }
    }

}
