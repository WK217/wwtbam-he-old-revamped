﻿using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace WwtbamOld.Model;

public sealed class Lifelines : ReadOnlyReactiveCollection<Lifeline>
{
    #region Fields

    private readonly ObservableAsPropertyHelper<Lifeline> _activated;

    #endregion Fields

    public Lifelines() : base()
    {
        _activated = _collection.ToObservableChangeSet()
                                .WhenPropertyChanged(lifeline => lifeline.IsExecuting)
                                .Select(x => x.Value ? x.Sender : (_activated.Value?.IsExecuting == true ? _activated.Value : null))
                                .ToProperty(this, nameof(Activated));
    }

    #region Properties

    public Lifeline Activated => _activated.Value;
    [Reactive] public Lifeline Selected { get; set; }

    #endregion Properties

    #region Methods

    public void Initialize(IEnumerable<Lifeline> collection)
    {
        _collection.Clear();
        _collection.AddRange(collection);
    }

    public static IEnumerable<Type> GetAllLifelineTypes()
    {
        yield return typeof(FiftyFifty);
        yield return typeof(PhoneAFriend);
        yield return typeof(AskTheAudience);
        yield return typeof(DoubleDip);
        yield return typeof(Google);
        yield return typeof(AskTheHost);
    }

    public void Add(Lifeline lifeline) => _collection.Add(lifeline);

    public void Remove(Lifeline lifeline) => _collection.Remove(lifeline);

    public void Remove(Type type) => Remove(_collection.LastOrDefault(lifeline => lifeline.GetType().Equals(type)));

    public bool IsDoubleDipActivated(out DoubleDip lifeline)
    {
        lifeline = Collection.FirstOrDefault(x => x is DoubleDip && (x as DoubleDip).Mode > DoubleDipMode.Deactivated) as DoubleDip;
        return lifeline is not null;
    }

    #endregion Methods
}
