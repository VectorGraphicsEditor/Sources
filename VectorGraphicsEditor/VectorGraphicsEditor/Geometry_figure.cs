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
                List<Point> CH = this.Build_Convex_Hull(pe); //convex hull
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
                Point P_prev = CH[0];
                Line Line_to_add = null;
                foreach (Point P_next in CH)
                {
                    if (P_next == CH[0])
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
    }
}
