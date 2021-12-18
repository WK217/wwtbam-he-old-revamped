using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.IO;
using System.Reflection;

namespace WwtbamOld.Media.Audio;

public sealed class AudioPlayer : IDisposable
{
    #region Fields

    private IWavePlayer _outputDevice;
    private LoopStream _fileStream;
    private AudioPropertiesAttribute _audioProperties;

    #endregion Fields

    public AudioPlayer()
    {
    }

    public AudioPlayer(string name)
        : this()
    {
        Name = name;
    }

    #region Properties

    public string Name { get; }

    public TimeSpan CurrentTime => _fileStream.CurrentTime;

    public double Position
    {
        get => _fileStream is not null ? Convert.ToDouble(1000 * _fileStream.Position / _fileStream.Length) : 0;
        set
        {
            if (_fileStream is not null)
                _fileStream.Position = Convert.ToInt64(value * _fileStream.Length / 1000);
        }
    }

    public string FileName => _audioProperties.FileName;
    public string NowPlaying => Path.GetFileNameWithoutExtension(FileName);

    #endregion Properties

    #region Methods

    public void Load(AudioPropertiesAttribute audioProps, FadeParameters? fadeOutParams = null)
    {
        _audioProperties = audioProps;

        Dispose();
        EnsureDeviceCreated();

        OpenFile(fadeOutParams);
    }

    public void Load(Audio audio, FadeParameters? fadeOutParams = null) => Load(AudioPropertiesAttribute.Get(audio), fadeOutParams);

    private void OpenFile(FadeParameters? fadeOutParams = null)
    {
        ISampleProvider inputStream = CreateInputStream(_audioProperties.FileName, _audioProperties.Loop);
        if (fadeOutParams.HasValue)
        {
            FadeInOutSampleProvider fadeOut = new(inputStream);
            fadeOut.BeginFadeOut(
                (double)fadeOutParams.Value.FadeOutStart.TotalMilliseconds,
                (double)fadeOutParams.Value.FadeOutDuration.TotalMilliseconds);

            inputStream = fadeOut;
        }

        _outputDevice.Init(new SampleToWaveProvider(inputStream));
    }

    private ISampleProvider CreateInputStream(string fileName, bool loop = false)
    {
        string resourceName = $"WwtbamOld.Resources.Audio.{fileName}";

        if (fileName.EndsWith(".wav"))
            _fileStream = OpenWaveStream(resourceName, loop);
        else if (fileName.EndsWith(".mp3"))
            _fileStream = new LoopStream(new Mp3FileReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)), loop);
        else
            throw new InvalidOperationException("Unsupported extension");

        SampleChannel inputStream = new(_fileStream);
        NotifyingSampleProvider sampleStream = new(inputStream);

        return sampleStream;
    }

    private static LoopStream OpenWaveStream(string fileName, bool loop = false)
    {
        WaveStream readerStream = new WaveFileReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(fileName));

        if (readerStream.WaveFormat.Encoding != WaveFormatEncoding.Pcm)
        {
            readerStream = WaveFormatConversionStream.CreatePcmStream(readerStream);
            readerStream = new BlockAlignReductionStream(readerStream);
        }

        return new LoopStream(readerStream, loop);
    }

    public void Play() => _outputDevice?.Play();

    public void Pause() => _outputDevice?.Pause();

    public void Stop()
    {
        if (_outputDevice is not null & _fileStream is not null)
        {
            _outputDevice.Stop();
            _fileStream.Position = 0;
        }
    }

    private void CloseFile()
    {
        if (_fileStream is not null)
        {
            _fileStream.Dispose();
            _fileStream = null;
        }
    }

    private void EnsureDeviceCreated()
    {
        if (_outputDevice is null)
            CreateOutputDevice();
    }

    private void CreateOutputDevice() => _outputDevice = new WasapiOut(AudioManager.Instance.SelectedSoundOutDevice, AudioClientShareMode.Shared, true, 200);

    // _outputDevice = new WaveOutEvent();
    // _outputDevice = new WasapiOut(AudioManager.Instance.SelectedOutputDevice, AudioClientShareMode.Shared, true, 200);

    public void Dispose()
    {
        Stop();
        CloseFile();

        _outputDevice?.Dispose();
        _outputDevice = null;

        _fileStream?.Dispose();
        _fileStream = null;
    }

    #endregion Methods

    public override string ToString() => Name;

    ~AudioPlayer() => Dispose();
}
