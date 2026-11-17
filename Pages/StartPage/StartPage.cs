using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;

namespace CircuitSim;

public sealed partial class StartPage : Page
{
    public StartPage()
    {
        InitializeComponent();
    }

    private void OnCreateNewBoardPressed(object sender, RoutedEventArgs e)
    {
        var board = new CircuitBoard();
        var transition = new DrillInNavigationTransitionInfo();

        App.Navigator.Navigate(typeof(EditorPage), board, transition);
    }
}