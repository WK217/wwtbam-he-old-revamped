using ReactiveUI;
using System;
using System.ComponentModel;
using System.Reactive;
using WwtbamOld.Model;

namespace WwtbamOld.ViewModel;

public sealed class LifelineTypeViewModel : ReactiveObject
{
    #region Fields

    private readonly Game _game;
    private readonly Type _type;

    #endregion Fields

    public LifelineTypeViewModel(Game game, Type type)
    {
        _game = game;
        _type = type;

        Description = GetDescription(_type);

        AddCommand = ReactiveCommand.Create(() =>
        {
            Lifeline newLifeline = (Lifeline)Activator.CreateInstance(_type, _game);
            _game.Lifelines.Add(newLifeline);
        });

        RemoveCommand = ReactiveCommand.Create(() =>
        {
            _game.Lifelines.Remove(_type);
        });
    }

    #region Properties

    public string Description { get; }

    #endregion Properties

    #region Methods

    private string GetDescription(Type type)
    {
        var descriptions = (DescriptionAttribute[])type.GetCustomAttributes(typeof(DescriptionAttribute), false);
        return descriptions.Length != 0 ? descriptions[0].Description : string.Empty;
    }

    #endregion Methods

    #region Commands

    public ReactiveCommand<Unit, Unit> AddCommand { get; }
    public ReactiveCommand<Unit, Unit> RemoveCommand { get; }

    #endregion Commands
}
