using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinChanceCalculator
{
    class Stan
    {
        public int firstPlayerField { get; set; }
        public int secondPlayerField { get; set; }
        public int playerMove { get; set; }
        public Stan[] usedItems { get; set; }
        public static double[,] cubeValues { get; set; }
        public static int numberOfCubeWalls = 3;

        public Stan(int firstPlayerField, int secondPlayerField, int playerMove, bool flag)
        {
            this.firstPlayerField = firstPlayerField;
            this.secondPlayerField = secondPlayerField;
            this.playerMove = playerMove;
            usedItems = new Stan[numberOfCubeWalls];
            cubeValues = new double[,] { { -1, 1.0/3 }, { 0, 1.0/3 }, { 1, 1.0/3 } };
            if (flag == true)
            {
                SetUsedItemTable();
            }
        }


        public void SetUsedItemTable()
        {
            int i = 0;
            for (int j = 0; j < (int)(cubeValues.Length / 2); j++, i++)
            {
                int whichFieldForPlayer;
                if (playerMove == 1)
                {
                    whichFieldForPlayer = firstPlayerField;
                }
                else
                {
                    whichFieldForPlayer = secondPlayerField;
                }

                if (whichFieldForPlayer + cubeValues[j, 0] < -Program.N)
                {
                    whichFieldForPlayer = Program.N - ((-(whichFieldForPlayer + (int)cubeValues[j, 0]) - Program.N) - 1);
                }
                else if (whichFieldForPlayer + cubeValues[j, 0] > Program.N)
                {
                    whichFieldForPlayer = -Program.N + (((whichFieldForPlayer + (int)cubeValues[j, 0]) - Program.N) - 1);
                }
                else
                {
                    whichFieldForPlayer += (int)cubeValues[j, 0];
                }

                if (playerMove == 1)
                {
                    usedItems[i] = new Stan(whichFieldForPlayer, secondPlayerField, 2, false);
                }
                else
                {
                    usedItems[i] = new Stan(firstPlayerField, whichFieldForPlayer, 1, false);
                }
            }
        }

        public bool IsGameFinished()
        {
            if (firstPlayerField == 0 || secondPlayerField == 0)
            {
                return true;
            }

            return false;
        }

        public int WhoWin()
        {
            if (firstPlayerField == 0)
            {
                return 1;
            }

            return 2;
        }
    }
}
