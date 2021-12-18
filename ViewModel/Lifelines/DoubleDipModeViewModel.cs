using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using WwtbamOld.Model;

namespace WwtbamOld.ViewModel;

public sealed class DoubleDipModeViewModel : ViewModelBase
{
    #region Fields

    private readonly DoubleDipViewModel _viewModel;

    #endregion Fields

    public DoubleDipModeViewModel(DoubleDipViewModel viewModel, DoubleDipMode mode)
    {
        _viewModel = viewModel;
        Mode = mode;
        Description = GetDescription(Mode);

        this.WhenAnyValue(x => x._viewModel.Mode).Select(mode => mode == Mode).BindTo(this, x => x.IsChecked);
        this.WhenAnyValue(x => x.IsChecked).Where(x => x).Select(x => Mode).BindTo(_viewModel, x => x.Mode);

        SetModeCommand = ReactiveCommand.Create(() => { _viewModel.SetModeCommand.Execute(Mode); });
    }

    public DoubleDipMode Mode { get; }
    public string Description { get; }
    [Reactive] public bool IsChecked { get; set; }
    public ReactiveCommand<Unit, Unit> SetModeCommand { get; }

    private string GetDescription(object obj)
    {
        MemberInfo[] member = obj.GetType().GetMember(obj.ToString());

        if (member is not null && member.Length != 0)
        {
            object[] customAttributes = member[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (customAttributes is not null && customAttributes.Length != 0)
                return ((DescriptionAttribute)customAttributes[0]).Description;
        }

        return obj.ToString();
    }
}
