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

        public List<IFigure> Figure()
        {
           
            {
                return FiguresList;
            }
        }
        
        public int addNewFigure(IFigure figure)
        {
            FiguresList.Add(figure);
            return FiguresList.Count;
        }

        public int removeFigures(List<int> index)
        {
            index.Sort();
            for (int i = index[index.Count - 1]; i >= 0; i--)
                FiguresList.RemoveAt(index[i]);
            return FiguresList.Count;
        }



        public int size()
        {
            return FiguresList.Count;
        }

    }
}
