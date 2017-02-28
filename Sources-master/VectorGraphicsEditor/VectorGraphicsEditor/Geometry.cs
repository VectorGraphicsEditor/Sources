using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace test_editor
{

    /*
public class Triangle
{
public Triangle(double A, double B, double C)
{ a = A; b = B; c = C; }

public double a { get; set; }
public double b { get; set; }
public double c { get; set; }
}
class Figure
{   

List<Triangle> Triangulation;

static void Main(string[] args)
{
}


}*/
    using Interfaces;
    using NGeometry;


    class Figure : IFigure
    {
        bool _colored;
        bool _is1d;
        Color _currentColor;
        IEnumerable<ILineContainer> _lines;
        IPath _paths;
        List<Triangle> _triangles;

        Point[] _convexHull;
        Point[] _figureBorder;

        private bool lineIntersection(Point A, Point B, Point C, Point D)
        {//SLAE coeffs
            double a11 = B.X - A.X;
            double a12 = C.X - D.X;
            double a21 = B.Y - A.Y;
            double a22 = C.Y - D.Y;

            //solving
            double det = a11 * a22 - a21 * a12;
            if (Math.Abs(det) < 1e-10) return false;

            double b1 = C.X - A.X;
            double b2 = C.Y - A.Y;
            double intersectionX = (b1 * a22 + b2 * a12) / det;
            double intersectionY = (b2 * a11 - b1 * a12) / det;

            return (intersectionX < 1 && intersectionX > 0 &&
                intersectionY < 1 && intersectionY > 0);
        }

        private string IsPointInTriagle(Point point, Triangle triangle, out Point[] edgeWithPoint)
        {
            double x1 = triangle.A.X;
            double y1 = triangle.A.Y;
            double x2 = triangle.B.X;
            double y2 = triangle.B.Y;
            double x3 = triangle.C.X;
            double y3 = triangle.C.Y;
            double x0 = point.X;
            double y0 = point.Y;

            double first = (x1 - x0) * (y2 - y1) - (x2 - x1) * (y1 - y0);
            double second = (x2 - x0) * (y3 - y2) - (x3 - x2) * (y2 - y0);
            double third = (x3 - x0) * (y1 - y3) - (x1 - x3) * (y3 - y0);

            double[] scalarProduct = new double[3] { first, second, third };
            IEnumerable<int> signs = scalarProduct.Select(x => Math.Sign(x));
            if (signs.Any(x => x == 0))
            {
                edgeWithPoint = new Point[2];
                //edgeWithPoint[0] =;
                //edgeWithPoint[1] =;

                return "On the border";
            }

            else
            if (signs.All(x => x < 0) || signs.All(x => x > 0))
            {
                edgeWithPoint = null;
                return "Inner";
            }

            else
            {
                edgeWithPoint = null;
                return "External";
            }
        }
        public bool IsPointInner(Point point)
        {
            if (_figureBorder.Contains(point)) return true;

            else
            {
                Point A = point;
                Point B = new Point(_convexHull[0].X + 1, A.Y);

                int intersections = 0;
                for (int i = 0; i < _figureBorder.Length; i++)
                {
                    Point C = _figureBorder[i];
                    Point D = new Point(_figureBorder[i + 1].X + 1e-10, _figureBorder[i + 1].Y + 1e-10);
                    if (lineIntersection(A, B, C, D)) intersections++;
                }
                return (intersections % 2 == 1);
            }
        }

        #region from interfaces
        public bool Colored
        {
            get
            {
                return _colored;
            }

            set
            {
                _colored = value;
            }
        }

        public Color FillColor
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool Is1D
        {
            get
            {
                return _is1d;
            }
        }

        public Color LineColor
        {
            get
            {
                return _currentColor;
            }

            set
            {
                _currentColor = value;
            }
        }

        public IEnumerable<ILineContainer> Lines
        {
            get
            {
                return _lines;
            }
        }

        public IPath Paths
        {
            get
            {
                return _paths;
            }
        }

        public IEnumerable<Triangle> Triangles
        {
            get
            {
                return _triangles;
            }
        }

        public string type
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<Point> pointsForAndrew
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IFigure Clone()
        {
            throw new NotImplementedException();
        }
        #endregion

        private bool IsPointAtLine(Point purpose, Point A, Point B)
        {
            throw new NotImplementedException();
        }

        public void NewTriangulation(double eps)
        {
            //начальная триангуляция с исползованием выпуклой оболочки
            for (int i = 0; i < _convexHull.Length - 2; i++)
            {
                Point A = _convexHull[0];
                Point B = _convexHull[i + 1];
                Point C = _convexHull[i + 2];
                //Point center = new Point((A.X + B.X + C.X) / 3, (A.Y + B.Y + C.Y) / 3);
                _triangles.Add(new Triangle(_convexHull[0], _convexHull[i + 1], _convexHull[i + 2]));
            }

            //выбираем вершины, не попавшие в выпуклую оболочку
            List<Point> InnerOrBoundary = _figureBorder.Except(_convexHull).ToList();

            //для каждой (не из выпуклой оболочки) вершине ставим в соответсиве два треугольника, которые содержат на своей границе эту вершину.
            List<List<Triangle>> boundaryTriangles = new List<List<Triangle>>();
            //в соответствие этим треугольникам поставим ребро, которое содержит на себе точку.
            List<Point[]> edgesWithPoint = new List<Point[]>();
            var edge = new Point[2];

            //вершины, которые собственно и лежат на границе
            List<Point> boundaryPoints = new List<Point>();

            //вершины внутри выпуклой оболочки и не на границе
            List<Point> innerPoints = new List<Point>();

            for (int i = 0; i < InnerOrBoundary.Count; i++)
            {

                var twoTriangles = _triangles.Where(x => IsPointInTriagle(InnerOrBoundary[i], x, out edge) == "On the border").ToList();

                if (twoTriangles != null)
                {
                    boundaryTriangles.Add(twoTriangles);
                    edgesWithPoint.Add(edge);
                    boundaryPoints.Add(InnerOrBoundary[i]);
                }
                else
                {
                    innerPoints.Add(InnerOrBoundary[i]);
                }
            }
            //теперь у нас есть соответствие (вершина на ребре - ребро - треугольники, для которых это ребро смежное)
            //+ вершины не на ребрах

            //Вместо двух треугольников запихаем четыре.
            while (boundaryTriangles.Count != 0)
            {
                foreach (Triangle triangle in boundaryTriangles.First())
                {
                    List<Point> trianglePoints = new List<Point> { triangle.A, triangle.B, triangle.C };
                    var outerPoint = trianglePoints.Except(edgesWithPoint.First());
                    var firstAdding = new Triangle(outerPoint.First(), edgesWithPoint.First()[0], boundaryPoints.First());
                    var secondAdding = new Triangle(outerPoint.First(), edgesWithPoint.First()[1], boundaryPoints.First());

                    _triangles.Add(firstAdding);
                    _triangles.Add(secondAdding);

                    //убираем использованный
                    _triangles.Remove(triangle);

                    //теперь, если вдруг после добавления тех двух треугольников внутренняя точка перешла в разряд точек на границе
                    //мы ее добавим в конец в граничные, а так же два треугольникак как смежные и тд.
                    foreach (Point point in innerPoints)
                    {
                        if (IsPointAtLine(point, edgesWithPoint.First()[0], edgesWithPoint.First()[1]))
                        {
                            boundaryTriangles.Add(new List<Triangle>() { firstAdding, secondAdding });
                            edgesWithPoint.Add(edgesWithPoint.First());
                            boundaryPoints.Add(point);
                        }
                    }
                }
                //обработали первую граничную точку, убрали все с ней связанное, и по новой
                boundaryTriangles.RemoveAt(0);
                edgesWithPoint.RemoveAt(0);
                boundaryPoints.RemoveAt(0);

            }

        }

        public IFigure Transform(ITransformation transform)
        {
            throw new NotImplementedException();
        }

        public void FillPaths()
        {
            throw new NotImplementedException();
        }
    }


    class Geometry : IGeometryForLogic
    {
        IFigure IGeometryForLogic.Intersection(IFigure first, IFigure second)
        {
            throw new NotImplementedException();
        }

        IFigure IGeometryForLogic.Subtraction(IFigure first, IFigure second)
        {
            throw new NotImplementedException();
        }

        IFigure IGeometryForLogic.Union(IFigure first, IFigure second)
        {
            throw new NotImplementedException();
        }
    }
}
