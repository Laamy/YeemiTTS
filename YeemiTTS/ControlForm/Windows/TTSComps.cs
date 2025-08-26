namespace YeemiTTS.ControlForm.Windows;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EdgeTTS;
using NAudio.CoreAudioApi;
using NAudio.Wave;

internal class TTSComps
{
    // deep - en-US-BrianMultilingualNeural
    // chinese 10 - en-US-AnaNeural
    public static string Speaker = "en-US-AnaNeural";
    public static double Pitch = 0;
    public static double Rate = 20;

    public static EdgeTTSClient ttsClient;
    public static AiClient aiClient;

    public static Random ran;

    public static void Init()
    {
        ttsClient = new EdgeTTSClient();
        aiClient = new AiClient();
        aiClient.Init();
        ran = new Random();
    }

    public static async Task Speak(string text)
    {
        var result = await ttsClient.SynthesisAsync(
            text,
            Speaker,
            $"{(Pitch >= 0 ? "+" : "")}{Pitch:f2}Hz",
            $"+{Rate}%"
        );

        var path = $"voice_{ran.Next(0, 100000)}.wav";

        if (result.Code == ResultCode.Success)
        {
            result.Data.Seek(0, SeekOrigin.Begin);

            var dataBytes = new byte[result.Data.Length];
            result.Data.Read(dataBytes, 0, (int)result.Data.Length);

            File.WriteAllBytes(path, dataBytes);

            PlaySound(path);

            File.Delete(path);

            Console.WriteLine($"Saved to: (path)");
        }
        else Console.WriteLine($"Failure in Synthesising. (Error: {result.Code})");
    }

    public static void PlaySound(string path)
    {
        MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
        MMDeviceCollection devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

        MMDevice outputDevice = devices.FirstOrDefault(d => d.FriendlyName == "CABLE Input (VB-Audio Virtual Cable)");

        var deviceIndex = -1;

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

        using var waveOut = new WaveOutEvent();
        using var mp3Reader = new Mp3FileReader(path);
        using var waveStream = WaveFormatConversionStream.CreatePcmStream(mp3Reader);

        waveOut.DeviceNumber = deviceIndex;

        waveOut.Init(waveStream);
        waveOut.Play();

        while (waveOut.PlaybackState == PlaybackState.Playing) { }
    }
}
