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

        public List<IFigure> getFigures()
        {
             return FiguresList;
        }

        public IFigure getFigure(int index)
        {
            return FiguresList[index];
        }

        public int addNewFigure(IFigure figure)
        {
            FiguresList.Add(figure);
            return FiguresList.Count;
        }

        public int removeFigures(List<int> index)
        {
            if (index.Count > 0)
            {
                index.Sort();
                for (int i = index[index.Count - 1]; i >= 0; i--)
                    FiguresList.RemoveAt(index[i]);
                return FiguresList.Count;
            }
            else
            {
                FiguresList.RemoveAt(FiguresList.Count - 1);
                return FiguresList.Count;
            }
        }


        public int size()
        {
            return FiguresList.Count;
        }

    }
}
