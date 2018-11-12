using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Utility
{
    internal class WalkableTile
    {
        public const string Wall = "Wall";
        public const string Floor = "Floor";
        public const string Nothing = "Nothing";

        private string type;

        public WalkableTile()
        {
            this.type = Nothing;
        }

        public WalkableTile(string type)
        {
            this.type = type;
        }

        public bool isWalkable()
        {
            return this.type != Wall && this.type != Floor;
        }
    }
}
