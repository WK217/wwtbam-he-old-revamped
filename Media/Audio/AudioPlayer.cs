using CSCore;
using CSCore.Codecs;
using CSCore.Codecs.MP3;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using CSCore.Streams;
using System;
using System.IO;

namespace WwtbamOld.Media.Audio
{
    public sealed class AudioPlayer : IDisposable
    {
        #region Fields

        private ISoundOut _soundOut;
        private IWaveSource _waveSource;

        #endregion Fields

        public event EventHandler<PlaybackStoppedEventArgs> PlaybackStopped;
        public AudioPlayer()
        {
        }

        #region Properties

        public MMDevice SelectedSoundOut { get; set; }

        public PlaybackState PlaybackState => _soundOut?.PlaybackState ?? PlaybackState.Stopped;

        public TimeSpan Position
        {
            get => _waveSource?.GetPosition() ?? TimeSpan.Zero;
            set => _waveSource?.SetPosition(value);
        }

        public TimeSpan Length => _waveSource?.GetLength() ?? TimeSpan.Zero;

        public int Volume
        {
            get => _soundOut != null ? Math.Min(100, Math.Max((int)(_soundOut.Volume * 100f), 0)) : 100;
            set
            {
                if (_soundOut != null)
                    _soundOut.Volume = Math.Min(1f, Math.Max(value / 100f, 0f));
            }
        }

        public bool Loop { get; set; }

        #endregion Properties

        #region Methods

        private void Open(IWaveSource waveSource, bool loop = false)
        {
            CleanupPlayback();
            _waveSource = waveSource;
            _soundOut = GetSoundOut();
            _soundOut.Initialize(_waveSource);

            Loop = loop;
        }

        public void OpenFromFile(string fileName, bool loop = false)
            => Open(CodecFactory.Instance.GetCodec(fileName).ToSampleSource().ToMono().ToWaveSource(), loop);

        public void OpenFromResource(string resourceName, bool loop = false)
        {
            Stream resourceStream = ResourceManager.GetAudioStream(resourceName);
            LoopStream loopStream = new LoopStream(new DmoMp3Decoder(resourceStream)) { EnableLoop = loop };
            Open(loopStream, loop);
        }

        private ISoundOut GetSoundOut()
            => WasapiOut.IsSupportedOnCurrentPlatform ? new WasapiOut { Latency = 100, Device = SelectedSoundOut } : (ISoundOut)new DirectSoundOut();

        public void Play() => _soundOut?.Play();

        public void Pause() => _soundOut?.Pause();

        public void Stop() => _soundOut?.Stop();

        public void CleanupPlayback()
        {
            _soundOut?.Dispose();
            _soundOut = null;

            _waveSource?.Dispose();
            _waveSource = null;
        }

        void IDisposable.Dispose() => CleanupPlayback();

        #endregion Methods
    }
}