using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Circular.Words;

namespace Circular.Decorations.Shapes
{
    [Serializable]
    public class ThreeLines : aDecoration
    {
        public override aDecoration HandlesEngLetter(engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {
            if (scriptStyle == Circular.aCircleObject.ScriptStyles.Ashcroft)
            {
                if ("_n_k_z_d_sh_".Contains("_" + letter.Consonant + "_"))
                    return new ThreeLines();
                else
                    return null;
            }
            else
            {
                if ("_f_m_s_ng_".Contains("_" + letter.Consonant + "_"))
                    return new ThreeLines();
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
                            double gapAngle = arcWidth * .5;
                            if (gapAngle > 35)
                                gapAngle = 35;

                            Point p1 = MathHelps.D2Coords(arcX, arcY, arcRadius + 3, (arcMidAngle - gapAngle));
                            Point p2 = MathHelps.D2Coords(arcX, arcY, arcRadius + 4, (arcMidAngle));
                            Point p3 = MathHelps.D2Coords(arcX, arcY, arcRadius + 7, (arcMidAngle + gapAngle));

                            Point p4 = MathHelps.D2Coords(arcX, arcY, arcRadius * 1.3, (arcMidAngle - gapAngle));
                            Point p5 = MathHelps.D2Coords(arcX, arcY, arcRadius * 1.3, (arcMidAngle));
                            Point p6 = MathHelps.D2Coords(arcX, arcY, arcRadius * 1.3, (arcMidAngle + gapAngle));

                            _DecorationDots.Add(new DecorationDot(p1, DecorationDot.Symbols.Anchor, 1, _Syllable));
                            _DecorationDots.Add(new DecorationDot(p2, DecorationDot.Symbols.Anchor, 2, _Syllable));
                            _DecorationDots.Add(new DecorationDot(p3, DecorationDot.Symbols.Anchor, 3, _Syllable));



                            _DecorationDots.Add(new DecorationDot(DecorationDot.Symbols.Diamond, p1, (float)arcMidAngle, 4));
                            _DecorationDots.Add(new DecorationDot(DecorationDot.Symbols.Diamond, p2, (float)arcMidAngle, 4));
                            _DecorationDots.Add(new DecorationDot(DecorationDot.Symbols.Diamond, p3, (float)arcMidAngle, 4));

                            _Sources.Add("LineSmall", new DecorationAnchor(new Point[] { p1, p4 }, .2, 2, this));
                            _Sources.Add("LineMed", new DecorationAnchor(new Point[] { p2, p5 }, .6, 3, this));
                            _Sources.Add("LineBig", new DecorationAnchor(new Point[] { p3, p6 }, .8, 4, this));


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
