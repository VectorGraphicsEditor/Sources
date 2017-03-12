using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test_editor;

namespace VectorGraphicsEditor
{

    class Rectangle : Figure
    {
        Rectangle(Point DownLeft, Point UpRight, Color BorderColor, Color FillColor)
        {
            _colored = true;
            _is1d = false;
            _currentColor = FillColor;
            _figureBorder = new List<Point>()
            {
                new Point(UpRight.X, DownLeft.Y),
                UpRight,
                new Point(UpRight.Y, DownLeft.X),
                DownLeft
            };

            _figureBorder = new List<Point>(_figureBorder);

             _triangles = new List<Triangle>()
            {
                new Triangle(_figureBorder[0], _figureBorder[1], _figureBorder[2]),
                new Triangle(_figureBorder[2], _figureBorder[3], _figureBorder[0])
            };

        }

        public override Tuple<IEnumerable<Triangle>, IEnumerable<ILineContainer>> NewTriangulation(double eps)
        {
            throw new NotImplementedException();
        }
    }
}

