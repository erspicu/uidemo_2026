namespace WinUI3Demo
{
    public partial class App : Application
    {
        private Window? _window;

        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            _window = new MainWindow();
            _window.Activate();
        }
    }
}
