using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml.Controls;
using System;

namespace CircuitSim.Circuitry;

public abstract partial class CircuitElement : UserControl
{
    public string Id = Guid.NewGuid().ToString();
    public CircuitBoard Board => this.FindAscendant<CircuitBoard>() ?? throw new Exception("Circuit Element is not on a circuit board.");
}
