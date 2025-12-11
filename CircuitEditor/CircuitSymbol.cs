using CircuitSim.Circuitry;
using Godot;

namespace CircuitSim.CircuitEditor;

[Tool]
[GlobalClass]
public partial class CircuitSymbol : Sprite2D
{
	private CircuitComponentType _componentType;

	[Export]
	public CircuitComponentType ComponentType
	{
		get => _componentType;
		set
		{
			_componentType = value;
			Texture = GD.Load<Texture2D>("res://Assets/CircuitSymbols/" + value + ".svg");
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{



	}

	public override void _Process(double delta)
	{
		Scale = Vector2.One * ((float)(Board?.BoardScale ?? 144) / 144);
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
