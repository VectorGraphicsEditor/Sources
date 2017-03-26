
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using VectorGraphicsEditor;

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
        }
        
        Interfaces.Point ILogicForGUI.ToScreen(Interfaces.Point xy)
        {
            throw new NotImplementedException();
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
    }
}
