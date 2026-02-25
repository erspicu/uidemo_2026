namespace WinFormsDemo.Pages;

public class DataPanel : Panel
{
    private static readonly Color BgColor      = ColorTranslator.FromHtml("#0F172A");
    private static readonly Color CardColor     = ColorTranslator.FromHtml("#1E293B");
    private static readonly Color AccentColor   = ColorTranslator.FromHtml("#6366F1");
    private static readonly Color TextPrimary   = ColorTranslator.FromHtml("#F1F5F9");
    private static readonly Color TextSecondary = ColorTranslator.FromHtml("#94A3B8");
    private static readonly Color BorderColor   = ColorTranslator.FromHtml("#1F2937");

    private readonly DataGridView _grid;
    private readonly TextBox _search;

    private readonly object[][] _allData = new[]
    {
        new object[] { "Alice Kim",     "Engineering",  92, "Active"   },
        new object[] { "Bob Chen",      "Marketing",    78, "Inactive" },
        new object[] { "Carol Davis",   "Design",       85, "Active"   },
        new object[] { "David Lee",     "Engineering",  91, "Pending"  },
        new object[] { "Eva Martinez",  "HR",           74, "Active"   },
        new object[] { "Frank Wilson",  "Finance",      88, "Inactive" },
        new object[] { "Grace Patel",   "Engineering",  95, "Active"   },
        new object[] { "Henry Brown",   "Marketing",    69, "Pending"  },
        new object[] { "Isla Johnson",  "Design",       82, "Active"   },
        new object[] { "Jake Thompson", "Finance",      77, "Inactive" },
    };

    public DataPanel()
    {
        SuspendLayout();
        BackColor = BgColor;
        Padding = new Padding(24);

        var titleLabel = new Label
        {
            Text = "Data",
            ForeColor = TextPrimary,
            BackColor = Color.Transparent,
            Font = new Font("Segoe UI", 20f, FontStyle.Bold),
            Location = new Point(24, 24),
            AutoSize = true
        };
        Controls.Add(titleLabel);

        var searchLabel = new Label
        {
            Text = "🔍  Search employees",
            ForeColor = TextSecondary,
            BackColor = Color.Transparent,
            Font = new Font("Segoe UI", 10f),
            Location = new Point(24, 62),
            AutoSize = true
        };
        Controls.Add(searchLabel);

        _search = new TextBox
        {
            BackColor = CardColor,
            ForeColor = TextPrimary,
            BorderStyle = BorderStyle.FixedSingle,
            Font = new Font("Segoe UI", 11f),
            Size = new Size(320, 32),
            Location = new Point(24, 86)
        };
        _search.TextChanged += OnSearchChanged;
        Controls.Add(_search);

        _grid = new DataGridView
        {
            Location = new Point(24, 132),
            BackgroundColor = CardColor,
            GridColor = BorderColor,
            BorderStyle = BorderStyle.None,
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
            EnableHeadersVisualStyles = false,
            RowHeadersVisible = false,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            Font = new Font("Segoe UI", 10f),
            RowTemplate = { Height = 38 }
        };

        // Column header style
        _grid.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#0F172A");
        _grid.ColumnHeadersDefaultCellStyle.ForeColor = TextSecondary;
        _grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
        _grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
        _grid.ColumnHeadersHeight = 40;

        // Default cell style
        _grid.DefaultCellStyle.BackColor = CardColor;
        _grid.DefaultCellStyle.ForeColor = TextPrimary;
        _grid.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#334155");
        _grid.DefaultCellStyle.SelectionForeColor = TextPrimary;
        _grid.DefaultCellStyle.Padding = new Padding(8, 0, 0, 0);

        // Alternating row style
        _grid.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#243044");
        _grid.AlternatingRowsDefaultCellStyle.ForeColor = TextPrimary;
        _grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#334155");
        _grid.AlternatingRowsDefaultCellStyle.SelectionForeColor = TextPrimary;

        _grid.Columns.Add("Name",       "Name");
        _grid.Columns.Add("Department", "Department");
        _grid.Columns.Add("Score",      "Score");
        _grid.Columns.Add("Status",     "Status");

        _grid.CellFormatting += OnCellFormatting;

        PopulateGrid(_allData);
        Controls.Add(_grid);

        ResumeLayout(false);
    }

    protected override void OnLayout(LayoutEventArgs levent)
    {
        base.OnLayout(levent);
        if (_grid != null)
        {
            _grid.Size = new Size(Width - 48, Height - 132 - 24);
        }
    }

    private void PopulateGrid(IEnumerable<object[]> rows)
    {
        _grid.Rows.Clear();
        foreach (var row in rows)
            _grid.Rows.Add(row);
    }

    private void OnSearchChanged(object? sender, EventArgs e)
    {
        string q = _search.Text.Trim().ToLowerInvariant();
        var filtered = string.IsNullOrEmpty(q)
            ? _allData
            : _allData.Where(r => r[0].ToString()!.ToLowerInvariant().Contains(q)).ToArray();
        PopulateGrid(filtered);
    }

    private void OnCellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (_grid.Columns[e.ColumnIndex].Name != "Status") return;
        string? val = e.Value?.ToString();
        (Color bg, Color fg) = val switch
        {
            "Active"   => (ColorTranslator.FromHtml("#064E3B"), ColorTranslator.FromHtml("#34D399")),
            "Inactive" => (ColorTranslator.FromHtml("#450A0A"), ColorTranslator.FromHtml("#F87171")),
            "Pending"  => (ColorTranslator.FromHtml("#451A03"), ColorTranslator.FromHtml("#FBBF24")),
            _          => (CardColor, TextPrimary)
        };
        e.CellStyle.BackColor = bg;
        e.CellStyle.ForeColor = fg;
        e.CellStyle.SelectionBackColor = bg;
        e.CellStyle.SelectionForeColor = fg;
    }
}
