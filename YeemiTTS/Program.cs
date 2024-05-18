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
    public static double Pitch = -15;
    public static double Rate = 0;

    public static EdgeTTSClient etts;
    public static Random ran;

    static async Task Main(string[] args)
    {
        etts = new EdgeTTSClient();
        ran = new Random();

        while (true)
        {
            Console.Write(">");

            string cmd = Console.ReadLine();

            if (cmd == ";reset")
            {
                etts = new EdgeTTSClient();
                continue; // skip synthesis step
            }

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

        if (outputDevice == null)
        {
            Console.WriteLine("Could not find the specified audio device.");
            //return;
        }

        //Console.ReadKey();
        int deviceIndex = -1;
        for (int i = 0; i < devices.Count; i++)
        {
            if (devices[i].ID == outputDevice.ID)
            {
                deviceIndex = i + 1;
                break;
            }
        }

        //Console.WriteLine(deviceIndex);

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