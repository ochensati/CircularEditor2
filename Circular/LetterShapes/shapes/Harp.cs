using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using Circular.Decorations;
using System.Drawing;
using Circular.Words;
using Circular.Decorations.Shapes;

namespace Circular.LetterShapes.Shapes
{
    [Serializable]
    class Harp : aSyllable
    {
        public override aSyllable HandlesEngLetter(engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {
            if (letter.Consonant != null && "_th_sh_x_y_ch_".Contains("_" + letter.Consonant + "_"))
                return new Harp();
            else
                return null;
        }

        protected override void DrawArc(ref Graphics path, ref GraphicsPath border, Color backgroundColor, Color foregroundColor, bool mockup)
        {
            try
            {
                //make sure to add error handling here and in the paint event.  Error handling bubbles into the .net graphics classes, which do weird things
                border.AddArc(_WordParent.CircleBounds, StartAngle, ArcWidth);
                for (int i = 0; i < StartLines.Length; i++)
                {
                    path.DrawLine(Pens.Black, StartLines[i], EndLines[i]);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);

            }
        }

        private Point[] StartLines;
        private Point[] EndLines;

        public override void CalculateArc()
        {
            try
            {
                //make sure to add error handling here and in the paint event.  Error handling bubbles into the .net graphics classes, which do weird things
                LetterRadius = maxInnerRadius * .8;
                if (LetterRadius > _WordParent.Radius * .4)
                    LetterRadius = _WordParent.Radius * .35;


                if (_Big)
                    LetterRadius *= 2;

                LetterBounds = MathHelps.BoundingRectangle(BehindLine(_WordParent.Radius - LetterRadius * .8), LetterRadius);
                FindEdges();

                StartLines = new Point[4];
                EndLines = new Point[4];
                double a1 = 361 - _mainAngles[0];
                double gapArc = (float)(Math.Abs((_mainAngles[1] + a1) % 360 - (_mainAngles[0] + a1) % 360));

                gapArc /= (StartLines.Length - 1);
                double[] d = new double[EndLines.Length];
                Point e = MathHelps.D2Coords(new Point((int)0, (int)0), _WordParent.Radius, MidAngle);

                for (int i = 0; i < StartLines.Length; i++)
                {
                    a1 = _mainAngles[0] + i * gapArc;
                    StartLines[i] = MathHelps.D2Coords(new Point((int)0, (int)0), _WordParent.Radius, a1);

                    //  d[i] = MathHelps.DistanceFromLine(WordParent.WordCenter, e, StartLines[i]);
                }

                double rTop = _WordParent.Radius - 2 * LetterRadius;

                //  Point p0 = MathHelps.D2Coords(WordParent.WordCenter, rTop, midAngle);

                // double a2 = (midAngle + 90) / 180 * Math.PI;

                for (int i = 0; i < StartLines.Length; i++)
                {
                    a1 = _mainAngles[0] + i * gapArc;
                    EndLines[i] = MathHelps.D2Coords(new Point((int)0, (int)0), rTop, a1);// new Point((int)(p0.X - d[i] * Math.Cos(a2)), (int)(p0.Y - d[i] * Math.Sin(a2)));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);

            }
        }

        public override void DrawLetter(ref Graphics path, ref GraphicsPath border, Color backgroundColor, Color foregroundColor, bool mockup)
        {
            if (!mockup)
                DrawArc(ref path, ref border, backgroundColor,foregroundColor,mockup);

            foreach (var v in Vowels)
                v.DrawVowel(ref path, backgroundColor, foregroundColor, mockup);

            foreach (var l in Lines)
                l.Draw(ref path);

            if (Dots != null)
            {
                foreach (var d in Dots)
                {
                    d.DrawDot(ref path, backgroundColor, foregroundColor, mockup);
                }
            }


        }

        public override void CalcPositions()
        {
            LocationProb prob = this.LocationProb();

            foreach (var v in Vowels)
                if (v != null)
                    v.Location = prob.GetVowelLocation();

            foreach (var d in Decorations)
                if (d != null)
                    d.Location = prob.GetDecLocation();
        }

        protected override LocationProb LocationProb()
        {
            return new LocationProb(
                vAbove: 0,
                vCenter: 0,
                vLeft: 1,
                dAbove: 1,
                dBottom:1,
                dCenter: 1,
                dLeft: 1,
                dRight: 1);
        }

        protected List<DecorationDot> Dots = new List<DecorationDot>();
        protected List<VectorGraphics.Line> Lines = new List<VectorGraphics.Line>();

        protected override void CalculateDecoration(Decorations.aDecoration decoration)
        {
            double x0 = LetterBounds.X + LetterRadius;
            double y0 = LetterBounds.Y + LetterRadius;
            double r0 = LetterRadius;

            double arcRadius = r0;
            double arcMidAngle = MidAngle + 180;
            double arcX = x0;
            double arcY = y0;
            double arcWidth = SubArc;

            Dots.Clear();
            Lines.Clear();

            if (decoration.GetType() == typeof(Rings))
            {
                #region Rings
                switch (decoration.Location)
                {
                    case DecorationLocation.Top:
                        {

                            for (int i = 0; i < EndLines.Length; i++)
                            {
                                EndLines[i] = MathHelps.TravelLine(EndLines[i], StartLines[i], .65 + (double)i / EndLines.Length * .35);
                            }
                            Lines.Add(new VectorGraphics.Line(new Point[] { EndLines[0], EndLines[EndLines.Length - 1] }, VectorGraphics.Line.LineTypes.Line, new Pen(Color.Black, 6)));
                            Lines.Add(new VectorGraphics.Line(new Point[]{
                                  MathHelps.TravelLine(EndLines[0], StartLines[0], .95) ,
                                  MathHelps.TravelLine(EndLines[EndLines.Length -1], StartLines[EndLines.Length -1], .95)}, VectorGraphics.Line.LineTypes.Line, new Pen(Color.Black, 4)));

                            break;
                        }
                    default:
                        {

                            for (int i = 0; i < EndLines.Length; i++)
                            {
                                EndLines[i] = MathHelps.TravelLine(EndLines[i], StartLines[i], 1 - (double)i / EndLines.Length * .35);
                            }
                            Lines.Add(new VectorGraphics.Line(new Point[] { EndLines[0], EndLines[EndLines.Length - 1] }, VectorGraphics.Line.LineTypes.Line, new Pen(Color.Black, 6)));
                            Lines.Add(new VectorGraphics.Line(new Point[]{
                                  MathHelps.TravelLine(EndLines[0], StartLines[0], .5) ,
                                  MathHelps.TravelLine(EndLines[EndLines.Length -1], StartLines[EndLines.Length -1], .4)},
                                  VectorGraphics.Line.LineTypes.Line,
                                  new Pen(Color.Black, 2)));

                            break;
                        }
                    //case DecorationLocation.Bottom:
                    //    {

                    //        for (int i = 0; i < EndLines.Length; i++)
                    //        {
                    //            EndLines[i] = MathHelps.TravelLine(EndLines[i], StartLines[i], .65 + Math.Pow((double)i / EndLines.Length, 2) * .35);
                    //        }
                    //        Lines.Add(new VectorGraphics.Line(new Point[] { EndLines[0], EndLines[EndLines.Length - 1] }, VectorGraphics.Line.LineTypes.Line, new Pen(Color.Black, 6)));
                    //        Lines.Add(new VectorGraphics.Line(new Point[]{
                    //              MathHelps.TravelLine(EndLines[0], StartLines[0], .95) ,
                    //              MathHelps.TravelLine(EndLines[EndLines.Length -1], StartLines[EndLines.Length -1], .95)},
                    //              VectorGraphics.Line.LineTypes.Line,
                    //              new Pen(Color.Black, 4)));

                    //        break;
                    //    }
                    //case DecorationLocation.Left:
                    //    {
                    //        Lines.Add(new VectorGraphics.Line(new Point[] { EndLines[0], StartLines[0] }, VectorGraphics.Line.LineTypes.Line, new Pen(Color.Black, 5)));
                    //        Lines.Add(new VectorGraphics.Line(new Point[] { EndLines[1], StartLines[1] }, VectorGraphics.Line.LineTypes.Line, new Pen(Color.Black, 3)));

                    //        break;
                    //    }
                    //case DecorationLocation.Right:
                    //    {
                    //        Lines.Add(new VectorGraphics.Line(new Point[] { EndLines[EndLines.Length - 1], StartLines[EndLines.Length - 1] }, VectorGraphics.Line.LineTypes.Line, new Pen(Color.Black, 5)));
                    //        Lines.Add(new VectorGraphics.Line(new Point[] { EndLines[EndLines.Length - 2], StartLines[EndLines.Length - 2] }, VectorGraphics.Line.LineTypes.Line, new Pen(Color.Black, 4)));

                    //        break;
                    //    }
                }
                #endregion
            }

            if (decoration.GetType() == typeof(ThreeDots))
            {
                #region ThreeDots
                switch (decoration.Location)
                {
                    case DecorationLocation.Top:
                        {

                            for (int i = 0; i < EndLines.Length; i++)
                            {
                                EndLines[i] = MathHelps.TravelLine(EndLines[i], StartLines[i], .65 + (double)i / EndLines.Length * .35d);
                            }

                            Dots.Add(new DecorationDot(MathHelps.TravelLine(EndLines[1], StartLines[1], .8), DecorationDot.Symbols.Dot, 15, this));
                            Dots.Add(new DecorationDot(MathHelps.TravelLine(EndLines[2], StartLines[2], .2), DecorationDot.Symbols.Dot, 9, this));
                            Dots.Add(new DecorationDot(MathHelps.TravelLine(EndLines[2], StartLines[2], .9), DecorationDot.Symbols.Dot, 4, this));

                            break;
                        }
                    case DecorationLocation.Center:
                        {

                            for (int i = 0; i < EndLines.Length; i++)
                            {
                                EndLines[i] = MathHelps.TravelLine(EndLines[i], StartLines[i], 1 - (double)i / EndLines.Length * .35);
                            }
                            Dots.Add(new DecorationDot(MathHelps.TravelLine(EndLines[1], StartLines[1], .2), DecorationDot.Symbols.Dot, 15, this));
                            Dots.Add(new DecorationDot(MathHelps.TravelLine(EndLines[1], StartLines[1], .6), DecorationDot.Symbols.Dot, 9, this));
                            Dots.Add(new DecorationDot(MathHelps.TravelLine(EndLines[2], StartLines[2], .9), DecorationDot.Symbols.Dot, 4, this));

                            break;
                        }
                    case DecorationLocation.Bottom:
                        {

                            for (int i = 0; i < EndLines.Length; i++)
                            {
                                EndLines[i] = MathHelps.TravelLine(EndLines[i], StartLines[i], .65 + Math.Pow((double)i / EndLines.Length, 2) * .35);
                            }
                            Dots.Add(new DecorationDot(MathHelps.TravelLine(EndLines[1], StartLines[1], .2), DecorationDot.Symbols.Dot, 15, this));
                            Dots.Add(new DecorationDot(MathHelps.TravelLine(EndLines[1], StartLines[1], .6), DecorationDot.Symbols.Dot, 12, this));
                            Dots.Add(new DecorationDot(MathHelps.TravelLine(EndLines[2], StartLines[2], .9), DecorationDot.Symbols.Dot, 9, this));

                            break;
                        }
                    case DecorationLocation.Left:
                        {
                            Dots.Add(new DecorationDot(MathHelps.TravelLine(EndLines[0], StartLines[0], .5), DecorationDot.Symbols.Dot, 14, this));
                            Dots.Add(new DecorationDot(MathHelps.TravelLine(EndLines[1], StartLines[1], .5), DecorationDot.Symbols.Dot, 10, this));
                            Dots.Add(new DecorationDot(MathHelps.TravelLine(EndLines[2], StartLines[2], .5), DecorationDot.Symbols.Dot, 8, this));

                            break;
                        }
                    case DecorationLocation.Right:
                        {
                            Dots.Add(new DecorationDot(MathHelps.TravelLine(EndLines[EndLines.Length - 2], StartLines[EndLines.Length - 2], .2), DecorationDot.Symbols.Dot, 12, this));
                            Dots.Add(new DecorationDot(MathHelps.TravelLine(EndLines[EndLines.Length - 2], StartLines[EndLines.Length - 2], .4), DecorationDot.Symbols.Dot, 11, this));
                            Dots.Add(new DecorationDot(MathHelps.TravelLine(EndLines[EndLines.Length - 2], StartLines[EndLines.Length - 2], .8), DecorationDot.Symbols.Dot, 10, this));
                            break;
                        }
                }
                #endregion
            }

            if (decoration.GetType() == typeof(TwoDots))
            {
                #region TwoDots
                switch (decoration.Location)
                {
                    case DecorationLocation.Top:
                        {

                            for (int i = 0; i < EndLines.Length; i++)
                            {
                                EndLines[i] = MathHelps.TravelLine(EndLines[i], StartLines[i], .65 + (double)i / EndLines.Length * .35d);
                            }

                            Dots.Add(new DecorationDot(MathHelps.TravelLine(EndLines[2], StartLines[2], .2), DecorationDot.Symbols.Dot, 9, this));
                            Dots.Add(new DecorationDot(MathHelps.TravelLine(EndLines[2], StartLines[2], .9), DecorationDot.Symbols.Dot, 6, this));

                            break;
                        }
                    case DecorationLocation.Center:
                        {

                            for (int i = 0; i < EndLines.Length; i++)
                            {
                                EndLines[i] = MathHelps.TravelLine(EndLines[i], StartLines[i], 1 - (double)i / EndLines.Length * .35);
                            }
                            Dots.Add(new DecorationDot(MathHelps.TravelLine(EndLines[1], StartLines[1], .6), DecorationDot.Symbols.Dot, 9, this));
                            Dots.Add(new DecorationDot(MathHelps.TravelLine(EndLines[2], StartLines[2], .8), DecorationDot.Symbols.Dot, 6, this));

                            break;
                        }
                    case DecorationLocation.Bottom:
                        {

                            for (int i = 0; i < EndLines.Length; i++)
                            {
                                EndLines[i] = MathHelps.TravelLine(EndLines[i], StartLines[i], .65 + Math.Pow((double)i / EndLines.Length, 2) * .35);
                            }
                            Dots.Add(new DecorationDot(MathHelps.TravelLine(EndLines[1], StartLines[1], .2), DecorationDot.Symbols.Dot, 15, this));
                            Dots.Add(new DecorationDot(MathHelps.TravelLine(EndLines[2], StartLines[2], .5), DecorationDot.Symbols.Dot, 9, this));

                            break;
                        }
                    case DecorationLocation.Left:
                        {
                            Dots.Add(new DecorationDot(MathHelps.TravelLine(EndLines[1], StartLines[1], .5), DecorationDot.Symbols.Dot, 14, this));
                            Dots.Add(new DecorationDot(MathHelps.TravelLine(EndLines[2], StartLines[2], .5), DecorationDot.Symbols.Dot, 10, this));

                            break;
                        }
                    case DecorationLocation.Right:
                        {
                            Dots.Add(new DecorationDot(MathHelps.TravelLine(EndLines[EndLines.Length - 2], StartLines[EndLines.Length - 2], .4), DecorationDot.Symbols.Dot, 12, this));
                            Dots.Add(new DecorationDot(MathHelps.TravelLine(EndLines[EndLines.Length - 2], StartLines[EndLines.Length - 2], .8), DecorationDot.Symbols.Dot, 10, this));
                            break;
                        }
                }
                #endregion
            }

            if (decoration.GetType() == typeof(TwoLines))
            {
                #region TwoLines
                switch (decoration.Location)
                {
                    case DecorationLocation.Top:
                        {

                            for (int i = 0; i < EndLines.Length; i++)
                            {
                                EndLines[i] = MathHelps.TravelLine(EndLines[i], StartLines[i], .65 + (double)i / EndLines.Length * .35);
                            }

                            AddSource("LineBig", new DecorationAnchor(new Point[] { StartLines[EndLines.Length - 2], EndLines[0] }, .2, 4, this));
                            AddSource("LineSmall", new DecorationAnchor(new Point[] { StartLines[EndLines.Length - 1], EndLines[1] }, .2, 2, this));
                            break;
                        }
                    case DecorationLocation.Center:
                        {

                            for (int i = 0; i < EndLines.Length; i++)
                            {
                                EndLines[i] = MathHelps.TravelLine(EndLines[i], StartLines[i], 1 - (double)i / EndLines.Length * .35);
                            }
                            Point p1 = MathHelps.TravelLine(EndLines[0], EndLines[1], .5);
                            Point p2 = MathHelps.TravelLine(EndLines[1], EndLines[2], .5);

                            Point p3 = MathHelps.TravelLine(StartLines[0], StartLines[1], .5);
                            Point p4 = MathHelps.TravelLine(StartLines[1], StartLines[2], .5);


                            AddSource("LineBig", new DecorationAnchor(new Point[] { p3, p1 }, .2, 4, this));
                            AddSource("LineSmall", new DecorationAnchor(new Point[] { p4, p2 }, .2, 4, this));
                            break;
                        }
                    case DecorationLocation.Bottom:
                        {

                            for (int i = 0; i < EndLines.Length; i++)
                            {
                                EndLines[i] = MathHelps.TravelLine(EndLines[i], StartLines[i], .65 + Math.Pow((double)i / EndLines.Length, 2) * .35);
                            }
                            Point p1 = MathHelps.TravelLine(EndLines[0], EndLines[1], .5);
                            Point p2 = MathHelps.TravelLine(EndLines[1], EndLines[2], .5);

                            Point p3 = MathHelps.TravelLine(StartLines[0], StartLines[1], .5);
                            Point p4 = MathHelps.TravelLine(StartLines[1], StartLines[2], .5);



                            AddSource("LineBig", new DecorationAnchor(new Point[] { p1 }, .2, 4, this));
                            AddSource("LineSmall", new DecorationAnchor(new Point[] { p2 }, .2, 4, this));
                            break;
                        }
                    case DecorationLocation.Left:
                        {

                            AddSource("LineBig", new DecorationAnchor(new Point[] { StartLines[EndLines.Length - 2], EndLines[0] }, .2, 6, this));
                            AddSource("LineSmall", new DecorationAnchor(new Point[] { StartLines[EndLines.Length - 1], EndLines[1] }, .2, 3, this));
                            break;
                        }
                    case DecorationLocation.Right:
                        {

                            AddSource("LineBig", new DecorationAnchor(new Point[] { StartLines[0], EndLines[EndLines.Length - 2] }, .2, 6, this));
                            AddSource("LineSmall", new DecorationAnchor(new Point[] { StartLines[1], EndLines[EndLines.Length - 1] }, .2, 3, this));
                            break;
                        }
                }
                #endregion
            }

            //foreach (var p in EndLines)
            //    mAnchors.Add(p);
        }
    }
}
