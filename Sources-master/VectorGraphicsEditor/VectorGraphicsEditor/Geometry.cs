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
    

    public partial class Figure : IFigure
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

        List<Point> _convexHull;
        List<Point> _figureBorder;


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
            for(int i = 1; i < m; i++)
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
            _convexHull = S.ToList();
            //ты хотел bool - держи
            return true;
        }

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
            _convexHull = _pointsFromGUI;
            if (!_is1d)CreateTriangulation();
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

 


        public IFigure Transform(ITransformation transform)
        {
            throw new NotImplementedException();
        }

        private void CreatePaths()
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
