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
        public static double[] SetDefaultVector(int numberOfColumns)
        {
            double[] defaultVector = new double[numberOfColumns];
            for (int i = 0; i < numberOfColumns; i++)
            {
                defaultVector[i] = 0;
            }

            return defaultVector;
        }


        public static double CountResultOfActionsForGivenRowOfXVector(int numberOfColumns, int rowNumber, double[] xVector, double[,] matrix)
        {
            double result = 0;
            for (int i = 0; i < numberOfColumns; i++)
            {
                if (i != rowNumber)
                {
                    result += matrix[rowNumber, i] * xVector[i];
                }
            }

            return result;
        }

        public static double ValueFromIterationForRow(double[] xVector, double[,] matrix, int rowNumber, int numberOfColumns, double[] bVector)
        {
            double resultOfActionsForGivenRowOfXVector =
                CountResultOfActionsForGivenRowOfXVector(numberOfColumns, rowNumber, xVector, matrix);

            resultOfActionsForGivenRowOfXVector =
                (-resultOfActionsForGivenRowOfXVector + bVector[rowNumber]) / matrix[rowNumber, rowNumber];

            return resultOfActionsForGivenRowOfXVector;

        }

        public static void CountXVector(double[] xVector, int numberOfIterations, double[,] matrix, int numberOfRows, int numberOfColumns, double[] bVector)
        {
            
            for (int i = 0; i < numberOfIterations; i++)
            {
                double[] lastIterationXVector = (double[])xVector.Clone();

                for (int j = 0; j < numberOfRows; j++)
                {
                    xVector[j] = ValueFromIterationForRow(lastIterationXVector, matrix, j, numberOfColumns, bVector);
                }
            }
            
        }
    }

}
