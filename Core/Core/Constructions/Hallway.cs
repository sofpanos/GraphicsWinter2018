using System;
using System.Collections.Generic;
using Core.Utility;

namespace Core.Constructions
{
    public class Hallway
    {
        public const int MIN_SIZE = 3;

        private List<Position> path;
        private List<Position> wall;
        private string id = "";

        public Hallway(string id, List<Position> path, List<Position> wall)
        {
            this.path = path;
            this.wall = wall;
        }

        public List<Position> getPath()
        {
            return path;
        }

        public List<Position> getWallPosition()
        {
            return wall;
        }

        public void setPath(List<Position> path)
        {
            this.path = path;
        }

        public void setWallPosition(List<Position> wallPositions)
        {
            this.wall = wallPositions;
        }

        public string getID()
        {
            return id;
        }

        public bool isWall(Position pos)
        {
            return wall.Contains(pos);
        }

        public bool isPath(Position pos)
        {
            return path.Contains(pos);
        }
    }
}
