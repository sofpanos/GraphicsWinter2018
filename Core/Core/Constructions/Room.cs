using System;
using Core.Utility;
using System.Collections.Generic;

namespace Core.Constructions
{

    public class Room
    {
        public static int MIN_WIDTH_HEIGHT = 4;
        public static int MIN_DISTANCE_FROM_SECTION_EDGE = 4;

        private Section roomContainer;
        private string type;
        private List<Position> floorPositions;
        private List<Position> wallPositions;
        private Position? entrance;
        private Position? exit;
        private bool hasLight = false;
        private string ID;

        public Room(int xOff, int yOff, int width, int height)
        {
            this.roomContainer = new Section(xOff, yOff, width, height);
        }

        public Room(int xOff, int yOff, int width, int height, string type)
        {
            this.roomContainer = new Section(xOff, yOff, width, height);
            this.type = type;
        }

        public string getID()
        {
            return ID;
        }

        public void setID(string id){
            ID = id;
        }
        public Section getRoomContainer(){
            return roomContainer;
        }

        public void setFloorPositions(List<Position> positions)
        {
            this.floorPositions = positions;
        }

        public List<Position> getFloorPositions()
        {
            return floorPositions;
        }

        public void setWallPositions(List<Position> positions)
        {
            this.wallPositions = positions;
        }

        public List<Position> getWallPositions()
        {
            return wallPositions;
        }

        public void setEntrance(Position entrance)
        {
            this.entrance = entrance;
        }

        public Position? getEntrance()
        {
            return this.entrance;
        }

        public void setExit(Position exit)
        {
            this.exit = exit;
        }

        public Position? getExit()
        {
            return this.exit;
        }

        public bool isRoomPosition(Position pos)
        {
            return this.floorPositions.Contains(pos) || this.wallPositions.Contains(pos);
        }

        public bool HasLight
        {
            get { return hasLight;}
            set
            {
                hasLight = value;
            }
        }
        
    }

    
}