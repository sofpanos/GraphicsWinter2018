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


        private void setLights(Random random, Dictionary<Position, BlockType> validBlocks)
        {
            List<Position> validKeys = new List<Position>(validBlocks.Keys);
            int numberOfLights = validKeys.Count / 5;
            numberOfLights = (numberOfLights == 0) ? 2 : numberOfLights;
            
            //Τα φώτα θα μπαίνουν σε τυχαίες θέσεις
            //Θα μπορούσαμε να βάλουμε με σειρά πέρνοντας τις θέσεις από το πάτωμα που γειτονικά με τοίχο
            //και αφού τα βάλουμε σε σειρά να αλλάζουμε το γειτονικό τοίχο κάθε "τόσα" σε φως.
            for (int i = 0; i < numberOfLights; i++)
            {
                Position lightPos = validKeys[random.Next(0, validKeys.Count)];
                validKeys.Remove(lightPos);
                wallBlocks.Remove(lightPos);
                wallBlocks.Add(lightPos, BlockType.Light);
            }
        }

        

        public Dictionary<Position, BlockType> getWallBlocks()
        {
            return wallBlocks;
        }

        // SPECIAL BLOCKS

        internal void setLightSwitch(Random random)
        {
            Dictionary<Position, BlockType> validBlocks = getValidSpecialWallblocks();
            List<Position> keyList = new List<Position>(validBlocks.Keys);
            Position lightSwitch = keyList[random.Next(0, keyList.Count)];
            wallBlocks.Remove(lightSwitch);
            wallBlocks.Add(lightSwitch, BlockType.Switch);
            validBlocks.Remove(lightSwitch);
            setLights(random, validBlocks);
            HasLight = true;
        }

        internal Dictionary<Position, BlockType> getValidSpecialWallblocks()
        {
            Dictionary<Position, BlockType> valid = new Dictionary<Position, BlockType>();
            foreach (KeyValuePair<Position, BlockType> wallBlock in wallBlocks)
            {
                if (isValidSpecialBlock(wallBlock.Key))
                {
                    valid.Add(wallBlock.Key, wallBlock.Value);
                }
            }
            return valid;
        }

        private bool isValidSpecialBlock(Position pos)
        {
            if (wallBlocks[pos] != BlockType.Wall)
            {
                return false;
            }
            for (int x = pos.getX() - 1; x <= pos.getX() + 1; x++)
            {
                for (int y = pos.getY() - 1; y <= pos.getY() + 1; y++)
                {
                    if ((x == pos.getX() && y == pos.getY()) || (x != pos.getX() && y != pos.getY()))
                    {
                        continue;
                    }
                    if (floorPositions.Contains(new Position(x, y)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        internal void setWorldExit(Random random)
        {
            List<Position> validBlockPositions = new List<Position>(getValidSpecialWallblocks().Keys);
            Position exitPos = validBlockPositions[random.Next(0, validBlockPositions.Count)];
            wallBlocks.Remove(exitPos);
            wallBlocks.Add(exitPos, BlockType.Exit);
        }

        internal void setWorldExitSwitch(Random random)
        {
            List<Position> validBlockPositions = new List<Position>(getValidSpecialWallblocks().Keys);
            Position exitSwitchPos = validBlockPositions[random.Next(0, validBlockPositions.Count)];
            wallBlocks.Remove(exitSwitchPos);
            wallBlocks.Add(exitSwitchPos, BlockType.ExitSwitch);
        }
    }

    
}