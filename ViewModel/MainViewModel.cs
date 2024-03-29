﻿using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using WwtbamOld.Model;

namespace WwtbamOld.ViewModel;

public sealed class MainViewModel : ReactiveObject
{
    #region Fields

    private readonly Game _game;

    #endregion Fields

    public MainViewModel(Game game)
    {
        _game = game;

        Lozenge = new LozengeViewModel(_game.Lozenge);
        Host = new HostViewModel(_game, this);
        Screen = new ScreenViewModel(_game, this);

        this.WhenAnyValue(vm => vm._game.CurrentLevel)
            .Do(lvl =>
            {
                this.RaisePropertyChanged(nameof(CurrentLevel));
                Host.RaisePropertyChanged(nameof(Host.CurrentLevel));
            })
            .Subscribe();

        CurrentLevel = Levels[0];
    }

    #region Properties

    public ReadOnlyObservableCollection<Level> Levels => _game.Levels.Collection;

    public Level CurrentLevel
    {
        get => _game.CurrentLevel;
        set
        {
            if (value != _game.CurrentLevel)
            {
                _game.CurrentLevel = value;
                this.RaisePropertyChanged(nameof(CurrentLevel));
            }
        }
    }

    public ReadOnlyObservableCollection<Lifeline> Lifelines => _game.Lifelines.Collection;

    #endregion Properties

    #region View Models

    public HostViewModel Host { get; }
    public ScreenViewModel Screen { get; }
    public LozengeViewModel Lozenge { get; }

    #endregion View Models
}