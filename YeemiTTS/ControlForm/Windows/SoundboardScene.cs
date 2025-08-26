namespace YeemiTTS.ControlForm.Windows;

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

using Serilog;

using EdgeTTS;
using Serilog.Events;

public partial class SoundboardScene : Form
{
    public SoundboardScene()
    {
        // LoggerConfiguration to COnsole.Writeline
        // Log.Error(ex.ToString());
        // Log.Information($"[EdegTTS]连接失败，开始第{retryTimes}次重试 :(");
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        Log.Information("teset");

        InitializeComponent();
        DoubleBuffered = true;

        Program.ttsClient = new EdgeTTSClient();
        Program.ran = new Random();

        foreach (var file in Directory.GetFiles("Soundboard", "*.mp3"))
        {
            listView1.Items.Add(new ListViewItem
            {
                Text = Path.GetFileNameWithoutExtension(file),
                Tag = file,
                ForeColor = SystemColors.Control
            });
        }

        listView1.Click += (s, e) =>
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var item = listView1.SelectedItems[0];
                var path = item.Tag.ToString();
                if (File.Exists(path))
                    Program.PlaySound(path);
            }
        };
    }

    private void SoundboardScene_Activated(object sender, EventArgs e)
    {
        textBox1.Focus();
    }

    // messy ass code
    bool isSynthesizing = false;
    private void textBox1_KeyDown(object sender, KeyEventArgs e)
    {
        if (isSynthesizing)
            return;

        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;

            if (string.IsNullOrWhiteSpace(textBox1.Text))
                return;

            var text = textBox1.Text;
            textBox1.Clear();

            isSynthesizing = true;
            Task.Factory.StartNew(() =>
            {
                //history.Add(text);
                //historyIndex = history.Count;

                var result = Program.ttsClient.SynthesisAsync(
                    text,
                    Program.Speaker,
                    $"{(Program.Pitch >= 0 ? "+" : "")}{Program.Pitch:f2}Hz",
                    $"+{Program.Rate}%"
                ).GetAwaiter().GetResult();
                var path = $"Cache\\voice_{Program.ran.Next(0, 100000)}.wav";
                Directory.CreateDirectory("Cache");

                if (result.Code == ResultCode.Success)
                {
                    result.Data.Seek(0, SeekOrigin.Begin);
                    var dataBytes = new byte[result.Data.Length];
                    result.Data.Read(dataBytes, 0, (int)result.Data.Length);
                    File.WriteAllBytes(path, dataBytes);
                    //result.Data.Seek(0, SeekOrigin.Begin);
                    Program.PlaySound(path);
                    Console.WriteLine($"Saved to: (path)");
                }
                else
                {
                    MessageBox.Show("Failure in Synthesising. (Error: " + result.Code + ")", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Program.ttsClient = new EdgeTTSClient();
                }

                isSynthesizing = false;
            });
        }
    }
}
