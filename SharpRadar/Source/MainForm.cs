using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpRadar
{
    public partial class MainForm : Form
    {
        private readonly Memory _memory;
        private Bitmap _currentMap;
        private Bitmap _currentRender;
        private volatile int _mapWidth; // Only used for testing
        private volatile int _mapHeight; // Only used for testing
        private System.Timers.Timer _timer;
        private static Random _rng = new Random();
        private readonly Game _game; // Testing class to store unit location data
        private float _zoom = 1.0f;
        private int _lastZoom = 0;
        private const int _maxZoom = 3500;
        public MainForm(Memory memory)
        {
            InitializeComponent();
            _memory = memory;
            this.DoubleBuffered = true; // Prevent flickering
            this.Resize += this.MainForm_Resize;
            _currentMap = new Bitmap(Image.FromFile("Resources\\lighthouse.png")); // Load Map File
            _mapWidth = _currentMap.Width;
            _mapHeight = _currentMap.Height;
            _currentRender = (Bitmap)_currentMap.Clone();
            _game = new Game();
            _game.CurrentPlayer = new Player()
            {
                X = 150,
                Y = 1000
            };
            mapCanvas.Paint += this.mapCanvas_OnPaint;
            _timer = new System.Timers.Timer(33);
            _timer.Elapsed += this.tick;
            _timer.Start();
            this.Load += this.MainForm_Load;
        }

        /// <summary>
        /// Testing Only, move units around the map
        /// </summary>
        private async void MainForm_Load(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            await Task.Run(() =>
            {
                /* NOTE: Was receiving exceptions after ~30-45 sec when not using Volatile Reads
                 Since the rendering is occuring on a timer callback, may be race conditions on several variables
                 Should work fine with this testing code though, when implementing location adjustments in Memory.cs, be sure
                to account for thread safety (use Interlocked?) */
                while (true)
                {
                    Thread.Sleep(33);
                    foreach (var scav in _game.Scavs)
                    {
                        int x = Volatile.Read(ref scav.X);
                        int y = Volatile.Read(ref scav.Y);
                        int xValue = _rng.Next(-2, 3);
                        int yValue = _rng.Next(-2, 3);
                        if (x + xValue >= 0 && x + xValue < _mapWidth) Interlocked.Add(ref scav.X, xValue);
                        if (y + yValue >= 0 && y + yValue < _mapHeight) Interlocked.Add(ref scav.Y, yValue);
                    }
                    foreach (var pmc in _game.PMCs)
                    {
                        int x = Volatile.Read(ref pmc.X);
                        int y = Volatile.Read(ref pmc.Y);
                        int xValue = _rng.Next(-2, 3);
                        int yValue = _rng.Next(-2, 3);
                        if (x + xValue >= 0 && x + xValue < _mapWidth) Interlocked.Add(ref pmc.X, xValue);
                        if (x + yValue >= 0 && y + yValue < _mapHeight) Interlocked.Add(ref pmc.Y, yValue);
                    }
                }
            });
        }

        /// <summary>
        /// Handle window resizing
        /// </summary>
        private void MainForm_Resize(object sender, System.EventArgs e)
        {
            _timer.Reset();
            mapCanvas.Size = new Size(this.Height, this.Height); // Keep square aspect ratio
            mapCanvas.Location = new Point(0, 0); // Keep in top left corner
            refresh();
        }

        /// <summary>
        /// Render timer (~30 fps?)
        /// </summary>
        private void tick(Object source, System.Timers.ElapsedEventArgs e)
        {
            refresh();
        }

        /// <summary>
        /// GUI Refresh method
        /// </summary>
        private void refresh() // Request GUI to render next frame
        {
            mapCanvas.Invalidate(); // Clears canvas, causing it to be re-drawn
        }

        /// <summary>
        /// Control handles map zoom
        /// </summary>
        private void trackBar_Zoom_Scroll(object sender, EventArgs e)
        {
            int amtChanged = trackBar_Zoom.Value - _lastZoom;
            _lastZoom = trackBar_Zoom.Value;
            _zoom -= (.01f) * (amtChanged);
        }

        /// <summary>
        /// Draw/Render on Map Canvas
        /// </summary>
        private void mapCanvas_OnPaint(object sender, PaintEventArgs e)
        {
            var render = GetRender(); // Construct next frame
            mapCanvas.Image = render; // Render next frame

            // Cleanup Resources
            _currentRender.Dispose(); // Dispose previous frame
            _currentRender = render; // Store reference of current frame
        }

        /// <summary>
        /// Draws next render frame and returns a completed Bitmap
        /// </summary>
        private Bitmap GetRender()
        {
            int zoom = (int)(_maxZoom * _zoom); // Get zoom level
            int strokeLength = zoom / 125; // Lower constant = longer stroke
            if (strokeLength < 5) strokeLength = 5; // Min value
            int strokeWidth = zoom / 300; // Lower constant = wider stroke
            if (strokeWidth < 4) strokeWidth = 4; // Min value
            label_debug.Text = $"{zoom}, {strokeLength}, {strokeWidth}";
            using (var render = (Bitmap)_currentMap.Clone()) // Get a fresh map to draw on
            using (var grn = new Pen(Color.LimeGreen)
            {
                Width = strokeWidth
            })
            using (var red = new Pen(Color.Red)
            {
                Width = strokeWidth
            })
            using (var ylw = new Pen(Color.Yellow)
            {
                Width = strokeWidth
            })
            {
                // Get map frame bounds (Based on Zoom Level)
                var bounds = new Rectangle(_game.CurrentPlayer.X - zoom / 2, _game.CurrentPlayer.Y - zoom / 2, zoom, zoom);
                using (var gr = Graphics.FromImage(render)) // Get fresh frame
                {
                    // Draw Player
                    gr.DrawLine(grn, new Point(_game.CurrentPlayer.X - strokeLength, _game.CurrentPlayer.Y), new Point(_game.CurrentPlayer.X + strokeLength, _game.CurrentPlayer.Y));
                    gr.DrawLine(grn, new Point(_game.CurrentPlayer.X, _game.CurrentPlayer.Y - strokeLength), new Point(_game.CurrentPlayer.X, _game.CurrentPlayer.Y + strokeLength));
                    // Draw Units
                    foreach (var pmc in _game.PMCs) // Draw PMCs
                    {
                        if (pmc.X >= bounds.Left // Only draw if in bounds
                            && pmc.Y >= bounds.Top
                            && pmc.X <= bounds.Right
                            && pmc.Y <= bounds.Bottom)
                        { // Draw Location Marker
                            gr.DrawLine(red, new Point(pmc.X - strokeLength, pmc.Y), new Point(pmc.X + strokeLength, pmc.Y));
                            gr.DrawLine(red, new Point(pmc.X, pmc.Y - strokeLength), new Point(pmc.X, pmc.Y + strokeLength));
                        }
                    }
                    foreach (var scav in _game.Scavs) // Draw Scavs
                    {
                        if (scav.X >= bounds.Left // Only draw if in bounds
                            && scav.Y >= bounds.Top
                            && scav.X <= bounds.Right
                            && scav.Y <= bounds.Bottom)
                        { // Draw Location Marker
                            gr.DrawLine(ylw, new Point(scav.X - strokeLength, scav.Y), new Point(scav.X + strokeLength, scav.Y));
                            gr.DrawLine(ylw, new Point(scav.X, scav.Y - strokeLength), new Point(scav.X, scav.Y + strokeLength));
                        }
                    }
                    /// ToDo Handle Units Dying, draw a marker on death location
                    /// Handle Loot/Items
                }
                return CropImage(render, bounds); // Return the portion of the map to be rendered based on Zoom Level
            }
        }

        /// <summary>
        /// Returns a rectangular section of a source Bitmap
        /// </summary>
        private Bitmap CropImage(Bitmap source, Rectangle section)
        {
            var bitmap = new Bitmap(section.Width, section.Height);
            using (var gr = Graphics.FromImage(bitmap))
            {
                gr.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);
                return bitmap;
            }
        }

        /// <summary>
        /// Testing Methods
        /// </summary>
        private void button_up_Click(object sender, EventArgs e)
        {
            _game.CurrentPlayer.Y-=10;
        }

        private void button_left_Click(object sender, EventArgs e)
        {
            _game.CurrentPlayer.X-=10;
        }

        private void button_down_Click(object sender, EventArgs e)
        {
            _game.CurrentPlayer.Y+=10;
        }

        private void button_right_Click(object sender, EventArgs e)
        {
            _game.CurrentPlayer.X+=10;
        }

        private void button_testAddUnits_Click(object sender, EventArgs e)
        {
            _game.PMCs.Add(new PMC() // Add one unit
            {
                X = _rng.Next(0, _mapWidth),
                Y = _rng.Next(0, _mapHeight)
            });
            _game.Scavs.Add(new Scav() // Add one unit
            {
                X = _rng.Next(0, _mapWidth),
                Y = _rng.Next(0, _mapHeight)
            });
        }
    }
}
