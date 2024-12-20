using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KihonEngine.Core.State
{
    public class GameState
    {
        public GameState()
        {
            Map = MapDescription.DefaultMap;
        }

        public Dictionary<int, PlayerState> Players { get; set; } = new();

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
            MaxX = 10,
            MaxY = 10,
            MaxZ = 10,
            MinX = -10,
            MinY = -10,
            MinZ = -10
        };
    }

    public class PlayerState
    {
        public int PeerId { get; }
        public string Guid { get; set; }
        public string Name { get; set; }
        public Position Position { get; set; }

        public PlayerState(int peerId)
        {
            PeerId = peerId;
            Position = new Position();
        }
    }

    public class Position
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
}
