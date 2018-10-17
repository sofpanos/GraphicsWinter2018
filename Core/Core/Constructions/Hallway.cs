using System;
using System.Collections.Generic;
using Core.Utility;

namespace Core.Constructions
{
    public class Hallway
    {
        public const int MIN_SIZE = 3;

        private List<Position> floorPositions;
        private List<Position> wallPositions;
        private Position? intersection;
        private string id = "";

        public Hallway(string id, List<Position> path, List<Position> wall)
        {
            this.floorPositions = path;
            this.wallPositions = wall;
        }

        public List<Position> getPath()
        {
            return floorPositions;
        }

        public List<Position> getWallPosition()
        {
            return wallPositions;
        }

        public void setFloorPositions(List<Position> path)
        {
            this.floorPositions = path;
        }

        public void setWallPosition(List<Position> wallPositions)
        {
            this.wallPositions = wallPositions;
        }

        public string getID()
        {
            return id;
        }

        public bool isWall(Position pos)
        {
            return wallPositions.Contains(pos);
        }

        public bool isFloor(Position pos)
        {
            return floorPositions.Contains(pos);
        }

        public Position? getIntersection()
        {
            return intersection;
        }

        public void setIntersection(Position intersection)
        {
            if (this.intersection != null)
            {
                this.floorPositions.Remove((Position)this.intersection);
                this.wallPositions.Add((Position)this.intersection);
            } 
            this.intersection = intersection;
            this.floorPositions.Add((Position)this.intersection);
            this.wallPositions.Add((Position)this.intersection);

        }
    }
}
