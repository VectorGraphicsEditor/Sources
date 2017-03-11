using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

//фабрика и классы всех возможных(пока(это хотя бы сделать)) комманд
//не могу разобраться с тем, как реализовать CanExecuteChanged думаю спросить об этом
//у Рояка если кто знает, то напишите мне или сами сделайте одну для примера.

namespace VectorGraphicsEditor
{
    class CommandsFactory
    {


        static Dictionary<string, ICommand> prototypes;
        static CommandsFactory()
        {
            prototypes = new Dictionary<string, ICommand>();
            prototypes["AddFigure"] = new AddFigure();
            prototypes["EditFigure"] = new EditFigure();
            prototypes["Transform"] = new Transform();
            prototypes["Union"] = new Union();
            prototypes["Intersection"] = new Intersection();
            prototypes["Difference"] = new Difference();
            prototypes["Pick"] = new Pick();
            prototypes["Save"] = new Save();
            prototypes["Load"] = new Load();
            prototypes["SaveSettings"] = new SaveSettings();
            prototypes["LoadSettings"] = new LoadSettings();
            prototypes["UnDo"] = new UnDo();
            prototypes["ReDo"] = new ReDo();
            prototypes["Copy"] = new Copy();
            prototypes["Paste"] = new Paste();
            prototypes["AddPrototipe"] = new AddPrototipe();
        }
        static ICommand Create(string type, Dictionary<string, object> parms)
        {
            return prototypes[type].Create(parms);
        }
        static IEnumerable<string> CommandsTypes
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
        public AddFigure()
        {

        }
        bool CanExecute(object x)
        {
            return true;
        }

        void Execute(object x)
        {

        }
    }

    //изменение выделенной фигуры, если выделенно больше фигур, то их изменить будет нельзя.
    //ну мне так кажется
    class EditFigure:ICommand
    {
        public EditFigure()
        {

        }
        bool CanExecute(object x)
        {
            return true;
        }

        void Execute(object x)
        {

        }
    }

    //трансформация выделенных фигур.
    class Transform:ICommand
    {
        public Transform()
        {

        }
        bool CanExecute(object x)
        {
            return true;
        }

        void Execute(object x)
        {

        }
    }

    //объединение выделенных, если фигур больше чем две то можно наверное не давать возможность выполнять комманду
    class Union:ICommand
    {
        public Union()
        {

        }
        bool CanExecute(object x)
        {
            return true;
        }

        void Execute(object x)
        {

        }
    }

    //пересечение тоже самое
    class Intersection:ICommand
    {
        public Intersection()
        {

        }
        bool CanExecute(object x)
        {
            return true;
        }

        void Execute(object x)
        {

        }
    }

    //разность
    class Difference:ICommand
    {
        public Difference()
        {

        }
        bool CanExecute(object x)
        {
            return true;
        }

        void Execute(object x)
        {

        }
    }

    //выделение фигуры. предусматривает просто выделение и выделение нескольких
    class Pick:ICommand
    {
        public Pick()
        {

        }
        bool CanExecute(object x)
        {
            return true;
        }

        void Execute(object x)
        {

        }
    }

    //сохранение
    class Save:ICommand
    {
        public Save()
        {

        }
        bool CanExecute(object x)
        {
            return true;
        }

        void Execute(object x)
        {

        }
    }

    //загрузка
    class Load:ICommand
    {
        public Load()
        {

        }
        bool CanExecute(object x)
        {
            return true;
        }

        void Execute(object x)
        {

        }
    }

    //сохранение шаблонов фигур
    class SaveSettings:ICommand
    {
        public SaveSettings()
        {

        }
        bool CanExecute(object x)
        {
            return true;
        }

        void Execute(object x)
        {

        }
    }

    //загузка шаблонов фигур
    class LoadSettings:ICommand
    {
        public LoadSettings()
        {

        }
        bool CanExecute(object x)
        {
            return true;
        }

        void Execute(object x)
        {

        }
    }
    
    //отмена последней команды
    class UnDo:ICommand
    {
        public UnDo()
        {

        }
        bool CanExecute(object x)
        {
            return true;
        }

        void Execute(object x)
        {

        }
    }

    //отмена отмены)
    class ReDo:ICommand
    {
        public ReDo()
        {

        }
        bool CanExecute(object x)
        {
            return true;
        }

        void Execute(object x)
        {

        }
    }

    //копирование фигур в буфер
    class Copy:ICommand
    {
        public Copy()
        {

        }
        bool CanExecute(object x)
        {
            return true;
        }

        void Execute(object x)
        {

        }
    }

    //вставка фигур(возможно будет принимать текущую точку и в нее будет копироваться правый верхний угол
    //фигуры
    class Paste:ICommand
    {
        public Paste()
        {

        }
        public bool CanExecute(object x)
        {
            return true;
        }

        public void Execute(object x)
        {

        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    //добавление прототипа фигуры
    class AddPrototipe:ICommand
    {
        public AddPrototipe()
        {

        }
        bool CanExecute(object x)
        {
            return true;
        }

        void Execute(object x)
        {

        }
    }
}
