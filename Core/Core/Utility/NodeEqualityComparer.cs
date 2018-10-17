﻿using System;
using System.Collections.Generic;
using Core.Utility.AStar;

namespace Core.Utility
{
    public class NodeEqualityComparer : IEqualityComparer<Node>
    {
        public bool Equals(Node node1, Node node2)
        {
            return node1.Equals(node2);
        }

        public int GetHashCode(Node node1)
        {
            return node1.getPosition().getX() + node1.getPosition().getY();
        }
    }
}