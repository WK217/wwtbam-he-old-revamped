using ReactiveUI;
using WwtbamOld.Model;

namespace WwtbamOld.ViewModel;

public interface IAnswerLozengeViewModel : IReactiveObject
{
    public AnswerID ID { get; }
    public bool IsShown { get; }
    public string Text { get; }
    public bool IsLocked { get; }
}
