using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Logic;

//фабрика и классы всех возможных(пока(это хотя бы сделать)) комманд
//не могу разобраться с тем, как реализовать CanExecuteChanged думаю спросить об этом
//у Рояка если кто знает, то напишите мне или сами сделайте одну для примера.

namespace VectorGraphicsEditor
{

    class CommandsFactory
     {
         Dictionary<string, ICommand> prototypes;
         CommandsFactory(ILogicForGUI Logic)
         {
             prototypes = new Dictionary<string, ICommand>();
             prototypes["AddFigure"] = new AddFigure(Logic);
             prototypes["RemoveFigure"] = new RemoveFigure(Logic);
             prototypes["EditFigure"] = new EditFigure(Logic);
             prototypes["Transform"] = new Transform(Logic);
             prototypes["Union"] = new Union(Logic);
             prototypes["Intersection"] = new Intersection(Logic);
             prototypes["Difference"] = new Difference(Logic);
             prototypes["Pick"] = new Pick(Logic);
             prototypes["Save"] = new Save(Logic);
             prototypes["Load"] = new Load(Logic);
             prototypes["SaveSettings"] = new SaveSettings(Logic);
             prototypes["LoadSettings"] = new LoadSettings(Logic);
             prototypes["UnDo"] = new UnDo(Logic);
             prototypes["ReDo"] = new ReDo(Logic);
             prototypes["Copy"] = new Copy(Logic);
             prototypes["Paste"] = new Paste(Logic);
             prototypes["AddPrototipe"] = new AddPrototipe(Logic);
         }
         ICommand Create(string type, Dictionary<string, object> parms)
         {
             return prototypes[type].Create(parms);
         }
         IEnumerable<string> CommandsTypes
         {
             get
             {
                 return prototypes.Keys;
             }
         }
     }

    //добавляет фигуру. то есть gui должен у себя создавать фигуру, редактировать ее при создании
    //и после того как мышку отпустили передать фигуру нам
    class AddFigure:ICommand
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

        public AddFigure Create(Dictionary<string, object> parms)
        {
            return new AddFigure(Logic);
        }
        bool ICommand.CanExecute(object x)
        {
            return true;
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

        public RemoveFigure Create(Dictionary<string, object> parms)
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


    //изменение выделенной фигуры, если выделенно больше фигур, то их изменить будет нельзя.
    //ну мне так кажется
    class EditFigure:ICommand
    {
        ILogicForCommand Logic;
        public EditFigure(ILogicForGUI Log)
        {
            Logic = (ILogicForCommand)Log;
        }

        public EditFigure(ILogicForCommand Log)
        {
            Logic = Log;
        }

        public EditFigure Create(Dictionary<string, object> parms)
        {
            return new EditFigure(Logic);
        }

        bool ICommand.CanExecute(object x)
        {
            return true;
        }

        void ICommand.Execute(object x)
        {

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

        public Transform Create(Dictionary<string, object> parms)
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

        public Union Create(Dictionary<string, object> parms)
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

        public Intersection Create(Dictionary<string, object> parms)
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

        public Difference Create(Dictionary<string, object> parms)
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
    class Pick:ICommand
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

        public Pick Create(Dictionary<string, object> parms)
        {
            return new Pick(Logic);
        }
        bool ICommand.CanExecute(object x)
        {
            return true;
        }

        void ICommand.Execute(object x)
        {

        }
    }

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

        public Save Create(Dictionary<string, object> parms)
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

        public Load Create(Dictionary<string, object> parms)
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

        public SaveSettings Create(Dictionary<string, object> parms)
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

        public LoadSettings Create(Dictionary<string, object> parms)
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

        public UnDo Create(Dictionary<string, object> parms)
        {
            return new UnDo(Logic);
        }
        bool ICommand.CanExecute(object x)
        {
            return true;
        }

        void ICommand.Execute(object x)
        {

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

        public ReDo Create(Dictionary<string, object> parms)
        {
            return new ReDo(Logic);
        }
        bool ICommand.CanExecute(object x)
        {
            return true;
        }

        void ICommand.Execute(object x)
        {

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

        public Copy Create(Dictionary<string, object> parms)
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

        public Paste Create(Dictionary<string, object> parms)
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

        public AddPrototipe Create(Dictionary<string, object> parms)
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
