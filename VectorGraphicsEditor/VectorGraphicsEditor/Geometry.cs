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





        protected List<trTriangle> _triangles { get; set; }
        protected List<Point> _convexHull { get; set; }
        protected List<Point> _figureBorder { get; set; }
        protected List<List<Point>> _onlyPoints { get; set; }

        public abstract string type { get; set; }
        public abstract Dictionary<string, object> Parameters { get; set; }
        public abstract IPath Paths { get; }
        public abstract bool Colored { get; set; }
        public abstract Color FillColor { get; set; }
        public abstract Color LineColor { get; set; }
        public abstract bool Is1D { get; protected set; }

        public abstract void FillPaths();
        public abstract Tuple<IEnumerable<trTriangle>, IEnumerable<ILineContainer>> NewTriangulation(double eps);
        public abstract IFigure Clone(Dictionary<string, object> parms);
        public abstract IFigure Transform(ITransformation transform);
    }
}


    