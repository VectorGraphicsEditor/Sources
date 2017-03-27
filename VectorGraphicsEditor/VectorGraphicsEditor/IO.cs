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
        public SVGCircle(Point center_, double r_, Color fill_, Color stroke_, int w_ = 1)
        {
            center = center_;
            r = r_;
            w = w_;
            fill = fill_;
            stroke = stroke_;
        }
        public override SvgBasicShape ToSVGLibShape(SvgDoc doc)
        {
            var res = new SvgCircle(doc,
                center.X.ToString()+"px",
                center.X.ToString()+"px",
                r.ToString() + "px"
            );
            res.Fill = System.Drawing.Color.FromArgb(fill.A, fill.R, fill.G, fill.B);
            res.Stroke = System.Drawing.Color.FromArgb(stroke.A, stroke.R, stroke.G, stroke.B);
            res.StrokeWidth = w.ToString() + "px";
            return res;
        }
    }

    public class SVGEllipse : SVGShape
    {
        public Point center { get; private set; }
        public double rx { get; private set; }
        public double ry { get; private set; }

        public SVGEllipse(Point center_, double rx_, double ry_, Color fill_, Color stroke_, int w_ = 1)
        {
            center = center_;
            rx = rx_;
            ry = ry_;
            w = w_;
            fill = fill_;
            stroke = stroke_;
        }
        public override SvgBasicShape ToSVGLibShape(SvgDoc doc)
        {
                var res = new SvgEllipse(doc,
                center.X.ToString() + "px",
                center.X.ToString() + "px",
                rx.ToString() + "px",
                ry.ToString() + "px"
            );
            res.Fill = System.Drawing.Color.FromArgb(fill.A, fill.R, fill.G, fill.B);
            res.Stroke = System.Drawing.Color.FromArgb(stroke.A, stroke.R, stroke.G, stroke.B);
            res.StrokeWidth = w.ToString() + "px";
            return res;
        }
    }

    public class SVGPolygon : SVGShape           
    {
        public List<Point> points { get; private set; }
        public SVGPolygon(List<Point> points_, Color fill_, Color stroke_, int w_ = 1)
        {
            points = points_;
            w = w_;
            fill = fill_;
            stroke = stroke_;
        }
        public override SvgBasicShape ToSVGLibShape(SvgDoc doc)
        {
            String pointsStr = "";
            foreach(var point in points) {
                pointsStr += point.X + "," + point.Y + " ";
            }

                var res = new SvgPolygon(doc,
                pointsStr
            );
            res.Fill = System.Drawing.Color.FromArgb(fill.A, fill.R, fill.G, fill.B);
            res.Stroke = System.Drawing.Color.FromArgb(stroke.A, stroke.R, stroke.G, stroke.B);
            res.StrokeWidth = w.ToString() + "px";
            return res;
        }
    }

    public class SVGRect : SVGShape
    {
        public double rx { get; private set; }
        public double ry { get; private set; }
        public double width { get; private set; }
        public double height { get; private set; }
        public SVGRect(double rx_, double ry_, double width_, double height_, Color fill_, Color stroke_, int w_ = 1)
        {
            width = width_;
            height = height_;
            rx = rx_;
            ry = ry_;
            w = w_;
            fill = fill_;
            stroke = stroke_;
        }
        public override SvgBasicShape ToSVGLibShape(SvgDoc doc)
        {
                var res = new SvgRect(doc,
                rx.ToString() + "px",
                ry.ToString() + "px",
                width.ToString() + "px",
                height.ToString() + "px",
                w.ToString() + "px",
                System.Drawing.Color.FromArgb(fill.A, fill.R, fill.G, fill.B),
                System.Drawing.Color.FromArgb(stroke.A, stroke.R, stroke.G, stroke.B)
            );
            return res;
        }
    }

    public class SVGPath : SVGShape
    {
        public List<Point> pathdata { get; private set; }
        public SVGPath(List<Point> pathdata_, Color fill_, Color stroke_, int w_ = 1)
        {
            pathdata = pathdata_;
            w = w_;
            fill = fill_;
            stroke = stroke_;
        }
        public override SvgBasicShape ToSVGLibShape(SvgDoc doc)
        {
            String pathdataStr = "";
            foreach (var point in pathdata)
            {
                pathdataStr += point.X + "," + point.Y + " ";
            }

            var res = new SvgPolygon(doc,
            pathdataStr
        );
            res.Fill = System.Drawing.Color.FromArgb(fill.A, fill.R, fill.G, fill.B);
            res.Stroke = System.Drawing.Color.FromArgb(stroke.A, stroke.R, stroke.G, stroke.B);
            res.StrokeWidth = w.ToString() + "px";
            return res;
        }
    }
}
