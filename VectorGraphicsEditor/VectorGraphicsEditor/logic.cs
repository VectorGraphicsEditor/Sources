
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using VectorGraphicsEditor;
using IO;

namespace Logic
{
    public class logic : ILogicForGUI, ILogicForCommand
    {
        //Контейнер фигур
        private Container Figures;
        //Список индексов на фигуры, которые мы обрабатываем. По умолчанию последняя созданная фигура
        private List<int> CurientFigures;
        //Стек состояний
        private Stack StackSituation;

        public logic()
        {
            Figures = new Container();
            CurientFigures = new List<int>();
            StackSituation = new Stack();
        }


        // возвращает список фигур
        IEnumerable<IFigure> ILogicForGUI.Figures
        {
            get
            {
                return Figures.getFigures();
            }
        }

        List<int> ILogicForGUI.CurientPickFigures
        {
            get
            {
                return CurientFigures;
            }
        }

        // выделение фигуры мышкой добавляет в список выделенных фигур. если add - true. При этом можно 
        // начать заново выделять, если add - false
        void ILogicForCommand.addCurientFigure(Interfaces.Point dot, bool add)
        {
            bool find = false;
            int index;
            for (index = Figures.Count - 1; index >= 0; index--)
            {
                //Проверяем лежит ли данная точка в этой фигуре
                find = Figures.getFigure(index).IsPointInner(dot);
                //Если нашли
                if (find)
                    //если надо добавить
                    if (add)
                    {
                        //и текущей фигуры нет в списке элементов
                        if (CurientFigures.IndexOf(index) < 0)
                        {
                            //то выходим из цикла, или продолжаем поиски
                            break;
                        }
                    //иначе выходим    
                    }
                    else
                    {
                        break;
                    }
            }
            if (find)
            {
                if (add)
                {
                    CurientFigures.Add(index);
                }
                else
                {
                    CurientFigures.Clear();
                    CurientFigures.Add(index);
                }
            }
            else
            {
                if (!add)
                {
                    CurientFigures.Clear();
                }
            }
        }

        void ILogicForCommand.addCurientFigureWithIndex(int index, bool add)
        {
            //если данной фигуры нет
            if (CurientFigures.IndexOf(index) < 0)
            {
                //если надо добавить
                if (add)
                {
                    CurientFigures.Add(index);
                }
                // если не надо добавлять
                else
                {
                    CurientFigures.Clear();
                    CurientFigures.Add(index);
                }
            }
            // если она есть, то ее надо удалить из списка
            else
            {
                CurientFigures.Remove(index);
            }
        }

        // Передвижение индекса: если выделена одна фигура, то мы перемещаемся по элементам начиная с
        // того на котором мы находимся, если там нет выделенных фигур, то шагаем от начала списка или
        // от конца. Предусмотренно циклическое передвижение. не знаю зачем)
        void ILogicForCommand.moveCurientIndex(bool direction)
        {
            if (CurientFigures.Count == 0)
            {
                if (direction)
                {
                    CurientFigures.Add(0);
                }
                else
                {
                    CurientFigures.Add(Figures.Count);
                }
            }
            if (CurientFigures.Count == 1)
            {
                if (direction)
                {
                    if (CurientFigures[0] == Figures.Count - 1)
                    {
                        CurientFigures[0] = 0;
                    }
                    else
                    {
                        CurientFigures[0] += 1;
                    }
                }
                else
                {
                    if (CurientFigures[0] == 0)
                    {
                        CurientFigures[0] = Figures.Count - 1;
                    }
                    else
                    {
                        CurientFigures[0] -= 1;
                    }
                }
            }
        }

        // перемещение фигур относительно "z координаты". у нас же типа слои.
        void ILogicForCommand.moveIndexFigure(bool direction)
        {
            StackSituation.AddCommand(Figures, CurientFigures);
            if (direction)
            {
                Figures.swap(CurientFigures[0], CurientFigures[0] + 1);
            }
            else
            {
                Figures.swap(CurientFigures[0] - 1, CurientFigures[0]);
            }
        }

        //Добавление новой фигуры
        void ILogicForCommand.addFigure(IFigure fig)
        {
           StackSituation.AddCommand(Figures, CurientFigures);
           Figures.addNewFigure(fig);
        }
        
        //Удаление выбранных фигур
        void ILogicForCommand.removeFigures()
        {
            StackSituation.AddCommand(Figures, CurientFigures);
            Figures.removeFigures(CurientFigures);
            CurientFigures.Clear();
        }

        void ILogicForCommand.editColor(Interfaces.Color newcolor)
        {
            foreach (int index in CurientFigures)
            {
                // ВОТ ТУТ Я ВООБЩЕ НЕ УВЕРЕН
                Figures.getFigure(index).FillColor = newcolor;
            }
        }

        void ILogicForCommand.editBorderColor(Interfaces.Color newcolor)
        {
            foreach (int index in CurientFigures)
            {
                // ВОТ ТУТ Я ВООБЩЕ НЕ УВЕРЕН
                Figures.getFigure(index).LineColor = newcolor;
            }
        }


        void ILogicForCommand.save(string filename)
        {
            List<SVGShape> ShapeList = new List<SVGShape>();
            IFigure buf;
            for (int i = 0; i < Figures.Count; i++)
            {
                buf = Figures.getFigure(i);
                Tuple<IEnumerable<trTriangle>, IEnumerable<ILineContainer>> triangulation;
                triangulation = buf.NewTriangulation(1.0);
                if (buf is VectorGraphicsEditor.Rectangle)
                {
                    List<Interfaces.Point> points = (List<Interfaces.Point>)triangulation.Item2;
                    Interfaces.Point min = findMinPoint(points);
                    Interfaces.Point max = findMaxPoint(points);
                    double xh, yh;
                    xh = (max.X - min.X);
                    yh = (max.Y - min.Y);
                    ShapeList.Add(new SVGRect(min.X + xh / 2.0, min.Y + yh / 2.0, xh, yh, buf.FillColor, buf.LineColor));
                }
                if (buf is VectorGraphicsEditor.Triangle)
                {
                    ShapeList.Add(new SVGPolygon((List<Interfaces.Point>)triangulation.Item2, buf.FillColor, buf.LineColor));
                }
                if (buf is VectorGraphicsEditor.Mutant)
                {
                    ShapeList.Add(new SVGPolygon((List<Interfaces.Point>)triangulation.Item2, buf.FillColor, buf.LineColor));
                }
            }
            SVGIO.export(ShapeList, filename, 800, 600);
        }

        void ILogicForCommand.load(string filename)
        {
            Tuple<List<SVGShape>, int, int> fromfile = SVGIO.import(filename);
            Figures.clear();
            CurientFigures.Clear();
            foreach (SVGShape item in fromfile.Item1)
            {
                if (item is SVGCircle)
                {

                }
                if (item is SVGEllipse)
                {

                }
                if (item is SVGRect)
                {
                    SVGRect fig = (SVGRect)item;
                    Interfaces.Point downleft = new Interfaces.Point(fig.rx - fig.width / 2.0, fig.ry - fig.height / 2.0);
                    Interfaces.Point upright = new Interfaces.Point(fig.rx + fig.width / 2.0, fig.ry + fig.height / 2.0);
                    Figures.addNewFigure(new VectorGraphicsEditor.Rectangle(downleft, upright, fig.stroke, fig.fill)
                }
                if (item is SVGPolygon)
                {
                    SVGPolygon fig = (SVGPolygon)item;
                    if (fig.points.Count == 3)
                    {
                        Figures.addNewFigure(new VectorGraphicsEditor.Triangle(fig.points[0], fig.points[1], fig.points[2], fig.stroke, fig.fill));
                    }
                    else
                    {
                        List<List<Segment>> list = new List<List<Segment>>();
                        list.Add(PointToSegment(fig.points));
                        Figures.addNewFigure(new VectorGraphicsEditor.Mutant(list, fig.stroke, fig.fill, 1.0));
                    }
                }
                if (item is SVGPath)
                {

                }
            }
        }

        Interfaces.Point findMaxPoint(List<Interfaces.Point> list)
        {
            Interfaces.Point max = list[0];
            foreach (Interfaces.Point item in list)
            {
                if (max.X <= item.X || max.Y <= item.Y)
                    max = item;
            }
            return max;
        }

        Interfaces.Point findMinPoint(List<Interfaces.Point> list)
        {
            Interfaces.Point min = list[0];
            foreach (Interfaces.Point item in list)
            {
                if (min.X > item.X || min.Y > item.Y)
                    min = item;
            }
            return min;
        }

        List<Segment> PointToSegment(List<Interfaces.Point> dots)
        {
            List<Segment> res = new List<Segment>();
            Line buf;
            for (int i = 0; i < dots.Count - 1; i++)
            {
                buf = new Line(dots[i], dots[i + 1]);
                res.Add(buf);
            }
            buf = new Line(dots[dots.Count - 1], dots[0]);
            res.Add(buf);
            return res;
        }

        // Количество фигур
        int ILogicForCommand.CountFigures 
        { 
            get 
            {
                return Figures.Count;
            } 
        }

        // Количество выделенных фигур
        int ILogicForCommand.CountCurientFigures 
        {
            get
            {
                return CurientFigures.Count;
            }
        }

        // Возвращает индекс первого элемента выбранных фигур.
        // Иногда приходится работать с одним выбранным элементом, тогда это и пригождается
        int ILogicForCommand.IndexCurientElem
        {
            get
            {
                return CurientFigures[0];
            }
        }

        //Возвращает актуальный индекс стека состояний
        int ILogicForCommand.GetStackIndex()
        {
            return StackSituation.GetIndex();
        }
        //Возвращает размер стека состояний
        int ILogicForCommand.GetStackCount()
        {
            return StackSituation.Size();
        }
        //Откатывает состояние контейнера фигур и списока индексов обрабатываемых фигур 
        //До предыдущего состояния
        void ILogicForCommand.SetPreviousStackState()
        {
            StackSituation.StepBack();
            Figures = StackSituation.GetState(StackSituation.GetIndex()).Item1;
            CurientFigures = StackSituation.GetState(StackSituation.GetIndex()).Item2;
        }
        //Устонавливает состояние контейнера фигур и списока индексов обрабатываемых фигур 
        //До предыдущего состояния
        void ILogicForCommand.SetNextStackState()
        {
            StackSituation.StepForward();
            Figures = StackSituation.GetState(StackSituation.GetIndex()).Item1;
            CurientFigures = StackSituation.GetState(StackSituation.GetIndex()).Item2;
        }

        void ILogicForCommand.PutIntoBuffer()
        {
            FiguresBuffer = new List<IFigure>(CurientFigures.Count);
            foreach (var i in CurientFigures)
                FiguresBuffer.Add(Figures.getFigure(i).Clone());
        }

        int ILogicForCommand.BufferSize
        {
            get { return (FiguresBuffer != null) ? FiguresBuffer.Count : 0; }
        }

        void ILogicForCommand.PushBuffer()
        {
            foreach (var f in FiguresBuffer)
            {
                Figures.addNewFigure(f.Clone());
            }
        }
    }
}
