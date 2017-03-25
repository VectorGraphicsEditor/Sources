using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test_editor;

namespace VectorGraphicsEditor
{
    public class SpecPath : ILineContainer
    {
        public IEnumerable<Point> Path
        {
            get;
            set;
        }
    }

    public class Rectangle : Figure
    {
        public Rectangle(Point DownLeft, Point UpRight, Color BorderColor, Color FillColor)
        {
            Colored = true;
            Is1D = false;
            this.FillColor = FillColor;
            this.LineColor = BorderColor;

            _figureBorder = new List<Point>()
            {
                new Point(UpRight.X, DownLeft.Y),
                UpRight,
                new Point(DownLeft.X, UpRight.Y),
                DownLeft
            };
            _convexHull = new List<Point>(_figureBorder);
            _figureBorder = new List<Point>(_figureBorder);

            _triangles = new List<trTriangle>()
            {
                new trTriangle(_figureBorder[0], _figureBorder[1], _figureBorder[2]),
                new trTriangle(_figureBorder[2], _figureBorder[3], _figureBorder[0])
            };

        }

        public override bool Colored { get; set; }

        public override Color FillColor { get; set; }

        public override bool Is1D { get; protected set; }

        public override Color LineColor { get; set; }

        public override Dictionary<string, object> Parameters { get; set; }

        public override IPath Paths { get; }

        public override string type { get; }

        public override IFigure Clone(Dictionary<string, object> parms)
        {
            return new Rectangle((Point)parms["DownLeft"],
                            (Point)parms["UpRight"],
                            (Color)parms["BorderColor"],
                            (Color)parms["FillColor"]);
        }

        public override void FillPaths()
        {
            throw new NotImplementedException();
        }


        public override Tuple<IEnumerable<trTriangle>, IEnumerable<ILineContainer>> NewTriangulation(double eps)
        {
            SpecPath path = new SpecPath();
            path.Path = _figureBorder;
            List<ILineContainer> ToReturn = new List<ILineContainer>() { path };
            return new
                Tuple<IEnumerable<trTriangle>, IEnumerable<ILineContainer>>
                (_triangles, ToReturn);
        }

        public override IFigure Transform(ITransformation transform)
        {
            throw new NotImplementedException();
        }
    }

    public class Triangle : Figure
    {
        public Triangle(Point Point1, Point Point2, Point Point3, Color BorderColor, Color FillColor)
        {
            Colored = true;
            Is1D = false;
            this.FillColor = FillColor;
            this.LineColor = BorderColor;

            _figureBorder = new List<Point>()
                {
                    new Point(Point1.X, Point1.Y),
                    new Point(Point2.X, Point2.Y),
                    new Point(Point3.X, Point3.Y)
                };
            _convexHull = new List<Point>(_figureBorder);
            _figureBorder = new List<Point>(_figureBorder);

            _triangles = new List<trTriangle>()
                {
                    new trTriangle(_figureBorder[0], _figureBorder[1], _figureBorder[2]),
                };

        }
        public override string type { get; }

        public override Dictionary<string, object> Parameters { get; set; }

        public override IPath Paths { get; }

        public override bool Colored { get; set; }

        public override Color FillColor { get; set; }

        public override Color LineColor { get; set; }

        public override bool Is1D { get; protected set; }

        public override void FillPaths()
        {
            throw new NotImplementedException();
        }

        public override Tuple<IEnumerable<Interfaces.trTriangle>, IEnumerable<ILineContainer>> NewTriangulation(double eps)
        {
            SpecPath path = new SpecPath();
            path.Path = _figureBorder;
            List<ILineContainer> ToReturn = new List<ILineContainer>() { path };
            return new
                Tuple<IEnumerable<trTriangle>, IEnumerable<ILineContainer>>
                (_triangles, ToReturn);
        }

        public override IFigure Clone(Dictionary<string, object> parms)
        {
            return new Triangle((Point)parms["Point1"],
                            (Point)parms["Point2"],
                            (Point)parms["Point3"],
                            (Color)parms["BorderColor"],
                            (Color)parms["FillColor"]);
        }

        public override IFigure Transform(ITransformation transform)
        {
            throw new NotImplementedException();
        }
    }

    public class Mutant : Figure
    {
        //представление в виде списка связных границ
        //произвольного типа
        List<List<Segment>> path;

        private double PerimetreOfEllipse(double a, double b)
        {
            double x = Math.Log(2) / Math.Log(Math.PI / 2);
            return 4 * Math.Pow((Math.Pow(a, x) + Math.Pow(b, x)), 1 / x);
        }
        public Mutant(List<List<Segment>> path, Color BorderColor, Color FillColor, double Epsilon)
        {
            this.FillColor = FillColor;
            this.LineColor = BorderColor;

            this.path = path;
            _onlyPoints = new List<List<Point>>();
            #region Обработка входных данных
            foreach (List<Segment> border in path)
            {
                List<Point> toAdd = new List<Point>();
                int count = border.Count;
                int i = 0;
                foreach (Segment segment in border)
                {

                    if (segment.Name == "Line")
                    {
                        toAdd.Add(segment.Beg);

                    }
                    else if (segment.Name == "Arc")
                    {
                        //должен возвращать все, кроме последней точки
                        toAdd.AddRange(ArcPointsConverter(segment, Epsilon));

                    }
                    if (i == count - 1)
                        toAdd.Add(segment.End);
                }

                _onlyPoints.Add(toAdd);
            }
            #endregion 
            ConvexHull();
            CreateTriangulation();

        }
        private IEnumerable<Point> ArcPointsConverter(Segment segment, double eps)
        {
            EllipseArc arc = (EllipseArc)segment;

            //какая часть периметра задействована
            double part = Math.Abs(arc.EndRad - arc.BegRad) / (2 * Math.PI);

            //сколько реально длины периметра задействовано
            part *= PerimetreOfEllipse(arc.A, arc.B);

            //сколько отрезков
            part = part / eps;



            List<Point> result = new List<Point>();
            double x, y, t = 0;
            for (int i = 0; i < (int)part; i++)
            {
                x = arc.A * Math.Cos(t) + arc.Center.X;
                y =  arc.Center.Y;
                if (arc.Beg.X < arc.End.X)
                    y += arc.B * Math.Sin(t);
                else
                    y -= arc.B * Math.Sin(t);

                result.Add(new Point(x, y));
                t += Math.PI / part;
            }
            double cos = Math.Cos(Math.PI*arc.RotAngle/180);
            double sin = Math.Sin(Math.PI * arc.RotAngle/180);

            foreach (Point point in result)
            {
                point.X = point.X * cos - point.Y * sin;
                point.Y = point.X * sin + point.Y * cos;   
            }
            if (arc.Beg.X < arc.End.X)
            {
                result.Add(arc.Beg);
                result.RemoveAt(0);
                result.Reverse();
                }
            return result;

        }




        #region интерфейса реализация
        public override bool Colored { get; set; }

        public override Color FillColor { get; set; }

        public override bool Is1D { get; protected set; }

        public override Color LineColor { get; set; }

        public override Dictionary<string, object> Parameters
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

        public override IPath Paths
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string type { get; }

        public override IFigure Clone(Dictionary<string, object> parms)
        {

            List<List<Segment>> NewPath = MakeNewMutant(path, parms["NewLeftDownCorner"], parms["NewRightUpCorner"]);
            double eps = (double)parms["Epsilon"];
            return new Mutant(NewPath, this.LineColor, this.FillColor, eps);
        }

        private List<List<Segment>> MakeNewMutant(List<List<Segment>> path, object v1, object v2)
        {
            throw new NotImplementedException();
        }

        public override void FillPaths()
        {
            throw new NotImplementedException();
        }

        public override Tuple<IEnumerable<trTriangle>, IEnumerable<ILineContainer>> NewTriangulation(double eps)
        {
            this.CreateTriangulation();//пока только для фигур без кривых линий!!!
            List<ILineContainer> toReturn = new List<ILineContainer>();

            foreach (List<Point> border in this._onlyPoints)
            {
                SpecPath toAdd = new SpecPath();
                toAdd.Path = border;
                toReturn.Add(toAdd);

            }
            return new Tuple<IEnumerable<trTriangle>, IEnumerable<ILineContainer>>(_triangles, toReturn);

        }

        public override IFigure Transform(ITransformation transform)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}



    public class Test : Figure
    {
        public Test()
        {
            _onlyPoints = new List<List<Point>>
            { new List<Point>()
            {
                new Point(2, 2),
                new Point(4, 5),
                new Point(6, 6),
                new Point(6, 2),
                new Point(9, 4),
                new Point(7, 8),
                new Point(3, 8),
                new Point(1, 5)
            }, 
            new List<Point>()
            {
                new Point(0, 0),
                new Point(0, 10),
                new Point(10, 10),
                new Point(10, 0)
            }
            };
            ConvexHull();
            CreateTriangulation();
    }
       
        #region интерфейс
        public override bool Colored
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

        public override Color FillColor
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

        public override bool Is1D
        {
            get
            {
                throw new NotImplementedException();
            }

            protected set
            {
                throw new NotImplementedException();
            }
        }

        public override Color LineColor
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

        public override Dictionary<string, object> Parameters
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

        public override IPath Paths
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string type
        {
            get
            {
                throw new NotImplementedException();
            }

        }

        public override IFigure Clone(Dictionary<string, object> parms)
        {
            throw new NotImplementedException();
        }

        public override void FillPaths()
        {
            throw new NotImplementedException();
        }

        public override Tuple<IEnumerable<trTriangle>, IEnumerable<ILineContainer>> NewTriangulation(double eps)
        {
            throw new NotImplementedException();
        }

        public override IFigure Transform(ITransformation transform)
        {
            throw new NotImplementedException();
        }
        #endregion
    }



