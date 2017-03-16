using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVGLib;
using Interfaces;

namespace IO
{
    public class SVGCircle : SVGShape
    {
        public Point center { get; private set; }
        public double r { get; private set; }
        public SVGCircle(Point center_, double r_)
        {
            center = center_;
            r = r_;
        }
        SvgBasicShape SVGShape.ToSVGLibShape(SvgDoc doc)
        {
            return new SvgCircle(doc,
                center.X.ToString()+"px",
                center.X.ToString()+"px",
                r.ToString() + "px"
            );
        }
    }
}
