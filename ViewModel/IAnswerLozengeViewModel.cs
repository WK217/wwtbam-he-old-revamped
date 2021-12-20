using ReactiveUI;
using WwtbamOld.Model;

namespace WwtbamOld.ViewModel;

public interface IAnswerLozengeViewModel : IAnswer, IReactiveObject
{
    public bool IsShown { get; }
    public bool IsLocked { get; }
}