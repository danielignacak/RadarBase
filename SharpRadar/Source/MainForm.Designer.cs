namespace SharpRadar
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.mapCanvas = new System.Windows.Forms.PictureBox();
            this.button_up = new System.Windows.Forms.Button();
            this.button_left = new System.Windows.Forms.Button();
            this.button_right = new System.Windows.Forms.Button();
            this.button_down = new System.Windows.Forms.Button();
            this.trackBar_Zoom = new System.Windows.Forms.TrackBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label_debug = new System.Windows.Forms.Label();
            this.button_testAddUnits = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.mapCanvas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Zoom)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 659);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // mapCanvas
            // 
            this.mapCanvas.ImageLocation = "";
            this.mapCanvas.Location = new System.Drawing.Point(0, 0);
            this.mapCanvas.Name = "mapCanvas";
            this.mapCanvas.Size = new System.Drawing.Size(900, 900);
            this.mapCanvas.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.mapCanvas.TabIndex = 1;
            this.mapCanvas.TabStop = false;
            // 
            // button_up
            // 
            this.button_up.Location = new System.Drawing.Point(59, 30);
            this.button_up.Name = "button_up";
            this.button_up.Size = new System.Drawing.Size(73, 40);
            this.button_up.TabIndex = 2;
            this.button_up.Text = "Up";
            this.button_up.UseVisualStyleBackColor = true;
            this.button_up.Click += new System.EventHandler(this.button_up_Click);
            // 
            // button_left
            // 
            this.button_left.Location = new System.Drawing.Point(8, 76);
            this.button_left.Name = "button_left";
            this.button_left.Size = new System.Drawing.Size(73, 40);
            this.button_left.TabIndex = 3;
            this.button_left.Text = "Left";
            this.button_left.UseVisualStyleBackColor = true;
            this.button_left.Click += new System.EventHandler(this.button_left_Click);
            // 
            // button_right
            // 
            this.button_right.Location = new System.Drawing.Point(110, 76);
            this.button_right.Name = "button_right";
            this.button_right.Size = new System.Drawing.Size(73, 40);
            this.button_right.TabIndex = 4;
            this.button_right.Text = "Right";
            this.button_right.UseVisualStyleBackColor = true;
            this.button_right.Click += new System.EventHandler(this.button_right_Click);
            // 
            // button_down
            // 
            this.button_down.Location = new System.Drawing.Point(59, 122);
            this.button_down.Name = "button_down";
            this.button_down.Size = new System.Drawing.Size(73, 40);
            this.button_down.TabIndex = 5;
            this.button_down.Text = "Down";
            this.button_down.UseVisualStyleBackColor = true;
            this.button_down.Click += new System.EventHandler(this.button_down_Click);
            // 
            // trackBar_Zoom
            // 
            this.trackBar_Zoom.Location = new System.Drawing.Point(36, 201);
            this.trackBar_Zoom.Maximum = 99;
            this.trackBar_Zoom.Name = "trackBar_Zoom";
            this.trackBar_Zoom.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar_Zoom.Size = new System.Drawing.Size(45, 349);
            this.trackBar_Zoom.TabIndex = 6;
            this.trackBar_Zoom.Scroll += new System.EventHandler(this.trackBar_Zoom_Scroll);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label_debug);
            this.groupBox1.Controls.Add(this.button_testAddUnits);
            this.groupBox1.Controls.Add(this.button_up);
            this.groupBox1.Controls.Add(this.trackBar_Zoom);
            this.groupBox1.Controls.Add(this.button_left);
            this.groupBox1.Controls.Add(this.button_down);
            this.groupBox1.Controls.Add(this.button_right);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.Location = new System.Drawing.Point(919, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(226, 925);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Testing Controls";
            // 
            // label_debug
            // 
            this.label_debug.AutoSize = true;
            this.label_debug.Location = new System.Drawing.Point(46, 628);
            this.label_debug.Name = "label_debug";
            this.label_debug.Size = new System.Drawing.Size(37, 13);
            this.label_debug.TabIndex = 8;
            this.label_debug.Text = "debug";
            // 
            // button_testAddUnits
            // 
            this.button_testAddUnits.Location = new System.Drawing.Point(121, 227);
            this.button_testAddUnits.Name = "button_testAddUnits";
            this.button_testAddUnits.Size = new System.Drawing.Size(75, 23);
            this.button_testAddUnits.TabIndex = 7;
            this.button_testAddUnits.Text = "Add units";
            this.button_testAddUnits.UseVisualStyleBackColor = true;
            this.button_testAddUnits.Click += new System.EventHandler(this.button_testAddUnits_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1145, 925);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.mapCanvas);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "SharpRadar";
            ((System.ComponentModel.ISupportInitialize)(this.mapCanvas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Zoom)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox mapCanvas;
        private System.Windows.Forms.Button button_up;
        private System.Windows.Forms.Button button_left;
        private System.Windows.Forms.Button button_right;
        private System.Windows.Forms.Button button_down;
        private System.Windows.Forms.TrackBar trackBar_Zoom;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_testAddUnits;
        private System.Windows.Forms.Label label_debug;
    }
}

