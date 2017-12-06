using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinChanceCalculator
{
    public class MyMatrix
    {
        public int rows;
        public int columns;
        public double[,] matrix;
        public double[,] defaultMatrix;

        public MyMatrix(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            matrix = new double[rows, columns];
            defaultMatrix = new double[rows, columns];
        }

        ////////Implementation////////

        public void complementMatrix(double[,] table)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    matrix[i, j] = table[i, j];
                    defaultMatrix[i, j] = table[i, j];
                }
            }
        }

        public void printMatrix()
        {
            for (int i = 0; i < rows; i++)
            {
                Console.Write("| ");
                for (int j = 0; j < columns; j++)
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

        public double[] gaussWithoutChoice(double[] bVector)
        {
            bVector = makeRowEchelonMatrix(bVector);
            double[] xVector = countXVector(bVector);
            setDefaultMatrix();
            return xVector;
        }


        public double[] gaussWithRowChoice(double[] bVector)
        {
            bVector = makeRowEchelonMatrixWithRowChoice(bVector);
            double[] xVector = countXVector(bVector);
            setDefaultMatrix();
            return xVector;
        }

        public double[] gaussWithFullChoice(double[] bVector)
        {
            int[] xVectorNumberChangeTable = { 1, 2, 3, 4 };
            bVector = makeRowEchelonMatrixWithFullChoice(bVector, xVectorNumberChangeTable);
            double[] xVector = countModifiedXVector(bVector, xVectorNumberChangeTable);
            setDefaultMatrix();
            return xVector;
        }

        private double[] makeRowEchelonMatrix(double[] bVector)
        {
            for (int k = 0; k < columns; k++)
            {
                for (int i = k; i < rows - 1; i++)
                {
                    double numberForMultiply = (dynamic)matrix[i + 1, k] / matrix[k, k];

                    for (int j = k; j < columns; j++)
                    {
                        matrix[i + 1, j] -= ((dynamic)matrix[k, j] * numberForMultiply);
                    }

                    bVector[i + 1] -= ((dynamic)bVector[k] * numberForMultiply);
                }
            }

            return bVector;
        }

        private double[] makeRowEchelonMatrixWithRowChoice(double[] bVector)
        {
            for (int k = 0; k < columns; k++)
            {
                int rowWithDiagonalNumber = k;
                int rowNumberWithMaxNumberInColumn = findRowWithMaxNumberInColumnUnderDiagonal(k);

                if (rowNumberWithMaxNumberInColumn != rowWithDiagonalNumber)
                {
                    bVector = swapRows(rowWithDiagonalNumber, rowNumberWithMaxNumberInColumn, bVector);
                }

                for (int i = k; i < rows - 1; i++)
                {
                    double numberForMultiply = (dynamic)matrix[i + 1, k] / matrix[k, k];

                    for (int j = k; j < columns; j++)
                    {
                        matrix[i + 1, j] -= ((dynamic)matrix[k, j] * numberForMultiply);
                    }

                    bVector[i + 1] -= ((dynamic)bVector[k] * numberForMultiply);
                }

            }
            return bVector;
        }


        private double[] makeRowEchelonMatrixWithFullChoice(double[] bVector, int[] xVectorNumberChangeTable)
        {
            for (int k = 0; k < columns; k++)
            {

                int rowNumberWithDiagonalPoint = k;
                int rowNumberWithMaxNumberInMatrix = rowNumberWithDiagonalPoint;
                int columnNumberWithMaxNumberInMatrix = rowNumberWithDiagonalPoint;

                findRowAndColumnWithMaxElementInMatrix(rowNumberWithDiagonalPoint, ref rowNumberWithMaxNumberInMatrix, ref columnNumberWithMaxNumberInMatrix);
                if (rowNumberWithMaxNumberInMatrix != rowNumberWithDiagonalPoint)
                {
                    bVector = swapRows(rowNumberWithDiagonalPoint, rowNumberWithMaxNumberInMatrix, bVector);
                }

                if (columnNumberWithMaxNumberInMatrix != rowNumberWithDiagonalPoint)
                {
                    xVectorNumberChangeTable = swapColumns(rowNumberWithDiagonalPoint, columnNumberWithMaxNumberInMatrix, xVectorNumberChangeTable);
                }

                for (int i = k; i < rows - 1; i++)
                {
                    double numberForMultiply = (dynamic)matrix[i + 1, k] / matrix[k, k];

                    for (int j = k; j < columns; j++)
                    {
                        matrix[i + 1, j] -= ((dynamic)matrix[k, j] * numberForMultiply);
                    }

                    bVector[i + 1] -= ((dynamic)bVector[k] * numberForMultiply);
                }
            }

            return bVector;
        }

        private int findRowWithMaxNumberInColumnUnderDiagonal(int columnNumber)
        {
            int rowNumberWithMaxNumberInColumn = columnNumber;
            int firstRowUnderDiagonal = columnNumber + 1;
            for (int i = firstRowUnderDiagonal; i < rows; i++)
            {
                if ((dynamic)matrix[rowNumberWithMaxNumberInColumn, columnNumber] < matrix[i, columnNumber])
                {
                    rowNumberWithMaxNumberInColumn = i;
                }
            }
            return rowNumberWithMaxNumberInColumn;
        }

        private void findRowAndColumnWithMaxElementInMatrix(int rowNumberWithDiagonalPoint, ref int rowNumberWithMaxNumberInMatrix, ref int columnNumberWithMaxNumberInMatrix)
        {
            int columnNumberWithDiagonalPoint = rowNumberWithDiagonalPoint;

            for (int i = rowNumberWithDiagonalPoint; i < rows; i++)
            {
                for (int j = columnNumberWithDiagonalPoint; j < columns; j++)
                {
                    if ((dynamic)matrix[rowNumberWithMaxNumberInMatrix, columnNumberWithMaxNumberInMatrix] < matrix[i, j])
                    {
                        rowNumberWithMaxNumberInMatrix = i;
                        columnNumberWithMaxNumberInMatrix = j;
                    }
                }
            }
        }

        private double[] swapRows(int rowWithDiagonalNumber, int rowNumberWithMaxNumber, double[] bVector)
        {
            double[] tempRow = new double[columns];
            double tempValue;
            for (int i = 0; i < columns; i++)
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

        private int[] swapColumns(int columnNumberWithDiagonalPoint, int columnNumberWithMaxNumber, int[] xVector)
        {
            double[] tempColumn = new double[rows];
            int tempValue;
            for (int i = 0; i < rows; i++)
            {
                tempColumn[i] = matrix[i, columnNumberWithDiagonalPoint];
                matrix[i, columnNumberWithDiagonalPoint] = matrix[i, columnNumberWithMaxNumber];
                matrix[i, columnNumberWithMaxNumber] = tempColumn[i];
            }

            tempValue = xVector[columnNumberWithDiagonalPoint];
            xVector[columnNumberWithDiagonalPoint] = xVector[columnNumberWithMaxNumber];
            xVector[columnNumberWithMaxNumber] = tempValue;

            return xVector;
        }

        private double[] countXVector(double[] bVector)
        {
            double[] xVector = new double[bVector.Length];
            for (int i = bVector.Length - 1; i >= 0; i--)
            {
                int j = i;
                double numerator = bVector[i];
                while (j < (columns - 1))
                {
                    numerator -= ((dynamic)matrix[i, j + 1] * xVector[j + 1]);
                    j++;
                }
                xVector[i] = (dynamic)numerator / matrix[i, i];

            }

            return xVector;
        }

        private double[] countModifiedXVector(double[] bVector, int[] xVectorNumberChangeTable)
        {
            double[] xVector = new double[bVector.Length];
            xVector = countXVector(bVector);

            int indexTemp;
            double valueTemp;

            for (int i = 0; i < xVector.Length; i++)
            {
                if (xVectorNumberChangeTable[i] != i)
                {
                    int indexForReplacing = xVectorNumberChangeTable[i] - 1;

                    indexTemp = xVectorNumberChangeTable[i];
                    xVectorNumberChangeTable[i] = xVectorNumberChangeTable[indexForReplacing];
                    xVectorNumberChangeTable[indexForReplacing] = indexTemp;

                    valueTemp = xVector[i];
                    xVector[i] = xVector[indexForReplacing];
                    xVector[indexForReplacing] = valueTemp;
                }
            }


            return xVector;
        }

        public void printVector(double[] vector)
        {
            Console.WriteLine("Wektor B");
            for (int i = 0; i < vector.Length; i++)
            {
                Console.WriteLine(vector[i]);
            }
        }

        public void setDefaultMatrix()
        {
            matrix = (double[,])defaultMatrix.Clone();
        }

        //public void WriteMatrixToFile(string name)
        //{
        //    if (!name.Contains("Result"))
        //    {
        //        for (int i = 0; i < rows; i++)
        //        {
        //            for (int j = 0; j < columns; j++)
        //            {
        //                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\pmatusza\Documents\MobaXterm\home\Studia\Algorytmy\Zad2\Zadanie2\Zadanie2\Data\DataRange\" + name + ".txt", true))
        //                {
        //                    file.Write(String.Format("{0:N3}", matrix[i, j]));
        //                    file.Write(" ");
        //                }
        //            }
        //            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\pmatusza\Documents\MobaXterm\home\Studia\Algorytmy\Zad2\Zadanie2\Zadanie2\Data\DataRange\" + name + ".txt", true))
        //            {
        //                file.Write("\n");
        //            }
        //        }
        //        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\pmatusza\Documents\MobaXterm\home\Studia\Algorytmy\Zad2\Zadanie2\Zadanie2\Data\DataRange\" + name + ".txt", true))
        //        {
        //            file.Write("*** *** *** *** *** ***\n");
        //        }
        //    }
        //    else
        //    {
        //        for (int i = 0; i < rows; i++)
        //        {
        //            for (int j = 0; j < columns; j++)
        //            {
        //                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\pmatusza\Documents\MobaXterm\home\Studia\Algorytmy\Zad2\Zadanie2\Zadanie2\Data\Results\" + name + ".txt", true))
        //                {
        //                    file.Write(String.Format("{0:N3}", matrix[i, j]));
        //                    file.Write(" ");
        //                }
        //            }
        //            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\pmatusza\Documents\MobaXterm\home\Studia\Algorytmy\Zad2\Zadanie2\Zadanie2\Data\Results\" + name + ".txt", true))
        //            {
        //                file.Write("\n");
        //            }
        //        }
        //        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\pmatusza\Documents\MobaXterm\home\Studia\Algorytmy\Zad2\Zadanie2\Zadanie2\Data\Results\" + name + ".txt", true))
        //        {
        //            file.Write("*** *** *** *** *** ***\n");
        //        }
        //    }

        //}

    }
}
