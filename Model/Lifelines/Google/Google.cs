using System.ComponentModel;
using WwtbamOld.Media.Audio;

namespace WwtbamOld.Model;

[Description("«Помощь Интернета»")]
public sealed class Google : TimerLifeline
{
    #region Basic Properties

    public override byte ID => 5;
    public override string Code => "google";
    public override string Name => "«Помощь Интернета»";

    #endregion Basic Properties

    public Google(Game game)
        : base(game)
    {
        TimerStopped += GoogleTimerStopped;
    }

    #region Properties

    protected override Audio AudioUse => Audio.TWMUse;
    protected override Audio AudioStart => Audio.TWMCountdown;
    protected override Audio AudioStop => Audio.TWMStop;

    #endregion Properties

    #region Methods

    private void GoogleTimerStopped(object sender, TimerLifelineStoppedEventArgs e) => AudioManager.Play(AudioStop);

    public override void Activate()
    {
        base.Activate();
        AudioManager.Play(Audio.TWMStart);
    }

    #endregion Methods
}
