using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace CircuitSim;

/// <summary>
/// A page that allows a circuit board to be viewed and edited.
/// </summary>
public sealed partial class EditorPage : Page
{
    public CircuitBoard CircuitBoard = new();

    public EditorPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is CircuitBoard board)
        {
            CircuitBoard = board;
        }
    }
}
