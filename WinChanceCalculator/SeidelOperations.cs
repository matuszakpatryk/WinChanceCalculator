using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinChanceCalculator
{
    class SeidelOperations
    {
        public static void CountXVector(double[] xVector, int numberOfIterations, double[,] matrix,  double[] bVector)
        {
            int numberOfRows = matrix.GetLength(0);
            while (true)
            {
                double[] lastIterationXVector = (double[])xVector.Clone();

                for (int j = 0; j < numberOfRows; j++)
                {
                    xVector[j] = IterationMethodsOperations.ValueFromIterationForRow(xVector, matrix, j, bVector);
                }

                if (IterationMethodsOperations.CheckIsVectorSameAfterNextIteration(lastIterationXVector, xVector))
                {
                    break;
                }
            }

        }
    }
}
