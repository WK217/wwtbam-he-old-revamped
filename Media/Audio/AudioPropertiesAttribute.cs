using System;

namespace WwtbamOld.Media.Audio
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    internal sealed class AudioPropertiesAttribute : Attribute
    {
        public byte PlayerNumber { get; }
        public string FileName { get; }
        public bool Loop { get; }

        public AudioPropertiesAttribute(string fileName, byte playerNumber, bool loop)
        {
            //FileName = $"wwtbam {fileName}.mp3";
            FileName = fileName;
            PlayerNumber = (byte)((playerNumber == 1 || playerNumber == 2) ? playerNumber : 1);
            Loop = loop;
        }

        public AudioPropertiesAttribute(string fileName, bool loop) : this(fileName, 1, loop)
        {
        }

        public AudioPropertiesAttribute(string fileName, byte playerNumber) : this(fileName, playerNumber, false)
        {
        }

        public AudioPropertiesAttribute(string fileName) : this(fileName, 1, false)
        {
        }
    }
}