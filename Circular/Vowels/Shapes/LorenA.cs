using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Circular.Decorations;

namespace Circular.Vowels.Shapes
{
    [Serializable]
    public class LorenA : aVowel
    {
        public override aVowel HandlesEngLetter(int vowelIndex, engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {
            if (letter.Vowel[vowelIndex].ToString().Contains("a") == true)
                return new LorenA();
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
                   
                        this._Syllable.Vowels[0].Location = VowelLocation.Below;

                    Recalcing = true;
                    _Syllable.CalculateVowel(this);

                    Recalcing = false;
                    //make sure to add error handling here and in the paint event.  Error handling bubbles into the .net graphics classes, which do weird things

                }
                else
                {
                    _ControlPoints = new Point[] { };

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
                    path.DrawEllipse(new Pen(Color.Black,2) , VowelBounds);
                   
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);

            }
        }
    }
}
