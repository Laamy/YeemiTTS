using EdgeTTS;

using NAudio.CoreAudioApi;
using NAudio.Wave;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using YeemiTTS;

public class Program
{
    // deep - en-US-BrianMultilingualNeural
    // chinese 10 - en-US-AnaNeural
    public static string Speaker = "en-US-AnaNeural";
    public static double Pitch = 0;
    public static double Rate = 10;

    public static EdgeTTSClient ttsClient;
    public static AiClient aiClient;

    public static Random ran;

    [STAThread]
    static async Task Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        new AppTray();
        Application.Run(new OverlapForm());
        Application.Exit();

        //while (true)
        //{
        //    Console.Clear();
        //    ttsClient = new EdgeTTSClient();
        //    aiClient = new AiClient();
        //    aiClient.Init();
        //    //await aiClient.AskAIAsync("boot", true);
        //    ran = new Random();
        //    CommandHandler.Init();
        //    while (true)
        //    {
        //        Console.Write(">");
        //        var cmd = Console.ReadLine();
        //        var args = cmd.Split(' ');
        //        var found = false;
        //        foreach (var CMD in CommandHandler.Commands)
        //        {
        //            if (CMD.Name.ToLower() == args[0].ToLower())
        //            {
        //                CMD.Execute(args);
        //                found = true;
        //            }
        //        }
        //        if (found)
        //            continue;
        //        var result = await ttsClient.SynthesisAsync(
        //            cmd,
        //            Speaker,
        //            $"{(Pitch >= 0 ? "+" : "")}{Pitch:f2}Hz",
        //            $"+{Rate}%"
        //        );
        //        var path = $"voice_{ran.Next(0, 100000)}.wav";
        //        if (result.Code == ResultCode.Success)
        //        {
        //            result.Data.Seek(0, SeekOrigin.Begin);
        //            var dataBytes = new byte[result.Data.Length];
        //            result.Data.Read(dataBytes, 0, (int)result.Data.Length);
        //            File.WriteAllBytes(path, dataBytes);
        //            //result.Data.Seek(0, SeekOrigin.Begin);
        //            PlaySound(path);
        //            File.Delete(path);
        //            Console.WriteLine($"Saved to: (path)");
        //        }
        //        else Console.WriteLine("Failure in Synthesising.");
        //    }
        //}
    }

    static MMDeviceEnumerator enumerator;
    static MMDeviceCollection devices;
    static MMDevice outputDevice;
    static int deviceIndex;
    public static void Init()
    {
        enumerator = new MMDeviceEnumerator();
        devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

        outputDevice = devices.FirstOrDefault(d => d.FriendlyName == "CABLE Input (VB-Audio Virtual Cable)");

        deviceIndex = -1;

        if (outputDevice == null)
        {
            Console.WriteLine("Could not find the specified audio device.");
            return;
        }
        else
        {
            for (var i = 0; i < devices.Count; i++)
            {
                if (devices[i].ID == outputDevice.ID)
                {
                    deviceIndex = i + 1;
                    break;
                }
            }
        }
    }

    static AudioPlaybackEngine engine = null;
    public static void PlaySound(string path)
    {
        if (engine == null)
        {
            Init();
            engine = new(deviceIndex);
        }

        engine.PlaySound(path);
    }
}