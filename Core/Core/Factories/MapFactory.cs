using System;
using System.Collections.Generic;
using Core.Utility;
namespace Core.Factories
{
    public class MapFactory
    {
        private const int MAX_SLICE_SIZE = 20;
        private const int MIN_SLICE_SIZE = 12;
        private const int START_ID_NUM = 1;
        public static GameMap getNewGameMap(int width, int height)
        {
            BlockType[,] newMap = new BlockType[width, height];
            initializeMapArray(newMap);
            Random random = new Random();
            List<Section> sections = createSections(width, height, random);

            Position? exit = null;
            Position? exitSwitch = null;
            Section? previousSection = null;
            foreach (Section current in sections)
            {

            }

            return new GameMap();
        }

        public static GameMap getNewGameMap(int width, int height, int seed)
        {
            BlockType[,] newMap = new BlockType[width, height];
            initializeMapArray(newMap);
            Random random = new Random(seed);
            List<Section> sections = createSections(width, height, random);

            return new GameMap();
        }

        private static void initializeMapArray(BlockType[,] mapArray)
        {
            //Να ελέγξουμε αν γίνεται με foreach γιατί στη java με passed by value types δε δουλεύει.
            for (int x = 0; x <= mapArray.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= mapArray.GetUpperBound(1); y++)
                {
                    mapArray[x, y] = BlockType.Nothing;
                }
            }
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
    }
}
