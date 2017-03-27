using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectorGraphicsEditor;

namespace Interfaces
{
    public struct Parameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }
        
    public interface ICommand
    {
        ICommand Create(Dictionary<string, object> parms);
        bool CanExecute(object x);

        void Execute(object x);
    }

    public abstract class Segment
    {
        public Point Beg { set; get; }
        public Point End { set; get; }
        public string Name { get; protected set; }
    }

    public class Line : Segment
    {

        public Line(Point beg, Point end)
        {
            Name = "Line";
            Beg = beg;
            End = end;
        }
    }

    public class EllipseArc : Segment
    {
        public Point Center { set; get; }
        public double Rad { set; get; }
        public double A { get; set; }//длина полуоси
        public double B { get; set; }
        public double BegRad { set; get; }
        public double EndRad { set; get; }
        public double RotAngle { get; set; }
        public EllipseArc(Point center, double a, double b, double begrad, double endrad, Point beg, Point end, double RotationAngle)
        {
            Name = "Arc";
            Center = center;
            A = a;
            B = b;
            BegRad = begrad;
            EndRad = endrad;
            Beg = beg;
            End = end;
            RotAngle = RotationAngle;
        }
    }

    public struct Color
    {
        public Color(int r, int g, int b, int a)
        {
            R = r; G = g; B = b; A = a;
        }
        public int R { get; private set; }
        public int G { get; private set; }
        public int B { get; private set; }
        public int A { get; private set; }
    }
    public class PointComparer : IEqualityComparer<Point>
    {

        public bool Equals(Point x, Point y)
        {
            return (
                Math.Abs(x.X - y.X) < Constants.epsForEqualPoints &&
                Math.Abs(x.Y - y.Y) < Constants.epsForEqualPoints
                );
        }

        public int GetHashCode(Point obj)
        {
            return Math.Pow(obj.X, obj.Y).GetHashCode();
        }
    }
    public class Point
    {
        public Point() { X = 0;Y = 0; }
        public Point(double x, double y)
        {
            X = x; Y = y;
        }

        public Point(Point NeedCopy)
        {
            X = NeedCopy.X;
            Y = NeedCopy.Y;
        }
        public double X { get;  set; }
        public double Y { get;  set; }

        }
    public interface IPath
    {
        IEnumerable<Segment> Path { get; }
    }

    public interface ILineContainer
    {
        IEnumerable<Point> Path { get; }

    }

    public class trTriangle
    {
        public Point A { get; set; }
        public Point B { get; set; }
        public Point C { get; set; }
        public Point GetCenter()
        {
            return( new Point((A.X + B.X + C.X) / 3, (A.Y + B.Y + C.Y) / 3));
        }
        public trTriangle(Point a, Point b, Point c)
        {
            A = a;
            B = b;
            C = c;
        }
    }
    public interface ITransformation
    {
        Point TransformPoint(Point p);
    }
    public interface IFigure
    {
        /* заметим, что Paths хранит отрезки и дуги, так что может хранить несколько кривых,
         * а Lines - точки, так что для представления разных кривых понадобится массив контейнеров точек.*/
        string type { get; }

        Dictionary<string, object> Parameters
        {
            get;
            set;
        }
        // тип параметры фигуры. для прямоугольника две точки, для окружности точка и радиус...

        IPath Paths { get; }//geometries

        bool IsPointInner(Point point);
        void FillPaths(); // это чо вообще??
        Tuple<IEnumerable<trTriangle>,IEnumerable<ILineContainer>> NewTriangulation(double eps);
        // стоит хранить предыдущий результат, что бы не перещитывать его, если функция вызывается с тем же eps

        bool Colored { get; set; }
        Color FillColor { get; set; }
        Color LineColor { get; set; }
        bool Is1D { get; }
        IFigure Clone(Dictionary<string, object> parms); // создать такую же фигуру с такими же параметрами
        IFigure Transform(ITransformation transform);
        /*Трансформация возвращает новую фигуру, трансформированную. однако очевидно что после этого
          фигура может сменить свой тип.*/
    }
}

namespace Logic
{
    using Interfaces;
    public interface ILogicForGUI
    {
        IEnumerable<IFigure> Figures { get; }
        List<int> CurientPickFigures { get; }
    }

    public interface ILogicForCommand
    {
        int CountFigures { get; }
        int CountCurientFigures { get; }
        int IndexCurientElem { get; }

        void addFigure(IFigure fig);

        void removeFigures();

        void addCurientFigure(Interfaces.Point dot, bool add);

        void addCurientFigureWithIndex(int index, bool add);

        void moveIndexFigure(bool direction);

        void moveCurientIndex(bool direction);


        void editColor(Interfaces.Color newcolor);

        void editBorderColor(Interfaces.Color newcolor);
      
        int GetStackIndex();

        int GetStackCount();

        void SetPreviousStackState();

        void SetNextStackState();

        void PutIntoBuffer();

        int BufferSize
        { get; }

        void PushBuffer();
    }
}

namespace NGeometry
{
    using Interfaces;
    interface IGeometryForLogic
    {
        /*допилить фабрику*/

        IFigure Intersection(IFigure first, IFigure second);
        IFigure Union(IFigure first, IFigure second);
        IFigure Subtraction(IFigure first, IFigure second);
    }
}

namespace IO
{
    using Interfaces;
    using SVGLib;
    //interface ISavePicture
    //{
    //    bool Save(string path, IEnumerable<IFigure> figures);
    //    IEnumerable<IFigure> Load(string path);

    //}
    //interface ISaveSettings
    //{
    //    /* понятия не имею в каком формате они будут */
    //    bool SaveSettings(string path, Parameter parametr /*запомненные новые фигуры, на пример*/);
    //    Parameter /*те же настройки */ LoadSettings(string path);
    //}

    public abstract class SVGShape {
        public Color fill { get; protected set; }
        public Color stroke { get; protected set; }
        public int w { get; protected set; } // stroke width
        public String name { get; protected set; }
        public abstract SvgBasicShape ToSVGLibShape(SvgDoc doc);
    }

    public static class SVGIO {

        public static void export(List<SVGShape> shapes, string filename, int width, int height)
        {
            SvgDoc doc = new SvgDoc();
            SvgRoot root = doc.CreateNewDocument();
            root.Width = width.ToString() + "px";
            root.Height = height.ToString() + "px";
            foreach(var shape in shapes)
            {
                var figure = shape.ToSVGLibShape(doc);
                doc.AddElement(root, figure);
            }
            doc.SaveToFile(filename);
        }

        public static Tuple<List<SVGShape>, int, int> import(string filename)
        {

            var shapes = new List<SVGShape>();
            SvgDoc doc = new SvgDoc();
            doc.LoadFromFile(filename);
            SvgRoot root = doc.GetSvgRoot();
            int width, height = 1;
            Int32.TryParse(root.Width.ToString().Substring(0, root.Width.Length - 2), out width);
            Int32.TryParse(root.Height.ToString().Substring(0, root.Height.Length - 2), out height);
            int i = 2;
            SvgElement el;
            double x, y, r, rx, ry, rectw, recth;
            int w;
            while ((el = doc.GetSvgElement(i)) != null)
            {
                el = doc.GetSvgElement(i);

                el.GetAttribute("RX");

                switch (el.getElementName())
                {

                    case "circle":

                        Double.TryParse(el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_CX).Substring(0, el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_CX).Length - 2), out x);
                        Double.TryParse(el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_CY).Substring(0, el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_CY).Length - 2), out y);
                        Double.TryParse(el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_R).Substring(0, el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_R).Length - 2), out r);
                        Int32.TryParse(el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrPaint_StrokeWidth).Substring(0, el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrPaint_StrokeWidth).Length - 2), out w);
                        var colorf = el.GetAttributeColorValue(SvgAttribute._SvgAttribute.attrPaint_Fill);
                        var colors = el.GetAttributeColorValue(SvgAttribute._SvgAttribute.attrPaint_Stroke);
                        var circle = new SVGCircle(
                            new Point(x, y), r,
                            new Color(colorf.R, colorf.G, colorf.B, colorf.A),
                            new Color(colors.R, colors.G, colors.B, colors.A),
                            w
                        );
                        shapes.Add(circle);
                        break;

                    case "ellipse":

                        Double.TryParse(el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_CX).Substring(0, el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_CX).Length - 2), out x);
                        Double.TryParse(el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_CY).Substring(0, el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_CY).Length - 2), out y);
                        Double.TryParse(el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_RX).Substring(0, el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_RX).Length - 2), out rx);
                        Double.TryParse(el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_RY).Substring(0, el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_RY).Length - 2), out ry);
                        Int32.TryParse(el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrPaint_StrokeWidth).Substring(0, el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrPaint_StrokeWidth).Length - 2), out w);
                        colorf = el.GetAttributeColorValue(SvgAttribute._SvgAttribute.attrPaint_Fill);
                        colors = el.GetAttributeColorValue(SvgAttribute._SvgAttribute.attrPaint_Stroke);
                        var ellipse = new SVGEllipse(
                            new Point(x, y), rx, ry,
                            new Color(colorf.R, colorf.G, colorf.B, colorf.A),
                            new Color(colors.R, colors.G, colors.B, colors.A),
                            w
                        );
                        shapes.Add(ellipse);
                        break;

                    case "rect":

                        Double.TryParse(el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_Width).Substring(0, el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_Width).Length - 2), out rectw);
                        Double.TryParse(el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_Height).Substring(0, el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_Height).Length - 2), out recth);
                        Double.TryParse(el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_X).Substring(0, el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_X).Length - 2), out x);
                        Double.TryParse(el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_Y).Substring(0, el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_Y).Length - 2), out y);
                        Int32.TryParse(el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrPaint_StrokeWidth).Substring(0, el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrPaint_StrokeWidth).Length - 2), out w);
                        colorf = el.GetAttributeColorValue(SvgAttribute._SvgAttribute.attrPaint_Fill);
                        colors = el.GetAttributeColorValue(SvgAttribute._SvgAttribute.attrPaint_Stroke);
                        var rect = new SVGRect(
                            x, y, rectw, recth,
                            new Color(colorf.R, colorf.G, colorf.B, colorf.A),
                            new Color(colors.R, colors.G, colors.B, colors.A),
                            w
                        );
                        shapes.Add(rect);
                        break;

                    case "polygon":

                        String pointsStr = "";
                        var points = new List<Point>();
                        pointsStr=el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrSpecific_Points);
                        string[] value = pointsStr.Split(' ', ',');
                        for (int k = 0; k<value.Length; k = k + 2)
                        {
                            if (value[k].Trim() != "" && value[k + 1].Trim() != "")
                            points.Add(new Point(Convert.ToDouble(value[k]), Convert.ToDouble(value[k + 1])));
                        };
                        Int32.TryParse(el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrPaint_StrokeWidth).Substring(0, el.GetAttributeStringValue(SvgAttribute._SvgAttribute.attrPaint_StrokeWidth).Length - 2), out w);
                        colorf = el.GetAttributeColorValue(SvgAttribute._SvgAttribute.attrPaint_Fill);
                        colors = el.GetAttributeColorValue(SvgAttribute._SvgAttribute.attrPaint_Stroke);
                        var polygon = new SVGPolygon(
                            points,
                            new Color(colorf.R, colorf.G, colorf.B, colorf.A),
                            new Color(colors.R, colors.G, colors.B, colors.A),
                            w
                        );
                        shapes.Add(polygon);
                        break;
                }

                ++i;
            }

            //foreach (var shape in shapes)
            //{
            //    var figure = shape.ToSVGLibShape(doc);
            //    doc.AddElement(root, figure);
            //}
            //doc.SaveToFile(filename);

            return Tuple.Create(shapes, width, height);
        }

    }

}
