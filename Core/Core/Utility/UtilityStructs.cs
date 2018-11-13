using System;
namespace Core.Utility
{
    public struct Position
    {
        private int x;
        private int y;

        public Position(int aX, int aY)
        {
            x = aX;
            y = aY;
        }

        public int getX() 
        {
            return x;
        }

        public int getY() 
        {
            return y;
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
            return x + y;
        }

        public bool Equals(Position other)
        {
            return x == other.x && y == other.y;
        }
    }

    public struct Section
    {
        private int x;
        private int y;
        private int width;
        private int height;

        public Section(int aX, int aY, int aWidth, int aHeight)
        {
            x = aX;
            y = aY;
            width = aWidth;
            height = aHeight;
        }
        
        public int getX() 
        {
            return x;
        }
        public int getY()
        {
            return y;
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

    
}
