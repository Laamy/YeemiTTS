using System.Drawing;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using YeemiTTS.ControlForm.Windows;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data;

public partial class OverlapForm : ControlForm
{
    public Color AdjustedForeColor()
    {
        Color colorCalc = TitleBarPanel.BackColor;

        // check if dark or light color
        int r = colorCalc.R;
        int g = colorCalc.G;
        int b = colorCalc.B;
        var luminance = (0.299 * r + 0.587 * g + 0.114 * b) / 255;

        return luminance > 0.5 ? Color.Black : SystemColors.Control;
    }

    public Dictionary<string, Form> tabForms = new();
    public List<(string icon, string name, Form form)> tabs = [
        ("", "Home", new HomeScene()),
        ("\ue720", "Soundboards", new SoundboardScene()),
        ("\ue9d5", "Star Charts", new StarChartScene()),
    ];

    public void SetActivePage(Form form)
    {
        foreach (var f in tabForms.Values)
            if (f != form)
            {
                f.Dock = DockStyle.None;
                f.Location = new Point(-1000, 0);
            }
            else
            {
                f.Dock = DockStyle.Fill;
                f.Location = new Point(0, 0);
            }
    }

    public static OverlapForm Instance { get; private set; } = null;
    public OverlapForm()
    {
        Instance = this;
        InitializeComponent();
        TopMost = true;

        foreach (var tab in tabs)
        {
            tab.form.StartPosition = FormStartPosition.Manual;
            tabForms.Add(tab.name, tab.form);
            //tab.form.Dock = tabForms.Count == 0 ? DockStyle.Fill : DockStyle.None;
            //tab.form.Location = tabForms.Count == 0 ? new Point(0, 0) : new Point(-1000, 0);
            tab.form.FormBorderStyle = FormBorderStyle.None;
            tab.form.TopLevel = false;
            tab.form.Visible = true;
            tab.form.MdiParent = controlContainer1.MdiForm;

            // new btn
            ControlLabel icon, name;
            var btn = new Panel
            {
                Name = "tab_" + tab.name,
                Size = new Size(137, 30),
                BackColor = Color.FromArgb(38, 38, 38),
                ForeColor = SystemColors.Control,
                Font = new Font("Arial", 9.75f, FontStyle.Regular), // Arial, 9.75pt
                Dock = DockStyle.Top,
                Cursor = Cursors.Hand
            };

            btn.Controls.Add(icon = new ControlLabel
            {
                Text = tab.icon,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe Fluent Icons", 9.75f, FontStyle.Regular), // Segoe Fluent Icons, 9.75pt
                Dock = DockStyle.Left,
                Size = new Size(34, 30),
            });

            btn.Controls.Add(name = new ControlLabel
            {
                Text = tab.name,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent,
                Dock = DockStyle.Right,
                Size = new Size(97, 30),
            });

            void OnPressed(object sender, EventArgs e) => SetActivePage(tab.form);

            void OnHover(object sender, EventArgs e)
            {
                btn.BackColor = Color.FromArgb(48, 48, 48);
            }
            
            void OnHoverLeave(object sender, EventArgs e)
            {
                btn.BackColor = Color.FromArgb(38, 38, 38);
            }

            btn.Click += OnPressed;
            btn.MouseEnter += OnHover;
            btn.MouseLeave += OnHoverLeave;

            name.Click += OnPressed;
            name.MouseEnter += OnHover;
            name.MouseLeave += OnHoverLeave;

            icon.Click += OnPressed;
            icon.MouseEnter += OnHover;
            icon.MouseLeave += OnHoverLeave;

            panel1.Controls.Add(btn);
            btn.BringToFront();
        }

        SetActivePage(tabForms.FirstOrDefault().Value);

        this.Text = "Yems utils";
        label6.Text = Text;

        DoubleBuffered = true;

        TitleBar.SetMovable(this, TitleBarPanel);
        TitleBar.SetMovable(this, panel3);

        // animations n code for titlebar btns
        {
            Titlebar_MinBtn.Click += (s, e) => WindowState = FormWindowState.Minimized;
            Titlebar_XBtn.Click += (s, e) => Close();//Application.Exit();
            Titlebar_MinMaxBtn.Click += (s, e) =>
            {
                WindowState = WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;
                Titlebar_MinMaxBtn.Text = WindowState == FormWindowState.Maximized ? "" : "";
            };

            // some animations for the titlebar btns
            void TitlebarBtn_MouseEnter(object sender, EventArgs e)
            {
                var btn = sender as Label;
                btn.ForeColor = Color.Gray;

                var t = TitleBarPanel.BackColor;
                btn.BackColor = Color.FromArgb(t.R + 15, t.G + 15, t.B + 15);
            }

            void TitlebarBtn_MouseLeave(object sender, EventArgs e)
            {
                var btn = sender as Label;
                btn.ForeColor = AdjustedForeColor();
                btn.BackColor = TitleBarPanel.BackColor;
            }

            TitleBarPanel.BackColor = panel2.BackColor;

            Titlebar_MinBtn.MouseEnter += TitlebarBtn_MouseEnter;
            Titlebar_MinBtn.MouseLeave += TitlebarBtn_MouseLeave;
            Titlebar_MinMaxBtn.MouseEnter += TitlebarBtn_MouseEnter;
            Titlebar_MinMaxBtn.MouseLeave += TitlebarBtn_MouseLeave;
            Titlebar_XBtn.MouseEnter += TitlebarBtn_MouseEnter;
            Titlebar_XBtn.MouseLeave += TitlebarBtn_MouseLeave;

            void FixTitlebarBtns()
            {
                Titlebar_MinBtn.ForeColor = AdjustedForeColor();
                Titlebar_MinBtn.BackColor = TitleBarPanel.BackColor;

                Titlebar_MinMaxBtn.ForeColor = AdjustedForeColor();
                Titlebar_MinMaxBtn.BackColor = TitleBarPanel.BackColor;

                Titlebar_XBtn.ForeColor = AdjustedForeColor();
                Titlebar_XBtn.BackColor = TitleBarPanel.BackColor;

            }

            Activated += (s, e) =>
            {
                FixTitlebarBtns();

                Invalidate();
            };

            Deactivate += (s, e) =>
            {
                FixTitlebarBtns();

                Invalidate();
            };
        }
    }

    private void OverlapForm_Shown(object sender, EventArgs e) => Hide();
}
