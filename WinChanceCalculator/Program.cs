using System;
using System.Diagnostics;

namespace WinChanceCalculator
{
    class Program
    {
        public static int size = 1000000;
        public static int N = 15;
        public static double seidelTime, buildMatrixTime;

        static void Main(string[] args)
        {
            Random rand = new Random();
            CleanFiles();
            double[] result = new double[3];
            double[,] cubeValues = new double[,] { { -3, 1.0 / 7 }, { -2, 1.0 / 7 }, { -1, 1.0 / 7 }, { 0, 1.0 / 7 }, { 1, 1.0 / 7 }, { 2, 1.0 / 7 }, { 3, 1.0 / 7 } };
            double gaussSum = 0, jacobiSum = 0, seidelSum = 0;
            result = ComputeAll();

            int firstPlayerWins = 0, secondPlayerWins = 0;
        }

        public static int GameSymulation(int firstPlayerStartField, int secondPlayerStartField, int cubeSize, double[,] cubeValues, Random rand)
        {
            int firstPlayerField = firstPlayerStartField;
            int secondPlayerField = secondPlayerStartField;
            while (true)
            {
                int cube = MyRandom(cubeValues, rand);
                firstPlayerField = WhichFieldForPlayer(firstPlayerField, cube);
                if (firstPlayerField == 0)
                {
                    return 1;
                }
                cube = MyRandom(cubeValues, rand);
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

        public static int MyRandom (double[,] cubeValues, Random rand)
        {
            int[] test = new int[100];
            int iterator = 0;
            for (int i = 0; i < cubeValues.Length / 2; i++)
            {               
                if (i != (cubeValues.Length / 2) - 1)
                {
                    int temp = (int)(cubeValues[i, 1] * 100);
                    for (int j = iterator; j < (int)(cubeValues[i, 1] * 100) + iterator; j++)
                    {

                        test[j] = (int)cubeValues[i, 0];
                    }
                    iterator += (int)(cubeValues[i, 1] * 100);
                }
                else
                {
                    int temp = (int)(cubeValues[i, 1] * 100);
                    for (int j = iterator; j < 100; j++)
                    {
                        test[j] = (int)cubeValues[i, 0];
                    }
                }
            }
            
            int x = rand.Next(100);
            return test[x];
        }

        public static double[] ComputeAll()
        {
            Stan[] tableOfStans = CreateTableOfStans();
            int numberOfColumns = tableOfStans.Length;

            var stopwatch = new Stopwatch();
            stopwatch.Reset();
            stopwatch.Start();
            FillTableOfStans(tableOfStans);

            double[,] tableForMatrix = new double[numberOfColumns, numberOfColumns];
            double[] bVector = new double[numberOfColumns];
            double[] resultVector = new double[3];

            double[,] matrixPoints = new double[numberOfColumns * Stan.numberOfCubeWalls + numberOfColumns, 3];
            double[,] vectorPoints = new double[numberOfColumns, 3];

            FillTableForMatrixAndBVectorWithZeros(tableForMatrix, bVector, numberOfColumns);
            FillTableForMatrixWithStans(tableOfStans, numberOfColumns, tableForMatrix, bVector, matrixPoints, vectorPoints);

            MyMatrix matrix = new MyMatrix(numberOfColumns, numberOfColumns);
            matrix.ComplementMatrix(tableForMatrix);

            buildMatrixTime = stopwatch.Elapsed.TotalMilliseconds;

            matrix.WriteMatrixToFile();
            WriteVectorToFile((double[])bVector.Clone());
            MyMatrix.PrintVector(bVector);
            WriteVectorWithoutZero((double[])bVector.Clone());

            stopwatch.Reset();
            stopwatch.Start();
            double[] sVector = matrix.Seidel((double[])bVector.Clone(), 100);
            stopwatch.Stop();
            resultVector[2] = sVector[0];
            seidelTime = stopwatch.Elapsed.TotalMilliseconds;
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
            double[,] tableForMatrix, double[] bVector, double[,] matrixPoints, double[,] vectorPoints)
        {
            int z = 0;
            for (int i = 0; i < numberOfColumns; i++)
            {
                matrixPoints[z, 0] = i;
                matrixPoints[z, 1] = i;
                matrixPoints[z, 2] = 1;
                z++;
                for (int j = 0; j < Stan.numberOfCubeWalls; j++)
                {
                    Stan[] usedItemsForGivenStan = tableOfStans[i].usedItems;

                    if (usedItemsForGivenStan[j].IsGameFinished())
                    {
                        if (usedItemsForGivenStan[j].WhoWin() == 1)
                        {
                            bVector[i] += (double)Stan.cubeValues[j, 1];
                        }
                    }
                    else
                    {
                        int columnWitchHoldStan = FindIndexOfStanInElements(tableOfStans, numberOfColumns, usedItemsForGivenStan[j]);
                        tableForMatrix[i, columnWitchHoldStan] += (double)Stan.cubeValues[j, 1];

                        bool isColumnRepeat = false;
                        int repeatedColumn = -1;
                        for (int k = 0; k < z; k++)
                        {
                            if (Math.Abs(matrixPoints[k, 0] - i) < 0.0001 && Math.Abs(matrixPoints[k, 1] - columnWitchHoldStan) < 0.00001)
                            {
                                isColumnRepeat = true;
                                repeatedColumn = k;
                            }
                        }

                        if (!isColumnRepeat)
                        {
                            matrixPoints[z, 0] = i;
                            matrixPoints[z, 1] = columnWitchHoldStan;
                            matrixPoints[z, 2] = (double)Stan.cubeValues[j, 1];
                            z++;
                        }
                        else
                        {
                            matrixPoints[repeatedColumn, 2] += (double)Stan.cubeValues[j, 1];
                        }
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

        public static void CleanFiles()
        {
            System.IO.File.WriteAllText(@"C:\Users\Patryk\Desktop\Data\DataRange.txt", "");
            System.IO.File.WriteAllText(@"C:\Users\Patryk\Desktop\Data\DataRangeNew.txt", "");
            System.IO.File.WriteAllText(@"C:\Users\Patryk\Desktop\Data\VectorWithoutZeros.txt", "");
        }

        public static void WriteVectorToFile(double[] vector)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Patryk\Desktop\Data\DataRange.txt", true))
            {
                for (int i = 0; i < vector.Length; i++)
                {
                    file.WriteLine(String.Format("{0:N16}", vector[i]));
                }
                file.Write("*** *** *** *** *** ***\n");
            }           
        }

        public static void WriteDataToFile(string name, double gauss, double jacobi, double seidel, double gaussTime, double jacobiTime, double seidelTime, double symulation, double symulationSpan, double buildTime)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Patryk\Desktop\Data\" + name + ".txt", true))
            {
                file.WriteLine("Data for N={0}, CubeSize={1} symetrical", N, 7);
                file.WriteLine("Build Matrix Time Result: {0}", buildMatrixTime);
                file.WriteLine("Gauss Result: {0}", gauss);
                file.WriteLine("Gauss Time: {0}", gaussTime);
                file.WriteLine("");
                file.WriteLine("Jacobi Result: {0}", jacobi);
                file.WriteLine("Jacobi Time: {0}", jacobiTime);
                file.WriteLine("");
                file.WriteLine("Seidel Result: {0}", seidel);
                file.WriteLine("Seidel Time: {0}", seidelTime);
                file.WriteLine("");
                //file.WriteLine("Symulation Result: {0}", symulation);
                //file.WriteLine("Symulation Time: {0}", symulationTime);
                file.WriteLine("");
            }
        }

        public static void WriteVectorWithoutZero(double[] vector)
        {
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"C:\Users\Patryk\Desktop\Data\VectorWithoutZeros.txt", true))
            {
                for (int i = 0; i < vector.Length; i++)
                {
                    if (!MyMatrix.CheckIsDoubleZero(vector[i]))
                    {
                        file.WriteLine(String.Format("{0} {1:N16}", i, vector[i]));
                    }

                }
            }
        }

        private static void WriteMatixWithoutZerosToFile(double[,] test)
        {
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"C:\Users\Patryk\Desktop\Data\DataRangeNew.txt", true))
            {

                for (int i = 0; i < test.GetLength(0); i++)
                {
                    if (!MyMatrix.CheckIsDoubleZero(test[i, 2]))
                    {
                        file.Write(String.Format("{0} {1} {2:N16}", test[i, 0], test[i, 1], test[i, 2]));
                        file.Write("\n");
                    }

                }
            }
        }


    }
}
