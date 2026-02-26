using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace UwpDemo.Pages
{
    public sealed partial class AnimationPage : Page
    {
        private static readonly Random _rng = new Random();
        private readonly List<Storyboard> _storyboards = new List<Storyboard>();
        private Windows.UI.Xaml.DispatcherTimer? _counterTimer;
        private Windows.UI.Xaml.DispatcherTimer? _orbitTimer;
        private int _counterValue = 0;
        private double _orbitAngle1 = 0, _orbitAngle2 = 120, _orbitAngle3 = 240;

        public AnimationPage()
        {
            InitializeComponent();
        }

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
            // Wave bars – staggered ScaleY animations
            var scales = new[] { Scale1, Scale2, Scale3, Scale4, Scale5, Scale6, Scale7 };
            var delays = new[] { 0.0, 0.12, 0.24, 0.08, 0.20, 0.16, 0.04 };
            var peaks  = new[] { 3.5, 2.8, 4.0, 2.2, 3.2, 2.6, 3.8 };

            for (int i = 0; i < scales.Length; i++)
            {
                var sb   = new Storyboard();
                var anim = new DoubleAnimation
                {
                    From           = 1.0,
                    To             = peaks[i],
                    BeginTime      = TimeSpan.FromSeconds(delays[i]),
                    Duration       = new Duration(TimeSpan.FromSeconds(0.5 + i * 0.04)),
                    AutoReverse    = true,
                    RepeatBehavior = RepeatBehavior.Forever,
                    EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
                };
                Storyboard.SetTarget(anim, scales[i]);
                Storyboard.SetTargetProperty(anim, "ScaleY");
                sb.Children.Add(anim);
                sb.Begin();
                _storyboards.Add(sb);
            }

            // Pulse ring – opacity animations
            var ringTargets = new (Windows.UI.Xaml.UIElement el, double from, double to, double dur)[]
            {
                (PulseOuter, 0.1, 0.6, 1.2),
                (PulseMid,   0.3, 0.8, 0.9),
                (PulseCore,  0.7, 1.0, 0.7)
            };
            foreach (var (el, from, to, dur) in ringTargets)
            {
                var sb   = new Storyboard();
                var anim = new DoubleAnimation
                {
                    From           = from,
                    To             = to,
                    Duration       = new Duration(TimeSpan.FromSeconds(dur)),
                    AutoReverse    = true,
                    RepeatBehavior = RepeatBehavior.Forever,
                    EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
                };
                Storyboard.SetTarget(anim, el);
                Storyboard.SetTargetProperty(anim, "Opacity");
                sb.Children.Add(anim);
                sb.Begin();
                _storyboards.Add(sb);
            }

            // Counter timer
            _counterTimer = new Windows.UI.Xaml.DispatcherTimer { Interval = TimeSpan.FromMilliseconds(80) };
            _counterTimer.Tick += (_, _) =>
            {
                _counterValue = (_counterValue + _rng.Next(1, 8)) % 100;
                CounterText.Text  = _counterValue.ToString();
                CounterBar.Value  = _counterValue;
            };
            _counterTimer.Start();

            // Orbit timer
            _orbitTimer = new Windows.UI.Xaml.DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) };
            _orbitTimer.Tick += (_, _) =>
            {
                _orbitAngle1 += 2.0;
                _orbitAngle2 += 1.3;
                _orbitAngle3 += 0.9;
                UpdateOrbit(Dot1, _orbitAngle1, 92);
                UpdateOrbit(Dot2, _orbitAngle2, 72);
                UpdateOrbit(Dot3, _orbitAngle3, 52);
            };
            _orbitTimer.Start();
        }

        private static void UpdateOrbit(Windows.UI.Xaml.Shapes.Ellipse dot, double angle, double radius)
        {
            var rad = angle * Math.PI / 180.0;
            var cx  = 100 + radius * Math.Cos(rad) - dot.Width  / 2;
            var cy  = 100 + radius * Math.Sin(rad) - dot.Height / 2;
            Windows.UI.Xaml.Controls.Canvas.SetLeft(dot, cx);
            Windows.UI.Xaml.Controls.Canvas.SetTop(dot, cy);
        }

        private void StopAnimations()
        {
            foreach (var sb in _storyboards) sb.Stop();
            _storyboards.Clear();
            _counterTimer?.Stop();
            _counterTimer = null;
            _orbitTimer?.Stop();
            _orbitTimer = null;
        }
    }
}
