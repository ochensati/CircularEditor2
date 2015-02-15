using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Gallafry2
{
    //public class GalLetter
    //{
        
        

    //    


       


       

    //    //public void DrawArc(ref GraphicsPath path)
    //    //{
    //    //    if (LetterType == LetterTypes.None || LetterType == LetterTypes.Vowel)
    //    //    {
    //    //        path.AddArc(WordParent.WordBounds, startAngle, ArcWidth);
    //    //    }
    //    //    else
    //    //    {
    //    //        if (edges == null || LetterType == LetterTypes.Saturn || LetterType == LetterTypes.FullMoon)
    //    //        {
    //    //            path.AddArc(WordParent.WordBounds, startAngle, ArcWidth);
    //    //            //newBounds = edge.BoundingRectangle(edge.CenterLine, maxRadius);
    //    //            path.AddEllipse(LetterBounds);

    //    //        }
    //    //        else
    //    //        {
    //    //            if (lineThrough == false)
    //    //            {
    //    //                path.AddArc(WordParent.WordBounds, startAngle, (float)(mainAngles[0] - startAngle));
    //    //                path.AddArc(LetterBounds, (float)subAngles[0], (float)(SubArc));
    //    //                path.AddArc(WordParent.WordBounds, (float)(mainAngles[1]), (float)(endAngle - mainAngles[1]));
    //    //            }
    //    //            else
    //    //            {
    //    //                path.AddArc(LetterBounds, (float)subAngles[0], (float)(SubArc));
    //    //                path.AddArc(WordParent.WordBounds, (float)(startAngle), (float)(endAngle - startAngle));
    //    //            }
    //    //        }
    //    //    }
    //    //}

      

      

    //    //private void AddArc()
    //    //{
    //    //    //double circleDepth = 1;
    //    //    LetterRadius = maxInnerRadius * .8;
    //    //    if (LetterRadius > WordParent.WordRadius * .4)
    //    //        LetterRadius = WordParent.WordRadius * .35;


    //    //    switch (LetterType)
    //    //    {
    //    //        #region BigSwitch
    //    //        case LetterTypes.Arc:
    //    //            LetterBounds = BoundingRectangle(CenterLine, LetterRadius);
    //    //            lineThrough = false;
    //    //            break;
    //    //        case LetterTypes.FullMoon:
    //    //            LetterBounds = BoundingRectangle(BehindLine(WordParent.WordRadius - LetterRadius * 1.1), LetterRadius);
    //    //            lineThrough = true;
    //    //            break;

    //    //        case LetterTypes.MoonRise:
    //    //            LetterBounds = BoundingRectangle(BehindLine(WordParent.WordRadius - LetterRadius * .8), LetterRadius);
    //    //            lineThrough = false;
    //    //            break;

    //    //        case LetterTypes.Saturn:
    //    //            LetterBounds = BoundingRectangle(CenterLine, LetterRadius);
    //    //            lineThrough = true;
    //    //            break;

    //    //        case LetterTypes.Harp:
    //    //            LetterBounds = BoundingRectangle(CenterLine, LetterRadius);
    //    //            lineThrough = true;
    //    //            break;

    //    //        case LetterTypes.Connection:
    //    //            LetterBounds = BoundingRectangle(CenterLine, LetterRadius);
    //    //            lineThrough = false;
    //    //            break;

    //    //        case LetterTypes.None:
    //    //            LetterBounds = BoundingRectangle(CenterLine, LetterRadius);
    //    //            lineThrough = true;
    //    //            break;

    //    //        case LetterTypes.Vowel:
    //    //            LetterBounds = BoundingRectangle(CenterLine, LetterRadius);
    //    //            lineThrough = true;
    //    //            break;
    //    //        #endregion
    //    //    }




    //    //    edges = FindIntersections(LetterBounds);

    //    //    if (edges == null)
    //    //    {
    //    //        SubArc = 360;
    //    //    }
    //    //    else
    //    //    {
    //    //        mainAngles = GetAngles(edges);
    //    //        subAngles = GetAngles(edges, LetterBounds);

    //    //        SubStartAngle = (float)subAngles[0];
    //    //        SubEndAngle = (float)subAngles[1];

    //    //        double a1 = 361 - subAngles[1];

    //    //        SubArc = (float)(-1 * Math.Abs((subAngles[1] + a1) % 360 - (subAngles[0] + a1) % 360));
    //    //    }
    //    //}


     
      

       

    //    //private void CalculateAppearance(engLetter letter)
    //    //{
    //    //    this.VowelType = VowelTypes.None;

    //    //    if (letter.Vowel.Contains('a'))
    //    //        this.VowelType = VowelTypes.Half;
    //    //    if (letter.Vowel.Contains('e'))
    //    //        this.VowelType = VowelTypes.CrossLine;
    //    //    if (letter.Vowel.Contains('i'))
    //    //        this.VowelType = VowelTypes.CenterDot;
    //    //    if (letter.Vowel.Contains('o'))
    //    //        this.VowelType = VowelTypes.EdgeDot;
    //    //    if (letter.Vowel.Contains('u'))
    //    //        this.VowelType = VowelTypes.TwoDot;


    //    //    this.LetterType = LetterTypes.None;
    //    //    if (letter.isVowel)
    //    //    {
    //    //        this.LetterType = LetterTypes.Vowel;
    //    //    }
    //    //    if ("mnjpb".Contains(letter.Consonant))
    //    //    {
    //    //        this.LetterType = LetterTypes.MoonRise;
    //    //    }
    //    //    if ("lkg~v".Contains(letter.Consonant))
    //    //    {
    //    //        this.LetterType = LetterTypes.FullMoon;
    //    //    }
    //    //    if ("rzh@f".Contains(letter.Consonant))
    //    //    {
    //    //        this.LetterType = LetterTypes.Arc;
    //    //    }
    //    //    if ("tdw#s".Contains(letter.Consonant))
    //    //    {
    //    //        this.LetterType = LetterTypes.Saturn;
    //    //    }
    //    //    if ("^_xy`".Contains(letter.Consonant))
    //    //    {
    //    //        this.LetterType = LetterTypes.Harp;
    //    //    }

    //    //    this.LetterDecoration = LetterDecorations.None;

    //    //    if ("nkzd_".Contains(letter.Consonant))
    //    //    {
    //    //        this.LetterDecoration = LetterDecorations.TwoLines;
    //    //    }
    //    //    if ("jghwx".Contains(letter.Consonant))
    //    //    {
    //    //        this.LetterDecoration = LetterDecorations.ThreeSpots;
    //    //    }
    //    //    if ("p~@#y".Contains(letter.Consonant))
    //    //    {
    //    //        this.LetterDecoration = LetterDecorations.RainBow;
    //    //    }
    //    //    if ("bvfs`".Contains(letter.Consonant))
    //    //    {
    //    //        this.LetterDecoration = LetterDecorations.TwoSpots;
    //    //    }

    //    //}

      
    //}

}
