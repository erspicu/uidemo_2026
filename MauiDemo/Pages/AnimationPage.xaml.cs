namespace MauiDemo.Pages;

public partial class AnimationPage : ContentPage
{
    private CancellationTokenSource? _progressCts;
    private CancellationTokenSource? _animCts;
    private bool _isRunning;

    public AnimationPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // 取消上次殘留的動畫（以防萬一），再重新啟動
        _animCts?.Cancel();
        _animCts = new CancellationTokenSource();
        StartWaveAnimation(_animCts.Token);
        StartPulseAnimation(_animCts.Token);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // 離開頁面時停止所有動畫迴圈
        _animCts?.Cancel();
        _progressCts?.Cancel();
    }

    private void StartWaveAnimation(CancellationToken ct)
    {
        var bars   = new[] { W1, W2, W3, W4, W5, W6, W7 };
        var delays = new[] { 0, 100, 200, 300, 400, 500, 600 };
        for (int i = 0; i < bars.Length; i++)
        {
            var bar   = bars[i];
            var delay = delays[i];
            _ = PulseBarAsync(bar, delay, ct);
        }
    }

    private static async Task PulseBarAsync(BoxView bar, int delayMs, CancellationToken ct)
    {
        try
        {
            await Task.Delay(delayMs, ct);
            while (!ct.IsCancellationRequested)
            {
                await bar.FadeToAsync(0.15, 450, Easing.SinInOut);
                await bar.FadeToAsync(1.0,  450, Easing.SinInOut);
            }
        }
        catch (OperationCanceledException) { }
        finally { bar.Opacity = 1.0; }
    }

    private void StartPulseAnimation(CancellationToken ct)
    {
        _ = PulseRingLoopAsync(ct);
        _ = PulseCoreLoopAsync(ct);
    }

    private async Task PulseRingLoopAsync(CancellationToken ct)
    {
        try
        {
            while (!ct.IsCancellationRequested)
            {
                await PulseRing.FadeToAsync(0.05, 750, Easing.SinInOut);
                await PulseRing.FadeToAsync(0.4,  750, Easing.SinInOut);
            }
        }
        catch (OperationCanceledException) { }
        finally { PulseRing.Opacity = 0.3; }
    }

    private async Task PulseCoreLoopAsync(CancellationToken ct)
    {
        try
        {
            await Task.Delay(375, ct);
            while (!ct.IsCancellationRequested)
            {
                await PulseCore.ScaleToAsync(0.85, 750, Easing.SinInOut);
                await PulseCore.ScaleToAsync(1.0,  750, Easing.SinInOut);
            }
        }
        catch (OperationCanceledException) { }
        finally { PulseCore.Scale = 1.0; }
    }

    private async void OnRunClicked(object? s, EventArgs e)
    {
        if (_isRunning) return;
        _progressCts  = new CancellationTokenSource();
        _isRunning    = true;
        RunBtn.IsEnabled  = false;
        StopBtn.IsEnabled = true;

        MainProgressBar.Progress = 0;
        CounterLabel.Text = "0%";

        try
        {
            for (int i = 1; i <= 100; i++)
            {
                if (_progressCts.Token.IsCancellationRequested) break;
                await MainProgressBar.ProgressTo(i / 100.0, 40, Easing.Linear);
                CounterLabel.Text = $"{i}%";
                await Task.Delay(10, _progressCts.Token);
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

    private void OnStopClicked(object? s, EventArgs e)  => _progressCts?.Cancel();

    private void OnResetClicked(object? s, EventArgs e)
    {
        _progressCts?.Cancel();
        MainProgressBar.Progress = 0;
        CounterLabel.Text = "0%";
        RunBtn.IsEnabled  = true;
        StopBtn.IsEnabled = false;
    }
}

