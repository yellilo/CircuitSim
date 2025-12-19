using CircuitSim.Circuitry;
using Godot;

namespace CircuitSim.CircuitEditor;

[Tool]
[GlobalClass]
public partial class CircuitSymbol : Node2D
{
	private Texture2D Texture => GD.Load<Texture2D>("res://Assets/CircuitSymbols/" + ComponentType + ".svg");

	private CircuitComponentType _componentType;

	[Export]
	public CircuitComponentType ComponentType
	{
		get => _componentType;
		set
		{
			_componentType = value;
			QueueRedraw();
		}
	}

	public Direction Orientation = Direction.Right;
	public bool IsVertical => Orientation == Direction.Dowm || Orientation == Direction.Up;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Board != null) Board.Zoomed += QueueRedraw;
	}

	public override void _Process(double delta)
	{
		QueueRedraw();

		if (ComponentType != CircuitComponentType.Ammeter && ComponentType != CircuitComponentType.Voltmeter)
		{
			RotationDegrees = (int)Orientation;
		}
	}

	public override void _Draw()
	{
		if (Board == null) return;

		var size = Texture.GetSize() / 16 * Board.WireScale;

		if (IsVertical) DrawLine(new(0, 0), new(0, Board.Metadata.Scale * 2), Colors.Black, Board.WireScale);
		else DrawLine(new(-Board.Metadata.Scale, 0), new(Board.Metadata.Scale, 0), Colors.Black, Board.WireScale);

		DrawTextureRect(Texture, new(-size.X / 2, -size.Y / 2, size.X, size.Y), false);
	}

	public CircuitBoard? Board
	{
		get
		{
			Node current = GetParent();

			while (current != null)
			{
				if (current is CircuitBoard typedNode) return typedNode;
				current = current.GetParent();
			}

			return null;
		}
	}
}
