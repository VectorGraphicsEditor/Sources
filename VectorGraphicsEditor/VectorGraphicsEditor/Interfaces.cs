using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Interfaces
{
    public struct Parameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }

    public interface ICommand
    {
        //...
    }

    public abstract class Segment
    {
        public string Name { get; protected set; }
    }

    public class Line : Segment
    {
        public Point Beg { set; get; }
        public Point End { set; get; }
        public Line(Point beg, Point end)
        {
            Name = "Line";
            Beg = beg;
            End = end;
        }
    }

    public class Arc : Segment
    {
        public Point Center { set; get; }
        public double Rad { set; get; }
        public double Beg { set; get; }
        public double End { set; get; }
        public Arc(Point center, double rad, double beg, double end)
        {
            Name = "Arc";
            Center = center;
            Rad = rad;
            Beg = beg;
            End = end;
        }
    }

    public struct Color
    {
        Color(int r, int g, int b, int a)
        {
            R = r; G = g; B = b; A = a;
        }
        public int R { get; private set; }
        public int G { get; private set; }
        public int B { get; private set; }
        public int A { get; private set; }
    }

    public class Point : IEquatable<Point>
    {
        //        public Point() { }
        public Point(double x, double y)
        {
            X = x; Y = y;
        }

        public Point(Point NeedCopy)
        {
            X = NeedCopy.X;
            Y = NeedCopy.Y;
        }
        public double X { get; private set; }
        public double Y { get; private set; }

        public bool Equals(Point other)
        {
            return (X == other.X && Y == other.Y);
        }
    }
    public interface IPath
    {
        IEnumerable<Segment> Path { get; }
    }

    public interface ILineContainer
    {
        IEnumerable<Point> Path { get; }

    }

    public class Triangle
    {
        public Point A { get; set; }
        public Point B { get; set; }
        public Point C { get; set; }

        public Triangle(Point a, Point b, Point c)
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
        string type { get; set; }

        IEnumerable<Point> pointsForAndrew { get; set; }
        IPath Paths { get; }//geometries
        IEnumerable<Triangle> Triangles { get; }
        IEnumerable<ILineContainer> Lines { get; }

        bool IsPointInner(Point point);
        void FillPaths();
        void NewTriangulation(double eps);
        bool Colored { get; set; }
        Color FillColor { get; set; }
        Color LineColor { get; set; }
        bool Is1D { get; }
        IFigure Clone();
        IFigure Transform(ITransformation transform);
    }
}

namespace Logic
{
    using Interfaces;
    public interface ILogicForGUI
    {
        IEnumerable<IFigure> Figures { get; }
        Interfaces.Point ToScreen(Interfaces.Point xy);

        /* Рояк немного переубедил меня по поводу моего начального представления о коммандах.
         * нужно обсудить устно с ГУИ и логикой то, что будет тут. */
        
        void CreateFigure(string FigureType, List<Parameter> Parameters);
        void EditFigure(List<Parameter> Parameters);
        void Transform(Parameter type);
        void LogicOperation(string name);
        void PickFigure(bool IsAdd, Point point);

        void Save(string Filename, string Format);
        void Load(string Filename);
        void SaveSettings(string Filename);
        void LoadSettings(string Filename);

         
        void UnDo();
        void ReDo();
        void Copy();
        void Paste();
        void AddPrototipe(string name);
    }
}

namespace NGeometry
{
    using Interfaces;
    interface IGeometryForLogic
    {
        //public IFigure makeRectangle(Point topLeft, Point botRight, bool colored, Color lineColor, Color fillColor);

        //public IFigure makePoligon(IEnumerable<Point> points, bool colored, Color lineColor, Color fillColor);

        //public IFigure makeCircle(Point center, double rad, bool colored, Color lineColor, Color fillColor);

        //public IFigure makeLine(Point a, Point b, Color lineColor);

        //public IFigure makeArc(Point center, double rad, double beg, double end, Color lineColor);

        //public bool isInScreen(IFigure figure, Point t, Point botRight);

        //public IFigureScaled Scale(IFigure figure, Point topLeft, Point botRight);

        //       public IFigure Transform(IFigure ffigure, Parameter transform);

        /* Тут реально имеет смысл завести фабрику. я подумаю о том, как это будет лучше сделать.
         * Не плохо было бы поговорить об этом с логикой и геометрией.*/

        IFigure Intersection(IFigure first, IFigure second);
        IFigure Union(IFigure first, IFigure second);
        IFigure Subtraction(IFigure first, IFigure second);
    }
}

namespace IO
{
    using Interfaces;
    interface ISavePicture
    {
        bool Save(string path, IEnumerable<IFigure> figures);
        IEnumerable<IFigure> Load(string path);

    }
    interface ISaveSettings
    {
        /* понятия не имею в каком формате они будут */
        bool SaveSettings(string path, Parameter parametr /*запомненные новые фигуры, на пример*/);
        Parameter /*те же настройки */ LoadSettings(string path);
    }
}
