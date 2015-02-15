using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circular.Vowels;
using Circular.Decorations;

namespace Circular.Words
{
    public class LocationProb
    {
        public double pVAbove = 1;
        public double pVCenter = 1;
        public double pVLeft = 1;
        public double pDAbove = 1;
        public double pDBottom = 1;
        public double pDCenter = 1;
        public double pDLeft = 1;
        public double pDRight = 1;

        public LocationProb(double vAbove, double vCenter, double vLeft, double dAbove, double dBottom, double dCenter, double dLeft, double dRight)
        {
            pVAbove = vAbove;
            pVCenter = vCenter;
            pVLeft = vLeft;
            pDAbove = dAbove;
            pDBottom = dBottom;
            pDCenter = dCenter;
            pDLeft = dLeft;
            pDRight = dRight;
        }

        public static LocationProb Multiply(LocationProb p1, LocationProb p2)
        {
            return new LocationProb(
                vAbove: p1.pVAbove * p2.pVAbove,
                vCenter: p1.pVCenter * p2.pVCenter,
                vLeft: p1.pVLeft * p2.pVLeft,
                dAbove: p1.pDAbove * p2.pDAbove,
                dBottom: p1.pDBottom * p2.pDBottom,
                dCenter: p1.pDCenter * p2.pDCenter,
                dLeft: p1.pDLeft * p2.pDLeft,
                dRight: p1.pDRight * p2.pDRight);

        }

        public VowelLocation GetVowelLocation()
        {

            VowelLocation VowelLocation = VowelLocation.Top;

            double sum = pVAbove + pVCenter + pVLeft;
            double pVAbove2 = pVAbove / sum;
            double pVCenter2 = pVCenter / sum + pVAbove;
            double pVLeft2 = pVLeft / sum + pVCenter;

            double r = rnd.NextDouble();

            //bah on this
            if (pVAbove == 0) pVAbove2 = 0;
            if (pVCenter == 0) pVCenter2 = 0;
            if (pVLeft == 0) pVLeft2 = 0;
            //hate these big if trees even more
            #region Place Vowel
            if (r < pVAbove2)
            {
                VowelLocation = VowelLocation.Top;

                pDAbove = 0;
            }
            else
            {
                if (r < pVCenter2)
                {
                    VowelLocation = VowelLocation.Center;
                    pDCenter = 0;
                }
                else
                {
                    VowelLocation = VowelLocation.Left;
                    pDLeft = 0;
                }
            }
            #endregion

            return VowelLocation;
        }

        static Random rnd = new Random();

        public DecorationLocation GetDecLocation()
        {
            DecorationLocation decorationLocation = DecorationLocation.Top;


            double[] nProbs = new double[5];
            nProbs[0] = pDAbove;
            nProbs[1] = pDBottom;
            nProbs[2] = pDCenter;
            nProbs[3] = pDLeft;
            nProbs[4] = pDRight;

            double sum = 0;
            for (int i = 0; i < nProbs.Length; i++)
            {
                sum += nProbs[i];
            }

            for (int i = 0; i < nProbs.Length; i++)
            {
                nProbs[i] /= sum;
            }

            sum = 0;
            for (int i = 0; i < nProbs.Length; i++)
            {
                double t = nProbs[i] + sum;
                sum += nProbs[i];
                if (nProbs[i] != 0)
                    nProbs[i] = t;
            }
            double r = rnd.NextDouble();
            //hate these big if trees even more
            #region Place Decoration
            //probably better iwth a for loop here, but legacy slightly updated
            if (r < nProbs[0])
            {
                decorationLocation = DecorationLocation.Top;

            }
            else
            {
                if (r < nProbs[1])
                {
                    decorationLocation = DecorationLocation.Bottom;
                }
                else
                {
                    if (r < nProbs[2])
                    {
                        decorationLocation = DecorationLocation.Center;
                    }
                    else
                    {
                        if (r < nProbs[3])
                        {
                            decorationLocation = DecorationLocation.Left;
                        }
                        else
                        {
                            decorationLocation = DecorationLocation.Right;
                        }
                    }
                }
            }
            #endregion

            return decorationLocation;
        }

    }
}
