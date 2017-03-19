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
        Container containerFigures = new Logic.Container();

        // Наименьший dpi 
        double minDpi = 1;

        private enum Figures {
            Line,
            Quadrangle,
            Triangle,
            Circle,
            Ellipse
        };

        // Временно
        class LineTemp : ILineContainer
        {
            public IEnumerable<Interfaces.Point> Path
            {
                get
                {
                    return new List<Interfaces.Point>()
                    {
                        new Interfaces.Point(200,200),
                        new Interfaces.Point(500,50),
                        new Interfaces.Point(100,20)
                    };
                }
            }
        }

        // Временно, пока не можем получать фигуры от логики
        List<IEnumerable<ILineContainer>> listForDisplay = new List<IEnumerable<ILineContainer>>()
        {
            new List<ILineContainer>() {
                new LineTemp()
            }
        };

        private Interfaces.Point[] last3Points;
        private Interfaces.Color borderColor = new Interfaces.Color(0, 0, 0, 255);
        private Interfaces.Color fillColor = new Interfaces.Color(0, 0, 255, 10);

        private int iPointTriangle = 0;

        private bool isMouseDown = false;
        private bool isChangedOpenGLView = true;
        private bool isLoadOpenGLView = false;
        private bool isStartDrag = false;
        private bool isModeSelectFigures = false;

        private float zoomOpenGLView = 1.0f;

        private Figures selectedFigure = Figures.Line;

        public MainForm()
        {
            InitializeComponent();

            last3Points = new Interfaces.Point[3] {
                new Interfaces.Point(0, 0),
                new Interfaces.Point(0, 0),
                new Interfaces.Point(0, 0)
            };
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
            selectedFigure = Figures.Quadrangle;
        }

        private void buttonLine_Click(object sender, EventArgs e)
        {
            selectedFigure = Figures.Line;
        }
        private void DrawTriangle(OpenGL gl, Interfaces.Point[] points)
        {
            gl.Color(0, 0, 255, 0.14);
            gl.Begin(OpenGL.GL_TRIANGLES);
            gl.Vertex(points[0].X, openGLControlView.Height - points[0].Y);
            gl.Vertex(points[1].X, openGLControlView.Height - points[1].Y);
            gl.Vertex(points[2].X, openGLControlView.Height - points[2].Y);
            gl.End();

            gl.Color(0, 0, 0);
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
            gl.Color(0, 0, 255, 0.14);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Vertex(firstPoint.X, openGLControlView.Height - firstPoint.Y);
            gl.Vertex(firstPoint.X, openGLControlView.Height - lastPoint.Y);
            gl.Vertex(lastPoint.X, openGLControlView.Height - lastPoint.Y);
            gl.Vertex(lastPoint.X, openGLControlView.Height - firstPoint.Y);
            gl.End();

            gl.Color(0, 0, 0);
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

            gl.Color(0, 0, 0);
            gl.Begin(OpenGL.GL_LINE_LOOP);
            for (int ii = 0; ii < numSegments; ii++)
            {
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

            gl.Color(0, 0, 255, 0.14);
            gl.Begin(OpenGL.GL_POLYGON);
            for (int ii = 0; ii < numSegments; ii++)
            {
                // Радиус и отступ
                gl.Vertex(x * r.X + centerPoint.X, openGLControlView.Height - (y * r.Y + centerPoint.Y));

                // Матрица поворота
                t = x;
                x = cos * x - sin * y;
                y = sin * t + cos * y;
            }
            gl.End();
        }

        void DrawCircle(OpenGL gl, Interfaces.Point centerPoint, Interfaces.Point borderPoint, int numSegments)
        {
            double r = Math.Sqrt((borderPoint.X - centerPoint.X) * (borderPoint.X - centerPoint.X) + (borderPoint.Y - centerPoint.Y) * (borderPoint.Y - centerPoint.Y));
            double theta = 2.0 * Math.PI / (double)(numSegments);
            double cos = Math.Cos(theta);
            double sin = Math.Sin(theta);
            double t;

            double x = 1;
            double y = 0;

            gl.Color(0, 0, 0);
            gl.Begin(OpenGL.GL_LINE_LOOP);
            for (int ii = 0; ii < numSegments; ii++)
            {
                // Радиус и отступ
                gl.Vertex(x * r + centerPoint.X, openGLControlView.Height - (y * r + centerPoint.Y));

                // Матрица поворота
                t = x;
                x = cos * x - sin * y;
                y = sin * t + cos * y;
            }
            gl.End();


            x = 1;
            y = 0;

            gl.Color(0, 0, 255, 0.14);
            gl.Begin(OpenGL.GL_POLYGON);
            for (int ii = 0; ii < numSegments; ii++)
            {
                // Радиус и отступ
                gl.Vertex(x * r + centerPoint.X, openGLControlView.Height - (y * r + centerPoint.Y));

                // Матрица поворота
                t = x;
                x = cos * x - sin * y;
                y = sin * t + cos * y;
            }
            gl.End();
        }


        private void DrawPoints(OpenGL gl)
        {
            gl.PointSize(2);
            gl.Begin(OpenGL.GL_POINTS);
            for (int i = 0; i < iPointTriangle; i++)
            {
                gl.Vertex(last3Points[2-i].X, openGLControlView.Height - last3Points[2 - i].Y);
            }
            gl.End();
            gl.PointSize(1);
        }

        private void DrawLine(OpenGL gl, Interfaces.Point firstPoint, Interfaces.Point lastPoint)
        {
            gl.Color(0, 0, 0);
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(firstPoint.X, openGLControlView.Height - firstPoint.Y);
            gl.Vertex(lastPoint.X, openGLControlView.Height - lastPoint.Y);
            gl.End();
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
                        
            //foreach (var obj in listForDisplay)
            //{
            //    foreach (var line in obj)
            //    {
            //        int i = 0;
            //        Interfaces.Point[] pointsLines = new Interfaces.Point[3];
            //        foreach (var path in line.Path)
            //        {
            //            pointsLines[i] = path;
            //            i++;
            //        }

            //        DrawTriangle(gl, pointsLines);
            //    }
            //}

            List<IFigure> figures = containerFigures.getFigures();
            Dictionary<string, object> figureParameters;

            foreach (var figure in figures)
            {
                figureParameters = figure.Parameters;
                if (figure is Rectangle)
                {
                    //Interfaces.Point leftDownPoint = (Interfaces.Point)figureParameters["DownLeft"];
                    //Interfaces.Point rightUpPoint = (Interfaces.Point)figureParameters["UpRight"];
                    //DrawQuadrangle(gl,
                    //    new Interfaces.Point(leftDownPoint.X, rightUpPoint.Y),
                    //    new Interfaces.Point(rightUpPoint.X, leftDownPoint.Y));
                }
            }

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
                        DrawPoints(gl);
                    }
                    break;
                case Figures.Line:
                    DrawLine(gl,
                        new Interfaces.Point(last3Points[1].X, last3Points[1].Y),
                        new Interfaces.Point(last3Points[2].X, last3Points[2].Y));
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
                        360
                        );
                    break;
            }

            gl.Flush();
            gl.Finish();
        }

        private void openGLControlView_MouseDown(object sender, MouseEventArgs e)
        {
            if (isModeSelectFigures)
            {
                // logic.SelectFigure(e.X, e.Y);
            }
            else
            {
                AddNewLastPoint(new Interfaces.Point(e.X, e.Y));

                if (selectedFigure == Figures.Triangle)
                {
                    if (iPointTriangle > 2)
                    {
                        iPointTriangle = 1;
                    }
                    else if (iPointTriangle == 2)
                    {
                        iPointTriangle++;

                        //IFigure figure = Factory.Create(
                        //    "Triangle",
                        //    new Dictionary<string, object>()
                        //    {
                        //        { "Point1", last3Points[0] },
                        //        { "Point2", last3Points[1] },
                        //        { "Point3", last3Points[2] }
                        //    });

                        //containerFigures.addNewFigure(figure);
                    }
                    else
                    {
                        iPointTriangle++;
                    }
                }
                else
                {
                    last3Points[0] = new Interfaces.Point(e.X, e.Y);
                    last3Points[1] = new Interfaces.Point(e.X, e.Y);
                    last3Points[2] = new Interfaces.Point(e.X, e.Y);
                }

                isMouseDown = true;
                isStartDrag = true;
                isChangedOpenGLView = true;
            }
        }

        private void openGLControlView_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isLoadOpenGLView) return;

            if (isMouseDown && (selectedFigure != Figures.Triangle))
            {
                if (isStartDrag)
                {
                    last3Points[2] = new Interfaces.Point(e.X, e.Y);
                }

                last3Points[2] = new Interfaces.Point(e.X, e.Y);

                isChangedOpenGLView = true;
            }
            isStartDrag = false;
        }

        private void openGLControlView_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!isLoadOpenGLView) return;

            if (e.Delta < 0)
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

                    containerFigures.addNewFigure(
                        Factory.Create(
                        "Rectangle",
                        new Dictionary<string, object>()
                        {
                            { "DownLeft", new Interfaces.Point(last3Points[1].X, last3Points[2].Y)  },
                            { "UpRight", new Interfaces.Point(last3Points[2].X, last3Points[1].Y) },
                            { "BorderColor", borderColor},
                            { "FillColor", fillColor}
                        })
                    );
                    break;

                case Figures.Circle:

                    //containerFigures.addNewFigure(
                    //    Factory.Create(
                    //    "Circle",
                    //    new Dictionary<string, object>()
                    //    {
                    //        { "Point1", last3Points[1] },
                    //        { "Point2", last3Points[2] }
                    //    })
                    //);
                    break;

                case Figures.Ellipse:

                    //containerFigures.addNewFigure(
                    //    Factory.Create(
                    //    "Ellipse",
                    //    new Dictionary<string, object>()
                    //    {
                    //        { "Point1", last3Points[1] },
                    //        { "Point2", last3Points[2] }
                    //    })
                    //);
                    break;
            }
            isStartDrag = false;
        }

        private void buttonTriangle_Click(object sender, EventArgs e)
        {
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
            //logic.executeCommand("open");
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //logic.executeCommand("save");
        }

        private void buttonEllipse_Click(object sender, EventArgs e)
        {
            selectedFigure = Figures.Ellipse;
        }

        private void buttonCircle_Click(object sender, EventArgs e)
        {
            selectedFigure = Figures.Circle;
        }
    }
}
