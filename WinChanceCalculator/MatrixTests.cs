using System;
using System.Collections.Generic;
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
            double[] result = {-1, 2, 3, -2};

            Assert.Equal(result, xVector);
        }
    }
}
