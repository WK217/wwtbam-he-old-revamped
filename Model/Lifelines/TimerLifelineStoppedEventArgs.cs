using System;

namespace WwtbamOld.Model
{
    public sealed class TimerLifelineStoppedEventArgs : EventArgs
    {
        public TimerLifelineStoppedEventArgs(bool ahead)
        {
            Ahead = ahead;
        }

        public bool Ahead { get; }
    }
}