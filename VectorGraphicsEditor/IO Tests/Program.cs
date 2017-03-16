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
            var circle = new SVGCircle(new Point(50.0, 50.0), 20.0);

            SVGIO.export(new List<SVGShape>(){ circle }, "D:\\Documents\\GitHubVisualStudio\\Sources\\VectorGraphicsEditor\\IO Tests\\output\\circle.svg", 100, 100);
        }
    }
}
