using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace test_editor
{

    using Interfaces;
    using Newtonsoft.Json;
    using NGeometry;


    public abstract partial class Figure : IFigure
    {
        const double ClosenessMeasure = 1e-12;//определяет меру лежания точки на прямой (через пс.скаляр. произв.)
        static int ClosenessCount = 12;//определяет число разрядов точности представления числа с плав.зпт.





        public List<trTriangle> _triangles { get; set; }
        public List<Point> _convexHull { get; set; }
        public List<Point> _figureBorder { get; set; }
        public List<List<Point>> _onlyPoints { get; set; }//списки связных границ в виде точек

        public abstract string type { get; }
        public abstract Dictionary<string, object> Parameters { get; set; }
        public abstract IPath Paths { get; }
        public abstract bool Colored { get; set; }
        public abstract Color FillColor { get; set; }
        public abstract Color LineColor { get; set; }
        public abstract bool Is1D { get; protected set; }

        public abstract void FillPaths();
        public abstract Tuple<IEnumerable<trTriangle>, IEnumerable<ILineContainer>> NewTriangulation(double eps);
        public abstract IFigure Clone(Dictionary<string, object> parms);
        //public abstract IFigure Transform(ITransformation transform);
        public IFigure Clone()
        {
            return Clone(Parameters);
            /*var serial = JsonConvert.SerializeObject(this);
            var a = JsonConvert.DeserializeObject(serial);
            return (this.type)a;*/

        }
    }
}


    
