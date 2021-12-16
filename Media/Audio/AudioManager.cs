using NAudio.CoreAudioApi;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WwtbamOld.Model;

namespace WwtbamOld.Media.Audio
{
    public sealed class AudioManager : ReactiveObject
    {
        #region Singleton

        private static AudioManager _instance;
        public static AudioManager Instance => _instance ??= new AudioManager();

        private AudioManager()
        {
            List<MMDevice> devicesList = new();
            SoundOutDevices = new ReadOnlyCollection<MMDevice>(devicesList);

            using MMDeviceEnumerator deviceEnumerator = new();
            MMDeviceCollection deviceCollection = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            foreach (MMDevice device in deviceCollection)
                devicesList.Add(device);

            if (deviceEnumerator.HasDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia))
            {
                string defaultDeviceId = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia).ID;
                SelectedSoundOutDevice = devicesList.FirstOrDefault(d => d.ID == defaultDeviceId);
            }

            Players = new ReadOnlyCollection<AudioPlayer>(new List<AudioPlayer>()
            {
                new AudioPlayer("Player #1"),
                new AudioPlayer("Player #2")
            });
        }

        #endregion Singleton

        #region Properties

        public ReadOnlyCollection<MMDevice> SoundOutDevices { get; }
        [Reactive] public MMDevice SelectedSoundOutDevice { get; set; }

        public ReadOnlyCollection<AudioPlayer> Players { get; }

        public Audio LightsDownAudio { get; set; }
        public Audio BackgroundAudio { get; set; }
        public Audio FinalAnswerAudio { get; set; }
        public Audio CorrectAudio { get; set; }
        public Audio WrongAudio { get; set; }
        public Audio WalkawayAudio { get; set; }

        public Level CurrentLevel
        {
            set
            {
                LightsDownAudio = value.LightsDownAudio;
                BackgroundAudio = value.BackgroundAudio;
                FinalAnswerAudio = value.FinalAnswerAudio;
                CorrectAudio = value.CorrectAudio;
                WrongAudio = value.WrongAudio;
                WalkawayAudio = value.WalkawayAudio;
            }
        }

        #endregion Properties

        #region Methods

        private static Audio SelectRandom(Audio[] audio)
        {
            int length = audio.Length;
            return length > 0 ? audio[new Random().Next(0, length)] : 0;
        }

        public static void PlayRandom(FadeParameters fadeOutParams, params Audio[] audio) => Play(SelectRandom(audio), fadeOutParams);

        public static void PlayRandom(params Audio[] audio) => Play(SelectRandom(audio));

        public static void Play(Audio audio, FadeParameters? fadeOutParams = null)
        {
            AudioPropertiesAttribute props = AudioPropertiesAttribute.Get(audio);
            props.Player.Load(props, fadeOutParams);
            props.Player.Play();
        }

        public static AudioPropertiesAttribute GetProperties(Audio audio)
            => (AudioPropertiesAttribute)audio.GetType().GetMember(audio.ToString())[0].GetCustomAttributes(typeof(AudioPropertiesAttribute), false)[0];

        public void PlayLightsDown() => Play(LightsDownAudio);

        public void PlayBackground() => Play(BackgroundAudio);

        public void PlayFinalAnswer() => Play(FinalAnswerAudio);

        public void PlayCorrect() => Play(CorrectAudio);

        public void PlayWrong() => Play(WrongAudio);

        public void PlayWalkaway() => Play(WalkawayAudio);

        public void Dispose()
        {
            foreach (var player in Players)
                player.Dispose();
        }

        #endregion Methods

        #region Collections

        public IEnumerable<Audio> GetGeneralMusic()
        {
            yield return Audio.PreLoop;
            yield return Audio.Opening;
            yield return Audio.Meeting;
            yield return Audio.BackgroundGeneral;
        }

        public IEnumerable<Audio> GetRulesMusic()
        {
            yield return Audio.RulesExplanation;
            yield return Audio.Ping1;
            yield return Audio.Ping2;
            yield return Audio.Ping3;
            yield return Audio.Ping4;
            yield return Audio.RulesEnd;
        }

        public IEnumerable<Audio> GetLightsDownMusic()
        {
            yield return Audio.LetsPlay;
            yield return Audio.LightsDown6;
            yield return Audio.LightsDown7;
            yield return Audio.LightsDown8;
            yield return Audio.LightsDown9;
            yield return Audio.LightsDown10;
            yield return Audio.LightsDown11;
            yield return Audio.LightsDown12;
            yield return Audio.LightsDown13;
            yield return Audio.LightsDown14;
            yield return Audio.LightsDown15;
        }

        public IEnumerable<Audio> GetBackgroundMusic()
        {
            yield return Audio.Background1;
            yield return Audio.Background6;
            yield return Audio.Background7;
            yield return Audio.Background8;
            yield return Audio.Background9;
            yield return Audio.Background10;
            yield return Audio.Background11;
            yield return Audio.Background12;
            yield return Audio.Background13;
            yield return Audio.Background14;
            yield return Audio.Background15;
        }

        public IEnumerable<Audio> GetFinalAnswerMusic()
        {
            yield return Audio.FinalAnswer6;
            yield return Audio.FinalAnswer7;
            yield return Audio.FinalAnswer8;
            yield return Audio.FinalAnswer9;
            yield return Audio.FinalAnswer10;
            yield return Audio.FinalAnswer11;
            yield return Audio.FinalAnswer12;
            yield return Audio.FinalAnswer13;
            yield return Audio.FinalAnswer14;
            yield return Audio.FinalAnswer15;
        }

        public IEnumerable<Audio> GetCorrectMusic()
        {
            yield return Audio.Correct1;
            yield return Audio.Correct6;
            yield return Audio.Correct7;
            yield return Audio.Correct8;
            yield return Audio.Correct9;
            yield return Audio.Correct10;
            yield return Audio.Correct11;
            yield return Audio.Correct12;
            yield return Audio.Correct13;
            yield return Audio.Correct14;
            yield return Audio.Correct15;
        }

        public IEnumerable<Audio> GetWrongMusic()
        {
            yield return Audio.Wrong1;
            yield return Audio.Wrong6;
            yield return Audio.Wrong7;
            yield return Audio.Wrong8;
            yield return Audio.Wrong9;
            yield return Audio.Wrong10;
            yield return Audio.Wrong11;
            yield return Audio.Wrong12;
            yield return Audio.Wrong13;
            yield return Audio.Wrong14;
            yield return Audio.Wrong15;
        }

        public IEnumerable<Audio> GetFiftyFiftyMusic()
        {
            yield return Audio.FiftyUse;
        }

        public IEnumerable<Audio> GetPhoneAFriendMusic()
        {
            yield return Audio.PhoneUse;
            yield return Audio.PhoneStart;
            yield return Audio.PhoneStop;
        }

        public IEnumerable<Audio> GetAskTheAudienceMusic()
        {
            yield return Audio.AtAUse;
            yield return Audio.AtAStart;
            yield return Audio.AtAStop;
            yield return Audio.AtAShow;
        }

        public IEnumerable<Audio> GetDoubleDipMusic()
        {
            yield return Audio.DDUse;
            yield return Audio.DDUseLoop;
            yield return Audio.DDLock1;
            yield return Audio.DDWrong;
            yield return Audio.DDWrongLoop;
            yield return Audio.DDLock2;
        }

        public IEnumerable<Audio> GetWalkawayMusic()
        {
            yield return Audio.SmallWalkaway;
            yield return Audio.SmallWalkaway2;
            yield return Audio.BigWalkaway;
            yield return Audio.BigWalkaway2;
        }

        public IEnumerable<Audio> GetGeneral2Music()
        {
            yield return Audio.CommercialIn;
            yield return Audio.CommercialOut;
            yield return Audio.Goodbye;
            yield return Audio.FinalSiren;
            yield return Audio.Closing;
        }

        #endregion Collections

        ~AudioManager() => Dispose();
    }
}