using ReactiveUI;
using System.Reactive;
using WwtbamOld.Model;

namespace WwtbamOld.ViewModel
{
    public interface ILifelineViewModel
    {
        public Lifeline Model { get; }

        public string Code { get; }
        public string Name { get; }

        public bool IsExecuting { get; }
        public bool IsEnabled { get; set; }
        public bool IsSecret { get; set; }

        public ReactiveCommand<Unit, Unit> PingCommand { get; }
        public ReactiveCommand<Unit, Unit> ActivateCommand { get; }
    }
}