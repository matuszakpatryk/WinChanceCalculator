using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinChanceCalculator
{
    public static class IterationMethodsOperations
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

        public static double ValueFromIterationForRow(double[] xVector, double[,] matrix, int rowNumber, double[] bVector)
        {
            int numberOfColumns = matrix.GetLength(1);
            double resultOfActionsForGivenRowOfXVector =
                CountResultOfActionsForGivenRowOfXVector(numberOfColumns, rowNumber, xVector, matrix);

            resultOfActionsForGivenRowOfXVector =
                (-resultOfActionsForGivenRowOfXVector + bVector[rowNumber]) / matrix[rowNumber, rowNumber];

            return resultOfActionsForGivenRowOfXVector;

        }


        public static bool CheckIsVectorSameAfterNextIteration(double[] lastVector, double[] vector)
        {
            bool isSame = true;
            for (int i = 0; i < vector.Length; i++)
            {
                if (lastVector[i] != vector[i])
                {
                    isSame = false;
                }
            }

            return isSame;
        }

    }
}
