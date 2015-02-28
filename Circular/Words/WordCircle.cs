using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using Circular.Decorations;
using EpPathFinding.cs;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.ComponentModel;


namespace Circular.Words
{


    [Serializable]
    public class WordCircle : aCircleObject
    {
        [CategoryAttribute("Read-Only")]
        public engWord Word { get; private set; }


        protected Point[] _startCross;

        protected override void RedrawImpl()
        {
            CalculateDecorations(false);
        }

        public override List<aCircleObject.AngleMark> PreferedAngles()
        {

            List<AngleMark> marks = new List<AngleMark>();

            foreach (var s in Syllables)
            {
                if (s.PreferredAngle() != 0)
                {
                    marks.Add(new AngleMark(s.MidAngle, s.PreferredAngle(), s));
                }
            }

            marks.Sort((a, b) => a.Preferences.CompareTo(b.Preferences));

            return marks;
        }

        private bool drawStart = true;

        protected override void DrawCircle(Graphics canvas, bool mockup)
        {
            if (SubWord != null)
            {
                SubWord.Draw(canvas, mockup);
            }

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

            if (drawStart)
            {
                canvas.FillEllipse(Brushes.White, _startMark);
                canvas.DrawEllipse(Pens.Black, _startMark);

                if (_startCross != null)
                {
                    canvas.DrawLine(Pens.Black, _startCross[0], _startCross[1]);
                    canvas.DrawLine(Pens.Black, _startCross[2], _startCross[3]);
                }
            }


            foreach (var p in Punctuations)
            {
                p.DrawDot(ref canvas, Color.White, Color.Black, false);
            }


        }

        protected override void CalculateCircle(bool fixConnections = true)
        {
            if (Word == null)
                return;
            double[] letterArc;
            bool singleLetter = false;
            if (Word.nSyllables == 1)
            {
                Word.AddBlank();
                singleLetter = true;
            }


            if (Word.nSyllables > 10 && ScriptStyle != Circular.aCircleObject.ScriptStyles.Small)
            {
                var midWord = new engWord(new engLetter[] { Word.Syllables[0], Word.Syllables[1] });
                Word.Syllables.RemoveAt(0);
                Word.Syllables.RemoveAt(0);
                SubWord = new WordCircle();
                SubWord.Initialize(this, ScriptStyle, 1, _letterWidth, true, null);
                ((WordCircle)SubWord).InitializeWord(midWord);
                SubWord.DrawBorder = false;
                SubWord.RedrawRequest += new RedrawRequestEvent(SubWord_RedrawRequest);
            }

            letterArc = new double[Word.nSyllables];
            double sLetters = 0;
            //space the letters, smaller for just vowels
            for (int i = 0; i < Word.nSyllables; i++)
            {
                if (Word.Syllables[i].isVowel && Word.nSyllables > 2)
                    letterArc[i] = .5;
                else
                {
                    if (Word.Syllables[i].isPunct)
                    {
                        letterArc[i] = .2;
                    }
                    else
                        letterArc[i] = 1;
                }
                sLetters += letterArc[i];
            }

            //now spread it around the whole circle
            for (int i = 0; i < Word.nSyllables; i++)
            {
                letterArc[i] = (letterArc[i] / sLetters) * 360;
            }

            //convert the number of effective letters into a working radius
            var radius = _letterWidth * sLetters * 1.1;

            if (radius < 50)
                radius = 50;

            //build the outside radius
            var radius2 = radius * 1.05;

            if (Math.Abs(radius2 - radius) < 5)
                radius2 = radius + 5;
            if (Math.Abs(radius2 - radius) > 15)
                radius2 = radius + 15;

            OuterBounds = MathHelps.Circle2Rect(new Point((int)0, (int)0), radius2);
            _CircleBounds = MathHelps.Circle2Rect(new Point((int)0, (int)0), radius);

            double curAngle = _CircleAngle;

            bool big = this._AllBig;
            bool fancy = this._AllFancy;


            Syllables = new List<LetterShapes.aSyllable>();

            int BigOne = -1;

            if (Word.nSyllables == 2)
            {

                int rand = rnd.Next(6);
                if (rand == 1)
                    BigOne = 0;
                if (rand == 2)
                    BigOne = 1;
            }


            if (isSubword)
            {
                big = false;
                BigOne = -1;
            }

            //need to check if there are enough connections for the word
            if (ScriptStyle == Circular.aCircleObject.ScriptStyles.Ashcroft && fixConnections)
            {
                List<AngleMark> marks = new List<AngleMark>();

                for (int i = 0; i < Word.nSyllables; i++)
                {
                    big = this._AllBig;

                    var l = LetterShapes.aSyllable.FindLetter(Word.Syllables[i], ScriptStyle);

                    if (singleLetter)
                        big = true;

                    if (i == BigOne)
                        big = true;

                    l.Initialize(this, ScriptStyle, curAngle, curAngle + letterArc[i], Word.Syllables[i], big, fancy);

                    if (l.PreferredAngle() != 0)
                    {
                        marks.Add(new AngleMark(l.MidAngle, l.PreferredAngle(), l));
                    }
                    curAngle += letterArc[i];
                }
                curAngle = 0;


                if (this.Connections != null && (this.Connections.Count > marks.Count))
                {
                    for (int i = marks.Count; i < this.Connections.Count; i++)
                    {
                        var conn = new engLetter();
                        conn.isConnector = true;
                        Word.Syllables.Add(conn);
                    }
                    CalculateCircle(false);
                    return;
                }
            }

            for (int i = 0; i < Word.nSyllables; i++)
            {
                big = this._AllBig;

                var l = LetterShapes.aSyllable.FindLetter(Word.Syllables[i], ScriptStyle);
                if (l != null)
                {
                    if (singleLetter)
                        big = true;

                    if (i == BigOne)
                        big = true;

                    l.Initialize(this, ScriptStyle, curAngle, curAngle + letterArc[i], Word.Syllables[i], big, fancy);

                    if (Word.Syllables[i].isPunct)
                    {
                        Punctuations.Add(new DecorationDot(DecorationDot.Symbols.Diamond, MathHelps.D2Coords(CircleBounds, curAngle + letterArc[i]), (float)(curAngle + letterArc[i]), 10));
                    }
                    Syllables.Add(l);
                }
                else
                {
                    if (Word.Syllables[i].isPunct == true || Word.Syllables[i].Consonant == "'")
                    {
                        Punctuations.Add(new DecorationDot(DecorationDot.Symbols.Diamond, MathHelps.D2Coords(CircleBounds, curAngle + letterArc[i]), (float)(curAngle + letterArc[i]), 10));
                    }
                    else
                    {
                        l = new LetterShapes.Shapes.Blank();
                        l.Initialize(this, ScriptStyle, curAngle, curAngle + letterArc[i], Word.Syllables[i], big, fancy);
                        Syllables.Add(l);
                    }
                }
                curAngle += letterArc[i];

            }

            CalculateDecorations(true);
            CalcWordDecorations();

            DoRedraw();
        }

        void SubWord_RedrawRequest()
        {
            DoRedraw();
        }

        public virtual void InitializeWord(engWord englishWord)
        {
            if (!(ScriptStyle == Circular.aCircleObject.ScriptStyles.Ashcroft || ScriptStyle == Circular.aCircleObject.ScriptStyles.Small))
            {
                drawStart = false;
            }

            this.Word = englishWord;
            CalculateCircle();
        }

        private void CalcWordDecorations()
        {
            Point sCenter = MathHelps.D2Coords(new Point((int)0, (int)0), Radius, _CircleAngle);
            double sRad = Radius * .05;
            if (sRad < 5)
                sRad = 5;

            _startMark = MathHelps.Circle2Rect(sCenter, sRad);

            double angle = 0;// 360 * rnd.NextDouble();
            _startCross = new Point[] { MathHelps.D2Coords(sCenter, sRad, angle + 45), MathHelps.D2Coords(sCenter, sRad, angle + 60), MathHelps.D2Coords(sCenter, sRad, angle - 40), MathHelps.D2Coords(sCenter, sRad, angle - 20) };
        }

        public WordCircle()
        {

        }

        public override string ToString()
        {
            return Word.ToString();
        }

        public override iMouseable HitTest(Point p)
        {
            double r = MathHelps.distance(this._DrawCenter, p) / Scale;

            if (r < this.Radius)
            {
                //transform coordinates
                Point p2 = new Point((int)((p.X - _DrawCenter.X) / Scale), (int)((p.Y - _DrawCenter.Y) / Scale));

                if (SubWord != null)
                {
                    var sub = SubWord.HitTest(p2);

                    if (sub != null)
                        return sub;
                }

                double r2 = MathHelps.distance(new Point((int)0, (int)0), p2);
                double angle = MathHelps.Atan2(p2.Y, p2.X);
                angle -= CircleAngle;
                Point p3 = MathHelps.D2Coords(new Point((int)0, (int)0), r2, angle);
                foreach (var s in Syllables)
                {
                    var o = s.HitTest(p3);
                    if (o != null)
                        return o;
                }

                return this;
            }
            else
            {

                return null;
            }
        }


    }
}
