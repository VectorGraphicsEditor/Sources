using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectorGraphicsEditor;
using Interfaces;
using IO;

namespace IO_Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var circle = new SVGCircle(new Point(250.0, 550.0), 20.0, 
                new Color(100,255,56,0),
                new Color(0, 0, 0, 0),
                2
            );

            var ellipse = new SVGEllipse(new Point(450.0, 550.0), 20.0, 10.0,
                new Color(100, 255, 56, 0),
                new Color(0, 0, 0, 0),
                2
            );
            
            var points = new List<Point>() { new Point(70, 5), new Point(90, 41), new Point(136, 48),
            new Point(103,80), new Point(111,126), new Point(70,105), new Point(29,126), new Point(36,80), new Point(5,48), new Point(48,41)};

            var pathdata = new List<Point>() { new Point(200, 100), new Point(250, 210), new Point(16, 210) };

            var path = new SVGPath(pathdata, new Color(255, 100, 56, 0), new Color(0, 0, 0, 0), 2);

            var polygon = new SVGPolygon(points, new Color(100, 255, 56, 0), new Color(0, 0, 0, 0), 2);

            var rect = new SVGRect(50.0, 20.0, 50.0, 20.0, new Color(100, 255, 56, 0), new Color(0, 0, 0, 0), 2);

            //SVGIO.export(new List<SVGShape>(){ polygon, ellipse, circle, rect, path }, "Z:\\Sources\\VectorGraphicsEditor\\IO Tests\\output\\mix.svg", 1000, 1001);

            var res = SVGIO.import("E:\\Sources\\VectorGraphicsEditor\\IO Tests\\output\\mix.svg");

           Console.WriteLine(res.Item1[0].w);
          
        }
    }
}
