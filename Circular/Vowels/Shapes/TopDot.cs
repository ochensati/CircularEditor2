using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Circular.Decorations;

namespace Circular.Vowels.Shapes
{
    [Serializable]
    class TopDot : aVowel
    {
        public override aVowel HandlesEngLetter(int vowelIndex, engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {
            if (letter.Vowel[vowelIndex].ToString().Contains("o") == true)
                return new TopDot();
            else
                return null;
        }

        protected override void CalcVowel(System.Drawing.Rectangle VowelBounds, double vX, double vY, double vR)
        {
            try
            {
                //make sure to add error handling here and in the paint event.  Error handling bubbles into the .net graphics classes, which do weird things
                //if (_Location == VowelLocation.Center)
                //{
                //    double angle = _TiltAngle;

                //    //Anchors = new Point[] { new Point((int)(vX + vR * Math.Cos(angle / 180 * Math.PI)), (int)(vY + vR * Math.Sin(angle / 180 * Math.PI))) };
                //    _ControlPoints = new Point[] { MathHelps.D2Coords(vX, vY, vR, angle) };
                //    Anchors.Add("Dot1", new DecorationAnchor(new Point[] { _ControlPoints[0] }, .7, 2, this));
                //}
                //else
                //{
                    double angle = _CircleAngle;

                    // Anchors = new Point[] { new Point((int)(vX + vR * Math.Cos(angle / 180 * Math.PI)), (int)(vY + vR * Math.Sin(angle / 180 * Math.PI))) };
                    _ControlPoints = new Point[] { MathHelps.D2Coords(vX, vY, vR, angle) };
                    Anchors.Add("Dot1", new DecorationAnchor(new Point[] { _ControlPoints[0] }, .7, 2, this));
                //}

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);

            }
        }

        public override void DrawVowelImpl(ref Graphics path, Color backgroundColor, Color foregroundColor, bool mockup)
        {
            try
            {
                //make sure to add error handling here and in the paint event.  Error handling bubbles into the .net graphics classes, which do weird things

                if (mockup)
                {

                    path.FillEllipse(new SolidBrush(backgroundColor), VowelBounds);

                }
                else
                {
                    if (!_Syllable.Fancy)
                    {
                        path.DrawEllipse(Pens.Black, VowelBounds);
                        Rectangle VowelBounds2 = VowelBounds;
                        VowelBounds2.Inflate(-3, -3);
                        VowelBounds2.X += 1;

                        path.FillEllipse(Brushes.White, VowelBounds2);
                        double r2 = 6;
                        VowelBounds2 = MathHelps.Circle2Rect(_ControlPoints[0], r2);// new Rectangle((int)(Anchors[0].X - r2), (int)(Anchors[0].Y - r2), (int)(2 * r2), (int)(2 * r2));
                        path.FillEllipse(Brushes.White, VowelBounds2);
                        path.FillEllipse(Brushes.Black, VowelBounds2);
                    }
                    else
                    {
                        path.FillEllipse(Brushes.Black, VowelBounds);
                        Rectangle VowelBounds2 = VowelBounds;
                        VowelBounds2.Inflate(-3, -3);
                        VowelBounds2.X += 1;

                        path.FillEllipse(Brushes.White, VowelBounds2);

                        double r2 = 5;
                        VowelBounds2 = MathHelps.Circle2Rect(_ControlPoints[0], r2);
                        path.FillEllipse(Brushes.White, VowelBounds2);

                        VowelBounds2 = MathHelps.Circle2Rect(_ControlPoints[0], r2);
                        path.DrawEllipse(Pens.Black, VowelBounds2);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);

            }
        }
    }
}
