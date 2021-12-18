using WwtbamOld.Model;

namespace WwtbamOld.ViewModel;

public sealed class GoogleViewModel : TimerLifelineViewModel
{
    public GoogleViewModel(LifelinesViewModel lifelinesViewModel, Google lifeline)
        : base(lifelinesViewModel, lifeline)
    {
    }
}
