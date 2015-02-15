using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Circular.Decorations
{
     [Serializable]
    public class DecorationAnchor
    {
        /// <summary>
        /// value from 0-1 that indicates the goodness of this spot for an anchor
        /// </summary>
        public double Goodness { get; set; }

        public Point[] ControlPoints { get; set; }

        public float LineThickness { get; set; }

        public iAnchorHolder Owner { get; set; }

        public string Name { get; set; }

        public DecorationAnchor(Point[] Points, double goodness, float penWidth, iAnchorHolder owner)
        {
            this.ControlPoints = Points;
            this.LineThickness = penWidth;
            this.Goodness = goodness;
            this.Owner = owner;
        }


        public DecorationAnchor(Point[] Points, double goodness, float penWidth, iAnchorHolder owner,string name)
        {
            this.ControlPoints = Points;
            this.LineThickness = penWidth;
            this.Goodness = goodness;
            this.Owner = owner;
            this.Name = name;
        }
    }
}
