using System;
using System.Collections.Generic;
using Core.Constructions;
using Core.Utility;

namespace Core
{
    public class GameMap
    {
        private Dictionary<string, Room> rooms;
        private Dictionary<string, Hallway> hallways;
        private Dictionary<string, Position> entrances;
        private Dictionary<string, Position> exits;
        private BlockType[,] map;
    }
}
