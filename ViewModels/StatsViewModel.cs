﻿using EstrellaAccesoriosWpf.Common;
using MaterialDesignThemes.Wpf;

namespace EstrellaAccesoriosWpf.ViewModels;

public class StatsViewModel(ISnackbarMessageQueue SnackbarMessageQueue) : ViewModel
{
    protected override Task LoadAsync()
    {
        return Task.CompletedTask;
    }

    protected override void Refresh()
    {
        throw new NotImplementedException();
    }

    protected override Task UnloadAsync()
    {
        return Task.CompletedTask;
    }
}
