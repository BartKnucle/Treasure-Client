using System;
using System.Collections.Generic;

namespace FunkySheep.OSM
{
    [Serializable]
    public class Way : Element
    {
        public List<Node> nodes = new List<Node>();
        public List<Tag> tags = new List<Tag>();
        public Tile tile;
        public Way(int id)
        {
          this.id = id;
        }
    }
}