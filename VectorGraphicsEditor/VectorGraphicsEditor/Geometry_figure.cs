using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace test_editor
{

    using Interfaces;
    using tree_class;
    using NGeometry;
    public abstract partial class Figure : IFigure
    {
        private List<Point> Build_Convex_Hull(List<Line> pe) //polytope edges. 
        {
            List<Point> result = null;
            //алгоритм для выпуклой оболочки
            return (result);
        }
        public Tree<List<Line>> Build_CHT(List<Line> pe) // Построить дерево выпуклых фигур.
        {
            Tree<List<Line>> CHT = null; // голова дерева
            if (pe == null) // пустая фигура
            {
                return CHT;
            }
            if (pe.Capacity == 1) // одно ребро. - нет //AHTUNG! Замыкание DR плохое, когда CHT_NODE = 1 отрезок.
            {
                CHT = new Tree<List<Line>>(pe);
                return CHT;
            }
            else
            {
                //List<Point> CH = this.Build_Convex_Hull(pe); //convex hull 
                //получаем che - convex hull edges(принадлежат CH).DONE

                //получаем dre - delta regions edges(не принадлежат CH).DONE

                //получаем dhp - delta&hull points - точки из dre, которые лежат на CH. DONE

                //ищем dr - отдельные наборы граней из dre. Включаем из dre пока не замкнемся,
                //либо пока не встретим новую точку из dhp. Тогда добавляем dr[i]. DONE

                //*Проходя вычеркиваем из dhp, dre пройденные точки, грани соответственно - ПРОВЕРЬ УПОРЯДОЧЕННОСТЬ ГРАНЕЙ*
                //Бежим по dre пока не обойдем все грани.
                List<Line> CHE = null, DRE = null;
                List<Point> DHP = null;
                List<List<Line>> DR = null;

                //che
                //Тут подпрограмма должна быть:
                Point P_prev = _convexHull[0];
                Line Line_to_add = null;
                foreach (Point P_next in _convexHull)
                {
                    if (P_next == _convexHull[0])
                        continue;
                    Line_to_add = new Line(P_prev, P_next);
                    P_prev = P_next;
                    CHE.Add(Line_to_add);
                }
                //.
                //dre
                foreach (Line L1 in pe)
                {
                    foreach (Line L2 in CHE)
                    {
                        if (L1 == L2) // найти несовпадающие и записать в DRE
                            continue;
                        else
                        {
                            //Line to_add = new Line(L1.Beg, L1.End);
                            DRE.Add(L1);
                        }
                    }
                    //замыкаем дельта регион
                    Line to_close = new Line(DRE[0].Beg, DRE[DRE.Count - 1].End);
                    DRE.Add(to_close);

                }
                //dhp
                Point Point_to_add = new Point();
                foreach (Line L1 in DRE)
                {
                    foreach (Line L2 in CHE)
                    {
                        Point_to_add = getLinesIntersect(L1.Beg, L1.End, L2.Beg, L2.End);
                    }
                    if (Point_to_add != null)
                    {
                        DHP.Add(Point_to_add);
                    }
                }
                //dr
                List<Line> DR_to_add = null;
                foreach (Line L1 in DRE)
                {
                    DR_to_add.Add(L1);
                    foreach (Point P in DHP)
                    {
                        if (L1.End == P)
                        {
                            DR.Add(DR_to_add);
                            DR_to_add = null;
                        }
                    }
                }
                //.
                //List<Line> DR_i = null;
                foreach (List<Line> DR_i in DR)
                {
                    Tree<List<Line>> node = null;

                    node = this.Build_CHT(DR_i);
                    CHT.Add(node);
                }
            }
            return CHT;
        }
        public List<Line> Build_figure_from_CHT(Tree<List<Line>> CHT)
        {
            List<Line> Polytope_edges = null;
            if (CHT != null)
            {
                Polytope_edges = CHT.Value;
                if (CHT.HasChildren == false) // если фигура выпуклая
                {
                    //Polytope_edges = CHT.Value;
                    return Polytope_edges;
                }
                List<Tree<List<Line>>> Child_List = null;
                Child_List = CHT.localChildren;

                List<Line> region_two_edges = null;
                foreach (Tree<List<Line>> Elem in Child_List) //обход дерева
                {
                    region_two_edges = (Build_figure_from_CHT(Elem));
                    // merge
                    foreach (Line L1 in Polytope_edges)
                    {
                        foreach (Line L2 in region_two_edges)
                        {
                            //заменить общее ребро дельта регионом без этого ребра
                            if (L1 == L2)
                            {
                                Polytope_edges.Remove(L2);
                                region_two_edges.Remove(L2);
                                Polytope_edges.AddRange(region_two_edges);
                                break;
                            }
                        }
                    }
                    return (Polytope_edges);
                }
            }
            return null;
        }

        public Tree<List<Line>> CNINT(Tree<List<Line>> poly1, List<Line> c_poly2)//обход по всему дереву пойдет, если узел не имеет пересечений, его потомкам не нужно искать пересечение, имхо
        {
            Tree<List<Line>> Res = null;
            //Res = new Tree<List<Line>>( CCINT(poly1.value,c_poly2) )// пересечение узла и c_poly2 как 2 выпуклых фигур
            if (poly1.HasChildren == false)
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
            return null;
        }
        public List<Line> CCINT(List<Line> c_poly1, List<Line> c_poly2)
        {
            /* Спросить у Паши уже функцию Convex Hull, я даже не знаю, что на выходе, точки или грани.
            List<Line> CH = ... // получить выпуклую оболочку для c_poly1 + c_poly2      (_convexHull -> list<lines>
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
            return null;
        }
    }
    class Geometry : IGeometryForLogic
    {
        IFigure IGeometryForLogic.Intersection(IFigure first, IFigure second)
        {
            //как-то приходим к List<Line> и проч. пока не знаю как.
            IFigure result = null;

            List<Line> pe = null;
            //если результат build_CHT - нельзя определить выпуклую оболочку, тогда возвращать null. Для арок, будет null, видимо.
            Tree<List<Line>> first_tree = first.Build_CHT(pe);
            Tree<List<Line>> second_tree = second.Build_CHT(pe);
            if (first_tree != null & second_tree != null)
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
    
}
