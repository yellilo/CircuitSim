using Godot;

namespace CircuitSim.Circuitry;

public class WireModel(string id) : ComponentModel(id, ComponentType.Wire)
{
	public Godot.Collections.Array<Vector2I> Vertices = [];
}
