using NAudio.Wave;

namespace YeemiTTS;

internal class AutoDisposeReader : IWaveProvider
{
    private readonly MediaFoundationResampler reader;
    private bool isDisposed;

    public AutoDisposeReader(MediaFoundationResampler input) => this.reader = input;
    public WaveFormat WaveFormat => reader.WaveFormat;

    public int Read(byte[] buffer, int offset, int count)
    {
        if (isDisposed)
            return 0;

        var read = reader.Read(buffer, offset, count);
        if (read == 0)
        {
            reader.Dispose();
            isDisposed = true;
        }

        return read;
    }
}
