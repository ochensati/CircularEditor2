using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Circular.Decorations;

namespace Circular.Vowels.Shapes
{
    [Serializable]
    class TwoDot : aVowel
    {
        public override aVowel HandlesEngLetter(int vowelIndex, engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {
            if (letter.Vowel[vowelIndex].ToString().Contains("u") == true)
                return new TwoDot();
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
                //    double angle = _TiltAngle; //Parent.midAngle + 90;

                //    _ControlPoints = new Point[] { MathHelps.D2Coords(vX, vY, vR, angle), MathHelps.D2Coords(vX, vY, -1 * vR, angle) };
                //    Anchors.Add("Dot1", new DecorationAnchor(new Point[] { _ControlPoints[0] }, .2, 2, this));
                //    Anchors.Add("Dot2", new DecorationAnchor(new Point[] { _ControlPoints[1] }, .4, 2, this));
                //}
                //else
                //{
                    double angle = _CircleAngle;

                    _ControlPoints = new Point[] { MathHelps.D2Coords(vX, vY, vR, angle), MathHelps.D2Coords(vX, vY, -1 * vR, angle) };
                    Anchors.Add("Dot1", new DecorationAnchor(new Point[] { _ControlPoints[0] }, .2, 2, this));
                    Anchors.Add("Dot2", new DecorationAnchor(new Point[] { _ControlPoints[1] }, .4, 2, this));
               // }
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

                    if (_Syllable.Fancy)
                    {
                        path.FillEllipse(Brushes.Black, VowelBounds);
                        Rectangle VowelBounds2 = VowelBounds;
                        VowelBounds2.Inflate(-3, -4);
                        VowelBounds2.X += 2;

                        path.FillEllipse(Brushes.White, VowelBounds2);

                        double r2 = 5;
                        VowelBounds2 = MathHelps.Circle2Rect(_ControlPoints[0], r2);
                        path.FillEllipse(Brushes.White, VowelBounds2);

                        VowelBounds2 = MathHelps.Circle2Rect(_ControlPoints[1], r2);
                        path.FillEllipse(Brushes.White, VowelBounds2);

                        VowelBounds2 = MathHelps.Circle2Rect(_ControlPoints[0], r2);
                        path.DrawEllipse(Pens.Black, VowelBounds2);

                        VowelBounds2 = MathHelps.Circle2Rect(_ControlPoints[1], r2);
                        path.DrawEllipse(Pens.Black, VowelBounds2);


                    }
                    else
                    {
                        path.DrawEllipse(Pens.Black, VowelBounds);

                        Rectangle VowelBounds2 = VowelBounds;
                        VowelBounds2.Inflate(-3, -3);
                        VowelBounds2.X += 1;

                        path.FillEllipse(Brushes.White, VowelBounds2);
                        double r2 = 3;
                         VowelBounds2 = MathHelps.Circle2Rect(_ControlPoints[0], r2);
                        path.FillEllipse(Brushes.Black, VowelBounds2);

                        VowelBounds2 = MathHelps.Circle2Rect(_ControlPoints[1], r2);
                        path.FillEllipse(Brushes.Black, VowelBounds2);

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

