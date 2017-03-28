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
    class Stack
    {
        int index;
        List<Tuple<Container,List<int>>> st;

        public Stack()
        {
            index = 1;
            st = new List<Tuple<Container, List<int>>>();
            st.Add(new Tuple<Container, List<int>>(new Container(), new List<int>()));
        }
        public void AddCommand(Container x, List<int> y)
        {
            st.RemoveRange(index, st.Count - index);
            List<int> newy = new List<int>(y);
            Container newx = x.Clone();
            st.Add(new Tuple<Container, List<int>>(newx, newy));
            index++;

        }
        public void StepForward()
        {
            index++;
        }
        public void StepBack()
        {
            index--;
        }
        public int Size()
        {
            return st.Count();
        }
        public int GetIndex()
        {
            return index;
        }
        public Tuple<Container, List<int>> GetState(int i)
        {
            return st[i];
        }

    }


}
