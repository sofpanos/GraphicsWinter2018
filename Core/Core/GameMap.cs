using System;
using System.Collections.Generic;
using Core.Constructions;
using Core.Utility;

namespace Core
{
    public class GameMap
    {
        private Dictionary<string, Room> rooms;
        private Dictionary<string, Hallway> hallways;
        private Dictionary<string, Position> entrances;
        private Dictionary<string, Position> exits;
        private BlockType[,] map;

        public GameMap(int width, int height)
        {
            this.rooms = new Dictionary<string, Room>();
            this.hallways = new Dictionary<string, Hallway>();
            this.entrances = new Dictionary<string, Position>();
            this.exits = new Dictionary<string, Position>();
            this.map = new BlockType[width, height];
            initializeMap();
        }

        private void initializeMap()
        {
            for (int x = 0; x < map.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= map.GetUpperBound(1); y++)
                {
                    this[x, y] = BlockType.Nothing;
                }
            }
        }

        public void addRoom(string ID, Room room)
        {
            rooms.Add(ID, room);
            foreach (Position wall in room.getWallPositions())
            {
                map[wall.getX(), wall.getY()] = BlockType.Wall;
            }
            foreach (Position floor in room.getFloorPositions())
            {
                map[floor.getX(), floor.getY()] = BlockType.Floor;
            }
        }

        public void addHallway(string ID, Hallway hallway)
        {
            hallways.Add(ID, hallway);
            //Ενημέρωση του 2D Πίνακα
            foreach (Position wall in hallway.getPath())
            {
                this.map[wall.getX(), wall.getY()] = BlockType.Floor;
            }
            foreach (Position floor in hallway.getWallPosition())
            {
                this.map[floor.getX(), floor.getY()] = BlockType.Wall;
            }
        }

        public void addEntrance(string ID, Position entrance)
        {
            entrances.Add(ID, entrance);
        }

        public void addExit(string ID, Position exit)
        {
            exits.Add(ID, exit);
        }

        public Room getRoom(string ID)
        {
            Room room;
            if (rooms.TryGetValue(ID,out room))
            {
                return room;
            }
            else
            {
                return null;
            }
        }

        public List<Room> getRooms()
        {
            List<Room> roomsList = new List<Room>();
            foreach (KeyValuePair<string, Room> roomPair in rooms)
            {
                roomsList.Add(roomPair.Value);
            }
            return roomsList;
        }

        public BlockType this[int x, int y]
        {
            get { return map[x, y]; }
            set { map[x, y] = value; }
        }
    }
}
