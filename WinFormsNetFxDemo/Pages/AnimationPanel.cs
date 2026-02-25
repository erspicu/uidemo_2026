using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsNetFxDemo.Pages
{
    // Double-buffered canvas to eliminate flicker during animation
    internal sealed class BufferedCanvas : Panel
    {
        public BufferedCanvas() { DoubleBuffered = true; }
    }

    public class AnimationPanel : Panel
    {
        private static readonly Color BgColor       = ColorTranslator.FromHtml("#0F172A");
        private static readonly Color CardColor      = ColorTranslator.FromHtml("#1E293B");
        private static readonly Color AccentColor    = ColorTranslator.FromHtml("#6366F1");
        private static readonly Color TextPrimary    = ColorTranslator.FromHtml("#F1F5F9");
        private static readonly Color TextSecondary  = ColorTranslator.FromHtml("#94A3B8");

        private readonly System.Windows.Forms.Timer _timer;
        private readonly Label _counterLabel;
        private readonly Button _toggleBtn;
        private readonly BufferedCanvas _canvas;

        private bool _running = false;
        private double _phase = 0.0;
        private int _counter = 0;
        private int _counterDir = 1;

        private double _orbitAngle = 0.0;

        public AnimationPanel()
        {
            DoubleBuffered = true;
            SuspendLayout();
            BackColor = BgColor;
            Padding = new Padding(24);

            var titleLabel = new Label
            {
                Text = "Animation",
                ForeColor = TextPrimary,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 20f, FontStyle.Bold),
                Location = new Point(24, 24),
                AutoSize = true
            };
            Controls.Add(titleLabel);

            var subLabel = new Label
            {
                Text = "Timer-driven animations using GDI+",
                ForeColor = TextSecondary,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 10f),
                Location = new Point(24, 58),
                AutoSize = true
            };
            Controls.Add(subLabel);

            _canvas = new BufferedCanvas
            {
                BackColor = CardColor,
                Location = new Point(24, 90),
                Size = new Size(700, 340),
                BorderStyle = BorderStyle.None
            };
            _canvas.Paint += OnCanvasPaint;
            Controls.Add(_canvas);

            _counterLabel = new Label
            {
                Text = "Counter: 0",
                ForeColor = AccentColor,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                Location = new Point(24, 448),
                AutoSize = true
            };
            Controls.Add(_counterLabel);

            _toggleBtn = new Button
            {
                Text = "▶ Start",
                BackColor = AccentColor,
                ForeColor = TextPrimary,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                Size = new Size(130, 38),
                Location = new Point(24, 480),
                Cursor = Cursors.Hand
            };
            _toggleBtn.FlatAppearance.BorderSize = 0;
            _toggleBtn.Click += ToggleAnimation;
            Controls.Add(_toggleBtn);

            _timer = new System.Windows.Forms.Timer { Interval = 50 };
            _timer.Tick += OnTick;

            ResumeLayout(false);
        }

        private void ToggleAnimation(object sender, EventArgs e)
        {
            _running = !_running;
            if (_running)
            {
                _timer.Start();
                _toggleBtn.Text = "⏸ Stop";
            }
            else
            {
                _timer.Stop();
                _toggleBtn.Text = "▶ Start";
            }
        }

        private void OnTick(object sender, EventArgs e)
        {
            _phase += 0.12;
            _orbitAngle += 0.06;

            _counter += _counterDir * 2;
            if (_counter >= 100) { _counter = 100; _counterDir = -1; }
            if (_counter <= 0)   { _counter = 0;   _counterDir = 1;  }

            _counterLabel.Text = string.Format("Counter: {0}", _counter);
            _canvas.Invalidate();
        }

        private void OnCanvasPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(CardColor);

            int cw = _canvas.Width;
            int ch = _canvas.Height;

            // --- Wave bars (left half) ---
            int barCount = 8;
            int barAreaWidth = cw / 2 - 20;
            int barSpacing = barAreaWidth / barCount;
            int barW = barSpacing - 8;
            int maxBarH = ch - 60;
            int barBaseY = ch - 20;

            var barColors = new Color[]
            {
                ColorTranslator.FromHtml("#6366F1"),
                ColorTranslator.FromHtml("#8B5CF6"),
                ColorTranslator.FromHtml("#EC4899"),
                ColorTranslator.FromHtml("#F59E0B"),
                ColorTranslator.FromHtml("#10B981"),
                ColorTranslator.FromHtml("#06B6D4"),
                ColorTranslator.FromHtml("#3B82F6"),
                ColorTranslator.FromHtml("#6366F1"),
            };

            for (int i = 0; i < barCount; i++)
            {
                double sineVal = Math.Sin(_phase + i * 0.7);
                int barH = (int)((sineVal + 1.0) / 2.0 * maxBarH * 0.7 + maxBarH * 0.15);
                int x = 20 + i * barSpacing;
                int y = barBaseY - barH;

                using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    new Point(x, y), new Point(x, barBaseY),
                    barColors[i], Color.FromArgb(60, barColors[i])))
                {
                    g.FillRectangle(brush, x, y, barW, barH);
                }
            }

            // "Wave Bars" label
            TextRenderer.DrawText(g, "Wave Bars", new Font("Segoe UI", 9f),
                new Point(20, 8), ColorTranslator.FromHtml("#94A3B8"));

            // --- Orbiting dot (right half) ---
            int orbitCx = cw * 3 / 4;
            int orbitCy = ch / 2;
            int orbitR  = 80;

            // Draw orbit ring
            using (var ringPen = new Pen(Color.FromArgb(60, AccentColor), 1.5f))
                g.DrawEllipse(ringPen, orbitCx - orbitR, orbitCy - orbitR, orbitR * 2, orbitR * 2);

            // Center dot
            int cDot = 10;
            using (var centerBrush = new SolidBrush(Color.FromArgb(120, AccentColor)))
                g.FillEllipse(centerBrush, orbitCx - cDot / 2, orbitCy - cDot / 2, cDot, cDot);

            // Orbiting dot
            int ox = (int)(orbitCx + Math.Cos(_orbitAngle) * orbitR);
            int oy = (int)(orbitCy + Math.Sin(_orbitAngle) * orbitR);
            int dotSize = 16;

            // Glow
            for (int g2 = 3; g2 >= 0; g2--)
            {
                int gs = dotSize + g2 * 5;
                using (var glowBrush = new SolidBrush(Color.FromArgb(30 - g2 * 6, AccentColor)))
                    g.FillEllipse(glowBrush, ox - gs / 2, oy - gs / 2, gs, gs);
            }
            using (var dotBrush = new SolidBrush(AccentColor))
                g.FillEllipse(dotBrush, ox - dotSize / 2, oy - dotSize / 2, dotSize, dotSize);

            // "Orbit" label
            TextRenderer.DrawText(g, "Orbiting Dot", new Font("Segoe UI", 9f),
                new Point(cw / 2 + 10, 8), ColorTranslator.FromHtml("#94A3B8"));

            // Divider
            using (var divPen = new Pen(Color.FromArgb(60, 255, 255, 255), 1))
                g.DrawLine(divPen, cw / 2, 10, cw / 2, ch - 10);
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);
            if (_canvas != null)
            {
                int availW = Width - 48;
                int availH = Height - 90 - 24;
                _canvas.Width  = Math.Max(200, Math.Min(availW, 800));
                _canvas.Height = Math.Max(150, Math.Min(availH - 100, 360));
            }
        }
    }
}
