namespace YeemiTTS;

using System;
using System.Diagnostics;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

public class AudioPlaybackEngine : IDisposable
{
    private readonly WaveOutEvent playerDevice;
    private readonly int outputDevice;
    private readonly MixingSampleProvider mixer;

    public AudioPlaybackEngine(int device, int rate = 24000, int channels = 2)
    {
        outputDevice = device >= 0 ? device : 0;

        playerDevice = new WaveOutEvent();
        mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(rate, channels)) { ReadFully = true };

        playerDevice.DeviceNumber = outputDevice;
        playerDevice.Init(mixer);
        playerDevice.Play();
    }

    // FixChannels(Mp3FileReader input, int channels = 2)
    private MediaFoundationResampler FixChannels(IWaveProvider input, int channels = 2)
    {
        var resampler = new MediaFoundationResampler(input, WaveFormat.CreateIeeeFloatWaveFormat(input.WaveFormat.SampleRate, channels));
        return resampler;
    }   

    public void PlaySound(string fileName)
    {
        try
        {
            var input = new Mp3FileReader(fileName);

            mixer.AddMixerInput(new AutoDisposeReader(FixChannels(input)));
        }
        catch
        {
            MessageBox.Show($"Failed to play sound: {fileName} (To long!)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public void Dispose() => playerDevice.Dispose();

}
