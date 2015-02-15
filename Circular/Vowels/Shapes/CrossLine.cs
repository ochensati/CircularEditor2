using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Circular.Vowels.Shapes
{
    [Serializable]
    class CrossLine : aVowel
    {
        public override aVowel HandlesEngLetter(int vowelIndex, engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {
            if (letter.Vowel[vowelIndex].ToString().Contains("e") == true)
                return new CrossLine();
            else
                return null;
        }
        // Point[] p1;
        Rectangle _Circle2;
        protected override void CalcVowel(System.Drawing.Rectangle VowelBounds, double vX, double vY, double vR)
        {
            try
            {
                //make sure to add error handling here and in the paint event.  Error handling bubbles into the .net graphics classes, which do weird things
                double angle = _CircleAngle;

                _ControlPoints = new Point[] { MathHelps.D2Coords(vX, vY, vR, angle), MathHelps.D2Coords(vX, vY, -1 * vR, angle) };

                _Circle2 = VowelBounds;
                _Circle2.Inflate((int)(VowelBounds.Width * -.2), (int)(VowelBounds.Width * -.2));

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
                    path.DrawEllipse(Pens.Black, VowelBounds);
                    Rectangle VowelBounds2 = VowelBounds;
                    VowelBounds2.Inflate(-2, -2);
                   

                    path.FillEllipse(Brushes.White, VowelBounds2);
                    if (_Syllable.Fancy)
                    {
                        Pen p = new Pen(Color.Black, 6);
                        path.DrawEllipse(p, _Circle2);
                    }
                    Pen p2 = new Pen(Color.Black, 2);


                    path.DrawLine(p2, _ControlPoints[0], _ControlPoints[1]);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);

            }
        }
    }
}
