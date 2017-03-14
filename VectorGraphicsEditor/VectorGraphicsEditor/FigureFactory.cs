﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace VectorGraphicsEditor
{
    using Interfaces;
    class FigureFactory
    {


        static Dictionary<string, IFigure> prototypes;
        static FigureFactory()
        {
            prototypes = new Dictionary<string, IFigure>();
            //prototypes["Line"] = new Line(new Point(0, 0), new Point(1, 1), new Color(0, 0, 0));

            //prototypes["Rectangle"] = new Rectangle(new Point(0, 0),
            //                                        new Point(1, 1),
            //                                        new Color(0, 0, 0),
            //                                        new Color(255, 255, 255)
            //                                        );
            //prototypes["Circle"] = new Circle(new Point(0, 0),
            //                                  1.0,
            //                                  new Color(0, 0, 0),
            //                                  new Color(255, 255, 255)
            //                                  );

        }
        static IFigure Create(string type, Dictionary<string, object> parms)
        {
            return prototypes[type].Create(parms);
        }
        static IEnumerable<string> ShapeTypes
        {
            get
            {
                return prototypes.Keys;
            }
        }


    }

}