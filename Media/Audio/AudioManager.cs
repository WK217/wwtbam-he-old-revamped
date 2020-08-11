using CSCore.CoreAudioAPI;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using WwtbamOld.Model;
using System;

namespace WwtbamOld.Media.Audio
{
    public sealed class AudioManager : ReadOnlyReactiveCollection<MMDevice>
    {
        #region Singleton

        private static AudioManager _instance;

        public static AudioManager Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new AudioManager();

                return _instance;
            }
        }

        private AudioManager()
        {
            using MMDeviceEnumerator deviceEnumerator = new MMDeviceEnumerator();
            using MMDeviceCollection deviceCollection = deviceEnumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active);
            foreach (MMDevice device in deviceCollection)
                _collection.Add(device);

            SelectedSoundOut = _collection[0];

            Player1 = new AudioPlayer();
            Player2 = new AudioPlayer();

            this.WhenAnyValue(x => x.SelectedSoundOut)
                .Subscribe(x =>
                {
                    Player1.SelectedSoundOut = x;
                    Player2.SelectedSoundOut = x;
                });
        }

        #endregion Singleton

        #region Properties

        [Reactive] public MMDevice SelectedSoundOut { get; set; }

        private AudioPlayer Player1 { get; }
        private AudioPlayer Player2 { get; }

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

        public void Play(Audio audio)
        {
            AudioPropertiesAttribute props = GetProperties(audio);
            AudioPlayer player = (props.PlayerNumber == 2) ? Player2 : Player1;

            /*string fileName = Path.GetFullPath($"Resources\\Audio\\{properties.FileName}");
            audioPlayer.OpenFromFile(fileName, properties.Loop);*/

            player.OpenFromResource(ResourceManager.GetResourceAudioName(props.FileName), props.Loop);
            player.Play();
        }

        private AudioPropertiesAttribute GetProperties(Audio audio)
            => (AudioPropertiesAttribute)audio.GetType().GetMember(audio.ToString())[0].GetCustomAttributes(typeof(AudioPropertiesAttribute), false)[0];

        public void CleanupPlayback()
        {
            Player1.CleanupPlayback();
            Player2.CleanupPlayback();
        }

        public void PlayLightsDown() => Play(LightsDownAudio);

        public void PlayBackground() => Play(BackgroundAudio);

        public void PlayFinalAnswer() => Play(FinalAnswerAudio);

        public void PlayCorrect() => Play(CorrectAudio);

        public void PlayWrong() => Play(WrongAudio);

        public void PlayWalkaway() => Play(WalkawayAudio);

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
            yield return Audio.PingFifty;
            yield return Audio.PingPhone;
            yield return Audio.PingAtA;
            yield return Audio.PingDoubleDip;
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
    }
}