using WwtbamOld.Model;

namespace WwtbamOld.ViewModel;

public sealed class PhoneAFriendViewModel : TimerLifelineViewModel
{
    public PhoneAFriendViewModel(LifelinesViewModel lifelinesViewModel, PhoneAFriend lifeline)
        : base(lifelinesViewModel, lifeline)
    {
    }
}
