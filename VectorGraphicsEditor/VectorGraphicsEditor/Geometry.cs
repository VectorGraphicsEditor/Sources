using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace test_editor
{

    using Interfaces;
    using NGeometry;
    

    public class Figure : IFigure
    {
        const double ClosenessMeasure = 1e-12;//определяет меру лежания точки на прямой (через пс.скаляр. произв.)
        static int ClosenessCount = 12;//определяет число разрядов точности представления числа с плав.зпт.

        string _type { get; set; }//тип фигуры (если понадобится)
        List <Point> _pointsFromGUI { get; set; } //точки, приходящии от ГУИ, если и использовать, то с типом фигуры)

        bool _colored;
        bool _is1d;
        Color _currentColor;
        IEnumerable<ILineContainer> _lines;
        IPath _paths;
        List<Triangle> _triangles;

        List <Point> _convexHull;
        List <Point> _figureBorder;

        public Figure(string type, ref List<Point> guiPoints )
        {
            _pointsFromGUI = guiPoints;
            _type = type;
            _is1d = false;
            _triangles = new List<Triangle>();
            _figureBorder = new List<Point>() {
new Point (2, 2),new Point(4, 5), new Point(5, 28.0/5), new Point (6, 2), new Point(7, 4), new Point(9, 4), new Point(7, 8), new Point(3, 8),  new Point(1, 5)};
            //
            //CreatePaths();
            //Настя, сюда запихай выпуклую оболочку вместо следующей строки
            _convexHull = _pointsFromGUI;
            if (!_is1d)CreateTriangulation();
        }

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

        private string IsPointInTriagle(Point point, Triangle triangle, ref Point[] edgeWithPoint)
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
            List<int> signs = scalarProduct.Select(x => Math.Sign(Math.Round(x, ClosenessCount))).ToList();

            int EdgeNumb = signs.FindIndex(x => Math.Abs(x) < ClosenessMeasure);
            if (EdgeNumb != -1)
            {
                
                Point beg = new Point(), end = new Point();
                switch (EdgeNumb)
                {
                    case 0:
                    {
                        beg =  new Point(x2, y2);
                        end = new Point(x1, y1);
                        break;
                    }
                    case 1:
                    {
                        beg = new Point(x3, y3);
                        end = new Point(x2, y2);
                        break;
                    }
                    case 2:
                    {
                        beg = new Point(x3, y3);
                        end = new Point(x1, y1);
                        break;
                    }
                }
                edgeWithPoint[0] = beg;
                edgeWithPoint[1] = end;

                return "On the border";
            }

            else
            if (signs.All(x => x < 0) || signs.All(x => x > 0))
            {
                
                return "Inner";
            }

            else
            {
                
                return "External";
            }
        }
        public bool IsPointInner(Point point)
        {
            //метод трассирующего луча
            if (_figureBorder.Contains(point)) return true;

            else
            {
                Point A = point;
                Point B = new Point(_convexHull[0].X + 1, A.Y);
                Point C = new Point();
                Point D = new Point();

                int intersections = 0;
                for (int i = 0; i < _figureBorder.Count - 1; i++)
                {
                    C = _figureBorder[i];
                    D = new Point(_figureBorder[i + 1].X + ClosenessMeasure, _figureBorder[i + 1].Y + ClosenessMeasure);
                    if (lineIntersection(A, B, C, D)) intersections++;
                }
                //проверим последнее ребро
                C = _figureBorder[_figureBorder.Count - 1];
                D = _figureBorder[0];
                if (lineIntersection(A, B, C, D)) intersections++;

                return (intersections % 2 != 1);
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

        public Dictionary<string, object> Parameters { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IFigure Clone()
        {
            throw new NotImplementedException();
        }
        #endregion

        private bool IsPointAtLine(Point purpose, Point A, Point B)
        {
            //y=kx+b
            double k = (A.Y - B.Y) / (A.X - B.X);
            double b = B.Y - k * B.X;
            return (Math.Abs(purpose.Y - k * purpose.X - b) < ClosenessMeasure);
        }

        private void CreateTriangulation()
        {   //просто одно ребро, потом понадобится
            Point[] edge;

            //начальная триангуляция с исползованием выпуклой оболочки
            for (int i = 0; i < _convexHull.Count - 2; i++)
            {
                Point A = _convexHull[0];
                Point B = _convexHull[i + 1];
                Point C = _convexHull[i + 2];
                //Point center = new Point((A.X + B.X + C.X) / 3, (A.Y + B.Y + C.Y) / 3);
                _triangles.Add(new Triangle(_convexHull[0], _convexHull[i + 1], _convexHull[i + 2]));
            }
                #region подготовка к корректированию триангуляции
            //выбираем вершины, не попавшие в выпуклую оболочку
            List<Point> InnerOrBoundary = _figureBorder.Except(_convexHull, new PointComparer()).ToList();
            if (InnerOrBoundary.Count != 0)
            {
                //для каждой (не из выпуклой оболочки) вершине ставим в соответсиве два треугольника, которые содержат на своей границе эту вершину.
                List<List<Triangle>> boundaryTriangles = new List<List<Triangle>>();
                //в соответствие этим треугольникам поставим ребро, которое содержит на себе точку.
                List<Point[]> edgesWithPoint = new List<Point[]>();
                edge = new Point[2];

                //вершины, которые собственно и лежат на границе
                List<Point> boundaryPoints = new List<Point>();

                //вершины внутри выпуклой оболочки и не на границе
                List<Point> innerPoints = new List<Point>();

                for (int i = 0; i < InnerOrBoundary.Count; i++)
                {

                    var twoTriangles = _triangles.Where(x => IsPointInTriagle(InnerOrBoundary[i], x, ref edge) == "On the border").ToList();

                    if (twoTriangles.Count != 0)
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
                #endregion
                //теперь у нас есть соответствие (вершина на ребре - ребро - треугольники, для которых это ребро смежное)
                //+ вершины не на ребрах

                DeleteBoundaryPoints:
                #region нейтрализация вершин на границах
                //Вместо двух треугольников запихаем четыре.
                while (boundaryTriangles.Count != 0)
                {
                    foreach (Triangle triangle in boundaryTriangles.First())
                    {
                        List<Point> trianglePoints = new List<Point> { triangle.A, triangle.B, triangle.C };
                        IEnumerable<Point> outerPoint =  trianglePoints.Except(edgesWithPoint.First(), new PointComparer()) ;
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
                #endregion

                edge = new Point[2];

                #region обработка внутренних точек
                //работаем с тупо внутренними точками
                while (innerPoints.Count != 0)
                {
                    Point considered = innerPoints.First();

                    //найдем внешний для точки треугольник
                    int indexExtTriangle = _triangles.FindIndex(x => IsPointInTriagle(considered, x, ref edge) == "Inner");
                    Triangle externalTriangle = _triangles[indexExtTriangle];

                    //создадим три новых треугольника вместо старого
                    Triangle first = new Triangle(externalTriangle.A, externalTriangle.B, considered);
                    Triangle second = new Triangle(externalTriangle.B, externalTriangle.C, considered);
                    Triangle third = new Triangle(externalTriangle.A, externalTriangle.C, considered);

                    _triangles.Add(first);
                    _triangles.Add(second);
                    _triangles.Add(third);

                    //точку и старый треугольник удалим
                    _triangles.RemoveAt(indexExtTriangle);
                    innerPoints.RemoveAt(0);

                    //определим точки, которые не были на ребрах, а теперь стали
                    boundaryPoints.AddRange(innerPoints.FindAll(x => IsPointAtLine(x, externalTriangle.A, considered)));
                    boundaryPoints.AddRange(innerPoints.FindAll(x => IsPointAtLine(x, externalTriangle.B, considered)));
                    boundaryPoints.AddRange(innerPoints.FindAll(x => IsPointAtLine(x, externalTriangle.C, considered)));

                    //если в итоге такие точки есть, то по новой их удалять
                    if (boundaryPoints.Count != 0) goto DeleteBoundaryPoints;
                }
                #endregion
                //
            }

            //выкинем те треугольники, которые не лежат в фигуре
            //(триангулировалась выпуклая оболочка с точками внутри)
            List<int> ExtraTriangles = new List<int>();
            foreach (var item in _triangles.Select((value, i) => new { i, value }))
            {
                if (!IsPointInner(item.value.GetCenter()))
                    ExtraTriangles.Add(item.i);
            }
            foreach (int index in ExtraTriangles)
            {
                _triangles.RemoveAt(index);
            }
        }

        public IFigure Transform(ITransformation transform)
        {
            throw new NotImplementedException();
        }

        private void CreatePaths()
        {
            throw new NotImplementedException();
        }

        public void FillPaths()
        {
            throw new NotImplementedException();
        }

        public Tuple<IEnumerable<Triangle>, IEnumerable<ILineContainer>> NewTriangulation(double eps)
        {
            throw new NotImplementedException();
        }

        public IFigure Create(Dictionary<string, object> parms)
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
