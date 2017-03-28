using SharpGL;

using System;
using System.Windows.Forms;
using Logic;
using System.Drawing;
using System.Collections.Generic;
using Interfaces;

namespace VectorGraphicsEditor
{
    public partial class MainForm : Form
    {
        ILogicForGUI logic = new logic();
        CommandsFactory commands;
        ICommand addCommand;
        ICommand removeCommand;
        ICommand pickCommand;
        ICommand editCommand;
        ICommand transformCommand;
        ICommand moveLayerCommand;
        ICommand pickFromListCommand;
        ICommand clearCurrentCommand;
        ICommand getIndexCommand;

        bool ctrlPressed = false;

        ICommand unionCommand;
        ICommand intersectionCommand;
        ICommand diffCommand;
        ICommand copyCommand;
        ICommand pasteCommand;
        ICommand undoCommand;
        ICommand redoCommand;
        ICommand saveCommand;
        ICommand loadCommand;
        ICommand saveSettingsCommand;
        ICommand loadSettingsCommand;

        // Наименьший dpi 
        double minDpi = 1;

        private enum Figures {
            Line,
            Quadrangle,
            Triangle,
            Circle,
            Ellipse,
            Mutant
        };

        // Режимы работы
        private enum Modes
        {
            Draw,       // Рисование
            Scale,      // Линейка
            Divider     // Циркуль
        };

        Modes mode = Modes.Draw;
        
        private Interfaces.Point[] last3Points;
        private List<Interfaces.Point> pointsMutant;
        List<List<Segment>> listFragments = new List<List<Segment>>();
        private Interfaces.Color borderColor = new Interfaces.Color(0, 0, 0, 255);
        private Interfaces.Color fillColor = new Interfaces.Color(255, 255, 255, 255);

        private int iPointTriangle = 0;
        private int prevLocationX, prevLocationY;//для запоминания предыдущего положения мыши
        private int canvasPositionX = 0, canvasPositionY = 0;//положение полотна

        private bool isMouseDown = false;
        private bool isMiddleButton = false; // флаг для средней кнопки мыши
        private bool isChangedOpenGLView = true;
        private bool isLoadOpenGLView = false;
        private bool isStartDrag = false;
        private bool isModeSelectFigures = false;

        private float zoomOpenGLView = 1.0f;

        private Figures selectedFigure = Figures.Line;


        // чтобы последняя нарисованная фигура, не перекрывала всё и вся
        // 
        private bool readyToDrawTempFigure = true;

        // используется для вывода в списке справа
        private int countTriangles = 1;
        private int countRectangles = 1;
        private int countCircles = 1;
        private int countElipses = 1;
        private int countLines = 1;

        private bool changedLayerOrder = false;

        private int quantitySegments = 360;


        public MainForm()
        {
     
            InitializeComponent();

            last3Points = new Interfaces.Point[3] {
                new Interfaces.Point(0, 0),
                new Interfaces.Point(0, 0),
                new Interfaces.Point(0, 0)
            };
            button_choose_fill_color.BackColor = System.Drawing.Color.White;
            button_choose_line_color.BackColor = System.Drawing.Color.Black;


            commands = new CommandsFactory(logic);
            addCommand = commands.Create("AddFigure", null);
            removeCommand = commands.Create("RemoveFigure", null);

            moveLayerCommand = commands.Create("MoveLayer", null);
            pickFromListCommand = commands.Create("PickFromList", null);
            clearCurrentCommand = commands.Create("ClearList", null);
            getIndexCommand = commands.Create("GetIndexFromPick", null);

            editCommand = commands.Create("EditColor", null);
            pickCommand = commands.Create("Pick", null);
            transformCommand = commands.Create("Transform", null);

            unionCommand = commands.Create("Union", null);
            intersectionCommand = commands.Create("Intersection", null);
            diffCommand = commands.Create("Difference", null);
            undoCommand = commands.Create("UnDo", null);
            redoCommand = commands.Create("ReDo", null);
            copyCommand = commands.Create("Copy", null);
            pasteCommand = commands.Create("Paste", null);

            saveCommand = commands.Create("Save", null);
            loadCommand = commands.Create("Load", null);
            saveSettingsCommand = commands.Create("SaveSettings", null);
            loadSettingsCommand = commands.Create("LoadSettings", null);

        }

        public OpenGL getOpenGL()
        {
            return openGLControlView.OpenGL;
        }

        private void preGL2D(OpenGL gl, int width, int height)
        {
            gl.DrawBuffer(OpenGL.GL_FRONT);
            gl.Viewport(0, 0, width, height);

            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
            gl.Ortho(0, width, 0, height, -1, 1);

            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();

            // Передвижение полотна при зажатом колёсике
            gl.Translate(canvasPositionX, -canvasPositionY, 0);

            gl.ShadeModel(OpenGL.GL_SMOOTH);
            gl.PixelStore(OpenGL.GL_UNPACK_ALIGNMENT, 4);
            gl.Hint(OpenGL.GL_PERSPECTIVE_CORRECTION_HINT, OpenGL.GL_NICEST);

            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.Disable(OpenGL.GL_CULL_FACE);
            gl.Enable(OpenGL.GL_BLEND);

            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Enable(OpenGL.GL_LINE_SMOOTH);
            gl.Hint(OpenGL.GL_LINE_SMOOTH_HINT, OpenGL.GL_NICEST);

            gl.ClearColor(1, 1, 1, 1);
            gl.ClearStencil(0);
            gl.ClearDepth(1.0);

            gl.DepthFunc(OpenGL.GL_LEQUAL);

            gl.Clear(OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_COLOR_BUFFER_BIT);
        }

        void AddNewLastPoint(Interfaces.Point lastPoint)
        {
            last3Points[0] = last3Points[1];
            last3Points[1] = last3Points[2];
            last3Points[2] = lastPoint;
        }    

        private void buttonRect_Click(object sender, EventArgs e)
        {
            isModeSelectFigures = false;
            mode = Modes.Draw;
            selectedFigure = Figures.Quadrangle;
        }

        private void buttonLine_Click(object sender, EventArgs e)
        {
            isModeSelectFigures = false;
            mode = Modes.Draw;
            selectedFigure = Figures.Line;
        }

        private double GetDistance(Interfaces.Point a, Interfaces.Point b)
        {
            return Math.Sqrt((b.X - a.X) * (b.X - a.X) + (b.Y - a.Y) * (b.Y - a.Y));
        }

        private void DrawTriangle(OpenGL gl, trTriangle triangle, Interfaces.Color color)
        {
            gl.Color(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
            gl.Begin(OpenGL.GL_TRIANGLES);
            gl.Vertex(triangle.A.X, openGLControlView.Height - triangle.A.Y);
            gl.Vertex(triangle.B.X, openGLControlView.Height - triangle.B.Y);
            gl.Vertex(triangle.C.X, openGLControlView.Height - triangle.C.Y);
            gl.End();
        }

        private void DrawTriangle(OpenGL gl, Interfaces.Point[] points)
        {
            gl.Color(fillColor.R / 255.0f, fillColor.G / 255.0f, fillColor.B / 255.0f, fillColor.A / 255.0f);
            gl.Begin(OpenGL.GL_TRIANGLES);
            gl.Vertex(points[0].X, openGLControlView.Height - points[0].Y);
            gl.Vertex(points[1].X, openGLControlView.Height - points[1].Y);
            gl.Vertex(points[2].X, openGLControlView.Height - points[2].Y);
            gl.End();

            gl.Color(borderColor.R / 255.0f, borderColor.G / 255.0f, borderColor.B / 255.0f, borderColor.A / 255.0f);
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(points[0].X, openGLControlView.Height - points[0].Y);
            gl.Vertex(points[1].X, openGLControlView.Height - points[1].Y);
            gl.Vertex(points[1].X, openGLControlView.Height - points[1].Y);
            gl.Vertex(points[2].X, openGLControlView.Height - points[2].Y);
            gl.Vertex(points[2].X, openGLControlView.Height - points[2].Y);
            gl.Vertex(points[0].X, openGLControlView.Height - points[0].Y);
            gl.End();
        }

        private void DrawQuadrangle(OpenGL gl, Interfaces.Point firstPoint, Interfaces.Point lastPoint)
        {
            gl.Color(fillColor.R / 255.0f, fillColor.G / 255.0f, fillColor.B / 255.0f, fillColor.A / 255.0f);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Vertex(firstPoint.X, openGLControlView.Height - firstPoint.Y);
            gl.Vertex(firstPoint.X, openGLControlView.Height - lastPoint.Y);
            gl.Vertex(lastPoint.X, openGLControlView.Height - lastPoint.Y);
            gl.Vertex(lastPoint.X, openGLControlView.Height - firstPoint.Y);
            gl.End();

            gl.Color(borderColor.R / 255.0f, borderColor.G / 255.0f, borderColor.B / 255.0f, borderColor.A / 255.0f);
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(firstPoint.X, openGLControlView.Height - firstPoint.Y);
            gl.Vertex(firstPoint.X, openGLControlView.Height - lastPoint.Y);

            gl.Vertex(firstPoint.X, openGLControlView.Height - lastPoint.Y);
            gl.Vertex(lastPoint.X, openGLControlView.Height - lastPoint.Y);

            gl.Vertex(lastPoint.X, openGLControlView.Height - lastPoint.Y);
            gl.Vertex(lastPoint.X, openGLControlView.Height - firstPoint.Y);

            gl.Vertex(lastPoint.X, openGLControlView.Height - firstPoint.Y);
            gl.Vertex(firstPoint.X, openGLControlView.Height - firstPoint.Y);
            gl.End();
        }

        void DrawEllipse(OpenGL gl, Interfaces.Point leftUpperPoint, Interfaces.Point rightDownPoint, int numSegments)
        {
            Interfaces.Point centerPoint = new Interfaces.Point(leftUpperPoint.X + (rightDownPoint.X - leftUpperPoint.X) / 2.0, leftUpperPoint.Y + (rightDownPoint.Y - leftUpperPoint.Y) / 2.0);
            Interfaces.Point r = new Interfaces.Point(Math.Abs(rightDownPoint.X - centerPoint.X), Math.Abs(rightDownPoint.Y - centerPoint.Y));
            double theta = 2.0 * Math.PI / (double)(numSegments);
            double cos = Math.Cos(theta);
            double sin = Math.Sin(theta);
            double t;

            double x = 1;
            double y = 0;

            gl.Begin(OpenGL.GL_LINE_LOOP);
            for (int ii = 0; ii < numSegments; ii++)
            {
                gl.Color(borderColor.R / 255.0f, borderColor.G / 255.0f, borderColor.B / 255.0f, borderColor.A / 255.0f);
                // Радиус и отступ
                gl.Vertex(x * r.X + centerPoint.X, openGLControlView.Height - (y * r.Y + centerPoint.Y));

                // Матрица поворота
                t = x;
                x = cos * x - sin * y;
                y = sin * t + cos * y;
            }
            gl.End();


            x = 1;
            y = 0;

            gl.Begin(OpenGL.GL_POLYGON);
            for (int ii = 0; ii < numSegments; ii++)
            {
                gl.Color(fillColor.R / 255.0f, fillColor.G / 255.0f, fillColor.B / 255.0f, fillColor.A / 255.0f);
                // Радиус и отступ
                gl.Vertex(x * r.X + centerPoint.X, openGLControlView.Height - (y * r.Y + centerPoint.Y));

                // Матрица поворота
                t = x;
                x = cos * x - sin * y;
                y = sin * t + cos * y;
            }
            gl.End();
        }

        void DrawFillCircle(OpenGL gl, Interfaces.Point centerPoint, Interfaces.Point borderPoint, int numSegments)
        {
            double r = GetDistance(centerPoint, borderPoint);
            double theta = 2.0 * Math.PI / (double)(numSegments);
            double cos = Math.Cos(theta);
            double sin = Math.Sin(theta);
            double t;

            double x = 1;
            double y = 0;
            gl.Begin(OpenGL.GL_POLYGON);
            for (int ii = 0; ii < numSegments; ii++)
            {
                gl.Color(fillColor.R / 255.0f, fillColor.G / 255.0f, fillColor.B / 255.0f, fillColor.A / 255.0f);
                // Радиус и отступ
                gl.Vertex(x * r + centerPoint.X, openGLControlView.Height - (y * r + centerPoint.Y));

                // Матрица поворота
                t = x;
                x = cos * x - sin * y;
                y = sin * t + cos * y;
            }
            gl.End();
        }

        void DrawContourCircle(OpenGL gl, Interfaces.Point centerPoint, Interfaces.Point borderPoint, int numSegments)
        {
            double r = GetDistance(centerPoint, borderPoint);
            double theta = 2.0 * Math.PI / (double)(numSegments);
            double cos = Math.Cos(theta);
            double sin = Math.Sin(theta);
            double t;

            double x = 1;
            double y = 0;

            gl.Begin(OpenGL.GL_LINE_LOOP);
            for (int ii = 0; ii < numSegments; ii++)
            {
                gl.Color(borderColor.R / 255.0f, borderColor.G / 255.0f, borderColor.B / 255.0f, borderColor.A / 255.0f);
                // Радиус и отступ
                gl.Vertex(x * r + centerPoint.X, openGLControlView.Height - (y * r + centerPoint.Y));

                // Матрица поворота
                t = x;
                x = cos * x - sin * y;
                y = sin * t + cos * y;
            }
            gl.End();
        }

        void DrawCircle(OpenGL gl, Interfaces.Point centerPoint, Interfaces.Point borderPoint, int numSegments)
        {
            DrawContourCircle(gl, centerPoint, borderPoint, numSegments);
            DrawFillCircle(gl, centerPoint, borderPoint, numSegments);
        }

        private void DrawPointsTriangle(OpenGL gl)
        {
            gl.PointSize(3);
            gl.Begin(OpenGL.GL_POINTS);
            for (int i = 0; i < iPointTriangle; i++)
            {
                gl.Vertex(last3Points[2-i].X, openGLControlView.Height - last3Points[2 - i].Y);
            }
            gl.End();
            gl.PointSize(1);
        }

        private void DrawPointsMutant(OpenGL gl)
        {
            gl.PointSize(3);
            gl.Begin(OpenGL.GL_POINTS);

            foreach (var point in pointsMutant)
            {
                gl.Vertex(point.X, openGLControlView.Height - point.Y);
            }
            gl.End();
            gl.PointSize(1);

            foreach (var fragment in listFragments)
            {
                foreach (var line in fragment)
                {
                    DrawLine(gl, line.Beg, line.End, borderColor);
                }
            }
        }

        private void DrawLine(OpenGL gl, Interfaces.Point firstPoint, Interfaces.Point lastPoint, Interfaces.Color color)
        {
            gl.Begin(OpenGL.GL_LINES);
            gl.Color(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
            gl.Vertex(firstPoint.X, openGLControlView.Height - firstPoint.Y);
            gl.Color(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
            gl.Vertex(lastPoint.X, openGLControlView.Height - lastPoint.Y);
            gl.End();
        }

        private void DrawText(OpenGL gl, Interfaces.Point placeTextPoint, string text)
        {
            double[] modelView = new double[16];
            double[] projection = new double[16];
            int[] viewport = new int[4];
            double[] tx = new double[1];
            double[] ty = new double[1];
            double[] tz = new double[1];

            gl.GetDouble(OpenGL.GL_MODELVIEW_MATRIX, modelView);
            gl.GetDouble(OpenGL.GL_PROJECTION_MATRIX, projection);
            gl.GetInteger(OpenGL.GL_VIEWPORT, viewport);

            gl.Project(placeTextPoint.X, placeTextPoint.Y, 0, modelView, projection, viewport, tx, ty, tz);
            gl.DrawText(Convert.ToInt32(tx[0]), openGLControlView.Height - Convert.ToInt32(ty[0]), 0.0f, 0.0f, 0.0f, "Arial", 12, text);
        }

        private void DrawAll()
        {
            OpenGL gl = openGLControlView.OpenGL;
            int contextWidth = openGLControlView.Width;
            int contextHeight = openGLControlView.Height;
            preGL2D(gl, openGLControlView.Width, openGLControlView.Height);

            Graphics graphics = this.CreateGraphics();

            // Берем наименьший dpi 
            minDpi = Math.Min(graphics.DpiX, graphics.DpiY);
            IEnumerable<IFigure> figures = logic.Figures;
            List<IFigure> figures2 = (List<IFigure>) logic.Figures;

            // убрать позже
            DrawText(gl, new Interfaces.Point(50, 50), figures2.Count.ToString());
            DrawText(gl, new Interfaces.Point(100, 50), ctrlPressed.ToString());

            int i = 0;
            foreach (var figure in figures)
            {
                Tuple<IEnumerable<trTriangle>, IEnumerable<ILineContainer>> triangulation;
                triangulation = figure.NewTriangulation(minDpi);

                // Обход по всем треугольникам
                foreach (var triangle in triangulation.Item1)
                {
                    DrawTriangle(gl, triangle, figure.FillColor);
                }

                // Обход по границам 

                // если выделено
                if (logic.CurientPickFigures.Contains(i))
                    gl.LineWidth(10.0f);

                foreach (var border in triangulation.Item2)
                {
                    Interfaces.Point firstPointBorder = null;
                    Interfaces.Point prevPointBorder = null;
                    bool isFirst = true;
                    foreach (var point in border.Path)
                    {
                        if (isFirst)
                        {
                            firstPointBorder = point;
                            isFirst = false;
                            prevPointBorder = point;
                            continue;
                        }
                        DrawLine(gl, prevPointBorder, point, figure.LineColor);
                        prevPointBorder = point;
                    }
                    DrawLine(gl, prevPointBorder, firstPointBorder, figure.LineColor);
                }
                gl.LineWidth(1.0f);
                ++i;
            }

            // Циркуль
            if (mode == Modes.Divider)
            {
                Interfaces.Point centerPoint = new Interfaces.Point(last3Points[1].X, last3Points[1].Y);
                Interfaces.Point borderPoint = new Interfaces.Point(last3Points[2].X, last3Points[2].Y);

                DrawContourCircle(gl, centerPoint, borderPoint, 360);
                DrawText(gl, centerPoint, string.Format("{0:0.000}", GetDistance(centerPoint, borderPoint)));
            }
            // Линейка
            else if(mode == Modes.Scale)
            {
                Interfaces.Point aPoint = new Interfaces.Point(last3Points[1].X, last3Points[1].Y);
                Interfaces.Point bPoint = new Interfaces.Point(last3Points[2].X, last3Points[2].Y);

                DrawLine(gl, aPoint, bPoint, new Interfaces.Color(0,0,0,255));
                DrawText(gl, new Interfaces.Point(aPoint.X + (bPoint.X - aPoint.X) / 2.0f, aPoint.Y + (bPoint.Y - aPoint.Y) / 2.0f - 10), string.Format("{0:0.000}", GetDistance(aPoint, bPoint)));
            }
            else if(readyToDrawTempFigure)
            {
                switch (selectedFigure)
                {
                    case Figures.Quadrangle:
                        DrawQuadrangle(gl,
                            new Interfaces.Point(last3Points[1].X, last3Points[1].Y),
                            new Interfaces.Point(last3Points[2].X, last3Points[2].Y));
                        break;
                    case Figures.Triangle:
                        if (iPointTriangle > 2)
                        {
                            DrawTriangle(gl,
                                new Interfaces.Point[3] {
                                new Interfaces.Point(last3Points[0].X, last3Points[0].Y),
                                new Interfaces.Point(last3Points[1].X, last3Points[1].Y),
                                new Interfaces.Point(last3Points[2].X, last3Points[2].Y)
                                });
                        }
                        else
                        {
                            DrawPointsTriangle(gl);
                        }
                        break;
                    case Figures.Line:
                        DrawLine(gl,
                            new Interfaces.Point(last3Points[1].X, last3Points[1].Y),
                            new Interfaces.Point(last3Points[2].X, last3Points[2].Y), borderColor);
                        break;
                    case Figures.Ellipse:
                        DrawEllipse(gl,
                            new Interfaces.Point(last3Points[1].X, last3Points[1].Y),
                            new Interfaces.Point(last3Points[2].X, last3Points[2].Y),
                            360
                            );
                        break;
                    case Figures.Circle:
                        DrawCircle(gl,
                            new Interfaces.Point(last3Points[1].X, last3Points[1].Y),
                            new Interfaces.Point(last3Points[2].X, last3Points[2].Y),
                            quantitySegments
                            );
                        break;
                    case Figures.Mutant:
                        DrawPointsMutant(gl);
                        break;
                }
            }

            gl.Flush();
            gl.Finish();
        }

        private void openGLControlView_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                if (isModeSelectFigures)
                {
                    int index = -2;
                    index = logic.GetIndexByPoint(new Interfaces.Point(e.X - canvasPositionX, e.Y - canvasPositionY));
                    if (!ctrlPressed)
                        listViewLayers.SelectedItems.Clear();

                    if (index != -1)
                    {
                        if (ctrlPressed)
                            if (logic.CurientPickFigures.Contains(index))
                                listViewLayers.Items[index].Selected = false;
                            else
                                listViewLayers.Items[index].Selected = true;
                        else
                            listViewLayers.Items[index].Selected = true;
                    }
                    isChangedOpenGLView = true;
                }
                else
                {
                    // тут к координатам точки прибавляю смещение полотна
                    AddNewLastPoint(new Interfaces.Point(e.X - canvasPositionX, e.Y - canvasPositionY));
                    readyToDrawTempFigure = true;
                    switch (selectedFigure)
                    {
                        case Figures.Triangle:
                            readyToDrawTempFigure = true;
                            if (iPointTriangle > 2)
                            {
                                iPointTriangle = 1;
                            }
                            else if (iPointTriangle == 2)
                            {
                                iPointTriangle++;

                                IFigure figure = Factory.Create(
                                    "Triangle",
                                    new Dictionary<string, object>()
                                    {
                                { "Point1", last3Points[0] },
                                { "Point2", last3Points[1] },
                                { "Point3", last3Points[2] },
                                { "BorderColor", borderColor},
                                { "FillColor", fillColor}
                                    });
                                listViewLayers.Items.Add("Triangle " + countTriangles.ToString());
                                countTriangles++;
                                // пока не дорисовали ещё точек - мы не будем отрисовывать 
                                // временную фигуру
                                readyToDrawTempFigure = false;

                                addCommand.Execute(figure);
                            }
                            else
                            {
                                iPointTriangle++;
                            }
                            break;

                        case Figures.Mutant:
                            pointsMutant.Add(new Interfaces.Point(e.X - canvasPositionX, e.Y - canvasPositionY));
                            break;

                        default:
                            last3Points[0] = new Interfaces.Point(e.X - canvasPositionX, e.Y - canvasPositionY);
                            last3Points[1] = new Interfaces.Point(e.X - canvasPositionX, e.Y - canvasPositionY);
                            last3Points[2] = new Interfaces.Point(e.X - canvasPositionX, e.Y - canvasPositionY);
                            break;
                    }

                    
                    

                    isMouseDown = true;
                    isStartDrag = true;
                    isChangedOpenGLView = true;
                }
            }
            if (e.Button == MouseButtons.Middle)
            {//Тут будем зажимать колёсико
                isMiddleButton = true;
                prevLocationX = e.X;
                prevLocationY = e.Y;
            }
        }

        private void openGLControlView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!isLoadOpenGLView) return;

                if (isMouseDown && (selectedFigure != Figures.Triangle))
                {
                    if (isStartDrag)
                    {
                        last3Points[2] = new Interfaces.Point(e.X - canvasPositionX, e.Y - canvasPositionY);
                    }

                    last3Points[2] = new Interfaces.Point(e.X - canvasPositionX, e.Y - canvasPositionY);

                    isChangedOpenGLView = true;
                }
                isStartDrag = false;
            }
            if (e.Button == MouseButtons.Middle)
            {//Тут будет двигаться полотно при зажатом колёсике
                canvasPositionX -= e.Location.X - prevLocationX;
                canvasPositionY -= e.Location.Y - prevLocationY;
                isChangedOpenGLView = true;
                Refresh();
                //glTranslate(e.Location.X - prevLocationX, prevLocationY - e.Location.Y, 0);
                prevLocationX = e.X;
                prevLocationY = e.Y;
            }
        }

        private void openGLControlView_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!isLoadOpenGLView) return;

            if (e.Delta > 0)
            {
                zoomOpenGLView *= 1.05f;
            }
            else
            {
                zoomOpenGLView *= 0.95f;
            }
            isChangedOpenGLView = true;
        }

        private void openGLControlView_OpenGLDraw(object sender, RenderEventArgs args)
        {
            if (!isLoadOpenGLView) return;
            if (isChangedOpenGLView)
            {
                DrawAll();
                isChangedOpenGLView = false;
            }
        }

        private void openGLControlView_OpenGLInitialized(object sender, EventArgs e)
        {
            isLoadOpenGLView = true;
        }

        private void openGLControlView_MouseUp(object sender, MouseEventArgs e)
        {
            if (isModeSelectFigures)
                return;
            else
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = false;

                switch (selectedFigure)
                {
                    case Figures.Line:

                        //containerFigures.addNewFigure(
                        //    Factory.Create(
                        //    "Line",
                        //    new Dictionary<string, object>()
                        //    {
                        //        { "Point1", last3Points[1] },
                        //        { "Point2", last3Points[2] }
                        //    })
                        //);
                        break;
                    case Figures.Quadrangle:
                        {
                            IFigure figure =
                                Factory.Create(
                                "Rectangle",
                                new Dictionary<string, object>()
                                {
                            { "DownLeft", new Interfaces.Point(last3Points[1].X, last3Points[2].Y)  },
                            { "UpRight", new Interfaces.Point(last3Points[2].X, last3Points[1].Y) },
                            { "BorderColor", borderColor},
                            { "FillColor", fillColor}

                            });
                        listViewLayers.Items.Add("Rectangle " + countRectangles.ToString());
                        countRectangles++;
                        // пока не дорисовали ещё точек - мы не будем отрисовывать 
                        // временную фигуру
                        readyToDrawTempFigure = false;
                        addCommand.Execute(figure);

                        }

                        break;

                    case Figures.Circle:
                        {
                            Interfaces.Point centerPoint = new Interfaces.Point(last3Points[1].X, last3Points[1].Y);
                            Interfaces.Point borderPoint = new Interfaces.Point(last3Points[2].X, last3Points[2].Y);

                            double radius = GetDistance(centerPoint, borderPoint);

                            double a = radius;
                            double b = radius;


                            Interfaces.Point begPoint = new Interfaces.Point(centerPoint.X + radius, centerPoint.Y);
                            Interfaces.Point endPoint = new Interfaces.Point(centerPoint.X - radius, centerPoint.Y);
                            
                            List<List<Segment>> fragments = new List<List<Segment>>();
                            List<Segment> segments = new List<Segment>();
                            segments.Add(new EllipseArc(centerPoint, a, b, 0, Math.PI, begPoint, endPoint, 0));
                            segments.Add(new EllipseArc(centerPoint, a, b, 0, Math.PI, endPoint, begPoint, 0));

                            fragments.Add(segments);

                            IFigure figure = new Mutant(fragments, borderColor, fillColor, 1);
                            addCommand.Execute(figure);
                        }
                        break;

                    case Figures.Ellipse:

                        {
                            
                            Interfaces.Point centerPoint = new Interfaces.Point(last3Points[1].X + (last3Points[2].X - last3Points[1].X) / 2.0, last3Points[1].Y + (last3Points[2].Y - last3Points[1].Y) / 2.0);
                            
                            double radius = GetDistance(last3Points[1], last3Points[2]) /2.0;

                            double a = Math.Abs(centerPoint.X - last3Points[1].X);
                            double b = Math.Abs(centerPoint.Y - last3Points[1].Y);


                            Interfaces.Point begPoint = new Interfaces.Point(centerPoint.X + a, centerPoint.Y);
                            Interfaces.Point endPoint = new Interfaces.Point(centerPoint.X - a, centerPoint.Y);

                            List<List<Segment>> fragments = new List<List<Segment>>();
                            List<Segment> segments = new List<Segment>();
                            segments.Add(new EllipseArc(centerPoint, a, b, 0, Math.PI, begPoint, endPoint, 0));
                            segments.Add(new EllipseArc(centerPoint, a, b, 0, Math.PI, endPoint, begPoint, 0));

                            fragments.Add(segments);

                            IFigure figure = new Mutant(fragments, borderColor, fillColor, 1);
                            addCommand.Execute(figure);
                        }
                        break;
                }
                isStartDrag = false;
            }
            if (e.Button == MouseButtons.Middle)
            {
                isMiddleButton = false;
            }
        }

        private void buttonTriangle_Click(object sender, EventArgs e)
        {
            isModeSelectFigures = false;
            mode = Modes.Draw;
            selectedFigure = Figures.Triangle;
        }

        private void buttonSelectFigure_Click(object sender, EventArgs e)
        {
            isModeSelectFigures = !isModeSelectFigures;
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //logic.executeCommand("create_new");
        }

        private void открытьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (loadCommand.CanExecute(null))
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 2;
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        loadCommand.Execute(openFileDialog1.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    }
                }
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveCommand.CanExecute(null))
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    saveCommand.Execute(saveFileDialog1.FileName);
                }
            }
        }

        private void buttonEllipse_Click(object sender, EventArgs e)
        {
            isModeSelectFigures = false;
            mode = Modes.Draw;
            selectedFigure = Figures.Ellipse;
        }

        private void buttonCircle_Click(object sender, EventArgs e)
        {
            isModeSelectFigures = false;
            mode = Modes.Draw;
            selectedFigure = Figures.Circle;
        }

        private void button_choose_line_color_Click(object sender, EventArgs e)
        {
            isModeSelectFigures = false;
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                borderColor = new Interfaces.Color(cd.Color.R, cd.Color.G, cd.Color.B, cd.Color.A);
                button_choose_line_color.BackColor = cd.Color;


                if (editCommand.CanExecute(null))
                {
                    editCommand.Execute(new Tuple<Interfaces.Color, bool>(borderColor, false));
                }
            }
            isChangedOpenGLView = true;
        }

        private void buttonScaleLine_Click(object sender, EventArgs e)
        {
            isModeSelectFigures = false;
            selectedFigure = Figures.Line;
            if (mode != Modes.Scale)
            {
                mode = Modes.Scale;
            }
            else
            {
                mode = Modes.Draw;
            }
        }

        private void buttonDivider_Click(object sender, EventArgs e)
        {
            isModeSelectFigures = false;
            selectedFigure = Figures.Line;
            if (mode != Modes.Divider)
            {
                mode = Modes.Divider;
            }
            else
            {
                mode = Modes.Draw;
            }
        }

        private void buttonIntersection_Click(object sender, EventArgs e)
        {

        }

        private void buttonUnion_Click(object sender, EventArgs e)
        {

        }

        private void buttonStartMutant_Click(object sender, EventArgs e)
        {
            isModeSelectFigures = false;
            pointsMutant = new List<Interfaces.Point>();

            selectedFigure = Figures.Mutant;
            pointsMutant.Clear();
            listFragments.Clear();
        }

        private void buttonSaveMutant_Click(object sender, EventArgs e)
        {
            isModeSelectFigures = false;
            if (selectedFigure == Figures.Mutant)
            {
                NextFragmentMutant();

                IFigure figure = new Mutant(
                    listFragments,
                    borderColor,
                    fillColor,
                    minDpi);

                addCommand.Execute(figure);

                pointsMutant.Clear();
                listFragments.Clear();
                isChangedOpenGLView = true;
            }
        }

        void NextFragmentMutant()
        {
            isModeSelectFigures = false;
            if (pointsMutant.Count > 0)
            {
                List<Segment> segments = new List<Segment>();

                Interfaces.Point firstPointBorder = null;
                Interfaces.Point prevPointBorder = null;
                bool isFirst = true;


                foreach (var point in pointsMutant)
                {
                    if (isFirst)
                    {
                        firstPointBorder = point;
                        isFirst = false;
                        prevPointBorder = point;
                        continue;
                    }
                    segments.Add(new Line(prevPointBorder, point));
                    prevPointBorder = point;
                }
                segments.Add(new Line(prevPointBorder, firstPointBorder));

                listFragments.Add(segments);
                pointsMutant.Clear();
            }
        }

        private void buttonMutantNext_Click(object sender, EventArgs e)
        {
            isModeSelectFigures = false;
            NextFragmentMutant();
            isChangedOpenGLView = true;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (removeCommand.CanExecute(null))
            {
                removeCommand.Execute(null);
                foreach (ListViewItem item in listViewLayers.SelectedItems)
                    item.Remove(); 
            }
            isChangedOpenGLView = true;
        }       

        private void listViewLayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            clearCurrentCommand.Execute(null);
            foreach (int item in listViewLayers.SelectedIndices)
                pickFromListCommand.Execute(new Tuple<int, bool>(item, ctrlPressed));

            if (listViewLayers.SelectedItems.Count == 1)
            {
                buttonDownLayer.Enabled = true;
                buttonUpLayer.Enabled = true;
            }
            else
            {
                buttonDownLayer.Enabled = false;
                buttonUpLayer.Enabled = false;
            }
            isChangedOpenGLView = true;
        }

        private void buttonUpLayer_Click(object sender, EventArgs e)
        {
            if (moveLayerCommand.CanExecute(false))
                moveLayerCommand.Execute(false);
            listViewLayers.Focus();
            var item = listViewLayers.SelectedItems[0];
            var index = listViewLayers.SelectedIndices[0];
            listViewLayers.Items.Remove(item);
            listViewLayers.Items.Insert(index - 1, item);
            listViewLayers.Items[index - 1].Selected = true;
            isChangedOpenGLView = true;
        }

        private void buttobDownLayer_Click(object sender, EventArgs e)
        {
            if (moveLayerCommand.CanExecute(true))
                moveLayerCommand.Execute(true);
            listViewLayers.Focus();
            var item = listViewLayers.SelectedItems[0];
            var index = listViewLayers.SelectedIndices[0];
            listViewLayers.Items.Remove(item);
            listViewLayers.Items.Insert(index + 1, item);
            listViewLayers.Items[index + 1].Selected = true;
            isChangedOpenGLView = true;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
                ctrlPressed = true;
            // убрать
            DrawAll();
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.ControlKey)
                ctrlPressed = false;
            // убрать
            DrawAll();
        }
          

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (copyCommand.CanExecute(null))
            {
                copyCommand.Execute(null);
                isChangedOpenGLView = true;
            }
        }

        private void впередToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(redoCommand.CanExecute(null))
            {
                redoCommand.Execute(null);
                isChangedOpenGLView = true;
            }
        }

        private void назадToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (undoCommand.CanExecute(null))
            {
                undoCommand.Execute(null);
                isChangedOpenGLView = true;
            }
        }

        private void button_choose_fill_color_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                fillColor = new Interfaces.Color(cd.Color.R, cd.Color.G, cd.Color.B, cd.Color.A);
                button_choose_fill_color.BackColor = cd.Color;

                if (isModeSelectFigures)
                {
                    if (editCommand.CanExecute(null))
                    {
                        editCommand.Execute(new Tuple<Interfaces.Color, bool>(fillColor, true));
                    }
                }
            }
            isChangedOpenGLView = true;
        }

        private void openGLControlView_SizeChanged(object sender, EventArgs e)
        {
            isChangedOpenGLView = true;
        }
    }
}
