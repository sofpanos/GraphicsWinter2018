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
            for (int x = 0; x <= map.GetUpperBound(0); x++)
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
            foreach (Position floor in hallway.getPath())
            {
                this.map[floor.getX(), floor.getY()] = BlockType.Floor;
            }
            foreach (Position wall in hallway.getWallPositions())
            {
                this.map[wall.getX(), wall.getY()] = BlockType.Wall;
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

        public List<Hallway> getHallways()
        {
            List<Hallway> hallList = new List<Hallway>();
            foreach(KeyValuePair<string, Hallway> pair in this.hallways){
                hallList.Add(pair.Value);
            }
            return hallList;
        }

        public BlockType this[int x, int y]
        {
            get { return map[x, y]; }
            set { map[x, y] = value; }
        }

        public int getWidth()
        {
            return map.GetUpperBound(0) + 1;
        }

        public int getHeight()
        {
            return map.GetUpperBound(1) + 1;
        }
    }
}
