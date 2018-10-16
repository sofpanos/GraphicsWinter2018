using System.Collections;
using System.Collections.Generic;
using Core.Utility;

namespace Core.Utility.AStar
{

    public class AStar
    {
        public const int VERTICAL_HORIZONTAL_SCORE = 5;
        public const int DIAGONAL_SCORE = 20;

        public static List<Position> findPath(Position start, Position goal, WalkableTile[,] grid)
        {
            Node[,] gridNodes = createGridNodes(start, goal, grid);
            
            Node finalNode = findPath(gridNodes, start, goal);

            if (finalNode == null)
            {
                return new List<Position>();
            }

            return finalNode.getPathToStart();
        }

        private static Node[,] createGridNodes(Position start, Position goal, WalkableTile[,] grid)
        {
            Node[,] gridNodes = new Node[grid.GetUpperBound(0) + 1, grid.GetUpperBound(1) + 1];
            for (int x = 0; x <= gridNodes.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= gridNodes.GetUpperBound(1); y++)
                {
                    if (x == start.getX() && y == start.getY())//Check for start
                    {
                        gridNodes[x,y] = new Node(new Position(x, y), goal, true, true);
                    }
                    else if (x == goal.getX() && y == goal.getY())//Check for goal
                    {
                        gridNodes[x,y] = new Node(new Position(x, y), goal, true);
                    }
                    else if (x == grid.GetUpperBound(0) || y == grid.GetUpperBound(1) || x == 0 || y == 0)//Check against grid edge
                    {
                        gridNodes[x, y] = new Node(new Position(x, y), goal, true);
                    }
                    else
                    {
                        gridNodes[x, y] = new Node(new Position(x, y), goal, grid[x, y].isWalkable());
                    }
                }
            }
            return gridNodes;
        }
        
        private static Node findPath(Node[,] gridNodes, Position start, Position goal)
        {
            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>(new NodeEqualityComparer());

            Node current = gridNodes[start.getX(), start.getY()];
            openSet.Add(current);

            while (openSet.Count != 0)
            {
                openSet.Sort((node1, node2) => node1.getFScore() - node2.getFScore());
                current = openSet[0];

                Node[] neighboors = getNeighboorNodes(current, gridNodes);
                foreach(Node neighboor in neighboors)
                {
                    if (closedSet.Contains(neighboor))//If node evaluated
                        continue;

                    if (!openSet.Contains(neighboor))
                    {
                        neighboor.update(current);
                        openSet.Add(neighboor);
                    }
                    else
                    {
                        if (neighboor.update(current))
                        {
                            openSet.Remove(neighboor);
                            openSet.Add(neighboor);
                        }
                    }

                    if (neighboor.getPosition().Equals(goal))
                    {
                        return neighboor;
                    }
                }
                closedSet.Add(current);
            }
            return null;
        }

        private static Node[] getNeighboorNodes(Node current, Node[,] gridNodes)
        {
            Node[] neighboors = new Node[8];
            int count = 0;
            for (int x = current.getPosition().getX(); x <= current.getPosition().getX() + 1; x++)
            {
                for (int y = current.getPosition().getY(); y <= current.getPosition().getY() + 1; y++)
                {
                    if (x < 0 || y < 0 || x > gridNodes.GetUpperBound(0) || y > gridNodes.GetUpperBound(1))
                    {
                        continue;
                    }
                    neighboors[count++] = gridNodes[x, y];
                }
            }
            return neighboors;
        }
    }
}