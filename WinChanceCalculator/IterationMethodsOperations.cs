using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinChanceCalculator
{
    public static class IterationMethodsOperations
    {
        public static int valueFrom, countResult, countResultFor, countResultIf, checkIsVectorSame, checkIsVectorSameFor, checkZero = 0;
        public static double valueFromTime, valueFrom1Time, countResultTime, checkIsVectorSameTime = 0;
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
            countResult++;
            double result = 0;
            var stopwatch = new Stopwatch();
            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < numberOfColumns; i++)
            {
                countResultFor++;
                if (i != rowNumber)
                {
                    countResultIf++;
                    result += matrix[rowNumber, i] * xVector[i];
                }
            }
            countResultTime = stopwatch.Elapsed.TotalMilliseconds;

            //Console.WriteLine("result: {0}", resultTime);

            return result;
        }

        public static double ValueFromIterationForRow(double[] xVector, double[,] matrix, int rowNumber, double[] bVector)
        {
            valueFrom++;
            int numberOfColumns = matrix.GetLength(1);
            var stopwatch = new Stopwatch();
            stopwatch.Reset();
            stopwatch.Start();
            double resultOfActionsForGivenRowOfXVector = CountResultOfActionsForGivenRowOfXVector(numberOfColumns, rowNumber, xVector, matrix);
            valueFromTime = stopwatch.Elapsed.TotalMilliseconds;

            stopwatch.Reset();
            stopwatch.Start();
            resultOfActionsForGivenRowOfXVector = (-resultOfActionsForGivenRowOfXVector + bVector[rowNumber]) / matrix[rowNumber, rowNumber];
            valueFrom1Time = stopwatch.Elapsed.TotalMilliseconds;

            return resultOfActionsForGivenRowOfXVector;

        }


        public static bool CheckIsVectorSameAfterNextIteration(double[] lastVector, double[] vector)
        {
            checkIsVectorSame++;
            var stopwatch = new Stopwatch();
            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < vector.Length; i++)
            {
                checkIsVectorSameFor++;
                if (!CheckIsDoubleZero(lastVector[i] - vector[i]))
                {
                    return false;
                }
            }

            checkIsVectorSameTime = stopwatch.Elapsed.TotalMilliseconds;

            return true;
        }

        private static bool CheckIsDoubleZero(double number)
        {
            checkZero++;
            if (Math.Abs(number) <= 0.0000000001)
            {
                return true;
            }

            return false;
        }


    }
}
