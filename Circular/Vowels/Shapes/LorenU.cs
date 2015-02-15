using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Circular.Decorations;

namespace Circular.Vowels.Shapes
{
    [Serializable]
    public class LorenU : aVowel
    {
        public override aVowel HandlesEngLetter(int vowelIndex, engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {
            if (letter.Vowel[vowelIndex].ToString().Contains("u") == true)
                return new LorenU();
            else
                return null;
        }
        private bool Recalcing = false;
        protected override void CalcVowel(System.Drawing.Rectangle VowelBounds, double vX, double vY, double vR)
        {

            try
            {
                if (Recalcing == false)
                {
                    if (this._Syllable.GetType() == typeof(Circular.LetterShapes.Shapes.FullMoon) || this._Syllable.GetType() == typeof(Circular.LetterShapes.Shapes.MoonRise))
                        this._Syllable.Vowels[0].Location = VowelLocation.Center;
                    else
                        this._Syllable.Vowels[0].Location = VowelLocation.OnLine;

                    Recalcing = true;
                    _Syllable.CalculateVowel(this);

                    Recalcing = false;
                    //make sure to add error handling here and in the paint event.  Error handling bubbles into the .net graphics classes, which do weird things

                }
                else
                {
                    _ControlPoints = new Point[] { MathHelps.D2Coords(new Point((int)vX, (int)vY), vR, _Syllable.MidAngle ), MathHelps.D2Coords(new Point((int)vX, (int)vY), 2 * vR, _Syllable.MidAngle ) };

                }

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
                    path.DrawEllipse(new Pen(Color.Black, 2), VowelBounds);
                    path.DrawLine(new Pen(Color.Black, 2), _ControlPoints[0], _ControlPoints[1]);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);

            }
        }
    }
}
