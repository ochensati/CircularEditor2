using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Circular.Words;
using PropertyGridEx;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using Circular.VectorGraphics;

namespace Circular
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();


            pbCanvas.Image = new Bitmap(pbCanvas.Width, pbCanvas.Height);
            if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Circular") == false)
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Circular");
            }
            WordDictionary.Initialize();

        }


        aCircleObject root;
        aCircleObject SelectedCircle;

        bool noDrawing = false;
        #region Drawing
        private void bDraw_Click(object sender, EventArgs e)
        {
            string text = tblText.Text;
            root = null;
            SelectedCircle = null;

            Circular.aCircleObject.ScriptStyles scriptStyle = Circular.aCircleObject.ScriptStyles.Ashcroft;
            if (rbAshcroft.Checked == false)
                scriptStyle = Circular.aCircleObject.ScriptStyles.Sherman;

            SelectedCircle = new Paragraph.Paragraph();
            SelectedCircle.Preview = pbCanvas;
            noDrawing = true;
            SelectedCircle.RedrawRequest += new RedrawRequestEvent(Sentence_RedrawRequest);
            ((Paragraph.Paragraph)SelectedCircle).Initialize(null, scriptStyle, 1, 20, false, null);
            ((Paragraph.Paragraph)SelectedCircle).InitializeParagraph(text);
        

            Rectangle r = SelectedCircle.GetSize();
            noDrawing = false;

            pbCanvas.Image = new Bitmap(r.Width * 3, 3 * r.Height, PixelFormat.Format32bppRgb);

            pbCanvas.Width = (int)(r.Width * 3);
            pbCanvas.Height = (int)(r.Height * 3);

            root = SelectedCircle;


            pbCanvas.Invalidate();
            pbCanvas.Refresh();

            propertyGridEx1.SelectedObject = root;
            propertyGridEx1.Refresh();
        }

        void Sentence_RedrawRequest()
        {

            pbCanvas.Invalidate();
            pbCanvas.Refresh();
        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            if (root != null)
            {
                try
                {
                    if (noDrawing == false)
                    {
                        e.Graphics.FillRectangle(Brushes.White, pbCanvas.Bounds);

                        if (BackgroundImage != null)
                            e.Graphics.DrawImageUnscaledAndClipped(BackgroundImage, pbCanvas.Bounds);

                        root.Draw(e.Graphics, false);
                        e.Graphics.Flush();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);

                }
            }
        }
        #endregion

        public Type GetFirstAbstractBaseType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            Type baseType = type.BaseType;
            if (baseType == null || baseType.IsAbstract)
            {
                return baseType;
            }
            return GetFirstAbstractBaseType(baseType);
        }


        #region MouseHandling
        Point _startPoint;
        Point _startRelativeCenter;
        Point _startActualCenter;

        double _startAngle;
        iMouseable hit;
        private void pbCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (root == null)
                return;

            hit = root.HitTest(new Point(e.X, e.Y));

            radomizeLinesMI.Visible = false;
            saveSentenceMI.Visible = false;
            saveWordMI.Visible = false;
            alignArcToolStripMenuItem.Visible = false;

            if (hit != null)
            {
                propertyGridEx1.SelectedObject = hit;

                if (hit.GetType() == typeof(Sentence.Sentence))
                {
                    radomizeLinesMI.Visible = true;
                    saveSentenceMI.Visible = true;
                    alignArcToolStripMenuItem.Visible = true;
                }

                if (hit.GetType() == typeof(WordCircle))
                {
                    radomizeLinesMI.Visible = true;
                    saveWordMI.Visible = true;
                }

                if (hit.GetType() == typeof(Paragraph.Paragraph))
                {
                    alignArcToolStripMenuItem.Visible = true;
                }

                if (hit != null)
                {
                    try
                    {
                        SelectedCircle = (aCircleObject)hit;
                        if (e.Button == System.Windows.Forms.MouseButtons.Left)
                        {
                            _startPoint = new Point(e.X, e.Y);
                            _startRelativeCenter = hit.DrawCenter;
                        }
                        else
                        {
                            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                            {
                                _startActualCenter = hit.GetStartAngle(new Point(e.X, e.Y), out _startAngle);
                                //_startAngle = hit.CircleAngle - MathHelps.Atan2(, hit.DrawCenter);
                            }
                        }
                    }
                    catch { }
                }
            }
            else
            {
                propertyGridEx1.SelectedObject = root;
            }

        }

        private void pbCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (hit != null && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                hit.MoveDelta(_startRelativeCenter, (e.X - _startPoint.X), (e.Y - _startPoint.Y));

            }

            if (hit != null && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                double angle = MathHelps.Atan2(new Point(e.X, e.Y), _startActualCenter) - _startAngle;
                hit.SetAngleDelta(angle);
                // hit.CircleAngle = MathHelps.Atan2(new Point(e.X, e.Y), hit.DrawCenter) + _startAngle;
            }
        }

        private void pbCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (hit != null && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                hit.MoveDelta(_startRelativeCenter, (e.X - _startPoint.X), (e.Y - _startPoint.Y));

            }
        }
        #endregion

        #region Menu

        private void saveToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Rectangle r = root.GetSize();
            Bitmap b = new Bitmap(r.Width, r.Height);
            Graphics g = Graphics.FromImage(b);
            root.Draw(g, false);

            g.Flush();

            g = null;
            //.Save("c:\\temp\\b3.bmp");

            saveFileDialog1.Filter = "Circular (.cir)|*.cir|Vector Image (.emf)|*.emf|Bitmap Image (.bmp)|*.bmp|Gif Image (.gif)|*.gif|JPEG Image (.jpeg)|*.jpeg|Png Image (.png)|*.png|Tiff Image (.tiff)|*.tiff";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                if (Path.GetExtension(saveFileDialog1.FileName).ToLower() == ".emf")
                {
                    var ms = GraphicsHelps.MakeMetafileStream((Bitmap)b);

                    using (FileStream file = new FileStream(saveFileDialog1.FileName, FileMode.Create, System.IO.FileAccess.Write))
                    {
                        byte[] bytes = new byte[ms.Length];
                        ms.Read(bytes, 0, (int)ms.Length);
                        file.Write(bytes, 0, bytes.Length);
                        ms.Close();
                    }
                }
                else
                {
                    if (Path.GetExtension(saveFileDialog1.FileName).ToLower() == ".cir")
                    {
                        root.ClearEvent();

                        FileStream stream = File.Create(saveFileDialog1.FileName);
                        var formatter = new BinaryFormatter();
                        Console.WriteLine("Serializing vector");
                        formatter.Serialize(stream, root);
                        stream.Close();


                        root.RedrawRequest += new RedrawRequestEvent(Sentence_RedrawRequest);


                    }
                    else
                        ((Bitmap)pbCanvas.Image).Save(saveFileDialog1.FileName);

                }

            }

            //    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(root.GetType());

            // System.IO.StreamWriter file = new System.IO.StreamWriter(
            //@".\words\_Prefered_" + gWords[0].Word.ToString() + ".xml");
            // x.Serialize(file, gWords[0]);
            // file.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "(*.cir) | *.cir";
            openFileDialog1.ShowDialog();

            if (openFileDialog1.FileName != "openFileDialog1")
            {
                // Open the file containing the data that you want to deserialize.
                FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open);
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    // Deserialize the hashtable from the file and  
                    // assign the reference to the local variable.
                    root = (aCircleObject)formatter.Deserialize(fs);

                    root.RedrawRequest += new RedrawRequestEvent(Sentence_RedrawRequest);

                    tblText.Text = ((Paragraph.Paragraph)root).OrignalText;
                }
                catch (SerializationException e2)
                {
                    Console.WriteLine("Failed to deserialize. Reason: " + e2.Message);
                    throw;
                }
                finally
                {
                    fs.Close();
                }
            }
            pbCanvas.Invalidate();
            pbCanvas.Refresh();
        }

        private void newToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            tblText.Text = "";
            bDraw_Click(this, e);
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            noDrawing = true;

            saveFileDialog1.Filter = "Vector Image (.emf)|*.emf|Bitmap Image (.bmp)|*.bmp|Gif Image (.gif)|*.gif|JPEG Image (.jpeg)|*.jpeg|Png Image (.png)|*.png|Tiff Image (.tiff)|*.tiff";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                if (Path.GetExtension(saveFileDialog1.FileName).ToLower() == ".emf")
                {

                    Rectangle r = root.GetSize();
                    Bitmap b = new Bitmap(r.Width, r.Height);
                    Graphics g = Graphics.FromImage(b);
                    root.Draw(g, false);

                    g.Flush();

                    g = null;
                    var ms = GraphicsHelps.MakeMetafileStream((Bitmap)b);

                    using (FileStream file = new FileStream(saveFileDialog1.FileName, FileMode.Create, System.IO.FileAccess.Write))
                    {
                        byte[] bytes = new byte[ms.Length];
                        ms.Read(bytes, 0, (int)ms.Length);
                        file.Write(bytes, 0, bytes.Length);
                        ms.Close();
                    }
                }
                else
                {
                    Rectangle r = root.GetSize();
                    Bitmap b = new Bitmap(r.Width, r.Height);
                    Graphics g = Graphics.FromImage(b);
                    root.Draw(g, false);

                    g.Flush();

                    g = null;

                    (b).Save(saveFileDialog1.FileName);

                }
            }

            noDrawing = false;
            pbCanvas.Invalidate();
            pbCanvas.Refresh();
        }

        private void randomizeMI_Click(object sender, EventArgs e)
        {
            bDraw_Click(this, e);
        }

        private void radomizeLinesMI_Click(object sender, EventArgs e)
        {

            if (SelectedCircle != null && SelectedCircle.GetType() == typeof(WordCircle))
            {
                ((WordCircle)SelectedCircle).CalculateDecorations(true);
            }
            Sentence_RedrawRequest();
        }

        Image background;
        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Vector Image (.emf)|*.emf|Bitmap Image (.bmp)|*.bmp|Gif Image (.gif)|*.gif|JPEG Image (.jpeg)|*.jpeg|Png Image (.png)|*.png|Tiff Image (.tiff)|*.tiff";
            openFileDialog1.ShowDialog();


            if (openFileDialog1.FileName != "openFileDialog1")
            {
                background = Bitmap.FromFile(openFileDialog1.FileName);
            }
        }

        private void alignArcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedCircle != null && (SelectedCircle.GetType() == typeof(Sentence.Sentence) || SelectedCircle.GetType() == typeof(Paragraph.Paragraph)))
            {
                SelectedCircle.AlignPacMouths();
                Sentence_RedrawRequest();
            }

        }

        private void saveWordMI_Click(object sender, EventArgs e)
        {
            if (SelectedCircle.GetType() == typeof(WordCircle))
            {
                WordCircle wc = (WordCircle)SelectedCircle;
                string filename = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Circular\\_wordDefine_" + wc.Word.ToString() + ".cir";

                wc.ClearEvent();

                FileStream stream = File.Create(filename);
                var formatter = new BinaryFormatter();
                Console.WriteLine("Serializing vector");
                formatter.Serialize(stream, root);
                stream.Close();


                root.SetEvent(wc);
            }
        }


        private void saveSentenceMI_Click(object sender, EventArgs e)
        {
            if (SelectedCircle.GetType() == typeof(Sentence.Sentence))
            {

                Sentence.Sentence wc = (Sentence.Sentence)SelectedCircle;
                string filename = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Circular\\_sentenceDefine_" + wc.OrignalSentence.GetHashCode() + ".cir";



                wc.ClearEvent();

                FileStream stream = File.Create(filename);
                var formatter = new BinaryFormatter();
                Console.WriteLine("Serializing vector");
                formatter.Serialize(stream, root);
                stream.Close();


                root.SetEvent(wc);
            }
        }
        #endregion
    }
}
