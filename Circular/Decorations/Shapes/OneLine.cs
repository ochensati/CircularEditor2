using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Circular.Words;

namespace Circular.Decorations.Shapes
{
    [Serializable]
    public class OneLine : aDecoration
    {
        public override aDecoration HandlesEngLetter(engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {
            if (scriptStyle == Circular.aCircleObject.ScriptStyles.Ashcroft)
            {
                if ("_n_k_c_z_d_".Contains("_" + letter.Consonant + "_"))
                    return new OneLine();
                else
                    return null;

            }
            else
            {
                if ("_g_n_v_q_".Contains("_" + letter.Consonant + "_"))
                    return new OneLine();
                else
                    return null;

            }
        }

        public override LocationProb LocationProb()
        {
            return new LocationProb(
             vAbove: .5,
             vCenter: 2,
             vLeft: 1,
             dAbove: 3,
             dBottom: 0,
             dCenter: 0,
             dLeft: 1,
             dRight: 1);
        }



        protected override void CalcDecoration(double arcRadius, double arcMidAngle, double arcX, double arcY, double arcWidth)
        {
            try
            {
                //make sure to add error handling here and in the paint event.  Error handling bubbles into the .net graphics classes, which do weird things


                switch (_Location)
                {
                    case DecorationLocation.Bottom:
                    case DecorationLocation.Top:
                    case DecorationLocation.Left:
                    case DecorationLocation.Right:
                    case DecorationLocation.Center:
                        {
                            double gapAngle = _Syllable.SubArc * .1;
                            Point p1 = MathHelps.D2Coords(arcX, arcY, arcRadius, (arcMidAngle - gapAngle));
                            Point p2 = MathHelps.D2Coords(arcX, arcY, arcRadius, (arcMidAngle));

                            Point p3 = MathHelps.D2Coords(arcX, arcY, arcRadius * 1.5, (arcMidAngle - gapAngle));
                            Point p4 = MathHelps.D2Coords(arcX, arcY, arcRadius * 1.5, (arcMidAngle));

                            _DecorationDots.Add(new DecorationDot(p2, DecorationDot.Symbols.Anchor, 3, _Syllable));
                            _DecorationDots.Add(new DecorationDot(DecorationDot.Symbols.Diamond, p2, (float)arcMidAngle, 4));

                            _Sources.Add("LineBig", new DecorationAnchor(new Point[] { p2, p4 }, .2, 3, this));


                            break;
                        }


                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);

            }
        }


    }
}
