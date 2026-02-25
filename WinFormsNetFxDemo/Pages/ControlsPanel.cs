using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsNetFxDemo.Pages
{
    public class ControlsPanel : Panel
    {
        private static readonly Color BgColor      = ColorTranslator.FromHtml("#0F172A");
        private static readonly Color CardColor     = ColorTranslator.FromHtml("#1E293B");
        private static readonly Color AccentColor   = ColorTranslator.FromHtml("#6366F1");
        private static readonly Color TextPrimary   = ColorTranslator.FromHtml("#F1F5F9");
        private static readonly Color TextSecondary = ColorTranslator.FromHtml("#94A3B8");
        private static readonly Color InputBg       = ColorTranslator.FromHtml("#0F172A");
        private static readonly Color BorderColor   = ColorTranslator.FromHtml("#1F2937");

        private readonly Label _trackLabel;

        public ControlsPanel()
        {
            SuspendLayout();
            BackColor = BgColor;
            AutoScroll = true;
            Padding = new Padding(24);

            int y = 24;
            int left = 24;

            // Title
            AddLabel("Controls", ref y, left, 20, FontStyle.Bold, TextPrimary);
            y += 8;

            // === Buttons Section ===
            AddSectionHeader("Buttons", ref y, left);

            var btnPrimary = new Button
            {
                Text = "Primary Button",
                BackColor = AccentColor,
                ForeColor = TextPrimary,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10f),
                Size = new Size(150, 36),
                Location = new Point(left, y),
                Cursor = Cursors.Hand
            };
            btnPrimary.FlatAppearance.BorderSize = 0;
            Controls.Add(btnPrimary);

            var btnSecondary = new Button
            {
                Text = "Secondary",
                BackColor = CardColor,
                ForeColor = TextPrimary,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10f),
                Size = new Size(130, 36),
                Location = new Point(left + 162, y),
                Cursor = Cursors.Hand
            };
            btnSecondary.FlatAppearance.BorderColor = BorderColor;
            btnSecondary.FlatAppearance.BorderSize = 1;
            Controls.Add(btnSecondary);

            var chk = new CheckBox
            {
                Text = "Enable notifications",
                BackColor = Color.Transparent,
                ForeColor = TextPrimary,
                Font = new Font("Segoe UI", 10f),
                Location = new Point(left + 305, y + 6),
                AutoSize = true,
                Checked = true
            };
            Controls.Add(chk);
            y += 52;

            var rb1 = new RadioButton
            {
                Text = "Option A",
                BackColor = Color.Transparent,
                ForeColor = TextPrimary,
                Font = new Font("Segoe UI", 10f),
                Location = new Point(left, y),
                AutoSize = true,
                Checked = true
            };
            var rb2 = new RadioButton
            {
                Text = "Option B",
                BackColor = Color.Transparent,
                ForeColor = TextPrimary,
                Font = new Font("Segoe UI", 10f),
                Location = new Point(left + 110, y),
                AutoSize = true
            };
            Controls.Add(rb1);
            Controls.Add(rb2);
            y += 40;

            // === Inputs Section ===
            AddSectionHeader("Inputs", ref y, left);

            AddLabel("Name", ref y, left, 10, FontStyle.Regular, TextSecondary, 0);
            var txtBox = new TextBox
            {
                BackColor = InputBg,
                ForeColor = TextPrimary,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10f),
                Size = new Size(280, 30),
                Location = new Point(left, y),
                Text = "John Doe"
            };
            Controls.Add(txtBox);
            y += 44;

            AddLabel("Quantity", ref y, left, 10, FontStyle.Regular, TextSecondary, 0);
            var nud = new NumericUpDown
            {
                BackColor = InputBg,
                ForeColor = TextPrimary,
                Font = new Font("Segoe UI", 10f),
                Size = new Size(120, 30),
                Location = new Point(left, y),
                Value = 5,
                Minimum = 0,
                Maximum = 100
            };
            Controls.Add(nud);
            y += 44;

            AddLabel("Category", ref y, left, 10, FontStyle.Regular, TextSecondary, 0);
            var combo = new ComboBox
            {
                BackColor = InputBg,
                ForeColor = TextPrimary,
                Font = new Font("Segoe UI", 10f),
                Size = new Size(200, 30),
                Location = new Point(left, y),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            combo.Items.AddRange(new object[] { "Technology", "Finance", "Marketing" });
            combo.SelectedIndex = 0;
            Controls.Add(combo);
            y += 44;

            // === Feedback Section ===
            AddSectionHeader("Feedback", ref y, left);

            AddLabel("Progress (65%)", ref y, left, 10, FontStyle.Regular, TextSecondary, 0);
            var progress = new ProgressBar
            {
                Size = new Size(320, 18),
                Location = new Point(left, y),
                Value = 65,
                Style = ProgressBarStyle.Continuous,
                BackColor = CardColor,
                ForeColor = AccentColor
            };
            Controls.Add(progress);
            y += 36;

            AddLabel("Volume", ref y, left, 10, FontStyle.Regular, TextSecondary, 0);
            var track = new TrackBar
            {
                Size = new Size(300, 40),
                Location = new Point(left, y),
                Minimum = 0,
                Maximum = 100,
                Value = 40,
                TickFrequency = 10,
                BackColor = BgColor
            };
            _trackLabel = new Label
            {
                Text = "40",
                ForeColor = AccentColor,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Location = new Point(left + 310, y + 10),
                AutoSize = true
            };
            track.ValueChanged += (s, e) => _trackLabel.Text = track.Value.ToString();
            Controls.Add(track);
            Controls.Add(_trackLabel);

            ResumeLayout(false);
        }

        private void AddLabel(string text, ref int y, int x, float size, FontStyle style, Color color, int extraBottom = 20)
        {
            var lbl = new Label
            {
                Text = text,
                ForeColor = color,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", size, style),
                Location = new Point(x, y),
                AutoSize = true
            };
            Controls.Add(lbl);
            y += (int)(size * 2) + extraBottom;
        }

        private void AddSectionHeader(string text, ref int y, int x)
        {
            var sep = new Panel
            {
                BackColor = ColorTranslator.FromHtml("#1F2937"),
                Size = new Size(500, 1),
                Location = new Point(x, y)
            };
            Controls.Add(sep);
            y += 10;

            var lbl = new Label
            {
                Text = text,
                ForeColor = ColorTranslator.FromHtml("#94A3B8"),
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                Location = new Point(x, y),
                AutoSize = true
            };
            Controls.Add(lbl);
            y += 32;
        }
    }
}
