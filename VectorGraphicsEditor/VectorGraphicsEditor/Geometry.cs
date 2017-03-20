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
                    oi = CNINT(first_tree.value, second_tree)
                    //если пересечение пусто
                    if(oi == null)
                        return null
                    иначе делать текст ниже:
               
                List<Tree<List<Line>>> Child_List = null;
                Child_List = oi.localChildren;
                foreach (Tree<List<Line>> Elem in Child_List) //обход дерева
                {

                    //local_intersection = CNINT ( 
                    //...
                }
              */
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
    public Tree<List<Line>> CNINT(Tree<List<Line>> poly1, List<Line> c_poly2)//обход по всему дереву пойдет, если узел не имеет пересечений, его потомкам не нужно искать пересечение, имхо
    {
        Tree<List<Line>> Res = null;
        //Res = new Tree<List<Line>>( CCINT(poly1.value,c_poly2) )// пересечение узла и c_poly2 как 2 выпуклых фигур
        if(poly1.HasChildren == false)
        {
            /*
            return (Res);
            */
        }
        /*
        List<Tree<List<Line>>> Child_List = null;
        Child_List = poly1.localChildren;
        foreach (Tree<List<Line>> Child in Child_List) //обход дерева с заменой узлов на пересечения
        {
            Tree<List<Line>> node = CNINT(Child,c_poly2)
            //Поставить node на место Child в имходном дереве// проверить, в шарпе ссылка(хорошо) или копирование(плохо).
        }
        */
    }
    public List<Line> CCINT(List<Line> c_poly1,List<Line> c_poly2)
    {
        /* Спросить у Паши уже функцию Convex Hull, я даже не знаю, что на выходе, точки или грани.
        List<Line> CH = ... // получить выпуклую оболочку для c_poly1 + c_poly2
        List<Line> DE1 = null;
        List<Line> DE2 = null;
        foreach(Line CH_Edge in CH)
        {
            foreach(Line Poly1_Edge in c_poly1) //овнокод?
            {
                if ( Poly1_Edge != CH_Edge)
                {
                    DE1.Add(Poly1_Edge)
                }
            }
            foreach((Line Poly1_Edge in c_poly2)
            {
                if ( Poly2_Edge != CH_Edge)
                {
                    DE2.Add(Poly2_Edge)
                }
            }
        }
        //нашли все Delta Edges. Найдем точки их пересечения, точки poly1, которые внутри poly 2 и наоборот. Совокупность точек дает CH, который явл. резултатом пересечения.
        List<Point> result = null;
        foreach(Line D_Edge1 in DE1)
        {
            foreach(Line D_Edge2 in DE2)
            {
                result.Add(getIntersectionPoints(D_Edge1.Beg,D_Edge1.End,D_Edge2.Beg,D_Edge2.End);
            }
        }
        foreach(Line D_Edge1 in DE1) // пройтись по всем точкам DE1, добавить, те, Которые внутри c_poly2
        {
            if(..)
            resutl.Add(..)
        }
        foreach(Line D_Edge2 in DE2) // пройтись по всем точкам DE2, добавить, те, Которые внутри c_poly1
        {
            if(..)
            resutl.Add(..)
        }
        */
    }
}


    
