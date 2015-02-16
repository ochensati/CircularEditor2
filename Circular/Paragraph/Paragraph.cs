using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Circular.Words;
using System.Drawing;

namespace Circular.Paragraph
{
    [Serializable]
    public class Paragraph : aCircleObject
    {


       

        public PredefinedArrangment SentenceArrangement
        {
            get
            {
                return _SentenceArrangement;
            }
            set
            {
                _SentenceArrangement = value;

                if (_SentenceArrangement == PredefinedArrangment.Circle || _SentenceArrangement == PredefinedArrangment.Tight || _SentenceArrangement == PredefinedArrangment.TightCircle)
                    DrawBorder = true;
                else
                    DrawBorder = false;

                if (SubCircles != null)
                {
                    foreach (var s in SubCircles)
                    {
                        if (s.GetType() == typeof(Sentence.Sentence))
                            ((Sentence.Sentence)s).SentenceArrangement = value;
                    }
                }
            }
        }



        public string OrignalText { get; protected set; }

        public void InitializeParagraph(string text)
        {
            OrignalText = text;

            SentenceArrangement = PredefinedArrangment.TightCircle;

            CalculateCircle(true);
        }


        protected override void CalculateCircle(bool fixConnections)
        {
            if (OrignalText == null)
                return;

            string temp = OrignalText.Trim();

            string borderWords=null;
            int idx = temp.IndexOf("{");
            if (idx > -1)
            {
                int idx2 = temp.IndexOf("}", idx);
                borderWords = temp.Substring(idx+1, idx2 - idx-1).Trim();
                temp = temp.Remove(idx, idx2 - idx+1).Trim();
            }

           
            string[] splits = new string[] { ".", ":", ";", "!", "?", "(", ")", "\"", "-", "\n" };


            for (int i = 0; i < splits.Length; i++)
            {
                temp = temp.Replace(splits[i], splits[i] + "" + i + "^^");
            }

            string[] sentences = temp.Split(splits, StringSplitOptions.RemoveEmptyEntries);

            SubCircles = new List<aCircleObject>();

            int cc = 0;

            foreach (var s in sentences)
            {
                string sentence = s;

                string punctuation = "";
                if (sentence.Contains("^^"))
                {
                    string[] parts = sentence.Split(new string[] { "^^" }, StringSplitOptions.None);
                    punctuation = splits[int.Parse(parts[0])];
                    sentence = parts[1];

                    if (sentence == "" && SubCircles.Count > 0)
                    {
                        ((Sentence.Sentence)SubCircles[SubCircles.Count - 1]).AddPunctuationMark(punctuation);
                    }
                }
                sentence = sentence.Trim();
                if (sentence != "")
                {
                    var s2 = new Sentence.Sentence();
                    s2.Preview = Preview;
                    s2.Initialize(this, ScriptStyle, this.Scale, 30, false, null);
                    s2.InitializeSentence(sentence, PredefinedArrangment.TightCircle, punctuation);
                    s2.RedrawRequest += new RedrawRequestEvent(word_RedrawRequest);
                    SubCircles.Add(s2);
                }
            }

            if (_SentenceArrangement == PredefinedArrangment.TightCircle)
                ArrangeInTightCircle(30);
            else
            {
                SentenceArrangement = _SentenceArrangement;

            }

            for (int i = 0; i < SubCircles.Count; i++)
            {
                List<Sentence.Shaker.Dot> Dots = new List<Sentence.Shaker.Dot>();
                for (int j = i + 1; j < SubCircles.Count; j++)
                {
                    double r = MathHelps.distance(SubCircles[i].DrawCenter, SubCircles[j].DrawCenter);
                    double theta = MathHelps.Atan2(SubCircles[i].DrawCenter, SubCircles[j].DrawCenter) - SubCircles[i].CircleAngle;

                    Dots.Add(new Sentence.Shaker.Dot(SubCircles[j].Radius + 25, MathHelps.D2Coords(new Point(0, 0), r, theta), 0, true));
                }

                if (Dots.Count > 0)
                {
                    Point p = SubCircles[i].DrawCenter;
                    // SubCircles[i].ArrangeInTightCircle(Dots.ToArray());

                    SubCircles[i + 1].UseWordForArc(SubCircles[i]);
                    SubCircles[i].DrawCenter = p;
                }
            }

            Rectangle b = this.GetSize();

            this._DrawCenter = new Point(-1 * b.X + 40, -1 * b.Y + 40);

            if (borderWords != null)
                CalculateBorderWords(borderWords, 3);

        }

        void word_RedrawRequest()
        {
            CallRedraw();
        }

        public override List<aCircleObject.AngleMark> PreferedAngles()
        {
            throw new NotImplementedException();
        }

        protected override void RedrawImpl()
        {
            // throw new NotImplementedException();
        }

        public override iMouseable HitTest(Point p)
        {
            double r = MathHelps.distance(this._DrawCenter, p) / Scale;
            if (r < this.Radius)
            {

                //transform coordinates
                Point p2 = new Point((int)((p.X - _DrawCenter.X) / Scale), (int)((p.Y - _DrawCenter.Y) / Scale));


                double r2 = MathHelps.distance(new Point((int)0, (int)0), p2);
                double angle = MathHelps.Atan2(p2.Y, p2.X);
                angle -= CircleAngle;
                Point p3 = MathHelps.D2Coords(new Point((int)0, (int)0), r2, angle);
                foreach (var s in SubCircles)
                {
                    var o = s.HitTest(p3);
                    if (o != null)
                        return o;
                }
                return this;
            }
            else
            {

                return null;
            }
        }

        protected override void DrawCircle(System.Drawing.Graphics canvas, bool mockup)
        {
            for (int i = 0; i < SubCircles.Count - 1; i++)
            {
                var w = SubCircles[i];
                w.DrawCircleBorder(canvas, false);

                ((Sentence.Sentence)SubCircles[i + 1]).FillCircleBorder(canvas, 10);

            }

            SubCircles[SubCircles.Count - 1].DrawCircleBorder(canvas, false);

            foreach (var w in SubCircles)
            {
                //canvas.FillEllipse(Brushes.White, MathHelps.Circle2Rect(w.DrawCenter, w.WordBounds.Width / 2-3));
            }


            foreach (var w in SubCircles)
            {
                w.Draw(canvas, false);
            }

            var w2 = SubCircles[0];
            canvas.DrawArc(new Pen(Color.Black, 5), MathHelps.Circle2Rect(w2.DrawCenter, w2.OuterBounds.Width / 2 + 2), (float)w2.CircleAngle + 90, 180);
        }
    }
}
