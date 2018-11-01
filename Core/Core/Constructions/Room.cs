﻿using System;
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
        private Dictionary<Position, BlockType> wallBlocks;
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

        public void setEntrance(Position entrance)
        {
            if (this.entrance != null)
            {
                wallBlocks.Remove((Position)entrance);
                wallBlocks.Add((Position)this.entrance, BlockType.Wall);
                floorPositions.Add(entrance);
                floorPositions.Remove((Position)this.entrance);
            }
            this.entrance = entrance;
        }

        public Position? getEntrance()
        {
            return this.entrance;
        }

        public void setExit(Position exit)
        {
            if (this.exit != null)
            {
                wallBlocks.Remove(exit);
                wallBlocks.Add((Position)this.exit, BlockType.Wall);
                floorPositions.Remove((Position)this.exit);
                floorPositions.Add(exit);
            }
            else
            {
                wallBlocks.Remove(exit);
                floorPositions.Add(exit);
            }
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
                else 
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
    }

    
}