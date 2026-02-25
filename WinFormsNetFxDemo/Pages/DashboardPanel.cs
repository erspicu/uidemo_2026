using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormsNetFxDemo.Pages
{
    public class DashboardPanel : Panel
    {
        private static readonly Color BgColor      = ColorTranslator.FromHtml("#0F172A");
        private static readonly Color CardColor     = ColorTranslator.FromHtml("#1E293B");
        private static readonly Color AccentColor   = ColorTranslator.FromHtml("#6366F1");
        private static readonly Color TextPrimary   = ColorTranslator.FromHtml("#F1F5F9");
        private static readonly Color TextSecondary = ColorTranslator.FromHtml("#94A3B8");
        private static readonly Color BorderColor   = ColorTranslator.FromHtml("#1F2937");

        private readonly (string Icon, string Label, string Value, Color Accent)[] _stats =
        {
            ("👥", "Users",   "12,453",  ColorTranslator.FromHtml("#6366F1")),
            ("📈", "Revenue", "$48,291", ColorTranslator.FromHtml("#10B981")),
            ("📦", "Orders",  "3,842",   ColorTranslator.FromHtml("#F59E0B")),
            ("⭐", "Rating",  "4.8",     ColorTranslator.FromHtml("#EC4899")),
        };

        private readonly ListView _listView;

        public DashboardPanel()
        {
            SuspendLayout();
            BackColor = BgColor;
            Padding = new Padding(24);

            // Recent Activity ListView
            _listView = new ListView
            {
                View = View.Details,
                FullRowSelect = true,
                GridLines = false,
                BackColor = CardColor,
                ForeColor = TextPrimary,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10f),
                OwnerDraw = true
            };
            _listView.Columns.Add("Time",   120);
            _listView.Columns.Add("User",   140);
            _listView.Columns.Add("Action", 220);
            _listView.Columns.Add("Status", 110);

            var rows = new[]
            {
                new[] { "09:41 AM", "Alice Kim",    "Logged in",       "Active"    },
                new[] { "10:05 AM", "Bob Chen",     "Made a purchase", "Completed" },
                new[] { "10:22 AM", "Carol Davis",  "Left a review",   "Completed" },
                new[] { "11:00 AM", "David Lee",    "Submitted order", "Pending"   },
                new[] { "11:35 AM", "Eva Martinez", "Updated profile", "Active"    },
            };
            foreach (var r in rows)
            {
                var item = new ListViewItem(r[0]) { ForeColor = TextPrimary, BackColor = CardColor };
                item.SubItems.Add(r[1]);
                item.SubItems.Add(r[2]);
                item.SubItems.Add(r[3]);
                _listView.Items.Add(item);
            }

            _listView.DrawColumnHeader += (s, e) =>
            {
                e.Graphics.FillRectangle(new SolidBrush(ColorTranslator.FromHtml("#0F172A")), e.Bounds);
                TextRenderer.DrawText(e.Graphics, e.Header.Text, new Font("Segoe UI", 9f, FontStyle.Bold),
                    new Point(e.Bounds.X + 6, e.Bounds.Y + 6), TextSecondary);
            };
            _listView.DrawItem += (s, e) => e.DrawDefault = true;
            _listView.DrawSubItem += (s, e) =>
            {
                var bg = e.ItemIndex % 2 == 0 ? CardColor : ColorTranslator.FromHtml("#243044");
                e.Graphics.FillRectangle(new SolidBrush(bg), e.Bounds);
                Color fg = TextPrimary;
                if (e.ColumnIndex == 3)
                {
                    switch (e.SubItem.Text)
                    {
                        case "Active":    fg = ColorTranslator.FromHtml("#34D399"); break;
                        case "Pending":   fg = ColorTranslator.FromHtml("#FBBF24"); break;
                        case "Completed": fg = ColorTranslator.FromHtml("#60A5FA"); break;
                    }
                }
                TextRenderer.DrawText(e.Graphics, e.SubItem.Text, new Font("Segoe UI", 10f),
                    new Point(e.Bounds.X + 6, e.Bounds.Y + 4), fg);
            };

            Controls.Add(_listView);
            ResumeLayout(false);
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);
            int pad = 24;
            int listTop = pad + 40 + 16 + 110 + 16 + 32;
            _listView.SetBounds(pad, listTop, Width - pad * 2, Height - listTop - pad);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int pad = 24;

            // Title
            TextRenderer.DrawText(g, "Dashboard", new Font("Segoe UI", 20f, FontStyle.Bold),
                new Point(pad, pad), TextPrimary);

            // Stat cards
            int cardY = pad + 40 + 16;
            int cardCount = _stats.Length;
            int totalGap = (cardCount - 1) * 16;
            int cardW = (Width - pad * 2 - totalGap) / cardCount;
            int cardH = 100;

            for (int i = 0; i < _stats.Length; i++)
            {
                int x = pad + i * (cardW + 16);
                var rect = new RectangleF(x, cardY, cardW, cardH);
                DrawRoundedRect(g, rect, 12, CardColor, _stats[i].Accent);

                // Icon + label
                TextRenderer.DrawText(g, _stats[i].Icon + "  " + _stats[i].Label,
                    new Font("Segoe UI", 10f), new Point(x + 16, cardY + 14), TextSecondary);
                // Value
                TextRenderer.DrawText(g, _stats[i].Value,
                    new Font("Segoe UI", 22f, FontStyle.Bold), new Point(x + 16, cardY + 42), TextPrimary);
                // Bottom accent line
                using (var accentBrush = new SolidBrush(_stats[i].Accent))
                    g.FillRectangle(accentBrush, x + 16, cardY + cardH - 8, 40, 3);
            }

            // Recent Activity header
            int actY = cardY + cardH + 16;
            TextRenderer.DrawText(g, "Recent Activity", new Font("Segoe UI", 13f, FontStyle.Bold),
                new Point(pad, actY), TextPrimary);
        }

        private static void DrawRoundedRect(Graphics g, RectangleF rect, float radius, Color fill, Color border)
        {
            using (var path = RoundedPath(rect, radius))
            using (var fillBrush = new SolidBrush(fill))
            {
                g.FillPath(fillBrush, path);
                using (var pen = new Pen(border, 1.5f))
                    g.DrawPath(pen, path);
            }
        }

        private static GraphicsPath RoundedPath(RectangleF r, float radius)
        {
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(r.Right - radius * 2, r.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(r.Right - radius * 2, r.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(r.X, r.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
