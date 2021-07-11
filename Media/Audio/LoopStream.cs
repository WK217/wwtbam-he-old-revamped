using NAudio.Wave;

namespace WwtbamOld.Media.Audio
{
    internal sealed class LoopStream : WaveStream
    {
        #region Fields

        private readonly WaveStream _sourceStream;

        #endregion Fields

        public LoopStream(WaveStream sourceStream, bool loop = false)
        {
            _sourceStream = sourceStream;
            EnableLooping = loop;
        }

        #region Properties

        public bool EnableLooping { get; private set; }
        public override WaveFormat WaveFormat => _sourceStream.WaveFormat;
        public override long Length => _sourceStream.Length;

        public override long Position
        {
            get => _sourceStream.Position;
            set => _sourceStream.Position = value;
        }

        #endregion Properties

        #region Methods

        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalBytesRead = 0;

            while (totalBytesRead < count)
            {
                int bytesRead = _sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);

                if (bytesRead == 0)
                {
                    if (_sourceStream.Position == 0 || !EnableLooping)
                        break;

                    _sourceStream.Position = 0;
                }

                totalBytesRead += bytesRead;
            }

            return totalBytesRead;
        }

        #endregion Methods
    }
}