using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Interfaces;
using test_editor;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class TriangulationTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            List<Point> points = new List<Point>() { new Point (1, 1),
                                                     new Point (6,1),
                                                     new Point(10,3),
                                                     new Point(10,10),
                                                     new Point(4,10),
                                                     new Point(1,5)};
            Figure polygon =
                new Figure("Polygon", ref points);
        }
    }
}
