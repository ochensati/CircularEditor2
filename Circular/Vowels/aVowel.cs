using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using Circular.Words;
using Circular.Vowels.Shapes;
using Circular.Decorations;
using System.ComponentModel;

namespace Circular.Vowels
{
    [Serializable]
    public abstract class aVowel : iAnchorHolder, iMouseable
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public abstract aVowel HandlesEngLetter(int vowelIndex, engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle);
        public abstract void DrawVowelImpl(ref Graphics path, Color backgroundColor, Color foregroundColor, bool mockup);
        protected abstract void CalcVowel(Rectangle VowelBounds, double vX, double vY, double vR);

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        
        public static aVowel FindLetter(int vowelIndex, engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {
            aVowel[] Mothers;


            if (scriptStyle == Circular.aCircleObject.ScriptStyles.Ashcroft || scriptStyle == Circular.aCircleObject.ScriptStyles.Small)
            {
                Mothers = new aVowel[] 
                {
                    new CenterDot(),
                    new CrossLine(),
                    new Half(),
                    new TopDot(),
                    new TwoDot(),
               
                  };
            }
            else
            {
                Mothers = new aVowel[] 
                {
                    new LorenA(),
                    new LorenE(),
                    new LorenI(),
                    new LorenO(),
                    new LorenU()
               
                  };

            }
            for (int i = 0; i < Mothers.Length; i++)
            {
                var t = Mothers[i].HandlesEngLetter(vowelIndex, letter, scriptStyle);
                if (t != null)
                    return t;
            }
            return null;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void DrawVowel(ref Graphics path, Color backgroundColor, Color foregroundColor, bool mockup)
        {
            var p = new Point[] { new Point(VowelBounds.X + VowelBounds.Width/2,VowelBounds.Y + VowelBounds.Height/2)  };
            path.Transform.TransformPoints(p);
            RealCenter=p[0];
            DrawVowelImpl(ref  path,  backgroundColor,  foregroundColor,  mockup);
        }

        public iMouseable HitTest(Point p)
        {
            double r = MathHelps.distance(this.VowelBounds, p);

        
            if (r < (VowelBounds.Width / 2d))
            {
                return this;
            }
            else
                return null;
        }

        [BrowsableAttribute(false)]
        public Dictionary<string, DecorationAnchor> Anchors { get; protected set; }
        [BrowsableAttribute(false)]
        public Dictionary<string, DecorationAnchor> Sources { get; protected set; }

        public Point RealCenter
        {
            get;
            protected set;
        }

        public Point GetStartAngle(Point mousePoint, out double startAngle)
        {
            Point center = new Point(RealCenter.X, RealCenter.Y);
            startAngle = MathHelps.Atan2(center, mousePoint) - CircleAngle;

            return center;
        }
        public void SetAngleDelta(double deltaAngle)
        {
            this.CircleAngle = deltaAngle;
        }
        public void MoveDelta(Point originalPoint, int deltaX, int deltaY)
        {
            //double r = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            //double theta = MathHelps.Atan2(deltaY, deltaX) -_Word. TotalAngle;
            //DrawCenter = MathHelps.D2Coords(originalPoint, r, theta);
            //Redraw();
        }

        [CategoryAttribute("Read-Only")]
        public Point DrawCenter
        {
            get
            {
                return new Point((int)(VowelBounds.X + VowelBounds.Width / 2), (int)(VowelBounds.Y + VowelBounds.Height / 2));
            }
            set
            {
                VowelBounds = new Rectangle((int)(value.X - VowelBounds.Width / 2), (int)(value.Y - VowelBounds.Height / 2), VowelBounds.Width, VowelBounds.Height);
                if (this._Word != null)
                    this._Word.Redraw();
            }
        }

        public void Redraw()
        {
            if (this._Word != null)
                this._Word.Redraw();
        }

        [CategoryAttribute("Read-Only")]
        public Rectangle VowelBounds { get; protected set; }

        protected VowelLocation _Location;
        [CategoryAttribute("Appearance")]
        public VowelLocation Location { get { return _Location; } set { _Location = value; _Syllable.Redraw(); } }


        protected double _CircleAngle = rnd.NextDouble() * 360;
        [CategoryAttribute("Appearance")]
        public double CircleAngle
        {
            get
            {
                return _CircleAngle;
            }
            set
            {
                _CircleAngle = value;
                _Syllable.Redraw();
            }
        }

        protected LetterShapes.aSyllable _Syllable;
        protected aCircleObject _Word;
        protected Point[] _ControlPoints;
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public void CalculateVowel(Rectangle VowelBounds, double vX, double vY, double vR)
        {
            Anchors = new Dictionary<string, DecorationAnchor>();
            Sources = new Dictionary<string, DecorationAnchor>();
            this.VowelBounds = VowelBounds;

            CalcVowel(VowelBounds, vX, vY, vR);
        }

        protected static Random rnd = new Random();
        public void Initalize(aCircleObject Word, LetterShapes.aSyllable parent)
        {
            _Word = Word;
            _Syllable = parent;


            Anchors = null;

        }

        public aVowel()
        {
            Anchors = new Dictionary<string, DecorationAnchor>();
            Sources = new Dictionary<string, DecorationAnchor>();
        }

        public override string ToString()
        {
            string[] parts = this.GetType().ToString().Split('.');
            return _Syllable.Syllable.Vowel + " " + parts[parts.Length - 1];
        }
    }
}
