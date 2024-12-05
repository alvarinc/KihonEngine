using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KihonEngin.Core
{
    internal class GameState
    {
        public GameState() 
        {
            Map = MapDescription.DefaultMap;
        }

        public Dictionary<int, Player> Players { get; set; } = new();
        
        public MapDescription Map { get; set; } = new();
    }

    public class MapDescription
    {
        public int MaxX { get; set; }
        public int MaxY { get; set; }
        public int MaxZ { get; set; }
        public int MinX { get; set; }
        public int MinY { get; set; }
        public int MinZ { get; set; }

        public static MapDescription DefaultMap => new MapDescription
        {
            MaxX = 20,
            MaxY = 20,
            MaxZ = 20,
            MinX = -20,
            MinY = -20,
            MinZ = -20
        };
    }

    internal class Player
    {
        public int Id { get; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Player(int id)
        {
            Id = id;
            X = 0;
            Y = 0;
            Z = 0;
        }

        public string GetPosition()
        {
            return $"{X:0.0},{Y:0.0},{Z:0.0}";
        }
    }
}
