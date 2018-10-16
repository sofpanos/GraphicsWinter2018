using System;
using System.Collections.Generic;

namespace Core.Utility.AStar
{
    public class Node
    {
        private Position nodePosition;
        private Position goal;
        private Node previousNode;
        private bool start;
        private bool validPathNode;
        private int gScore;
        private int hScore;

        public Node(Position pos, Position goal, bool validPath)
        {
            this.nodePosition = pos;
            this.goal = goal;
            this.validPathNode = validPath;
            this.start = false;
            this.gScore = int.MaxValue;
        }

        public Node(Position pos, Position goal, bool validPath, bool start)
        {
            this.nodePosition = pos;
            this.goal = goal;
            this.validPathNode = validPath;
            this.start = start;
            this.gScore = 0;
            this.hScore = 0;
        }

        public Node getPreviousNode()
        {
            return this.previousNode;
        }

        public Position getPosition()
        {
            return this.nodePosition;
        }

        public void setValidPathNode(bool value)
        {
            this.validPathNode = value;
        }

        public int getFScore()
        {
            return this.gScore + this.hScore;
        }

        public int getPathLengthToStart()
        {
            return findPathLengthRecurs();
        }

        public List<Position> getPathToStart()
        {
            return getPathToStartRecurs(this);
        }

        public bool isValidPathNode()
        {
            return this.validPathNode;
        }

        public int nextNodeCost(Position nextPos)
        {
            if (this.nodePosition.getX() == nextPos.getX() || this.nodePosition.getY() == nextPos.getY())
            {
                return AStar.VERTICAL_HORIZONTAL_SCORE;
            }
            else
            {
                return AStar.DIAGONAL_SCORE;
            }
        }

        public bool update(Node previous)
        {
            if (calculateGScore(previous))
            {
                calculateHScore();
                return true;
            }
            return false;
        }

        private bool calculateGScore(Node previous)
        {
            if (previous == null)
            {
                gScore = int.MaxValue;
                return true;
            }

            if (this.previousNode == null && !this.start)
            {
                this.previousNode = previous;
                this.gScore = recursCalcGScore(previous);
                return true;
            }

            int newGScore = nextNodeCost(previous.nodePosition) + recursCalcGScore(previous);
            if (newGScore <= this.gScore)
            {
                this.previousNode = previous;
                this.gScore = newGScore;
                return true;
            }
            return false;
        }

        private void calculateHScore()
        {
            int dX = Math.Abs(this.nodePosition.getX() - this.goal.getX());
            int dY = Math.Abs(this.nodePosition.getY() - this.goal.getY());
            this.hScore = AStar.DIAGONAL_SCORE * Math.Min(dX, dY) + AStar.VERTICAL_HORIZONTAL_SCORE *
                (Math.Max(dX, dY) - Math.Min(dX, dY));
        }

        private int findPathLengthRecurs()
        {
            if (this.start)
            {
                return 0;
            }

            return this.previousNode.findPathLengthRecurs() + 1;
        }

        public bool Equals(Node other)
        {
            if (other == null)
            {
                return false;
            }
            if (this.nodePosition.Equals(other.nodePosition))
            {
                return true;
            }
            return false;
        }

        private static int recursCalcGScore(Node theNode)
        {
            if (theNode.start)
            {
                return 0;
            }

            return theNode.nextNodeCost(theNode.previousNode.nodePosition) + recursCalcGScore(theNode.previousNode);
        }

        private static List<Position> getPathToStartRecurs(Node node)
        {
            List<Position> thePath = new List<Position>();
            thePath.Add(node.nodePosition);
            if (node.start)
            {
                return thePath;
            }

            List<Position> previousItems = getPathToStartRecurs(node.previousNode);
            thePath.AddRange(previousItems);
            return thePath;
        }
    }
}