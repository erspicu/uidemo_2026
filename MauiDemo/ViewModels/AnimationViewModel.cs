using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MauiDemo.ViewModels;

public class AnimationViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private void Set<T>(ref T field, T value, [CallerMemberName] string? n = null)
    { if (!Equals(field, value)) { field = value; PropertyChanged?.Invoke(this, new(n)); } }

    private double _progressValue = 0;
    private bool   _isRunning     = false;
    private int    _counterValue  = 0;

    public double ProgressValue { get => _progressValue; set => Set(ref _progressValue, value); }
    public bool   IsRunning     { get => _isRunning;     set { Set(ref _isRunning, value); PropertyChanged?.Invoke(this, new(nameof(IsNotRunning))); } }
    public bool   IsNotRunning  => !_isRunning;
    public int    CounterValue  { get => _counterValue;  set => Set(ref _counterValue, value); }

    private CancellationTokenSource? _cts;

    public ICommand RunCommand => new Command(async () =>
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
            if (!_cts.Token.IsCancellationRequested) { ProgressValue = 100; CounterValue = 100; }
        }
        catch (OperationCanceledException) { }
        finally { IsRunning = false; }
    });

    public ICommand StopCommand  => new Command(() => _cts?.Cancel());
    public ICommand ResetCommand => new Command(() =>
    {
        _cts?.Cancel();
        ProgressValue = 0;
        CounterValue  = 0;
        IsRunning     = false;
    });
}
