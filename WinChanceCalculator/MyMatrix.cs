using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace WinChanceCalculator
{
    public class MyMatrix
    {
        private int numberOfRows;
        private int numberOfColumns;
        private double[,] matrix;
        private double[,] defaultMatrix;

        public MyMatrix(int rows, int columns)
        {
            numberOfRows = rows;
            numberOfColumns = columns;
            matrix = new double[rows, columns];
            defaultMatrix = new double[rows, columns];
        }

        ////////Implementation////////

        public void ComplementMatrix(double[,] table)
        {
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    matrix[i, j] = table[i, j];
                    defaultMatrix[i, j] = table[i, j];
                }
            }
        }

        public void PrintMatrix()
        {
            for (int i = 0; i < numberOfRows; i++)
            {
                Console.Write("| ");
                for (int j = 0; j < numberOfColumns; j++)
                {
                    if (j != 0)
                    {
                        Console.Write("| ");
                    }
                    String s = String.Format("{0:N3}", matrix[i, j]);
                    Console.Write(s);

                }
                Console.WriteLine("|");
            }
        }

        public double[] GaussWithRowChoice(double[] bVector)
        {
            bVector = MakeRowEchelonMatrixWithRowChoice(bVector);
            double[] xVector = GaussOperations.CountXVector(bVector, numberOfColumns, matrix);
            SetDefaultMatrix();
            return xVector;
        }

        private double[] MakeRowEchelonMatrixWithRowChoice(double[] bVector)
        {
            for (int k = 0; k < numberOfColumns; k++)
            {
                int rowWithDiagonalNumber = k;
                int rowNumberWithMaxNumberInColumn = FindRowWithMaxNumberInColumnUnderDiagonal(k);

                if (rowNumberWithMaxNumberInColumn != rowWithDiagonalNumber)
                {
                    bVector = SwapRows(rowWithDiagonalNumber, rowNumberWithMaxNumberInColumn, bVector);
                }

                for (int i = k; i < numberOfRows - 1; i++)
                {
                    double numberForMultiply = (dynamic)matrix[i + 1, k] / matrix[k, k];

                    for (int j = k; j < numberOfColumns; j++)
                    {
                        matrix[i + 1, j] -= ((dynamic)matrix[k, j] * numberForMultiply);
                    }

                    bVector[i + 1] -= ((dynamic)bVector[k] * numberForMultiply);
                }

            }
            return bVector;
        }

        private int FindRowWithMaxNumberInColumnUnderDiagonal(int columnNumber)
        {
            int rowNumberWithMaxNumberInColumn = columnNumber;
            int firstRowUnderDiagonal = columnNumber + 1;
            for (int i = firstRowUnderDiagonal; i < numberOfRows; i++)
            {
                if ((dynamic)matrix[rowNumberWithMaxNumberInColumn, columnNumber] < matrix[i, columnNumber])
                {
                    rowNumberWithMaxNumberInColumn = i;
                }
            }
            return rowNumberWithMaxNumberInColumn;
        }

        private double[] SwapRows(int rowWithDiagonalNumber, int rowNumberWithMaxNumber, double[] bVector)
        {
            double[] tempRow = new double[numberOfColumns];
            double tempValue;
            for (int i = 0; i < numberOfColumns; i++)
            {
                tempRow[i] = matrix[rowWithDiagonalNumber, i];
                matrix[rowWithDiagonalNumber, i] = matrix[rowNumberWithMaxNumber, i];
                matrix[rowNumberWithMaxNumber, i] = tempRow[i];
            }

            tempValue = bVector[rowWithDiagonalNumber];
            bVector[rowWithDiagonalNumber] = bVector[rowNumberWithMaxNumber];
            bVector[rowNumberWithMaxNumber] = tempValue;

            return bVector;
        }
        

        public static void PrintVector(double[] vector)
        {
            Console.WriteLine("Wektor B");
            for (int i = 0; i < vector.Length; i++)
            {
                Console.WriteLine(vector[i]);
            }
        }

        public void SetDefaultMatrix()
        {
            matrix = (double[,])defaultMatrix.Clone();
        }

        public double[] Jacobi(double[] bVector, int numberOfIterations)
        {
            double[] xVector = IterationMethodsOperations.SetDefaultVector(numberOfColumns);
            JacobiOperations.CountXVector(xVector, numberOfIterations, matrix,  bVector);
            return xVector;
        }

        public double[] Seidel(double[] bVector, int numberOfIterations)
        {
            double[] xVector = IterationMethodsOperations.SetDefaultVector(numberOfColumns);
            SeidelOperations.CountXVector(xVector, numberOfIterations, matrix, bVector);
            return xVector;

        }



        public void WriteMatrixToFile()
        {
            
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Patryk\Desktop\Data\DataRange.txt", true))
                    {
                        file.Write(String.Format("{0:N3}", matrix[i, j]));
                        file.Write(" ");
                    }
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Patryk\Desktop\Data\DataRange.txt", true))
                {
                    file.Write("\n");
                }
            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Patryk\Desktop\Data\DataRange.txt", true))
            {
                file.Write("*** *** *** *** *** ***\n");
            }                 
        }
    }
}
