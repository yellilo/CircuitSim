using Godot;

namespace CircuitSim.Circuitry;

public class JunctionModel(string id) : Model(id)
{
	public required Vector2I Position = Vector2I.Zero;
}