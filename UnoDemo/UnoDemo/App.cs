using System;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace UnoDemo;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
	public App()
	{
	}

	internal static Window MainWindow { get; private set; }

	protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
	{
#if NET6_0_OR_GREATER && WINDOWS && !HAS_UNO
		MainWindow = new Window();
		MainWindow.Activate();
#else
		MainWindow = Microsoft.UI.Xaml.Window.Current;
#endif

		if (MainWindow.Content is not Frame rootFrame)
		{
			rootFrame = new Frame();
			rootFrame.NavigationFailed += OnNavigationFailed;
			MainWindow.Content = rootFrame;
		}

#if !(NET6_0_OR_GREATER && WINDOWS)
		if (args.UWPLaunchActivatedEventArgs.PrelaunchActivated == false)
#endif
		{
			if (rootFrame.Content == null)
			{
				rootFrame.Navigate(typeof(MainPage), args.Arguments);
			}
			MainWindow.Activate();
		}
	}

	void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
	{
		throw new InvalidOperationException($"Failed to load {e.SourcePageType.FullName}: {e.Exception}");
	}
}
