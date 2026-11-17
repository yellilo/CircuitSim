using CircuitSim.Circuitry;
using CircuitSim.Numerics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;

namespace CircuitSim;

public sealed partial class CircuitBoard : UserControl
{
    private bool IsLoading = true;

    public CircuitBoardOptions Options = new();

    public Dictionary<string, Component> Components = [];
    public Dictionary<string, Junction> Junctions = [];

    public MNA MNA = new();

    public CircuitBoard()
    {
        InitializeComponent();

        ProtectedCursor = Cursors.Cross;

        PointerEntered += OnPointerEnter;
        PointerExited += OnPointerExited;

        PointerMoved += OnPointerMoved;
        PointerPressed += OnPointerUpdate;
        PointerReleased += OnPointerUpdate;
        PointerWheelChanged += OnPointerWheelChanged;

        WireCanvas.Draw += OnDrawWires;
        SizeChanged += (_, __) => UpdateDisplay();
        ActualThemeChanged += (_, __) => WireCanvas.Invalidate();

        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        var n0 = AddJunction(new Junction() { Position = new Vector2Int(1, 0) });
        var n1 = AddJunction(new Junction() { Position = new Vector2Int(1, 2) });
        var n2 = AddJunction(new Junction() { Position = new Vector2Int(-1, 2) });
        var n3 = AddJunction(new Junction() { Position = new Vector2Int(-1, 0) });

        var c = AddComponent(new Cell() { EMF = 10, PositiveTerminalId = n3.Id, NegativeTerminalId = n0.Id });
        var w0 = AddComponent(new Wire() { PositiveTerminalId = n2.Id, NegativeTerminalId = n3.Id, Vertices = [new Vector2Int(-2, 2), new Vector2Int(-2, 0)] });
        var r = AddComponent(new FixedResistor() { FixedResistance = 2, PositiveTerminalId = n1.Id, NegativeTerminalId = n2.Id, Position = new Vector2Int(0, 2) });
        var w1 = AddComponent(new Wire() { PositiveTerminalId = n0.Id, NegativeTerminalId = n1.Id, Vertices = [new Vector2Int(2, 0), new Vector2Int(2, 2)] });

        IsLoading = false;
    }

    private void UpdateDisplay()
    {
        if (IsLoading) return;

        Grid.Update();
        Cursor.OnPositionChange();

        UpdateRigidComponentCanvasPositions();
        WireCanvas.Invalidate();

        new MNA().Solve(Junctions.Values.ToArray(), Components.Values.ToArray());
    }

    // --------------------
    //    Adding Elements
    // --------------------

    public T AddComponent<T>(T component) where T : Component
    {
        Components.Add(component.Id, component);
        ElementCanvas.Children.Add(component);

        if (component is RigidComponent rigidComponent)
        {
            UpdateRigidComponentCanvasPosition(rigidComponent);
        }

        if (component is Wire)
        {
            WireCanvas.Invalidate();
        }

        return component;
    }

    public Junction AddJunction(Junction junction)
    {
        Junctions.Add(junction.Id, junction);
        ElementCanvas.Children.Add(junction);

        return junction;
    }

    public Junction AddJunctionAtPosition(Vector2Int position)
    {
        return AddJunction(new Junction() { Position = position });
    }

    // --------------------
    //        Input
    // --------------------

    private void OnPointerEnter(object sender, PointerRoutedEventArgs e)
    {
        Cursor.Visibility = Visibility.Visible;
    }

    private void OnPointerExited(object sender, PointerRoutedEventArgs e)
    {
        Cursor.Visibility = Visibility.Collapsed;
    }

    public void Zoom(float factor)
    {
        int newScale = (int)(Options.Scale * factor);
        Options.Scale = Math.Clamp(newScale, CircuitBoardOptions.MinScale, CircuitBoardOptions.MaxScale);
        UpdateDisplay();
    }

    private void OnPointerWheelChanged(object sender, PointerRoutedEventArgs e)
    {
        var point = e.GetCurrentPoint(this);
        int delta = point.Properties.MouseWheelDelta;

        Zoom(delta > 0 ? 1.3f : 0.7f);
    }

    private void OnPointerUpdate(object sender, PointerRoutedEventArgs e)
    {
        var point = e.GetCurrentPoint(this);
        var position = point.Position.ToVector2();
        var updateKind = point.Properties.PointerUpdateKind;
        
        switch (updateKind)
        {
            case PointerUpdateKind.RightButtonPressed:
                IsDragging = true;
                DragOrigin = position;
                DragOffset = Options.Offset;
                break;
            case PointerUpdateKind.RightButtonReleased:
                IsDragging = false;
                break;
        }
    }

    private void OnPointerMoved(object sender, PointerRoutedEventArgs e)
    {
        var point = e.GetCurrentPoint(this);
        var position = point.Position.ToVector2();

        Cursor.Position = ScreenToBoardPosition(position);
        
        if (IsDragging)
        {
            var dragVector = (position - DragOrigin) / Grid.CellSize;
            Options.Offset = DragOffset + Vector2Int.Round(dragVector);
            UpdateDisplay();
        }
    }


    // --------------------
    //       Dragging
    // --------------------

    // Whether the user is currently dragging the circuit board
    // Changes the cursor when the user is dragging
    private bool _isDragging = false;
    public bool IsDragging
    {
        get => _isDragging;
        set
        {
            _isDragging = value;

            if (IsDragging)
            {
                ProtectedCursor = Cursors.Drag;
            }
            else
            {
                ProtectedCursor = Cursors.Cross;
            }
        }
    }

    // The screen position at which the user started dragging the mouse
    public Vector2 DragOrigin = Vector2.Zero;

    // The offset of the board when the user started dragging the mouse
    public Vector2Int DragOffset = Vector2Int.Zero;

    // --------------------
    //  Placing Components
    // --------------------

    public string SelectedComponentTypeId = "Ammeter";
    public bool IsPlacingComponent = false;
    public RigidComponent? ComponentBeingPlaced;

    private void StartPlacingComponent()
    {
        IsPlacingComponent = true;
        ComponentBeingPlaced = ComponentTypeRegistry.CreateComponent<RigidComponent>(SelectedComponentTypeId);
        ComponentBeingPlaced.Position = Cursor.Position;
    }

    private void PlaceComponent()
    {
        if (!IsPlacingComponent || ComponentBeingPlaced == null) return;

        AddComponent(ComponentBeingPlaced);
        IsPlacingComponent = false;
    }

    // --------------------
    //     Positioning
    // --------------------

    public Vector2 CenterPosition => (Vector2Int)ActualSize / 2;
    public Vector2 OriginPosition => CenterPosition + Options.Offset * Grid.CellSize;

    /// <summary>
    /// Converts a local screen position into a position on the circuit board.
    /// </summary>
    public Vector2Int ScreenToBoardPosition(Vector2 localPosition)
    {
        Vector2 originRelativePosition = localPosition - OriginPosition;
        return Vector2Int.Round(originRelativePosition / Grid.CellSize);
    }

    /// <summary>
    /// Converts a board position into a screen position local to this circuit board node.
    /// </summary>
    public Vector2 BoardToScreenPosition(Vector2Int boardPosition)
    {
        return OriginPosition + boardPosition * Grid.CellSize;
    }

    // --------------------
    //       Rendering
    // --------------------

    public void UpdateRigidComponentCanvasPositions()
    {
        foreach (var component in ElementCanvas.Children.OfType<RigidComponent>())
        {
            UpdateRigidComponentCanvasPosition(component);
        }
    }

    private void UpdateRigidComponentCanvasPosition(RigidComponent component)
    {
        component.CircuitSymbol.SymbolScale = Options.Scale;

        var canvasPosition = OriginPosition + component.Position * Grid.CellSize - Grid.CellSize;
        Canvas.SetLeft(component, canvasPosition.X);
        Canvas.SetTop(component, canvasPosition.Y);
    }

    private void OnDrawWires(CanvasControl sender, CanvasDrawEventArgs args)
    {
        var ds = args.DrawingSession;
        ds.Antialiasing = CanvasAntialiasing.Aliased;

        foreach (Wire wire in Components.Values.OfType<Wire>())
        {
            DrawWire(ds, wire);
        }
    }

    private void DrawWire(CanvasDrawingSession ds, Wire wire)
    {
        var color = wire.IsPreview ? CircuitSymbol.PreviewColor : CircuitSymbol.DefaultColor;
        Vector2Int[] vertices = [wire.PositiveTerminal.Position, .. wire.Vertices, wire.NegativeTerminal.Position];

        for (var i = 0; i < vertices.Length - 1; i++)
        {
            var vertex = vertices[i];
            var position = BoardToScreenPosition(vertex);

            // Draw a square at each vertex to cover the point where line segments meet
            DrawWireVertex(ds, position, color);

            var nextVertex = vertices[i + 1];
            var nextPosition = BoardToScreenPosition(nextVertex);

            ds.DrawLine(position, nextPosition, color, Options.Scale);
        }
    }

    private void DrawWireVertex(CanvasDrawingSession ds, Vector2 position, Color color)
    {
        var size = Options.Scale * Vector2.One;
        var pos = position - size / 2;

        var square = new Rect(pos.X, pos.Y, size.X, size.Y);

        ds.FillRectangle(square, color);
    }
}