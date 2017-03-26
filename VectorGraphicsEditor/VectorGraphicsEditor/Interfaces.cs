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
        public double BegRad { set; get; }
        public double EndRad { set; get; }
        public EllipseArc(Point center,double r1, double r2, double rad, double beg, double end)
        {
            Name = "Arc";
            Center = center;
            Rad = rad;
            BegRad = beg;
            EndRad = end;
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
        public double X { get; private set; }
        public double Y { get; private set; }

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
        Interfaces.Point ToScreen(Interfaces.Point xy);
    }

    public interface ILogicForCommand
    {
        int CountFigures { get; }
        int CountCurientFigures { get; }
        int IndexCurientElem { get; }

        void addFigure(IFigure fig);

        void removeFigures();

        void addCurientFigure(Interfaces.Point dot, bool add);

        void moveIndexFigure(bool direction);

        void moveCurientIndex(bool direction);

        int GetStackIndex();

        int GetStackCount();

        void SetPreviousStackState();

        void SetNextStackState();
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
