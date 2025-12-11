using Godot;
using System;

namespace CircuitSim.CircuitEditor;

[Tool]
[GlobalClass]
public partial class CircuitBoard : Control
{
  [Export]
  public required CircuitBoardCursor Cursor;
  [Export]
  public required CircuitBoardGrid Grid;

  public Circuitry.CircuitBoard Circuit = CircuitBoardManager.CurrentBoard!;

  public event Action? Dragged;
  public event Action? Zoomed;

  public bool IsHovered = false;

  public override void _Ready()
  {
    MouseEntered += () => IsHovered = true;
    MouseExited += () => IsHovered = false;

    SetDefaultCursorShape(CursorShape.Cross);
  }


  // --------------------
  //        Input
  // --------------------

  public override void _Input(InputEvent e)
  {

    if (e is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.ButtonIndex == MouseButton.Middle)
    {
      IsDragging = mouseButtonEvent.IsPressed();
    }
    else if (IsDragging && IsHovered && e is InputEventMouseMotion)
    {
      BoardOffset = DragOffset + (Vector2I)((Cursor.Position - DragOrigin) / BoardScale).Round();
    }

    if (!e.IsPressed()) return;

    // Zoom
    if (e.IsAction("zoom_in")) BoardScale += 5;
    else if (e.IsAction("zoom_out")) BoardScale -= 5;
  }

  // --------------------
  //       Scaling
  // --------------------

  private int _scale = 60;

  [Export(PropertyHint.None, "suffix:px")]
  public int BoardScale
  {
    get => _scale;
    set
    {
      _scale = Math.Clamp(value, 30, 120);
      Zoomed?.Invoke();
    }
  }

  // --------------------
  //       Dragging
  // --------------------

  private Vector2I _offset = Vector2I.Zero;

  [Export(PropertyHint.None, "suffix:units")]
  public Vector2I BoardOffset
  {
    get => _offset;
    set
    {
      _offset = value;
      Dragged?.Invoke();
    }
  }

  public Vector2 DragOrigin = Vector2.Zero;
  public Vector2I DragOffset = Vector2I.Zero;

  private bool _isDragging = false;
  public bool IsDragging
  {
    get => _isDragging;
    set
    {
      _isDragging = value;

      if (IsDragging)
      {
        SetDefaultCursorShape(CursorShape.Drag);
        DragOrigin = Cursor.Position;
        DragOffset = BoardOffset;
      }
      else
      {
        SetDefaultCursorShape(CursorShape.Cross);
      }
    }
  }

  // --------------------
  //     Positioning
  // --------------------

  public Vector2 CenterPosition => Size / 2;
  public Vector2 OriginPosition => CenterPosition + BoardOffset * BoardScale;

  public Vector2 SnapPosition(Vector2 localPosition)
  {
    return BoardToLocalPosition(LocalToBoardPosition(localPosition));
  }

  public Vector2I LocalToBoardPosition(Vector2 localPosition)
  {
    var originRelativePosition = localPosition - OriginPosition;
    var boardPosition = (originRelativePosition / BoardScale).Round();

    return (Vector2I)boardPosition;
  }

  public Vector2 BoardToLocalPosition(Vector2I boardPosition)
  {
    return OriginPosition + boardPosition * BoardScale;
  }
}