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

        public static Room getRoom(Section mapSection, Random random)
        {
            int numberOfRects = random.Next(1, 8);
            Rectangle[] roomRectangles = createRoomRectangles(mapSection, numberOfRects, random);
            Room newRoom = getRoom(mapSection, roomRectangles);
            setRoomComponents(newRoom, roomRectangles);
            return newRoom;
        }

        private static Rectangle[] createRoomRectangles(Section mapSection, int numberOfRectangles, Random random)
        {
            Rectangle[] roomRects = new Rectangle[numberOfRectangles];
            for (int rect = 0; rect < roomRects.Length; rect++)
            {
                bool validRect = false;
                do
                {
                    int rectX = 0;
                    int rectY = 0;
                    int rectWidth = 0;
                    int rectHeight = 0;

                    //create x coordinate for the rectangle
                    do
                    {
                        rectX = random.Next(mapSection.getXOff() + Room.MIN_DISTANCE_FROM_SECTION_EDGE,
                            mapSection.getXOff() + mapSection.getWidth() - Room.MIN_DISTANCE_FROM_SECTION_EDGE);
                    } while (Room.MIN_WIDTH_HEIGHT >= mapSection.getWidth()
                        - (rectX - mapSection.getXOff() - Room.MIN_DISTANCE_FROM_SECTION_EDGE - 1));

                    //create y coordinate for the rectangle
                    do
                    {
                        rectY = random.Next(mapSection.getYOff() + Room.MIN_DISTANCE_FROM_SECTION_EDGE
                            , mapSection.getYOff() + mapSection.getHeight() - Room.MIN_DISTANCE_FROM_SECTION_EDGE);
                    } while (Room.MIN_WIDTH_HEIGHT >= mapSection.getHeight() - (rectY - mapSection.getYOff() - Room.MIN_DISTANCE_FROM_SECTION_EDGE - 1));

                    //create width for the rectangle
                    if (Room.MIN_WIDTH_HEIGHT == mapSection.getWidth() - (rectX - mapSection.getXOff() - Room.MIN_DISTANCE_FROM_SECTION_EDGE - 1))
                    {
                        rectWidth = Room.MIN_WIDTH_HEIGHT;
                    }
                    else
                    {
                        rectWidth = random.Next(Room.MIN_WIDTH_HEIGHT
                            , mapSection.getWidth() - (rectX - mapSection.getXOff() - Room.MIN_DISTANCE_FROM_SECTION_EDGE - 1));
                    }

                    //create height for the rectangle
                    if (Room.MIN_WIDTH_HEIGHT == mapSection.getHeight() - (rectY - mapSection.getYOff() - Room.MIN_DISTANCE_FROM_SECTION_EDGE - 1))
                    {
                        rectHeight = Room.MIN_WIDTH_HEIGHT;
                    }
                    else
                    {
                        rectHeight = random.Next(Room.MIN_WIDTH_HEIGHT, mapSection.getHeight() - (rectY - mapSection.getYOff() - Room.MIN_DISTANCE_FROM_SECTION_EDGE - 1));
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

        private static Room getRoom(Section mapSection, Rectangle[] roomRectangles)
        {
            int minX = mapSection.getXOff() + mapSection.getWidth();
            int minY = mapSection.getYOff() + mapSection.getHeight();
            int maxX = mapSection.getXOff();
            int maxY = mapSection.getYOff();

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

            if (minX != mapSection.getXOff())
            {
                minX--;
            }

            if (minY != mapSection.getYOff())
            {
                minY--;
            }
            if (maxX != mapSection.getXOff() + mapSection.getWidth())
            {
                maxX++;
            }
            if (maxY != mapSection.getYOff() + mapSection.getWidth())
            {
                maxY++;
            }

            return new Room(minX, minY, maxX - minX + 1, maxY - minY + 1);
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
                for (int neighboorY = y - 1; neighboorY <= x + 1; neighboorY++)
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

            for (int x = roomContainer.getXOff(); x < roomContainer.getXOff() + roomContainer.getWidth(); x++)
            {
                for (int y = roomContainer.getYOff(); y < roomContainer.getYOff() + roomContainer.getHeight(); y++)
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