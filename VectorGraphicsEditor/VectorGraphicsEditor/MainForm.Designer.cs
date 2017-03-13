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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.PickColor = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.новыйToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.правкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.повторитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.отменитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonElipse = new System.Windows.Forms.Button();
            this.buttonSelection = new System.Windows.Forms.Button();
            this.buttonUnion = new System.Windows.Forms.Button();
            this.buttonIntersect = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.openGLControlView)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // openGLControlView
            // 
            this.openGLControlView.DrawFPS = false;
            this.openGLControlView.FrameRate = 60;
            this.openGLControlView.Location = new System.Drawing.Point(12, 27);
            this.openGLControlView.Name = "openGLControlView";
            this.openGLControlView.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL1_1;
            this.openGLControlView.RenderContextType = SharpGL.RenderContextType.FBO;
            this.openGLControlView.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.openGLControlView.Size = new System.Drawing.Size(907, 657);
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
            this.buttonLine.Location = new System.Drawing.Point(62, 19);
            this.buttonLine.Name = "buttonLine";
            this.buttonLine.Size = new System.Drawing.Size(50, 50);
            this.buttonLine.TabIndex = 1;
            this.buttonLine.Text = "Линия";
            this.buttonLine.UseVisualStyleBackColor = true;
            this.buttonLine.Click += new System.EventHandler(this.buttonLine_Click);
            // 
            // buttonRect
            // 
            this.buttonRect.Location = new System.Drawing.Point(6, 19);
            this.buttonRect.Name = "buttonRect";
            this.buttonRect.Size = new System.Drawing.Size(50, 50);
            this.buttonRect.TabIndex = 2;
            this.buttonRect.Text = "Прямоугольник";
            this.buttonRect.UseVisualStyleBackColor = true;
            this.buttonRect.Click += new System.EventHandler(this.buttonRect_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonElipse);
            this.groupBox1.Controls.Add(this.buttonRect);
            this.groupBox1.Controls.Add(this.buttonLine);
            this.groupBox1.Location = new System.Drawing.Point(925, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(121, 228);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Доступные фигуры";
            // 
            // PickColor
            // 
            this.PickColor.Location = new System.Drawing.Point(6, 19);
            this.PickColor.Name = "PickColor";
            this.PickColor.Size = new System.Drawing.Size(50, 50);
            this.PickColor.TabIndex = 4;
            this.PickColor.Text = "Цвет";
            this.PickColor.UseVisualStyleBackColor = true;
            this.PickColor.Click += new System.EventHandler(this.PickColor_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.правкаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1056, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.новыйToolStripMenuItem,
            this.сохранитьToolStripMenuItem,
            this.загрузитьToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // сохранитьToolStripMenuItem
            // 
            this.сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            this.сохранитьToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.сохранитьToolStripMenuItem.Text = "Сохранить";
            // 
            // загрузитьToolStripMenuItem
            // 
            this.загрузитьToolStripMenuItem.Name = "загрузитьToolStripMenuItem";
            this.загрузитьToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.загрузитьToolStripMenuItem.Text = "Открыть";
            // 
            // новыйToolStripMenuItem
            // 
            this.новыйToolStripMenuItem.Name = "новыйToolStripMenuItem";
            this.новыйToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.новыйToolStripMenuItem.Text = "Создать";
            // 
            // правкаToolStripMenuItem
            // 
            this.правкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.повторитьToolStripMenuItem,
            this.отменитьToolStripMenuItem});
            this.правкаToolStripMenuItem.Name = "правкаToolStripMenuItem";
            this.правкаToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.правкаToolStripMenuItem.Text = "Правка";
            // 
            // повторитьToolStripMenuItem
            // 
            this.повторитьToolStripMenuItem.Name = "повторитьToolStripMenuItem";
            this.повторитьToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.повторитьToolStripMenuItem.Text = "Повторить";
            // 
            // отменитьToolStripMenuItem
            // 
            this.отменитьToolStripMenuItem.Name = "отменитьToolStripMenuItem";
            this.отменитьToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.отменитьToolStripMenuItem.Text = "Отменить";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonIntersect);
            this.groupBox2.Controls.Add(this.buttonUnion);
            this.groupBox2.Controls.Add(this.buttonSelection);
            this.groupBox2.Controls.Add(this.PickColor);
            this.groupBox2.Location = new System.Drawing.Point(925, 261);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(121, 189);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Инструменты";
            // 
            // buttonElipse
            // 
            this.buttonElipse.Location = new System.Drawing.Point(6, 75);
            this.buttonElipse.Name = "buttonElipse";
            this.buttonElipse.Size = new System.Drawing.Size(50, 50);
            this.buttonElipse.TabIndex = 4;
            this.buttonElipse.Text = "Овал";
            this.buttonElipse.UseVisualStyleBackColor = true;
            // 
            // buttonSelection
            // 
            this.buttonSelection.Location = new System.Drawing.Point(62, 19);
            this.buttonSelection.Name = "buttonSelection";
            this.buttonSelection.Size = new System.Drawing.Size(50, 50);
            this.buttonSelection.TabIndex = 6;
            this.buttonSelection.Text = "Выбор";
            this.buttonSelection.UseVisualStyleBackColor = true;
            // 
            // buttonUnion
            // 
            this.buttonUnion.Location = new System.Drawing.Point(6, 75);
            this.buttonUnion.Name = "buttonUnion";
            this.buttonUnion.Size = new System.Drawing.Size(50, 50);
            this.buttonUnion.TabIndex = 7;
            this.buttonUnion.Text = "Объединение";
            this.buttonUnion.UseVisualStyleBackColor = true;
            // 
            // buttonIntersect
            // 
            this.buttonIntersect.Location = new System.Drawing.Point(62, 75);
            this.buttonIntersect.Name = "buttonIntersect";
            this.buttonIntersect.Size = new System.Drawing.Size(50, 50);
            this.buttonIntersect.TabIndex = 8;
            this.buttonIntersect.Text = "Пересечение";
            this.buttonIntersect.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1056, 696);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.openGLControlView);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Графический редактор";
            ((System.ComponentModel.ISupportInitialize)(this.openGLControlView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SharpGL.OpenGLControl openGLControlView;
        private System.Windows.Forms.Button buttonLine;
        private System.Windows.Forms.Button buttonRect;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button PickColor;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem новыйToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загрузитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem правкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem повторитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem отменитьToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonElipse;
        private System.Windows.Forms.Button buttonSelection;
        private System.Windows.Forms.Button buttonIntersect;
        private System.Windows.Forms.Button buttonUnion;
    }
}

