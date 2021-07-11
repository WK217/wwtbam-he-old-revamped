using System;

namespace WwtbamOld.Media.Audio
{
    public struct FadeParameters
    {
        public FadeParameters(int fadeInStart, int fadeInDuration, int fadeOutStart, int fadeOutDuration)
        {
            FadeInStart = new TimeSpan(0, 0, 0, 0, fadeInStart);
            FadeInDuration = new TimeSpan(0, 0, 0, 0, fadeInDuration);

            FadeOutStart = new TimeSpan(0, 0, 0, 0, fadeOutStart);
            FadeOutDuration = new TimeSpan(0, 0, 0, 0, fadeOutDuration);
        }

        #region Properties

        public TimeSpan FadeInStart { get; }
        public TimeSpan FadeInDuration { get; }

        public TimeSpan FadeOutStart { get; }
        public TimeSpan FadeOutDuration { get; }

        #endregion Properties
    }
}