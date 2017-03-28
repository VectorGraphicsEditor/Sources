using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace Logic
{
    public class Container
    {
        private List<IFigure> FiguresList;

        public Container()
        {
            FiguresList = new List<IFigure>();
        }

        // Колличество элементов
        public int Count
        {
            get
            {
                return FiguresList.Count;
            }
        }

        // Возврашает лист фигур
        public List<IFigure> getFigures()
        {
             return FiguresList;
        }

        // Возвращает фигуру по индексу
        public IFigure getFigure(int index)
        {
            return FiguresList[index];
        }

        // Добавление фигуры в конец массива
        public int addNewFigure(IFigure figure)
        {
            FiguresList.Add(figure);
            return FiguresList.Count;
        }

        // удаление фигур с заданными индексами причем если список пуст, то он удалит последний
        public int removeFigures(List<int> index)
        {
            if (index.Count > 0)
            {
                index.Sort();
                for (int i = index.Count - 1; i >= 0; i--)
                    FiguresList.RemoveAt(index[i]);
                return FiguresList.Count;
            }
            else
            {
                FiguresList.RemoveAt(FiguresList.Count - 1);
                return FiguresList.Count;
            }
        }

        public void clear()
        {
            FiguresList.Clear();
        }

        // в шарпах нет реализации swap или я не умею в гугл?
        // ну в общем мы меняем местами в списке положение элементов
        // может этого нельзя делать?
        public void swap(int firstindex, int secondindex)
        {
            IFigure buf = FiguresList[firstindex];
            FiguresList[firstindex] = FiguresList[secondindex];
            FiguresList[secondindex] = buf;
        }

        public Container Clone()
        {
            Container buf = new Container();
            for (int i = 0; i < Count; i++)
            {
                buf.addNewFigure(FiguresList[i].Clone());
            }
            return buf;
        }
    }
}
