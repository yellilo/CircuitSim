using CircuitSim.Circuitry;
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

	private CircuitComponentType? _componentType = null;
	public CircuitComponentType? ComponentType
	{
		get => _componentType;
		set
		{
			_componentType = value;

			var circuitSymbol = GetNodeOrNull<CircuitSymbol>("CircuitSymbol");

			if (value is CircuitComponentType type)
			{
				if (circuitSymbol == null)
				{
					circuitSymbol = new CircuitSymbol() { Name = "CircuitSymbol" };
					AddChild(circuitSymbol);
				}

				circuitSymbol.ComponentType = type;
			}
			else
			{
				circuitSymbol.QueueFree();
			}
		}
	}

	private Label Tooltip = new()
	{
		LabelSettings = new() { FontColor = Colors.Black },
		OffsetLeft = 5
	};

	public override void _Ready()
	{
		ComponentType = CircuitComponentType.Lamp;
		ZIndex = 1000;
		AddChild(Tooltip);
		Tooltip.SetAnchorsPreset(Control.LayoutPreset.Center);

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
		BoardPosition = Board.LocalToBoardPosition(Board.GetLocalMousePosition());
		Tooltip.Text = BoardPosition.ToString();
	}
}