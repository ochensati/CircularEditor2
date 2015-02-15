using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circular.Words;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;

namespace Circular.Sentence
{

    [Serializable]
    public class Sentence : aCircleObject
    {
        PredefinedArrangment _SentenceArrangement;

        public PredefinedArrangment SentenceArrangement
        {
            get
            { return _SentenceArrangement; }
            set
            {
                _SentenceArrangement = value;

                //    if (SubCircles!=null)
                ArrangeSentence();
            }
        }

        public void ArrangeSentence()
        {

            switch (_SentenceArrangement)
            {
                case PredefinedArrangment.CenterLine:
                    ArrangeInMiddleLine();
                    break;
                case PredefinedArrangment.TopLine:
                    ArrangeInLine();
                    break;
                case PredefinedArrangment.ZagZig:
                case PredefinedArrangment.Zigzag:
                    ArrangeInZigzag(_SentenceArrangement);
                    break;

                case PredefinedArrangment.Spots:
                    ArrangeInSpots();
                    break;

                case PredefinedArrangment.Loop:
                    ArrangeInLoop();
                    break;

                case PredefinedArrangment.Circle:
                    ArrangeInCircle();
                    break;

                case PredefinedArrangment.Tight:
                    ArrangeInTight();
                    break;

                case PredefinedArrangment.TightCircle:
                    ArrangeInTightCircle();
                    break;

            }

            word_RedrawRequest();
        }

        public string OrignalSentence { get; protected set; }


        string borderWords = null;
        protected override void CalculateCircle(bool fixConnections)
        {

            if (OrignalSentence == null)
                return;

            string temp = OrignalSentence.Trim();

           
            int idx = temp.IndexOf("[");
            if (idx > -1)
            {
                int idx2 = temp.IndexOf("]", idx);
                borderWords = temp.Substring(idx + 1, idx2 - idx - 1).Trim();
                temp = temp.Remove(idx, idx2 - idx + 1).Trim();
            }


            string[] words = temp.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            int letterwidth = 20;

            if (ScriptStyle == Circular.aCircleObject.ScriptStyles.Small)
                letterwidth = 5;

            SubCircles = new List<aCircleObject>();

            for (int wordI = words.Length - 1; wordI >= 0; wordI--)
            {
                string w = words[wordI].Trim();

                if (w.Contains(","))
                {


                }

                if (WordDictionary.PreferedDictionary.Contains(w.ToLower()))
                {
                    WordCircle v = null;

                    try
                    {
                        FileStream stream = File.OpenRead(@".\words\_Prefered_" + w + ".xml");
                        var formatter = new BinaryFormatter();
                        v = (WordCircle)formatter.Deserialize(stream);
                        stream.Close();
                    }
                    catch
                    {

                    }

                    if (v == null || v.PreferedAngles().Count < 1)
                    {
                        var word = new engWord(words[wordI].Trim(), ScriptStyle);
                        var g = new WordCircle();
                        g.RedrawRequest += new RedrawRequestEvent(word_RedrawRequest);

                        if (SubCircles.Count == 0)
                        {
                            g.Initialize(this, ScriptStyle, 1, letterwidth, false, null);
                            g.InitializeWord(word);
                        }
                        else
                        {
                            var c = new List<aCircleObject>();
                            c.Add((WordCircle)SubCircles[SubCircles.Count - 1]);
                            g.Initialize(this, ScriptStyle, 1, letterwidth, false, c);
                            g.InitializeWord(word);
                        }

                        if (ScriptStyle != Circular.aCircleObject.ScriptStyles.Ashcroft)
                            g.AllFancy = false;

                        SubCircles.Add(g);
                    }
                    else
                    {
                        SubCircles.Add(v);
                        v.RedrawRequest += new RedrawRequestEvent(word_RedrawRequest);
                    }
                }
                else
                {
                    var word = new engWord(words[wordI].Trim(), ScriptStyle);
                    var g = new WordCircle();
                    g.RedrawRequest += new RedrawRequestEvent(word_RedrawRequest);

                    if (SubCircles.Count == 0)
                    {
                        g.Initialize(this, ScriptStyle, 1, letterwidth, false, null);
                        g.InitializeWord(word);
                    }
                    else
                    {
                        var c = new List<aCircleObject>();
                        c.Add((WordCircle)SubCircles[SubCircles.Count - 1]);
                        g.Initialize(this, ScriptStyle, 1, letterwidth, false, c);
                        g.InitializeWord(word);
                    }

                    if (ScriptStyle != Circular.aCircleObject.ScriptStyles.Ashcroft)
                        g.AllFancy = false;

                    SubCircles.Add(g);
                }
            }

            SubCircles.Reverse();




        }

        void word_RedrawRequest()
        {
            CallRedraw();
        }

        private string PunctuationMark = "";
        private string addPunctuationMark = "";

        public void AddPunctuationMark(string mark)
        {
            addPunctuationMark = mark;
            CallRedraw();
        }

        public void FillCircleBorder(Graphics canvas, int inflate)
        {

            var TBackup = canvas.Transform;

            canvas.TranslateTransform(_DrawCenter.X, _DrawCenter.Y);
            canvas.RotateTransform((float)_CircleAngle);
            canvas.ScaleTransform((float)_Scale, (float)_Scale);

            Rectangle r = CircleBounds;
            r.Inflate(inflate, inflate);
            if (SubArcStart == -1)
                canvas.FillEllipse(Brushes.White, r);
            else
            {
                double mid = SubArcStart + SubArc / 2;
                double step = SubArc / 4;

                switch (PunctuationMark)
                {  //  string[] splits = new string[] { ".", ":", ";", "(", ")" };
                    case ".":
                        canvas.FillPie(Brushes.White, r, (float)SubArcStart, (float)SubArc);
                        canvas.DrawPie(Pens.Black, r, (float)SubArcStart, (float)SubArc);
                        canvas.DrawPie(Pens.Black, r, (float)(SubArcStart + step), (float)(SubArc - 2 * step));
                        canvas.DrawLine(Pens.Black, new Point(0, 0), MathHelps.D2Coords(new Point(0, 0), this.Radius, mid));
                        break;
                    case "?":
                        canvas.FillPie(Brushes.Black, r, (float)SubArcStart, (float)SubArc);
                        canvas.DrawPie(Pens.White, r, (float)SubArcStart, (float)SubArc);
                        canvas.DrawPie(Pens.White, r, (float)(SubArcStart + step), (float)(SubArc - 2 * step));
                        canvas.DrawLine(Pens.White, new Point(0, 0), MathHelps.D2Coords(new Point(0, 0), this.Radius, mid));
                        break;
                    case "!":
                        canvas.FillPie(Brushes.Black, r, (float)SubArcStart, (float)SubArc);
                        canvas.DrawLine(new Pen(Color.White, 4), new Point(0, 0), MathHelps.D2Coords(new Point(0, 0), this.Radius, mid));
                        break;
                    case ";":
                    case ")":
                    case "-":
                        canvas.FillPie(Brushes.White, r, (float)SubArcStart, (float)SubArc);
                        canvas.DrawPie(Pens.Black, r, (float)SubArcStart, (float)SubArc);
                        break;
                    case ":":
                        canvas.FillPie(Brushes.White, r, (float)SubArcStart, (float)SubArc);
                        canvas.DrawPie(Pens.Black, r, (float)SubArcStart, (float)SubArc);
                        canvas.DrawLine(Pens.Black, new Point(0, 0), MathHelps.D2Coords(new Point(0, 0), this.Radius, mid));
                        break;
                    case "(":

                        canvas.FillPie(Brushes.White, r, (float)SubArcStart, (float)SubArc);
                        canvas.DrawPie(Pens.Black, r, (float)SubArcStart, (float)SubArc);
                        canvas.DrawArc(new Pen(Color.Black, 3), r, (float)SubArcStart, (float)SubArc);
                        break;
                    case "\"":
                        r.Inflate(4, 4);
                        canvas.DrawEllipse(new Pen(Color.Black, 3), r);
                        break;
                }

                if (addPunctuationMark != "")
                {
                    mid = SubArcStart + SubArc / 2 + 180;
                    step = SubArc / 4;
                    switch (addPunctuationMark)
                    {  //  string[] splits = new string[] { ".", ":", ";", "(", ")" };
                        case ".":
                            canvas.FillPie(Brushes.White, r, (float)SubArcStart + 180, (float)SubArc);
                            canvas.DrawPie(Pens.Black, r, (float)SubArcStart + 180, (float)SubArc);
                            canvas.DrawPie(Pens.Black, r, (float)(SubArcStart + step) + 180, (float)(SubArc - 2 * step));
                            canvas.DrawLine(Pens.Black, new Point(0, 0), MathHelps.D2Coords(new Point(0, 0), this.Radius, mid));
                            break;
                        case "?":
                            canvas.FillPie(Brushes.Black, r, (float)SubArcStart + 180, (float)SubArc);
                            canvas.DrawPie(Pens.White, r, (float)SubArcStart + 180, (float)SubArc);
                            canvas.DrawPie(Pens.White, r, (float)(SubArcStart + step + 180), (float)(SubArc - 2 * step));
                            canvas.DrawLine(Pens.White, new Point(0, 0), MathHelps.D2Coords(new Point(0, 0), this.Radius, mid));
                            break;
                        case "!":
                            canvas.FillPie(Brushes.Black, r, (float)SubArcStart + 180, (float)SubArc);
                            canvas.DrawLine(new Pen(Color.White, 4), new Point(0, 0), MathHelps.D2Coords(new Point(0, 0), this.Radius, mid));
                            break;
                        case ";":
                        case ")":
                            canvas.FillPie(Brushes.White, r, (float)SubArcStart + 180, (float)SubArc);
                            canvas.DrawPie(Pens.Black, r, (float)SubArcStart + 180, (float)SubArc);
                            break;
                        case ":":
                            canvas.FillPie(Brushes.White, r, (float)SubArcStart + 180, (float)SubArc);
                            canvas.DrawPie(Pens.Black, r, (float)SubArcStart + 180, (float)SubArc);
                            canvas.DrawLine(Pens.Black, new Point(0, 0), MathHelps.D2Coords(new Point(0, 0), this.Radius, mid));
                            break;
                        case "(":

                            canvas.FillPie(Brushes.White, r, (float)SubArcStart + 180, (float)SubArc);
                            canvas.DrawPie(Pens.Black, r, (float)SubArcStart + 180, (float)SubArc);
                            canvas.DrawArc(new Pen(Color.Black, 3), r, (float)SubArcStart, (float)SubArc);
                            break;
                    }


                }
            }



            canvas.Transform = TBackup;
        }

        public void InitializeSentence(string sentence, PredefinedArrangment startSentenceArrangment, string punctuationMark)
        {


            this.PunctuationMark = punctuationMark;

            OrignalSentence = sentence;

            CalculateCircle(true);

            SentenceArrangement = startSentenceArrangment;

            if (borderWords != null)
                CalculateBorderWords(borderWords, 3);
        }

        //void word_RedrawRequest()
        //{

        //    //if (RedrawRequest != null && BlockRedraw == false)
        //    //    RedrawRequest();
        //}

        public override List<aCircleObject.AngleMark> PreferedAngles()
        {
            List<AngleMark> a = new List<AngleMark>();
            a.Add(new aCircleObject.AngleMark(0, 1, null));
            return a;
        }

        protected override void RedrawImpl()
        {
            // throw new NotImplementedException();
        }

        protected override void DrawCircle(Graphics canvas, bool mockup)
        {

            foreach (var w in SubCircles)
            {
                if (w.DrawBorder)
                {
                    canvas.DrawEllipse(Pens.Black, MathHelps.Circle2Rect(w.DrawCenter, w.OuterBounds.Width / 2 + 2));
                }
            }


            foreach (var w in SubCircles)
            {
                canvas.FillEllipse(Brushes.White, MathHelps.Circle2Rect(w.DrawCenter, w.OuterBounds.Width / 2));
            }

            var w2 = SubCircles[0];
            canvas.DrawArc(new Pen(Color.Black, 5), MathHelps.Circle2Rect(w2.DrawCenter, w2.OuterBounds.Width / 2 + 2), (float)w2.CircleAngle, 180);
            w2 = SubCircles[SubCircles.Count - 1];
            canvas.DrawArc(new Pen(Color.Black, 5), MathHelps.Circle2Rect(w2.DrawCenter, w2.OuterBounds.Width / 2 + 2), (float)w2.CircleAngle + 180, 180);


            foreach (var w in SubCircles)
            {
                w.Draw(canvas, false);
            }

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
    }
}
