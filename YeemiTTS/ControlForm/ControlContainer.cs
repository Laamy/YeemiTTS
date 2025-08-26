using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class ControlContainer : Label
{
    private Form mdiForm;
    private MdiClient mdiClient = new();

    public ControlContainer()
    {
        DoubleBuffered = true;
        Disable3DBorder(mdiClient);
        Controls.Add(this.mdiClient);
    }

    public Form MdiForm
    {
        get
        {
            if (mdiForm == null)
            {
                mdiForm = new Form();
                FieldInfo fieldInfo = typeof(Form).GetField("ctlClient", BindingFlags.NonPublic | BindingFlags.Instance);
                fieldInfo.SetValue(mdiForm, mdiClient);
            }

            return mdiForm;
        }
    }

    private void Disable3DBorder(MdiClient client)
    {
        const int GWL_EXSTYLE = -20;
        const int WS_EX_CLIENTEDGE = 0x00000200;

        int exStyle = GetWindowLong(client.Handle, GWL_EXSTYLE);
        exStyle &= ~WS_EX_CLIENTEDGE;
        SetWindowLong(client.Handle, GWL_EXSTYLE, exStyle);

        client.Invalidate();
    }

    [DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
}