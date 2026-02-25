using System;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaDemo.ViewModels;

public partial class AnimationDemoViewModel : ViewModelBase
{
    [ObservableProperty] private double _progressValue = 0;
    [ObservableProperty] private bool   _isRunning     = false;
    [ObservableProperty] private int    _counterValue  = 0;

    private CancellationTokenSource? _cts;

    [RelayCommand]
    private async Task RunProgress()
    {
        if (IsRunning) return;
        _cts          = new CancellationTokenSource();
        IsRunning     = true;
        ProgressValue = 0;
        CounterValue  = 0;

        try
        {
            while (ProgressValue < 100 && !_cts.Token.IsCancellationRequested)
            {
                ProgressValue += 1;
                CounterValue   = (int)ProgressValue;
                await Task.Delay(40, _cts.Token);
            }
            if (!_cts.Token.IsCancellationRequested)
            {
                ProgressValue = 100;
                CounterValue  = 100;
            }
        }
        catch (OperationCanceledException) { }
        finally { IsRunning = false; }
    }

    [RelayCommand] private void StopProgress()  => _cts?.Cancel();

    [RelayCommand]
    private void ResetProgress()
    {
        _cts?.Cancel();
        ProgressValue = 0;
        CounterValue  = 0;
        IsRunning     = false;
    }
}
