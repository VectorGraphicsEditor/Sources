using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace VectorGraphicsEditor
{
    using Interfaces;
    public class Factory
    {
        static Dictionary<string, IFigure> prototypes = new Dictionary<string, IFigure>();
        static Factory()
        {
            //prototypes = ;
            //prototypes["Line"] = new Line(new Point(0, 0), new Point(1, 1), new Color(0, 0, 0));
            
            prototypes["Rectangle"] = new Rectangle(new Point(0, 0),
                                                    new Point(1, 1),
                                                    new Color(0, 0, 0, 1),
                                                    new Color(255, 255, 255, 1)
                                                    );
            //prototypes["Circle"] = new Circle(new Point(0, 0),
            //                                  1.0,
            //                                  new Color(0, 0, 0),
            //                                  new Color(255, 255, 255)
            //                                  );

            // prototypes["Triangle"] = new Triangle(new Point(0, 0),
            //                                      new Point(1, 0),
            //                                      new Point(0, 0),
            //                                      new Color(0, 0, 0),
            //                                      new Color(255, 255, 255));
            prototypes["Mutant_"+(prototypes.Count()).ToString()] = new Mutant(new List<Segment>());
        }
        public static IFigure Create(string type, Dictionary<string, object> parms)
        {
            return prototypes[type].Clone(parms);
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
