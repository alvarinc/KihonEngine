using KihonEngine.Core.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KihonEngine.Core.Client
{
    internal static class ClientRenderer
    {
        public static void Render(GameState _gameState, string playerId)
        {
            var player = _gameState.Players.Values.First(x => x.Guid == playerId);
            Console.WriteLine($"connection {player.PeerId} : {player.Name}");

            for (int column = _gameState.Map.MinZ; column <= _gameState.Map.MaxZ; column++)
            {
                for (int row = _gameState.Map.MinX; row <= _gameState.Map.MaxX; row++)
                {
                    if (_gameState.Players.Values.Any(p => -p.Position.Z == column && p.Position.X == row))
                    {
                        if (-player.Position.Z == column && player.Position.X == row)
                        {
                            Console.Write("X");
                        }
                        else
                        {
                            Console.Write("@");
                        }
                    }
                    else if (column == _gameState.Map.MinZ || column == _gameState.Map.MaxZ || row == _gameState.Map.MinX || row == _gameState.Map.MaxX)
                    {
                        Console.Write("*");
                    }
                    else
                    {
                        Console.Write("_");
                    }

                    if (row == _gameState.Map.MaxX)
                    {
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}
