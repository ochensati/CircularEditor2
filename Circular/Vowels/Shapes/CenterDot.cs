using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Circular.Decorations;

namespace Circular.Vowels.Shapes
{
    [Serializable]
    public class CenterDot : aVowel
    {
        public override aVowel HandlesEngLetter(int vowelIndex, engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {
            if (letter.Vowel[vowelIndex].ToString().Contains("i") == true)
                return new CenterDot();
            else
                return null;
        }

        protected override void CalcVowel(System.Drawing.Rectangle VowelBounds, double vX, double vY, double vR)
        {

            try
            {
                //make sure to add error handling here and in the paint event.  Error handling bubbles into the .net graphics classes, which do weird things
                _ControlPoints = new Point[] { new Point((int)vX, (int)vY) };
                Anchors .Add( "Dot1", new DecorationAnchor(new Point[] { _ControlPoints[0] }, .2, 2, this) );
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
                    VowelBounds2.Inflate(-3, -3);
                    VowelBounds2.X += 1;

                    path.FillEllipse(Brushes.White, VowelBounds2);

                    double r2 = 3;
                     VowelBounds2 = MathHelps.Circle2Rect(VowelBounds, r2);// new Rectangle((int)(VowelBounds.X + VowelBounds.Width / 2-r2), (int)(VowelBounds.Y + VowelBounds.Height / 2-r2), (int)(2*r2), (int)(2*r2));
                    path.FillEllipse(Brushes.Black, VowelBounds2);

                    if (_Syllable.Fancy)
                    {
                        Rectangle r3 = VowelBounds;
                        r3.Inflate((int)(VowelBounds.Width / -3d), (int)(VowelBounds.Width / -3d));
                        Pen p = new Pen(Color.Black, 6);
                        path.DrawEllipse(p, r3);
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
