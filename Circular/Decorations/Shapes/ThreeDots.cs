using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Circular.Words;

namespace Circular.Decorations.Shapes
{
    [Serializable]
    public class ThreeDots : aDecoration
    {
        public override aDecoration HandlesEngLetter(engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {

            if (scriptStyle == Circular.aCircleObject.ScriptStyles.Ashcroft)
            {
                if ("_j_g_h_w_x_".Contains("_" + letter.Consonant + "_"))
                return new ThreeDots();
            else
                return null;
            }
            else
            {
                if ("_d_l_r_z_".Contains("_" + letter.Consonant + "_"))
                    return new ThreeDots();
                else
                    return null;

            }

        }

        public override LocationProb LocationProb()
        {
            return new LocationProb(
             vAbove: 1,
             vCenter: 2,
             vLeft: 1,
             dAbove: 1,
             dBottom: .5,
             dCenter: 1,
             dLeft: 2,
             dRight: 2);
        }

        protected override void CalcDecoration(double arcRadius, double arcMidAngle, double arcX, double arcY, double arcWidth)
        {
            try
            {
                //make sure to add error handling here and in the paint event.  Error handling bubbles into the .net graphics classes, which do weird things
                switch (_Location)
                {
                    case DecorationLocation.Bottom:
                        {
                            double gapAngle = arcWidth * .5;
                            if (gapAngle > 35)
                                gapAngle = 35;

                            Point p1 = MathHelps.D2Coords(arcX, arcY, arcRadius, (arcMidAngle - gapAngle));
                            Point p2 = MathHelps.D2Coords(arcX, arcY, arcRadius, (arcMidAngle));
                            Point p3 = MathHelps.D2Coords(arcX, arcY, arcRadius, (arcMidAngle + gapAngle));

                            Point p4 = MathHelps.D2Coords(arcX, arcY, arcRadius * 1.3, (arcMidAngle - gapAngle));
                            Point p5 = MathHelps.D2Coords(arcX, arcY, arcRadius * 1.3, (arcMidAngle));
                            Point p6 = MathHelps.D2Coords(arcX, arcY, arcRadius * 1.3, (arcMidAngle + gapAngle));

                            _DecorationDots.Add(new DecorationDot(p1, DecorationDot.Symbols.Dot, 6, _Syllable));
                            _DecorationDots.Add(new DecorationDot(p2, DecorationDot.Symbols.Dot, 8, _Syllable));
                            _DecorationDots.Add(new DecorationDot(p3, DecorationDot.Symbols.Dot, 13, _Syllable));


                            break;
                        }

                    case DecorationLocation.Top:
                    case DecorationLocation.Left:
                    case DecorationLocation.Right:
                        {
                            double gapAngle = arcWidth * .5;
                            if (gapAngle > 35)
                                gapAngle = 35;

                            Point p1 = MathHelps.D2Coords(arcX, arcY, arcRadius , (arcMidAngle - gapAngle));
                            Point p2 = MathHelps.D2Coords(arcX, arcY, arcRadius + 4, (arcMidAngle));
                            Point p3 = MathHelps.D2Coords(arcX, arcY, arcRadius + 6, (arcMidAngle + gapAngle));

                            Point p4 = MathHelps.D2Coords(arcX, arcY, arcRadius * 1.3, (arcMidAngle - gapAngle));
                            Point p5 = MathHelps.D2Coords(arcX, arcY, arcRadius * 1.3, (arcMidAngle));
                            Point p6 = MathHelps.D2Coords(arcX, arcY, arcRadius * 1.3, (arcMidAngle + gapAngle));

                            _DecorationDots.Add(new DecorationDot(p1, DecorationDot.Symbols.Dot, 6, _Syllable));
                            _DecorationDots.Add(new DecorationDot(p2, DecorationDot.Symbols.Dot, 8, _Syllable));
                            _DecorationDots.Add(new DecorationDot(p3, DecorationDot.Symbols.Dot, 13, _Syllable));

                            if (arcRadius > _Syllable.LetterRadius)
                            {
                                if (_Location != DecorationLocation.Left)
                                    _Anchors.Add("DotSmall", new DecorationAnchor(new Point[] { p1, p4 }, .2, 2, this));

                                _Anchors.Add("DotMed", new DecorationAnchor(new Point[] { p2, p5 }, .6, 3, this));

                                if (_Location != DecorationLocation.Right)
                                    _Anchors.Add("DotBig", new DecorationAnchor(new Point[] { p3, p6 }, .8, 4, this));
                            }
                            break;
                        }
                    case DecorationLocation.Center:
                        {
                            double aMiddle = arcMidAngle + 90;

                            if (arcWidth == 0)
                            {

                                aMiddle = arcMidAngle;

                                Point p1 = MathHelps.D2Coords(arcX, arcY, .1 * arcRadius, (aMiddle));
                                Point p2 = MathHelps.D2Coords(arcX, arcY, .5 * arcRadius, (aMiddle));
                                Point p3 = MathHelps.D2Coords(arcX, arcY, 1.3 * arcRadius, (aMiddle));


                                _DecorationDots.Add(new DecorationDot(p1, DecorationDot.Symbols.Dot, 6, _Syllable));
                                _DecorationDots.Add(new DecorationDot(p2, DecorationDot.Symbols.Dot, 8, _Syllable));
                                _DecorationDots.Add(new DecorationDot(p3, DecorationDot.Symbols.Dot, 16, _Syllable));

                                _Anchors.Add("DotBig", new DecorationAnchor(new Point[] { p2, p3 }, .8, 2, this));

                            }
                            else
                            {
                                Point p1 = _Syllable.LetterCenter;
                                Point p2 = MathHelps.D2Coords(arcX, arcY, -1 * arcRadius, (aMiddle));
                                Point p3 = MathHelps.D2Coords(arcX, arcY, 1 * arcRadius, (aMiddle));

                                Point p4 = MathHelps.D2Coords(arcX, arcY, .3 * arcRadius, (_Syllable.MidAngle + 180));
                                Point p5 = MathHelps.D2Coords(arcX, arcY, -1.3 * arcRadius, (aMiddle));
                                Point p6 = MathHelps.D2Coords(arcX, arcY, 1.3 * arcRadius, (aMiddle));

                                _DecorationDots.Add(new DecorationDot(p1, DecorationDot.Symbols.Dot, 6, _Syllable));
                                _DecorationDots.Add(new DecorationDot(p2, DecorationDot.Symbols.Dot, 8, _Syllable));
                                _DecorationDots.Add(new DecorationDot(p3, DecorationDot.Symbols.Dot, 13, _Syllable));

                                _Anchors.Add("DotSmall", new DecorationAnchor(new Point[] { p1, p4 }, .3, 2, this));
                                _Anchors.Add("DotMed", new DecorationAnchor(new Point[] { p2, p5 }, .6, 2, this));
                                _Anchors.Add("DotBig", new DecorationAnchor(new Point[] { p3, p6 }, .8, 2, this));
                            }
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
