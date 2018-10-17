using Core.Constructions;
using Core.Utility;
using System;
using System.Collections.Generic;

namespace Core.Factories
{
    public class HallwayFactory
    {
        public static Hallway getHallway(string id, Room previousRoom, Hallway previousHall, Room nextRoom, GameMap map)
        {
            throw new NotImplementedException();
        }

        private static Hallway getHallway(string hallwayID, Position entrance, Position exit, WalkableTile[,] grid)
        {
            throw new NotImplementedException();
        }

        
    }
}
