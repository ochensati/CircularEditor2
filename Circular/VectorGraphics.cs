using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing.Imaging;

namespace Circular.VectorGraphics
{
    [Serializable]
    public class Line
    {
        public Point[] Points { get; set; }
        public Color LineColor { get; set; }
        public LineTypes LineType { get; set; }
        public int LineWidth { get; set; }
        public enum LineTypes
        {
            Beizer, Line, Unfinished, Cubic
        }

        public void Draw(ref Graphics canvas)
        {
            Pen pen = new Pen(LineColor, LineWidth);
           
            switch (LineType)
            {
                case LineTypes.Line:
                    {
                        canvas.DrawLine(pen, Points[0], Points[Points.Length -1]);
                        break;
                    }
                case LineTypes.Beizer:
                    {
                        canvas.DrawBeziers(pen, Points);
                        break;
                    }
                case LineTypes.Cubic:
                    {

                        canvas.DrawCurve(pen, Points);
                        break;
                    }
                case LineTypes.Unfinished:
                    {
                        if (Points.Length > 1)
                            canvas.DrawLine(pen, Points[0], Points[1]);
                        break;
                    }
            }
        }

        public Line(Point[] Points, LineTypes lineType, Pen pen)
        {
            this.Points = Points;
            this.LineType = lineType;
            this.LineColor = pen.Color;
            this.LineWidth =(int) pen.Width;
        }

        public Line(Point[] Points, LineTypes lineType, float  penWidth)
        {
            this.Points = Points;
            this.LineType = lineType;
            LineColor = Color.Black;
            LineWidth=(int) penWidth;
        }
    }

    public static class GraphicsHelps
    {

        [Flags]
        private enum EmfToWmfBitsFlags
        {
            EmfToWmfBitsFlagsDefault = 0x00000000,
            EmfToWmfBitsFlagsEmbedEmf = 0x00000001,
            EmfToWmfBitsFlagsIncludePlaceable = 0x00000002,
            EmfToWmfBitsFlagsNoXORClip = 0x00000004
        }

        private static int MM_ISOTROPIC = 7;
        private static int MM_ANISOTROPIC = 8;

        [DllImport("gdiplus.dll")]
        private static extern uint GdipEmfToWmfBits(IntPtr _hEmf, uint _bufferSize,
            byte[] _buffer, int _mappingMode, EmfToWmfBitsFlags _flags);
        [DllImport("gdi32.dll")]
        private static extern IntPtr SetMetaFileBitsEx(uint _bufferSize,
            byte[] _buffer);
        [DllImport("gdi32.dll")]
        private static extern IntPtr CopyMetaFile(IntPtr hWmf,
            string filename);
        [DllImport("gdi32.dll")]
        private static extern bool DeleteMetaFile(IntPtr hWmf);
        [DllImport("gdi32.dll")]
        private static extern bool DeleteEnhMetaFile(IntPtr hEmf);

        public static MemoryStream MakeMetafileStream(System.Drawing.Bitmap image)
        {
            Metafile metafile = null;
            using (Graphics g = Graphics.FromImage(image))
            {
                IntPtr hDC = g.GetHdc();
                metafile = new Metafile(hDC, EmfType.EmfOnly);
                g.ReleaseHdc(hDC);
            }

            using (Graphics g = Graphics.FromImage(metafile))
            {
                g.DrawImage(image, 0, 0);
            }
            IntPtr _hEmf = metafile.GetHenhmetafile();
            uint _bufferSize = GdipEmfToWmfBits(_hEmf, 0, null, MM_ANISOTROPIC,
                EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);
            byte[] _buffer = new byte[_bufferSize];
            GdipEmfToWmfBits(_hEmf, _bufferSize, _buffer, MM_ANISOTROPIC,
                    EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);
            DeleteEnhMetaFile(_hEmf);

            var stream = new MemoryStream();
            stream.Write(_buffer, 0, (int)_bufferSize);
            stream.Seek(0, 0);

            return stream;
        }

    }
}
