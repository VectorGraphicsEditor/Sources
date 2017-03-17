using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace VectorGraphicsEditor
{

    using Interfaces;
    using NGeometry;
    using tree_class;

    public abstract partial class Figure : IFigure
    {
        const double ClosenessMeasure = 1e-12;//определяет меру лежания точки на прямой (через пс.скаляр. произв.)
        static int ClosenessCount = 12;//определяет число разрядов точности представления числа с плав.зпт.

        protected bool _colored { get; set; }
        #region реализация интерфейса
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

        public Dictionary<string, object> Parameters
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

        public IPath Paths
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Colored
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

        public Color LineColor
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
                throw new NotImplementedException();
            }
        }
        #endregion
        protected bool _is1d;
        protected Color _currentColor { get; set; }

        protected List<Triangle> _triangles { get; set; }

        protected List<Point> _convexHull { get; set; }
        protected List<Point> _figureBorder { get; set; }

        public abstract Tuple<IEnumerable<Triangle>, IEnumerable<ILineContainer>> NewTriangulation(double eps);

        public void FillPaths()
        {
            throw new NotImplementedException();
        }

        public IFigure Clone()
        {
            throw new NotImplementedException();
        }

        public IFigure Create(Dictionary<string, object> parms)
        {
            throw new NotImplementedException();
        }

        public IFigure Transform(ITransformation transform)
        {
            throw new NotImplementedException();
        }

        public abstract IFigure Clone(Dictionary<string, object> parms);

        public bool IsPointInner(Point point)
        {
            throw new NotImplementedException();
        }


        /*    public Figure(string type, ref List<Point> guiPoints)
            {
                _pointsFromGUI = guiPoints;
                _type = type;
                _is1d = false;
                _triangles = new List<Triangle>();
                _figureBorder = new List<Point>() {
    new Point (2, 2),new Point(4, 5), new Point(5, 28.0/5), new Point (6, 2), new Point(7, 4), new Point(9, 4), new Point(7, 8), new Point(3, 8),  new Point(1, 5)};
                //
                //CreatePaths();
                if (!_is1d) CreateTriangulation();
            }*/
    }

    class Geometry : IGeometryForLogic
    {
        IFigure IGeometryForLogic.Intersection(IFigure first, IFigure second)
        {
            //как-то приходим к List<Line> и проч. пока не знаю как.
            IFigure result = null;

            List<Line> pe = null; 
            //если результат build_CHT - нельзя определить выпуклую оболочку, тогда возвращать null. Для арок, будет null, видимо.
            Tree<List<Line>> first_tree =  first.Build_CHT(pe);
            Tree<List<Line>> second_tree = second.Build_CHT(pe);
            if( first_tree != null & second_tree != null)
            {
                if (first_tree.HasChildren == false)
                {
                    //CNINT (first_tree.value, second_tree ); // first - выпуклая, second - дерево выпуклых
                    //return
                }
                if (second_tree.HasChildren == false)
                {
                    //CNINT (second_tree.value, first_tree ); // наоборот
                    //return
                }
                //обе фигуры невыпуклые
                /*
                    //проверка на пересечение CNINT first.value и second_tree
                    если пересечение пусто
                    return null
                    иначе делать текст ниже:
                */
                List<Tree<List<Line>>> Child_List = null;
                Child_List = first_tree.localChildren;
                foreach (Tree<List<Line>> Elem in Child_List) //обход дерева
                {

                    //local_intersection = CNINT ( 
                }
            }
            return result;
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


    