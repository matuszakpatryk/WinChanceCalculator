using System;

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



            Stan[] tableOfStans = CreateTableOfStans();
            int numberOfColumns = tableOfStans.Length;

            

            FillTableOfStans(tableOfStans);
            //PrintTableOfStans(tableOfStans);
            //PrintUsedStansForStan(tableOfStans[7]);

            double[,] tableForMatrix = new double[numberOfColumns,numberOfColumns];
            double[] bVector = new double[numberOfColumns];

            FillTableForMatrixAndBVectorWithZeros(tableForMatrix, bVector, numberOfColumns);

            FillTableForMatrixWithStans(tableOfStans, numberOfColumns, tableForMatrix, bVector);
            
           

            MyMatrix matrix = new MyMatrix(numberOfColumns, numberOfColumns);
            matrix.ComplementMatrix(tableForMatrix);
            matrix.PrintMatrix();

            MyMatrix.PrintVector(bVector);

           
            double[] xVector = matrix.GaussWithRowChoice(bVector);
            Console.WriteLine("Wynik: {0}", xVector[0]);

           
            Console.ReadKey();
        }

        public static int FindIndexOfStanInElements(Stan[] tableOfStans, int numberOfColumn, Stan stan)
        {
            for(int i = 0; i < numberOfColumn; i++)
            {
                if (tableOfStans[i].firstPlayerField == stan.firstPlayerField &&
                    tableOfStans[i].secondPlayerField == stan.secondPlayerField &&
                    tableOfStans[i].playerMove == stan.playerMove)
                {
                    return i;
                }
            }
            return -10000;
        }

        public static void FillTableForMatrixWithStans(Stan[] tableOfStans, int numberOfColumns,
            double[,] tableForMatrix, double[] bVector)
        {
            for (int i = 0; i < numberOfColumns; i++)
            {
                for (int j = 0; j < Stan.numberOfCubeWalls; j++)
                {
                    Stan[] usedItemsForGivenStan = tableOfStans[i].usedItems;

                    if (usedItemsForGivenStan[j].IsGameFinished())
                    {
                        if (usedItemsForGivenStan[j].WhoWin() == 1)
                        {
                            bVector[i] = (double) 1 / Stan.numberOfCubeWalls;
                        }
                    }
                    else
                    {
                        int columnWitchHoldStan =
                            FindIndexOfStanInElements(tableOfStans, numberOfColumns, usedItemsForGivenStan[j]);
                        tableForMatrix[i, columnWitchHoldStan] = (double) 1 / Stan.numberOfCubeWalls;
                    }
                }
            }
        }


        public static Stan[] CreateTableOfStans()
        {
            int m = (2 * N + 1);
            int numbersOfColumns = (m * m - (m - 1) - (m - 1) - 1) * 2;
            return new Stan[numbersOfColumns];

        }


        public static void FillTableOfStans(Stan[] tableOfStans)
        {
            int i = 0;

            for (int j = N; j >= -N; j--)
            {
                for (int k = -N; k <= N; k++)
                {
                    if (!((j == k) || (j == 0) || (k == 0)))
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
                    tableOfStans[i] = new Stan(j, j, 1, true);
                    i++;
                    tableOfStans[i] = new Stan(j, j, 2, true);
                    i++;
                }
            }
        }


        public static void FillTableForMatrixAndBVectorWithZeros(double[,] tableForMatrix, double[] bVector,int numbersOfColumns)
        {
            for (int i = 0; i < numbersOfColumns; i++)
            {
                for (int j = 0; j < numbersOfColumns; j++)
                {
                    if (i == j)
                    {
                        tableForMatrix[i, j] = 1;
                    }
                    else
                    {
                        tableForMatrix[i, j] = 0;
                    }
                    
                }
                bVector[i] = 0;
            }
        }

        public static void PrintTableOfStans(Stan[] tableOfStans)
        {
            Console.WriteLine("Tabela stanów");
            for (int i = 0; i < tableOfStans.Length; i++)
            {
                Console.WriteLine("Stan {0}: P{1}({2},{3})", i + 1, tableOfStans[i].playerMove, tableOfStans[i].firstPlayerField, tableOfStans[i].secondPlayerField);
            }
        }

        public static void PrintUsedStansForStan(Stan stan)
        {
            Stan[] usedItemsForGivenStan = stan.usedItems;

            Console.WriteLine("Używane stany przez dany stan");
            for (int i = 0; i < Stan.numberOfCubeWalls; i++)
            {
                Console.WriteLine("Stan {0}: P{1}({2},{3})", i + 1,usedItemsForGivenStan[i].playerMove, usedItemsForGivenStan[i].firstPlayerField, usedItemsForGivenStan[i].secondPlayerField);
            }
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
