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

        public BlockType this[int x, int y]
        {
            get { return map[x, y]; }
            set { map[x, y] = value; }
        }
    }
}
