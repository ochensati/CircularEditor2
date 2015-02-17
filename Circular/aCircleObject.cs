using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using Circular.Decorations;
using Circular.Words;
using System.Drawing.Drawing2D;
using EpPathFinding.cs;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Circular.Sentence;

namespace Circular
{
    public delegate void RedrawRequestEvent();

    [Serializable]
    public abstract class aCircleObject : iAnchorHolder, iMouseable
    {
        protected PredefinedArrangment _SentenceArrangement;

        public event RedrawRequestEvent RedrawRequest;

        public void SetEvent(aCircleObject test)
        {
            //        foreach (var s in SubCircles)
            //        {
            //            if (s == test)
            //            {
            test.RedrawRequest += new RedrawRequestEvent(test.CircleParent.CallRedraw);
            //       s.RedrawRequest += new RedrawRequestEvent(CallRedraw);
            //        return s;
            //    }
            //}

            //foreach (var s in SubCircles)
            //{
            //    var s2 = s.SetEvent(test);
            //    if (s2 != null)
            //        return s2;
            //}

            //return null;
        }

        public void ClearEvent()
        {
            foreach (Delegate d in RedrawRequest.GetInvocationList())
            {
                RedrawRequest -= (RedrawRequestEvent)d;
            }

        }

        protected void CallRedraw()
        {
            if (RedrawRequest != null && _doRedraw == true)
                RedrawRequest();
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////

        public aCircleObject CircleParent
        {
            get;
            set;
        }

        public enum ScriptStyles
        {
            Sherman, Ashcroft, Small
        }

        protected ScriptStyles _scriptStyle = ScriptStyles.Ashcroft;
        public ScriptStyles ScriptStyle
        {
            get { return _scriptStyle; }
            set
            {
                if (_scriptStyle != value)
                {
                    _scriptStyle = value;

                    CalculateCircle(true);
                    CallRedraw();
                }
            }

        }

        [CategoryAttribute("Syllables")]
        public List<aCircleObject> SubCircles { get; set; }


        [CategoryAttribute("Read-Only")]
        public Rectangle OuterBounds { get; protected set; }

        protected Point _DrawCenter;
        [CategoryAttribute("Appearance")]
        public Point DrawCenter
        {
            get
            {
                return _DrawCenter;
            }
            set
            {
                _DrawCenter = value;
                Redraw();
            }
        }

        [CategoryAttribute("Read-Only")]
        public double Radius
        {
            get
            {
                return _CircleBounds.Width / 2;
            }
        }

        protected Rectangle _CircleBounds;
        [CategoryAttribute("Read-Only")]
        public Rectangle CircleBounds
        {
            get
            {
                return _CircleBounds;
            }

            private set
            {
                _CircleBounds = value;
                // CalculateWord(false);
            }
        }

        protected bool _doRedraw = true;

        public double TotalAngle
        {
            get
            {
                if (CircleParent != null)
                    return CircleParent.TotalAngle + CircleParent.CircleAngle;
                else
                    return 0;
            }
        }

        public Point RealCenter
        {
            get;
            private set;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////

        protected bool _AllFancy = true;
        [CategoryAttribute("Appearance"),
        DefaultValueAttribute(true)]
        public bool AllFancy
        {
            get
            {
                return _AllFancy;
            }

            set
            {
                _AllFancy = value;
                _doRedraw = false;

                ChangeFancy();
                _doRedraw = true;
                Redraw();
            }
        }

        protected virtual void ChangeFancy()
        {
            foreach (var s in SubCircles)
                s.AllFancy = _AllFancy;

        }

        protected bool _AllBig = false;
        [CategoryAttribute("Appearance"),
        DefaultValueAttribute(false)]
        public bool AllBig
        {
            get
            {
                return _AllBig;
            }

            set
            {
                _AllBig = value;
                _doRedraw = false;
                ChangeBig();
                _doRedraw = true;
                Redraw();
            }
        }

        protected virtual void ChangeBig()
        {
            foreach (var s in SubCircles)
                s.AllBig = _AllBig;
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////

        [CategoryAttribute("Appearance"),
        DefaultValueAttribute(1)]
        public bool ShowDecorations { get; set; }

        [CategoryAttribute("Appearance")]
        public aCircleObject SubWord { get; set; }
        protected bool isSubword;

        protected double _Scale = 1;
        [CategoryAttribute("Appearance"),
        DefaultValueAttribute(1)]
        public double Scale
        {
            get { return _Scale; }

            set
            {
                _Scale = value;
                Redraw();
            }
        }

        protected bool _DrawBorder = true;
        [CategoryAttribute("Appearance"),
        DefaultValueAttribute(true)]
        public bool DrawBorder
        {
            get { return _DrawBorder; }

            set
            {
                _DrawBorder = value;
                Redraw();
            }
        }


        protected double _letterWidth = 30;
        [CategoryAttribute("Appearance"),
        DefaultValueAttribute(30)]
        public double LetterWidth
        {
            get
            {
                return _letterWidth;

            }

            set
            {
                _letterWidth = value;
                foreach (var s in SubCircles)
                    s.LetterWidth = _letterWidth;

            }
        }


        protected double _CircleAngle = 0;// rnd.NextDouble() ;

        [CategoryAttribute("Appearance"),
       DefaultValueAttribute(0)]
        public double CircleAngle
        {
            get
            {
                return _CircleAngle;
            }
            set
            {
                _CircleAngle = value;

                Redraw();
            }
        }


        [CategoryAttribute("Syllables")]
        public List<LetterShapes.aSyllable> Syllables { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////////////

        protected abstract void RedrawImpl();

        public void Redraw()
        {
            if (_doRedraw)
            {
                RedrawImpl();
                if (RedrawRequest != null)
                    RedrawRequest();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////

        public abstract iMouseable HitTest(Point p);



        public void MoveDelta(Point originalPoint, int deltaX, int deltaY)
        {
            double r = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            double theta = MathHelps.Atan2(deltaY, deltaX) - TotalAngle;
            DrawCenter = MathHelps.D2Coords(originalPoint, r, theta);

        }

        public Point GetStartAngle(Point mousePoint, out double startAngle)
        {
            Point center = new Point(RealCenter.X, RealCenter.Y);
            startAngle = MathHelps.Atan2(mousePoint, center) - CircleAngle;

            return center;
        }
        public void SetAngleDelta(double deltaAngle)
        {
            this.CircleAngle = deltaAngle;

        }

        ///////////////////////////////////////////////////////////////////////////////////////////////

        protected Rectangle _startMark;
        protected List<DecorationDot> Punctuations = new List<DecorationDot>();

        ///////////////////////////////////////////////////////////////////////////////////////////////

        [CategoryAttribute("Sentence")]
        public List<aCircleObject> Connections { get; set; }

        protected List<DecorationLine> _CurrentDecorations;

        [CategoryAttribute("Appearance")]
        public List<DecorationLine> CurrentDecorations { get { return _CurrentDecorations; } set { _CurrentDecorations = value; Redraw(); } }

        ///////////////////////////////////////////////////////////////////////////////////////////////

        protected Dictionary<string, DecorationAnchor> _Anchors = new Dictionary<string, DecorationAnchor>();
        protected Dictionary<string, DecorationAnchor> _Sources = new Dictionary<string, DecorationAnchor>();
        public void AddSource(string key, DecorationAnchor decoration)
        {
            decoration.Name = key;

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
            decoration.Name = key;
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

        ///////////////////////////////////////////////////////////////////////////////////////////////

        public class AngleMark
        {
            public double Angle { get; private set; }
            public double Preferences { get; private set; }
            public iArcJoin JoinableArc { get; private set; }
            public AngleMark(double angle, double preference, iArcJoin joinableArc)
            {
                Angle = angle;
                this.Preferences = preference;
                this.JoinableArc = joinableArc;
            }
        }

        public abstract List<AngleMark> PreferedAngles();


        ///////////////////////////////////////////////////////////////////////////////////////////////
        protected void DoRedraw()
        {
            if (RedrawRequest != null)
                RedrawRequest();
        }

        public void Draw(Graphics canvas, bool mockup)
        {

            if (CircleParent == null)
                DrawCircleBorder(canvas, mockup);

            var TBackup = canvas.Transform;
            if (mockup == false)
            {
                canvas.TranslateTransform(_DrawCenter.X, _DrawCenter.Y);
                canvas.RotateTransform((float)_CircleAngle);
                canvas.ScaleTransform((float)_Scale, (float)_Scale);

                if (_CurrentDecorations != null)
                {
                    foreach (var l in _CurrentDecorations)
                        l.Draw(ref canvas);
                }
            }
            else
            {
                canvas.TranslateTransform(OuterBounds.Width / 2f, OuterBounds.Height / 2f);
            }

            var p = new Point[] { new Point(0, 0) };
            canvas.Transform.TransformPoints(p);
            RealCenter = p[0];



            DrawCircle(canvas, mockup);


            canvas.Transform = TBackup;
        }

        public virtual void DrawCircleBorder(Graphics canvas, bool mockup)
        {

            if (DrawBorder)
            {
                var TBackup = canvas.Transform;

                canvas.TranslateTransform(_DrawCenter.X, _DrawCenter.Y);
                canvas.RotateTransform((float)_CircleAngle);
                canvas.ScaleTransform((float)_Scale, (float)_Scale);


                if (Syllables != null && Syllables.Count > 0)
                {
                    GraphicsPath border = new GraphicsPath();

                    border.StartFigure();
                    for (int i = 0; i < Syllables.Count; i++)
                    {
                        if (mockup)
                            Syllables[i].DrawLetter(ref canvas, ref border, Color.Black, Color.Black, mockup);
                        else
                            Syllables[i].DrawLetter(ref canvas, ref border, Color.White, Color.Black, mockup);
                    }
                    border.CloseFigure();
                    if (ScriptStyle == Circular.aCircleObject.ScriptStyles.Small)
                    {
                        Pen p = new Pen(Color.Black, 1);
                        canvas.DrawPath(p, border);
                    }
                    else
                    {
                        Pen p = new Pen(Color.Black, 3);
                        canvas.DrawPath(p, border);
                    }
                    border.Dispose();

                }
                else
                    canvas.DrawEllipse(Pens.Black, CircleBounds);


                canvas.Transform = TBackup;
            }
        }


        protected abstract void DrawCircle(Graphics canvas, bool mockup);


        protected static Random rnd = new Random();

        ///////////////////////////////////////////////////////////////////////////////////////////////
        protected static BaseGrid BitmapToSearchGrid(Bitmap bitmap)
        {


            BaseGrid searchGrid = new StaticGrid(bitmap.Width, bitmap.Height);

            BitmapData bmpdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
            int stride = bmpdata.Stride;
            int numbytes = bmpdata.Stride * bitmap.Height;
            byte[] bytedata = new byte[numbytes];
            IntPtr ptr = bmpdata.Scan0;

            Marshal.Copy(ptr, bytedata, 0, numbytes);

            bitmap.UnlockBits(bmpdata);

            int k = 0;
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    //if (i == 55 && j == 12)
                    //    k++;

                    if (bytedata[j * stride + i * 4] != 0)
                    {
                        searchGrid.SetWalkableAt(i, j, true);
                    }
                    else
                    {
                        k++;
                    }
                }
            }


            return searchGrid;

        }


        ///////////////////////////////////////////////////////////////////////////////////////////////
        protected abstract void CalculateCircle(bool fixConnections);

        public virtual void Initialize(
            aCircleObject parent,
            Circular.aCircleObject.ScriptStyles scriptStyle,
            double scale,
            double defaultLetterWidth,
            bool isSubword,
            List<aCircleObject> connections)
        {
            this.CircleParent = parent;

            ScriptStyle = scriptStyle;
            this.isSubword = isSubword;

            ShowDecorations = true;

            Connections = connections;
            this._letterWidth = defaultLetterWidth;
            this._Scale = scale;

        }

        protected List<Shaker.Dot> BorderSyllables;
        public virtual void CalculateBorderWords(string text, int rearrange = 0)
        {
            string[] words = text.Split(new string[] { ",", " ", ".", ":", ";", "!", "?", "(", ")", "\"", "-" }, StringSplitOptions.RemoveEmptyEntries);
            int letterwidth = 80;



            List<engLetter> eWords = new List<engLetter>();
            for (int wordI = words.Length - 1; wordI >= 0; wordI--)
            {
                string w = words[wordI].Trim();
                var word = new engWord(w, ScriptStyle);
                eWords.AddRange(word.Syllables);
                eWords.Add(new engLetter(true));
            }



            double circum = Math.Abs(this.CircleRadius * 2 * Math.PI);

            double[] letterArc;
            letterArc = new double[eWords.Count];
            double sLetters = 0;
            //space the letters, smaller for just vowels
            for (int i = 0; i < eWords.Count; i++)
            {
                if (eWords[i].isVowel && eWords.Count > 2)
                    letterArc[i] = .5;
                else
                {
                    if (eWords[i].isPunct)
                    {
                        letterArc[i] = .2;
                    }
                    else
                        letterArc[i] = 1;
                }
                sLetters += letterArc[i];
            }

            int letterBlanks = (int)Math.Round((circum / letterwidth - sLetters) / words.Length);

            eWords.Insert(0, new engLetter(true));

            letterArc = new double[eWords.Count];
            sLetters = 0;


            //space the letters, smaller for just vowels
            for (int i = 0; i < eWords.Count; i++)
            {
                if (eWords[i].isVowel && eWords.Count > 2)
                    letterArc[i] = .5;
                else
                {
                    if (eWords[i].isPunct)
                        letterArc[i] = .2;
                    else
                        letterArc[i] = 1;

                    if (eWords[i].isConnector)
                    {
                        if (i == 0 || i == eWords.Count - 1)
                            letterArc[i] = letterBlanks / 2;
                        else
                            letterArc[i] = letterBlanks;
                    }

                }
                sLetters += letterArc[i];
            }
            if (sLetters == 0)
                System.Diagnostics.Debug.Print("");

            //now spread it around the whole circle
            for (int i = 0; i < eWords.Count; i++)
            {
                letterArc[i] = (letterArc[i] / sLetters) * 360;
            }


            Syllables = new List<LetterShapes.aSyllable>();

            double curAngle = 0;


            for (int i = 0; i < eWords.Count; i++)
            {
                if (eWords[i].isConnector == true)
                {
                    eWords[i] = new engLetter(false);

                }

                var l = LetterShapes.aSyllable.FindLetter(eWords[i], ScriptStyle);
                if (l != null)
                {

                    l.Initialize(this, ScriptStyle, curAngle, curAngle + letterArc[i], eWords[i], false, true);

                    if (eWords[i].isPunct)
                    {
                        Punctuations.Add(new DecorationDot(DecorationDot.Symbols.Diamond, MathHelps.D2Coords(CircleBounds, curAngle + letterArc[i]), (float)(curAngle + letterArc[i]), 10));
                    }
                    Syllables.Add(l);
                }
                else
                {
                    if (eWords[i].isPunct == true || eWords[i].Consonant == "'")
                    {
                        Punctuations.Add(new DecorationDot(DecorationDot.Symbols.Diamond, MathHelps.D2Coords(CircleBounds, curAngle + letterArc[i]), (float)(curAngle + letterArc[i]), 10));
                    }
                }
                curAngle += letterArc[i];

            }

            BorderSyllables = new List<Shaker.Dot>();
            foreach (var s in Syllables)
            {
                Rectangle t = s.LetterBounds;
                t.Inflate((int)(t.Width * .5), (int)(t.Height * .5));
                BorderSyllables.Add(new Shaker.Dot(t, true));
            }

            if (rearrange > 0)
            {
                ArrangeInTightCircle();

                Rectangle b = this.GetSize();

                this._DrawCenter = new Point(-1 * b.X + 40, -1 * b.Y + 40);

                CalculateBorderWords(text, rearrange - 1);
            }
            DoRedraw();
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////

        public Rectangle GetSize()
        {
            if (CircleRadius == -1)
            {
                int minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;

                foreach (var w in SubCircles)
                {
                    if (minX > (w._DrawCenter.X - w.Radius * w.Scale))
                        minX = (int)(w._DrawCenter.X - w.Radius * w.Scale);
                    if (maxX < (w._DrawCenter.X + w.Radius * w.Scale))
                        maxX = (int)(w._DrawCenter.X + w.Radius * w.Scale);

                    if (minY > (w._DrawCenter.Y - w.Radius * w.Scale))
                        minY = (int)(w._DrawCenter.Y - w.Radius * w.Scale);
                    if (maxY < (w._DrawCenter.Y + w.Radius * w.Scale))
                        maxY = (int)(w._DrawCenter.Y + w.Radius * w.Scale);
                }

                int bufferX = (int)((maxX - minX) * .1);
                int bufferY = (int)((maxY - minY) * .1);

                return new Rectangle(minX - bufferX, minY - bufferY, maxX - minX + 2 * bufferX, maxY - minY + 2 * bufferY);
            }
            else
            {
                return MathHelps.Circle2Rect(new Point(0, 0), CircleRadius);
            }

        }

        public void AlignPacMouths()
        {
            for (int i = 0; i < SubCircles.Count - 1; i++)
            {
                var w = SubCircles[i];
                var n = SubCircles[i + 1];

                var angle = MathHelps.Atan2(w._DrawCenter, n._DrawCenter);

                var v = w.PreferedAngles();

                if (v.Count > 0)
                {
                    w.CircleAngle = (angle - v[0].Angle) + 180;

                    if (v[0].JoinableArc != null)
                        v[0].JoinableArc.UseWordForArc(n);
                }
                //  w.CircleAngle = 0;
            }

            if (SubCircles.Count > 1)
            {
                var n = SubCircles[SubCircles.Count - 2];
                var w = SubCircles[SubCircles.Count - 1];

                var angle = MathHelps.Atan2(w._DrawCenter, n._DrawCenter);

                var v = w.PreferedAngles();

                if (v.Count > 0)
                {
                    w.CircleAngle = (angle - v[0].Angle) + 180;
                }

            }

        }

        public void ArrangeInSpots()
        {
            CircleRadius = -1;
            int x = 0;
            int y = 0;
            int r;

            int h = 0;

            foreach (var w in SubCircles)
            {
                r = (int)(1.1 * w.OuterBounds.Width / 2 * w.Scale);
                if (h < r)
                    h = r;
            }

            y = (int)(h * 1.5);

            foreach (var w in SubCircles)
            {
                r = (int)(w.OuterBounds.Width / 2 * w.Scale * 1.1);
                if (x == 0)
                    x = r;
                else
                {
                    x = x + r;
                }
                w._DrawCenter = new Point(x, y);

                x = x + r;
            }

            //  AlignPacMouths();

            Rectangle bounds = this.GetSize();

            //x = bounds.X + bounds.Width / 2;
            //y = bounds.Y + bounds.Height / 2;
            //foreach (var w in SubCircles)
            //{
            //    w._DrawCenter = new Point(w._DrawCenter.X - x, w._DrawCenter.Y - y);
            //}

            bounds = this.GetSize();

            this._CircleBounds = bounds; this.OuterBounds = new Rectangle(bounds.X - 8, bounds.Y - 8, bounds.Width + 16, bounds.Height + 16);
        }

        public void ArrangeInLine()
        {
            int x = 0;
            int y = 0;
            int r;

            int h = 0;

            foreach (var w in SubCircles)
            {
                r = (int)(1.1 * w.OuterBounds.Width / 2 * w.Scale);
                if (h < r)
                    h = r;
            }

            y = h * 3;

            foreach (var w in SubCircles)
            {
                r = (int)(w.OuterBounds.Width / 2 * w.Scale);
                if (x == 0)
                    x = r;
                else
                {
                    x = x + r;
                }

                w._DrawCenter = new Point(x, y);
                var v = w.PreferedAngles();

                if (v.Count > 0)
                {
                    w.CircleAngle = -1 * v[0].Angle;

                    w._DrawCenter = new Point(x, y);

                    x = x - (int)(.15 * r);
                }
                x = x + r;
            }

            AlignPacMouths();

            Rectangle bounds = this.GetSize();

            x = bounds.X + bounds.Width / 2;
            y = bounds.Y + bounds.Height / 2;
            foreach (var w in SubCircles)
            {
                w._DrawCenter = new Point(w._DrawCenter.X - x, w._DrawCenter.Y - y);
            }
            bounds = this.GetSize();

            this._CircleBounds = bounds; this.OuterBounds = new Rectangle(bounds.X - 8, bounds.Y - 8, bounds.Width + 16, bounds.Height + 16);
        }

        public void ArrangeInMiddleLine()
        {
            int x = 0;
            int y = 0;
            int r;
            foreach (var w in SubCircles)
            {
                r = (int)(1.1 * w.OuterBounds.Width / 2 * w.Scale);
                if (y < r)
                    y = r;
            }

            foreach (var w in SubCircles)
            {
                r = (int)(w.Radius * w.Scale);
                if (x == 0)
                    x = r;
                else
                {
                    x = x + r;
                }

                w._DrawCenter = new Point(x, y);
                var v = w.PreferedAngles();

                if (v.Count > 0)
                {
                    w.CircleAngle = -1 * v[0].Angle;

                    w._DrawCenter = new Point(x, y);

                    x = x - (int)(.15 * r);
                }
                x = x + r;
            }
            AlignPacMouths();

            Rectangle bounds = this.GetSize();

            x = bounds.X + bounds.Width / 2;
            y = bounds.Y + bounds.Height / 2;
            foreach (var w in SubCircles)
            {
                w._DrawCenter = new Point(w._DrawCenter.X - x, w._DrawCenter.Y - y);
            }
            bounds = this.GetSize();

            this._CircleBounds = bounds; this.OuterBounds = new Rectangle(bounds.X - 8, bounds.Y - 8, bounds.Width + 16, bounds.Height + 16);
        }

        public void ArrangeInZigzag(PredefinedArrangment startSentenceArrangment)
        {
            _doRedraw = false;
            int h = 0;
            int r;
            int cc = 0;
            aCircleObject[] words = new aCircleObject[SubCircles.Count];
            foreach (var w in SubCircles)
            {
                words[cc] = w;
                r = (int)(1.1 * w.OuterBounds.Width / 2 * w.Scale);
                if (h < r)
                    h = r;
                cc++;
            }

            int y = (int)(h * 1.5);
            int x = 0;
            for (int i = 0; i < words.Length; i += 2)
            {
                var w = words[i];

                r = (int)(w.Radius * w.Scale * 1.05);

                if (x == 0)
                    x = r;
                else
                    x = x + r;

                w._DrawCenter = new Point(x, y);

                var v = w.PreferedAngles();
                x = x + r;
            }


            for (int i = 1; i < words.Length - 1; i += 2)
            {
                var b = words[i - 1];
                var w = words[i];
                var n = words[i + 1];

                double bR = b.Radius;
                double nR = n.Radius;

                var v = b.PreferedAngles();
                if (v.Count > 0)
                {
                    bR *= .85;
                }
                bR += w.Radius;

                v = w.PreferedAngles();
                if (v.Count > 0)
                {
                    nR += w.Radius * .85;
                }
                else
                    nR += w.Radius;

                var rect1 = MathHelps.Circle2Rect(b._DrawCenter, bR);
                var rect2 = MathHelps.Circle2Rect(n._DrawCenter, nR);

                var points = MathHelps.FindIntersections(rect1, rect2);

                if ((points[0].Y > points[1].Y && startSentenceArrangment == PredefinedArrangment.Zigzag) || (points[0].Y < points[1].Y && startSentenceArrangment == PredefinedArrangment.ZagZig))
                {
                    w._DrawCenter = points[0];
                }
                else
                {
                    w._DrawCenter = points[1];
                }
            }


            if (words.Length % 2 == 0 && words.Length > 1)
            {
                var b = words[words.Length - 2];
                var w = words[words.Length - 1];

                var R = b.Radius;

                var v = w.PreferedAngles();
                if (v.Count > 0)
                {
                    R *= .85;
                }

                R += w.Radius;



                if (startSentenceArrangment == PredefinedArrangment.ZagZig)
                    w._DrawCenter = MathHelps.D2Coords(b._DrawCenter, R, 45);
                else
                    w._DrawCenter = MathHelps.D2Coords(b._DrawCenter, R, -45);
            }


            List<Shaker.Dot> dots = new List<Shaker.Dot>();
            List<Shaker.Connection> connections = new List<Shaker.Connection>();
            Shaker ai = new Shaker();
            dots.Clear();
            connections.Clear();
            for (int i = 0; i < words.Length; i++)
            {
                dots.Add(new Shaker.Dot(words[i].Radius, new PointD(words[i]._DrawCenter.X, words[i]._DrawCenter.Y), 0, false));
            }
            int d1;
            int d2;
            for (int i = 0; i < dots.Count - 1; i++)
            {
                d1 = (i);
                d2 = (i + 1);

                if (d1 != d2)
                    connections.Add(new Shaker.Connection((dots[d1].radius + dots[d2].radius) * .7, d1, d2));
            }
            //d1 = dots.Count - 1;
            //d2 = 0;
            //connections.Add(new Shaker.Connection((dots[d1].radius + dots[d2].radius) * .7, d1, d2));

            for (int iter = 4900; iter < 5000; iter++)
                ai.Jiggle(dots, connections, .2, 1, 0, 5, iter, 5000);

            for (int i = 0; i < words.Length; i++)
            {
                words[i]._DrawCenter = new Point((int)dots[i].center.X, (int)dots[i].center.Y);
            }


            AlignPacMouths();


            Rectangle bounds = this.GetSize();

            x = bounds.X + bounds.Width / 2;
            y = bounds.Y + bounds.Height / 2;
            foreach (var w in SubCircles)
            {
                w._DrawCenter = new Point(w._DrawCenter.X - x, w._DrawCenter.Y - y);
            }
            bounds = this.GetSize();

            this._CircleBounds = bounds; this.OuterBounds = new Rectangle(bounds.X - 8, bounds.Y - 8, bounds.Width + 16, bounds.Height + 16);
            _doRedraw = true;

            CallRedraw();
        }

        public void ArrangeInLoop()
        {
            _doRedraw = false;
            ArrangeInSpots();


            List<Shaker.Dot> dots = new List<Shaker.Dot>();
            List<Shaker.Connection> connections = new List<Shaker.Connection>();
            Shaker ai = new Shaker();
            dots.Clear();
            connections.Clear();
            for (int i = 0; i < SubCircles.Count; i++)
            {
                dots.Add(new Shaker.Dot(SubCircles[i].Radius, new PointD(SubCircles[i]._DrawCenter.X, SubCircles[i]._DrawCenter.Y), 0, false));
            }
            int d1;
            int d2;
            for (int i = 0; i < dots.Count - 1; i++)
            {
                d1 = (i);
                d2 = (i + 1);

                if (d1 != d2)
                    connections.Add(new Shaker.Connection((dots[d1].radius + dots[d2].radius) * .7, d1, d2));
            }
            d1 = dots.Count - 1;
            d2 = 0;
            connections.Add(new Shaker.Connection((dots[d1].radius + dots[d2].radius) * .7, d1, d2));

            for (int iter = 0; iter < 5000; iter++)
                ai.Jiggle(dots, connections, .2, 1, 0, 5, iter, 5000);

            for (int i = 0; i < SubCircles.Count; i++)
            {
                SubCircles[i]._DrawCenter = new Point((int)dots[i].center.X, (int)dots[i].center.Y);
            }


            AlignPacMouths();


            Rectangle bounds = this.GetSize();

            int x = bounds.X + bounds.Width / 2;
            int y = bounds.Y + bounds.Height / 2;
            foreach (var w in SubCircles)
            {
                w._DrawCenter = new Point(w._DrawCenter.X - x, w._DrawCenter.Y - y);
            }

            bounds = this.GetSize();

            this._CircleBounds = bounds; this.OuterBounds = new Rectangle(bounds.X - 8, bounds.Y - 8, bounds.Width + 16, bounds.Height + 16);

            _doRedraw = true;

            CallRedraw();
        }

        public void ArrangeInTight()
        {
            _doRedraw = false;
            ArrangeInSpots();


            List<Shaker.Dot> dots = new List<Shaker.Dot>();
            List<Shaker.Connection> connections = new List<Shaker.Connection>();
            Shaker ai = new Shaker();
            dots.Clear();
            connections.Clear();
            for (int i = 0; i < SubCircles.Count; i++)
            {
                dots.Add(new Shaker.Dot(SubCircles[i].Radius, new PointD(SubCircles[i]._DrawCenter.X, SubCircles[i]._DrawCenter.Y), 0, false));
            }
            int d1;
            int d2;
            for (int i = 0; i < dots.Count - 1; i++)
            {
                d1 = (i);
                d2 = (i + 1);

                if (d1 != d2)
                    connections.Add(new Shaker.Connection((dots[d1].radius + dots[d2].radius) * .7, d1, d2));
            }
            d1 = dots.Count - 1;
            d2 = 0;
            connections.Add(new Shaker.Connection((dots[d1].radius + dots[d2].radius) * .7, d1, d2));

            //for (int iter = 0; iter < 5000; iter++)
            //    ai.JiggleCenter(dots, connections, .2, .00001, 1, 0, 5, iter, 5000);

            for (int i = 0; i < SubCircles.Count; i++)
            {
                SubCircles[i]._DrawCenter = new Point((int)dots[i].center.X, (int)dots[i].center.Y);
            }


            AlignPacMouths();


            Rectangle bounds = this.GetSize();

            int x = bounds.X + bounds.Width / 2;
            int y = bounds.Y + bounds.Height / 2;
            foreach (var w in SubCircles)
            {
                w._DrawCenter = new Point(w._DrawCenter.X - x, w._DrawCenter.Y - y);
            }

            SetBoundingCircle();
            bounds = this.GetSize();

            this._CircleBounds = bounds; this.OuterBounds = new Rectangle(bounds.X - 8, bounds.Y - 8, bounds.Width + 16, bounds.Height + 16);
            _doRedraw = true;

            CallRedraw();
        }

        public void ArrangeInTightCircle(double tightness = 1)
        {
            _doRedraw = false;
            //ArrangeInSpots();

            List<Shaker.Dot> dots = new List<Shaker.Dot>();
            List<Shaker.Connection> connections = new List<Shaker.Connection>();
            Shaker ai = new Shaker();
            dots.Clear();
            connections.Clear();
            for (int i = 0; i < SubCircles.Count; i++)
            {
                dots.Add(new Shaker.Dot(SubCircles[i].OuterBounds.Width / 2f, new PointD(SubCircles[i]._DrawCenter.X + (.5 - rnd.NextDouble()), SubCircles[i]._DrawCenter.Y + (.5 - rnd.NextDouble())), 0, false));
            }

            int d1;
            int d2;
            for (int i = 0; i < dots.Count - 1; i++)
            {
                d1 = (i);
                d2 = (i + 1);

                if (d1 != d2)
                    connections.Add(new Shaker.Connection((dots[d1].radius + dots[d2].radius) * .9, d1, d2));
            }

            if (connections.Count > 1)
            {
                d1 = dots.Count - 1;
                d2 = 0;
                connections.Add(new Shaker.Connection((dots[d1].radius + dots[d2].radius) * 1.1, d1, d2));
            }



            if (BorderSyllables != null)
            {
                foreach (var d in BorderSyllables)
                {
                    d.center = new PointD(d.center.X * 1.1, d.center.Y * 1.1);
                    dots.Add(d);
                }

                tightness *= 4;
            }


            double area = 0;

            for (int i = 0; i < dots.Count; i++)
            {
                area += Math.PI * Math.Pow(dots[i].radius, 2);
            }


            double sentenceRadius = Math.Sqrt(area * 1.5 / Math.PI) * 1;

            if (BorderSyllables != null && BorderSyllables.Count > 0)
                sentenceRadius *= 1.2;

            if (CircleParent != null)
            {
                Shaker.Dot[] ExtraDots = new Shaker.Dot[] { new Shaker.Dot(sentenceRadius, new Point((int)(sentenceRadius * 1.85), 0), 0, true) };

                if (ExtraDots != null)
                {
                    dots.AddRange(ExtraDots);
                }
            }

            double[] scores = new double[1];
            PointD[][] centers = new PointD[scores.Length][];
            for (int i = 0; i < scores.Length; i++)
            {
                for (int iter = 0; iter < 5000; iter++)
                {
                    ai.Jiggle(dots, connections, sentenceRadius, .2 * tightness, 1, 0, 45 + i, iter, 5000);

                    if (Preview != null && (iter % 10) == 0)
                    {
                        DrawPreview(dots, connections);
                    }
                }
                centers[i] = new PointD[dots.Count];
                for (int j = 0; j < dots.Count; j++)
                    centers[i][j] = dots[j].center;

                double score = 0;
                for (int j = 0; j < dots.Count; j++)
                {
                    double d = Math.Abs(MathHelps.distance(dots[j].center, new PointD(0, 0)) + dots[j].radius - sentenceRadius);
                    if (d > 0)
                        score += d;
                }

                scores[i] = score;
            }
            int bestI = 0;
            double bestScore = double.MaxValue;
            for (int i = 0; i < scores.Length; i++)
            {
                if (scores[i] < bestScore)
                {
                    bestScore = scores[i];
                    bestI = i;
                }
            }


            double sentenceRadius2 = 0;
            double ccSR = 0;
            for (int j = 0; j < dots.Count; j++)
            {
                dots[j].center = centers[bestI][j];

                double d = Math.Abs(MathHelps.distance(dots[j].center, new PointD(0, 0)) + dots[j].radius);
                if (d >= sentenceRadius * .9)
                {
                    sentenceRadius2 += d;
                    ccSR++;
                }
            }

            sentenceRadius = sentenceRadius2 / ccSR;

            for (int i = 0; i < SubCircles.Count; i++)
            {
                SubCircles[i]._DrawCenter = new Point((int)dots[i].center.X, (int)dots[i].center.Y);
            }

            AlignPacMouths();

            Rectangle bounds = this.GetSize();

            int x = bounds.X + bounds.Width / 2;
            int y = bounds.Y + bounds.Height / 2;
            foreach (var w in SubCircles)
            {
                w._DrawCenter = new Point(w._DrawCenter.X - x, w._DrawCenter.Y - y);
            }

            _DrawCenter = new Point(0, 0);
            SetBoundingCircle();
            bounds = this.GetSize();

            this._CircleBounds = bounds;

            this.OuterBounds = new Rectangle(bounds.X - 8, bounds.Y - 8, bounds.Width + 16, bounds.Height + 16);

            _doRedraw = true;

            CallRedraw();
        }

        public void ArrangeInCircle()
        {
            _doRedraw = false;
            ArrangeInMiddleLine();

            List<Shaker.Dot> dots = new List<Shaker.Dot>();
            List<Shaker.Connection> connections = new List<Shaker.Connection>();
            Shaker ai = new Shaker();
            dots.Clear();
            connections.Clear();

            double area = 0;
            double[] arcs = new double[SubCircles.Count];
            double totalArc = 0;
            for (int i = 1; i < SubCircles.Count; i++)
            {
                area += Math.PI * Math.Pow(SubCircles[i].Radius, 2);
                arcs[i] = SubCircles[i].Radius + SubCircles[i - 1].Radius;
                totalArc += arcs[i];
            }
            totalArc += (2 * SubCircles[0].Radius);

            Rectangle bounds = this.GetSize();

            double sentenceRadius = bounds.Width / 2 / Math.PI;//
            double areaRadius = Math.Sqrt(area * 5 / Math.PI);
            if (areaRadius > sentenceRadius)
                sentenceRadius = areaRadius;

            //DrawRadius = (float)(5 + sentenceRadius);
            double totalArc2 = 0;
            for (int i = 0; i < SubCircles.Count; i++)
            {
                arcs[i] = arcs[i] / totalArc * 360 + totalArc2;
                totalArc2 = arcs[i];
            }

            for (int i = 0; i < SubCircles.Count; i++)
            {

                //dots.Add(
                //    new Shaker.Dot(SubCircles[i].Radius,
                //        MathHelps.D2Coords(new Point((int)sentenceRadius, (int)sentenceRadius), sentenceRadius - SubCircles[i].Radius, arcs[i])
                //        , 0));
            }

            for (int i = 0; i < SubCircles.Count; i++)
            {
                SubCircles[i]._DrawCenter = new Point((int)dots[i].center.X, (int)dots[i].center.Y);
                SubCircles[i].DrawBorder = false;
            }


            AlignPacMouths();


            bounds = this.GetSize();

            int x = bounds.X + bounds.Width / 2;
            int y = bounds.Y + bounds.Height / 2;
            foreach (var w in SubCircles)
            {
                w._DrawCenter = new Point(w._DrawCenter.X - x, w._DrawCenter.Y - y);
            }

            _DrawCenter = new Point(0, 0);

            SetBoundingCircle();

            bounds = this.GetSize();

            this._CircleBounds = bounds; this.OuterBounds = new Rectangle(bounds.X - 8, bounds.Y - 8, bounds.Width + 16, bounds.Height + 16);
            _doRedraw = true;

            CallRedraw();
        }

        public System.Windows.Forms.PictureBox Preview { get; set; }

        public void DrawPreview(List<Shaker.Dot> dots, List<Shaker.Connection> connections)
        {

            double minX = 0, minY = 0;
            foreach (var d in dots)
            {
                double mX = d.center.X - d.radius;
                double mY = d.center.Y - d.radius;
                if (mX < minX)
                    minX = mX;
                if (mY < minY)
                    minY = mY;
            }

            var g = Graphics.FromImage(Preview.Image);
            g.Clear(Color.White);
            g.ScaleTransform(.25f, .25f);
            g.TranslateTransform((float)(-1 * minX + 50), (float)(-1 * minY + 50));
            foreach (var d in dots)
            {
                if (d.Fixed)
                    g.DrawEllipse(Pens.Red, d.Bounds);
                else
                    g.DrawEllipse(Pens.Black, d.Bounds);
            }

            Preview.Invalidate();
            Preview.Refresh();
            System.Windows.Forms.Application.DoEvents();

        }

        protected bool cutSub = false;
        protected double SubArcStart = -1, SubArc = -1;
        public void UseWordForArc(aCircleObject otherWord)
        {

            FindEdges(otherWord);
        }

        protected void FindEdges(aCircleObject otherWord)
        {

            var p = new Point(otherWord._DrawCenter.X - this._DrawCenter.X, otherWord._DrawCenter.Y - this._DrawCenter.Y);
            double r2 = Math.Sqrt(p.X * p.X + p.Y * p.Y);

            double angle = MathHelps.Atan2(p.Y, p.X) - this.CircleAngle;



            var LetterBounds = MathHelps.Circle2Rect(MathHelps.D2Coords(new Point((int)0, (int)0), r2, angle), otherWord.Radius);


            var _edges = FindIntersections(LetterBounds);

            if (_edges == null)
            {
                SubArc = 360;
            }
            else
            {
                var _mainAngles = MathHelps.GetAngles(_edges, new Point((int)0, (int)0));

                if (_mainAngles[0] > _mainAngles[1])
                {
                    var t = _mainAngles[1];
                    _mainAngles[1] = _mainAngles[0];
                    _mainAngles[0] = t;
                }
                cutSub = true;
                if (Math.Abs(_mainAngles[1] - _mainAngles[0]) < 180)
                {
                    this.SubArcStart = _mainAngles[0];
                    this.SubArc = _mainAngles[1] - _mainAngles[0];
                }
                else
                {
                    this.SubArcStart = _mainAngles[1];
                    this.SubArc = _mainAngles[0] + 360 - _mainAngles[1];
                }
            }
        }

        protected Point[] FindIntersections(Rectangle circle2)
        {
            double r1 = this.CircleBounds.Width / 2d;
            Point[] p = MathHelps.FindIntersections(this.CircleBounds, circle2);

            if (p == null)
            {
                return null;
            }
            return p;
        }

        private float CircleRadius = -1;

        public void SetBoundingCircle()
        {
            // All of the points.
            List<PointF> m_Points = new List<PointF>();

            // The convex hull points.
            List<PointF> ConvexHull = null;
            foreach (var s in SubCircles)
            {
                for (double theta = 0; theta < 360; theta += 360d / 6)
                {
                    // Add the new point.
                    m_Points.Add(MathHelps.D2Coords(s._DrawCenter, s.Radius, theta));
                }
            }

            // Get the convex hull.
            ConvexHull = Geometry.MakeConvexHull(m_Points);

            PointF CircleCenter;
            // Get a minimal bounding circle.
            Geometry.FindMinimalBoundingCircle(ConvexHull,
                out CircleCenter, out CircleRadius);

            if (CircleRadius != -1)
            {
                foreach (var s in SubCircles)
                {
                    s._DrawCenter = new Point(s._DrawCenter.X - (int)CircleCenter.X, s._DrawCenter.Y - (int)CircleCenter.Y);
                }

                _CircleBounds = MathHelps.Circle2Rect(new Point(0, 0), CircleRadius);
            }
            else
                System.Diagnostics.Debug.Print("");

        }

    }
}
