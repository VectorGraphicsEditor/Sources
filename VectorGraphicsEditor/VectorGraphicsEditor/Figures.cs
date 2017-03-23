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

            _figureBorder = new List<Point>()
            {
                new Point(UpRight.X, DownLeft.Y),
                UpRight,
                new Point(DownLeft.X, UpRight.Y),
                DownLeft
            };

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

        public override string type { get; set; }

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
            List<ILineContainer> ToReturn = new List<ILineContainer>() {path};
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

            _figureBorder = new List<Point>()
                {
                    new Point(Point1.X, Point1.Y),
                    new Point(Point2.X, Point2.Y),
                    new Point(Point3.X, Point3.Y)
                };

            _figureBorder = new List<Point>(_figureBorder);

            _triangles = new List<trTriangle>()
                {
                    new trTriangle(_figureBorder[0], _figureBorder[1], _figureBorder[2]),
                };

        }
        public override string type { get; set; }

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
            throw new NotImplementedException();
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
        public Mutant(List<Segment> path)
        {
            _figureBorder = new List<Point>();
            if (path.Count == 0)
            {
                //do nothing

            }
            else
            {
                Line temp = null;
                foreach (Segment segm in path)
                {
                    if (segm.Name == "Line")
                    {
                        temp = (Line)segm;
                        _figureBorder.Add(temp.Beg);
                    };
                }
                _figureBorder.Add(temp.End);
            }
        }


        #region интерфейса реализация
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

            set
            {
                throw new NotImplementedException();
            }
        }

        public override IFigure Clone(Dictionary<string, object> parms)
        {
            List<Segment> path = (List<Segment>)parms["Vertexes"];
            return new Mutant(path);
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
    }
    #endregion

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

            set
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

    public class TestMamykin : Figure
    {
        TestMamykin()
        {

        }
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

            set
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
    }
}


