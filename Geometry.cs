using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace test_editor
{

    using Interfaces;
    using NGeometry;


    public abstract partial class Figure : IFigure
    {
        const double ClosenessMeasure = 1e-12;//определяет меру лежания точки на прямой (через пс.скаляр. произв.)
        static int ClosenessCount = 12;//определяет число разрядов точности представления числа с плав.зпт.

        protected bool _colored;
        protected bool _is1d;
        protected Color _currentColor;

        protected List<Triangle> _triangles;

        protected List<Point> _convexHull;
        protected List<Point> _figureBorder;

        public abstract Tuple<IEnumerable<Triangle>, IEnumerable<ILineContainer>> NewTriangulation(double eps);


        /*    public Figure(string type, ref List<Point> guiPoints)
            {
                _pointsFromGUI = guiPoints;
                _type = type;
                _is1d = false;
                _triangles = new List<Triangle>();
                _figureBorder = new List<Point>() {
    new Point (2, 2),new Point(4, 5), new Point(5, 28.0/5), new Point (6, 2), new Point(7, 4), new Point(9, 4), new Point(7, 8), new Point(3, 8),  new Point(1, 5)};
                //
                //CreatePaths();
                if (!_is1d) CreateTriangulation();
            }*/
    }
}


    