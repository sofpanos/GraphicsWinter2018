using System;
using System.Collections.Generic;

namespace Core.Utility.AStar
{
    public class Node
    {
        public static Position start;
        public static Position goal;
        
        public Node(Position pos, bool validPath)
        {
            this.Position = pos;
            this.Start = Node.start.Equals(this.Position);
            this.Goal = Node.goal.Equals(this.Position);
            if (this.Start || this.Goal){
                this.Valid = true;
                if (this.Start)
                {
                    this.GScore = 0;
                }
            }   
            else
                this.Valid = validPath;
            if(!this.Start)
                this.GScore = int.MaxValue;
            
            calculateHScore();
        }

        public bool Start
        {
            get;
            private set;
        }

        public bool Goal 
        { 
            get; private set; 
        }

        public bool Valid { get; private set; }

        public Position Position { get; set; }

        public Node PreviousNode { get; set; }

        public int HScore { get; private set; }

        public int GScore { get; set; }
        

        public int getFScore()
        {
            return this.GScore + this.HScore;
        }

        public List<Position> getPathToStart()
        {
            return getPathToStartRecurs(this);
        }

        

        public int distanceCost(Position pos)
        {
            if (this.Position.getX() == pos.getX() || this.Position.getY() == pos.getY())
            {
                return AStar.VERTICAL_HORIZONTAL_SCORE;
            }
            else
            {
                return AStar.DIAGONAL_SCORE;
            }
        }

        public int calculateNewGScore(Node newPrevious)
        {
            return distanceCost(newPrevious.Position) + newPrevious.GScore;
        }

        private void calculateHScore()
        {
            int dX = Math.Abs(this.Position.getX() - Node.goal.getX());
            int dY = Math.Abs(this.Position.getY() - Node.goal.getY());
            this.HScore = AStar.DIAGONAL_SCORE * Math.Min(dX, dY) + AStar.VERTICAL_HORIZONTAL_SCORE *
                (Math.Max(dX, dY) - Math.Min(dX, dY));
        }

        public bool Equals(Node other)
        {
            if (other == null)
            {
                return false;
            }
            if (this.Position.Equals(other.Position))
            {
                return true;
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if(obj is Node)
                return Equals((Node)obj);
            return false;
        }

        public override int GetHashCode()
        {
            return this.Position.GetHashCode();
        }

        private static int recursCalcGScore(Node theNode)
        {
            if (theNode.Start)
            {
                return 0;
            }

            return theNode.distanceCost(theNode.PreviousNode.Position) + recursCalcGScore(theNode.PreviousNode);
        }

        private static List<Position> getPathToStartRecurs(Node node)
        {
            List<Position> thePath = new List<Position>();
            thePath.Add(node.Position);
            if (node.Start)
            {
                return thePath;
            }

            List<Position> previousItems = getPathToStartRecurs(node.PreviousNode);
            thePath.AddRange(previousItems);
            return thePath;
        }
    }
}