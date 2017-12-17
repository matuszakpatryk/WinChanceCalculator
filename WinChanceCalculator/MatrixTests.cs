using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WinChanceCalculator
{
    public class MatrixTests
    {
        [Fact]
        public void CheckGaussWithRowChoiceForSquareMatrix()
        {
            double[,] table = {{4, -2, 4, -2}, {3, 1, 4, 2}, {2, 4, 2, 1}, {2, -2, 4, 2}};
            double[] bVector = {8, 7, 10, 2};
            
            MyMatrix doubleMatrix = new MyMatrix(4, 4);
            doubleMatrix.ComplementMatrix(table);
            double[] xVector = doubleMatrix.GaussWithRowChoice(bVector);
            double[] expect = {-1, 2, 3, -2};

            Assert.Equal(expect, xVector);
        }

        [Fact]
        public void CheckSetDefaultVector()
        {
            double[] vector = IterationMethodsOperations.SetDefaultVector(4);
            double[] expect = {0, 0 ,0 ,0};
            
            Assert.Equal(expect,vector);
        }

        [Fact]
        public void CheckCountResultOfActionsForGivenRowOfXVector()
        {
            double[,] matrix1 = {{1, 2}, {2, 1}};
            double[,] matrix2 = { { 4, -1, -0.2, 2 }, { -1, 5, 0, -2 }, { 0.2, 1, 10, -1 }, { 0, -2, -1, 4 } };
            double[] xVector1 = IterationMethodsOperations.SetDefaultVector(2);
            double[] xVector2 = IterationMethodsOperations.SetDefaultVector(4);

            double result1 = IterationMethodsOperations.CountResultOfActionsForGivenRowOfXVector(2, 0, xVector1, matrix1);
            double result2 = IterationMethodsOperations.CountResultOfActionsForGivenRowOfXVector(4, 0, xVector2, matrix2);

            double expect1 = 0;
            double expect2 = 0;


            Assert.Equal(expect1, result1);
            Assert.Equal(expect2, result2);
        }

        [Fact]
        public void CheckValueFromIterationForRow()
        {
            double[,] matrix1 = { { 4, -1, -0.2, 2 }, { -1, 5, 0, -2 }, { 0.2, 1, 10, -1 }, { 0, -2, -1, 4 } };
            double[] bVector1 = {30, 0, -10, 5};
            double[] xVector1 = IterationMethodsOperations.SetDefaultVector(4);

            double result1 = IterationMethodsOperations.ValueFromIterationForRow(xVector1, matrix1, 0, bVector1);
            double result2 = IterationMethodsOperations.ValueFromIterationForRow(xVector1, matrix1, 1, bVector1);
            double result3 = IterationMethodsOperations.ValueFromIterationForRow(xVector1, matrix1, 2, bVector1);
            double result4 = IterationMethodsOperations.ValueFromIterationForRow(xVector1, matrix1, 3, bVector1);

            double expect1 = 7.5;
            double expect2 = 0;
            double expect3 = -1;
            double expect4 = 1.25;

            Assert.Equal(expect1, result1);
            Assert.Equal(expect2, result2);
            Assert.Equal(expect3, result3);
            Assert.Equal(expect4, result4);
        }

        [Fact]
        public void CheckCountXVectorForJacobi()
        {
            double[,] matrix1 = { { 1, 2 }, { 2, 1 } };
            double[,] matrix2 = { { 4, -1, -0.2, 2 }, { -1, 5, 0, -2 }, { 0.2, 1, 10, -1 }, { 0, -2, -1, 4 } };
            double[] xVector1 = IterationMethodsOperations.SetDefaultVector(2);
            double[] xVector2 = IterationMethodsOperations.SetDefaultVector(4);
            double[] bVector1 = { 1, 1 };
            double[] bVector2 = { 30, 0, -10, 5 };

            JacobiOperations.CountXVector(xVector1,  matrix1, bVector1);
            JacobiOperations.CountXVector(xVector2,  matrix2, bVector2);

            double[] expect1 = { 1, 1 };
            double[] expect2 = { 6.825, 2, -1.025, 1 };

            //Assert.Equal(expect1, xVector1);
            Assert.Equal(expect2, xVector2);
        }

        [Fact]
        public void CheckJacobi()
        {
            double[,] table1 = {{1, 2}, {2, 1}};
            double[,] table2 =
            {
                {10.0, -1,2, 0},
                {-1, 11.0, -1.0, 3},
                {2.0, -1.0, 10.0, -1},
                {0, 3, -1, 8 }
            };

            double[] bVector1 = {1, 1};
            double[] bVector2 = { 6, 25.0, -11.0, 15 };
            MyMatrix doubleMatrix1 = new MyMatrix(2,2);
            MyMatrix doubleMatrix2 = new MyMatrix(4,4);

            doubleMatrix1.ComplementMatrix(table1);
            doubleMatrix2.ComplementMatrix(table2);

            double[] xVector1 = doubleMatrix1.Jacobi(bVector1, 1);
            double[] xVector2 = doubleMatrix2.Jacobi(bVector2, 8);

            double[] expect1 = { 1, 1 };
            double[] expect2 = {1.0006, 1.9987, -0.9990, 0.9989};


            Assert.Equal(expect1, xVector1);
            Assert.Equal(expect2[0], Math.Round(xVector2[0], 4));
            Assert.Equal(expect2[1], Math.Round(xVector2[1], 4));
            Assert.Equal(expect2[2], Math.Round(xVector2[2], 4));
            Assert.Equal(expect2[3], Math.Round(xVector2[3], 4));

        }

        [Fact]
        public void CheckSeidel()
        {
            double[,] table =
            {
                {10.0, -1,2, 0},
                {-1, 11.0, -1.0, 3},
                {2.0, -1.0, 10.0, -1},
                {0, 3, -1, 8 }
            };
            double[] bVector1 = { 6, 25.0, -11.0, 15 };
            MyMatrix doubleMatrix1 = new MyMatrix(4, 4);
            doubleMatrix1.ComplementMatrix(table);

            double[] xVector1 = doubleMatrix1.Seidel(bVector1, 5);

            double[] expect1 = { 1.0001, 2.0000, -1.0000, 1.0000 };


            Assert.Equal(expect1[0],Math.Round(xVector1[0], 4));
            Assert.Equal(expect1[1], Math.Round(xVector1[1], 4));
            Assert.Equal(expect1[2], Math.Round(xVector1[2], 4));
            Assert.Equal(expect1[3], Math.Round(xVector1[3], 4));
        }
    
    }
}
