using System;
namespace Core.Utility
{
    public struct Position
    {
        private int x;
        private int y;
        private bool iniatilized;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.iniatilized = true;
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }

        public bool isInitialized()
        {
            return iniatilized;
        }

        public override bool Equals(object obj)
        {
            if(!(obj is Position))
            {
                return false;
            }
            Position otherPosition = (Position)obj;
            return Equals(otherPosition);
        }

        public override int GetHashCode()
        {
            return this.x + this.y;
        }

        public bool Equals(Position other)
        {
            return x == other.x && y == other.y;
        }
    }

    public struct Section
    {
        private int xOff;
        private int yOff;
        private int width;
        private int height;

        public Section(int x, int y, int width, int height)
        {
            xOff = x;
            yOff = y;
            this.width = width;
            this.height = height;
        }

        public int getXOff(){
            return xOff;
        }

        public int getYOff()
        {
            return yOff;
        }

        public int getWidth()
        {
            return width;
        }

        public int getHeight()
        {
            return height;
        }

        public void setWidth(int newWidth)
        {
            this.width = newWidth;
        }

        public void setHeight(int newHeigth)
        {
            this.height = newHeigth;
        }
    }

    public struct WalkableTile
    {
        public const string Wall = "Wall";
        public const string Floor = "Floor";

        private string type;

        public WalkableTile(string type)
        {
            this.type = type;
        }

        public bool isWalkable()
        {
            return !(this.type == Wall || this.type == Floor);
        }
    }
}
