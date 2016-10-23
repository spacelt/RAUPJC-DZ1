using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong
{
    public class CollisionDetector
    {
        public static bool Overlaps(IPhysicalObject2D a, IPhysicalObject2D b)
        {
            return a.X + a.Width < b.X || 
                b.X + b.Width < a.X ||
                a.Y + a.Height < b.Y || 
                b.Y + b.Height < a.Y ? 
                false : true;
        }
    }
}
