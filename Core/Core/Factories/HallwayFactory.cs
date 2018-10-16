using Core.Constructions;
using Core.Utility;
using System;
using System.Collections.Generic;

namespace Core.Factories
{
    public class HallwayFactory
    {
        public static Hallway getHallway(string hallwayID, Position entrance, Position exit, WalkableTile[,] grid)
        {
            List<Position> path = new List<Position>();
            List<Position> wall = new List<Position>();

            return new Hallway(hallwayID, path, wall);
        }
    }
}
