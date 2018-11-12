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
        private List<Position> floorPositions = new List<Position>();
        private Dictionary<Position, BlockType> wallBlocks = new Dictionary<Position,BlockType>();
        private Position? entrance;
        private Position? exit;
        private string ID;

        public Room(int xOff, int yOff, int width, int height)
        {
            this.roomContainer = new Section(xOff, yOff, width, height);
            HasLight = false;
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

        internal void setFloorPositions(List<Position> positions)
        {
            this.floorPositions = positions;
        }

        public List<Position> getFloorPositions()
        {
            return floorPositions;
        }

        internal void setWallPositions(List<Position> positions)
        {
            foreach (Position wallPosition in positions)
            {
                this.wallBlocks.Add(wallPosition, BlockType.Wall);
            }
        }

        public List<Position> getWallPositions()
        {
            Position[] temp = new Position[wallBlocks.Count];
            wallBlocks.Keys.CopyTo(temp, 0);
            return new List<Position>(temp);
        }

        internal void setEntrance(Position entrance)
        {
            //Αν υπάρχει είσοδος επανέφερε τη θέση.
            if (this.entrance != null)
            {
                floorPositions.Remove((Position)this.entrance);
                wallBlocks.Add((Position)this.entrance, BlockType.Wall);
            }

            floorPositions.Add(entrance);
            wallBlocks.Remove(entrance);
            this.entrance = entrance;
        }

        public Position? getEntrance()
        {
            return this.entrance;
        }

        internal void setExit(Position exit)
        {
            //Αν υπάρχει έξοδος επανέφερε τη θέση.
            if (this.exit != null)
            {
                floorPositions.Remove((Position)this.exit);
                wallBlocks.Add((Position)this.exit, BlockType.Wall);
            }
            floorPositions.Add(exit);
            wallBlocks.Remove(exit);
            this.exit = exit;
        }

        public Position? getExit()
        {
            return this.exit;
        }

        public bool isRoomPosition(Position pos)
        {
            return this.floorPositions.Contains(pos) || this.wallBlocks.ContainsKey(pos);
        }

        public bool HasLight
        {
            get;
            set;
        }


        internal void setLights(Random random)
        {
            List<Position> validLightPositions = new List<Position>();
            Position[] temp = new Position[wallBlocks.Count];
            wallBlocks.Keys.CopyTo(temp,0);
            Position wallPosition = temp[0];
            for (int i = 0; i < wallBlocks.Count; i++)
            {
                if (isValidLightPosition(wallPosition))
                {
                    validLightPositions.Add(wallPosition);
                }

                if (wallBlocks.ContainsKey(new Position(wallPosition.getX() + 1, wallPosition.getY())))
                    wallPosition = new Position(wallPosition.getX() + 1, wallPosition.getY());
                else if (wallBlocks.ContainsKey(new Position(wallPosition.getX() - 1, wallPosition.getY())))
                    wallPosition = new Position(wallPosition.getX() - 1, wallPosition.getY());
                else if (wallBlocks.ContainsKey(new Position(wallPosition.getX(), wallPosition.getY() + 1)))
                    wallPosition = new Position(wallPosition.getX(), wallPosition.getY() + 1);
                else if (wallBlocks.ContainsKey(new Position(wallPosition.getX(), wallPosition.getY() - 1)))
                    wallPosition = new Position(wallPosition.getX(), wallPosition.getY() - 1);
                else //Na prosthesw na proxoraei sto toixo an briskei eisodo i exodo.
                    break;
                if (validLightPositions.Contains(wallPosition))
                    break;
            }

            int lightSwitchIndex = random.Next(0, validLightPositions.Count);
            Position lightSwitchPosition = validLightPositions[lightSwitchIndex];
            for (int i = 0; i < validLightPositions.Count; i++)
            {
                if (i % 5 == 0 || i != lightSwitchIndex || wallBlocks[validLightPositions[i]] != BlockType.Exit 
                     || wallBlocks[validLightPositions[i]] != BlockType.ExitSwitch)
                {
                    wallBlocks[validLightPositions[i]] =  BlockType.Light;
                }


            }
        }

        private bool isValidLightPosition(Position wallPosition)
        {
            if(floorPositions.Contains(new Position(wallPosition.getX() + 1, wallPosition.getY())) 
                || floorPositions.Contains(new Position(wallPosition.getX() - 1, wallPosition.getY()))
                || floorPositions.Contains(new Position(wallPosition.getX(), wallPosition.getY() + 1))
                || floorPositions.Contains(new Position(wallPosition.getX(), wallPosition.getY() - 1)))
                return true;
            return false;
        }

        public Dictionary<Position, BlockType> getWallBlocks()
        {
            return wallBlocks;
        }

        internal bool setLightSwitch(Position pos)
        {
            throw new NotSupportedException();
        }
    }

    
}