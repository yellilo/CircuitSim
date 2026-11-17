using CircuitSim.Numerics;
using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml.Controls;
using System;

namespace CircuitSim;

public sealed partial class CircuitBoardCursor : UserControl
{
    private CircuitBoard Board => this.FindAscendant<CircuitBoard>() ?? throw new Exception("Failed to get circuit board.");

    private Vector2Int _position = Vector2Int.Zero;
    public Vector2Int Position
    {
        get => _position;
        set
        {
            _position = value;
            OnPositionChange();
        }
    }

    public CircuitBoardCursor()
    {
        InitializeComponent();
    }

    public void OnPositionChange()
    {
        var position = Board.BoardToScreenPosition(Position);
        var canvasPosition = position - ActualSize / 2;

        Canvas.SetLeft(this, canvasPosition.X);
        Canvas.SetTop(this, canvasPosition.Y);
    }
}
