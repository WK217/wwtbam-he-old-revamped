﻿using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;

namespace WwtbamOld.Model
{
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

            /*this.WhenAnyValue(vm => vm.Selected, vm => vm.Selected.IsExecuting)
                .Buffer(2, 1)
                .Where(b => b.Count == 2)
                .Select(b => (Previous: b[0], Current: b[1]))
                .Subscribe(x =>
                {
                    if (x.Previous.Item1 == x.Current.Item1 && x.Previous.Item2 && !x.Current.Item2)
                        Selected = null;
                });*/
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

        public IEnumerable<Type> GetAllLifelineTypes()
        {
            Type lifelineType = typeof(Lifeline);
            return Assembly.GetAssembly(lifelineType)
                           .GetTypes()
                           .Where(type => type.IsSubclassOf(lifelineType));
        }

        public void Add(Lifeline lifeline) => _collection.Add(lifeline);

        public bool IsDoubleDipActivated(out DoubleDip lifeline)
        {
            lifeline = Collection.FirstOrDefault(x => x is DoubleDip && (x as DoubleDip).Mode > DoubleDipMode.Deactivated) as DoubleDip;
            return lifeline != null;
        }

        #endregion Methods
    }
}