using System;

namespace WwtbamOld.Media.Audio
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class AudioPropertiesAttribute : Attribute
    {
        #region Properties

        public byte PlayerNumber { get; }
        public AudioPlayer Player { get; }
        public string FileName { get; }
        public bool Loop { get; }

        #endregion Properties

        public AudioPropertiesAttribute(string fileName, byte playerNumber, bool loop)
        {
            FileName = fileName;

            PlayerNumber = playerNumber;
            Player = AudioManager.Instance.Players[Math.Min(Math.Max((byte)1, PlayerNumber), AudioManager.Instance.Players.Count) - 1];

            Loop = loop;
        }

        public AudioPropertiesAttribute(string fileName, bool loop)
            : this(fileName, 1, loop)
        {
        }

        public AudioPropertiesAttribute(string fileName, byte playerNumber)
            : this(fileName, playerNumber, false)
        {
        }

        public AudioPropertiesAttribute(string fileName)
            : this(fileName, 1, false)
        {
        }

        public static AudioPropertiesAttribute Get(Audio audio)
            => (AudioPropertiesAttribute)audio.GetType().GetMember(audio.ToString())[0].GetCustomAttributes(typeof(AudioPropertiesAttribute), false)[0];
    }
}