using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace test_editor
{
    public abstract partial class Figure : IFigure
    {
        private void IntersectSpliting(Figure figure1, Figure figure2, out List<trTriangle> trs_in_f1,
            out List<trTriangle> trs_in_f2, out List<trTriangle> trs_part_in_f1,
            out List<trTriangle> trs_part_in_f2)
        {
            trs_in_f1 = new List<trTriangle>(); 
            trs_in_f2 = new List<trTriangle>(); 
            trs_part_in_f1 = new List<trTriangle>(); 
            trs_part_in_f2 = new List<trTriangle>(); 

            foreach (var triangle in figure1._triangles)
            {
                int k = 0;
                if (figure2.IsPointInner(triangle.A)) k++;
                if (figure2.IsPointInner(triangle.B)) k++;
                if (figure2.IsPointInner(triangle.C)) k++;
                if (figure2.IsPointInner(triangle.GetCenter())) k++;

                if (k == 4) trs_in_f2.Add(triangle);
                else if (k > 0) trs_part_in_f2.Add(triangle);
            }

            foreach (var triangle in figure2._triangles)
            {
                int k = 0;
                if (figure1.IsPointInner(triangle.A)) k++;
                if (figure1.IsPointInner(triangle.B)) k++;
                if (figure1.IsPointInner(triangle.C)) k++;
                if (figure1.IsPointInner(triangle.GetCenter())) k++;

                if (k == 4) trs_in_f1.Add(triangle);
                else if (k > 0) trs_part_in_f1.Add(triangle);
            }
        }


    }
}
