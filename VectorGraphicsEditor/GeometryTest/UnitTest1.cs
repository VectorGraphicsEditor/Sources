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
            /*
            Rectangle TestFigure = new Rectangle(
                                                new Point(1,1),
                                                new Point(10,10),
                                                new Color(0, 0, 0, 1),
                                                new Color(255, 255, 255, 1));
             Конструктор работает
           */

            
            FigureFactory Factory = new FigureFactory();

            var TestRect = Factory.Create("Rectangle", new Dictionary<string, object>()
            {
                ["DownLeft"] = new Point(1, 1),
                ["UpRight"] = new Point(10, 10),
                ["BorderColor"] = new Color(0, 0, 0, 1),
                ["FillColor"] = new Color(255, 255, 255, 1)
            });

            FigureFactory B = new FigureFactory();

        }
    }
}
