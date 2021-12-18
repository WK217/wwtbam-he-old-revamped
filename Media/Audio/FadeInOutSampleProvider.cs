using NAudio.Wave;

namespace WwtbamOld.Media.Audio;

public sealed class FadeInOutSampleProvider : ISampleProvider
{
    private enum FadeState : byte
    {
        Silence,
        FadingIn,
        FullVolume,
        FadingOut,
    }

    #region Fields

    private readonly object _lockObject = new();

    private readonly ISampleProvider _source;

    private int _fadeSamplePosition;
    private int _fadeSampleCount;

    private int _fadeOutDelaySamples;
    private int _fadeOutDelayPosition;

    private FadeState _fadeState;

    #endregion Fields

    /// <summary>
    /// Creates a new FadeInOutSampleProvider
    /// </summary>
    /// <param name="source">The source stream with the audio to be faded in or out</param>
    /// <param name="initiallySilent">If true, we start faded out</param>
    public FadeInOutSampleProvider(ISampleProvider source, bool initiallySilent = false)
    {
        _source = source;
        _fadeState = initiallySilent ? FadeState.Silence : FadeState.FullVolume;
    }

    #region Properties

    /// <summary>
    /// WaveFormat of this SampleProvider
    /// </summary>
    public WaveFormat WaveFormat => _source.WaveFormat;

    #endregion Properties

    #region Methods

    /// <summary>
    /// Requests that a fade-in begins (will start on the next call to Read)
    /// </summary>
    /// <param name="fadeDurationInMilliseconds">Duration of fade in milliseconds</param>
    public void BeginFadeIn(double fadeDurationInMilliseconds)
    {
        lock (_lockObject)
        {
            _fadeSamplePosition = 0;
            _fadeSampleCount = (int)(fadeDurationInMilliseconds * _source.WaveFormat.SampleRate / 1000);
            _fadeState = FadeState.FadingIn;
        }
    }

    /// <summary>
    /// Requests that a fade-out begins
    /// </summary>
    /// <param name="fadeAfterMilliseconds">Start of fade in milliseconds from beginning</param>
    /// <param name="fadeDurationInMilliseconds">Duration of fade in milliseconds</param>
    public void BeginFadeOut(double fadeAfterMilliseconds, double fadeDurationInMilliseconds)
    {
        lock (_lockObject)
        {
            _fadeSamplePosition = 0;
            _fadeSampleCount = (int)(fadeDurationInMilliseconds * _source.WaveFormat.SampleRate / 1000);
            _fadeOutDelaySamples = (int)(fadeAfterMilliseconds * _source.WaveFormat.SampleRate / 1000);
            _fadeOutDelayPosition = 0;

            //fadeState = FadeState.FadingOut;
        }
    }

    /// <summary>
    /// Reads samples from this sample provider
    /// </summary>
    /// <param name="buffer">Buffer to read into</param>
    /// <param name="offset">Offset within buffer to write to</param>
    /// <param name="count">Number of samples desired</param>
    /// <returns>Number of samples read</returns>
    public int Read(float[] buffer, int offset, int count)
    {
        int sourceSamplesRead = _source.Read(buffer, offset, count);

        lock (_lockObject)
        {
            if (_fadeOutDelaySamples > 0)
            {
                _fadeOutDelayPosition += sourceSamplesRead / WaveFormat.Channels;
                if (_fadeOutDelayPosition >= _fadeOutDelaySamples)
                {
                    _fadeOutDelaySamples = 0;
                    _fadeState = FadeState.FadingOut;
                }
            }

            if (_fadeState == FadeState.FadingIn)
                FadeIn(buffer, offset, sourceSamplesRead);
            else if (_fadeState == FadeState.FadingOut)
                FadeOut(buffer, offset, sourceSamplesRead);
            else if (_fadeState == FadeState.Silence)
                ClearBuffer(buffer, offset, count);
        }

        return sourceSamplesRead;
    }

    private static void ClearBuffer(float[] buffer, int offset, int count)
    {
        for (int n = 0; n < count; n++)
            buffer[n + offset] = 0;
    }

    private void FadeOut(float[] buffer, int offset, int sourceSamplesRead)
    {
        int sample = 0;
        while (sample < sourceSamplesRead)
        {
            float multiplier = 1.0f - (_fadeSamplePosition / (float)_fadeSampleCount);

            for (int ch = 0; ch < _source.WaveFormat.Channels; ch++)
                buffer[offset + sample++] *= multiplier;

            _fadeSamplePosition++;
            if (_fadeSamplePosition > _fadeSampleCount)
            {
                _fadeState = FadeState.Silence;
                // clear out the end
                ClearBuffer(buffer, sample + offset, sourceSamplesRead - sample);
                break;
            }
        }
    }

    private void FadeIn(float[] buffer, int offset, int sourceSamplesRead)
    {
        int sample = 0;
        while (sample < sourceSamplesRead)
        {
            float multiplier = (_fadeSamplePosition / (float)_fadeSampleCount);

            for (int ch = 0; ch < _source.WaveFormat.Channels; ch++)
                buffer[offset + sample++] *= multiplier;

            _fadeSamplePosition++;
            if (_fadeSamplePosition > _fadeSampleCount)
            {
                _fadeState = FadeState.FullVolume;
                // no need to multiply any more
                break;
            }
        }
    }

    #endregion Methods
}
