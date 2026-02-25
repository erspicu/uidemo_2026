using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;

namespace UnoDemo.Pages;

public sealed partial class AnimationPage : Page
{
    private readonly List<Storyboard> _storyboards = new();
    private DispatcherTimer? _counterTimer;
    private DispatcherTimer? _orbitTimer;
    private int _counter = 0;
    private double _a1 = 0, _a2 = 120, _a3 = 240;

    public AnimationPage() => InitializeComponent();

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        StartAnimations();
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);
        StopAnimations();
    }

    private void StartAnimations()
    {
        // Wave bars
        var scales = new[] { Scale1, Scale2, Scale3, Scale4, Scale5, Scale6, Scale7 };
        var peaks  = new[] { 3.5, 2.8, 4.0, 2.2, 3.2, 2.6, 3.8 };
        var delays = new[] { 0.0, 0.12, 0.24, 0.08, 0.20, 0.16, 0.04 };
        for (int i = 0; i < scales.Length; i++)
        {
            var sb = new Storyboard();
            var anim = new DoubleAnimation
            {
                From = 1.0, To = peaks[i],
                BeginTime    = TimeSpan.FromSeconds(delays[i]),
                Duration     = new Duration(TimeSpan.FromSeconds(0.5 + i * 0.04)),
                AutoReverse  = true,
                RepeatBehavior = RepeatBehavior.Forever,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            Storyboard.SetTarget(anim, scales[i]);
            Storyboard.SetTargetProperty(anim, "ScaleY");
            sb.Children.Add(anim);
            sb.Begin();
            _storyboards.Add(sb);
        }

        // Pulse ring opacity
        var rings  = new Microsoft.UI.Xaml.UIElement[] { PulseOuter, PulseMid, PulseCore };
        var froms  = new[] { 0.1, 0.3, 0.7 };
        var tos    = new[] { 0.6, 0.8, 1.0 };
        var durs   = new[] { 1.2, 0.9, 0.7 };
        for (int i = 0; i < rings.Length; i++)
        {
            var sb = new Storyboard();
            var anim = new DoubleAnimation
            {
                From = froms[i], To = tos[i],
                Duration = new Duration(TimeSpan.FromSeconds(durs[i])),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            Storyboard.SetTarget(anim, rings[i]);
            Storyboard.SetTargetProperty(anim, "Opacity");
            sb.Children.Add(anim);
            sb.Begin();
            _storyboards.Add(sb);
        }

        _counterTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(80) };
        _counterTimer.Tick += (_, _) =>
        {
            _counter = (_counter + Random.Shared.Next(1, 8)) % 100;
            CounterText.Text = _counter.ToString();
            CounterBar.Value = _counter;
        };
        _counterTimer.Start();

        _orbitTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) };
        _orbitTimer.Tick += (_, _) =>
        {
            _a1 += 2.0; _a2 += 1.3; _a3 += 0.9;
            Orbit(Dot1, _a1, 92); Orbit(Dot2, _a2, 72); Orbit(Dot3, _a3, 52);
        };
        _orbitTimer.Start();
    }

    private static void Orbit(Microsoft.UI.Xaml.Shapes.Ellipse dot, double angle, double r)
    {
        var rad = angle * Math.PI / 180.0;
        Canvas.SetLeft(dot, 100 + r * Math.Cos(rad) - dot.Width / 2);
        Canvas.SetTop(dot,  100 + r * Math.Sin(rad) - dot.Height / 2);
    }

    private void StopAnimations()
    {
        foreach (var sb in _storyboards) sb.Stop();
        _storyboards.Clear();
        _counterTimer?.Stop(); _counterTimer = null;
        _orbitTimer?.Stop();   _orbitTimer = null;
    }
}
