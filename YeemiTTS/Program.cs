using EdgeTTS;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

public class Program
{
    public static string Speaker = "en-US-AnaNeural";
    public static double Pitch = 0;
    public static double Rate = 20;

    public static EdgeTTSClient etts;
    public static Random ran;

    static async Task Main()
    {
        etts = new EdgeTTSClient();
        ran = new Random();

        CommandHandler.Init();

        while (true)
        {
            Console.Write(">");

            string cmd = Console.ReadLine();

            string[] args = cmd.Split(' ');

            bool found = false;
            foreach (var CMD in CommandHandler.Commands)
            {
                if (CMD.Name.ToLower() == args[0].ToLower())
                {
                    CMD.Execute(args);
                    found = true;
                }
            }

            if (found)
                continue;

            var result = await etts.SynthesisAsync(
                cmd,
                Speaker,
                $"{(Pitch >= 0 ? "+" : "")}{Pitch:f2}Hz",
                $"+{Rate}%"
            );

            var path = $"voice_{ran.Next(0, 100000)}.wav";

            if (result.Code == ResultCode.Success)
            {
                //Console.WriteLine($"msg; {result.Message}");
                //Console.WriteLine($"size; {result.Data.Length}");
                //Console.WriteLine($"code; {result.Code}");

                result.Data.Seek(0, SeekOrigin.Begin);

                byte[] dataBytes = new byte[result.Data.Length];
                result.Data.Read(dataBytes, 0, (int)result.Data.Length);

                File.WriteAllBytes(path, dataBytes);

                //result.Data.Seek(0, SeekOrigin.Begin);

                PlaySound(path);

                File.Delete(path);

                Console.WriteLine($"Saved to: (path)");
            }
            else Console.WriteLine("Failure in Synthesising.");
        }

        Console.ReadKey();
    }

    public static void PlaySound(string path)
    {
        // Get the list of available audio playback devices
        MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
        MMDeviceCollection devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

        // Find the audio device by name
        MMDevice outputDevice = devices.FirstOrDefault(d => d.FriendlyName == "CABLE Input (VB-Audio Virtual Cable)");

        int deviceIndex = -1;
        
        if (outputDevice == null)
        {
            //Console.WriteLine("Could not find the specified audio device.");
            //return;
        }
        else
        {
            for (int i = 0; i < devices.Count; i++)
            {
                if (devices[i].ID == outputDevice.ID)
                {
                    deviceIndex = i + 1;
                    break;
                }
            }
        }

        using (var waveOut = new WaveOutEvent())
        {
            using (var mp3Reader = new Mp3FileReader(path))
            {
                using (var waveStream = WaveFormatConversionStream.CreatePcmStream(mp3Reader))
                {
                    waveOut.DeviceNumber = deviceIndex;

                    waveOut.Init(waveStream);
                    waveOut.Play();

                    while (waveOut.PlaybackState == PlaybackState.Playing) { }
                }
            }
        }
    }
}