using WinFormsDemo.Pages;

namespace WinFormsDemo;

public class MainForm : Form
{
    private static readonly Color BgColor      = ColorTranslator.FromHtml("#0F172A");
    private static readonly Color SidebarColor = ColorTranslator.FromHtml("#111827");
    private static readonly Color AccentColor  = ColorTranslator.FromHtml("#6366F1");
    private static readonly Color TextPrimary  = ColorTranslator.FromHtml("#F1F5F9");
    private static readonly Color TextSecondary = ColorTranslator.FromHtml("#94A3B8");

    private readonly Panel _sidebar;
    private readonly Panel _content;
    private readonly Button[] _navButtons;
    private readonly Panel[] _pages;
    private int _activePage = 0;

    private readonly string[] _navLabels = { "📊 Dashboard", "🎛 Controls", "🎬 Animation", "🗃 Data" };

    public MainForm()
    {
        DoubleBuffered = true;
        SuspendLayout();

        Text = "WinForms Demo";
        Size = new Size(1100, 700);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = BgColor;
        ForeColor = TextPrimary;
        MinimumSize = new Size(800, 500);

        // Sidebar
        _sidebar = new Panel
        {
            Dock = DockStyle.Left,
            Width = 220,
            BackColor = SidebarColor,
            Padding = new Padding(0, 16, 0, 0)
        };

        // App title in sidebar
        var titleLabel = new Label
        {
            Text = "✦ WinForms Demo",
            ForeColor = AccentColor,
            BackColor = Color.Transparent,
            Font = new Font("Segoe UI", 13f, FontStyle.Bold),
            Dock = DockStyle.Top,
            Height = 50,
            TextAlign = ContentAlignment.MiddleCenter
        };
        _sidebar.Controls.Add(titleLabel);

        // Nav buttons
        _navButtons = new Button[4];
        var pages = new Panel[]
        {
            new DashboardPanel(),
            new ControlsPanel(),
            new AnimationPanel(),
            new DataPanel()
        };
        _pages = pages;

        for (int i = 3; i >= 0; i--)
        {
            int idx = i;
            var btn = new Button
            {
                Text = _navLabels[i],
                Dock = DockStyle.Top,
                Height = 48,
                FlatStyle = FlatStyle.Flat,
                BackColor = SidebarColor,
                ForeColor = TextPrimary,
                Font = new Font("Segoe UI", 11f),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(16, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#1E293B");
            btn.Click += (_, __) => NavigateTo(idx);
            _navButtons[i] = btn;
            _sidebar.Controls.Add(btn);
        }

        // Content area
        _content = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = BgColor,
            Padding = new Padding(0)
        };

        foreach (var page in _pages)
        {
            page.Dock = DockStyle.Fill;
            page.Visible = false;
            _content.Controls.Add(page);
        }

        Controls.Add(_content);
        Controls.Add(_sidebar);

        ResumeLayout(false);
        PerformLayout();

        NavigateTo(0);

        Load += (_, __) => Program.EnableDarkTitleBar(Handle);
    }

    private void NavigateTo(int index)
    {
        _activePage = index;
        for (int i = 0; i < _navButtons.Length; i++)
        {
            _navButtons[i].BackColor = i == index ? AccentColor : SidebarColor;
            _navButtons[i].ForeColor = ColorTranslator.FromHtml("#F1F5F9");
        }
        for (int i = 0; i < _pages.Length; i++)
            _pages[i].Visible = i == index;
    }
}
