using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circular.Decorations
{
   public  interface iAnchorHolder
    {
       Dictionary<string, DecorationAnchor> Anchors { get; }
       Dictionary<string, DecorationAnchor> Sources { get; }
    }
}
