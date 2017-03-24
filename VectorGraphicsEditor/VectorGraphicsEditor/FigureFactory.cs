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

            prototypes["Triangle"] = new Triangle(new Point(0, 0),
                                                  new Point(1, 0),
                                                  new Point(0, 0),
                                                  new Color(0, 0, 0, 1),
                                                  new Color(255, 255, 255, 1));
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

        //Все таки наверное так и для мутанта хранить еще значения этого "квадрата", в который он вложен
        //и как-то преобразовывать новую фигуру сжимая мутанта.
        public static void AddPrototipe(IFigure mutant)
        {
            prototypes["Mutant_" + (prototypes.Count() - 3).ToString()] = mutant;
        }
    }

}
