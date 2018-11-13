using Core.Constructions;
using Core.Utility;
using Core.Utility.AStar;
using System;
using System.Collections.Generic;

namespace Core.Factories
{
    internal class HallwayFactory
    {
        private const int MAX_ATEMPTS_FOR_EXIT = 2;
        private const int MIN_PASSAGE_X_OR_Y = 2;
        
        
        public static Hallway getHallway(string id, Room previousRoom, Hallway previousHall, Room nextRoom, GameMap map, Random random, List<Position> wallPositions, List<Position> floorPositions)
        {
            int triesToCreateExit = 0;
            List<Position> path = new List<Position>();
            Position entrance = new Position();//Αρχικοποιήση πρως ικανοποίηση του compiler
            Position exit = new Position();//Το ίδιο

            while (path.Count == 0)
            {
                entrance = createHallwayEntrance(previousRoom, previousHall, triesToCreateExit++, map, random);
                exit = getExit(nextRoom, map, random);
                WalkableTile[,] walkableMap = initializeWalkable(map);
                path = AStar.findPath(entrance, exit, walkableMap);
            }

            List<Position> hallWallPositions = createWallPositions(path, entrance, exit, map);

            foreach (Position wallPosition in hallWallPositions)
            {
                if (!wallPositions.Contains(wallPosition))
                {
                    wallPositions.Add(wallPosition);
                }
            }
            floorPositions.AddRange(path);
            
            return new Hallway(id, path, hallWallPositions);
        }

        private static Position getExit(Room nextRoom, GameMap map, Random random)
        {
            Position exit = getPassage(nextRoom.getWallPositions(), map, random);

            if (nextRoom.getEntrance() != null)
            {
                switchWallFloor((Position)nextRoom.getEntrance());
            }
            nextRoom.setEntrance(exit);

            switchWallFloor(exit);
            
            return exit;
        }

        private static List<Position> createWallPositions(List<Position> path,Position entrance, Position exit, GameMap map)
        {
            List<Position> wall = new List<Position>();
            foreach (Position step in path)
            {
                if (step.Equals(entrance) || step.Equals(exit))
                {
                    continue;
                }
                List<Position> neighbors = getNeighbors(step);
                foreach (Position neighbor in neighbors)
                {
                    if (path.Contains(neighbor))
                    {
                        continue;
                    }
                    else
                    {
                        if (wall.Contains(neighbor))
                        {
                            continue;
                        }
                        wall.Add(neighbor);
                    }
                }
            }
            return wall;
        }

        private static List<Position> getNeighbors(Position pos)
        {
            List<Position> neighbors = new List<Position>();
            for (int x = pos.getX() - 1; x <= pos.getX() + 1; x++)
            {
                for (int y = pos.getY() - 1; y <= pos.getY() + 1; y++)
                {
                    if (pos.Equals(new Position(x, y)))
                    {
                        continue;
                    }
                    neighbors.Add(new Position(x, y));
                }
            }
            return neighbors;
        }

        private static WalkableTile[,] initializeWalkable(GameMap map)
        {
            WalkableTile[,] walkables = new WalkableTile[map.getWidth(), map.getHeight()];
            foreach (Room room in map.getRooms())
            {
                foreach (Position wallPos in room.getWallPositions())
                {
                    walkables[wallPos.getX(), wallPos.getY()] = new WalkableTile(WalkableTile.Wall);
                }
                foreach (Position floorPos in room.getFloorPositions())
                {
                    walkables[floorPos.getX(), floorPos.getY()] = new WalkableTile(WalkableTile.Floor);
                }
            }
            foreach (Hallway hall in map.getHallways())
            {
                foreach (Position wallPos in hall.getWallPositions())
                {
                    walkables[wallPos.getX(), wallPos.getY()] = new WalkableTile(WalkableTile.Wall);
                }
                foreach (Position floorPos in hall.getPath())
                {
                    walkables[floorPos.getX(), floorPos.getY()] = new WalkableTile(WalkableTile.Floor);
                }
            }
            for (int x = 0; x < walkables.GetLength(0); x++)
            {
                for (int y = 0; y < walkables.GetLength(1); y++)
                {
                    if (walkables[x, y] != null)
                    {
                        continue;
                    }
                    walkables[x, y] = new WalkableTile(WalkableTile.Nothing);
                }
            }
                return walkables;
        }

        private static Position createHallwayEntrance(Room previousRoom, Hallway previousHall, int tries, GameMap map, Random random)
        {
            Position entrance;
            if (tries < MAX_ATEMPTS_FOR_EXIT || previousHall == null)
            {
                entrance = getPassage(previousRoom.getWallPositions(), map, random);

                if (previousRoom.getExit() != null)
                {
                    switchWallFloor((Position)previousRoom.getExit());
                }
                
                previousRoom.setExit(entrance);
            }
            else
            {
                entrance = getPassage(previousHall.getWallPositions(), map, random);

                if (previousHall.getIntersection() != null)
                {
                    switchWallFloor((Position)previousHall.getIntersection());
                }
                
                previousHall.setIntersection(entrance);
            }
            switchWallFloor(entrance);
            return entrance;
        }

        private static Position getPassage(List<Position> roomWallPositions, GameMap map, Random random)
        {
            bool created = false;
            Position passage = new Position();//Αρχικοποίηση για την ικανοποίηση του compiler
            while (!created)
            {
                int index = random.Next(roomWallPositions.Count);
                passage = roomWallPositions[index];                                     //Επιλογή τυχαίας θέσης
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
            
            if (!validDistanceFromEdge)
            {
                return false; //Στη περίπτωση που δεν έχει την απαιτούμενη απόσταση να επιστρέψει false.
            }
            //Έλεγχος προσβασιμότητας.
            //Οριζόντια
            bool reachable = !MapFactory.wallPositions.Contains(new Position(passage.getX() + 1, passage.getY())) && !MapFactory.wallPositions.Contains(new Position(passage.getX() - 1, passage.getY()))
                && MapFactory.wallPositions.Contains(new Position(passage.getX(), passage.getY() + 1)) && MapFactory.wallPositions.Contains(new Position(passage.getX(), passage.getY() - 1));
            //Κάθετα
            reachable = reachable || !MapFactory.wallPositions.Contains(new Position(passage.getX(), passage.getY() + 1)) && !MapFactory.wallPositions.Contains(new Position(passage.getX(), passage.getY() - 1))
                && MapFactory.wallPositions.Contains(new Position(passage.getX() + 1, passage.getY())) && MapFactory.wallPositions.Contains(new Position(passage.getX() - 1, passage.getY()));
            return reachable;//To validDistance ελέγχθηκε πιο πριν, δε χρειάζεται ξανά.
        }

        private static void switchWallFloor(Position pos)
        {
            if (MapFactory.wallPositions.Contains(pos))
            {
                MapFactory.wallPositions.Remove(pos);
                MapFactory.floorPositions.Add(pos);
            }
            else
            {
                MapFactory.floorPositions.Remove(pos);
                MapFactory.wallPositions.Add(pos);
            }
        }
    }
}
