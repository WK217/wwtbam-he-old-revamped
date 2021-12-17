using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.ComponentModel;
using System.Reactive;
using System.Windows;
using System.Windows.Media;
using WwtbamOld.Model;

namespace WwtbamOld.ViewModel
{
    [Description("Подсказка")]
    public abstract class LifelineViewModel<T> : ViewModelBase, ILifelineViewModel
        where T : Lifeline
    {
        public string WindowTitle => string.Join(" :: ", ((App)Application.Current).AppName, _model.Name);

        #region Fields

        protected readonly LifelinesViewModel _lifelinesViewModel;
        protected readonly T _model;

        private readonly ObservableAsPropertyHelper<bool> _isUsable;
        private readonly ObservableAsPropertyHelper<bool> _isExecuting;

        #endregion Fields

        public LifelineViewModel(LifelinesViewModel lifelinesViewModel, T model)
        {
            _lifelinesViewModel = lifelinesViewModel;
            _model = model;

            _isUsable = this.WhenAnyValue(vm => vm._model.IsUsable)
                            .ToProperty(this, nameof(IsUsable));

            _isExecuting = this.WhenAnyValue(vm => vm._model.IsExecuting)
                               .ToProperty(this, nameof(IsExecuting));

            this.WhenAnyValue(vm => vm._model.IsEnabled).BindTo(this, x => x.IsEnabled);
            this.WhenAnyValue(vm => vm.IsEnabled).BindTo(this, x => x._model.IsEnabled);

            this.WhenAnyValue(vm => vm._model.IsSecret).BindTo(this, x => x.IsSecret);
            this.WhenAnyValue(vm => vm.IsSecret).BindTo(this, x => x._model.IsSecret);

            this.WhenAnyValue(vm => vm._model.GeneralImage).Subscribe(_ => this.RaisePropertyChanged(nameof(GeneralImage)));

            #region Commands

            PingCommand = ReactiveCommand.Create(() => _model.Ping());
            ActivateCommand = ReactiveCommand.Create(() => _model.Activate(), _model.CanActivate);

            #endregion Commands
        }

        #region Properties

        public Lifeline Model => _model;

        public bool IsUsable => _isUsable != null && _isUsable.Value;
        public bool IsExecuting => _isExecuting != null && _isExecuting.Value;

        public string Code => _model.Code;
        public string Name => _model.Name;

        [Reactive] public bool IsEnabled { get; set; }
        [Reactive] public bool IsSecret { get; set; }

        public ImageSource GeneralImage => _model.GeneralImage;

        #endregion Properties

        #region Commands

        public ReactiveCommand<Unit, Unit> PingCommand { get; }
        public ReactiveCommand<Unit, Unit> ActivateCommand { get; }

        #endregion Commands
    }
}