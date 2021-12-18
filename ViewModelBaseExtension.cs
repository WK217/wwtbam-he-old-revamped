using ReactiveUI;
using Splat;

namespace WwtbamOld;

public static class ViewModelBaseExtension
{
    public static ReactiveWindow<T> GetView<T>(this T viewModel)
        where T : ViewModelBase
    {
        var view = Locator.Current.GetService<IViewFor<T>>();
        view.ViewModel = viewModel;
        return view as ReactiveWindow<T>;
    }
}
