using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using Circular.LetterShapes;

using Circular.Words;
using Circular.Decorations.Shapes;
using System.ComponentModel;

namespace Circular.Decorations
{
    [Serializable]
    public abstract class aDecoration : iAnchorHolder
    {

        ///////////////////////////////////////////////////////////////////////////////////////
        public abstract aDecoration HandlesEngLetter(engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle);

        protected abstract void CalcDecoration(double arcRadius, double arcMidAngle, double arcX, double arcY, double arcWidth);

        public abstract LocationProb LocationProb();

        public virtual void DrawDec(ref Graphics path, Color backgroundColor, Color foregroundColor, bool mockup)
        {
            foreach (var dec in _DecorationDots)
                dec.DrawDot(ref path, backgroundColor, foregroundColor, mockup);

        }

        public void CalculateDecoration(double arcRadius, double arcMidAngle, double arcX, double arcY, double arcWidth)
        {
            _Anchors = new Dictionary<string, DecorationAnchor>();
            _Sources = new Dictionary<string, DecorationAnchor>();
            _DecorationDots.Clear();

            CalcDecoration(arcRadius, arcMidAngle, arcX, arcY, arcWidth);
        }
        ///////////////////////////////////////////////////////////////////////////////////////

        public static aDecoration FindDecoration(engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {


            aDecoration[] Mothers;


            if (scriptStyle == Circular.aCircleObject.ScriptStyles.Ashcroft)
            {
                Mothers = new aDecoration[] 
            {
                new Rings(), 
                new ThreeDots(),
                new TwoDots(),
                new TwoLines()
              };
            }
            else
            {
                Mothers = new aDecoration[] 
            {
                new OneLine(), 
                new ThreeDots(),
                new TwoDots(),
                new TwoLines(),
                new ThreeLines()
              };

            }

            for (int i = 0; i < Mothers.Length; i++)
            {
                if (letter != null && letter.Consonant != null)
                {
                    var t = Mothers[i].HandlesEngLetter(letter, scriptStyle);

                    if (t != null)
                        return t;
                }
            }
            return null;
        }

        protected DecorationLocation _Location;
        [CategoryAttribute("Appearance")]
        public DecorationLocation Location
        {
            get
            {
                return _Location;
            }
            set
            {
                _Location = value;
                _Syllable.Redraw();
            }
        }

        protected LetterShapes.aSyllable _Syllable;
        protected aCircleObject _Word;

        protected List<DecorationDot> _DecorationDots;

        protected Dictionary<string, DecorationAnchor> _Anchors = new Dictionary<string, DecorationAnchor>();
        protected Dictionary<string, DecorationAnchor> _Sources = new Dictionary<string, DecorationAnchor>();

        ///////////////////////////////////////////////////////////////////////////////////////
        [BrowsableAttribute(false)]
        public Dictionary<string, DecorationAnchor> Anchors
        {
            get
            {

                return _Anchors;
            }
        }
        [BrowsableAttribute(false)]
        public Dictionary<string, DecorationAnchor> Sources
        {
            get
            {

                return _Sources;
            }
        }

        public virtual void Initialize(LetterShapes.aSyllable Letter, aCircleObject WordParent)
        {
            this._Location = _Location;
            this._Syllable = Letter;
            this._Word = WordParent;

            double wX = 0;
            double wY = 0;
            double wR = WordParent.Radius;

            _DecorationDots = new List<DecorationDot>();
        }

        public aDecoration() { }

        public override string ToString()
        {
            string[] parts = this.GetType().ToString().Split('.');
            return parts[parts.Length - 1];
        }
    }
}
