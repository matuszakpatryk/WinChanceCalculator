﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinChanceCalculator
{
    class Program
    {
        public static int size = 10;

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

            double[,] table =
            {
                {10.0, -1,2, 0},
                {-1, 11.0, -1.0, 3},
                {2.0, -1.0, 10.0, -1},
                {0, 3, -1, 8 }
            };
            double[] vector = { 6, 25.0, -11.0, 15 };

            MyMatrix test = new MyMatrix( 4,4);
            test.ComplementMatrix(table);
            vector = test.Seidel(vector, 5);
            
            MyMatrix.PrintVector(vector);

            Console.ReadKey();
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
