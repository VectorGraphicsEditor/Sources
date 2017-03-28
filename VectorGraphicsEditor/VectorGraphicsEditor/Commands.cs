using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Logic;

//фабрика и классы всех возможных(пока(это хотя бы сделать)) комманд

namespace VectorGraphicsEditor
{

    public class CommandsFactory
    {
        Dictionary<string, ICommand> prototypes;
        public CommandsFactory(ILogicForGUI Logic)
        {
            prototypes = new Dictionary<string, ICommand>();
            prototypes["AddFigure"] = new AddFigure(Logic);
            prototypes["RemoveFigure"] = new RemoveFigure(Logic);
            prototypes["EditColor"] = new EditColor(Logic);
            prototypes["Transform"] = new Transform(Logic);
            prototypes["Union"] = new Union(Logic);
            prototypes["Intersection"] = new Intersection(Logic);
            prototypes["Difference"] = new Difference(Logic);
            prototypes["Pick"] = new Pick(Logic);
            //for nikita - korito
            prototypes["PickFromList"] = new PickFromList(Logic);
            prototypes["MoveIndex"] = new MoveIndex(Logic);
            prototypes["MoveLayer"] = new MoveLayer(Logic);
            prototypes["Save"] = new Save(Logic);
            prototypes["Load"] = new Load(Logic);
            prototypes["SaveSettings"] = new SaveSettings(Logic);
            prototypes["LoadSettings"] = new LoadSettings(Logic);
            prototypes["UnDo"] = new UnDo(Logic);
            prototypes["ReDo"] = new ReDo(Logic);
            prototypes["Copy"] = new Copy(Logic);
            prototypes["Paste"] = new Paste(Logic);
            prototypes["AddPrototipe"] = new AddPrototipe(Logic);
            prototypes["ClearList"] = new ClearList(Logic);
            prototypes["GetIndexFromPick"] = new GetIndexFromPick(Logic);
        }
        public ICommand Create(string type, Dictionary<string, object> parms)
        {
            return prototypes[type].Create(parms);
        }
        public IEnumerable<string> CommandsTypes
        {
            get
            {
                return prototypes.Keys;
            }
        }
    }

    //добавляет фигуру. то есть gui должен у себя создавать фигуру, редактировать ее при создании
    //и после того как мышку отпустили передать фигуру нам
    class AddFigure : ICommand
    {
        ILogicForCommand Logic;
        public AddFigure(ILogicForGUI Log)
        {
            Logic = (ILogicForCommand)Log;
        }

        public AddFigure(ILogicForCommand Log)
        {
            Logic = Log;
        }

        ICommand ICommand.Create(Dictionary<string, object> parms)
        {
            return new AddFigure(Logic);
        }
        bool ICommand.CanExecute(object x)
        {
            if (Logic.GetStackIndex() != 0)
                return true;
            return false;
        }

        void ICommand.Execute(object x)
        {
            Logic.addFigure((IFigure) x);
        }
    }

    class RemoveFigure : ICommand
    {
        ILogicForCommand Logic;
        public RemoveFigure(ILogicForGUI Log)
        {
            Logic = (ILogicForCommand)Log;
        }

        public RemoveFigure(ILogicForCommand Log)
        {
            Logic = Log;
        }

        ICommand ICommand.Create(Dictionary<string, object> parms)
        {
            return new RemoveFigure(Logic);
        }
        bool ICommand.CanExecute(object x)
        {
            if (Logic.CountFigures > 0)
                return true;
            else
                return false;
        }

        void ICommand.Execute(object x)
        {
            Logic.removeFigures();
        }
    }


    //изменение выделенной цвета выделенной фигуры.
    // принимает цвет и bool: если значение bool true, то изменяет заливку
    // если значение false, то изменяет контур.
    class EditColor:ICommand
    {
        ILogicForCommand Logic;
        public EditColor(ILogicForGUI Log)
        {
            Logic = (ILogicForCommand)Log;
        }

        public EditColor(ILogicForCommand Log)
        {
            Logic = Log;
        }

        ICommand ICommand.Create(Dictionary<string, object> parms)
        {
            return new EditColor(Logic);
        }

        bool ICommand.CanExecute(object x)
        {
            if (Logic.CountCurientFigures > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void ICommand.Execute(object x)
        {
            Tuple<Interfaces.Color, bool> buf = (Tuple<Interfaces.Color, bool>)x;
            if (buf.Item2)
                Logic.editColor(buf.Item1);
            else
                Logic.editBorderColor(buf.Item1);
        }
    }

    //трансформация выделенных фигур.
    class Transform:ICommand
    {
        ILogicForCommand Logic;
        public Transform(ILogicForGUI Log)
        {
            Logic = (ILogicForCommand)Log;
        }

        public Transform(ILogicForCommand Log)
        {
            Logic = Log;
        }

        ICommand ICommand.Create(Dictionary<string, object> parms)
        {
            return new Transform(Logic);
        }

        bool ICommand.CanExecute(object x)
        {
            if (Logic.CountCurientFigures > 0)
                return true;
            else
                return false;
        }

        void ICommand.Execute(object x)
        {

        }
    }

    //объединение выделенных, если фигур больше чем две то можно наверное не давать возможность выполнять комманду
    class Union:ICommand
    {
        ILogicForCommand Logic;
        public Union(ILogicForGUI Log)
        {
            Logic = (ILogicForCommand)Log;
        }

        public Union(ILogicForCommand Log)
        {
            Logic = Log;
        }

        ICommand ICommand.Create(Dictionary<string, object> parms)
        {
            return new Union(Logic);
        }
        bool ICommand.CanExecute(object x)
        {
            if (Logic.CountCurientFigures == 2)
                return true;
            else
                return false;
        }

        void ICommand.Execute(object x)
        {

        }
    }

    //пересечение тоже самое
    class Intersection:ICommand
    {
        ILogicForCommand Logic;
        public Intersection(ILogicForGUI Log)
        {
            Logic = (ILogicForCommand)Log;
        }

        public Intersection(ILogicForCommand Log)
        {
            Logic = Log;
        }

        ICommand ICommand.Create(Dictionary<string, object> parms)
        {
            return new Intersection(Logic);
        }
        bool ICommand.CanExecute(object x)
        {
            if (Logic.CountCurientFigures == 2)
                return true;
            else
                return false;
        }

        void ICommand.Execute(object x)
        {

        }
    }

    //разность
    class Difference:ICommand
    {
        ILogicForCommand Logic;
        public Difference(ILogicForGUI Log)
        {
            Logic = (ILogicForCommand)Log;
        }

        public Difference(ILogicForCommand Log)
        {
            Logic = Log;
        }

        ICommand ICommand.Create(Dictionary<string, object> parms)
        {
            return new Difference(Logic);
        }
        bool ICommand.CanExecute(object x)
        {
            if (Logic.CountCurientFigures == 2)
                return true;
            else
                return false;
        }

        void ICommand.Execute(object x)
        {

        }
    }

    //выделение фигуры. предусматривает просто выделение и выделение нескольких
    // если выделить уже выделенное, но под ней есть фигура, то выделется та, что под ней
    // если там нет ничего, то ничего не произойдет.
    class Pick : ICommand
    {
        ILogicForCommand Logic;
        public Pick(ILogicForGUI Log)
        {
            Logic = (ILogicForCommand)Log;
        }

        public Pick(ILogicForCommand Log)
        {
            Logic = Log;
        }

        ICommand ICommand.Create(Dictionary<string, object> parms)
        {
            return new Pick(Logic);
        }
        bool ICommand.CanExecute(object x)
        {
            return true;
        }

        void ICommand.Execute(object x)
        {
            Tuple<Interfaces.Point, bool> buf = (Tuple<Interfaces.Point, bool>)x;
            Logic.addCurientFigure(buf.Item1, buf.Item2);
        }

    }
        //выделение фигуры в списке фигур
        // Так хочет Никита. предусматривает просто выделение и выделение нескольких фигур
        // если выбрать уже выбранную фигуру, то тогда она перестанет быть выделенной
    class PickFromList:ICommand
    {
        ILogicForCommand Logic;
        public PickFromList(ILogicForGUI Log)
        {
            Logic = (ILogicForCommand)Log;
        }

        public PickFromList(ILogicForCommand Log)
        {
            Logic = Log;
        }

        ICommand ICommand.Create(Dictionary<string, object> parms)
        {
            return new PickFromList(Logic);
        }
        bool ICommand.CanExecute(object x)
        {
            return true;
        }


        void ICommand.Execute(object x)
        {
            Tuple<int, bool> buf = (Tuple<int, bool>)x;
            Logic.addCurientFigureWithIndex(buf.Item1, buf.Item2);
        }
    }

    class GetIndexFromPick : ICommand
    {
        ILogicForCommand Logic;
        public GetIndexFromPick(ILogicForGUI Log)
        {
            Logic = (ILogicForCommand)Log;
        }

        public GetIndexFromPick(ILogicForCommand Log)
        {
            Logic = Log;
        }

        ICommand ICommand.Create(Dictionary<string, object> parms)
        {
            return new GetIndexFromPick(Logic);
        }
        bool ICommand.CanExecute(object x)
        {
            return true;
        }


        void ICommand.Execute(object x)
        {
            Tuple<Interfaces.Point, int> buf = (Tuple<Interfaces.Point, int>) x;
            Logic.GetIndexFromPick(buf.Item1, buf.Item2);
        }
    }

    class ClearList : ICommand
    {
        ILogicForCommand Logic;
        public ClearList(ILogicForGUI Log)
        {
            Logic = (ILogicForCommand)Log;
        }

        public ClearList(ILogicForCommand Log)
        {
            Logic = Log;
        }

        ICommand ICommand.Create(Dictionary<string, object> parms)
        {
            return new ClearList(Logic);
        }
        bool ICommand.CanExecute(object x)
        {
            return true;
        }


        void ICommand.Execute(object x)
        {
            Logic.ClearCurrentFigures();
        }
    }

    class MoveIndex : ICommand
    {
        ILogicForCommand Logic;
        public MoveIndex(ILogicForGUI Log)
        {
            Logic = (ILogicForCommand)Log;
        }

        public MoveIndex(ILogicForCommand Log)
        {
            Logic = Log;
        }

        ICommand ICommand.Create(Dictionary<string, object> parms)
        {
            return new MoveIndex(Logic);
        }
        bool ICommand.CanExecute(object x)
        {
            if (Logic.CountCurientFigures == 0 || Logic.CountCurientFigures == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void ICommand.Execute(object x)
        {
            Logic.moveIndexFigure((bool)x);
        }
    }

    class MoveLayer : ICommand
    {
        ILogicForCommand Logic;
        public MoveLayer(ILogicForGUI Log)
        {
            Logic = (ILogicForCommand)Log;
        }

        public MoveLayer(ILogicForCommand Log)
        {
            Logic = Log;
        }

        ICommand ICommand.Create(Dictionary<string, object> parms)
        {
            return new MoveLayer(Logic);
        }

        bool ICommand.CanExecute(object x)
        {
            if (Logic.CountCurientFigures == 1)
            {
                if ((bool)x)
                {
                    if (Logic.IndexCurientElem != Logic.CountFigures - 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (Logic.IndexCurientElem != 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        void ICommand.Execute(object x)
        {
            Logic.moveIndexFigure((bool)x);
        }
    }

#region IO commands

    //сохранение
    class Save:ICommand
    {
        ILogicForCommand Logic;
        public Save(ILogicForGUI Log)
        {
            Logic = (ILogicForCommand)Log;
        }

        public Save(ILogicForCommand Log)
        {
            Logic = Log;
        }

        ICommand ICommand.Create(Dictionary<string, object> parms)
        {
            return new Save(Logic);
        }
        bool ICommand.CanExecute(object x)
        {
            return true;
        }

        void ICommand.Execute(object x)
        {

        }
    }

    //загрузка
    class Load:ICommand
    {
        ILogicForCommand Logic;
        public Load(ILogicForGUI Log)
        {
            Logic = (ILogicForCommand)Log;
        }

        public Load(ILogicForCommand Log)
        {
            Logic = Log;
        }

        ICommand ICommand.Create(Dictionary<string, object> parms)
        {
            return new Load(Logic);
        }
        bool ICommand.CanExecute(object x)
        {
            return true;
        }

        void ICommand.Execute(object x)
        {

        }
    }

    //сохранение шаблонов фигур
    class SaveSettings:ICommand
    {
        ILogicForCommand Logic;
        public SaveSettings(ILogicForGUI Log)
        {
            Logic = (ILogicForCommand)Log;
        }

        public SaveSettings(ILogicForCommand Log)
        {
            Logic = Log;
        }

        ICommand ICommand.Create(Dictionary<string, object> parms)
        {
            return new SaveSettings(Logic);
        }
        bool ICommand.CanExecute(object x)
        {
            return true;
        }

        void ICommand.Execute(object x)
        {

        }
    }

    //загузка шаблонов фигур
    class LoadSettings:ICommand
    {
        ILogicForCommand Logic;
        public LoadSettings(ILogicForGUI Log)
        {
            Logic = (ILogicForCommand)Log;
        }

        public LoadSettings(ILogicForCommand Log)
        {
            Logic = Log;
        }

        ICommand ICommand.Create(Dictionary<string, object> parms)
        {
            return new LoadSettings(Logic);
        }
        bool ICommand.CanExecute(object x)
        {
            return true;
        }

        void ICommand.Execute(object x)
        {

        }
    }
#endregion

    //отмена последней команды
    class UnDo:ICommand
    {
        ILogicForCommand Logic;
        public UnDo(ILogicForGUI Log)
        {
            Logic = (ILogicForCommand)Log;
        }

        public UnDo(ILogicForCommand Log)
        {
            Logic = Log;
        }

        ICommand ICommand.Create(Dictionary<string, object> parms)
        {
            return new UnDo(Logic);
        }
        bool ICommand.CanExecute(object x)
        {
            return true;
        }

        void ICommand.Execute(object x)
        {
            Logic.SetPreviousStackState();
        }
    }

    //отмена отмены)
    class ReDo:ICommand
    {
        ILogicForCommand Logic;
        public ReDo(ILogicForGUI Log)
        {
            Logic = (ILogicForCommand)Log;
        }

        public ReDo(ILogicForCommand Log)
        {
            Logic = Log;
        }

        ICommand ICommand.Create(Dictionary<string, object> parms)
        {
            return new ReDo(Logic);
        }
        bool ICommand.CanExecute(object x)
        {
            return true;
        }

        void ICommand.Execute(object x)
        {
            Logic.SetNextStackState();
        }
    }

    //копирование фигур в буфер
    class Copy:ICommand
    {
        ILogicForCommand Logic;
        public Copy(ILogicForGUI Log)
        {
            Logic = (ILogicForCommand)Log;
        }

        public Copy(ILogicForCommand Log)
        {
            Logic = Log;
        }

        ICommand ICommand.Create(Dictionary<string, object> parms)
        {
            return new Copy(Logic);
        }
        bool ICommand.CanExecute(object x)
        {
            return true;
        }

        void ICommand.Execute(object x)
        {

        }
    }

    //вставка фигур(возможно будет принимать текущую точку и в нее будет копироваться правый верхний угол
    //фигуры
    class Paste:ICommand
    {
        ILogicForCommand Logic;
        public Paste(ILogicForGUI Log)
        {
            Logic = (ILogicForCommand)Log;
        }

        public Paste(ILogicForCommand Log)
        {
            Logic = Log;
        }

        ICommand ICommand.Create(Dictionary<string, object> parms)
        {
            return new Paste(Logic);
        }
        bool ICommand.CanExecute(object x)
        {
            return true;
        }

        void ICommand.Execute(object x)
        {

        }
    }

    //добавление прототипа фигуры
    class AddPrototipe:ICommand
    {
        ILogicForCommand Logic;
        public AddPrototipe(ILogicForGUI Log)
        {
            Logic = (ILogicForCommand)Log;
        }

        public AddPrototipe(ILogicForCommand Log)
        {
            Logic = Log;
        }

        ICommand ICommand.Create(Dictionary<string, object> parms)
        {
            return new AddPrototipe(Logic);
        }
        bool ICommand.CanExecute(object x)
        {
            return true;
        }

        void ICommand.Execute(object x)
        {

        }
    }
}
