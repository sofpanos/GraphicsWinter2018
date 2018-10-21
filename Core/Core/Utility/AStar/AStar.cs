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
            Node.start = start;
            Node.goal = goal;
            Node[,] gridNodes = createGridNodes(grid);
            
            Node finalNode = findPath(gridNodes, start, goal);

            if (finalNode == null)
            {
                return new List<Position>();
            }

            return finalNode.getPathToStart();
        }

        private static Node[,] createGridNodes(WalkableTile[,] grid)
        {
            Node[,] gridNodes = new Node[grid.GetUpperBound(0) + 1, grid.GetUpperBound(1) + 1];
            for (int x = 0; x <= gridNodes.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= gridNodes.GetUpperBound(1); y++)
                {
                    bool isWalkable = grid[x, y].isWalkable();
                    gridNodes[x, y] = new Node(new Position(x, y), isWalkable);
                }
            }
            return gridNodes;
        }
        
        private static Node findPath(Node[,] gridNodes, Position start, Position goal)
        {
            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>(new NodeEqualityComparer());

            Node current = gridNodes[Node.start.getX(), Node.start.getY()];
            openSet.Add(current);

            while (openSet.Count != 0)
            {
                openSet.Sort((node1, node2) => node1.getFScore() - node2.getFScore());
                current = openSet[0];
                openSet.Remove(current);
                closedSet.Add(current);

                if (current.Goal)
                    return current;

                List<Node> neighbors = getNeighborNodes(current, gridNodes);
                foreach (Node neighbor in neighbors)
                {

                    
                    if (closedSet.Contains(neighbor)|| !neighbor.Valid)
                    {
                        continue;
                    }

                    int newGScore = neighbor.calculateNewGScore(current);
                    
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                    else if (newGScore >= neighbor.GScore)
                    {
                        continue;
                    }

                    neighbor.PreviousNode = current;
                    neighbor.GScore = newGScore;
                }
            }
            return null;
        }

        private static List<Node> getNeighborNodes(Node current, Node[,] gridNodes)
        {
            List<Node> neighbors = new List<Node>(8);
            for (int x = current.Position.getX() - 1; x <= current.Position.getX() + 1; x++)
            {
                for (int y = current.Position.getY() - 1; y <= current.Position.getY() + 1; y++)
                {
                    if (x < 0 || y < 0 || x > gridNodes.GetUpperBound(0) || y > gridNodes.GetUpperBound(1))
                        continue;
                    else if (x == current.Position.getX() && y == current.Position.getY())
                        continue;
                    
                    neighbors.Add(gridNodes[x, y]);
                }
            }
            return neighbors;
        }
    }
}