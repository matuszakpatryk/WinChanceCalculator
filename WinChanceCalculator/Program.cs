using System;

namespace WinChanceCalculator
{
    class Program
    {
        public static int size = 10;
        public static int N = 2;

        static void Main(string[] args)
        {
            double[] result = new double[3];
            double[,] cubeValues = new double[,] { { -1, 1/3 }, { 0, 1/3 }, { 1, 1/3 } };
            double gaussSum = 0, jacobiSum = 0, seidelSum = 0;
            result = ComputeAll();
            gaussSum += result[0];
            jacobiSum += result[1];
            seidelSum += result[2];
            Console.WriteLine("Wyniki dla N={0}, Kostki o {1} scianach", N, Stan.cubeValues.Length / 2);
            Console.WriteLine("Gauss result: {0}", gaussSum);
            Console.WriteLine("Jacobi result: {0}", jacobiSum);
            Console.WriteLine("Seidel result: {0}", seidelSum);

            double firstPlayerWins = 0, secondPlayerWins = 0;
            for (int i = 0; i < 100; i++)
            {
                Random rand = new Random();
                int x = rand.Next(3) - 1;
                Console.WriteLine("x={0}", x);
                int game = GameSymulation(2, -2, 3, cubeValues);
                Console.WriteLine("Game {0}: game={1}", i, game);
                if (game == 1)
                {
                    firstPlayerWins++;
                }
                else
                {
                    secondPlayerWins++;
                }
            }

            Console.WriteLine("Prawdopodobienstwo wygrania gracza 1: {0}", firstPlayerWins / 100);


            Console.ReadKey();
        }

        public static int GameSymulation(int firstPlayerStartField, int secondPlayerStartField, int cubeSize, double[,] cubeValues)
        {
            int firstPlayerField = firstPlayerStartField;
            int secondPlayerField = secondPlayerStartField;
            while (true)
            {
                int cube = MyRandom(cubeValues);
                Console.WriteLine("Pole 1: {0}, rzut: {1}", firstPlayerField, cube);
                firstPlayerField = WhichFieldForPlayer(firstPlayerField, cube);
                if (firstPlayerField == 0)
                {
                    return 1;
                }
                cube = MyRandom(cubeValues);
                Console.WriteLine("Pole 2: {0}, rzut: {1}", secondPlayerField, cube);
                secondPlayerField = WhichFieldForPlayer(secondPlayerField, cube);
                if (secondPlayerField == 0)
                {
                    return 2;
                }
            }
        }

        public static int WhichFieldForPlayer(int playerField, int cubeValue)
        {
            int whichFieldForPlayer = playerField;
            if (whichFieldForPlayer + cubeValue < -Program.N)
            {
                whichFieldForPlayer = Program.N - ((-(whichFieldForPlayer + cubeValue) - Program.N) - 1);
            }
            else if (whichFieldForPlayer + cubeValue > Program.N)
            {
                whichFieldForPlayer = -Program.N + (((whichFieldForPlayer + cubeValue) - Program.N) - 1);
            }
            else
            {
                whichFieldForPlayer += cubeValue;
            }

            return whichFieldForPlayer;
        }

        public static int MyRandom (double[,] cubeValues)
        {
            Random rand = new Random();
            int x = rand.Next(3) - 1;
            //Console.WriteLine("X = {0}", x);
            return x;
            //if ( (x >= 0) && (x <= cubeValues[0, 1]*100) )
            //{
            //    return (int)cubeValues[0, 0];
            //}
            //else if ( (x > cubeValues[0, 1] * 100) && (x <= cubeValues[1, 1] * 100) )
            //{
            //    return (int)cubeValues[1, 0];
            //}
            //else
            //{
            //    return (int)cubeValues[2, 0];
            //}
        //    if (x == 0)
        //    {
        //        return -1;
        //    }
        //    else if ( x == 1 )
        //    {
        //        return 0;
        //    }
        //    else
        //    {
        //        return 1;
        //    }
        }

        public static double[] ComputeAll()
        {
            Stan[] tableOfStans = CreateTableOfStans();
            int numberOfColumns = tableOfStans.Length;
            FillTableOfStans(tableOfStans);

            double[,] tableForMatrix = new double[numberOfColumns, numberOfColumns];
            double[] bVector = new double[numberOfColumns];
            double[] resultVector = new double[3]; 

            FillTableForMatrixAndBVectorWithZeros(tableForMatrix, bVector, numberOfColumns);
            FillTableForMatrixWithStans(tableOfStans, numberOfColumns, tableForMatrix, bVector);

            MyMatrix matrix = new MyMatrix(numberOfColumns, numberOfColumns);
            matrix.ComplementMatrix(tableForMatrix);

            double[] gVector = matrix.GaussWithRowChoice((double[])bVector.Clone());
            resultVector[0] = gVector[0];
            double[] jVector = matrix.Jacobi((double[])bVector.Clone(),100);
            resultVector[1] = jVector[0];
            double[] sVector = matrix.Seidel((double[])bVector.Clone(),100);
            resultVector[2] = sVector[0];
            return resultVector;

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
                            bVector[i] = (double) Stan.cubeValues[j,1];
                        }
                    }
                    else
                    {
                        int columnWitchHoldStan = FindIndexOfStanInElements(tableOfStans, numberOfColumns, usedItemsForGivenStan[j]);
                        tableForMatrix[i, columnWitchHoldStan] = (double)Stan.cubeValues[j, 1];
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
