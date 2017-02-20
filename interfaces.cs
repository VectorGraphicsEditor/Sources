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

    public abstract class Segment
    {
        public string Name { get; protected set; }


    }

    public class Line : Segment
    {
        public Point Beg {set; get;}
        public Point End {set; get;}
        public Line(Point beg, Point end)
        {
            Name = "Line";
            Beg = beg;
            End = end;
        }
    }

    public class Arc : Segment
    {
        public Point Center {set; get;}
        public double Rad {set; get;}
        public double Beg {set; get;}
        public double End {set; get;}
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
        Color(int r,int g,int b, int a)
        {
            R = r;G = g;B = b;A = a;
        }
        public int R { get; private set; }
        public int G { get; private set; }
        public int B { get; private set; }
        public int A { get; private set; }
    }

    public struct Point
    {
//        public Point() { }
        public Point(double x, double y)
        { X = x; Y = y; }

        public double X { get; private set; }
        public double Y { get; private set; }
    }
    public interface IPath
    {
        IEnumerable<Segment> Path { get; }

        Color Color { get; set; }
    }

    public interface ILineContainer
    {
        IEnumerable<Line> Path { get; }
        Color Color { get; set; }
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
        IPath Paths { get; }
        Tuple<IEnumerable<Triangle>, ILineContainer> AsTriangulation(double eps);
        bool Colored { get; set; }
        Color FillColor { get; set; }
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
        System.Drawing.Point ToScreen(Point xy);

        void executeCommand(Parameter p);
        /* Не знаю, что лучше: внутри через case,или для каждой команды свою функцию.
         * В любом случае, тут всякая жажа, как:
         * на пример, пользователь тыкнул кнопку "рисовать квадрат", логике отправилась эта команда,
         * логика перевела программу в ожидание клика по холсту.
         * далее произошел клик - логика поставила запомнила точку, далее до отпускания мышки,
         * от этой точки до текущего положения мышки тянется контур квадрата.
         * мышку отпустили - квадрат нарисовался.
         * если пользователь передумал рисовать - логике передастся информация о том, что бы
         * она вышла из режима рисования квадрата.
         * 
         * не понятно? ну щито поделать. надо устно обсуждать.
         * 
         * тут же, кстати, фичи типа stepBack, stepForward, Scale, Exit, ritualDance...
         */
    }
}

namespace Geometry
{
    using Interfaces;
    interface IGeometry
    {
        //public IFigure makeRectangle(Point topLeft, Point botRight, bool colored, Color lineColor, Color fillColor);

        //public IFigure makePoligon(IEnumerable<Point> points, bool colored, Color lineColor, Color fillColor);

        //public IFigure makeCircle(Point center, double rad, bool colored, Color lineColor, Color fillColor);

        //public IFigure makeLine(Point a, Point b, Color lineColor);

        //public IFigure makeArc(Point center, double rad, double beg, double end, Color lineColor);

        //public bool isInScreen(IFigure figure, Point t, Point botRight);

        //public IFigureScaled Scale(IFigure figure, Point topLeft, Point botRight);

 //       public IFigure Transform(IFigure ffigure, Parameter transform);

        IFigure Intersection(IFigure first, IFigure second);
        IFigure Union(IFigure first, IFigure second);
        IFigure Subtraction(IFigure first, IFigure second);
    }
}

namespace IO
{
    using Interfaces;
    interface ILogic
    {
        public bool ToSVG(string path, IEnumerable<IFigure> figures);
        public IEnumerable<IFigure> FromSVG(string path);
        public bool SaveSettings(string path, Parameter parametr /*запомненные новые фигуры, на пример*/);
        public Parameter /*те же настройки */ LoadSettings(string path);

        // в pdf может не надо, а?..
        public bool ToPDF(string path, IEnumerable<IFigure> figures);
        public IEnumerable<IFigure> FromPDF(string path);
    }
}
