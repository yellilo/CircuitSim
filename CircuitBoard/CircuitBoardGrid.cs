using CircuitSim.Numerics;
using CommunityToolkit.WinUI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.UI;

namespace CircuitSim;

public sealed partial class CircuitBoardGrid : UserControl
{
    public CircuitBoard Board => this.FindAscendant<CircuitBoard>() ?? throw new Exception("Failed to get circuit board.");
    public static Color LineColor => (Color)Application.Current.Resources["DividerStrokeColorDefault"];

    // Each grid cell is 10 wires wide/tall
    public Vector2Int CellSize => Vector2Int.One * Board.Options.Scale * 10;

    public CircuitBoardGrid()
    {
        InitializeComponent();

        Canvas.Draw += OnDraw;
        ActualThemeChanged += (_, __) => Update();
    }

    public void Update()
    {
        Canvas.Invalidate();
    }

    private void OnDraw(CanvasControl sender, CanvasDrawEventArgs args)
    {
        var ds = args.DrawingSession;
        ds.Antialiasing = CanvasAntialiasing.Aliased;

        var origin = Board.OriginPosition;
        var size = Board.ActualSize;

        // Vertical grid lines
        for (float x = origin.X; x <= size.X; x += CellSize.X)
        {
            ds.DrawLine(x, 0, x, size.Y, LineColor);
        }

        for (float x = origin.X; x >= 0; x -= CellSize.X)
        {
            ds.DrawLine(x, 0, x, size.Y, LineColor);
        }

        // Horizontal grid lines
        for (float y = origin.Y; y <= size.Y; y += CellSize.Y)
        {
            ds.DrawLine(0, y, size.X, y, LineColor);
        }

        for (float y = origin.Y; y >= 0; y -= CellSize.Y)
        {
            ds.DrawLine(0, y, size.X, y, LineColor);
        }
    }
}
