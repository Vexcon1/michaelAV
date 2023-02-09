using System.Drawing.Drawing2D;

namespace MichaelAV
{

    static class GraphicsExtension
    {
        private static GraphicsPath GenerateRoundedRectangle(
            this Graphics graphics,
            RectangleF rectangle,
            float radius)
        {
            float diameter;
            GraphicsPath path = new GraphicsPath();
            if (radius <= 0.0F)
            {
                path.AddRectangle(rectangle);
                path.CloseFigure();
                return path;
            }
            else
            {
                if (radius >= (Math.Min(rectangle.Width, rectangle.Height)) / 2.0)
                    return graphics.GenerateCapsule(rectangle);
                diameter = radius * 2.0F;
                SizeF sizeF = new SizeF(diameter, diameter);
                RectangleF arc = new RectangleF(rectangle.Location, sizeF);
                path.AddArc(arc, 180, 90);
                arc.X = rectangle.Right - diameter;
                path.AddArc(arc, 270, 90);
                arc.Y = rectangle.Bottom - diameter;
                path.AddArc(arc, 0, 90);
                arc.X = rectangle.Left;
                path.AddArc(arc, 90, 90);
                path.CloseFigure();
            }
            return path;
        }
        private static GraphicsPath GenerateCapsule(
            this Graphics graphics,
            RectangleF baseRect)
        {
            float diameter;
            RectangleF arc;
            GraphicsPath path = new GraphicsPath();
            try
            {
                if (baseRect.Width > baseRect.Height)
                {
                    diameter = baseRect.Height;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    path.AddArc(arc, 90, 180);
                    arc.X = baseRect.Right - diameter;
                    path.AddArc(arc, 270, 180);
                }
                else if (baseRect.Width < baseRect.Height)
                {
                    diameter = baseRect.Width;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    path.AddArc(arc, 180, 180);
                    arc.Y = baseRect.Bottom - diameter;
                    path.AddArc(arc, 0, 180);
                }
                else path.AddEllipse(baseRect);
            }
            catch { path.AddEllipse(baseRect); }
            finally { path.CloseFigure(); }
            return path;
        }

        /// <summary>
        /// Draws a rounded rectangle specified by a pair of coordinates, a width, a height and the radius 
        /// for the arcs that make the rounded edges.
        /// </summary>
        /// <param name="brush">System.Drawing.Pen that determines the color, width and style of the rectangle.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the rectangle to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the rectangle to draw.</param>
        /// <param name="width">Width of the rectangle to draw.</param>
        /// <param name="height">Height of the rectangle to draw.</param>
        /// <param name="radius">The radius of the arc used for the rounded edges.</param>

        public static void DrawRoundedRectangle(
            this Graphics graphics,
            Pen pen,
            float x,
            float y,
            float width,
            float height,
            float radius)
        {
            RectangleF rectangle = new RectangleF(x, y, width, height);
            GraphicsPath path = graphics.GenerateRoundedRectangle(rectangle, radius);
            SmoothingMode old = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.DrawPath(pen, path);
            graphics.SmoothingMode = old;
        }

        /// <summary>
        /// Draws a rounded rectangle specified by a pair of coordinates, a width, a height and the radius 
        /// for the arcs that make the rounded edges.
        /// </summary>
        /// <param name="brush">System.Drawing.Pen that determines the color, width and style of the rectangle.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the rectangle to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the rectangle to draw.</param>
        /// <param name="width">Width of the rectangle to draw.</param>
        /// <param name="height">Height of the rectangle to draw.</param>
        /// <param name="radius">The radius of the arc used for the rounded edges.</param>

        public static void DrawRoundedRectangle(
            this Graphics graphics,
            Pen pen,
            int x,
            int y,
            int width,
            int height,
            int radius)
        {
            graphics.DrawRoundedRectangle(
                pen,
                Convert.ToSingle(x),
                Convert.ToSingle(y),
                Convert.ToSingle(width),
                Convert.ToSingle(height),
                Convert.ToSingle(radius));
        }

        /// <summary>
        /// Fills the interior of a rounded rectangle specified by a pair of coordinates, a width, a height
        /// and the radius for the arcs that make the rounded edges.
        /// </summary>
        /// <param name="brush">System.Drawing.Brush that determines the characteristics of the fill.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the rectangle to fill.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the rectangle to fill.</param>
        /// <param name="width">Width of the rectangle to fill.</param>
        /// <param name="height">Height of the rectangle to fill.</param>
        /// <param name="radius">The radius of the arc used for the rounded edges.</param>

        public static void FillRoundedRectangle(
            this Graphics graphics,
            Brush brush,
            float x,
            float y,
            float width,
            float height,
            float radius)
        {
            RectangleF rectangle = new RectangleF(x, y, width, height);
            GraphicsPath path = graphics.GenerateRoundedRectangle(rectangle, radius);
            SmoothingMode old = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.FillPath(brush, path);
            graphics.SmoothingMode = old;
        }

        /// <summary>
        /// Fills the interior of a rounded rectangle specified by a pair of coordinates, a width, a height
        /// and the radius for the arcs that make the rounded edges.
        /// </summary>
        /// <param name="brush">System.Drawing.Brush that determines the characteristics of the fill.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the rectangle to fill.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the rectangle to fill.</param>
        /// <param name="width">Width of the rectangle to fill.</param>
        /// <param 
        /// <param name="height">Height of the rectangle to fill.</param>
        /// <param name="radius">The radius of the arc used for the rounded edges.</param>

        public static void FillRoundedRectangle(
            this Graphics graphics,
            Brush brush,
            int x,
            int y,
            int width,
            int height,
            int radius)
        {
            graphics.FillRoundedRectangle(
                brush,
                Convert.ToSingle(x),
                Convert.ToSingle(y),
                Convert.ToSingle(width),
                Convert.ToSingle(height),
                Convert.ToSingle(radius));
        }
    }
    partial class ClientAV
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            /*
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.FillRoundedRectangle(new SolidBrush(Color.White), 10, 10, this.Width - 40, this.Height - 60, 10);
            SolidBrush brush = new SolidBrush(
                Color.White
                );
            g.FillRoundedRectangle(brush, 12, 12, this.Width - 44, this.Height - 64, 10);
            g.DrawRoundedRectangle(new Pen(ControlPaint.Light(Color.White, 0.00f)), 12, 12, this.Width - 44, this.Height - 64, 10);
            g.FillRoundedRectangle(new SolidBrush(Color.White), 12, 12 + ((this.Height - 64) / 2), this.Width - 44, (this.Height - 64) / 2, 10);
            */
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientAV));
            this.logBox = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.logo = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.minusBTN = new System.Windows.Forms.Button();
            this.closeBTN = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ScanBTN = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.excludeBTN = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel9 = new System.Windows.Forms.Panel();
            this.scanFileBTN = new System.Windows.Forms.Button();
            this.pictureBox9 = new System.Windows.Forms.PictureBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.fullScanBTN = new System.Windows.Forms.Button();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.virusLabel = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.procLabel = new System.Windows.Forms.Label();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.scanLabel = new System.Windows.Forms.Label();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.panel8 = new System.Windows.Forms.Panel();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Panel = new System.Windows.Forms.ToolStripMenuItem();
            this.Results = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.FullScan = new System.Windows.Forms.ToolStripMenuItem();
            this.QuickScan = new System.Windows.Forms.ToolStripMenuItem();
            this.FileScan = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.Credits = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // logBox
            // 
            this.logBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(46)))), ((int)(((byte)(60)))));
            this.logBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.logBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.logBox.ForeColor = System.Drawing.Color.White;
            this.logBox.HideSelection = false;
            this.logBox.Location = new System.Drawing.Point(364, 77);
            this.logBox.Margin = new System.Windows.Forms.Padding(2);
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.Size = new System.Drawing.Size(546, 371);
            this.logBox.TabIndex = 13;
            this.logBox.TabStop = false;
            this.logBox.Text = "Output Log";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(355, 10);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(159, 50);
            this.label2.TabIndex = 17;
            this.label2.Text = "Michael";
            this.label2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label2_MouseMove);
            // 
            // logo
            // 
            this.logo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.logo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(54)))), ((int)(((byte)(77)))));
            this.logo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.logo.Image = global::MichaelAV.Properties.Resources.MichaelAV;
            this.logo.InitialImage = global::MichaelAV.Properties.Resources.MichaelAV;
            this.logo.Location = new System.Drawing.Point(0, -9);
            this.logo.Margin = new System.Windows.Forms.Padding(2);
            this.logo.Name = "logo";
            this.logo.Size = new System.Drawing.Size(102, 85);
            this.logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.logo.TabIndex = 14;
            this.logo.TabStop = false;
            this.logo.WaitOnLoad = true;
            this.logo.MouseMove += new System.Windows.Forms.MouseEventHandler(this.logo_MouseMove_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label1.Location = new System.Drawing.Point(501, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 47);
            this.label1.TabIndex = 19;
            this.label1.Text = "AV";
            this.label1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label1_MouseMove);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(46)))), ((int)(((byte)(60)))));
            this.panel1.Controls.Add(this.minusBTN);
            this.panel1.Controls.Add(this.closeBTN);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.logo);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(-1, -7);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(954, 65);
            this.panel1.TabIndex = 20;
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            // 
            // minusBTN
            // 
            this.minusBTN.BackColor = System.Drawing.Color.Transparent;
            this.minusBTN.BackgroundImage = global::MichaelAV.Properties.Resources.minus;
            this.minusBTN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.minusBTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.minusBTN.FlatAppearance.BorderSize = 0;
            this.minusBTN.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.minusBTN.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.minusBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.minusBTN.ForeColor = System.Drawing.Color.Transparent;
            this.minusBTN.Location = new System.Drawing.Point(824, 14);
            this.minusBTN.Name = "minusBTN";
            this.minusBTN.Size = new System.Drawing.Size(46, 40);
            this.minusBTN.TabIndex = 26;
            this.minusBTN.UseVisualStyleBackColor = false;
            this.minusBTN.Click += new System.EventHandler(this.minusBTN_Click);
            // 
            // closeBTN
            // 
            this.closeBTN.BackColor = System.Drawing.Color.Transparent;
            this.closeBTN.BackgroundImage = global::MichaelAV.Properties.Resources.close__1_;
            this.closeBTN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.closeBTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.closeBTN.FlatAppearance.BorderSize = 0;
            this.closeBTN.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.closeBTN.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.closeBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeBTN.ForeColor = System.Drawing.Color.Transparent;
            this.closeBTN.Location = new System.Drawing.Point(876, 19);
            this.closeBTN.Name = "closeBTN";
            this.closeBTN.Size = new System.Drawing.Size(46, 30);
            this.closeBTN.TabIndex = 25;
            this.closeBTN.UseVisualStyleBackColor = false;
            this.closeBTN.Click += new System.EventHandler(this.closeBTN_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(46)))), ((int)(((byte)(60)))));
            this.panel2.Controls.Add(this.ScanBTN);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Location = new System.Drawing.Point(24, 86);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(277, 87);
            this.panel2.TabIndex = 21;
            // 
            // ScanBTN
            // 
            this.ScanBTN.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ScanBTN.FlatAppearance.BorderSize = 0;
            this.ScanBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ScanBTN.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ScanBTN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(134)))), ((int)(((byte)(145)))));
            this.ScanBTN.Location = new System.Drawing.Point(99, 20);
            this.ScanBTN.Name = "ScanBTN";
            this.ScanBTN.Size = new System.Drawing.Size(145, 50);
            this.ScanBTN.TabIndex = 15;
            this.ScanBTN.Text = "Quick Scan";
            this.ScanBTN.UseVisualStyleBackColor = true;
            this.ScanBTN.Click += new System.EventHandler(this.ScanBTN_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Image = global::MichaelAV.Properties.Resources._1842869;
            this.pictureBox1.InitialImage = global::MichaelAV.Properties.Resources.MichaelAV;
            this.pictureBox1.Location = new System.Drawing.Point(21, 20);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(60, 51);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.WaitOnLoad = true;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(46)))), ((int)(((byte)(60)))));
            this.panel3.Controls.Add(this.excludeBTN);
            this.panel3.Controls.Add(this.pictureBox2);
            this.panel3.Location = new System.Drawing.Point(24, 441);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(277, 87);
            this.panel3.TabIndex = 22;
            // 
            // excludeBTN
            // 
            this.excludeBTN.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.excludeBTN.FlatAppearance.BorderSize = 0;
            this.excludeBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.excludeBTN.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.excludeBTN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(134)))), ((int)(((byte)(145)))));
            this.excludeBTN.Location = new System.Drawing.Point(99, 20);
            this.excludeBTN.Name = "excludeBTN";
            this.excludeBTN.Size = new System.Drawing.Size(145, 50);
            this.excludeBTN.TabIndex = 16;
            this.excludeBTN.Text = "Exclusion";
            this.excludeBTN.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox2.Image = global::MichaelAV.Properties.Resources._2258853;
            this.pictureBox2.InitialImage = global::MichaelAV.Properties.Resources.MichaelAV;
            this.pictureBox2.Location = new System.Drawing.Point(21, 20);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(60, 51);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 14;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.WaitOnLoad = true;
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(46)))), ((int)(((byte)(60)))));
            this.panel9.Controls.Add(this.scanFileBTN);
            this.panel9.Controls.Add(this.pictureBox9);
            this.panel9.Location = new System.Drawing.Point(24, 320);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(277, 87);
            this.panel9.TabIndex = 23;
            // 
            // scanFileBTN
            // 
            this.scanFileBTN.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.scanFileBTN.FlatAppearance.BorderSize = 0;
            this.scanFileBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.scanFileBTN.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.scanFileBTN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(134)))), ((int)(((byte)(145)))));
            this.scanFileBTN.Location = new System.Drawing.Point(99, 20);
            this.scanFileBTN.Name = "scanFileBTN";
            this.scanFileBTN.Size = new System.Drawing.Size(145, 50);
            this.scanFileBTN.TabIndex = 16;
            this.scanFileBTN.Text = "Scan File";
            this.scanFileBTN.UseVisualStyleBackColor = true;
            this.scanFileBTN.Click += new System.EventHandler(this.scanFileBTN_Click);
            // 
            // pictureBox9
            // 
            this.pictureBox9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox9.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox9.Image = global::MichaelAV.Properties.Resources._2258853;
            this.pictureBox9.InitialImage = global::MichaelAV.Properties.Resources.MichaelAV;
            this.pictureBox9.Location = new System.Drawing.Point(21, 20);
            this.pictureBox9.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox9.Name = "pictureBox9";
            this.pictureBox9.Size = new System.Drawing.Size(60, 51);
            this.pictureBox9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox9.TabIndex = 14;
            this.pictureBox9.TabStop = false;
            this.pictureBox9.WaitOnLoad = true;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(46)))), ((int)(((byte)(60)))));
            this.panel4.Controls.Add(this.fullScanBTN);
            this.panel4.Controls.Add(this.pictureBox3);
            this.panel4.Location = new System.Drawing.Point(24, 203);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(277, 87);
            this.panel4.TabIndex = 23;
            // 
            // fullScanBTN
            // 
            this.fullScanBTN.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.fullScanBTN.FlatAppearance.BorderSize = 0;
            this.fullScanBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fullScanBTN.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fullScanBTN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(134)))), ((int)(((byte)(145)))));
            this.fullScanBTN.Location = new System.Drawing.Point(99, 20);
            this.fullScanBTN.Name = "fullScanBTN";
            this.fullScanBTN.Size = new System.Drawing.Size(145, 50);
            this.fullScanBTN.TabIndex = 17;
            this.fullScanBTN.Text = "Full Scan";
            this.fullScanBTN.UseVisualStyleBackColor = true;
            this.fullScanBTN.Click += new System.EventHandler(this.button2_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox3.Image = global::MichaelAV.Properties.Resources._24380781;
            this.pictureBox3.InitialImage = global::MichaelAV.Properties.Resources.MichaelAV;
            this.pictureBox3.Location = new System.Drawing.Point(21, 20);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(60, 51);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 14;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.WaitOnLoad = true;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(46)))), ((int)(((byte)(60)))));
            this.panel5.Controls.Add(this.virusLabel);
            this.panel5.Controls.Add(this.pictureBox4);
            this.panel5.Location = new System.Drawing.Point(364, 476);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(149, 52);
            this.panel5.TabIndex = 22;
            // 
            // virusLabel
            // 
            this.virusLabel.AutoSize = true;
            this.virusLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.virusLabel.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.virusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(134)))), ((int)(((byte)(145)))));
            this.virusLabel.Location = new System.Drawing.Point(57, 15);
            this.virusLabel.Name = "virusLabel";
            this.virusLabel.Size = new System.Drawing.Size(49, 21);
            this.virusLabel.TabIndex = 15;
            this.virusLabel.Text = "Virus:";
            // 
            // pictureBox4
            // 
            this.pictureBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox4.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox4.Image = global::MichaelAV.Properties.Resources._1761201;
            this.pictureBox4.InitialImage = global::MichaelAV.Properties.Resources.MichaelAV;
            this.pictureBox4.Location = new System.Drawing.Point(2, 2);
            this.pictureBox4.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(50, 50);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 14;
            this.pictureBox4.TabStop = false;
            this.pictureBox4.WaitOnLoad = true;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(46)))), ((int)(((byte)(60)))));
            this.panel6.Controls.Add(this.procLabel);
            this.panel6.Controls.Add(this.pictureBox7);
            this.panel6.Controls.Add(this.pictureBox5);
            this.panel6.Location = new System.Drawing.Point(544, 476);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(163, 52);
            this.panel6.TabIndex = 23;
            // 
            // procLabel
            // 
            this.procLabel.AutoSize = true;
            this.procLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.procLabel.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.procLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(134)))), ((int)(((byte)(145)))));
            this.procLabel.Location = new System.Drawing.Point(57, 15);
            this.procLabel.Name = "procLabel";
            this.procLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.procLabel.Size = new System.Drawing.Size(74, 21);
            this.procLabel.TabIndex = 16;
            this.procLabel.Text = "Program:";
            this.procLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox7
            // 
            this.pictureBox7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox7.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox7.Image = global::MichaelAV.Properties.Resources.web_programming;
            this.pictureBox7.InitialImage = global::MichaelAV.Properties.Resources.MichaelAV;
            this.pictureBox7.Location = new System.Drawing.Point(2, 0);
            this.pictureBox7.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(50, 50);
            this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox7.TabIndex = 15;
            this.pictureBox7.TabStop = false;
            this.pictureBox7.WaitOnLoad = true;
            // 
            // pictureBox5
            // 
            this.pictureBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox5.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox5.Image = global::MichaelAV.Properties.Resources._2438078;
            this.pictureBox5.InitialImage = global::MichaelAV.Properties.Resources.MichaelAV;
            this.pictureBox5.Location = new System.Drawing.Point(2, 2);
            this.pictureBox5.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(83, 0);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox5.TabIndex = 14;
            this.pictureBox5.TabStop = false;
            this.pictureBox5.WaitOnLoad = true;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(46)))), ((int)(((byte)(60)))));
            this.panel7.Controls.Add(this.scanLabel);
            this.panel7.Controls.Add(this.pictureBox8);
            this.panel7.Controls.Add(this.pictureBox6);
            this.panel7.Location = new System.Drawing.Point(736, 476);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(174, 52);
            this.panel7.TabIndex = 24;
            // 
            // scanLabel
            // 
            this.scanLabel.AutoSize = true;
            this.scanLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.scanLabel.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.scanLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(134)))), ((int)(((byte)(145)))));
            this.scanLabel.Location = new System.Drawing.Point(55, 15);
            this.scanLabel.Name = "scanLabel";
            this.scanLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.scanLabel.Size = new System.Drawing.Size(72, 21);
            this.scanLabel.TabIndex = 17;
            this.scanLabel.Text = "Scanned:";
            this.scanLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox8
            // 
            this.pictureBox8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox8.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox8.Image = global::MichaelAV.Properties.Resources._2415363;
            this.pictureBox8.InitialImage = global::MichaelAV.Properties.Resources.MichaelAV;
            this.pictureBox8.Location = new System.Drawing.Point(0, 0);
            this.pictureBox8.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(50, 50);
            this.pictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox8.TabIndex = 16;
            this.pictureBox8.TabStop = false;
            this.pictureBox8.WaitOnLoad = true;
            // 
            // pictureBox6
            // 
            this.pictureBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox6.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox6.Image = global::MichaelAV.Properties.Resources._2438078;
            this.pictureBox6.InitialImage = global::MichaelAV.Properties.Resources.MichaelAV;
            this.pictureBox6.Location = new System.Drawing.Point(2, 2);
            this.pictureBox6.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(83, 0);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox6.TabIndex = 14;
            this.pictureBox6.TabStop = false;
            this.pictureBox6.WaitOnLoad = true;
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(31)))), ((int)(((byte)(40)))));
            this.panel8.Location = new System.Drawing.Point(-14, 551);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(960, 48);
            this.panel8.TabIndex = 24;
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipText = "Started up.";
            this.notifyIcon.BalloonTipTitle = "MichaelAV";
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "MichaelAV";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Panel,
            this.Results,
            this.toolStripSeparator1,
            this.FullScan,
            this.QuickScan,
            this.FileScan,
            this.toolStripSeparator2,
            this.Credits});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(134, 148);
            this.contextMenuStrip1.Text = "MichaelAV";
            // 
            // Panel
            // 
            this.Panel.Name = "Panel";
            this.Panel.Size = new System.Drawing.Size(133, 22);
            this.Panel.Text = "Panel";
            this.Panel.Click += new System.EventHandler(this.Panel_Click);
            // 
            // Results
            // 
            this.Results.Name = "Results";
            this.Results.Size = new System.Drawing.Size(133, 22);
            this.Results.Text = "Results";
            this.Results.Click += new System.EventHandler(this.Results_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(130, 6);
            // 
            // FullScan
            // 
            this.FullScan.Name = "FullScan";
            this.FullScan.Size = new System.Drawing.Size(133, 22);
            this.FullScan.Text = "Full Scan";
            this.FullScan.Click += new System.EventHandler(this.button2_Click);
            // 
            // QuickScan
            // 
            this.QuickScan.Name = "QuickScan";
            this.QuickScan.Size = new System.Drawing.Size(133, 22);
            this.QuickScan.Text = "Quick Scan";
            this.QuickScan.Click += new System.EventHandler(this.ScanBTN_Click);
            // 
            // FileScan
            // 
            this.FileScan.Name = "FileScan";
            this.FileScan.Size = new System.Drawing.Size(133, 22);
            this.FileScan.Text = "File Scan";
            this.FileScan.Click += new System.EventHandler(this.scanFileBTN_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(130, 6);
            // 
            // Credits
            // 
            this.Credits.BackColor = System.Drawing.Color.Transparent;
            this.Credits.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Credits.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.Credits.Name = "Credits";
            this.Credits.Size = new System.Drawing.Size(133, 22);
            this.Credits.Text = "MichaelAV";
            this.Credits.ToolTipText = "Thanks for using MichaelAV";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Title = "MichaelAV Scanner";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // ClientAV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(36)))), ((int)(((byte)(48)))));
            this.ClientSize = new System.Drawing.Size(933, 587);
            this.Controls.Add(this.panel9);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.logBox);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "ClientAV";
            this.Text = "MichaelAV";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(9)))), ((int)(((byte)(0)))));
            this.Load += new System.EventHandler(this.ClientAV_Load);
            ((System.ComponentModel.ISupportInitialize)(this.logo)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).EndInit();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox logBox;
        private System.Windows.Forms.PictureBox logo;
        private System.Windows.Forms.Label label2;
        private Label label1;
        private Panel panel1;
        private Panel panel2;
        private PictureBox pictureBox1;
        private Panel panel3;
        private PictureBox pictureBox2;
        private Panel panel4;
        private PictureBox pictureBox3;
        private Panel panel5;
        private PictureBox pictureBox4;
        private Panel panel6;
        private PictureBox pictureBox7;
        private PictureBox pictureBox5;
        private Panel panel7;
        private PictureBox pictureBox8;
        private PictureBox pictureBox6;
        private Button ScanBTN;
        private Button excludeBTN;
        private Button fullScanBTN;
        private Label virusLabel;
        private Panel panel8;
        private Button minusBTN;
        private Button closeBTN;
        private NotifyIcon notifyIcon;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Label procLabel;
        private Label scanLabel;
        private Panel panel9;
        private Button scanFileBTN;
        private PictureBox pictureBox9;
        private OpenFileDialog openFileDialog1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem Panel;
        private ToolStripMenuItem Results;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem FullScan;
        private ToolStripMenuItem QuickScan;
        private ToolStripMenuItem FileScan;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem Credits;
    }
}