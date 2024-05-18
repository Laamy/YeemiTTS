using EdgeTTS;

using System;
using System.IO;
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
                Console.WriteLine($"msg; {result.Message}");
                Console.WriteLine($"size; {result.Data.Length}");
                Console.WriteLine($"code; {result.Code}");

                result.Data.Seek(0, SeekOrigin.Begin);

                byte[] dataBytes = new byte[result.Data.Length];
                result.Data.Read(dataBytes, 0, (int)result.Data.Length);

                File.WriteAllBytes(path, dataBytes);

                //result.Data.Seek(0, SeekOrigin.Begin);

                var player = new MediaPlayer();
                player.Open(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}\\{path}"));
                player.Play();

                // Wait for playback to finish
                while (player.Position < player.NaturalDuration) {}

                Console.WriteLine($"Saved to: (path)");
            }
            else Console.WriteLine("Failure in Synthesising.");
        }

        Console.ReadKey();
    }
}