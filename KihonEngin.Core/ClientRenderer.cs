using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KihonEngin.Core
{
    internal static class ClientRenderer
    {
        public static void Render(int x, int y, int z)
        {
            for (int column = -10; column <= 10; column++)
            {
                for (int row = -10; row <= 10; row++)
                {
                    if (-z == column && x == row)
                    {
                        Console.Write("X");
                    }
                    else if (column == -10 || column == 10 || row == -10 || row == 10)
                    {
                        Console.Write("*");
                    }
                    else
                    {
                        Console.Write("_");
                    }

                    if (row == 10)
                    {
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}
