using System;
using Core.Utility;
using Core.Constructions;
using System.Collections.Generic;

namespace Core.Factories
{
    public class RoomFactory
    {
        public RoomFactory()
        {
        }

        public static Room getRoom(Section mapSection, string id, Random random)
        {
            int numberOfRects = random.Next(1, 8);
            Rectangle[] roomRectangles = createRoomRectangles(mapSection, numberOfRects, random);
            Room newRoom = getRoom(mapSection, roomRectangles, id);
            
            return newRoom;
        }

        private static Rectangle[] createRoomRectangles(Section mapSection, int numberOfRectangles, Random random)
        {
            Rectangle[] roomRects = new Rectangle[numberOfRectangles];
            for (int rect = 0; rect < numberOfRectangles; rect++)
            {
                bool validRect = false;
                do
                {
                    int rectX = 0;
                    int rectY = 0;
                    int rectWidth = 0;
                    int rectHeight = 0;

                    //create x coordinate for the rectangle
                    int minX = mapSection.getX() + Room.MIN_DISTANCE_FROM_SECTION_EDGE + 1;
                    int maxX = mapSection.getX() + mapSection.getWidth() - Room.MIN_DISTANCE_FROM_SECTION_EDGE - Room.MIN_WIDTH_HEIGHT;
                    rectX = random.Next(minX, maxX);
                    
                    //create y coordinate for the rectangle
                    int minY = mapSection.getY() + Room.MIN_DISTANCE_FROM_SECTION_EDGE + 1;
                    int maxY = mapSection.getY() + mapSection.getHeight() - Room.MIN_DISTANCE_FROM_SECTION_EDGE - Room.MIN_WIDTH_HEIGHT;
                    rectY = random.Next(minY, maxY);
                    
                    //create width for the rectangle
                    if (rectX == maxX - 1)
                    {
                        rectWidth = Room.MIN_WIDTH_HEIGHT;
                    }
                    else
                    {
                        int maxWidth = maxX - rectX + Room.MIN_WIDTH_HEIGHT;
                        rectWidth = random.Next(Room.MIN_WIDTH_HEIGHT, maxWidth);
                    }

                    //create height for the rectangle
                    if (Room.MIN_WIDTH_HEIGHT == maxY - 1)
                    {
                        rectHeight = Room.MIN_WIDTH_HEIGHT;
                    }
                    else
                    {
                        int maxHeight = maxY - rectY + Room.MIN_WIDTH_HEIGHT;
                        rectHeight = random.Next(Room.MIN_WIDTH_HEIGHT, maxHeight);
                    }

                    Rectangle roomRect = new Rectangle(rectX, rectY, rectWidth, rectHeight);

                    if (rect == 0)
                    {
                        roomRects[rect] = roomRect;
                        validRect = true;
                    }
                    else
                    {
                        if (!containedByOtherRect(roomRects, roomRect, rect) && atLeastOneCornerContained(roomRects, roomRect, rect))
                        {
                            roomRects[rect] = roomRect;
                            validRect = true;
                        }
                    }
                } while (!validRect);
            }
            return roomRects;
        }

        private static bool containedByOtherRect(Rectangle[] roomRectangles, Rectangle rectangle, int numberOfRectsDone)
        {
            for (int rect = 0; rect < numberOfRectsDone; rect++)
            {
                if (roomRectangles[rect].containsFullRect(rectangle))
                    return true;
            }
            return false;
        }

        private static bool atLeastOneCornerContained(Rectangle[] roomRectangles, Rectangle rectangle, int numberOfRectsDone)
        {
            for (int rect = 0; rect < numberOfRectsDone; rect++)
            {
                foreach (Position corner in rectangle.getCorners())
                {
                    if (roomRectangles[rect].contains(corner))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static Room getRoom(Section mapSection, Rectangle[] roomRectangles, string id)
        {
            int minX = mapSection.getX() + mapSection.getWidth();
            int minY = mapSection.getY() + mapSection.getHeight();
            int maxX = mapSection.getX();
            int maxY = mapSection.getY();

            foreach (Rectangle rect in roomRectangles)
            {
                foreach (Position corner in rect.getCorners())
                {
                    if (minX > corner.getX())
                    {
                        minX = corner.getX();
                    }

                    if (minY > corner.getY())
                    {
                        minY = corner.getY();
                    }

                    if (maxX < corner.getX())
                    {
                        maxX = corner.getX();
                    }

                    if (maxY < corner.getY())
                    {
                        maxY = corner.getY();
                    }
                }
            }

            if (minX != mapSection.getX())
            {
                minX--;
            }

            if (minY != mapSection.getY())
            {
                minY--;
            }
            if (maxX != mapSection.getX() + mapSection.getWidth())
            {
                maxX++;
            }
            if (maxY != mapSection.getY() + mapSection.getWidth())
            {
                maxY++;
            }
            Room room = new Room(minX, minY, maxX - minX + 1, maxY - minY + 1);
            setRoomComponents(room, roomRectangles);
            room.setID(id);
            return room;
        }

        public static bool isRoomPosition(int x, int y, Rectangle[] roomRectangles)
        {
            foreach (Rectangle rectangle in roomRectangles)
            {
                if (rectangle.contains(new Position(x, y)))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool isWallPosition(int x, int y, Rectangle[] roomRectangles)
        {
            if (isRoomPosition(x, y, roomRectangles))
            {
                return false;
            }
            for (int neighboorX = x - 1; neighboorX <= x + 1; neighboorX++)
            {
                for (int neighboorY = y - 1; neighboorY <= y + 1; neighboorY++)
                {
                    if (neighboorX == x && neighboorY == y)
                    {
                        continue;
                    }
                    if (isRoomPosition(neighboorX, neighboorY, roomRectangles))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void setRoomComponents(Room room, Rectangle[] roomRectangles)
        {
            Section roomContainer = room.getRoomContainer();
            List<Position> roomPositions = new List<Position>();
            List<Position> wallPositions = new List<Position>();

            for (int x = roomContainer.getX(); x < roomContainer.getX() + roomContainer.getWidth(); x++)
            {
                for (int y = roomContainer.getY(); y < roomContainer.getY() + roomContainer.getHeight(); y++)
                {
                    if (isRoomPosition(x, y, roomRectangles))
                    {
                        roomPositions.Add(new Position(x, y));
                    }
                    else if (isWallPosition(x, y, roomRectangles))
                    {
                        wallPositions.Add(new Position(x, y));
                    }
                }
            }
            room.setFloorPositions(roomPositions);
            room.setWallPositions(wallPositions);
        }
    }
}