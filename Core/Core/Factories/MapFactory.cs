﻿using System;
using System.Collections.Generic;
using Core.Utility;
using Core.Constructions;

namespace Core.Factories
{
    public class MapFactory
    {
        private const int MIN_NUM_OF_SLICES = 2;
        private const int MIN_SLICE_SIZE = 30;
        private const string ROOM_ID_FORMAT_STRING = "R{0,3:D3}";
        private const string HALLWAY_ID_FORMAT_STRING = "H{0,3:D3}";
        private const int START_ID_NUM = 1; 
        internal static List<Position> wallPositions;
        internal static List<Position> floorPositions;
        
        
        
        public static GameMap getNewGameMap(int width, int height, int level)
        {
            GameMap theMap = new GameMap(width, height);

            MapFactory.wallPositions = new List<Position>();
            MapFactory.floorPositions = new List<Position>();
            
            Random random = new Random();
            List<Section> sections = createSections(width, height, random);
            
            createRoomsAndHallways(sections, theMap, random);

            createLevelExitAndSwitch(theMap, random);

            createLightsAndSwitches(theMap, random, level, 0.8);
 
            return theMap;
        }

        public static GameMap getNewGameMap(int width, int height,int level, int seed)
        {
            GameMap theMap = new GameMap(width, height);

            MapFactory.wallPositions = new List<Position>();
            MapFactory.floorPositions = new List<Position>();

            Random random = new Random(seed);
            List<Section> sections = createSections(width, height, random);

            createRoomsAndHallways(sections, theMap, random);

            createLevelExitAndSwitch(theMap, random);

            createLightsAndSwitches(theMap, random, level, 0.8);

            return new GameMap(width, height);
        }

        public static void createLevelExitAndSwitch(GameMap theMap, Random random)
        {
            //Create Level Exit
            theMap.getRooms()[random.Next(theMap.getRooms().Count)].setWorldExit(random);
            //Create Level Exit Switch
            theMap.getRooms()[random.Next(theMap.getRooms().Count)].setWorldExitSwitch(random);
        }

        public static List<Section> createSections(int width, int height, Random random)
        {
            List<Section> theSections = new List<Section>();
            width -= 10;  // Αφαιρούμε την απόσταση "ασφάλειας" από την άκρη του κόσμου.
            height -= 10; // Το ίδιο
            int xStart = 5; //Αρχίζουμε από το 0 + την απόσταση "ασφάλειας" από την άκρη.
            int yStart = 5; //Το ίδιο

            int maxNumOfSlices = width / MIN_SLICE_SIZE;
            int numOfSlices = random.Next(MIN_NUM_OF_SLICES, maxNumOfSlices);
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
                        sliceHeight = random.Next(MIN_SLICE_SIZE, height - MIN_SLICE_SIZE);

                        temp.Add(new Section(xStart, yStart, sliceWidth, sliceHeight));
                        temp.Add(new Section(xStart, yStart + sliceHeight, sliceWidth, height - sliceHeight));
                    }
                }
                else
                {
                    temp.Add(new Section(xStart, yStart, sliceWidth, height));
                }

                //40% πιθανότητα για έξτρα πλάτος.
                if (restWidth != 0 && random.NextDouble() <= 0.4)
                {
                    int extraWidth = random.Next(1, restWidth + 1);
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

        public static void createRoomsAndHallways(List<Section> sections, GameMap map, Random random)
        {
            int id = START_ID_NUM;
            Room previousRoom = null;
            Hallway previousHall = null;

            foreach (Section current in sections)
            {
                map.addRoom(String.Format(ROOM_ID_FORMAT_STRING, id), RoomFactory.getRoom(current,String.Format(ROOM_ID_FORMAT_STRING, id++), random, MapFactory.wallPositions, MapFactory.floorPositions));
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
                previousHall = currentHall = HallwayFactory.getHallway(String.Format(HALLWAY_ID_FORMAT_STRING, id++), previousRoom, previousHall, currentRoom, map, random, MapFactory.wallPositions, MapFactory.floorPositions);
                map.addHallway(currentHall.getID(), currentHall);
                previousRoom = currentRoom;
            }
        }

        public static void createLightsAndSwitches(GameMap map, Random random, int level, double propability)
        {
            foreach (Room room in map.getRooms())
            {
                //80% Πιθανότητα / τα περασμένα επίπεδα ώστε να υπάρχει φως σε κάποιο δωμάτιο.
                if (random.NextDouble() < propability / level)
                {
                    room.setLightSwitch(random);
                }
            }
        }

        
    }
}
