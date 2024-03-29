﻿using ReactiveUI;
using System.Collections.Generic;
using System.Reactive;
using WwtbamOld.Model;

namespace WwtbamOld.Interactions;

public static class DialogWindowInteractions
{
    public static Interaction<Unit, IEnumerable<Quiz>> ShowOpenQuizbaseDialog { get; } = new Interaction<Unit, IEnumerable<Quiz>>();
}