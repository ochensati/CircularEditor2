using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using Circular.Vowels;

using Circular.Words;
using Circular.LetterShapes.Shapes;
using Circular.Decorations;
using System.ComponentModel;

namespace Circular.LetterShapes
{
    [Serializable]
    public abstract class aSyllable : iAnchorHolder, iArcJoin, iMouseable
    {

        ///////////////////////////////////////////////////////////////////////////////////////

        public abstract aSyllable HandlesEngLetter(engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle);

        public abstract void CalculateArc();

        protected abstract void DrawArc(ref Graphics path, ref GraphicsPath border, Color backgroundColor, Color foregroundColor, bool mockup);

        protected abstract LocationProb LocationProb();

        protected abstract void CalculateDecoration(Decorations.aDecoration decoration);


        public virtual double PreferredAngle()
        {
            return 0;
        }

        public virtual void UseWordForArc(aCircleObject otherWord)
        {

        }

        public virtual void CalculateVowel(aVowel vowel)
        {
            double x0 = LetterBounds.X + LetterBounds.Width / 2;
            double y0 = LetterBounds.Y + LetterBounds.Height / 2;
            double r = (LetterBounds.Width / 2.0);
            double vR = r / 2;
            double vX = 0;
            double vY = 0;
            Rectangle VowelBounds = new Rectangle();

            switch (vowel.Location)
            {
                #region Locations

                case VowelLocation.Center:
                    {
                        vR = r * .8;
                        vX = x0;
                        vY = y0;
                        VowelBounds = MathHelps.Circle2Rect(vX, vY, vR);
                        break;
                    }
                case VowelLocation.HighTop:
                    {
                        vR = r * 2;

                        double x1 = 0;
                        double y1 = 0;

                        vX = (x0 + x1) / 2;
                        vY = (y0 + y1) / 2;
                        VowelBounds = MathHelps.Circle2Rect(vX, vY, vR);
                        break;
                    }
                case VowelLocation.Top:
                    {
                        vR = r * .5;

                        double x1 = 0;
                        double y1 = 0;

                        vX = (x0 + x1) / 2;
                        vY = (y0 + y1) / 2;
                        VowelBounds = MathHelps.Circle2Rect(vX, vY, vR);// new Rectangle((int)(vX - vR), (int)(vY - vR), (int)(2 * vR), (int)(2 * vR));
                        break;
                    }
                case VowelLocation.Left:
                    {
                        vR = r * .5;

                        double x1 = 0;
                        double y1 = 0;

                        double bRadius = _WordParent.Radius - LetterRadius / 2;
                        double bAngle = MidAngle + ArcWidth * .35;

                        Point v = MathHelps.D2Coords(new Point((int)0, (int)0), bRadius, bAngle);
                        vX = v.X; vY = v.Y;


                        VowelBounds = MathHelps.Circle2Rect(vX, vY, vR);


                        break;
                    }

                case VowelLocation.Right:
                    {
                        vR = r * .5;

                        double x1 = 0;
                        double y1 = 0;

                        double bRadius = _WordParent.Radius - LetterRadius / 2;
                        double bAngle = MidAngle - ArcWidth * .35;

                        Point v = MathHelps.D2Coords(new Point((int)0, (int)0), bRadius, bAngle);
                        vX = v.X; vY = v.Y;


                        VowelBounds = MathHelps.Circle2Rect(vX, vY, vR);


                        break;
                    }

                case VowelLocation.Below:
                    {
                        double x1 = 0;
                        double y1 = 0;

                        double bRadius = _WordParent.Radius * 1.2;
                        double bAngle = MidAngle;

                        Point v = MathHelps.D2Coords(new Point((int)0, (int)0), bRadius, bAngle);
                        vX = v.X; vY = v.Y;


                        VowelBounds = MathHelps.Circle2Rect(vX, vY, vR);


                        break;
                    }
                case VowelLocation.Above:
                    {
                        vR = r * .5;

                        double x1 = 0;
                        double y1 = 0;

                        vX = (x0 + x1) / 2;
                        vY = (y0 + y1) / 2;
                        VowelBounds = MathHelps.Circle2Rect(vX, vY, vR);// new Rectangle((int)(vX - vR), (int)(vY - vR), (int)(2 * vR), (int)(2 * vR));
                        break;
                    }
                case VowelLocation.OnLine:
                    {
                        double x1 = 0;
                        double y1 = 0;

                        double bRadius = _WordParent.Radius;
                        double bAngle = MidAngle;

                        Point v = MathHelps.D2Coords(new Point((int)0, (int)0), bRadius, bAngle);
                        vX = v.X; vY = v.Y;


                        VowelBounds = MathHelps.Circle2Rect(vX, vY, vR);


                        break;

                    }
                #endregion
            }

            vowel.CalculateVowel(VowelBounds, vX, vY, vR);
        }
        ///////////////////////////////////////////////////////////////////////////////////////
        public static aSyllable FindLetter(engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {


            aSyllable[] Mothers;


            if (scriptStyle == Circular.aCircleObject.ScriptStyles.Ashcroft)
            {
                Mothers = new aSyllable[] 
                {
                    new VowelOnly(),
                    new Blank(),
                    new Arc(),
                    new FullMoon(),
                    new Harp(),
                    new MoonRise(),
                    new Sunrise(),
                    new SliverMoon(),
                    new Spiral(),
                    new Connector()
                };
            }
            else
            {
                if (scriptStyle == Circular.aCircleObject.ScriptStyles.Small)
                {
                    Mothers = new aSyllable[] 
                {
                    new SmallVowelOnly(),
                    new Blank(),
                    new SmallArc(),
                    new SmallFullMoon(),
                    new SmallSaturn(),
                    new SmallHarp(),
                    new SmallMoonRise()
                };


                }
                else
                {
                    Mothers = new aSyllable[] 
                {
                    new VowelOnly(),
                    new Blank(),
                    new Arc(),
                    new FullMoon(),
                    new Saturn(),
                    new MoonRise()
                };
                }
            }



            for (int i = 0; i < Mothers.Length; i++)
            {
                var t = Mothers[i].HandlesEngLetter(letter, scriptStyle);
                if (t != null)
                    return t;
            }
            return null;
        }


        [CategoryAttribute("Appearance")]
        public engLetter Syllable { get; protected set; }

        [CategoryAttribute("Appearance")]
        public string Gal_Letter { get { return this.GetType().ToString(); } }


        public void MoveDelta(Point originalPoint, int deltaX, int deltaY)
        {
            double r = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            double theta = MathHelps.Atan2(deltaY, deltaX) - _WordParent. TotalAngle;
            DrawCenter = MathHelps.D2Coords(originalPoint, r, theta);
            Redraw();
        }

        public Point GetStartAngle(Point mousePoint, out double startAngle)
        {
            Point center = new Point(RealCenter.X , RealCenter.Y );
            startAngle = MathHelps.Atan2(center, mousePoint) - CircleAngle;

            return center;
        }
        public void SetAngleDelta(double deltaAngle)
        {
            this.CircleAngle = deltaAngle;
        }


        [CategoryAttribute("Read-Only")]
        public Point DrawCenter
        {
            get { return LetterCenter; }
            set
            {
                LetterBounds = new Rectangle((int)(value.X - LetterBounds.Width / 2), (int)(value.Y - LetterBounds.Height / 2), LetterBounds.Width,LetterBounds.Height);
                this._WordParent.Redraw();
            }
        }

        private double _CircleAngle = 0;
        [CategoryAttribute("Read-Only")]
        public double  CircleAngle
        {
            get { return _CircleAngle; }
            set
            {
                _CircleAngle = value;

                this._WordParent.Redraw();
            }
        } 


        [CategoryAttribute("Read-Only")]
        public Point LetterCenter { get { return new Point((int)(LetterBounds.X + LetterBounds.Width / 2), (int)(LetterBounds.Y + LetterBounds.Height / 2f)); } }

        public Point RealCenter
        {
            get;
            private  set;
        }

        [CategoryAttribute("Read-Only")]
        public Rectangle LetterBounds { get; protected set; }
        [CategoryAttribute("Read-Only")]
        public double LetterRadius { get; protected set; }

        public float StartAngle
        {
            get;
            protected set;
        }

        [CategoryAttribute("Read-Only")]
        public float MidAngle { get; protected set; }
        [CategoryAttribute("Read-Only")]
        public float EndAngle { get; protected set; }
        [CategoryAttribute("Read-Only")]
        public float ArcWidth { get; protected set; }

        [CategoryAttribute("Read-Only")]
        public float SubStartAngle { get; protected set; }
        [CategoryAttribute("Read-Only")]
        public float SubEndAngle { get; protected set; }
        [CategoryAttribute("Read-Only")]
        public float SubArc { get; protected set; }

        protected bool _Big = false;
        [CategoryAttribute("Appearance")]
        public bool Big
        {
            get
            {
                return _Big;

            }
            set
            {
                _Big = value;
                Redraw();
            }
        }

        protected bool _Fancy = true;
        [CategoryAttribute("Appearance")]
        public bool Fancy
        {
            get
            {
                return _Fancy;

            }
            set
            {
                _Fancy = value;
                Redraw();
            }
        }


        [CategoryAttribute("Appearance")]
        public List<Circular.Vowels.aVowel> Vowels { get; set; }



        protected Dictionary<string, DecorationAnchor> _Anchors = new Dictionary<string, DecorationAnchor>();
        protected Dictionary<string, DecorationAnchor> _Sources = new Dictionary<string, DecorationAnchor>();
        [BrowsableAttribute(false)]
        public Dictionary<string, DecorationAnchor> Anchors
        {
            get
            {
                Dictionary<string, DecorationAnchor> p = new Dictionary<string, DecorationAnchor>();
                int cc = 0;
                foreach (var vowel in Vowels)
                {

                    foreach (var kvp in vowel.Anchors)
                    {
                        p.Add(kvp.Key + vowel.ToString() + cc, kvp.Value);
                        cc++;
                    }
                }

                foreach (var dec in Decorations)
                {
                    foreach (var kvp in dec.Anchors)
                        p.Add(kvp.Key, kvp.Value);

                }

                foreach (var kvp in _Anchors)
                    p.Add(kvp.Key, kvp.Value);


                return p;
            }
        }
        [BrowsableAttribute(false)]
        public Dictionary<string, DecorationAnchor> Sources
        {
            get
            {
                Dictionary<string, DecorationAnchor> p = new Dictionary<string, DecorationAnchor>();
                foreach (var vowel in Vowels)
                {
                    foreach (var kvp in vowel.Sources)
                        p.Add(kvp.Key, kvp.Value);
                }

                foreach (var dec in Decorations)
                {
                    foreach (var kvp in dec.Sources)
                        p.Add(kvp.Key, kvp.Value);

                }

                foreach (var kvp in _Sources)
                    p.Add(kvp.Key, kvp.Value);


                return p;
            }
        }


        [CategoryAttribute("Appearance")]
        public List<Decorations.aDecoration> Decorations { get; set; }

        protected double _gapAngle1;
        protected double _gapAngle2;

        protected aCircleObject _WordParent;

        protected Point[] _edges;

        protected double[] _mainAngles;
        protected double[] _subAngles;


        protected static Random rnd = new Random();
        ///////////////////////////////////////////////////////////////////////////////////////

        public iMouseable HitTest(Point p)
        {

            foreach (var v in Vowels)
            {
                var o = v.HitTest(p);
                if (o != null)
                    return o;
            }

          

            double r = MathHelps.distance(this.LetterBounds, p);
            if (r < (this.LetterRadius))
            {
                return this;
            }
            else
                return null;
        }

        public virtual void DrawLetter(ref Graphics path, ref GraphicsPath border, Color backgroundColor, Color foregroundColor, bool mockup)
        {
            var p = new Point[] { LetterCenter };
            path.Transform.TransformPoints(p);
            RealCenter = p[0];

            DrawArc(ref path, ref border, backgroundColor, foregroundColor, mockup);


            foreach (var v in Vowels)
                v.DrawVowel(ref path, backgroundColor, foregroundColor, mockup);

            if (mockup == false)
            {
                foreach (var d in Decorations)
                    d.DrawDec(ref path, backgroundColor, foregroundColor, mockup);
            }
        }


        public void Initialize(aCircleObject parent, Circular.aCircleObject.ScriptStyles scriptStyle, double startAngle, double endAngle, engLetter letter, bool big, bool fancy)
        {
            _doRedraw = false;
            this._WordParent = parent;
            Syllable = letter;

            this.StartAngle = (float)startAngle;
            this.EndAngle = (float)endAngle;

            _Big = big;
            _Fancy = fancy;

            ArcWidth = (float)(endAngle - startAngle);

            MidAngle = (float)(startAngle + ArcWidth / 2);


            if (ArcWidth > 150)
            {
                _gapAngle1 = MidAngle - 90;
                _gapAngle2 = MidAngle + 90;

            }
            else
            {
                double gap = 1;
                _gapAngle1 = MidAngle - ArcWidth / 2 * gap;
                _gapAngle2 = MidAngle + ArcWidth / 2 * gap;
            }

            startPoint = MathHelps.D2Coords(new Point((int)0, (int)0), _WordParent.Radius, startAngle);
            //new Point((int)(WordParent.WordCenter.X + WordParent.WordRadius * Math.Cos(startAngle / 180 * Math.PI)), (int)(WordParent.WordCenter.Y + WordParent.WordRadius * Math.Sin(startAngle / 180 * Math.PI)));

            // CalculateAppearance(letter);
            Vowels = new List<aVowel>();
            Decorations = new List<Circular.Decorations.aDecoration>();

            CalculateArc();
            AddVowel(letter, scriptStyle);
            AddDecoration(letter, scriptStyle);


            AddAnchor("Edge", new DecorationAnchor(new Point[] { startPoint }, .001, 1, this));

            CalcPositions();

            foreach (var v in Vowels)
            {
                if (v != null)
                    CalculateVowel(v);
            }
            foreach (var d in Decorations)
            {
                if (d != null)
                    CalculateDecoration(d);
            }
            _doRedraw = true;

        }

        public void AddSource(string key, DecorationAnchor decoration)
        {
            if (!_Sources.ContainsKey(key))
            {
                _Sources.Add(key, decoration);
            }
            else
            {
                _Sources.Remove(key);
                _Sources.Add(key, decoration);

            }
        }

        public void AddAnchor(string key, DecorationAnchor decoration)
        {
            if (!_Anchors.ContainsKey(key))
            {
                _Anchors.Add(key, decoration);
            }
            else
            {
                _Anchors.Remove(key);
                _Anchors.Add(key, decoration);

            }
        }

        protected bool _doRedraw = true;

        public void Redraw()
        {
            if (_doRedraw)
            {
                _doRedraw = false;
                _Anchors = new Dictionary<string, DecorationAnchor>();
                _Sources = new Dictionary<string, DecorationAnchor>();

                CalculateArc();
                foreach (var v in Vowels)
                {
                    if (v != null)
                        CalculateVowel(v);
                }



                foreach (var d in Decorations)
                {
                    if (d != null)
                        CalculateDecoration(d);
                }

                _WordParent.Redraw();

                _doRedraw = true;
            }
        }

        public virtual void CalcPositions()
        {
            LocationProb prob = this.LocationProb();

            foreach (var d in Decorations)
                if (d != null)
                    prob = Circular.Words.LocationProb.Multiply(prob, d.LocationProb());


            if (Vowels.Count == 1)
            {
                var v = Vowels[0];
                if (v != null)
                    v.Location = prob.GetVowelLocation();
            }

            if (Vowels.Count == 2)
            {
                var v = Vowels[0];
                if (v != null)
                    v.Location = VowelLocation.Right;
                v = Vowels[1];
                if (v != null)
                    v.Location = VowelLocation.Left;
            }

            if (Vowels.Count == 3)
            {
                var v = Vowels[0];
                if (v != null)
                    v.Location = VowelLocation.Right;

                v = Vowels[1];
                if (v != null)
                    v.Location = VowelLocation.Top;
                v = Vowels[2];
                if (v != null)
                    v.Location = VowelLocation.Left;
            }

            foreach (var d in Decorations)
                if (d != null)
                    d.Location = prob.GetDecLocation();
        }

        public virtual void AddDecoration(engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {
            var t = Circular.Decorations.aDecoration.FindDecoration(letter, scriptStyle);
            if (t != null)
            {
                t.Initialize(this, _WordParent);
                Decorations.Add(t);
            }
        }

        public virtual void AddVowel(engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {

            if (letter != null && letter.Vowel != null)
            {
                for (int i = 0; i < letter.Vowel.Length; i++)
                {
                    var t = Circular.Vowels.aVowel.FindLetter(i, letter, scriptStyle);
                    if (t != null)
                    {
                        t.Initalize(_WordParent, this);
                        Vowels.Add(t);
                    }
                }
            }


        }

        public aSyllable() { }


        #region Math

        protected void FindEdges()
        {
            _edges = FindIntersections(LetterBounds);

            if (_edges == null)
            {
                SubArc = 360;
            }
            else
            {
                _mainAngles = MathHelps.GetAngles(_edges,new Point((int)0, (int)0));
                // subAngles = MathHelps.GetAngles(edges, WordParent.WordCenter, LetterBounds);
                _subAngles = MathHelps.GetAngles(_edges,new Point((int)0, (int)0), LetterBounds);


                if (_mainAngles[1] < _mainAngles[0])
                    _mainAngles[1] += 360;

                if (_subAngles[1] < _subAngles[0])
                    _subAngles[1] += 360;

                SubStartAngle = (float)_subAngles[0];
                SubEndAngle = (float)_subAngles[1];



                SubArc = (float)(-1 * Math.Abs((_subAngles[1]) - (_subAngles[0])));
            }
        }

        protected Point startPoint { get; set; }

        protected Point[] FindIntersections(Rectangle circle2)
        {
            double r1 = _WordParent.CircleBounds.Width / 2d;
            Point[] p = MathHelps.FindIntersections(_WordParent.CircleBounds, circle2);

            if (p == null)
            {
                return null;
            }

            var d1 = MathHelps.distance(p[0], startPoint);
            var d2 = MathHelps.distance(p[1], startPoint);
            var d3 = MathHelps.distance(new Point((int)0, (int)0), p[0]);

            if (d3 > r1 * 1.1)
                return null;

            if (d1 < d2)
                return new Point[] { p[0], p[1] };
            else
                return new Point[] { p[1], p[0] };
        }

        public Point CenterLine
        {
            get
            {
                //double x = WordParent.WordCenter.X + WordParent.WordRadius * Math.Cos(midAngle / 180 * Math.PI);
                //double y = WordParent.WordCenter.Y + WordParent.WordRadius * Math.Sin(midAngle / 180 * Math.PI);

                return MathHelps.D2Coords(new Point((int)0, (int)0), _WordParent.Radius, MidAngle);  //new Point((int)x, (int)y);
            }
        }

        public Point BehindLine(double radius)
        {
            //double x = WordParent.WordCenter.X + radius * Math.Cos(midAngle / 180 * Math.PI);
            //double y = WordParent.WordCenter.Y + radius * Math.Sin(midAngle / 180 * Math.PI);

            //return //new Point((int)x, (int)y);

            return MathHelps.D2Coords(new Point((int)0, (int)0), radius, MidAngle);
        }

        public float maxInnerRadius
        {
            get
            {

                Point p1 = MathHelps.D2Coords(new Point(), _WordParent.Radius, _gapAngle1);
                Point p2 = MathHelps.D2Coords(new Point(), _WordParent.Radius, _gapAngle2);

                double r = (Math.Sqrt(Math.Pow((p1.X - p2.X), 2) + Math.Pow((p1.Y - p2.Y), 2))) / 2;
                if (r < 1 || r > _WordParent.Radius)
                    r = _WordParent.Radius / 2;

                return (float)r;
            }
        }


        #endregion


        public override string ToString()
        {
            string[] parts = this.GetType().ToString().Split('.');
            if (Syllable == null)
                return "Blank";
            else
                return Syllable.ToString().PadRight(10) + parts[parts.Length - 1];
        }
    }
}
