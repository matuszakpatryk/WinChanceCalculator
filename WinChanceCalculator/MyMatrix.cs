using System;
using System.Collections.Generic;
using System.Globalization;
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

        public void ComplementMatrix(double[,] table)
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

        public void PrintMatrix()
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

        public double[] GaussWithRowChoice(double[] bVector)
        {
            bVector = MakeRowEchelonMatrixWithRowChoice(bVector);
            double[] xVector = CountXVector(bVector);
            SetDefaultMatrix();
            return xVector;
        }

        private double[] MakeRowEchelonMatrixWithRowChoice(double[] bVector)
        {
            for (int k = 0; k < columns; k++)
            {
                int rowWithDiagonalNumber = k;
                int rowNumberWithMaxNumberInColumn = FindRowWithMaxNumberInColumnUnderDiagonal(k);

                if (rowNumberWithMaxNumberInColumn != rowWithDiagonalNumber)
                {
                    bVector = SwapRows(rowWithDiagonalNumber, rowNumberWithMaxNumberInColumn, bVector);
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

        private int FindRowWithMaxNumberInColumnUnderDiagonal(int columnNumber)
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

        private double[] SwapRows(int rowWithDiagonalNumber, int rowNumberWithMaxNumber, double[] bVector)
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

        private double[] CountXVector(double[] bVector)
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
        

        public void PrintVector(double[] vector)
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
