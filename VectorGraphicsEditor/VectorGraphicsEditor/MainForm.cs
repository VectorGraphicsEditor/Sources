using SharpGL;

using System;
using System.Windows.Forms;
using System.Drawing;

namespace VectorGraphicsEditor
{
    public partial class MainForm : Form
    {
        private class Point
        {
            public Point() { }
            public Point(float x, float y)
            { X = x; Y = y; }

            public float X { get; set; }
            public float Y { get; set; }
        };

        private enum Figures { Line, Quadrangle };

        private Point firstPoint = new Point(0, 0);
        private Point lastPoint = new Point(0, 0);

        private bool isMouseDown = false;

        private bool isChangedOpenGLView = true;
        private bool isLoadOpenGLView = false;

        private float zoomOpenGLView = 1.0f;

        private Figures selectedFigure = Figures.Line;

        Color currentColor = Color.Black;
        

        

        public MainForm()
        {
            InitializeComponent();
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

            gl.ShadeModel(OpenGL.GL_SMOOTH);               // Enable Smooth Shading
            gl.PixelStore(OpenGL.GL_UNPACK_ALIGNMENT, 4);  //4-byte pixel alignment
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
        
        private void openGLControlView_MouseDown(object sender, MouseEventArgs e)
        {
            firstPoint.X = e.X;
            firstPoint.Y = e.Y;

            isMouseDown = true;
            isChangedOpenGLView = true;
            // Logic.IGUI.executeCommand("mouse_down", true);
        }

        private void openGLControlView_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isLoadOpenGLView) return;

            if (isMouseDown)
            {
                lastPoint.X = e.X;
                lastPoint.Y = e.Y;

                isChangedOpenGLView = true;
            }
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
            // Logic.IGUI.executeCommand("zoom", zoomOpenGLView);
        }

        private void buttonRect_Click(object sender, EventArgs e)
        {
            selectedFigure = Figures.Quadrangle;
        }

        private void buttonLine_Click(object sender, EventArgs e)
        {
            selectedFigure = Figures.Line;
        }

        private void DrawQuadrangle(OpenGL gl, float firstPointX, float firstPointY, float lastPointX, float lastPointY)
        {
            gl.Begin(OpenGL.GL_QUADS);

            gl.Color(currentColor.R, currentColor.G, currentColor.B);
            gl.Vertex(firstPointX, firstPointY);

            gl.Vertex(firstPointX, lastPointY);

            gl.Vertex(lastPointX, lastPointY);

            gl.Vertex(lastPointX, firstPointY);
            gl.End();
        }

        private void DrawLine(OpenGL gl, float firstPointX, float firstPointY, float lastPointX, float lastPointY)
        {
            gl.Begin(OpenGL.GL_LINES);

            gl.Color(currentColor.R, currentColor.G, currentColor.B);
            gl.Vertex(firstPointX, firstPointY);

            gl.Vertex(lastPointX, lastPointY);
            gl.End();
        }

        private void openGLControlView_OpenGLDraw(object sender, RenderEventArgs args)
        {
            if (!isLoadOpenGLView) return;
            if (isChangedOpenGLView)
            {
                OpenGL gl = openGLControlView.OpenGL;
                int contextWidth = openGLControlView.Width;
                int contextHeight = openGLControlView.Height;
                preGL2D(gl, openGLControlView.Width, openGLControlView.Height);
                
                // IEnumerable<IFigureScaled> = Logic.IGUI.getToDraw();

                switch (selectedFigure)
                {
                    case Figures.Quadrangle:
                        DrawQuadrangle(gl,
                            firstPoint.X,
                            openGLControlView.Height - firstPoint.Y,
                            lastPoint.X,
                            openGLControlView.Height - lastPoint.Y);
                        break;
                    case Figures.Line:
                        DrawLine(gl,
                            firstPoint.X,
                            openGLControlView.Height - firstPoint.Y,
                            lastPoint.X,
                            openGLControlView.Height - lastPoint.Y);
                        break;
                }

                gl.Flush();
                gl.Finish();
                isChangedOpenGLView = false;
            }
        }

        private void openGLControlView_OpenGLInitialized(object sender, EventArgs e)
        {
            isLoadOpenGLView = true;
        }

        private void openGLControlView_MouseUp(object sender, MouseEventArgs e)
        {
            // Logic.IGUI.executeCommand("mouse_down", false);
            isMouseDown = false;
        }

        private void PickColor_Click(object sender, EventArgs e)
        {
            if(colorDialog1.ShowDialog() == DialogResult.OK)
            {
                PickColor.BackColor = colorDialog1.Color;
                currentColor = colorDialog1.Color;

            }
        }
    }
}
