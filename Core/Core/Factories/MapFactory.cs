using System;
using System.Collections.Generic;
using Core.Utility;
using Core.Constructions;

namespace Core.Factories
{
    public class MapFactory
    {
        private const int MAX_SLICE_SIZE = 20;
        private const int MIN_SLICE_SIZE = 12;
        private const string ROOM_ID_FORMAT_STRING = "R{0,3:D3}";
        private const string HALLWAY_ID_FORMAT_STRING = "H{0,3:D3}";
        private const int START_ID_NUM = 1;
        
        
        public static GameMap getNewGameMap(int width, int height, int level)
        {
            GameMap theMap = new GameMap(width, height);
            initializeMapArray(theMap, width, height);
            
            Random random = new Random();
            List<Section> sections = createSections(width, height, random);
            
            createRoomsAndHallways(sections, theMap, random);

            createLightsAndSwitches(theMap, random, level, 0.8);

            createExitSwitch(theMap, random);

            return new GameMap(width, height);
        }

        public static GameMap getNewGameMap(int width, int height,int level, int seed)
        {
            GameMap theMap = new GameMap(width, height);
            initializeMapArray(theMap, width, height);
            
            Random random = new Random(seed);
            List<Section> sections = createSections(width, height, random);

            createRoomsAndHallways(sections, theMap, random);

            createLightsAndSwitches(theMap, random, level, 0.8);

            createExitSwitch(theMap, random);

            return new GameMap(width, height);
        }

        private static void createExitSwitch(GameMap theMap, Random random)
        {
            Room room = theMap.getRooms()[random.Next(theMap.getRooms().Count)];
            Position exit = room.getWallPositions()[room.getWallPositions().Count];
            room = theMap.getRooms()[random.Next(theMap.getRooms().Count)];
            Position exitSwitch = room.getWallPositions()[room.getWallPositions().Count];
            theMap[exit.getX(), exit.getY()] = BlockType.Exit;
            theMap[exitSwitch.getX(), exitSwitch.getY()] = BlockType.ExitSwitch;
        }

        private static void initializeMapArray(GameMap mapArray, int width, int height)
        {
            //Να ελέγξουμε αν γίνεται με foreach γιατί στη java με passed by value types δε δουλεύει.
            
        }

        private static List<Section> createSections(int width, int height, Random random)
        {
            List<Section> theSections = new List<Section>();
            width -= 10;
            height -= 10;
            int xStart = 4;
            int yStart = 5;

            int numOfSlices = random.Next(width / MAX_SLICE_SIZE, width / MIN_SLICE_SIZE);
            int sliceWidth = width / numOfSlices;
            int restWidth = width % numOfSlices;

            for (int slice = 0; slice < numOfSlices; slice++)
            {
                List<Section> temp = new List<Section>();
                //Να κάνει κάθετη τομή με 60% πιθανότητα
                //και το ύψος επαρκεί.
                if (random.NextDouble() < 0.6 && height - MIN_SLICE_SIZE >= MIN_SLICE_SIZE)
                {
                    int sliceHeight = 0;
                    if (height == MIN_SLICE_SIZE * 2)
                    {
                        temp.Add(new Section(xStart, yStart, sliceWidth, MIN_SLICE_SIZE));
                        temp.Add(new Section(xStart, yStart, sliceWidth, MIN_SLICE_SIZE));
                    }
                    else
                    {
                        //Βρες το σημείο της οριζόντιας τομής.
                        //+Έλεγχος εγκυρότητας
                        do
                        {
                            sliceHeight = random.Next(MIN_SLICE_SIZE, height);
                        } while (height - sliceHeight >= MIN_SLICE_SIZE && height - sliceHeight <= MAX_SLICE_SIZE 
                            && sliceHeight <= MAX_SLICE_SIZE);

                        temp.Add(new Section(xStart, yStart, sliceWidth, sliceHeight));
                        temp.Add(new Section(xStart, yStart + sliceHeight, sliceWidth, sliceHeight));
                    }
                }
                else
                {
                    temp.Add(new Section(xStart, yStart, sliceWidth, height));
                }

                //40% πιθανότητα για έξτρα πλάτος.
                if (restWidth != 0 && random.NextDouble() <= 0.4)
                {
                    int extraWidth = random.Next(1, restWidth);
                    restWidth -= extraWidth;
                    
                    foreach(Section sectionItem in temp)
                    {
                        sectionItem.setWidth(sliceWidth + extraWidth);
                        theSections.Add(sectionItem);
                    }
                    //Μετακίνησε το x για την επόμενη τομή.
                    xStart += sliceWidth + extraWidth;
                }
                else
                {
                    foreach (Section sectionItem in temp)
                    {
                        theSections.Add(sectionItem);
                    }
                    //Μετακίνησε το x για την επόμενη τομή.
                    xStart += sliceWidth;
                }
            }

            return theSections;
        }

        private static void createRoomsAndHallways(List<Section> sections, GameMap map, Random random)
        {
            int id = START_ID_NUM;
            Room previousRoom = null;
            Hallway previousHall = null;

            foreach (Section current in sections)
            {
                map.addRoom(String.Format(ROOM_ID_FORMAT_STRING, id++), RoomFactory.getRoom(current, random));
            }

            id = START_ID_NUM;
            Hallway currentHall = null;
            foreach (Room currentRoom in map.getRooms())
            {
                if (previousRoom == null)
                {
                    previousRoom = currentRoom;
                    continue;
                }
                previousHall = currentHall = HallwayFactory.getHallway(String.Format(HALLWAY_ID_FORMAT_STRING, id++), previousRoom, previousHall, currentRoom, map);
            }
        }

        private static void createLightsAndSwitches(GameMap map, Random random, int level, double propability)
        {
            foreach (Room room in map.getRooms())
            {
                //80% Πιθανότητα / τα περασμένα επίπεδα ώστε να υπάρχει φως σε κάποιο δωμάτιο.
                if (random.NextDouble() < propability / level)
                {
                    Position lightSwitch = room.getWallPositions()[random.Next(room.getWallPositions().Count)];
                    map[lightSwitch.getX(), lightSwitch.getY()] = BlockType.Switch;
                    Position light = room.getFloorPositions()[random.Next(room.getFloorPositions().Count)];
                    map[light.getX(), light.getY()] = BlockType.Light;
                }
            }
        }
    }
}
