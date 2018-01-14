using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinChanceCalculator
{
    class SeidelOperations
    {
        public static int countXwhile, countXFirstFor, countXSecondFor = 0;
        public static double countXTime, countXFirstForTime, countXSecondForTime = 0;

        public static void CountXVector(double[] xVector, int numberOfIterations, double[,] matrix,  double[] bVector)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Reset();
            stopwatch.Start();
            int numberOfRows = matrix.GetLength(0);
            double[] lastIterationXVector = new double[numberOfRows];
            int l = 1;
            while (true)
            {
                stopwatch.Reset();
                stopwatch.Start();
                for (int i=0; i < numberOfRows; i++)
                {
                    countXFirstFor++;
                    lastIterationXVector[i] = xVector[i];
                }

                countXFirstForTime = stopwatch.Elapsed.TotalMilliseconds;
                //Console.WriteLine("While: {0}, firstfor:{1}", l, firstfor);


                stopwatch.Reset();
                stopwatch.Start();
                for (int j = 0; j < numberOfRows; j++)
                {
                    countXSecondFor++;
                    xVector[j] = IterationMethodsOperations.ValueFromIterationForRow(xVector, matrix, j, bVector);
                }
                countXSecondForTime = stopwatch.Elapsed.TotalMilliseconds;
                //Console.WriteLine("While: {0}, secondfor:{1}", l, secondfor);

                if (IterationMethodsOperations.CheckIsVectorSameAfterNextIteration(lastIterationXVector, xVector))
                {                  
                    break;
                }
                countXwhile++;
            }

            countXTime = stopwatch.Elapsed.TotalMilliseconds;

            Console.WriteLine("CountXVector Time: " + String.Format("{0:N16}", countXTime));
            Console.WriteLine("CountXVector While Count: " + countXwhile);
            Console.WriteLine("CountXVector first for time: " + String.Format("{0:N16}", countXFirstForTime));
            Console.WriteLine("CountXVector first for count: " + countXFirstFor);
            Console.WriteLine("CountXVector second for time: " + String.Format("{0:N16}", countXSecondForTime));
            Console.WriteLine("CountXVector second for count: " + countXSecondFor);
            Console.WriteLine("ValueFrom Time: " + String.Format("{0:N16}", IterationMethodsOperations.valueFromTime));
            Console.WriteLine("ValueFrom1 Time: " + String.Format("{0:N16}", IterationMethodsOperations.valueFrom1Time));
            Console.WriteLine("ValueFrom count: " + IterationMethodsOperations.valueFrom);
            Console.WriteLine("CountResult Time: " + String.Format("{0:N16}", IterationMethodsOperations.countResultTime));
            Console.WriteLine("CountResult count: " + IterationMethodsOperations.countResult);
            Console.WriteLine("CountResult For count: " + IterationMethodsOperations.countResultFor);
            Console.WriteLine("CountResult If count: " + IterationMethodsOperations.countResultIf);
            Console.WriteLine("CheckIsVectorSame Time: " + String.Format("{0:N16}", IterationMethodsOperations.checkIsVectorSameTime));
            Console.WriteLine("CheckIsVectorSame Count: " + IterationMethodsOperations.checkIsVectorSame);
            Console.WriteLine("CheckIsVectorSame For Count: " + IterationMethodsOperations.checkIsVectorSameFor);
            Console.WriteLine("CheckIsDoubleZero count: " + IterationMethodsOperations.checkZero);

        }
    }
}
