using CircuitSim.Circuitry;
using Godot;

namespace CircuitSim.Editor;

[Tool]
public partial class CircuitBoardCursor : Node2D
{
	public required CircuitBoard Board;
	public Vector2I BoardPosition
	{
		get => Board.LocalToBoardPosition(Position);
		set => Position = Board.BoardToLocalPosition(value);
	}

	private ComponentType? _componentType = null;
	public ComponentType? ComponentType
	{
		get => _componentType;
		set
		{
			_componentType = value;

			var circuitSymbol = GetNodeOrNull<CircuitSymbol>("CircuitSymbol");

			if (value is ComponentType type)
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
		ComponentType = Circuitry.ComponentType.Lamp;
		ZIndex = 1000;
		AddChild(Tooltip);
		Tooltip.SetAnchorsPreset(Control.LayoutPreset.Center);

		Board.MouseEntered += () => Visible = true;
		Board.MouseExited += () => Visible = false;
		Board.Pressed += () =>
		{
			if (ComponentType is ComponentType type)
			{
				var model = ComponentModel.FromType<RigidComponentModel>(type);
				model.Position = BoardPosition;
				Board.AddComponentFromModel(model);
				Board.HasUnsavedChanges = true;
			}

		};
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