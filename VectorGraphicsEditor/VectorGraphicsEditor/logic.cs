
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace Logic
{
    class logic : ILogicForGUI, ILogicForCommand
    {
        //Контейнер фигур
        private Container Figures;
        //Список индексов на фигуры, которые мы обрабатываем. По умолчанию последняя созданная фигура
        private List<int> CurientFigures;

        private int countFigures;

        logic()
        {
            Figures = new Container();
            CurientFigures = new List<int>();
            countFigures = 0;
        }

        IEnumerable<IFigure> ILogicForGUI.Figures
        {
            get
            {
                return Figures.getFigures();
            }
        }


         
        public void addCurientFigure(Interfaces.Point dot, bool add)
        {
            bool find = false;
            int index;
            for (index = countFigures - 1; index >= 0; index--)
            {
                //Проверяем лежит ли данная точка в этой фигуре
                //find = isPointInner(Figures.getFigure(index), dot)
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

        //Добавление новой фигуры
        void ILogicForCommand.addFigure(IFigure fig)
        { 
           countFigures = Figures.addNewFigure(fig);
        }
        
        //Удаление выбранных фигур
        void ILogicForCommand.removeFigures()
        {
            countFigures = Figures.removeFigures(CurientFigures);
        }
        
        Interfaces.Point ILogicForGUI.ToScreen(Interfaces.Point xy)
        {
            throw new NotImplementedException();
        }

        int ILogicForCommand.CountFigures 
        { 
            get 
            {
                return countFigures;
            } 
        }
        int ILogicForCommand.CountCurientFigures 
        {
            get
            {
                return CurientFigures.Count;
            }
        }
    }
}
