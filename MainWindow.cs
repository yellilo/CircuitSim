using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace CircuitSim;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ExtendsContentIntoTitleBar = true;
        SetMinimumSize(1000, 500);

        Frame.Navigate(typeof(StartPage));
    }

    private void SetMinimumSize(int width, int height)
    {
        if (AppWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.PreferredMinimumWidth = width;
            presenter.PreferredMinimumHeight = height;
        }
    }
}
