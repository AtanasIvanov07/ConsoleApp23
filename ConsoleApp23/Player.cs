using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp23
{
    public class Player
    {
        public string Name { get; set; }
        public string Position { get; set; }

        public Player(string name, string position)
        {
            Name = name;
            Position = position;
        }

        public override string ToString()
        {
            return $"{Name} ({Position})";
        }
    }
}
