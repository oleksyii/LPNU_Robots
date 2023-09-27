using Robot.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OleksiiUzhva_RobotChallange
{
    public class Cell
    {
        
        public Cell(Position pos, bool free) 
        {
            this.position = pos;
            this.free = free;
        }

        public Cell(Position pos)
        {
            this.position=pos;
        }

        public Position position { get; set; }
        public bool free { get; set; } = true;
        
        
        
    }
}
