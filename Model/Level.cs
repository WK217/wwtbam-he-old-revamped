using ReactiveUI;
using WwtbamOld.Media.Audio;

namespace WwtbamOld.Model;

public sealed class Level : ReactiveObject
{
    public byte Number { get; }
    public float Prize { get; }
    public string Description => ToString();

    public Level(byte number, float prize)
    {
        Number = number;
        Prize = prize;
    }

    #region Audio

    public Audio LightsDownAudio => Number >= 6 ? Audio.LightsDown6 + Number - 6 : Audio.LetsPlay;
    public Audio BackgroundAudio => Number >= 6 ? Audio.Background6 + Number - 6 : Audio.Background1;
    public Audio FinalAnswerAudio => Number >= 6 ? Audio.FinalAnswer6 + Number - 6 : Audio.Silence2;
    public Audio CorrectAudio => Number < 5 ? Audio.Correct1 : Audio.Correct5 + Number - 5;
    public Audio WrongAudio => Number < 6 ? Audio.Wrong1 : Audio.Wrong6 + Number - 6;
    public Audio WalkawayAudio => Number >= 11 ? Audio.BigWalkaway2 : Audio.SmallWalkaway2;

    #endregion Audio

    #region Graphics

    /*public ImageSource BigMoneyTreeImage => GraphicsManager.GetBigMoneyTreeImage(Number);
    public ImageSource SmallMoneyTreeImage => GraphicsManager.GetSmallMoneyTreeImage(Number);
    public ImageSource SumImage => GraphicsManager.GetSumImage(Number - 1);
    public ImageSource PrizeImage => GraphicsManager.GetPrizeImage(Number);*/

    #endregion Graphics

    public override string ToString()
    {
        return string.Format("{0}. {1:C0}", Number, Prize);
    }
}
