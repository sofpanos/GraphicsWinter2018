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
        private int width;
        private int height;

        public GameMap(int width, int height)
        {
            this.rooms = new Dictionary<string, Room>();
            this.hallways = new Dictionary<string, Hallway>();
            this.entrances = new Dictionary<string, Position>();
            this.exits = new Dictionary<string, Position>();
            this.width = width;
            this.height = height;
        }

        public void addRoom(string ID, Room room)
        {
            rooms.Add(ID, room);
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
 
        public int getWidth()
        {
            return this.width;
        }

        public int getHeight()
        {
            return this.height;
        }
    }
}
