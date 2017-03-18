using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace Logic
{
    class logic : ILogicForGUI
    {
        //Контейнер фигур
        private Container Figures;
        //Список индексов на фигуры, которые мы обрабатываем. По умолчанию последняя созданная фигура
        private List<int> CurientFigures;

        IEnumerable<IFigure> ILogicForGUI.Figures
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        //Меняет изменяемую фигуру. хотелось бы от геометрии функцию, которая бы искала по списку фигур
        //ту, на которую указывает точка. Можно спросить у Андрея, она у меня есть)
        //если мы выбрали пустоту, то тогда мы выбираем последнюю созданную фигуру(возможно лучше просто
        //очистить массив и не давать прав ничего менять, пока не выберешь фигуру
        public void changeCurientFigure(Interfaces.Point dot)
        {
            int index = 0;
            //int index = FindFigure(dot, Figures.FiguresList.get);
            CurientFigures.Clear();
            if (index != -1)
                CurientFigures.Add(index);
            else
                CurientFigures.Add(Figures.size());
        }

        //тоже самое но добавление к уже имеющимся элементам. 
        public void addCurientFigure(Interfaces.Point dot)
        {
            int index = 0;
           // int index = FindFigure(dot, Figures.FiguresList.get);
            if (index != - 1)
                CurientFigures.Add(index);
        }

        //Добавление новой фигуры
        public void addFigure(Interfaces.Point beg, Interfaces.Point end, int type)
        {
            //IFigure Fig = MakeFigure(Point beg, Point end, int type);
           // Figures.addNewFigure(Fig);
        }

        // 1. Так как для треугольника точек начала и конца не хватит, то предлагаю передавать их через массив.
        // 2. Также наверно стоит возвращать успешность операции. 
        public bool addFigure(Interfaces.Point[] points, string type)
        {
            // TODO
            return true;
        }

        //Удаление выбранных фигур
        public void removeFigures()
        {
            Figures.removeFigures(CurientFigures);
        }
        

        public void executeCommand(ICommand p)
        {
            throw new NotImplementedException();
        }

        Interfaces.Point ILogicForGUI.ToScreen(Interfaces.Point xy)
        {
            throw new NotImplementedException();
        }
    }
}
