using Godot;

namespace CircuitSim.CircuitEditor;

[Tool]
[GlobalClass]
public partial class CircuitBoardCursor : Node2D
{
  [Export]
  public required CircuitBoard Board;
  public Vector2I BoardPosition
  {
    get => Board.LocalToBoardPosition(Position);
    set => Position = Board.BoardToLocalPosition(value);
  }

  private Label Tooltip = new()
  {
    LabelSettings = new() { FontColor = Colors.Black },
    OffsetLeft = 5
  };

  public override void _Ready()
  {
    ZIndex = 1000;
    AddChild(Tooltip);
    Tooltip.SetAnchorsPreset(Control.LayoutPreset.Center);

    AddChild(new CircuitSymbol()
    {
      ComponentType = Circuitry.CircuitComponentType.Lamp,
      Modulate = new Color(1, 1, 1, 0.3f)
    });

    Board.MouseEntered += () => Visible = true;
    Board.MouseExited += () => Visible = false;
  }

  public override void _Draw()
  {
    DrawCircle(Vector2.Zero, 2, Colors.Black, true, -1, true);
    DrawMultiline([new(-12, 0), new(12, 0), new(0, -12), new(0, 12)], Colors.Black, 2, false);
  }

  public override void _Process(double delta)
  {
    Board = GetParent<CircuitBoard>();

    BoardPosition = Board.LocalToBoardPosition(Board.GetLocalMousePosition());
    Tooltip.Text = BoardPosition.ToString();
  }
}