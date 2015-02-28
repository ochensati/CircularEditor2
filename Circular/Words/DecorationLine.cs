using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Circular.Decorations;
using EpPathFinding.cs;
using System.ComponentModel;

namespace Circular.Words
{
    [Serializable]
    public class DecorationLine
    {
        private static Random rnd = new Random();

        private VectorGraphics.Line.LineTypes _LineType;

        [BrowsableAttribute(false)]
        public string[] AllSources { get { return _Word.Sources.Keys.ToArray(); } }

        [BrowsableAttribute(false)]
        public string[] AllAnchors { get { return _Word.Anchors.Keys.ToArray(); } }

        [CategoryAttribute("Appearance")]
        public VectorGraphics.Line.LineTypes LineType
        {
            get
            {
                return _LineType;
            }
            set
            {
                _LineType = value;
                TheDecoration = new VectorGraphics.Line(_ControlPoints.ToArray(), _LineType, new Pen(_LineColor, _LineWidth));

                Point[] offsetP = new Point[_ControlPoints.Count];
                for (int i = 0; i < _ControlPoints.Count; i++)
                    offsetP[i] = new Point(_ControlPoints[i].X + 2, _ControlPoints[i].Y + 2);
                TheDecoration2 = new VectorGraphics.Line(offsetP, this._LineType, new Pen(Color.White, this._LineWidth ));
                _Word.Redraw();
            }
        }

        private float _LineWidth;
        [CategoryAttribute("Appearance")]
        public float LineWidth
        {
            get
            {
                return _LineWidth;
            }
            set
            {
                _LineWidth = value;
                TheDecoration = new VectorGraphics.Line(_ControlPoints.ToArray(), _LineType, new Pen(_LineColor, _LineWidth));
                Point[] offsetP = new Point[_ControlPoints.Count];
                for (int i = 0; i < _ControlPoints.Count; i++)
                    offsetP[i] = new Point(_ControlPoints[i].X + 2, _ControlPoints[i].Y + 2);
                TheDecoration2 = new VectorGraphics.Line(offsetP, this._LineType, new Pen(Color.White, this._LineWidth ));
                _Word.Redraw();
            }
        }

        private Color _LineColor;
        [CategoryAttribute("Appearance")]
        public Color LineColor
        {
            get
            {
                return _LineColor;
            }
            set
            {
                _LineColor = value;
                TheDecoration = new VectorGraphics.Line(_ControlPoints.ToArray(), _LineType, new Pen(_LineColor, _LineWidth));
                Point[] offsetP = new Point[_ControlPoints.Count];
                for (int i = 0; i < _ControlPoints.Count; i++)
                    offsetP[i] = new Point(_ControlPoints[i].X + 2, _ControlPoints[i].Y + 2);
                TheDecoration2 = new VectorGraphics.Line(offsetP, this._LineType, new Pen(Color.White, this._LineWidth ));
                _Word.Redraw();
            }
        }

        private List<Point> _ControlPoints;
        [CategoryAttribute("Read-Only")]
        public List<Point> ControlPoints
        {
            get
            {
                return _ControlPoints;
            }
            set
            {
                _ControlPoints = value;
                TheDecoration = new VectorGraphics.Line(_ControlPoints.ToArray(), _LineType, new Pen(_LineColor, _LineWidth));
                Point[] offsetP = new Point[_ControlPoints.Count];
                for (int i = 0; i < _ControlPoints.Count; i++)
                    offsetP[i] = new Point(_ControlPoints[i].X + 2, _ControlPoints[i].Y + 2);
                TheDecoration2 = new VectorGraphics.Line(offsetP, this._LineType, new Pen(Color.White, this._LineWidth ));
                _Word.Redraw();
            }
        }


        private aCircleObject _Word;

        private string _SourceName;
        private string _anchorName;

        [CategoryAttribute("Appearance")]
        [BrowsableAttribute(true)]
        [EditorAttribute(typeof(MyEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string SourceName
        {
            get
            {
                return _SourceName;
            }
            set
            {
                _SourceName = value;
                _Word.Redraw();
            }
        }

        [CategoryAttribute("Appearance")]
        [BrowsableAttribute(true)]
        [EditorAttribute(typeof(MyEditor2), typeof(System.Drawing.Design.UITypeEditor))]
        public string AnchorName
        {
            get
            {
                return _anchorName;
            }
            set
            {
                _anchorName = value;
                _Word.Redraw();

            }
        }

        VectorGraphics.Line TheDecoration;
        VectorGraphics.Line TheDecoration2;

        public DecorationLine() { }
        public DecorationLine(aCircleObject word, VectorGraphics.Line.LineTypes lineType, string SourceName, Color foregroundColor)
        {

            _Word = word;
            this._LineColor = foregroundColor;

            this._LineWidth = _Word.Sources[SourceName].LineThickness;

            _SourceName = SourceName;

            this._LineType = lineType;
        }

        public DecorationLine(aCircleObject word, VectorGraphics.Line.LineTypes lineType, string SourceName, string AnchorName, Color foregroundColor)
        {

            this._LineColor = foregroundColor;
            _Word = word;
            _anchorName = AnchorName;
            _SourceName = SourceName;
            this._LineWidth = _Word.Sources[SourceName].LineThickness; ;
            this._LineType = lineType;
        }

        public DecorationLine(aCircleObject word, VectorGraphics.Line.LineTypes lineType, Point[] controlPoints, string SourceName, string AnchorName, Color foregroundColor)
        {

            this._LineColor = foregroundColor;
            _Word = word;
            _anchorName = AnchorName;
            _SourceName = SourceName;
            this._LineWidth = _Word.Sources[SourceName].LineThickness;
            this._LineType = lineType;
            this._ControlPoints = new List<Point>(controlPoints);
        }

        public void SetControlPoints(Point[] ControlPoints)
        {
            this._ControlPoints = new List<Point>(ControlPoints);

            Point[] offset = new Point[_ControlPoints.Count];
            for (int i = 0; i < _ControlPoints.Count; i++)
                offset[i] = new Point(_ControlPoints[i].X + 2, _ControlPoints[i].Y + 2);


            TheDecoration = new VectorGraphics.Line(ControlPoints.ToArray(), this._LineType, new Pen(_LineColor, this._LineWidth));
            TheDecoration2 = new VectorGraphics.Line(offset, this._LineType, new Pen(Color.White, this._LineWidth));
        }

        public void SetControlPoints(List<Point> ControlPoints)
        {
            this._ControlPoints = ControlPoints;
            TheDecoration = new VectorGraphics.Line(ControlPoints.ToArray(), this._LineType, new Pen(_LineColor, this._LineWidth));
            Point[] offset = new Point[_ControlPoints.Count];
            for (int i = 0; i < _ControlPoints.Count; i++)
                offset[i] = new Point(_ControlPoints[i].X + 2, _ControlPoints[i].Y + 2);
            TheDecoration2 = new VectorGraphics.Line(offset, this._LineType, new Pen(Color.White, this._LineWidth ));
        }

        public void SetControlPoints(double convertToSearch, JumpPointParam jpParam, double offsetX, double offsetY)
        {
            GridPos startPos;
            GridPos endPos;
            List<GridPos> resultPathList;
            DecorationAnchor s = _Word.Sources[_SourceName];
            int ccTries = 0;
            DecorationAnchor anchor = null;
            do
            {

                int r;
                if (this._anchorName == null)
                {
                    anchor = ChoseAnchor(_Word.Anchors, s);
                }
                else
                {
                    if (AnchorName == null)
                    {
                        _ControlPoints.AddRange(s.ControlPoints);

                        //if (controlPoints.Count < 2)
                        //    controlPoints.Add(this.WordCenter);

                        TheDecoration = new VectorGraphics.Line(_ControlPoints.ToArray(), _LineType, new Pen(_LineColor, this._LineWidth));

                        Point[] offset = new Point[_ControlPoints.Count];
                        for (int i = 0; i < _ControlPoints.Count; i++)
                            offset[i] = new Point(_ControlPoints[i].X + 2, _ControlPoints[i].Y + 2);
                        TheDecoration2 = new VectorGraphics.Line(offset, this._LineType, new Pen(Color.White, this._LineWidth ));

                        return;
                    }
                    else
                        anchor = _Word.Anchors[AnchorName];

                }

                _ControlPoints = new List<Point>();

                _ControlPoints.AddRange(s.ControlPoints);
                _ControlPoints.AddRange(anchor.ControlPoints.Reverse());

                if (_ControlPoints.Count == 4)
                {
                    startPos = new GridPos((int)((_ControlPoints[1].X + offsetX) * convertToSearch), (int)((_ControlPoints[1].Y+ offsetY) * convertToSearch  ));
                    endPos = new GridPos((int)((_ControlPoints[_ControlPoints.Count - 2].X + offsetX) * convertToSearch), (int)((_ControlPoints[_ControlPoints.Count - 2].Y + offsetY)* convertToSearch  ));
                }
                else
                {
                    startPos = new GridPos((int)((_ControlPoints[1].X + offsetX)* convertToSearch  ), (int)((_ControlPoints[1].Y+ offsetY) * convertToSearch));
                    endPos = new GridPos((int)((_ControlPoints[_ControlPoints.Count - 1].X + offsetX)* convertToSearch ), (int)((_ControlPoints[_ControlPoints.Count - 1].Y+ offsetY) * convertToSearch  ));
                }

                try
                {
                    jpParam.Reset(startPos, endPos);

                    //check to see if the path is legal.  
                    resultPathList = JumpPointFinder.FindPath(jpParam);
                }
                catch
                {
                    resultPathList = null;
                }
                ccTries++;
            } while (resultPathList == null && ccTries < 5);


            _anchorName = anchor.Name;

            if (resultPathList != null)
            {
                List<Point> c2 = new List<Point>();
                c2.Add(_ControlPoints[0]);
                for (int i = 0; i < resultPathList.Count; i += 4)
                {
                    var p = resultPathList[i];
                    c2.Add(new Point((int)(p.x / convertToSearch - offsetX), (int)(p.y / convertToSearch - offsetY)));
                }
                c2.Add(_ControlPoints[_ControlPoints.Count - 1]);

                TheDecoration = new VectorGraphics.Line(c2.ToArray(), _LineType, new Pen(_LineColor, _LineWidth));
                _ControlPoints = c2;
            }
            else
            {
                _ControlPoints = new List<Point>();

                _ControlPoints.AddRange(s.ControlPoints);

                if (_ControlPoints.Count < 2)
                    _ControlPoints.Add(new Point((int)0, (int)0));

                TheDecoration = new VectorGraphics.Line(_ControlPoints.ToArray(), _LineType, new Pen(_LineColor, _LineWidth));
            }

            Point[] offsetP = new Point[_ControlPoints.Count];
            for (int i = 0; i < _ControlPoints.Count; i++)
                offsetP[i] = new Point(_ControlPoints[i].X + 2, _ControlPoints[i].Y + 2);
            TheDecoration2 = new VectorGraphics.Line(offsetP, this._LineType, new Pen(Color.White, this._LineWidth ));
        }

        private DecorationAnchor ChoseAnchor(Dictionary<string, DecorationAnchor> anchors, DecorationAnchor source)
        {
            double[] probs = new double[anchors.Count];
            double sProbs = 0;
            int i = 0;
            foreach (var kvp in anchors)
            {
                probs[i] = kvp.Value.Goodness;
                sProbs += probs[i];
                i++;
            }

            i = 0;
            foreach (var kvp in anchors)
            {
                if (i != 0)
                    probs[i] = probs[i] / sProbs + probs[i - 1];
                else
                    probs[i] = probs[i] / sProbs;

                i++;
            }
            sProbs = rnd.NextDouble();
            i = 0;
            foreach (var kvp in anchors)
            {
                if (sProbs < probs[i] && source.Owner != kvp.Value.Owner)
                    return kvp.Value;

                i++;
            }

            sProbs = Math.Floor(rnd.NextDouble() * probs.Length);

            i = 0;
            foreach (var kvp in anchors)
            {
                if (sProbs == i)
                    return kvp.Value;

                i++;
            }

            return null;
        }

        public void Draw(ref Graphics canvas)
        {
            if (TheDecoration2 != null)
                TheDecoration2.Draw(ref canvas);

            TheDecoration.Draw(ref canvas);


        }

        public override string ToString()
        {
            return SourceName + "  --->  " + AnchorName;
        }
    }
}
