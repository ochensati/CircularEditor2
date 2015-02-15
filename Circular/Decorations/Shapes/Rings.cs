using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circular.Words;

namespace Circular.Decorations.Shapes
{
     [Serializable]
    public class Rings : aDecoration
    {
         public override aDecoration HandlesEngLetter(engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {
            if ("_p_q_ng_st_y_".Contains("_" + letter.Consonant + "_"))
                return new Rings();
            else
                return null;
        }

        public override LocationProb LocationProb()
        {
            return new LocationProb(
             vAbove: 1.5,
             vCenter: 2,
             vLeft: 1,
             dAbove: 1,
             dBottom: .5,
             dCenter: 4,
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
                    case DecorationLocation.Top:
                    case DecorationLocation.Left:
                    case DecorationLocation.Right:
                        {
                            _DecorationDots.Add(new DecorationDot(DecorationDot.Symbols.Arc, _Syllable, (float)_Syllable.SubStartAngle, (float)_Syllable.SubArc, (float)arcRadius, 6));
                            break;
                        }
                    case DecorationLocation.Center:
                        {
                            _DecorationDots.Add(new DecorationDot(DecorationDot.Symbols.Arc, _Syllable, (float)_Syllable.SubStartAngle, (float)_Syllable.SubArc, (float)arcRadius, 2));
                            break;
                        }
                   
                        //{
                        //    float endAngle = _Syllable.SubStartAngle + _Syllable.SubArc;

                        //    float arcGap = (float)(arcMidAngle - _Syllable.SubStartAngle);

                        //    float arcStart2 = _Syllable.SubStartAngle + _Syllable.SubArc * .05f;

                        //    float arcGap2 = endAngle - arcStart2;

                        //    _DecorationDots.Add(new DecorationDot(DecorationDot.Symbols.Arc, _Syllable, (float)_Syllable.SubStartAngle, arcGap, (float)arcRadius, 3));
                        //    _DecorationDots.Add(new DecorationDot(DecorationDot.Symbols.Arc, _Syllable, arcStart2, (float)(arcGap2), (float)arcRadius, 3));

                        //    break;
                        //}
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
