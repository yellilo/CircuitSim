using CircuitSim.Circuitry;
using Godot;

namespace CircuitSim.Editor;

[Tool]
[GlobalClass]
public partial class CircuitSymbol : Node2D
{
	private Texture2D Texture => GD.Load<Texture2D>("res://Assets/CircuitSymbols/" + ComponentType + ".svg");

	private ComponentType _componentType;

	[Export]
	public ComponentType ComponentType
	{
		get => ComponentModel?.Type ?? _componentType;
		set
		{
			_componentType = value;
			QueueRedraw();
		}
	}

	public RigidComponentModel? ComponentModel = null;

	public ComponentOrientation Orientation => ComponentModel?.Orientation ?? ComponentOrientation.Right;
	public bool IsVertical => Orientation == ComponentOrientation.Dowm || Orientation == ComponentOrientation.Up;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Board != null) Board.Zoomed += QueueRedraw;
	}

	public override void _Process(double delta)
	{
		if (ComponentType != ComponentType.Ammeter && ComponentType != ComponentType.Voltmeter)
		{
			RotationDegrees = (int)Orientation;
		}

		QueueRedraw();
	}

	public override void _Draw()
	{
		DrawWire();
		DrawTexture();
	}

	private void DrawWire()
	{
		if (Board == null) return;

		float thickness = 1f / 9;

		if (IsVertical) DrawLine(new(0, -1), new(0, 1), Colors.Black, thickness);
		else DrawLine(new(-1, 0), new(1, 0), Colors.Black, thickness);
	}

	private void DrawTexture()
	{
		var size = Texture.GetSize() / 154;
		var origin = -size / 2;

		DrawTextureRect(Texture, new(origin.X, origin.Y, size.X, size.Y), false);
	}

	public CircuitBoard? Board
	{
		get
		{
			Node current = GetParent();

			while (current != null)
			{
				if (current is CircuitBoard board) return board;
				current = current.GetParent();
			}

			return null;
		}
	}
}
