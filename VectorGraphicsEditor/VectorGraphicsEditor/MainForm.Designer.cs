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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.openGLControlView = new SharpGL.OpenGLControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.открытьToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button_choose_line_color = new System.Windows.Forms.Button();
            this.button_choose_fill_color = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonSaveMutant = new System.Windows.Forms.Button();
            this.buttonMinus = new System.Windows.Forms.Button();
            this.buttonMutantNext = new System.Windows.Forms.Button();
            this.buttonStartMutant = new System.Windows.Forms.Button();
            this.buttonUnion = new System.Windows.Forms.Button();
            this.buttonIntersection = new System.Windows.Forms.Button();
            this.buttonDivider = new System.Windows.Forms.Button();
            this.buttonScaleLine = new System.Windows.Forms.Button();
            this.buttonCircle = new System.Windows.Forms.Button();
            this.buttonEllipse = new System.Windows.Forms.Button();
            this.buttonSelectFigure = new System.Windows.Forms.Button();
            this.buttonTriangle = new System.Windows.Forms.Button();
            this.buttonRect = new System.Windows.Forms.Button();
            this.buttonLine = new System.Windows.Forms.Button();
            this.правкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.копироватьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.впередToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.назадToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.openGLControlView)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openGLControlView
            // 
            this.openGLControlView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.openGLControlView.AutoSize = true;
            this.openGLControlView.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.openGLControlView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.openGLControlView.DrawFPS = false;
            this.openGLControlView.Location = new System.Drawing.Point(12, 82);
            this.openGLControlView.Name = "openGLControlView";
            this.openGLControlView.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL1_1;
            this.openGLControlView.RenderContextType = SharpGL.RenderContextType.DIBSection;
            this.openGLControlView.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.openGLControlView.Size = new System.Drawing.Size(1564, 728);
            this.openGLControlView.TabIndex = 0;
            this.openGLControlView.OpenGLInitialized += new System.EventHandler(this.openGLControlView_OpenGLInitialized);
            this.openGLControlView.OpenGLDraw += new SharpGL.RenderEventHandler(this.openGLControlView_OpenGLDraw);
            this.openGLControlView.SizeChanged += new System.EventHandler(this.openGLControlView_SizeChanged);
            this.openGLControlView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.openGLControlView_MouseDown);
            this.openGLControlView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.openGLControlView_MouseMove);
            this.openGLControlView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.openGLControlView_MouseUp);
            this.openGLControlView.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.openGLControlView_MouseWheel);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.правкаToolStripMenuItem,
            this.справкаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1588, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.открытьToolStripMenuItem1,
            this.сохранитьToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "Создать";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.открытьToolStripMenuItem_Click);
            // 
            // открытьToolStripMenuItem1
            // 
            this.открытьToolStripMenuItem1.Name = "открытьToolStripMenuItem1";
            this.открытьToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.открытьToolStripMenuItem1.Text = "Открыть";
            this.открытьToolStripMenuItem1.Click += new System.EventHandler(this.открытьToolStripMenuItem1_Click);
            // 
            // сохранитьToolStripMenuItem
            // 
            this.сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            this.сохранитьToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.сохранитьToolStripMenuItem.Text = "Сохранить";
            this.сохранитьToolStripMenuItem.Click += new System.EventHandler(this.сохранитьToolStripMenuItem_Click);
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.справкаToolStripMenuItem.Text = "Справка";
            // 
            // button_choose_line_color
            // 
            this.button_choose_line_color.Location = new System.Drawing.Point(6, 19);
            this.button_choose_line_color.Name = "button_choose_line_color";
            this.button_choose_line_color.Size = new System.Drawing.Size(74, 25);
            this.button_choose_line_color.TabIndex = 10;
            this.button_choose_line_color.UseVisualStyleBackColor = true;
            this.button_choose_line_color.Click += new System.EventHandler(this.button_choose_line_color_Click);
            // 
            // button_choose_fill_color
            // 
            this.button_choose_fill_color.Location = new System.Drawing.Point(88, 19);
            this.button_choose_fill_color.Name = "button_choose_fill_color";
            this.button_choose_fill_color.Size = new System.Drawing.Size(74, 25);
            this.button_choose_fill_color.TabIndex = 11;
            this.button_choose_fill_color.UseVisualStyleBackColor = true;
            this.button_choose_fill_color.Click += new System.EventHandler(this.button_choose_fill_color_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button_choose_line_color);
            this.groupBox1.Controls.Add(this.button_choose_fill_color);
            this.groupBox1.Location = new System.Drawing.Point(348, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(168, 50);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Цвет контура";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(85, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Цвет заливки";
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(1487, 30);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(71, 24);
            this.buttonDelete.TabIndex = 20;
            this.buttonDelete.Text = "Удалить";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonSaveMutant
            // 
            this.buttonSaveMutant.Image = global::VectorGraphicsEditor.Properties.Resources.Save;
            this.buttonSaveMutant.Location = new System.Drawing.Point(858, 53);
            this.buttonSaveMutant.Name = "buttonSaveMutant";
            this.buttonSaveMutant.Size = new System.Drawing.Size(60, 24);
            this.buttonSaveMutant.TabIndex = 18;
            this.buttonSaveMutant.UseVisualStyleBackColor = true;
            this.buttonSaveMutant.Click += new System.EventHandler(this.buttonSaveMutant_Click);
            // 
            // buttonMinus
            // 
            this.buttonMinus.Image = global::VectorGraphicsEditor.Properties.Resources.Разность;
            this.buttonMinus.Location = new System.Drawing.Point(746, 27);
            this.buttonMinus.Name = "buttonMinus";
            this.buttonMinus.Size = new System.Drawing.Size(50, 50);
            this.buttonMinus.TabIndex = 21;
            this.buttonMinus.UseVisualStyleBackColor = true;
            // 
            // buttonMutantNext
            // 
            this.buttonMutantNext.Image = ((System.Drawing.Image)(resources.GetObject("buttonMutantNext.Image")));
            this.buttonMutantNext.Location = new System.Drawing.Point(858, 27);
            this.buttonMutantNext.Name = "buttonMutantNext";
            this.buttonMutantNext.Size = new System.Drawing.Size(60, 24);
            this.buttonMutantNext.TabIndex = 19;
            this.buttonMutantNext.UseVisualStyleBackColor = true;
            this.buttonMutantNext.Click += new System.EventHandler(this.buttonMutantNext_Click);
            // 
            // buttonStartMutant
            // 
            this.buttonStartMutant.Image = global::VectorGraphicsEditor.Properties.Resources.Мутант;
            this.buttonStartMutant.Location = new System.Drawing.Point(802, 27);
            this.buttonStartMutant.Name = "buttonStartMutant";
            this.buttonStartMutant.Size = new System.Drawing.Size(50, 50);
            this.buttonStartMutant.TabIndex = 17;
            this.buttonStartMutant.UseVisualStyleBackColor = true;
            this.buttonStartMutant.Click += new System.EventHandler(this.buttonStartMutant_Click);
            // 
            // buttonUnion
            // 
            this.buttonUnion.Image = global::VectorGraphicsEditor.Properties.Resources.Объединение;
            this.buttonUnion.Location = new System.Drawing.Point(690, 27);
            this.buttonUnion.Name = "buttonUnion";
            this.buttonUnion.Size = new System.Drawing.Size(50, 50);
            this.buttonUnion.TabIndex = 16;
            this.buttonUnion.UseVisualStyleBackColor = true;
            this.buttonUnion.Click += new System.EventHandler(this.buttonUnion_Click);
            // 
            // buttonIntersection
            // 
            this.buttonIntersection.Image = global::VectorGraphicsEditor.Properties.Resources.Пересечение;
            this.buttonIntersection.Location = new System.Drawing.Point(634, 27);
            this.buttonIntersection.Name = "buttonIntersection";
            this.buttonIntersection.Size = new System.Drawing.Size(50, 50);
            this.buttonIntersection.TabIndex = 15;
            this.buttonIntersection.UseVisualStyleBackColor = true;
            this.buttonIntersection.Click += new System.EventHandler(this.buttonIntersection_Click);
            // 
            // buttonDivider
            // 
            this.buttonDivider.Image = global::VectorGraphicsEditor.Properties.Resources.Циркуль;
            this.buttonDivider.Location = new System.Drawing.Point(578, 27);
            this.buttonDivider.Name = "buttonDivider";
            this.buttonDivider.Size = new System.Drawing.Size(50, 50);
            this.buttonDivider.TabIndex = 14;
            this.buttonDivider.UseVisualStyleBackColor = true;
            this.buttonDivider.Click += new System.EventHandler(this.buttonDivider_Click);
            // 
            // buttonScaleLine
            // 
            this.buttonScaleLine.Image = global::VectorGraphicsEditor.Properties.Resources.Линейка;
            this.buttonScaleLine.Location = new System.Drawing.Point(522, 27);
            this.buttonScaleLine.Name = "buttonScaleLine";
            this.buttonScaleLine.Size = new System.Drawing.Size(50, 50);
            this.buttonScaleLine.TabIndex = 13;
            this.buttonScaleLine.UseVisualStyleBackColor = true;
            this.buttonScaleLine.Click += new System.EventHandler(this.buttonScaleLine_Click);
            // 
            // buttonCircle
            // 
            this.buttonCircle.Image = global::VectorGraphicsEditor.Properties.Resources.Круг;
            this.buttonCircle.Location = new System.Drawing.Point(180, 27);
            this.buttonCircle.Name = "buttonCircle";
            this.buttonCircle.Size = new System.Drawing.Size(50, 50);
            this.buttonCircle.TabIndex = 9;
            this.buttonCircle.UseVisualStyleBackColor = true;
            this.buttonCircle.Click += new System.EventHandler(this.buttonCircle_Click);
            // 
            // buttonEllipse
            // 
            this.buttonEllipse.Image = global::VectorGraphicsEditor.Properties.Resources.Элипс;
            this.buttonEllipse.Location = new System.Drawing.Point(236, 27);
            this.buttonEllipse.Name = "buttonEllipse";
            this.buttonEllipse.Size = new System.Drawing.Size(50, 50);
            this.buttonEllipse.TabIndex = 8;
            this.buttonEllipse.UseVisualStyleBackColor = true;
            this.buttonEllipse.Click += new System.EventHandler(this.buttonEllipse_Click);
            // 
            // buttonSelectFigure
            // 
            this.buttonSelectFigure.Image = global::VectorGraphicsEditor.Properties.Resources.Выбор;
            this.buttonSelectFigure.Location = new System.Drawing.Point(292, 27);
            this.buttonSelectFigure.Name = "buttonSelectFigure";
            this.buttonSelectFigure.Size = new System.Drawing.Size(50, 50);
            this.buttonSelectFigure.TabIndex = 7;
            this.buttonSelectFigure.UseVisualStyleBackColor = true;
            this.buttonSelectFigure.Click += new System.EventHandler(this.buttonSelectFigure_Click);
            // 
            // buttonTriangle
            // 
            this.buttonTriangle.Image = global::VectorGraphicsEditor.Properties.Resources.Треугольник;
            this.buttonTriangle.Location = new System.Drawing.Point(124, 27);
            this.buttonTriangle.Name = "buttonTriangle";
            this.buttonTriangle.Size = new System.Drawing.Size(50, 50);
            this.buttonTriangle.TabIndex = 4;
            this.buttonTriangle.UseVisualStyleBackColor = true;
            this.buttonTriangle.Click += new System.EventHandler(this.buttonTriangle_Click);
            // 
            // buttonRect
            // 
            this.buttonRect.Image = global::VectorGraphicsEditor.Properties.Resources.квадрат_второй;
            this.buttonRect.Location = new System.Drawing.Point(68, 27);
            this.buttonRect.Name = "buttonRect";
            this.buttonRect.Size = new System.Drawing.Size(50, 50);
            this.buttonRect.TabIndex = 2;
            this.buttonRect.UseVisualStyleBackColor = true;
            this.buttonRect.Click += new System.EventHandler(this.buttonRect_Click);
            // 
            // buttonLine
            // 
            this.buttonLine.Image = global::VectorGraphicsEditor.Properties.Resources.Линия;
            this.buttonLine.Location = new System.Drawing.Point(12, 27);
            this.buttonLine.Name = "buttonLine";
            this.buttonLine.Size = new System.Drawing.Size(50, 50);
            this.buttonLine.TabIndex = 1;
            this.buttonLine.UseVisualStyleBackColor = true;
            this.buttonLine.Click += new System.EventHandler(this.buttonLine_Click);
            // 
            // правкаToolStripMenuItem
            // 
            this.правкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.копироватьToolStripMenuItem,
            this.впередToolStripMenuItem,
            this.назадToolStripMenuItem});
            this.правкаToolStripMenuItem.Name = "правкаToolStripMenuItem";
            this.правкаToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.правкаToolStripMenuItem.Text = "Правка";
            // 
            // копироватьToolStripMenuItem
            // 
            this.копироватьToolStripMenuItem.Name = "копироватьToolStripMenuItem";
            this.копироватьToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.копироватьToolStripMenuItem.Text = "Копировать";
            this.копироватьToolStripMenuItem.Click += new System.EventHandler(this.копироватьToolStripMenuItem_Click);
            // 
            // впередToolStripMenuItem
            // 
            this.впередToolStripMenuItem.Name = "впередToolStripMenuItem";
            this.впередToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.впередToolStripMenuItem.Text = "Вперед";
            this.впередToolStripMenuItem.Click += new System.EventHandler(this.впередToolStripMenuItem_Click);
            // 
            // назадToolStripMenuItem
            // 
            this.назадToolStripMenuItem.Name = "назадToolStripMenuItem";
            this.назадToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.назадToolStripMenuItem.Text = "Назад";
            this.назадToolStripMenuItem.Click += new System.EventHandler(this.назадToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1588, 822);
            this.Controls.Add(this.buttonMinus);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonMutantNext);
            this.Controls.Add(this.buttonSaveMutant);
            this.Controls.Add(this.buttonStartMutant);
            this.Controls.Add(this.buttonUnion);
            this.Controls.Add(this.buttonIntersection);
            this.Controls.Add(this.buttonDivider);
            this.Controls.Add(this.buttonScaleLine);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCircle);
            this.Controls.Add(this.buttonEllipse);
            this.Controls.Add(this.buttonSelectFigure);
            this.Controls.Add(this.buttonTriangle);
            this.Controls.Add(this.buttonRect);
            this.Controls.Add(this.buttonLine);
            this.Controls.Add(this.openGLControlView);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Графический редактор";
            ((System.ComponentModel.ISupportInitialize)(this.openGLControlView)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SharpGL.OpenGLControl openGLControlView;
        private System.Windows.Forms.Button buttonLine;
        private System.Windows.Forms.Button buttonRect;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem открытьToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.Button buttonTriangle;
        private System.Windows.Forms.Button buttonSelectFigure;
        private System.Windows.Forms.Button buttonEllipse;
        private System.Windows.Forms.Button buttonCircle;
        private System.Windows.Forms.Button button_choose_line_color;
        private System.Windows.Forms.Button button_choose_fill_color;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonScaleLine;
        private System.Windows.Forms.Button buttonDivider;
        private System.Windows.Forms.Button buttonIntersection;
        private System.Windows.Forms.Button buttonUnion;
        private System.Windows.Forms.Button buttonStartMutant;
        private System.Windows.Forms.Button buttonSaveMutant;
        private System.Windows.Forms.Button buttonMutantNext;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonMinus;
        private System.Windows.Forms.ToolStripMenuItem правкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem копироватьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem впередToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem назадToolStripMenuItem;
    }
}

