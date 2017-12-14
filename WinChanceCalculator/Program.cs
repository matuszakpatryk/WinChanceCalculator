using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WinChanceCalculator
{
    class Program
    {
        public static int size = 10;
        public static int N = 2;
        static void Main(string[] args)
        {

            Random rand = new Random();

            MyMatrix firstMatrix = new MyMatrix(size, size);
            MyMatrix secondMatrix = new MyMatrix(size, size);
            MyMatrix thirdMatrix = new MyMatrix(size, size);
            double[] firstVector = new double[size];

            /*
            for (int j = 0; j < size; j++)
            {
                for (int k = 0; k < size; k++)
                {
                    int a = rand.Next(1, 10);
                    int b = rand.Next(1, 10);
                    firstMatrix.matrix[j, k] = (double)a / b;
                    a = rand.Next(1, 10);
                    b = rand.Next(1, 10);
                    secondMatrix.matrix[j, k] = (double)a / b;
                    a = rand.Next(1, 10);
                    b = rand.Next(1, 10);
                    thirdMatrix.matrix[j, k] = (double)a / b;
                    a = rand.Next(1, 10);
                    b = rand.Next(1, 10);
                    firstVector[j] = (double)a / b;
                }

            }
            */
            // firstMatrix.PrintMatrix();
            // Console.WriteLine("First Matrix End!");

            // secondMatrix.PrintMatrix();
            // Console.WriteLine("Second Matrix End!");

            //thirdMatrix.PrintMatrix();
            // Console.WriteLine("Third Matrix End!");

            
            int M = (2 * N + 1);
            int numbersOfColumns = (M * M - (M - 1) - (M - 1) - 1)*2;
            double[,] table = new double[numbersOfColumns, numbersOfColumns];
            Stan[] tableOfStans = new Stan[numbersOfColumns];
            
            int i = 0;

            for (int j = N; j >= -N; j--)
            {
                for (int k = -N; k <= N; k++)
                {
                    if (!((j == k) || (j == 0) ||(k ==0)))
                    {
                        tableOfStans[i] = new Stan(j, k, 1, true);
                        i++;
                        tableOfStans[i] = new Stan(j, k, 2, true);
                        i++;
                    }
                   
                }
            }

            for (int j = N; j >= -N; j--)
            {
                if (j != 0)
                {
                    tableOfStans[i] = new Stan(j,j,1, true);
                    i++;
                    tableOfStans[i] = new Stan(j, j, 2, true);
                    i++;
                }
            }
            

            double[,] tableForMatrix = new double[numbersOfColumns,numbersOfColumns];
            double[] bVector = new double[numbersOfColumns];
            for (int j = 0; j < numbersOfColumns; j++)
            {
                for (int k = 0; k < numbersOfColumns; k++)
                {
                    tableForMatrix[j, k] = 0;
                }
                bVector[j] = 0;
            }
            

            for (i = 0; i < numbersOfColumns; i++)
            {
                for (int j = 0; j < Stan.numberOfCubeWalls; j++)
                {
                    if (tableOfStans[i].usedItems[j].IsGameFinished())
                    {
                        if (tableOfStans[i].usedItems[j].WhoWin() == 1)
                        {
                            bVector[i] = 1 / 7;
                        }
                        else
                        {
                            bVector[i] = 0;
                        }

                    }
                    else
                    {
                        Console.WriteLine("I = {0}, J = {1}", i, j);
                        tableForMatrix[i, indexOfStanInElements(tableOfStans, numbersOfColumns, tableOfStans[i].usedItems[j])] = -1 / 7;
                        Console.WriteLine("I = {0}, J = {1}", i, j);
                    }
                }
            }

            MyMatrix matrix = new MyMatrix(numbersOfColumns, numbersOfColumns);
            matrix.ComplementMatrix(tableForMatrix);
            //matrix.PrintMatrix();

            MyMatrix.PrintVector(bVector);
           
           /*
            for (int j = 0; j < numbersOfColumns; j++)
            {
                Console.WriteLine("Numer : {3} Stan P{0}({1},{2})",tableOfStans[j].playerMove, tableOfStans[j].firstPlayerField, tableOfStans[j].secondPlayerField, j+1);
            }
            */
            
           
            Console.ReadKey();
        }

        public static int indexOfStanInElements(Stan[] tableOfStans, int numberOfColumn, Stan stan)
        {
            for(int i = 0; i < numberOfColumn; i++)
            {
                if (tableOfStans[i].firstPlayerField == stan.firstPlayerField &&
                    tableOfStans[i].secondPlayerField == stan.secondPlayerField &&
                    tableOfStans[i].playerMove == stan.playerMove)
                {
                    Console.WriteLine("Rzucam {0}", i);
                    return i;
                }
            }
            Console.WriteLine("return -1000");
            return -10000;
        }




        //FILES

        //public static void CleanFiles()
        //{
        //    int i = 1;
        //    while (i <= 3)
        //    {
        //        System.IO.File.WriteAllText(@"C:\Users\pmatusza\Documents\MobaXterm\home\Studia\Algorytmy\Zad2\Zadanie2\Zadanie2\Data\DataRange\DataRangeDouble" + i + ".txt", "");
        //        System.IO.File.WriteAllText(@"C:\Users\pmatusza\Documents\MobaXterm\home\Studia\Algorytmy\Zad2\Zadanie2\Zadanie2\Data\DataRange\DataRangeFloat" + i + ".txt", "");
        //        System.IO.File.WriteAllText(@"C:\Users\pmatusza\Documents\MobaXterm\home\Studia\Algorytmy\Zad2\Zadanie2\Zadanie2\Data\DataRange\DataRangeFactorial" + i + ".txt", "");
        //        i++;
        //    }
        //    System.IO.File.WriteAllText(@"C:\Users\pmatusza\Documents\MobaXterm\home\Studia\Algorytmy\Zad2\Zadanie2\Zadanie2\Data\Results\(AxX)DataResultFactorial.txt", "");

        //}

        //public static void WriteVectorToFile<T>(T[] vector, string name)
        //{
        //    for (int i = 0; i < vector.Length; i++)
        //    {
        //        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\pmatusza\Documents\MobaXterm\home\Studia\Algorytmy\Zad2\Zadanie2\Zadanie2\Data\Results\" + name + ".txt", true))
        //        {
        //            file.WriteLine(String.Format("{0:N3}", vector[i]));
        //        }
        //    }
        //    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\pmatusza\Documents\MobaXterm\home\Studia\Algorytmy\Zad2\Zadanie2\Zadanie2\Data\Results\" + name + ".txt", true))
        //    {
        //        file.Write("*** *** *** *** *** ***\n");
        //    }
        //}


    }
}
