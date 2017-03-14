using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_editor
{
    public abstract partial class Figure : IFigure
    {
        private double PolarAngle(Point p0, Point p1)
        {
            double angle = Math.Atan((p1.Y - p0.Y) / (p1.X - p0.X));
            if (angle < 0) return Math.PI + angle;
            else return angle;
        }

        private bool ConvexHull()
        {
            List<Point> _CopyFigureBody = _figureBorder;

            _CopyFigureBody.OrderByDescending(point => point.X).ThenBy(point => point.Y).ToList();
            Point p0 = _CopyFigureBody[0];
            _CopyFigureBody.Remove(p0);

            _CopyFigureBody.OrderBy(point => PolarAngle(p0, point)).ThenBy(point => Math.Sqrt(Math.Pow(p0.X - point.X, 2) + Math.Pow(p0.Y - point.Y, 2))).ToList();
            var S = new Stack<Point>();
            S.Push(p0);
            S.Push(_CopyFigureBody[0]);
            Point Top = S.Peek();
            Point NextToTop = p0;

            int m = _CopyFigureBody.Count();
            for (int i = 1; i < m; i++)
            {
                Point u = new Point(Top.X - NextToTop.X, Top.Y - NextToTop.Y);
                Point v = new Point(_CopyFigureBody[i].X - Top.X, _CopyFigureBody[i].Y - Top.Y);
                while (u.X * v.Y - u.Y * v.X < 0)
                {
                    S.Pop();
                    Top = S.Pop();
                    NextToTop = S.Peek();
                    S.Push(Top);
                }
                NextToTop = S.Peek();
                S.Push(_CopyFigureBody[i]);
                Top = S.Peek();
            }
            _convexHull = S.Reverse().ToList();
            //ты хотел bool - держи
            return true;
        }

        private bool IslinesIntersect(Point A, Point B, Point C, Point D)
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
            double intersectionX = (b1 * a22 - b2 * a12) / det;
            double intersectionY = (b2 * a11 - b1 * a12) / det;

            return (intersectionX < 1 && intersectionX > 0 &&
                intersectionY < 1 && intersectionY > 0);
        }
        private Point getLinesIntersect(Point A, Point B, Point C, Point D)
        {
            double a11 = B.X - A.X;
            double a12 = C.X - D.X;
            double a21 = B.Y - A.Y;
            double a22 = C.Y - D.Y;

            //solving
            double det = a11 * a22 - a21 * a12;
            if (Math.Abs(det) < 1e-10) return null;

            double b1 = C.X - A.X;
            double b2 = C.Y - A.Y;
            double intersectionX = (b1 * a22 - b2 * a12) / det;
            double intersectionY = (b2 * a11 - b1 * a12) / det;

            if (intersectionX < 1 && intersectionX > ClosenessMeasure &&
                intersectionY < 1 && intersectionY > ClosenessMeasure)
                return new Point(A.X + intersectionX * (B.X - A.X), A.Y + intersectionX * (B.Y - A.Y));
            else return null;
        }

        private List<Triangle> LineTriangleIntersection(Point[] line, Triangle triangle)
        {
            List<Point> dots = new List<Point>
            { triangle.A, triangle.B, triangle.C};

            //чтобы понять, необходимо нарисовать пересеение линии и треугольника

            //сделаем соответствие - {пересекает ли искомая линия ребро треугольника; само ребро}
            var all = new[]
            {
                new { intersection = getLinesIntersect(line[0], line[1], triangle.A, triangle.B), segmt = new Point[2] { triangle.A, triangle.B } },
                new { intersection = getLinesIntersect(line[0], line[1], triangle.B, triangle.C), segmt = new Point[2] { triangle.B, triangle.C } },
                new { intersection = getLinesIntersect(line[0], line[1], triangle.A, triangle.C), segmt = new Point[2] { triangle.A, triangle.C } }
            }.ToList();

            //если не пересекает ни одно, то выйдем
            if (all.All(item => item.intersection == null)) return null;
            //иначе вернем новые треугольники вместо старого
            List<Triangle> result = new List<Triangle>();

            //точки пересечения
            var intersecters = all.FindAll(item => item.intersection != null);

            //костыль: если треугольник пересекается линией, исходящей из его вершины
            if (intersecters.Count == 1)
            {

                Point beginOfLine = dots.Except
                (
                 new List<Point>()
                    {
                            intersecters[0].segmt[0], intersecters[0].segmt[1]
                    },
                new PointComparer()
                ).First();

                result.Add(new Triangle(beginOfLine, intersecters.First().intersection, intersecters.First().segmt[0]));
                result.Add(new Triangle(beginOfLine, intersecters.First().intersection, intersecters.First().segmt[1]));
                return result;
            }
            else
            {
                List<Point> intersectVertex = intersecters.Select(item => item.intersection).ToList();
                if (intersectVertex[0] == intersectVertex[1]) return null;

                //запилим три треугольника вместо одного
                //первый (включающий две точки пересечения)
                Point thirdVertex = new Point(
                    intersecters[0].segmt[0] == intersecters[1].segmt[0] || intersecters[0].segmt[0] == intersecters[1].segmt[1] ?
                    intersecters[0].segmt[0] : intersecters[0].segmt[1]);

                result.Add(new Triangle(thirdVertex, intersectVertex[0], intersectVertex[1]));

                //два других

                //возьмем то ребро, которое она не пересекает
                Point[] OnNonIntersctEdge = all.Find(item => item.intersection == null).segmt;
                result.Add(new Triangle(OnNonIntersctEdge[0], intersectVertex[0], intersectVertex[1]));
                result.Add(new Triangle(OnNonIntersctEdge[0], OnNonIntersctEdge[1], intersectVertex[1]));

                return result;

            }
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
                        beg = new Point(x2, y2);
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
                _figureBorder.Add(_figureBorder.First());
                for (int i = 0; i < _figureBorder.Count - 1; i++)
                {
                    C = _figureBorder[i];
                    D = new Point(_figureBorder[i + 1].X + ClosenessMeasure, _figureBorder[i + 1].Y + ClosenessMeasure);
                    if (IslinesIntersect(A, B, C, D)) intersections++;
                }
                _figureBorder.RemoveAt(_figureBorder.Count - 1);
                return (intersections % 2 != 1);
            }
        }



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
                        IEnumerable<Point> outerPoint = trianglePoints.Except(edgesWithPoint.First(), new PointComparer());
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

                #region коррекция пересечения ребер с треугольниками в триангуляции
                for (int i = 0; i < _figureBorder.Count - 1; i++)
                {//организовать цикл по всем ребрам, включая последнее
                    edge[0] = _figureBorder[i];
                    edge[1] = _figureBorder[i + 1];
                    List<int> ToRemove = new List<int>();
                    List<Triangle> ToAdd = new List<Triangle>();
                    foreach (var item in _triangles.Select((value, j) => new { value, j }))
                    {
                        List<Triangle> possibleAdd = LineTriangleIntersection(edge, item.value);

                        if (possibleAdd != null)
                        {
                            ToAdd.AddRange(possibleAdd);
                            ToRemove.Add(item.j);
                        }

                    }
                    foreach (int index in ToRemove.OrderByDescending(item => item))
                    {
                        _triangles.RemoveAt(index);
                    }
                    _triangles.AddRange(ToAdd);

                }


                #endregion




                //выкинем те треугольники, которые не лежат в фигуре
                //(триангулировалась выпуклая оболочка с точками внутри)
                List<int> ExtraTriangles = new List<int>();
                foreach (var item in _triangles.Select((value, i) => new { i, value }))
                {
                    if (!IsPointInner(item.value.GetCenter()))
                        ExtraTriangles.Add(item.i);
                }
                foreach (int index in ExtraTriangles.OrderByDescending(item => item))
                {
                    _triangles.RemoveAt(index);
                }
            }
        }
    }
}