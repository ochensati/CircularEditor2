using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Circular.Decorations
{
    [Serializable]
    public class DecorationDot
    {
        public enum Symbols
        {
            Dot, Tick, Anchor, Arc, Diamond
        }


        public Point Location { get; private set; }
        public Symbols Symbol { get; private set; }
        public float Size { get; private set; }
        private LetterShapes.aSyllable _Syllable;

        public void DrawDot(ref Graphics path, Color backgroundColor, Color foregroundColor, bool mockup)
        {
            try
            {
                //make sure to add error handling here and in the paint event.  Error handling bubbles into the .net graphics classes, which do weird things
                switch (Symbol)
                {
                    case Symbols.Anchor:
                        {
                            //path.FillEllipse(Brushes.White, new RectangleF(Location.X - Size / 2f - 2, Location.Y - Size / 2f - 2, Size + 4, Size + 4));
                            //path.FillEllipse(Brushes.Black, new RectangleF(Location.X - Size / 2f, Location.Y - Size / 2f, Size, Size));

                            break;
                        }
                    case Symbols.Dot:
                        {
                            path.FillEllipse(Brushes.White, new RectangleF(Location.X - Size / 2f - 2, Location.Y - Size / 2f - 2, Size + 4, Size + 4));
                            path.FillEllipse(Brushes.Black, new RectangleF(Location.X - Size / 2f, Location.Y - Size / 2f, Size, Size));
                            break;
                        }
                    case Symbols.Tick:
                        {
                            Point p2 = MathHelps.D2Coords(Location, 5, _Syllable.MidAngle);


                            //new Point((int)(Location.X + 5 * Math.Cos((Letter.midAngle + 180) / 180 * Math.PI)), (int)(Location.X + 5 * Math.Sin((Letter.midAngle + 180) / 180 * Math.PI)));
                            path.DrawLine(Pens.Black, Location, p2);
                            break;
                        }
                    case Symbols.Arc:
                        {
                            Rectangle r = MathHelps.Circle2Rect(_Syllable.LetterCenter, ArcRadius);// new Rectangle((int)(Letter.LetterCenter.X - ArcRadius), (int)(Letter.LetterCenter.Y - ArcRadius), (int)(ArcRadius * 2), (int)(ArcRadius * 2));

                            Pen p = new Pen(Color.Black, Size);
                            path.DrawArc(p, r, AngleStart, ArcLength);

                            break;
                        }

                    case Symbols.Diamond:
                        {

                            GraphicsPath pBlack;
                            GraphicsPath pWhite;

                            pBlack = new GraphicsPath();
                            pBlack.StartFigure();
                            pBlack.AddLine(corners[0], corners[1]);
                            pBlack.AddLine(corners[1], corners[2]);
                            pBlack.AddLine(corners[2], corners[0]);
                            pBlack.CloseFigure();

                            pWhite = new GraphicsPath();
                            pWhite.StartFigure();
                            pWhite.AddLine(corners[0], corners[2]);
                            pWhite.AddLine(corners[2], corners[3]);
                            pWhite.AddLine(corners[3], corners[0]);
                            pWhite.CloseFigure();
                            path.FillPath(Brushes.White, pWhite);
                           path.FillPath(Brushes.Black, pBlack);

                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);

            }
        }

        public DecorationDot(Point location, Symbols symbol, float size, LetterShapes.aSyllable parent)
        {

            Location = location;
            this.Symbol = symbol;
            this.Size = size;
            _Syllable = parent;
        }


        float AngleStart, ArcLength, ArcRadius;

        public DecorationDot(Symbols symbol, LetterShapes.aSyllable parent, float angleStart, float arcLength, float arcRadius, float arcPenWidth)
        {
            Location = parent.LetterCenter;
            this.Symbol = symbol;
            this.Size = arcPenWidth;
            _Syllable = parent;
            AngleStart = angleStart;
            ArcLength = arcLength;
            ArcRadius = arcRadius;
        }


        Point[] corners;

        
        public DecorationDot(Symbols symbol, Point location, float angleStart, float size)
        {

            this.Symbol = symbol;
            this.Size = size;
            AngleStart = angleStart;

           
            corners = new Point[4];
            corners[0] = MathHelps.D2Coords(location, size, angleStart);
            corners[1] = MathHelps.D2Coords(location, size*.75, angleStart + 90);
            corners[2] = MathHelps.D2Coords(location, size, angleStart + 180);
            corners[3] = MathHelps.D2Coords(location, size*.75, angleStart + 270);

           

        }
    }

}
