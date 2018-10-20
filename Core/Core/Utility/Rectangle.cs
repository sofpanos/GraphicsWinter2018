using System;

namespace Core.Utility
{
    public class Rectangle
    {
        private int x;
        private int y;
        private int width = 0;
        private int height = 0;

        public Rectangle(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public Rectangle(Section section)
        {
            this.x = section.getXOff();
            this.y = section.getYOff();
            this.width = section.getWidth();
            this.height = section.getHeight();
        }

        public Rectangle(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int getX()
        {
            return x;
        }

        public void setX(int x)
        {
            this.x = x;
        }

        public int getY()
        {
            return y;
        }

        public void setY(int y)
        {
            this.y = y;
        }

        public int getWidth()
        {
            return width;
        }

        public void setWidth(int width)
        {
            this.width = width;
        }

        public int getHeight()
        {
            return height;
        }

        public void setHeight(int height)
        {
            this.height = height;
        }

        public Position[] getCorners()
        {
            Position[] corners = {new Position(x,y), new Position(x + width - 1,y), new Position(x + width - 1, y + height - 1), new Position(x, y + height - 1)};
            return corners;
        }

        public bool contains(Position pos)
        {
            bool result = this.x <= pos.getX() && pos.getX() < this.x + width;
            result = result && this.y <= pos.getY() && pos.getY() < this.y + height;
            return result;
        }

        public bool containsFullRect(Rectangle otherRect)
        {
            foreach(Position corner in otherRect.getCorners()){
                if (!contains(corner))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
