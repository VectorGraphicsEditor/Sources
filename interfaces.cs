using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Interfaces
{

    struct Parameter
    {
        string Name { get; set; }
        object Value { get; set; }
    }

    abstract class Segment
    {
        public string name { get; protected set; }


    }

    public class Line : Segment
    {
        public Point Beg {set; get;}
        public Point End {set; get;}
        public Line(Point beg, Point end)
        {
            name = "Line";
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
            name = "Arc";
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
        public Point(double x, double y)
        { X = x; Y = y; }

        public double X { get; set; }
        public double Y { get; set; }
    }
    interface IPath
    {
        public List<Segment> path = new List<Segment>();

        public Color color { get; set; }
        public IPath() { }
    }

    interface IPathScaled : IPath
    {
        public List<Line> path = new List<Line>();
        public Color color { get; set; }
        public IPathScaled(IPath path);
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

    interface IFigure
    {
        public List<IPath> paths;
        public List<Triangle> Triangulation;
        public bool Colored;
        public Color FillColor;
        public int dimensional;

        public IFigure Clone(Point shift);
    }

    interface IFigureScaled
    {
        public List<IPathScaled> paths;
        public List<Triangle> Triangulation;
        public bool Colored;
        public Color FillColor;

        public IFigureScaled(IFigure figure, Point topLeft, Point botRight); 
    }

}

namespace Logic
{
    using Interfaces;
    interface IGUI
    {
        IEnumerable<IFigureScaled> getToDraw();

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
    interface ILogic
    {
        IFigure makeRectangle(Point topLeft, Point botRight, bool colored, Color lineColor, Color fillColor);

        IFigure makePoligon(IEnumerable<Point> points, bool colored, Color lineColor, Color fillColor);

        IFigure makeCircle(Point center, double rad, bool colored, Color lineColor, Color fillColor);

        IFigure makeLine(Point a, Point b, Color lineColor);

        IFigure makeArc(Point center, double rad, double beg, double end, Color lineColor);

        bool isInScreen(IFigure figure, Point t, Point botRight);

        IFigureScaled Scale(IFigure figure, Point topLeft, Point botRight);

        IFigure Transform(IFigure ffigure, Parameter transform);

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
        public bool ToSVN(string path, IEnumerable<IFigure> figures);
        public IEnumerable<IFigure> FromSVN(string path);
        public bool SaveSettings(string path, Parameter parametr /*запомненные новые фигуры, на пример*/);
        public Parameter /*те же настройки */ LoadSettings(string path);

        // в pdf может не надо, а?..
        public bool ToPDF(string path, IEnumerable<IFigure> figures);
        public IEnumerable<IFigure> FromPDF(string path);
    }
}
