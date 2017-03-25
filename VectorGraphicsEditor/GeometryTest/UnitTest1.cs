using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Interfaces;
using VectorGraphicsEditor;
using System.Collections.Generic;

namespace GeometryTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            List<List<Segment>> test = new List<List<Segment>>()
            {
                new List<Segment>()
                {
                    new EllipseArc(new Point(0, 0),  3, 2, 0, Math.PI, new Point(3, 0), new Point(-3, 0), 0),
                    new EllipseArc(new Point(0, 0),  3, 2,  0, Math.PI,  new Point(-3, 0),new Point(3, 0), 0)
                }
        };
            var Test = new Mutant(test, new Color(1, 1, 1, 1), new Color(1, 1, 1, 1), 0.7);

        }
       
    }
}
