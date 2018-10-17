using Core.Constructions;
using Core.Utility;
using Core.Utility.AStar;
using System;
using System.Collections.Generic;

namespace Core.Factories
{
    public class HallwayFactory
    {
        private const int MAX_ATEMPTS_FOR_EXIT = 2;
        private const int MIN_PASSAGE_X_OR_Y = 2;
        
        
        public static Hallway getHallway(string id, Room previousRoom, Hallway previousHall, Room nextRoom, GameMap map, Random random)
        {
            int triesToCreateExit = 0;
            List<Position> path = new List<Position>();
            Position entrance = new Position();//Αρχικοποιήση πρως ικανοποίηση του compiler
            Position exit = new Position();//Το ίδιο
            while (path.Count == 0)
            {
                entrance = createHallwayEntrance(previousRoom, previousHall, triesToCreateExit, map, random);
                exit = getExit(nextRoom, map, random);
                WalkableTile[,] walkableMap = initializeWalkable(map);
                path = AStar.findPath(entrance, exit, walkableMap);
            }
            List<Position> wallPositions = createWallPositions(path, entrance, exit, map);
            
            return new Hallway(id, path, wallPositions);
        }

        private static Position getExit(Room nextRoom, GameMap map, Random random)
        {
            Position exit = getPassage(nextRoom.getWallPositions(), map, random);
            nextRoom.setEntrance(exit);
            switchFloorWallTile(map, exit);
            return exit;
        }

        private static void switchFloorWallTile(GameMap map, Position tilePosition)
        {
            map[tilePosition.getX(), tilePosition.getY()] = (map[tilePosition.getX(), tilePosition.getY()] == BlockType.Wall) ? BlockType.Floor : BlockType.Wall;
        }

        private static List<Position> createWallPositions(List<Position> path,Position entrance, Position exit, GameMap map)
        {

            throw new NotImplementedException();
        }

        private static WalkableTile[,] initializeWalkable(GameMap map)
        {
            WalkableTile[,] walkables = new WalkableTile[map.getWidth(), map.getHeight()];
            for (int x = 0; x < map.getWidth(); x++)
            {
                for (int y = 0; y < map.getHeight(); y++)
                {
                    WalkableTile walkable;
                    //Δεν εξατάζουμε για τα υπόλοιπα είδη από tiles γιατί δεν υπάρχουν σε αυτό το στάδιο.
                    switch (map[x, y])
                    {
                        case BlockType.Floor:
                        case BlockType.Wall:
                            walkable = new WalkableTile(WalkableTile.Wall);
                            break;
                        default:
                            walkable = new WalkableTile(WalkableTile.Nothing);
                            break;
                    }
                }
            }
            return walkables;
        }

        private static Position createHallwayEntrance(Room previousRoom, Hallway previousHall, int tries, GameMap map, Random random)
        {
            Position entrance;
            if (tries < MAX_ATEMPTS_FOR_EXIT)
            {
                entrance = getPassage(previousRoom.getWallPositions(), map, random);
                previousRoom.setExit(entrance);
            }
            else
            {
                entrance = getPassage(previousHall.getWallPosition(), map, random);
                previousHall.setIntersection(entrance);
            }
            switchFloorWallTile(map, entrance);
            return entrance;
        }

        private static Position getPassage(List<Position> roomWallPositions, GameMap map, Random random)
        {
            bool created = false;
            Position passage = new Position();//Αρχικοποίηση για την ικανοποίηση του compiler
            while (!created)
            {
                passage = roomWallPositions[random.Next(roomWallPositions.Count)]; //Επιλογή τυχαίας θέσης
                created = isValidPassage(passage, map);                            //Έλεγχος εγγυρότητας θέσης
            }
            return passage;
        }

        private static bool isValidPassage(Position passage, GameMap map)
        {
            //Έλεγχος της απόστασης από την άκρη του χάρτη.
            bool validDistanceFromEdge = passage.getX() >= MIN_PASSAGE_X_OR_Y && passage.getY() >= MIN_PASSAGE_X_OR_Y;
            validDistanceFromEdge = validDistanceFromEdge && passage.getX() < map.getWidth() - MIN_PASSAGE_X_OR_Y 
                && passage.getY() < map.getHeight() - MIN_PASSAGE_X_OR_Y;
            //Έλεγχος προσβασιμότητας.
            //Οριζόντια
            bool reachable = map[passage.getX() + 1, passage.getY()] == BlockType.Floor 
                && map[passage.getX() - 1, passage.getY()] == BlockType.Floor;
            //Κάθετα
            reachable = reachable || map[passage.getX(), passage.getY() + 1] == BlockType.Floor 
                && map[passage.getX(), passage.getY() - 1] == BlockType.Floor;
            return validDistanceFromEdge && reachable;
        }
    }
}
