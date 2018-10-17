using System;
using System.Collections.Generic;
using Core.Utility;
namespace Core.Factories
{
    public class MapFactory
    {
        public static GameMap getNewGameMap(int width, int height)
        {
            BlockType[,] newMap = new BlockType[width, height];
            return new GameMap();
        }

        public static GameMap getNewGameMap(int width, int height, int seed)
        {

            return new GameMap();
        }
    }
}
