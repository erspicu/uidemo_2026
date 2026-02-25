using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfDemo.Views;

public partial class AnimationPage : Page
{
    private CancellationTokenSource? _cts;
    private bool _isRunning;

    public AnimationPage()
    {
        InitializeComponent();
    }

    private async void OnRunClicked(object sender, RoutedEventArgs e)
    {
        if (_isRunning) return;
        _cts       = new CancellationTokenSource();
        _isRunning = true;
        RunBtn.IsEnabled  = false;
        StopBtn.IsEnabled = true;

        MainProgressBar.Value = 0;
        CounterLabel.Text = "0%";

        try
        {
            for (int i = 1; i <= 100; i++)
            {
                if (_cts.Token.IsCancellationRequested) break;
                MainProgressBar.Value = i;
                CounterLabel.Text = $"{i}%";
                await Task.Delay(40, _cts.Token);
            }
        }
        catch (OperationCanceledException) { }
        finally
        {
            _isRunning = false;
            RunBtn.IsEnabled  = true;
            StopBtn.IsEnabled = false;
        }
    }

    private void OnStopClicked(object sender, RoutedEventArgs e) => _cts?.Cancel();

    private void OnResetClicked(object sender, RoutedEventArgs e)
    {
        _cts?.Cancel();
        MainProgressBar.Value = 0;
        CounterLabel.Text = "0%";
        RunBtn.IsEnabled  = true;
        StopBtn.IsEnabled = false;
    }
}
