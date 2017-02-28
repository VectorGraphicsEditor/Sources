namespace VectorGraphicsEditor
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.openGLControlView = new SharpGL.OpenGLControl();
            this.buttonLine = new System.Windows.Forms.Button();
            this.buttonRect = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.openGLControlView)).BeginInit();
            this.SuspendLayout();
            // 
            // openGLControlView
            // 
            this.openGLControlView.DrawFPS = false;
            this.openGLControlView.Location = new System.Drawing.Point(12, 82);
            this.openGLControlView.Name = "openGLControlView";
            this.openGLControlView.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL1_1;
            this.openGLControlView.RenderContextType = SharpGL.RenderContextType.DIBSection;
            this.openGLControlView.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.openGLControlView.Size = new System.Drawing.Size(1113, 602);
            this.openGLControlView.TabIndex = 0;
            this.openGLControlView.OpenGLInitialized += new System.EventHandler(this.openGLControlView_OpenGLInitialized);
            this.openGLControlView.OpenGLDraw += new SharpGL.RenderEventHandler(this.openGLControlView_OpenGLDraw);
            this.openGLControlView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.openGLControlView_MouseDown);
            this.openGLControlView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.openGLControlView_MouseMove);
            this.openGLControlView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.openGLControlView_MouseUp);
            this.openGLControlView.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.openGLControlView_MouseWheel);
            // 
            // buttonLine
            // 
            this.buttonLine.Location = new System.Drawing.Point(33, 24);
            this.buttonLine.Name = "buttonLine";
            this.buttonLine.Size = new System.Drawing.Size(120, 38);
            this.buttonLine.TabIndex = 1;
            this.buttonLine.Text = "Линия";
            this.buttonLine.UseVisualStyleBackColor = true;
            this.buttonLine.Click += new System.EventHandler(this.buttonLine_Click);
            // 
            // buttonRect
            // 
            this.buttonRect.Location = new System.Drawing.Point(191, 24);
            this.buttonRect.Name = "buttonRect";
            this.buttonRect.Size = new System.Drawing.Size(120, 38);
            this.buttonRect.TabIndex = 2;
            this.buttonRect.Text = "Прямоугольник";
            this.buttonRect.UseVisualStyleBackColor = true;
            this.buttonRect.Click += new System.EventHandler(this.buttonRect_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1137, 696);
            this.Controls.Add(this.buttonRect);
            this.Controls.Add(this.buttonLine);
            this.Controls.Add(this.openGLControlView);
            this.Name = "MainForm";
            this.Text = "Графический редактор";
            ((System.ComponentModel.ISupportInitialize)(this.openGLControlView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private SharpGL.OpenGLControl openGLControlView;
        private System.Windows.Forms.Button buttonLine;
        private System.Windows.Forms.Button buttonRect;
    }
}

