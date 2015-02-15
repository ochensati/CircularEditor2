using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Circular.Vowels.Shapes
{
    [Serializable]
    class Half : aVowel
    {
        public override aVowel HandlesEngLetter(int vowelIndex, engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {
            if (letter.Vowel[vowelIndex].ToString().Contains("a") == true)
                return new Half();
            else
                return null;
        }

        protected override void CalcVowel(System.Drawing.Rectangle VowelBounds, double vX, double vY, double vR)
        {

        }

        public override void DrawVowelImpl(ref Graphics path, Color backgroundColor, Color foregroundColor, bool mockup)
        {
            try
            {
                //make sure to add error handling here and in the paint event.  Error handling bubbles into the .net graphics classes, which do weird things
                float angle = (float)(_CircleAngle);
                if (mockup)
                {
                    path.FillPie(new SolidBrush(backgroundColor), VowelBounds, angle, 180);
                }
                else
                {
                    
                    path.FillPie(Brushes.White, new Rectangle(VowelBounds.X - 3, VowelBounds.Y - 3, VowelBounds.Width + 6, VowelBounds.Height + 6), angle, 180);
                    path.FillPie(Brushes.Black, VowelBounds, angle, 180);
                    path.DrawEllipse(Pens.Black, VowelBounds);

                    Rectangle r2 = VowelBounds;
                    r2.Inflate((int)(VowelBounds.Width / -3d), (int)(VowelBounds.Width / -3d));
                    path.FillPie(Brushes.White, r2, angle, 180);
                    path.DrawEllipse(Pens.Black, r2);

                    if (_Syllable.Fancy)
                    {
                      
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
