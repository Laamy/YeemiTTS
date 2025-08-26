using System.Drawing;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using AutoClicker;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;

namespace YeemiTTS;

// atrribute that contains an action category id(int8) and text (for the context menu item text)
public class ActionAttribute : Attribute
{
    public int CategoryId { get; }
    public string Text { get; }
    public uint[] Keybind { get; set; } = [0];
    public bool Visible { get; set; } = true;
    public ActionAttribute(int categoryId, string text, uint[] keybind, bool visible = true)
    {
        CategoryId = categoryId;
        Text = text;
        Keybind = keybind;
        Visible = visible;
    }
}

public class AppTray : ApplicationContext
{
    private readonly NotifyIcon trayIcon;

    Keymap keymap;
    public AppTray()
    {
        trayIcon = new NotifyIcon()
        {
            Icon = SystemIcons.Shield,
            ContextMenuStrip = new ContextMenuStrip(),
            Visible = true,
            Text = "YeemiTray"
        };

        var items = trayIcon.ContextMenuStrip.Items;

        List<int> categories = new();

        var methods = GetType().GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        this.methods = methods;

        foreach (var method in methods)
        {
            var actionAttr = (ActionAttribute)Attribute.GetCustomAttribute(method, typeof(ActionAttribute));
            if (actionAttr != null)
            {
                if (!categories.Contains(actionAttr.CategoryId))
                {
                    categories.Add(actionAttr.CategoryId);
                    items.Add(new ToolStripSeparator());
                }
                var item = new ToolStripMenuItem(actionAttr.Text, null, (EventHandler)method.CreateDelegate(typeof(EventHandler), this));
                item.Tag = actionAttr.CategoryId;
                items.Add(item);
            }
        }

        keymap = new Keymap();
        keymap.OnKeyPress += APP_OnKeyPress;
        Task.Run(() =>
        {
            while (true)
            {
                keymap.Tick();
                System.Threading.Thread.Sleep(100);
            }
        });
    }

    private MethodInfo[] methods;

    private void APP_OnKeyPress(KeyEvent @event)
    {
        if (@event.VKey != VKeyCodes.KeyUp)
            return;

        foreach (var method in methods)
        {
            var actionAttr = (ActionAttribute)Attribute.GetCustomAttribute(method, typeof(ActionAttribute));

            if (actionAttr == null || !actionAttr.Visible)
                continue;

            bool skip = false;
            foreach (var key in actionAttr.Keybind)
                if (!keymap.GetDown(key))
                    skip = true;

            if (skip)
                continue;

            if (actionAttr != null && actionAttr.Keybind.Last() == @event.Key)
            {
                method.Invoke(this, [this, EventArgs.Empty]);
                break;
            }
        }
    }

    [Action(1, "Toggle", [(uint)Keys.RShiftKey])]
    private void OnSample1(object sender, EventArgs e)
    {
        var form = OverlapForm.Instance;

        form.Invoke((MethodInvoker)delegate
        {
            if (form.Visible)
            {
                form.Hide();
            }
            else
            {
                form.Show();
                form.Focus();
            }
        });
    }

    [Action(255, "Exit", [99999])]
    private void OnExit(object sender, EventArgs e)
    {
        trayIcon.Visible = false;
        Application.Exit();
    }
}
